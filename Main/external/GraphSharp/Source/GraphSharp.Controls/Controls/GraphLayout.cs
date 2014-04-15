using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows;
using System.Linq;
using GraphSharp.Algorithms.EdgeRouting;
using GraphSharp.Algorithms.Highlight;
using GraphSharp.Algorithms.Layout;
using GraphSharp.Algorithms.OverlapRemoval;
using QuickGraph;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using GraphSharp.Algorithms.Layout.Compound;

namespace GraphSharp.Controls
{
    /// <summary>
    /// For general purposes, with general types.
    /// </summary>
    public class GraphLayout : GraphLayout<object, IEdge<object>, IBidirectionalGraph<object, IEdge<object>>>
    {
        public GraphLayout()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                var g = new BidirectionalGraph<object, IEdge<object>>();
                var vertices = new object[] { "S", "A", "M", "P", "L", "E" };
                var edges = new IEdge<object>[] {
                    new Edge<object>(vertices[0], vertices[1]),
                    new Edge<object>(vertices[1], vertices[2]),
                    new Edge<object>(vertices[1], vertices[3]),
                    new Edge<object>(vertices[3], vertices[4]),
                    new Edge<object>(vertices[0], vertices[4]),
                    new Edge<object>(vertices[4], vertices[5])
                };
                g.AddVerticesAndEdgeRange(edges);
                OverlapRemovalAlgorithmType = "FSA";
                LayoutAlgorithmType = "FR";
                Graph = g;
            }
        }
    }

    /// <summary>
    /// THE layout control. Support layout, edge routing and overlap removal algorithms, with multiple layout states.
    /// </summary>
    /// <typeparam name="TVertex">Type of the vertices.</typeparam>
    /// <typeparam name="TEdge">Type of the edges.</typeparam>
    /// <typeparam name="TGraph">Type of the graph.</typeparam>
    public partial class GraphLayout<TVertex, TEdge, TGraph> : GraphCanvas
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {
        protected readonly Dictionary<TEdge, EdgeControl> _edgeControls = new Dictionary<TEdge, EdgeControl>();
        private readonly Queue<TEdge> _edgesAdded = new Queue<TEdge>();
        private readonly Queue<TEdge> _edgesRemoved = new Queue<TEdge>();
        private readonly List<LayoutState<TVertex, TEdge>> _layoutStates = new List<LayoutState<TVertex, TEdge>>();
        private readonly TimeSpan _notificationLayoutDelay = new TimeSpan(0, 0, 0, 0, 5); // 5 ms
        private readonly object _notificationSyncRoot = new object();
        protected readonly Dictionary<TVertex, VertexControl> _vertexControls = new Dictionary<TVertex, VertexControl>();
        private readonly Queue<TVertex> _verticesAdded = new Queue<TVertex>();
        private readonly Queue<TVertex> _verticesRemoved = new Queue<TVertex>();
        private readonly Stopwatch stopWatch = new Stopwatch();
        private DateTime _lastNotificationTimestamp = DateTime.Now;

        protected IDictionary<TVertex, SizeF> _sizes;
        protected BackgroundWorker _worker;

        protected IDictionary<TVertex, SizeF> VertexSizes
        {
            get
            {
                if (_sizes == null)
                {
                    InvalidateMeasure();
                    UpdateLayout();
                    _sizes = new Dictionary<TVertex, SizeF>();
                    foreach (var kvp in _vertexControls)
                    {
                        var size = kvp.Value.DesiredSize;
                        _sizes.Add(kvp.Key, new SizeF(
                                                 (double.IsNaN(size.Width) ? 0 : (float)size.Width),
                                                 (double.IsNaN(size.Height) ? 0 : (float)size.Height)));
                    }
                }
                return _sizes;
            }
        }


        #region Layout

        protected Algorithms.Layout.LayoutMode ActualLayoutMode
        {
            get
            {
                if (LayoutMode == LayoutMode.Compound ||
                     LayoutMode == LayoutMode.Automatic && Graph != null && Graph is ICompoundGraph<TVertex, TEdge>)
                    return Algorithms.Layout.LayoutMode.Compound;

                return Algorithms.Layout.LayoutMode.Simple;
            }
        }

        private bool initVerticesPosition = true;
        /// <summary>
        /// Gets or sets a value indicating whether vertices positions should be init to its neighours 
        /// when vertices are added simultanously and are connected to each other. 
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [init vertices position]; otherwise, <c>false</c>.
        /// </value>
        protected bool InitVerticesPosition
        {
            get { return initVerticesPosition; }
            set { initVerticesPosition = value; }
        }

        protected bool IsCompoundMode
        {
            get { return ActualLayoutMode == Algorithms.Layout.LayoutMode.Compound; }
        }

        protected virtual bool CanLayout
        {
            get { return true; }
        }

        public override void ContinueLayout()
        {
            Layout(true);
        }

        public override void Relayout()
        {
            //clear the states before
            _layoutStates.Clear();
            Layout(false);
        }

        public void CancelLayout()
        {
            if (_worker != null && _worker.IsBusy && _worker.WorkerSupportsCancellation)
                _worker.CancelAsync();
        }

        public void RecalculateEdgeRouting()
        {
            foreach (var state in _layoutStates)
                state.RouteInfos = RouteEdges(state.OverlapRemovedPositions, GetLatestVertexSizes());
            ChangeState(StateIndex);
        }

        public void RecalculateOverlapRemoval()
        {
            foreach (var state in _layoutStates)
                state.OverlapRemovedPositions = OverlapRemoval(state.Positions, GetLatestVertexSizes());
            ChangeState(StateIndex);
        }

        protected virtual ILayoutContext<TVertex, TEdge, TGraph> CreateLayoutContext(
            IDictionary<TVertex, Point> positions, IDictionary<TVertex, Size> sizes)
        {
            if (!CanLayout)
                return null;

            if (ActualLayoutMode == Algorithms.Layout.LayoutMode.Simple)
                return new LayoutContext<TVertex, TEdge, TGraph>(Graph, positions, sizes, ActualLayoutMode);
            else
            {
                var borders = (from kvp in _vertexControls
                               where kvp.Value is CompoundVertexControl
                               select kvp).ToDictionary(
                    kvp => kvp.Key,
                    kvp => ((CompoundVertexControl)kvp.Value).VertexBorderThickness);

                var layoutTypes = (from kvp in _vertexControls
                                   where kvp.Value is CompoundVertexControl
                                   select kvp).ToDictionary(
                    kvp => kvp.Key,
                    kvp => ((CompoundVertexControl)kvp.Value).LayoutMode);

                return new CompoundLayoutContext<TVertex, TEdge, TGraph>(Graph, positions, sizes, ActualLayoutMode, borders, layoutTypes);
            }
        }

        protected virtual IHighlightContext<TVertex, TEdge, TGraph> CreateHighlightContext()
        {
            return new HighlightContext<TVertex, TEdge, TGraph>(Graph);
        }

        protected virtual IOverlapRemovalContext<TVertex> CreateOverlapRemovalContext(
            IDictionary<TVertex, Point> positions, IDictionary<TVertex, Size> sizes)
        {
            var rectangles = new Dictionary<TVertex, Rect>();
            foreach (var vertex in Graph.Vertices)
            {
                Point position;
                Size size;
                if (!positions.TryGetValue(vertex, out position) || !sizes.TryGetValue(vertex, out size))
                    continue;

                rectangles[vertex] =
                    new Rect(
                        position.X - size.Width * (float)0.5,
                        position.Y - size.Height * (float)0.5,
                        size.Width,
                        size.Height);
            }

            return new OverlapRemovalContext<TVertex>(rectangles);
        }

        protected virtual void Layout(bool continueLayout)
        {
            if (Graph == null || !LayoutAlgorithmFactory.IsValidAlgorithm(LayoutAlgorithmType) || !CanLayout)
                return; //no graph to layout, or wrong layout algorithm

            UpdateLayout();
            if (!this.IsLoaded)
            {
                RoutedEventHandler handler = null;
                handler = new RoutedEventHandler((s, e) =>
                {
                    Layout(continueLayout);
                    var gl = (GraphLayout<TVertex, TEdge, TGraph>)e.Source;
                    gl.Loaded -= handler;
                });
                this.Loaded += handler;
                return;
            }

            //get the actual positions if we want to continue the layout
            IDictionary<TVertex, Point> oldPositions = GetOldVertexPositions(continueLayout);
            IDictionary<TVertex, Size> oldSizes = GetLatestVertexSizes();

            //create the context
            var layoutContext = CreateLayoutContext(oldPositions, oldSizes);

            //create the layout algorithm using the factory
            LayoutAlgorithm = LayoutAlgorithmFactory.CreateAlgorithm(LayoutAlgorithmType, layoutContext,
                                                                      LayoutParameters);

            if (AsyncCompute)
            {
                //Asynchronous computing - progress report & anything else
                //if there's a running progress than cancel that
                CancelLayout();

                _worker = new BackgroundWorker
                              {
                                  WorkerSupportsCancellation = true,
                                  WorkerReportsProgress = true
                              };

                //run the algorithm on a background thread
                _worker.DoWork += ((sender, e) =>
                                       {
                                           var worker = (BackgroundWorker)sender;
                                           var argument = (AsyncThreadArgument)e.Argument;
                                           if (argument.showAllStates)
                                               argument.algorithm.IterationEnded +=
                                                   ((s, args) =>
                                                        {
                                                            var iterArgs = args;
                                                            if (iterArgs != null)
                                                            {
                                                                worker.ReportProgress(
                                                                    (int)Math.Round(iterArgs.StatusInPercent), iterArgs);
                                                                iterArgs.Abort = worker.CancellationPending;
                                                            }
                                                        });
                                           else
                                               argument.algorithm.ProgressChanged +=
                                                   ((s, percent) => worker.ReportProgress((int)Math.Round(percent)));
                                           argument.algorithm.Compute();
                                       });

                //progress changed if an iteration ended
                _worker.ProgressChanged +=
                    ((s, e) =>
                         {
                             if (e.UserState == null)
                                 LayoutStatusPercent = e.ProgressPercentage;
                             else
                                 OnLayoutIterationFinished(e.UserState as ILayoutIterationEventArgs<TVertex>);
                         });

                //background thread finished if the iteration ended
                _worker.RunWorkerCompleted += ((s, e) =>
                                                    {
                                                        OnLayoutFinished();
                                                        _worker = null;
                                                    });

                OnLayoutStarted();
                _worker.RunWorkerAsync(new AsyncThreadArgument
                                           {
                                               algorithm = LayoutAlgorithm,
                                               showAllStates = ShowAllStates
                                           });
            }
            else
            {
                //Syncronous computing - no progress report
                LayoutAlgorithm.Started += ((s, e) => OnLayoutStarted());
                if (ShowAllStates)
                    LayoutAlgorithm.IterationEnded += ((s, e) => OnLayoutIterationFinished(e));
                LayoutAlgorithm.Finished += ((s, e) => OnLayoutFinished());

                LayoutAlgorithm.Compute();
            }
        }

        private IDictionary<TVertex, Point> GetOldVertexPositions(bool continueLayout)
        {
            if (ActualLayoutMode == GraphSharp.Algorithms.Layout.LayoutMode.Simple)
            {
                return continueLayout ? GetLatestVertexPositions() : null;
            }
            else
            {
                return GetRelativePositions(continueLayout);
            }
        }

        private IDictionary<TVertex, Point> GetLatestVertexPositions()
        {
            IDictionary<TVertex, Point> vertexPositions = new Dictionary<TVertex, Point>(_vertexControls.Count);

            //go through the vertex presenters and get the actual layoutpositions
            if (ActualLayoutMode == Algorithms.Layout.LayoutMode.Simple)
            {
                foreach (var vc in _vertexControls)
                {
                    var x = GetX(vc.Value);
                    var y = GetY(vc.Value);
                    vertexPositions[vc.Key] =
                        new Point(
                            double.IsNaN(x) ? 0.0 : x,
                            double.IsNaN(y) ? 0.0 : y);
                }
            }
            else
            {
                Point topLeft = new Point(0, 0);
                foreach (var vc in _vertexControls)
                {
                    Point position = vc.Value.TranslatePoint(topLeft, this);
                    position.X += vc.Value.ActualWidth / 2;
                    position.Y += vc.Value.ActualHeight / 2;
                    vertexPositions[vc.Key] = position;
                }
            }

            return vertexPositions;
        }

        private Point GetRelativePosition(VertexControl vc, UIElement relativeTo)
        {
            return vc.TranslatePoint(new Point(vc.ActualWidth / 2.0, vc.ActualHeight / 2.0), relativeTo);
        }

        private Point GetRelativePosition(VertexControl vc)
        {
            return GetRelativePosition(vc, this);
        }

        private IDictionary<TVertex, Point> GetRelativePositions(bool continueLayout)
        {
            //if layout is continued it gets the relative position of every vertex
            //otherwise it's gets it only for the vertices with fixed parents
            var posDict = new Dictionary<TVertex, Point>(_vertexControls.Count);
            if (continueLayout)
            {
                foreach (var kvp in _vertexControls)
                {
                    posDict[kvp.Key] = GetRelativePosition(kvp.Value);
                }
            }
            else
            {
                foreach (var kvp in _vertexControls)
                {
                    if (!(kvp.Value is CompoundVertexControl))
                        continue;

                    var cvc = (CompoundVertexControl)kvp.Value;
                    foreach (var vc in cvc.Vertices)
                    {
                        posDict[(TVertex)vc.Vertex] = GetRelativePosition(vc, cvc);
                    }
                }
            }
            return posDict;
        }

        private IDictionary<TVertex, Size> GetLatestVertexSizes()
        {
            if (!IsMeasureValid)
                Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            IDictionary<TVertex, Size> vertexSizes =
                new Dictionary<TVertex, Size>(_vertexControls.Count);

            //go through the vertex presenters and get the actual layoutpositions
            foreach (var vc in _vertexControls)
                vertexSizes[vc.Key] = new Size(vc.Value.ActualWidth, vc.Value.ActualHeight);

            return vertexSizes;
        }

        protected void OnLayoutStarted()
        {
            stopWatch.Reset();
            stopWatch.Start();
            LayoutStatusPercent = 0.0;
        }

        protected virtual void OnLayoutIterationFinished(ILayoutIterationEventArgs<TVertex> iterArgs)
        {
            if (iterArgs == null || iterArgs.VertexPositions == null)
            {
                if (LayoutAlgorithm is ICompoundLayoutAlgorithm<TVertex, TEdge, TGraph>)
                {
                    var la = (ICompoundLayoutAlgorithm<TVertex, TEdge, TGraph>)LayoutAlgorithm;
                }
                else
                {

                }
                OnLayoutIterationFinished(LayoutAlgorithm.VertexPositions, null);
                SetLayoutInformations();
            }
            else
            {
                //TODO compound layout
                OnLayoutIterationFinished(iterArgs.VertexPositions, iterArgs.Message);
                LayoutStatusPercent = iterArgs.StatusInPercent;
                SetLayoutInformations(iterArgs as ILayoutInfoIterationEventArgs<TVertex, TEdge>);
            }
        }

        protected virtual void OnLayoutIterationFinished(
            IDictionary<TVertex, Point> vertexPositions,
            string message)
        {
            var sizes = GetLatestVertexSizes();
            var overlapRemovedPositions = OverlapRemoval(vertexPositions, sizes);
            var edgeRoutingInfos = RouteEdges(overlapRemovedPositions, sizes);

            var state = new LayoutState<TVertex, TEdge>(
                vertexPositions,
                overlapRemovedPositions,
                edgeRoutingInfos,
                stopWatch.Elapsed,
                _layoutStates.Count,
                (message ?? string.Empty));

            _layoutStates.Add(state);
            StateCount = _layoutStates.Count;
        }

        protected virtual void OnLayoutFinished()
        {
            stopWatch.Stop();
            OnLayoutIterationFinished(null);
            StateIndex = StateCount - 1;

            //animating to the finish state
            if (StateIndex == 0)
                ChangeState(StateIndex);

            LayoutStatusPercent = 100;
        }

        private void SetLayoutInformations(ILayoutInfoIterationEventArgs<TVertex, TEdge> iterArgs)
        {
            if (iterArgs == null)
                return;

            foreach (var kvp in _vertexControls)
            {
                var vertex = kvp.Key;
                var control = kvp.Value;

                GraphElementBehaviour.SetLayoutInfo(control, iterArgs.GetVertexInfo(vertex));
            }

            foreach (var kvp in _edgeControls)
            {
                var edge = kvp.Key;
                var control = kvp.Value;

                GraphElementBehaviour.SetLayoutInfo(control, iterArgs.GetEdgeInfo(edge));
            }
        }

        private void SetLayoutInformations()
        {
            if (LayoutAlgorithm == null)
                return;

            foreach (var kvp in _vertexControls)
            {
                var vertex = kvp.Key;
                var control = kvp.Value;

                GraphElementBehaviour.SetLayoutInfo(control, LayoutAlgorithm.GetVertexInfo(vertex));
            }

            foreach (var kvp in _edgeControls)
            {
                var edge = kvp.Key;
                var control = kvp.Value;

                GraphElementBehaviour.SetLayoutInfo(control, LayoutAlgorithm.GetEdgeInfo(edge));
            }
        }

        protected IDictionary<TVertex, Point> OverlapRemoval(IDictionary<TVertex, Point> positions,
                                                              IDictionary<TVertex, Size> sizes)
        {
            if (positions == null || sizes == null)
                return positions; //not valid positions or sizes

            bool isValidAlgorithm = OverlapRemovalAlgorithmFactory.IsValidAlgorithm(OverlapRemovalAlgorithmType);
            if (OverlapRemovalConstraint == AlgorithmConstraints.Skip
                 ||
                 (OverlapRemovalConstraint == AlgorithmConstraints.Automatic &&
                   (!LayoutAlgorithmFactory.NeedOverlapRemoval(LayoutAlgorithmType) || !isValidAlgorithm))
                 || (OverlapRemovalConstraint == AlgorithmConstraints.Must && !isValidAlgorithm))
                return positions;

            //create the algorithm parameters based on the old parameters
            OverlapRemovalParameters = OverlapRemovalAlgorithmFactory.CreateParameters(OverlapRemovalAlgorithmType,
                                                                                        OverlapRemovalParameters);

            //create the context - rectangles, ...
            var context = CreateOverlapRemovalContext(positions, sizes);

            //create the concreate algorithm
            OverlapRemovalAlgorithm = OverlapRemovalAlgorithmFactory.CreateAlgorithm(OverlapRemovalAlgorithmType,
                                                                                      context, OverlapRemovalParameters);
            if (OverlapRemovalAlgorithm != null)
            {
                OverlapRemovalAlgorithm.Compute();

                var result = new Dictionary<TVertex, Point>();
                foreach (var res in OverlapRemovalAlgorithm.Rectangles)
                {
                    result[res.Key] = new Point(
                        (res.Value.Left + res.Value.Size.Width * 0.5),
                        (res.Value.Top + res.Value.Size.Height * 0.5));
                }

                return result;
            }

            return positions;
        }

        /// <summary>
        /// This method runs the proper edge routing algorithm.
        /// </summary>
        /// <param name="positions">The vertex positions.</param>
        /// <param name="sizes">The sizes of the vertices.</param>
        /// <returns>The routes of the edges.</returns>
        protected IDictionary<TEdge, Point[]> RouteEdges(IDictionary<TVertex, Point> positions,
                                                          IDictionary<TVertex, Size> sizes)
        {
            IEdgeRoutingAlgorithm<TVertex, TEdge, TGraph> algorithm = null;
            bool isValidAlgorithmType = EdgeRoutingAlgorithmFactory.IsValidAlgorithm(EdgeRoutingAlgorithmType);

            if (EdgeRoutingConstraint == AlgorithmConstraints.Must && isValidAlgorithmType)
            {
                //an EdgeRouting algorithm must be used
                EdgeRoutingParameters = EdgeRoutingAlgorithmFactory.CreateParameters(EdgeRoutingAlgorithmType,
                                                                                      EdgeRoutingParameters);

                var context = CreateLayoutContext(positions, sizes);

                algorithm = EdgeRoutingAlgorithmFactory.CreateAlgorithm(EdgeRoutingAlgorithmType, context,
                                                                         EdgeRoutingParameters);
                algorithm.Compute();
            }
            else if (EdgeRoutingConstraint == AlgorithmConstraints.Automatic)
            {
                if (!LayoutAlgorithmFactory.NeedEdgeRouting(LayoutAlgorithmType) &&
                     LayoutAlgorithm is IEdgeRoutingAlgorithm<TVertex, TEdge, TGraph>)
                    //the layout algorithm routes the edges
                    algorithm = LayoutAlgorithm as IEdgeRoutingAlgorithm<TVertex, TEdge, TGraph>;
                else if (isValidAlgorithmType)
                {
                    //the selected EdgeRouting algorithm will route the edges
                    EdgeRoutingParameters = EdgeRoutingAlgorithmFactory.CreateParameters(EdgeRoutingAlgorithmType,
                                                                                          EdgeRoutingParameters);
                    var context = CreateLayoutContext(positions, sizes);
                    algorithm = EdgeRoutingAlgorithmFactory.CreateAlgorithm(EdgeRoutingAlgorithmType, context,
                                                                             EdgeRoutingParameters);
                    algorithm.Compute();
                }
            }

            if (algorithm == null)
                return null;

            //route the edges
            return algorithm.EdgeRoutes;
        }

        /// <summary>
        /// Changes which layout state should be shown.
        /// </summary>
        /// <param name="stateIndex">The index of the shown layout state.</param>
        protected void ChangeState(int stateIndex)
        {
            Contract.Requires(0 <= stateIndex);
            Contract.Requires(stateIndex < _layoutStates.Count);

            var activeState = _layoutStates[stateIndex];

            LayoutState = activeState;

            var positions = activeState.OverlapRemovedPositions;

            //Animate the vertices
            if (positions != null)
            {
                Point pos;
                foreach (var v in Graph.Vertices)
                {
                    VertexControl vp;
                    if (!_vertexControls.TryGetValue(v, out vp))
                        continue;

                    if (positions.TryGetValue(v, out pos))
                        RunMoveAnimation(vp, pos.X, pos.Y);
                }
            }

            //Change the edge routes
            if (activeState.RouteInfos != null)
            {
                foreach (var e in Graph.Edges)
                {
                    EdgeControl ec;
                    if (!_edgeControls.TryGetValue(e, out ec))
                        continue;

                    Point[] routePoints;
                    activeState.RouteInfos.TryGetValue(e, out routePoints);
                    ec.RoutePoints = routePoints;
                }
            }
        }

        public void RefreshHighlight()
        {
            //TODO doit
        }

        private class AsyncThreadArgument
        {
            public ILayoutAlgorithm<TVertex, TEdge, TGraph> algorithm;
            public bool showAllStates;
        }

        #endregion
    }
}
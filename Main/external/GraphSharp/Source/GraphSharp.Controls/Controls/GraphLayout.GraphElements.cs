﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using QuickGraph;

namespace GraphSharp.Controls
{
    public partial class GraphLayout<TVertex, TEdge, TGraph> : GraphCanvas
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {

        protected void RemoveAllGraphElement()
        {
            foreach (var vertex in _vertexControls.Keys.ToArray())
                RemoveVertexControl(vertex);
            foreach (var edge in _edgeControls.Keys.ToArray())
                RemoveEdgeControl(edge);
            _vertexControls.Clear();
            _edgeControls.Clear();
        }

        /// <summary>
        /// If the graph has been changed, the elements will be regenerated.
        /// </summary>
        protected void RecreateGraphElements(bool tryKeepControls, TGraph oldGraph = null)
        {
            if (Graph == null)
            {
                RemoveAllGraphElement();
            }
            else
            {
                if (tryKeepControls && !IsCompoundMode)
                {
                    //remove the old graph elements
                    foreach (var kvp in _edgeControls.ToList())
                    {
                        bool remove = false;
                        try
                        {
                            remove = !Graph.ContainsEdge(kvp.Key.Source, kvp.Key.Target) || !Graph.ContainsEdge(kvp.Key);
                        }
                        catch
                        {
                        }

                        if (remove)
                        {
                            RemoveEdgeControl(kvp.Key);
                        }
                    }
                    foreach (var kvp in _vertexControls.ToList())
                    {
                        if (!Graph.ContainsVertex(kvp.Key))
                        {
                            RemoveVertexControl(kvp.Key);
                        }
                    }
                }
                else
                {
                    RemoveAllGraphElement();
                }

                //
                // Presenters for vertices
                //
                foreach (var vertex in Graph.Vertices)
                    if (!_vertexControls.ContainsKey(vertex))
                        CreateVertexControl(vertex);

                //
                // Presenters for edges
                //
                foreach (var edge in Graph.Edges)
                    if (!_edgeControls.ContainsKey(edge))
                        CreateEdgeControl(edge);
            }

            _sizes = null;
        }

        /// <summary>
        /// subscribe to events of the Graph mutations
        /// </summary>
        private void SubscribeToEvents(TGraph graph)
        {
            if (!IsCompoundMode)
            {
                var mutableGraph = graph as IMutableBidirectionalGraph<TVertex, TEdge>;
                if (mutableGraph != null)
                {
                    mutableGraph.VertexAdded += OnMutableGraph_VertexAdded;
                    mutableGraph.VertexRemoved += OnMutableGraph_VertexRemoved;
                    mutableGraph.EdgeAdded += OnMutableGraph_EdgeAdded;
                    mutableGraph.EdgeRemoved += OnMutableGraph_EdgeRemoved;
                }
            }
        }

        private void UnsubscribeEvents(TGraph graph)
        {
            if (!IsCompoundMode)
            {
                var mutableGraph = graph as IMutableBidirectionalGraph<TVertex, TEdge>;
                if (mutableGraph != null)
                {
                    mutableGraph.VertexAdded -= OnMutableGraph_VertexAdded;
                    mutableGraph.VertexRemoved -= OnMutableGraph_VertexRemoved;
                    mutableGraph.EdgeAdded -= OnMutableGraph_EdgeAdded;
                    mutableGraph.EdgeRemoved -= OnMutableGraph_EdgeRemoved;
                }
            }
        }


        private void DoNotificationLayout()
        {
            lock (_notificationSyncRoot)
            {
                _lastNotificationTimestamp = DateTime.Now;
            }
            if (_worker != null)
                return;

            _worker = new BackgroundWorker();
            _worker.DoWork += (s, e) =>
            {
                var w = (BackgroundWorker)s;
                lock (_notificationSyncRoot)
                {
                    while ((DateTime.Now - _lastNotificationTimestamp) < _notificationLayoutDelay)
                    {
                        Thread.Sleep(_notificationLayoutDelay);
                        if (w.CancellationPending)
                            break;
                    }
                }
            };
            _worker.RunWorkerCompleted += (s, e) =>
            {
                _worker = null;
                OnMutation();
                ContinueLayout();
                if (HighlightAlgorithm != null)
                    HighlightAlgorithm.ResetHighlight();
            };
            _worker.RunWorkerAsync();
        }

        private void OnMutation()
        {
            while (_edgesRemoved.Count > 0)
            {
                var edge = _edgesRemoved.Dequeue();
                RemoveEdgeControl(edge);
            }
            while (_verticesRemoved.Count > 0)
            {
                var vertex = _verticesRemoved.Dequeue();
                RemoveVertexControl(vertex);
            }
            var verticesToInitPos = _verticesAdded.ToList();
            while (_verticesAdded.Count > 0)
            {
                var vertex = _verticesAdded.Dequeue();
                CreateVertexControl(vertex);
            }
            while (_edgesAdded.Count > 0)
            {
                var edge = _edgesAdded.Dequeue();
                CreateEdgeControl(edge);
            }
            if (InitVerticesPosition)
            {
                foreach (var vertex in verticesToInitPos)
                {
                    InitializePosition(vertex);
                }
            }
        }

        private void OnMutableGraph_EdgeRemoved(TEdge edge)
        {
            if (_edgeControls.ContainsKey(edge))
            {
                _edgesRemoved.Enqueue(edge);
                DoNotificationLayout();
            }
        }

        private void OnMutableGraph_EdgeAdded(TEdge edge)
        {
            _edgesAdded.Enqueue(edge);
            DoNotificationLayout();
        }

        private void OnMutableGraph_VertexRemoved(TVertex vertex)
        {
            if (_vertexControls.ContainsKey(vertex))
            {
                _verticesRemoved.Enqueue(vertex);
                DoNotificationLayout();
            }
        }

        private void OnMutableGraph_VertexAdded(TVertex vertex)
        {
            _verticesAdded.Enqueue(vertex);
            DoNotificationLayout();
        }

        protected VertexControl GetOrCreateVertexControl(TVertex vertex)
        {
            if (!_vertexControls.ContainsKey(vertex))
                CreateVertexControl(vertex);

            return _vertexControls[vertex];
        }

        protected virtual void CreateVertexControl(TVertex vertex)
        {
            VertexControl presenter;
            var compoundGraph = Graph as ICompoundGraph<TVertex, TEdge>;

            if (IsCompoundMode && compoundGraph != null && compoundGraph.IsCompoundVertex(vertex))
            {
                var compoundPresenter = new CompoundVertexControl
                {
                    Vertex = vertex
                };
                compoundPresenter.Expanded += CompoundVertexControl_ExpandedOrCollapsed;
                compoundPresenter.Collapsed += CompoundVertexControl_ExpandedOrCollapsed;
                presenter = compoundPresenter;
            }
            else
            {
                // Create the Control of the vertex
                presenter = new VertexControl
                {
                    Vertex = vertex
                };
            }

            //var presenter = _vertexPool.GetObject();
            //presenter.Vertex = vertex;
            _vertexControls[vertex] = presenter;
            presenter.RootCanvas = this;

            if (IsCompoundMode && compoundGraph != null && compoundGraph.IsChildVertex(vertex))
            {
                var parent = compoundGraph.GetParent(vertex);
                var parentControl = GetOrCreateVertexControl(parent) as CompoundVertexControl;

                Debug.Assert(parentControl != null);

                parentControl.Vertices.Add(presenter);
            }
            else
            {
                //add the presenter to the GraphLayout
                Children.Add(presenter);
            }

            //Measuring & Arrange
            presenter.InvalidateMeasure();
            SetHighlightProperties(vertex, presenter);
            RunCreationTransition(presenter);
        }

        protected virtual void InitializePosition(TVertex vertex)
        {
            VertexControl presenter = _vertexControls[vertex];
            //initialize position
            if (Graph.Degree(vertex) > 0)
            {
                var pos = new Point();
                int count = 0;
                foreach (var neighbour in Graph.GetNeighbours(vertex))
                {
                    VertexControl neighbourControl;
                    if (_vertexControls.TryGetValue(neighbour, out neighbourControl))
                    {
                        double x = GetX(neighbourControl);
                        double y = GetY(neighbourControl);
                        pos.X += double.IsNaN(x) ? 0.0 : x;
                        pos.Y += double.IsNaN(y) ? 0.0 : y;
                        count++;
                    }
                }
                if (count > 0)
                {
                    //pos.X /= count;
                    //pos.Y /= count;
                    //SetX(presenter, pos.X);
                    //SetY(presenter, pos.Y);
                }
            }
        }

        private void CompoundVertexControl_ExpandedOrCollapsed(object sender, RoutedEventArgs e)
        {
            //TODO relayout perhaps
        }

        protected EdgeControl GetOrCreateEdgeControl(TEdge edge)
        {
            if (!_edgeControls.ContainsKey(edge))
                CreateEdgeControl(edge);

            return _edgeControls[edge];
        }

        protected virtual void CreateEdgeControl(TEdge edge)
        {
            var edgeControl = new EdgeControl
            {
                Edge = edge
            };
            //var edgeControl = _edgePool.GetObject();
            //edgeControl.Edge = edge;
            _edgeControls[edge] = edgeControl;

            //set the Source and the Target
            edgeControl.Source = _vertexControls[edge.Source];
            edgeControl.Target = _vertexControls[edge.Target];

            if (ActualLayoutMode == GraphSharp.Algorithms.Layout.LayoutMode.Simple)
                Children.Insert(0, edgeControl);
            else
                Children.Add(edgeControl);
            SetHighlightProperties(edge, edgeControl);
            RunCreationTransition(edgeControl);
        }

        protected virtual void RemoveVertexControl(TVertex vertex)
        {
            RunDestructionTransition(_vertexControls[vertex], false);
            _vertexControls.Remove(vertex);
        }

        protected virtual void RemoveEdgeControl(TEdge edge)
        {
            RunDestructionTransition(_edgeControls[edge], false);
            _edgeControls.Remove(edge);
        }
	}
}

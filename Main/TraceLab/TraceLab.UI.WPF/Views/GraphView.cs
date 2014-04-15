// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Commands;
using TraceLab.UI.WPF.Controls.ZoomControl;
using TraceLab.UI.WPF.EventArgs;
using TraceLab.UI.WPF.Selectors;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views
{
    public abstract class GraphView : UserControl, INodeControlSelector
    {
        static GraphView()
        {
            IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(Boolean), typeof(GraphView),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsSelectedChanged));

            SelectedNodesProperty = DependencyProperty.Register("SelectedNodes", typeof(IEnumerable<ExperimentNode>), typeof(GraphView));
            SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GraphView));
            UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GraphView));
            SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GraphView));
        }

        public GraphView()
        {
            // TODO _autoAlignCommand = new DelegateCommand(DoAutoAlign, CanAutoAlign);
            DataContextChanged += new DependencyPropertyChangedEventHandler(GraphView_DataContextChanged);
            Focusable = true;
        }

        /*
         * Handles the positioning of the view for the new data context 
         * and saving out the old position for the old view
         */
        private void GraphView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TraceLab.UI.WPF.ViewModels.IZoomableViewModel newModel = e.NewValue as TraceLab.UI.WPF.ViewModels.IZoomableViewModel;
            TraceLab.UI.WPF.ViewModels.IZoomableViewModel oldModel = e.OldValue as TraceLab.UI.WPF.ViewModels.IZoomableViewModel;

            SavePositionAndZoom(oldModel);
            SetPositionAndZoom(newModel);

            UnselectAll();
        }

        private void SavePositionAndZoom(TraceLab.UI.WPF.ViewModels.IZoomableViewModel oldModel)
        {
            if (oldModel != null)
            {
                oldModel.TranslateX = ZoomControl.TranslateX;
                oldModel.TranslateY = ZoomControl.TranslateY;
                oldModel.Zoom = ZoomControl.Zoom;
            }
        }
        
        private void SetPositionAndZoom(TraceLab.UI.WPF.ViewModels.IZoomableViewModel newModel)
        {
            //if (newModel != null)
            //{
            //    double newX = newModel.TranslateX;
            //    double newY = newModel.TranslateY;
            //    double newZoom = newModel.Zoom;

            //    zoomControl.SetPositionAndZoom(newX, newY, newZoom);
            //}
        }

        #region Abstract UI Elements 

        protected abstract TraceLab.UI.WPF.Controls.NodeGraphLayout GraphLayout { get; }
        protected abstract TraceLab.UI.WPF.Controls.ZoomControl.ZoomControl ZoomControl { get; }
        protected abstract GraphSharp.Controls.VertexControl HACK_VertexControl { get; }
        protected abstract GraphSharp.Controls.EdgeControl HACK_EdgeControl { get; }
        protected abstract System.Windows.Shapes.Rectangle MarqueeAdorner { get; }

        #endregion Abstract UI Elements

        #region Selection

        #region INodeControlSelector Members

        public GraphSharp.Controls.VertexControl ControlForNode(ExperimentNode node)
        {
            return GraphLayout.GetNodeControl(node);
        }

        #endregion

        /// <summary>
        /// Tracks selected vertices by all graph views
        /// </summary>
        private static HashSet<GraphSharp.Controls.VertexControl> s_selectedVertices = new HashSet<GraphSharp.Controls.VertexControl>();

        public static readonly DependencyProperty IsSelectedProperty;
        public static readonly DependencyProperty SelectedNodesProperty;

        public static readonly RoutedEvent SelectionChangedEvent;
        public static readonly RoutedEvent SelectedEvent;
        public static readonly RoutedEvent UnselectedEvent;

        public IEnumerable<object> SelectedNodes
        {
            get { return (IEnumerable<object>)GetValue(SelectedNodesProperty); }
            set { SetValue(SelectedNodesProperty, value); }
        }

        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }


        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        public event RoutedEventHandler Unselected
        {
            add { AddHandler(UnselectedEvent, value); }
            remove { RemoveHandler(UnselectedEvent, value); }
        }

        public static void SetIsSelected(GraphSharp.Controls.VertexControl element, Boolean value)
        {
            element.SetValue(IsSelectedProperty, value);
        }

        public static Boolean GetIsSelected(GraphSharp.Controls.VertexControl element)
        {
            return (Boolean)element.GetValue(IsSelectedProperty);
        }

        public static void IsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            // TODO: 
            var vertex = (GraphSharp.Controls.VertexControl)sender;
            var graph = vertex.GetParent<GraphView>(null);

            if (graph != null)
            {
                if ((bool)args.NewValue)
                {
                    GraphView.s_selectedVertices.Add(vertex);
                }
                else
                {
                    GraphView.s_selectedVertices.Remove(vertex);
                }
            }
        }


        /// <summary>
        /// Unselects all nodes in all graphs views
        /// </summary>
        public void UnselectAll()
        {
            var selection = new List<GraphSharp.Controls.VertexControl>(s_selectedVertices);

            foreach (GraphSharp.Controls.VertexControl control in selection)
            {
                SetIsSelected(control, false);
            }
        }
        
        #endregion

        #region IsExperimentRunning Dependency Property

        public static readonly DependencyProperty IsExperimentRunningProperty = DependencyProperty.Register("IsExperimentRunning", typeof(bool), typeof(GraphView));

        /// <summary>
        /// Gets or sets a value indicating whether the experiment running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the experiment is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsExperimentRunning
        {
            get { return (bool)GetValue(IsExperimentRunningProperty); }
            set { SetValue(IsExperimentRunningProperty, value); }
        }

        #endregion

        #region DragOver and Drop Functionality

        protected void ExecuteCreateConnection(object sender, ExecutedRoutedEventArgs e)
        {
            var node = e.Parameter as GraphSharp.Controls.VertexControl;

            if (node != null)
            {
                StartCreateLinkDrag(node);
            }
        }

        #region Drop Dependency Command Property

        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register("DropCommand", typeof(ICommand), typeof(GraphView));

        /// <summary>
        /// Gets or sets the drop command.
        /// </summary>
        /// <value>
        /// The drop command.
        /// </value>
        public ICommand DropCommand
        {
            get
            {
                return (ICommand)GetValue(DropCommandProperty);
            }
            set
            {
                SetValue(DropCommandProperty, value);
            }
        }

        protected void Graph_Drop(object sender, DragEventArgs e)
        {
            //command should not be executed if element is being added to scope view, or scope view in the scope view, and so on.
            //therefore check, if this graph view is occluded by other graph view. Only execute the command if it is not occluded.
            bool isGraphViewOccluded = IsUIElementOccludedByAnotherGraphView(this, e.GetPosition(this));

            if (!isGraphViewOccluded)
            {
                DropCommand.Execute(new GraphDragEventArgs(GraphLayout, e));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Determines whether the given ui element at the given position is occluded by other graph view (for example occluded by scope view).
        /// </summary>
        /// <param name="uiElement">The UI element.</param>
        /// <param name="clickPoint">The click point.</param>
        /// <returns>
        ///   <c>true</c> if this ui element is occluded at the specified position; otherwise, <c>false</c>.
        /// </returns>
        private bool IsUIElementOccludedByAnotherGraphView(UIElement uiElement, Point clickPoint)
        {
            bool isOccluded = true;

            var hitElement = uiElement.InputHitTest(clickPoint) as DependencyObject;
            if (hitElement != null)
            {
                var graphView = hitElement.GetParent<TraceLab.UI.WPF.Views.GraphView>(this);
                if (graphView == this)
                {
                    isOccluded = false;
                }
            }

            return isOccluded;
        }

        #endregion

        #region DragOver Dependency Command Property

        public static readonly DependencyProperty DragOverCommandProperty = DependencyProperty.Register("DragOverCommand", typeof(ICommand), typeof(GraphView));

        /// <summary>
        /// Gets or sets the drag over command.
        /// </summary>
        /// <value>
        /// The drag over command.
        /// </value>
        public ICommand DragOverCommand
        {
            get
            {
                return (ICommand)GetValue(DragOverCommandProperty);
            }
            set
            {
                SetValue(DragOverCommandProperty, value);
            }
        }

        protected void Graph_DragOver(object sender, DragEventArgs e)
        {
            DragOverCommand.Execute(new GraphDragEventArgs(GraphLayout, e));
        }

        #endregion

        #region Related to Dragging DependencyProperties

        public static readonly DependencyProperty IsLinkDropTargetProperty = DependencyProperty.RegisterAttached("IsLinkDropTarget", typeof(bool), typeof(GraphView));
        public static readonly DependencyProperty IsLinkDropSourceProperty = DependencyProperty.RegisterAttached("IsLinkDropSource", typeof(bool), typeof(GraphView));
        public static readonly DependencyProperty IsLinkValidProperty = DependencyProperty.RegisterAttached("IsLinkValid", typeof(bool), typeof(GraphView));

        public static bool GetIsLinkDropTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLinkDropTargetProperty);
        }

        public static void SetIsLinkDropTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLinkDropTargetProperty, value);
        }

        public static bool GetIsLinkDropSource(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLinkDropSourceProperty);
        }

        public static void SetIsLinkDropSource(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLinkDropSourceProperty, value);
        }

        public static bool GetIsLinkValid(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLinkValidProperty);
        }

        public static void SetIsLinkValid(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLinkValidProperty, value);
        }

        #endregion

        #region Private Properties

        GraphSharp.Controls.VertexControl m_potentialLinkTarget;
        private GraphSharp.Controls.VertexControl PotentialLinkTarget
        {
            get { return m_potentialLinkTarget; }
            set
            {
                if (m_potentialLinkTarget != null)
                    SetIsLinkDropTarget(m_potentialLinkTarget, false);

                m_potentialLinkTarget = value;

                if (m_potentialLinkTarget != null)
                    SetIsLinkDropTarget(m_potentialLinkTarget, true);
            }
        }

        GraphSharp.Controls.VertexControl m_potentialLinkSource;
        private GraphSharp.Controls.VertexControl PotentialLinkSource
        {
            get { return m_potentialLinkSource; }
            set
            {
                if (m_potentialLinkSource != null)
                    SetIsLinkDropSource(m_potentialLinkSource, false);

                m_potentialLinkSource = value;

                if (m_potentialLinkSource != null)
                    SetIsLinkDropSource(m_potentialLinkSource, true);
            }
        }

        GraphSharp.Controls.EdgeControl m_mallableLink;
        private GraphSharp.Controls.EdgeControl LinkBeingMoved
        {
            get { return m_mallableLink; }
            set
            {
                if (m_mallableLink != null)
                    SetIsLinkValid(m_mallableLink, true);

                m_mallableLink = value;

                if (m_mallableLink != null)
                    SetIsLinkValid(m_mallableLink, false);
            }
        }

        #endregion

        #region Dragging

        protected void zoomControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (HACK_VertexControl.Visibility != System.Windows.Visibility.Visible)
            {
                Point clickPoint = e.GetPosition(GraphLayout);
                HACK_VertexControl.CenterX = clickPoint.X;
                HACK_VertexControl.CenterY = clickPoint.Y;
            }
        }

        protected void graphLayout_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsExperimentRunning == false)
            {
                //check if current GraphLayout at the specified coordinates is occluded by another graph view
                //Checking on GraphLayout not this GraphView, because hit elemenent must be checked relative to the current GraphLayout origin, not constant graph view element
                Point clickPoint = e.GetPosition(GraphLayout);
                bool isGraphLayoutOccluded = IsUIElementOccludedByAnotherGraphView(GraphLayout, clickPoint);
                
                if (!isGraphLayoutOccluded)
                {
                    IInputElement ele = GraphLayout.InputHitTest(clickPoint);
                    if (ele != null && HACK_VertexControl.Visibility != System.Windows.Visibility.Visible)
                    {
                        // if they clicked on a edge start redirecting the edge 
                        DependencyObject obj = (DependencyObject)ele;
                        TraceLab.UI.WPF.Controls.RoutePointThumb routePointThumb = obj.GetParent<TraceLab.UI.WPF.Controls.RoutePointThumb>(this);
                        if (routePointThumb == null)
                        {
                            GraphSharp.Controls.EdgeControl edge = obj.GetParent<GraphSharp.Controls.EdgeControl>(this);
                            if (CanRedirectLink(edge))
                            {
                                StartRedirectLinkDrag(e, clickPoint, edge);
                            }
                        }
                    }
                }
            }
        }

        void graphLayout_LostMouseCapture(object sender, MouseEventArgs e)
        {
            CancelAllDragging();
        }

        void HACK_Control_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point clickPoint = e.GetPosition(GraphLayout);
            DependencyObject obj = (DependencyObject)GraphLayout.InputHitTest(clickPoint);
            GraphSharp.Controls.VertexControl parent = null;
            if (obj != null)
            {
                parent = obj.GetParent<GraphSharp.Controls.VertexControl>(this);
            }

            if (parent != null && parent != PotentialLinkSource && DropCommand.CanExecute(new DropLinkEventArgs(PotentialLinkSource.Vertex, parent.Vertex, null)))
            {
                PotentialLinkTarget = parent;
            }
            else
            {
                PotentialLinkTarget = null;
            }
        }

        private void HACK_Control_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DropLinkEventArgs args = null;
            if (PotentialLinkSource != null)
            {
                object target = PotentialLinkTarget != null ? PotentialLinkTarget.Vertex : null;
                object edge = LinkBeingMoved != null ? LinkBeingMoved.Edge : null;

                args = new DropLinkEventArgs(PotentialLinkSource.Vertex, target, edge);
                LinkBeingMoved = null;
            }

            if (args != null && DropCommand.CanExecute(args))
            {
                /// checking if Target is different HACK_NODE is related to hard to reproduce error
                /// somehow when you drag a link quickly to the existing node, but you don't really reach it or you are too far from node, then the target appears to be HACK_NODE
                if (args.Target == null || (args.Target != null && args.Target.ToString().Equals("HACK_Node") == false))
                {
                    DropCommand.Execute(args);
                }
            }

            CancelAllDragging();
        }

        private void CancelAllDragging()
        {
            HACK_VertexControl.Visibility = Visibility.Hidden;
            HACK_EdgeControl.SetValue(GraphSharp.Controls.EdgeControl.SourceProperty, null);
            HACK_EdgeControl.SetValue(GraphSharp.Controls.EdgeControl.TargetProperty, null);
            HACK_EdgeControl.Visibility = Visibility.Collapsed;

            HACK_VertexControl.PreviewMouseLeftButtonUp -= HACK_Control_PreviewMouseLeftButtonUp;
            HACK_VertexControl.PreviewMouseMove -= HACK_Control_PreviewMouseMove;

            PotentialLinkTarget = null;
            PotentialLinkSource = null;

            if (LinkBeingMoved != null)
            {
                // Move was cancelled, re-validate the link.
                SetIsLinkValid(LinkBeingMoved, true);
                LinkBeingMoved = null;
            }
        }

        private void StartRedirectLinkDrag(MouseButtonEventArgs e, Point clickPoint, GraphSharp.Controls.EdgeControl currentEdge)
        {
            // Cache information about currentEdge
            LinkBeingMoved = currentEdge;

            GraphSharp.Controls.VertexControl source = currentEdge.Source;

            // Reveal the manipulation controls.
            RevealHACKControls(clickPoint, source);

            // handle mouse event so any other control 
            e.Handled = true;

            // When we lose mouse capture, then cancel all dragging.
            BeginDrag(e.MouseDevice, e.Timestamp, e.ChangedButton);
        }

        /// <summary>
        /// Determines whether the edge can be redirected
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can redirect link] the specified current edge; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRedirectLink(GraphSharp.Controls.EdgeControl edge)
        {
            bool canRedirect = false;

            if (edge != null)
            {
                ExperimentNodeConnection connection = edge.Edge as ExperimentNodeConnection;
                if (connection != null && connection.IsFixed == false)
                {
                    canRedirect = true;
                }
            }

            return canRedirect;
        }

        private void StartCreateLinkDrag(GraphSharp.Controls.VertexControl node)
        {
            Point clickPoint = Mouse.GetPosition(GraphLayout);
            
            RevealHACKControls(clickPoint, node);

            int timestamp = new TimeSpan(DateTime.Now.Ticks).Milliseconds;
            BeginDrag(Mouse.PrimaryDevice, timestamp, MouseButton.Left);
        }

        /// <summary>
        /// Begins the drag.
        /// </summary>
        /// <param name="mouse">The logical Mouse device associated with this event.</param>
        /// <param name="timestamp"> The time when the input occurred.</param>
        /// <param name="button">The mouse button whose state is being described.</param>
        private void BeginDrag(MouseDevice mouse, int timestamp, MouseButton button)
        {
            MouseButtonEventArgs args = new MouseButtonEventArgs(mouse, timestamp, button);
            args.RoutedEvent = MouseLeftButtonDownEvent;
            args.Source = HACK_VertexControl;

            // Don't raise the event immediately so that the MouseDevice has time to 
            // update it's position relative to the control.
            HACK_VertexControl.Dispatcher.BeginInvoke(new Action(() => { HACK_VertexControl.RaiseEvent(args); }), System.Windows.Threading.DispatcherPriority.Input, null);

            // When we lose mouse capture, then cancel all dragging.
            HACK_VertexControl.LostMouseCapture += graphLayout_LostMouseCapture;
        }

        private void RevealHACKControls(Point clickPoint, GraphSharp.Controls.VertexControl parent)
        {
            PotentialLinkSource = parent;

            HACK_VertexControl.CenterX = clickPoint.X;
            HACK_VertexControl.CenterY = clickPoint.Y;
            HACK_VertexControl.Visibility = Visibility.Visible;
            HACK_VertexControl.PreviewMouseLeftButtonUp += HACK_Control_PreviewMouseLeftButtonUp;
            HACK_VertexControl.PreviewMouseMove += HACK_Control_PreviewMouseMove;

            HACK_EdgeControl.SetValue(GraphSharp.Controls.EdgeControl.SourceProperty, parent);
            HACK_EdgeControl.SetValue(GraphSharp.Controls.EdgeControl.TargetProperty, HACK_VertexControl);
            HACK_EdgeControl.Visibility = Visibility.Visible;
        }

        #endregion

        #endregion

        #region Mouse events

        private Point? m_dragStartPoint;
        private Point? m_lastDragPoint;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.Handled != true)
            {
                // Hit-test order of operations
                // if hit: button
                // - this must happen first so that button presses do not start a drag-behaviour.
                // - do nothing
                // - return
                // if hit: vertex info
                // - do nothing
                // if hit: connection between vertexes
                // - do nothing
                // if hit: vertex
                // - select vertex (if not selected)
                // - prepare to start dragging
                // if hit: empty
                // - prepare marquee draw box
                // - prepare to start dragging

                bool shouldStartDragging = true;
                var hitElement = InputHitTest(e.GetPosition(this)) as DependencyObject;
                if (hitElement != null)
                {
                    // If the user clicked on a button, then we don't want to do anything (let the button handle all the processing)
                    if (hitElement.GetParent<Button>(this) != null)
                    {
                        shouldStartDragging = false;
                    }

                    // If the user clicked on an info panel, then we don't want to do anything (they exist "above" the graph and cannot be selected in the same way.)
                    if (shouldStartDragging == true && hitElement.GetParent<TraceLab.UI.WPF.Views.Nodes.NodeInfoContainer>(this) != null)
                    {
                        shouldStartDragging = false;
                    }

                    // If the user clicked on an edge - do nothing, let the edge handle everything.
                    if (shouldStartDragging == true && hitElement.GetParent<GraphSharp.Controls.EdgeControl>(this) != null)
                    {
                        shouldStartDragging = false;
                    }

                    // We're definitely going to start dragging _something_ - either the marquee or the current selection.
                    if (shouldStartDragging == true)
                    {

                        e.Handled = true;
                        m_dragStartPoint = e.GetPosition(GraphLayout);
                        m_lastDragPoint = m_dragStartPoint.Value;

                        var hitVertex = hitElement.GetParent<GraphSharp.Controls.VertexControl>(this);
                        if (hitVertex != null)
                        {
                            // 1) if vertex is not already selected, then clear selection and select this vertex.
                            if ((bool)hitVertex.GetValue(GraphView.IsSelectedProperty) == false)
                            {
                                UnselectAll();
                                SetIsSelected(hitVertex, true);
                            }

                            // 2) Begin dragging of entire selection
                            // - nothing special needs to happen here, since we're prepared to start dragging
                            // - and "marquee" dragging only happens if the marquee is NOT collapsed.
                        }
                        else
                        {
                            // Begin dragging marquee
                            MarqueeAdorner.SetValue(Canvas.LeftProperty, m_dragStartPoint.Value.Y);
                            MarqueeAdorner.SetValue(Canvas.TopProperty, m_dragStartPoint.Value.Y);
                            MarqueeAdorner.Width = 1;
                            MarqueeAdorner.Height = 1;
                            MarqueeAdorner.Visibility = System.Windows.Visibility.Hidden;

                            // Clear the selection in preparation.
                            UnselectAll();
                        }
                    }
                }

                // Take focus
                Focus();
            }
        }

        private void ResetDrag()
        {
            m_dragStartPoint = null;
            m_lastDragPoint = null;
            MarqueeAdorner.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!e.Handled)
            {
                // Order of operations:
                // if marquee dragging
                // - apply delta to marquee
                // - test all components to see if they are within the marquee and set selection accordingly.
                // else if move dragging
                // - apply delta to all selected items


                // if we've started a drag
                if (m_dragStartPoint.HasValue)
                {
                    e.Handled = true;

                    Point currentPoint = e.GetPosition(GraphLayout);

                    // if the marquee is visible, then we're doing a marquee selection.
                    if (MarqueeAdorner.Visibility != System.Windows.Visibility.Collapsed)
                    {
                        var left = Math.Min(m_dragStartPoint.Value.X, currentPoint.X);
                        var top = Math.Min(m_dragStartPoint.Value.Y, currentPoint.Y);
                        var right = Math.Max(m_dragStartPoint.Value.X, currentPoint.X);
                        var bottom = Math.Max(m_dragStartPoint.Value.Y, currentPoint.Y);

                        // If the user has moved the mouse at all, then show the selection box.
                        if (right != left || bottom != top)
                        {
                            MarqueeAdorner.Visibility = System.Windows.Visibility.Visible;
                        }

                        MarqueeAdorner.SetValue(Canvas.LeftProperty, left);
                        MarqueeAdorner.SetValue(Canvas.TopProperty, top);
                        MarqueeAdorner.Width = right - left;
                        MarqueeAdorner.Height = bottom - top;

                        Rect marqueeBounds = new Rect(new Point(left, top), new Point(right, bottom));

                        foreach (GraphSharp.Controls.VertexControl potentialSelectedVertex in GraphLayout.GetAllVertexControls())
                        {
                            Rect vertexBounds = new Rect(new Point(potentialSelectedVertex.TopLeftX, potentialSelectedVertex.TopLeftY), new Size(potentialSelectedVertex.ActualWidth, potentialSelectedVertex.ActualHeight));
                            if (marqueeBounds.IntersectsWith(vertexBounds))
                            {
                                SetIsSelected(potentialSelectedVertex, true);
                            }
                            else
                            {
                                SetIsSelected(potentialSelectedVertex, false);
                            }
                        }
                    }
                    else
                    {
                        Vector delta = currentPoint - m_lastDragPoint.Value;
                        foreach (GraphSharp.Controls.VertexControl potentialSelectedVertex in s_selectedVertices)
                        {
                            potentialSelectedVertex.CenterX += delta.X;
                            potentialSelectedVertex.CenterY += delta.Y;
                        }
                    }

                    m_lastDragPoint = currentPoint;
                }
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            // if dragging
            // always abort all drag operations
            ResetDrag();
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);

            // if focus is NOT within,
            // - abort all drag operations (verify if OnIsKeyboardFocusWithinChanged will do everything we need - I think it will)
            if (IsKeyboardFocusWithin == false)
            {
                ResetDrag();
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (e.Handled != true)
            {
                var hitElement = InputHitTest(e.GetPosition(this)) as DependencyObject;
                if (hitElement != null)
                {
                    e.Handled = true;
                    GraphSharp.Controls.VertexControl hitVertex = hitElement.GetParent<GraphSharp.Controls.VertexControl>(this);
                    if (hitVertex != null)
                    {
                        // 1) if vertex is not already selected, then clear selection and select this vertex.
                        if ((bool)hitVertex.GetValue(GraphView.IsSelectedProperty) == false)
                        {
                            UnselectAll();
                            SetIsSelected(hitVertex, true);
                        }
                    }
                    else
                    {
                        GraphSharp.Controls.EdgeControl hitEdge = hitElement.GetParent<GraphSharp.Controls.EdgeControl>(this);
                        if (hitEdge != null)
                        {
                            CreateRoutePointOnEdge(e, hitEdge);
                        }
                    }
                }

                // Take focus
                Focus();
            }
        }

        /// <summary>
        /// Creates the route point on edge.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="hitEdge">The hit edge.</param>
        private void CreateRoutePointOnEdge(MouseButtonEventArgs e, GraphSharp.Controls.EdgeControl hitEdge)
        {
            ExperimentNodeConnection connection = (ExperimentNodeConnection)hitEdge.Edge;
            
            Point canvasPoint = e.GetPosition(GraphLayout);
            RoutePoint finalPoint = new RoutePoint(canvasPoint.X, canvasPoint.Y);

            connection.RoutePoints.Add(finalPoint);
        }

        #endregion

        #region Remove Node

        #region Dependency Property RemoveNodeCommand

        /// <summary>
        /// The dependency property that allows binding the RemoveNodeCommand to the view-model command
        /// from view xaml. 
        /// For example ScopeGraph and DockableGraph controls in files: GraphDocument, ScopeNodeControl and LoopScopeNode control
        /// bind this property to the view-model RemoveNode Command of TopLevelEditableExperimentViewModel.
        /// </summary>
        public static readonly DependencyProperty RemoveNodeCommandProperty = DependencyProperty.Register("RemoveNodeCommand", typeof(ICommand), typeof(GraphView));

        /// <summary>
        /// Gets or sets the remove node command.
        /// This is the property of above dependency property
        /// </summary>
        /// <value>
        /// The remove node command.
        /// </value>
        public ICommand RemoveNodeCommand
        {
            get
            {
                return (ICommand)GetValue(RemoveNodeCommandProperty);
            }
            set
            {
                SetValue(RemoveNodeCommandProperty, value);
            }
        }

        #endregion

        #region Routed Command Binding Handlers

        /// <summary>
        /// Handler that executes the remove node command.
        /// This particular handler delegates execution to the RemoveNodeCommand, that must be set previously via binding. 
        /// (See above dependency property.)
        /// 
        /// The handler is needed for RoutedCommand. See <see cref="RoutedCommands"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        protected void ExecuteRemoveNode(object sender, ExecutedRoutedEventArgs e)
        {
            if (RemoveNodeCommand != null)
            {
                RemoveNodeCommand.Execute(e.Parameter);
            }
        }

        /// <summary>
        /// Handler that determines whether remove node command can be executed on the specified node.
        /// The handler delegates it to the RemoveNodeCommand, that must be set previously via binding. 
        /// (See above dependency property.)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        protected void CanExecuteRemoveNode(object sender, CanExecuteRoutedEventArgs e)
        {
            if (RemoveNodeCommand != null)
            {
                e.CanExecute = RemoveNodeCommand.CanExecute(e.Parameter);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        #endregion Remove Node

        #region Remove Selected Nodes

        #region Dependency Property RemoveNodeCommand

        /// <summary>
        /// The dependency property that allows binding the RemoveSelectedNodesCommand to the view-model command
        /// from view xaml. 
        /// For example ScopeGraph and DockableGraph controls in files: GraphDocument, ScopeNodeControl and LoopScopeNode control
        /// bind this property to the view-model RemoveSelectedNodes Command of TopLevelEditableExperimentViewModel.
        /// </summary>
        public static readonly DependencyProperty RemoveSelectedNodesCommandProperty = DependencyProperty.Register("RemoveSelectedNodesCommand", typeof(ICommand), typeof(GraphView));

        /// <summary>
        /// Gets or sets the remove selected node command.
        /// This is the property of above dependency property
        /// </summary>
        /// <value>
        /// The remove node command.
        /// </value>
        public ICommand RemoveSelectedNodesCommand
        {
            get { return (ICommand)GetValue(RemoveSelectedNodesCommandProperty); }
            set { SetValue(RemoveSelectedNodesCommandProperty, value); }
        }

        #endregion

        #region Routed Command Binding Handlers

        /// <summary>
        /// Handler that executes the remove selected node command.
        /// This particular handler delegates execution to the RemoveSelectedNodesCommand, that must be set previously via binding. 
        /// (See above dependency property.)
        /// 
        /// The handler is needed for RoutedCommand. See <see cref="RoutedCommands"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        protected void ExecuteRemoveSelectedNodes(object sender, ExecutedRoutedEventArgs e)
        {
            if (RemoveSelectedNodesCommand != null)
            {
                RemoveSelectedNodesCommand.Execute(e.Parameter);
            }
        }

        /// <summary>
        /// Handler that determines whether remove node command can be executed on the specified node.
        /// The handler delegates it to the RemoveSelectedNodesCommand, that must be set previously via binding. 
        /// (See above dependency property.)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        protected void CanExecuteRemoveSelectedNodes(object sender, CanExecuteRoutedEventArgs e)
        {
            if (RemoveSelectedNodesCommand != null)
            {
                e.CanExecute = RemoveSelectedNodesCommand.CanExecute(e.Parameter);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        #endregion Remove Selected Nodes

        #region Toogle Log Level

        #region Dependency Property RemoveNodeCommand

        /// <summary>
        /// The dependency property that allows binding the ToogleLogLevelCommand to the view-model command
        /// from view xaml. 
        /// For example ScopeGraph and DockableGraph controls in files: GraphDocument, ScopeNodeControl and LoopScopeNode control
        /// bind this property to the view-model ToogleLogLevel Command of TopLevelEditableExperimentViewModel.
        /// </summary>
        public static readonly DependencyProperty ToogleLogLevelCommandProperty = DependencyProperty.Register("ToogleLogLevelCommand", typeof(ICommand), typeof(GraphView));

        /// <summary>
        /// Gets or sets the toogle log level command.
        /// This is the property of above dependency property
        /// </summary>
        /// <value>
        /// The toogle log level command.
        /// </value>
        public ICommand ToogleLogLevelCommand
        {
            get { return (ICommand)GetValue(ToogleLogLevelCommandProperty); }
            set { SetValue(ToogleLogLevelCommandProperty, value); }
        }

        #endregion

        #region Routed Command Binding Handlers

        /// <summary>
        /// Handler that executes the Toogle Log Level command.
        /// This particular handler delegates execution to the ToogleLogLevelCommand, that must be set previously via binding. 
        /// (See above dependency property.)
        /// 
        /// The handler is needed for RoutedCommand. See <see cref="RoutedCommands"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        protected void ExecuteToggleLogLevel(object sender, ExecutedRoutedEventArgs e)
        {
            if (ToogleLogLevelCommand != null)
            {
                ToogleLogLevelCommand.Execute(e.Parameter);
            }
        }

        /// <summary>
        /// Handler that determines whether Toogle Log Level command can be executed on the specified node.
        /// The handler delegates it to the ToogleLogLevelCommand, that must be set previously via binding. 
        /// (See above dependency property.)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        protected void CanToggleLogLevel(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ToogleLogLevelCommand != null)
            {
                e.CanExecute = ToogleLogLevelCommand.CanExecute(e.Parameter);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        #endregion Toogle Log Level

        #region Add Scope to Decision

        /// <summary>
        /// Performs operation of adding the new scope to the given decision.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        protected void ExecuteAddScopeToDecision(object sender, ExecutedRoutedEventArgs e)
        {
            GraphSharp.Controls.VertexControl vertexControl = e.Parameter as GraphSharp.Controls.VertexControl;

            if (vertexControl != null)
            {
                ExperimentDecisionNode decisionNode = vertexControl.Vertex as ExperimentDecisionNode;
                
                if (decisionNode != null) 
                {
                    ExitDecisionNode exitDecisionNode = null;
                    IEditableExperiment experiment = decisionNode.Owner as IEditableExperiment;
                    double rightmostX = decisionNode.Data.X;
                    HashSet<string> currentLabels = new HashSet<string>();

                    //iterate through outgoing scopes and find the scope with right border located most to the right among all scopes
                    //also locate among outgoing edges reference to exit 
                    IEnumerable<ExperimentNodeConnection> outEdges;
                    if(experiment.TryGetOutEdges(decisionNode, out outEdges)) 
                    {
                        foreach (ExperimentNodeConnection connection in outEdges)
                        {
                            ScopeNode scope = connection.Target as ScopeNode;
                            
                            if (scope != null)
                            {
                                double candidateRightMostX = scope.DataWithSize.X + scope.DataWithSize.Width / 2;
                                if (candidateRightMostX > rightmostX)
                                {
                                    rightmostX = candidateRightMostX;
                                }

                                //also collect labels
                                currentLabels.Add(scope.Data.Metadata.Label);
                            }
                            else if (exitDecisionNode == null)
                            {
                                //try find exit decision node
                                exitDecisionNode = connection.Target as ExitDecisionNode;
                            }
                        }
                    }

                    double xPosition = rightmostX + 100;
                    double yPosition = decisionNode.Data.Y + 120;

                    string finalLabel = DetermineNewScopeLabel(currentLabels);

                    //check if deicion node is not null. In old decision nodes without scopes, there were no associated exit node, 
                    //thus the scope cannot be added for these decisions.
                    if (exitDecisionNode != null)
                    {
                        ComponentFactory.AddScopeToDecision(finalLabel, xPosition, yPosition, decisionNode, exitDecisionNode, experiment);
                    }
                }
            }
        }

        /// <summary>
        /// Determines the new scope label.
        /// </summary>
        /// <param name="currentLabels">The current labels.</param>
        /// <returns></returns>
        private static string DetermineNewScopeLabel(HashSet<string> currentLabels)
        {
            //determine scope label
            string finalLabel;
            int counter = 0;
            do
            {
                counter++;
                finalLabel = "Scope " + counter.ToString();
            } while (currentLabels.Contains(finalLabel));
            return finalLabel;
        }

        #endregion

        #region AutoAlign Command TODO

        private ICommand _autoAlignCommand;
        /// <summary>
        /// Gets the auto align command.
        /// </summary>
        public ICommand AutoAlignCommand
        {
            get
            {
                return _autoAlignCommand;
            }
        }

        private void DoAutoAlign(object param)
        {
            //TODO fix!
            //graphLayout.LayoutAlgorithmType = appvm.ExperimentManager.SelectedExperiment.ExperimentInfo.LayoutName;
            //graphLayout.Relayout();
            //graphLayout.LayoutAlgorithmType = "";
        }

        private bool CanAutoAlign(object param)
        {
            return false;
        }

        public void Relayout()
        {
            GraphLayout.Relayout();
        }

        public string LayoutAlgorithmType
        {
            get
            {
                return GraphLayout.LayoutAlgorithmType;
            }
            set
            {
                GraphLayout.LayoutAlgorithmType = value;
            }
        }

        #endregion
    }
}

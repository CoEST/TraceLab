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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TraceLab.Core.Experiments;
using TraceLab.Core.Settings;
using TraceLab.UI.WPF.Commands;
using TraceLab.UI.WPF.EventArgs;
using TraceLab.UI.WPF.Selectors;
using TraceLab.UI.WPF.Utilities;
using System.Collections.Generic;
using TraceLab.UI.WPF.Views.Nodes;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for ReadonlyDockableGraph.xaml
    /// </summary>
    public partial class ReadonlyDockableGraph : UserControl, INodeControlSelector
    {
        #region Constructor

        static ReadonlyDockableGraph()
        {
            IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(Boolean), typeof(ReadonlyDockableGraph),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsSelectedChanged));

            SelectedNodesProperty = DependencyProperty.Register("SelectedNodes", typeof(IEnumerable<ExperimentNode>), typeof(ReadonlyDockableGraph));
            SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ReadonlyDockableGraph));
            UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ReadonlyDockableGraph));
            SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ReadonlyDockableGraph));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DockableGraph"/> class.
        /// </summary>
        public ReadonlyDockableGraph()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(ReadonlyDockableGraph_DataContextChanged);
            Loaded += ReadonlyDockableGraph_Loaded;
        }


        #endregion

        #region Toogle Log Level

        private void ExecuteToggleLogLevel(object sender, ExecutedRoutedEventArgs e)
        {
            LogLevelItem item = e.Parameter as LogLevelItem;
            if (item != null)
            {
                item.IsEnabled = !item.IsEnabled;
            }
        }

        private void CanToggleLogLevel(object sender, CanExecuteRoutedEventArgs e)
        {
            LogLevelItem item = e.Parameter as LogLevelItem;
            if (item != null && item.IsLocked == false)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        #region INodeControlSelector Members

        public GraphSharp.Controls.VertexControl ControlForNode(ExperimentNode node)
        {
            return graphLayout.GetNodeControl(node);
        }

        #endregion

        void ReadonlyDockableGraph_Loaded(object sender, RoutedEventArgs e)
        {
            SetPositionAndZoom(DataContext as TraceLab.UI.WPF.ViewModels.IZoomableViewModel);
        }

        /*
         * Handles the positioning of the view for the new data context 
         * and saving out the old position for the old view
         */
        void ReadonlyDockableGraph_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                oldModel.TranslateX = zoomControl.TranslateX;
                oldModel.TranslateY = zoomControl.TranslateY;
                oldModel.Zoom = zoomControl.Zoom;
            }
        }

        private void SetPositionAndZoom(TraceLab.UI.WPF.ViewModels.IZoomableViewModel newModel)
        {
            if (newModel != null)
            {
                double newX = newModel.TranslateX;
                double newY = newModel.TranslateY;
                double newZoom = newModel.Zoom;

                zoomControl.SetPositionAndZoom(newX, newY, newZoom);
            }
        }

        public static readonly DependencyProperty IsConfigEnabledProperty = DependencyProperty.Register("IsConfigEnabled", typeof(bool), typeof(ReadonlyDockableGraph));

        public bool IsConfigEnabled
        {
            get { return (bool)GetValue(IsConfigEnabledProperty); }
            set { SetValue(IsConfigEnabledProperty, value); }
        }



        ////       

        #region Selection

        private HashSet<GraphSharp.Controls.VertexControl> m_selectedVertices = new HashSet<GraphSharp.Controls.VertexControl>();

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
            var graph = vertex.GetParent<ReadonlyDockableGraph>(null);

            if (graph != null)
            {
                if ((bool)args.NewValue)
                {
                    graph.m_selectedVertices.Add(vertex);
                }
                else
                {
                    graph.m_selectedVertices.Remove(vertex);
                }
            }
        }

        public void UnselectAll()
        {
            var selection = new List<GraphSharp.Controls.VertexControl>(m_selectedVertices);

            foreach (GraphSharp.Controls.VertexControl control in selection)
            {
                SetIsSelected(control, false);
            }
        }

        protected void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

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
                    if (shouldStartDragging == true && hitElement.GetParent<NodeInfoContainer>(this) != null)
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
                        m_dragStartPoint = e.GetPosition(graphLayout);
                        m_lastDragPoint = m_dragStartPoint.Value;

                        var hitVertex = hitElement.GetParent<GraphSharp.Controls.VertexControl>(this);
                        if (hitVertex != null)
                        {
                            // 1) if vertex is not already selected, then clear selection and select this vertex.
                            if ((bool)hitVertex.GetValue(DockableGraph.IsSelectedProperty) == false)
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

                    Point currentPoint = e.GetPosition(graphLayout);

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

                        foreach (GraphSharp.Controls.VertexControl potentialSelectedVertex in graphLayout.GetAllVertexControls())
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
                        foreach (GraphSharp.Controls.VertexControl potentialSelectedVertex in m_selectedVertices)
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

        #endregion

    }
}

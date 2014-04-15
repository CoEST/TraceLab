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
//
using System;
using MonoHotDraw.Util;
using MonoHotDraw.Figures;
using System.Collections.Generic;
using MonoHotDraw.Tools;
using TraceLab.Core.Experiments;
using MonoHotDraw;
using MonoHotDraw.Locators;
using MonoHotDraw.Handles;
using System.Linq;
using Gdk;
using Gtk;

namespace TraceLab.UI.GTK
{
    public class BasicNodeControl: SimpleTextFigure, IComponentControl
    {
        public BasicNodeControl(ExperimentNode node, ApplicationContext applicationContext)
            : this(node, applicationContext, s_waitForAnyAllHandleXLocation, s_waitForAnyAllHandleYLocation)
        {
        }

        protected BasicNodeControl(ExperimentNode node, ApplicationContext applicationContext, 
                                double waitForAnyAllHandleXLocation, 
                                double waitForAnyAllHandleYLocation) : base(node.Data.Metadata.Label)
        {
            m_node = node;
            m_applicationContext = applicationContext;

            this.TextChanged += HandleTextChanged;

            //if error changes invalidate figure so that it is redraw
            m_node.ErrorChanged += (sender, e) => 
            { 
                Invalidate(); 
            };
                       
            m_node.PropertyChanged += RedrawOnIsExecutingChange;

            InitWaitForAnyHandle(waitForAnyAllHandleXLocation, waitForAnyAllHandleYLocation);

            PaddingLeft = 15.0;
            PaddingTop = 7.0;
            PaddingRight = 24.0;
            PaddingBottom = 7.0;
        }

        private void HandleTextChanged (object sender, EventArgs e)
        {
            //update corresponding model metadata
            if(m_node.Data.Metadata.Label != this.Text)
            {
                m_node.Data.Metadata.Label = this.Text;
            }
        }

        private static String isExecutingPropertyName = "IsExecuting";
        private void RedrawOnIsExecutingChange (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(isExecutingPropertyName))
            {
                Invalidate();
            }
        }

        static BasicNodeControl() 
        {
            s_waitForAllIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.all16.png");
            s_waitForAnyIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.any16.png");
        }

        /// <summary>
        /// Inits the wait for any handle at the given x an y location
        /// </summary>
        /// <param name="xLocation">X location.</param>
        /// <param name="yLocation">Y location.</param>
        private void InitWaitForAnyHandle(double xLocation, double yLocation)
        {
            //get absolute position
            m_waitForAnyAllHandle = new PixToggleButtonHandle(this, 
                                                              new AbsoluteLocator(xLocation, yLocation, AbsoluteLocator.AbsoluteTo.TopRight), 
                                                               s_waitForAnyIcon, s_waitForAllIcon);

            m_waitForAnyAllHandle.Toggled += delegate (object sender, ToggleEventArgs e) 
            {
                m_node.Data.Metadata.WaitsForAllPredecessors = e.Active;
            };

            m_waitForAnyAllHandle.Active = m_node.Data.Metadata.WaitsForAllPredecessors;
        }

        /// <summary>
        /// Default draw of the control (when it is not selected)
        /// </summary>
        /// <param name="context">Context.</param>
        public override void BasicDraw (Cairo.Context context) 
        {
            Cairo.Color fillColor = s_defaultFillColor;

            //node can be highlighted if user is over corresponding info pane
            //or when it is executing
            if(isHighlighted || m_node.IsExecuting)
                fillColor = s_highlightFillColor;

            if(m_node.HasError)
                fillColor = s_errorFillColor;

            DrawFrame(context, s_defaultLineWidth, s_defaultLineColor, fillColor);
            FontColor = s_defaultFontColor;
            base.BasicDraw(context);
        }

        /// <summary>
        /// It draws control when it is selected.
        /// </summary>
        /// <param name="context">Context.</param>
        public override void BasicDrawSelected (Cairo.Context context)
        {
            Cairo.Color fillColor = s_selectedFillColor;

            //node can be highlighted if user is over corresponding info pane
            //or when it is executing
            if(isHighlighted || m_node.IsExecuting)
                fillColor = s_highlightFillColor;

            if(m_node.HasError)
                fillColor = s_errorFillColor;

            DrawFrame(context, s_selectedLineWidth, s_selectedLineColor, fillColor);
            FontColor = s_selectedFontColor;
            base.BasicDraw(context);
        }

        public bool IsHighlighted
        {
            get { return isHighlighted; }
            set 
            {
                isHighlighted = value;
                Invalidate();
            }
        }

        private bool isHighlighted = false;
        
        /// <summary>
        /// Draws the rectangle frame into the given cairo context
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="lineWidth">Line width.</param>
        /// <param name="lineColor">Line color.</param>
        /// <param name="fillColor">Fill color.</param>
        protected virtual void DrawFrame(Cairo.Context context, double lineWidth, Cairo.Color lineColor, Cairo.Color fillColor)
        {
            RectangleD rect = DisplayBox;
            rect.OffsetDot5();
            CairoFigures.CurvedRectangle (context, rect, 8);
            context.Color = fillColor;
            context.FillPreserve();
            context.Color = lineColor;
            context.LineWidth = lineWidth;
            context.Stroke();
        }

        /// <summary>
        /// The corresponding node in the experiment
        /// </summary>
        private ExperimentNode m_node;
        
        /// <summary>
        /// Gets the experiment node.
        /// </summary>
        /// <value>The experiment node.</value>
        public ExperimentNode ExperimentNode 
        {
            get { 
                return m_node;
            }
        }
        
        /// <summary>
        /// It refreshes component display when dragging it. 
        /// 
        /// This property returns the DisplayBox inflated by enough pixels,
        /// so that when moving component to the left or up the marks of  
        /// handle icons are not being left on the canvas.
        /// 
        /// Previously ugly marks were left on the canvas when dragging component. 
        /// This property is responsible for clearing the context behind
        /// it must by basically sligly larger than current display box. Adjusting it, fixes display.
        /// </summary>
        /// <value>
        /// The invalidate display box.
        /// </value>
        public override RectangleD InvalidateDisplayBox 
        {
            get 
            { 
                RectangleD rect = DisplayBox;
                rect.Inflate (60.0, 60.0);
                return rect;
            }
        }
        
        public override ITool CreateFigureTool(IPrimaryToolDelegator mainTool, IDrawingEditor editor, 
                                               ITool defaultTool, MouseEvent ev)
        {
            DragTool dragTool = defaultTool as DragTool;
            if(dragTool != null)
            {
                //when drag is completed update model data
                dragTool.DragCompleted += (object sender, EventArgs e) =>  
                {
                    this.ExperimentNode.Data.X = this.DisplayBox.X;
                    this.ExperimentNode.Data.Y = this.DisplayBox.Y;
                };
            }
            
            return base.CreateFigureTool(mainTool, editor, defaultTool, ev);
        }

        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            {
                if(IsEditable) 
                {
                    foreach (IHandle handle in base.HandlesEnumerator) 
                    {
                        yield return handle;
                    }

                    int numberOfIncomingEdges = m_node.Owner.InEdges(m_node).Count();
                    if(numberOfIncomingEdges >= 2) 
                    {
                        if(m_waitForAnyAllHandle != null) 
                        {
                            yield return m_waitForAnyAllHandle;
                        }
                    }
                }
            }
        }

        public bool IsEditable 
        {
            get;
            set;
        }

        public override bool IsSelected {
            get {
                return base.IsSelected;
            }
            set {
                //update also corresponding value in model experiment node
                base.IsSelected = value;
                this.ExperimentNode.IsSelected = value;
            }
        }

        protected ApplicationContext m_applicationContext;
        protected ToggleButtonHandle m_waitForAnyAllHandle;

        private static Cairo.Color s_defaultFillColor = new Cairo.Color(227.0/255, 227.0/255, 227.0/255);
        private static Cairo.Color s_defaultLineColor = new Cairo.Color(122.0/255, 122.0/255, 122.0/255);
        private static Cairo.Color s_defaultFontColor = new Cairo.Color(0,0,0);
        private static double s_defaultLineWidth = 1.5;
        
        private static Cairo.Color s_selectedFillColor = new Cairo.Color(95.0/255, 129.0/255, 230.0/255);
        private static Cairo.Color s_selectedLineColor = new Cairo.Color(64.0/255,64.0/255,168.0/255);
        private static Cairo.Color s_selectedFontColor = new Cairo.Color(1.0, 1.0, 1.0);
        private static double s_selectedLineWidth = 1.5;

        private static Cairo.Color s_errorFillColor = new Cairo.Color(254.0/255, 0.0, 0.0);
        private static Cairo.Color s_highlightFillColor = new Cairo.Color(29.0/255, 186.0/255, 0.0);

        private static Gdk.Pixbuf s_waitForAllIcon;
        private static Gdk.Pixbuf s_waitForAnyIcon;
        
        /// <summary>
        /// Determines the default x location of Any/All button 
        /// </summary>
        private static double s_waitForAnyAllHandleXLocation = 11;
        
        /// <summary>
        /// Determines the default x location of Any/All button 
        /// </summary>
        private static double s_waitForAnyAllHandleYLocation = 14.9;
    }
}


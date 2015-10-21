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
using MonoHotDraw.Figures;
using System.Collections.Generic;
using MonoHotDraw.Handles;
using MonoHotDraw;
using Cairo;
using MonoHotDraw.Util;
using TraceLab.Core.Experiments;
using MonoHotDraw.Locators;
using TraceLab.UI.GTK.Extensions;

namespace TraceLab.UI.GTK
{
    public class NodeConnectionControl: LineConnection 
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.NodeConnection"/> class.
        /// </summary>
        public NodeConnectionControl(Experiment experimentOwner): base() 
        {
            m_experimentOwner = experimentOwner;
            InitRemoveHandle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.NodeConnection"/> class.
        /// </summary>
        /// <param name='source'>
        /// Source.
        /// </param>
        /// <param name='target'>
        /// Target.
        /// </param>
        public NodeConnectionControl(Experiment experimentOwner, IFigure source, IFigure target): base(source, target) 
        {
            m_experimentOwner = experimentOwner;
            InitRemoveHandle();
        }

        public bool IsEditable 
        {
            get;
            set;
        }

        private void InitRemoveHandle() 
        {
            m_removeHandle = new RemoveConnectionHandle (this, new CenterLineLocator(0.5, -12.0));
        }

        #endregion

        #region Connecting source and target components

        /// <summary>
        /// Determines whether the specified figure can be connected at the end of connection
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified figure can be connected at the end of connection; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='figure'>
        /// Component control
        /// </param>
        public override bool CanConnectEnd (IFigure figure) 
        {
            bool isTargetValid = false;
            ExperimentNode node;
            IComponentControl componentControl = figure as IComponentControl;
            if(componentControl != null) 
            {
                node = componentControl.ExperimentNode;

                if (node != null && node is ScopeNode == false &&
                    (node is ExperimentEndNode || node is ComponentNode || node is ExperimentDecisionNode || node is CompositeComponentNode))
                {
                    isTargetValid = true;
                }
                // HERZUM SPRINT 0.0
                else if (node != null && node is ScopeNode == true) {
                    ScopeNode scopeNode = node as ScopeNode;
                    if (scopeNode.DecisionNode == null)
                    {
                        isTargetValid = true;
                    }
                }
                // END HERZUM 0.0
                // SPRINT 2.0 TLAB-135
                if (isTargetValid)
                {
                    IComponentControl startComponentControl = StartConnector.Owner as IComponentControl;
                    if (startComponentControl.ExperimentNode!=null)
                        foreach(ExperimentNodeConnection edge in componentControl.ExperimentNode.Owner.Edges)
                            if (edge.Source.Equals(startComponentControl.ExperimentNode) && edge.Target.Equals(node))
                                isTargetValid = false;
                    // END SPRINT 2.0 TLAB-135
                }

            }
            return isTargetValid;
        }

        private bool isSourceComment =false;

        /// <summary>
        /// Determines whether the specified figure can be connected at the start of connection
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified figure can be connected at the start of connection; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='figure'>
        /// Component control
        /// </param>
        public override bool CanConnectStart (IFigure figure) 
        {
            bool isSourceValid = false;
            IComponentControl componentControl = figure as IComponentControl;
            if(componentControl != null) 
            {
                ExperimentNode node = componentControl.ExperimentNode;
                if (node != null && node is ScopeNode == false &&
                    (node is ExperimentEndNode || node is ComponentNode || node is ExperimentDecisionNode || node is CompositeComponentNode))
                {
                    isSourceValid = true;
                }
            }

            return isSourceValid;
        }

        /// <summary>
        /// Connects the end.
        /// When connecting the end, it also updates information in the experiment model data.
        /// </summary>
        /// <param name='end'>
        /// End.
        /// </param>
        public override void ConnectEnd(MonoHotDraw.Connectors.IConnector end)
        {
            base.ConnectEnd(end);

            if(end != null) 
            {
                IComponentControl startComponentControl = StartConnector.Owner as IComponentControl;
                IComponentControl endComponentControl = end.Owner as IComponentControl;
                if(endComponentControl != null && startComponentControl != null) 
                {
                    //get access to owner experiment
                    IEditableExperiment ownerExperiment = endComponentControl.ExperimentNode.Owner as IEditableExperiment;
                    if(ownerExperiment != null) 
                    {
                        ExperimentNodeConnection nodeConnection = ownerExperiment.AddConnection(startComponentControl.ExperimentNode, endComponentControl.ExperimentNode);
                        OnConnectionCompleted(nodeConnection);
                        m_experimentNodeConnection = nodeConnection;
                        
                        // SPRINT 2: TLAB-129
                        CommentNode comment = nodeConnection.Source as CommentNode;
                        if (comment != null)
                             isSourceComment = true;
                        // END SPRINT 2: TLAB-129
                    }
                }
            }
        }

        #endregion

        internal event EventHandler<ConnectionCompletedEventArgs> ConnectionCompleted;

        private void OnConnectionCompleted(ExperimentNodeConnection nodeConnection) 
        {
            if(ConnectionCompleted != null) 
            {
                ConnectionCompleted(this, new ConnectionCompletedEventArgs(nodeConnection));
            }
        }

        private static Cairo.Color s_defaultColor = new Cairo.Color(144.0/255, 144.0/255, 144.0/255);
        private static Cairo.Color s_selectedColor = new Cairo.Color(53.0/255, 154.0/255, 1.0);

        public override void BasicDraw (Context context)
        {
            LineColor = s_defaultColor;
            FillColor = s_defaultColor;

            // SPRINT 2: TLAB-129
            if (isSourceComment)
            {
                double[] dash = {2, 0, 2};
                base.Dashes=dash;
                base.BasicDraw(context);
                return;
            }
            // END SPRINT 2: TLAB-129

            base.BasicDraw(context);
            DrawArrowHead(context, EndPoint, StartPoint);
        }

        public override void BasicDrawSelected (Context context)
        {
            LineColor = s_selectedColor;
            FillColor = s_selectedColor;
            //base draw selected is not implemented, and if so, it calls BasicDraw overriden above
            //thus call BasicDraw directly

            // SPRINT 2: TLAB-129
            if (isSourceComment)
            {
                double[] dash = {2, 0, 2};
                base.Dashes=dash;
                base.BasicDraw(context);
                return;
            }
            // END SPRINT 2: TLAB-129



            base.BasicDraw(context);
            DrawArrowHead(context, EndPoint, StartPoint);
        }

        private static void DrawArrowHead(Context context, PointD tip, PointD tail)
        {
            // Length & width of arrow (Adjustable)
            const double aLength = 10.0;
            const double aWidth = 3.5;
            
            // Arrow direction & perpendicular vector
            PointD direction = tail.Substract(tip); // tail - tip
            direction = direction.Normalize();
            PointD perpendicular = new PointD(-direction.Y, direction.X);
            
            // Arrow vertices
            PointD pointOnLine = tip.Add(direction.Multiply(aLength)); // tip + aLength * direction
            PointD arrowVertex1 = pointOnLine.Add(perpendicular.Multiply(aWidth)); // pointOnLine + aWidth * perpendicular
            PointD arrowVertex2 = pointOnLine.Substract(perpendicular.Multiply(aWidth)); // pointOnLine - aWidth * perpendicular

            context.MoveTo (tip);
            context.LineTo (arrowVertex1);
            context.LineTo (arrowVertex2);
            context.LineTo (tip);
            context.ClosePath ();
            context.FillPreserve ();
            context.Stroke ();
        }

        /// <summary>
        /// Gets the handles enumerator.
        /// If control is editable it shows remove trash icon,
        /// and handles to reconnect connection.
        /// </summary>
        /// <value>The handles enumerator.</value>
        public override IEnumerable <IHandle> HandlesEnumerator {
            get 
            { 
                if(IsEditable) 
                {
                    foreach (IHandle handle in base.HandlesEnumerator) 
                    {
                        yield return handle;
                    }

                    // HERZUM SPRINT 2.1: TLAB-140
                    bool ifScopeEdge = false;
                    bool scopeExitEdge = false;
                    if (m_experimentNodeConnection != null && m_experimentNodeConnection.Source != null && m_experimentNodeConnection.Source != null){
                        ExperimentDecisionNode decisionSource = (m_experimentNodeConnection.Source as ExperimentDecisionNode);
                        ScopeNode scopeSource = m_experimentNodeConnection.Source as ScopeNode;
                        ScopeNode scopeTarget = m_experimentNodeConnection.Target as ScopeNode;
                        ExitDecisionNode exitTarget = m_experimentNodeConnection.Target as ExitDecisionNode;
                        ifScopeEdge = decisionSource != null && (scopeTarget != null || exitTarget !=null);
                        scopeExitEdge = scopeSource != null && exitTarget != null;
                    }
                    if (!ifScopeEdge && !scopeExitEdge) 
                    // END HERZUM SPRINT 2.1: TLAB-140.1: TLAB-140
                        yield return m_removeHandle;
                }
            }
        }

        /// <summary>
        /// It refreshes connection display when dragging it. 
        /// 
        /// This property returns the DisplayBox inflated by enough pixels,
        /// so that when dragging connection it doesn't leave marks of attached remove handle.
        /// 
        /// Previously ugly marks were left on the canvas when dragging connection. 
        /// This property is responsible for clearing the context behind
        /// it must by basically sligly larger than current display box. Adjusting it, fixes display.
        /// </summary>
        /// <value>
        /// The invalidate display box.
        /// </value>
        public override RectangleD InvalidateDisplayBox 
        {
            get {
                RectangleD rect = DisplayBox;
                rect.Inflate (60.0, 60.0);
                return rect;
            }
        }

        public Experiment Owner
        {
            get { return m_experimentOwner; }
        }

        public ExperimentNodeConnection ExperimentNodeConnection
        {
            get { return m_experimentNodeConnection; }
        }

        private Experiment m_experimentOwner;
        private ExperimentNodeConnection m_experimentNodeConnection;
        private RemoveConnectionHandle m_removeHandle;
    }

    
    internal class ConnectionCompletedEventArgs : EventArgs
    {
        public ConnectionCompletedEventArgs(ExperimentNodeConnection nodeConnection) 
        {
            ExperimentNodeConnection = nodeConnection;
        }
        
        public ExperimentNodeConnection ExperimentNodeConnection
        {
            get;
            private set;
        }
    }
}


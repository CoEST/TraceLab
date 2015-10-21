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
using TraceLab.Core.Experiments;
using MonoHotDraw.Handles;
using System.Collections.Generic;
using MonoHotDraw.Util;

// HERZUM SPRINT 1.1 IF
using MonoHotDraw.Locators;
// END HERZUM SPRINT 1.1 IF

// HERZUM SPRINT 2 TLAB 131
using TraceLab.Core.Components;
// END HERZUM SPRINT 2 TLAB 131

namespace TraceLab.UI.GTK
{
    public class DecisionNodeControl: ComponentControl
    {

        // HERZUM SPRINT 1.1 IF
        private static Gdk.Pixbuf s_newScopeIcon;
        private PixButtonHandle m_addScopeInDecisionComponent;

        // HERZUM SPRINT SPRINT 2 TLAB 131
        private bool IsIfNode = false;
        // END HERZUM SPRINT 2 TLAB 131

        static DecisionNodeControl()
        {
            s_newScopeIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.addscope.png");
        }

        /// <summary>
        /// Gets the handles enumerator of all the icons:
        /// new connection, remove, and info
        /// </summary>
        /// <value>The handles enumerator.</value>
        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            {
                foreach (IHandle handle in base.HandlesEnumerator) 
                {
                    // HERZUM SPRINT 2 TLAB 131
                    // if (!(handle is NewConnectionHandle))
                    // yield return handle;
                    if (IsIfNode && !(handle is NewConnectionHandle))
                        yield return handle;
                    else if(!IsIfNode)
                        yield return handle;
                    // END HERZUM SPRINT 2 TLAB 131
                }


                // HERZUM SPRINT 2 TLAB 131
                if (IsIfNode)
                    // END HERZUM SPRINT 2 TLAB 131
                //always show magnifier glass button
                    yield return m_addScopeInDecisionComponent;

            }
        }

        private void AddScopeInDecisionComponent() 
        {
            // HERZUM SPRINT 2.0: TLAB-148
            // m_applicationContext.MainWindow.ExperimentCanvasPad.AddScopeInDecisionComponent(this);
            ExperimentCanvasPad experimentCanvasPad = ExperimentCanvasPadFactory.GetExperimentCanvasPad (m_applicationContext, this);
            experimentCanvasPad.AddScopeInDecisionComponent(this);
            // END HERZUM SPRINT 2.0: TLAB-148
        }
        // END HERZUM SPRINT 1.1 IF

        public DecisionNodeControl(ExperimentNode node, ApplicationContext applicationContext) 
            : base(node, applicationContext, s_waitForAnyAllHandleXLocation, s_waitForAnyAllHandleYLocation)
        {
            PaddingLeft = 30.0;
            PaddingTop = 7.0;
            PaddingRight = 40.0;
            PaddingBottom = 7.0;

            // HERZUM SPRINT 1.1 IF
            m_addScopeInDecisionComponent = new PixButtonHandle(this, new QuickActionLocator (35, 0.8, QuickActionPosition.Right),
                                                                s_newScopeIcon, AddScopeInDecisionComponent);
            // END HERZUM SPRINT 1.1 IF

            // HERZUM SPRINT 2 TLAB 131
            foreach (ExperimentNodeConnection edge in ExperimentNode.Owner.Edges)
                // HERZUM SPRINT 2.3 TLAB 140
                // if (edge.Target is ExitDecisionNode){
                if (edge.Source.Equals(this.ExperimentNode) && edge.Target is ExitDecisionNode){
                // END HERZUM SPRINT 2.3 TLAB 140
                    IsIfNode = true;
                    break;
                }
            // END HERZUM SPRINT 2 TLAB 131
        }

        protected override void DrawFrame (Cairo.Context context, double lineWidth, Cairo.Color lineColor, Cairo.Color fillColor)
        {
            RectangleD rect = DisplayBox;
            rect.OffsetDot5();
            CairoFigures.AngleFrame(context, rect, 15, 0, 15, 0);
            context.Color = fillColor;
            context.FillPreserve();
            context.Color = lineColor;
            context.LineWidth = lineWidth;
            context.Stroke();
        }

        /// <summary>
        /// Determines the default x location of Any/All button 
        /// </summary>
        private static double s_waitForAnyAllHandleXLocation = 25;
        
        /// <summary>
        /// Determines the default x location of Any/All button 
        /// </summary>
        private static double s_waitForAnyAllHandleYLocation = 14.9;
    }
}


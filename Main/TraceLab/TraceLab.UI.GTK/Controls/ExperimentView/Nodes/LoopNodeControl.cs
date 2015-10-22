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

// HERZUM SPRINT 1.1 CLASS

using System;
using TraceLab.Core.Experiments;

using MonoHotDraw.Handles;
using System.Collections.Generic;
using MonoHotDraw.Util;

namespace TraceLab.UI.GTK
{
    public class LoopNodeControl: ScopeNodeControl
    {
        public LoopNodeControl(ExperimentNode node, ApplicationContext applicationContext) 
        : base(node, applicationContext) {
        }

        protected override void DrawFrame (Cairo.Context context, double lineWidth, Cairo.Color lineColor, Cairo.Color fillColor)
        {        
            // base.DrawFrame (context, lineWidth, lineColor, fillColor);

            rect = DisplayBox;
            rect.OffsetDot5();

            // HERZUM SPRINT 1.2
            // CairoFigures.CurvedRectangle(context, rect, 30);
            CairoFigures.AngleFrame(context, rect, 0,0,0,0);
            // END HERZUM SPRINT 1.2

            Cairo.Color fillColorOrigin;
            fillColorOrigin = fillColor;

            lineWidth = 1;
            fillColor = new Cairo.Color (1.0, 1.0, 1.0, 1.0);

            context.Color = fillColor;  
            context.FillPreserve();
            context.Color = lineColor;
            context.LineWidth = lineWidth;

            double[] dash = {2, 0, 2};
            context.SetDash (dash, 0);

            context.Stroke();

            rect2 = DisplayBox;
            rect2.Width = DisplayBox.Width;
            rect2.Height = 30;
            rect2.OffsetDot5();
            CairoFigures.CurvedRectangle(context, rect2, 30);
            fillColor = fillColorOrigin;
            context.Color = fillColor;  
            context.FillPreserve();
            context.Color = lineColor;
            context.LineWidth = lineWidth;

            context.Stroke();

            // HERZUM SPRINT 2.1
            // m_applicationContext.MainWindow.ExperimentCanvasPad.LoopNodeControlCurrent = this;
            // END HERZUM SPRINT 2.1

            // HERZUM SPRINT 5.0: TLAB-235
            // DrawScope ();
            DrawScope ("Enter","Exit");
            // END HERZUM SPRINT 5.0: TLAB-235
        }

        // HERZUM SPRINT 1.2 IF
        protected override bool IsDecisionScope()
        {
            return false;
        }
        // END HERZUM SPRINT 1.2 IF

    }


}


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

namespace TraceLab.UI.GTK
{
    public class DecisionNodeControl: ComponentControl
    {
        public DecisionNodeControl(ExperimentNode node, ApplicationContext applicationContext) 
            : base(node, applicationContext, s_waitForAnyAllHandleXLocation, s_waitForAnyAllHandleYLocation)
        {
            PaddingLeft = 30.0;
            PaddingTop = 7.0;
            PaddingRight = 40.0;
            PaddingBottom = 7.0;
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


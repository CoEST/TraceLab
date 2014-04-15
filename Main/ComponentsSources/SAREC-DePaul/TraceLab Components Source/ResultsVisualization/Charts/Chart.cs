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
using System.Windows.Media;

namespace ResultsVisualization.Charts
{
    /// <summary>
    /// Abstract data structure for charts
    /// </summary>
    public abstract class Chart
    {
        public static Brush[] Colors = { Brushes.DodgerBlue, Brushes.Red, Brushes.Green,
                                           Brushes.Yellow, Brushes.MediumPurple,
                                           Brushes.Orange, Brushes.Gray, Brushes.Aqua, 
                                           Brushes.RoyalBlue, Brushes.SeaGreen, 
                                           Brushes.Sienna, Brushes.LightGray, 
                                           Brushes.Lime, Brushes.Coral, 
                                           Brushes.DarkOrchid, Brushes.LightBlue, 
                                           Brushes.YellowGreen, Brushes.LightPink, 
                                           Brushes.BurlyWood, Brushes.DarkSlateGray };

        #region Members

        protected string title;
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        protected string description;
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        protected ChartStyle chartStyle;

        protected double strokeThickness = 2.0d;
        public double StrokeThickness
        {
            get { return this.strokeThickness; }
            set { this.strokeThickness = value; }
        }

        #endregion

        abstract public void Draw(Canvas TextCanvas, Canvas ChartCanvas, Canvas LegendCanvas);
    }
}

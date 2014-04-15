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
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ResultsVisualization.Charts
{
    /// <summary>
    /// Different types of line pattern
    /// </summary>
    public enum LinePatternEnum
    {
        Solid = 1,
        Dash = 2,
        Dot = 3,
        DashDot = 4
    }

    public static class ChartsHelper
    {
        /// <summary>
        /// Creates an empty line.
        /// </summary>
        /// <param name="pColor">Color of the line.</param>
        /// <param name="pThickness">The line thickness.</param>
        /// <param name="pPattern">The line pattern.</param>
        /// <returns></returns>
        public static Line CreateLine(Brush pColor, double pThickness, LinePatternEnum pPattern)
        {
            Line line = new Line();
            line.Stroke = pColor;
            line.StrokeThickness = pThickness;

            switch (pPattern)
            {
                case LinePatternEnum.Dash:
                    line.StrokeDashArray = new DoubleCollection(new double[2] { 4, 2 });
                    break;

                case LinePatternEnum.Dot:
                    line.StrokeDashArray = new DoubleCollection(new double[2] { 1, 2 });
                    break;

                case LinePatternEnum.DashDot:
                    line.StrokeDashArray = new DoubleCollection(new double[4] { 4, 2, 1, 2 });
                    break;

                default:
                    break;
            }

            return line;
        }
    }
}

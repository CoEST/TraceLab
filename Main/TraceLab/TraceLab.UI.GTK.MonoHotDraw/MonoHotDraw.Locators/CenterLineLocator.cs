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
using Cairo;
using MonoHotDraw.Util;
using System.Collections.Generic;

namespace MonoHotDraw.Locators
{
    /// <summary>
    /// Locator finds center relative position on the given PolyLineFigure
    /// and returns location distant by given value from the line.
    /// </summary>
    public class CenterLineLocator: ILocator {
        
        public CenterLineLocator (double relativePosition, double distanceFromLine) {
            _relativePos = relativePosition;
            _distance = distanceFromLine;
        }
        
        public PointD Locate (IFigure owner) {
            if (owner != null) {
                PolyLineFigure figure = (PolyLineFigure) owner; 
                return GetPosition(figure.Points);
            }
            
            return new PointD ();
        }

        // This metod obtains the label position based on relativPos and distance.
        public PointD GetPosition(List<PointD> points) {

            double length = Geometry.PolyLineSize(points);
            
            if (length == 0) {
                return new PointD();
            }
            
            // first we convert the relativePos to an absolutePos
            double position = length * _relativePos;
            
            // second, we look for the point inside the PolyLine.
            for (int i=0; i<points.Count-1; i++) {
                double len = Geometry.LineSize(points[i], points[i+1]);
                // when position is less than the lenght of the current line segment
                // that's the right line segment (we use Math.Round to avoid floating 
                // point comparison problems)
                if (len != 0 && Math.Round(position, 2) <= Math.Round(len, 2)) {
                    double w = points[i+1].X - points[i].X;
                    double h = points[i+1].Y - points[i].Y;
                    
                    double x = position * w / len;
                    double y = position * h / len;
                    
                    // then we calculate the point inside the poly line.
                    PointD middle = new PointD(points[i].X + x, points[i].Y + y);

                    // we look for the normal point in order to locate the label.
                    PointD normal = new PointD(0.0, 0.0);
                    normal.X = -h / len;
                    normal.Y = w / len;
                    
                    // then the label is located.

                    double finalX = middle.X + normal.X * _distance;
                    double finalY = middle.Y + normal.Y * _distance;

                    return new PointD(finalX, finalY);
                }
                position -= len;
            }

            return new PointD();
        }

        private double _relativePos;
        private double _distance;
    }
}


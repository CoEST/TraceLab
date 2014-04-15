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
using Cairo;

namespace TraceLab.UI.GTK
{
    public static class PointDExtensions
    {
        public static double Length(this PointD point)
        {
            return (double)Math.Sqrt((double)point.Dot(point));
        }
        
        public static PointD Normalize(this PointD point)
        {
            double length = point.Length();
            return new PointD(point.X / length, point.Y / length);
        }
        
        // s * v2
        public static PointD Multiply(this PointD point, double f)
        {
            return new PointD(f * point.X, f * point.Y);
        }
        
        // v1 dot v2
        public static double Dot(this PointD v1, PointD v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y);
        }
        
        // v1 + v2
        public static PointD Add(this PointD v1, PointD v2)
        {
            return new PointD(v1.X + v2.X, v1.Y + v2.Y);
        }
        
        // v1 - v2
        public static PointD Substract(this PointD v1, PointD v2)
        {
            return new PointD(v1.X - v2.X, v1.Y - v2.Y);
        }
    }
}


// MonoHotDraw. Diagramming Framework
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//
// Copyright (C) 2006, 2007, 2008, 2009 MonoUML Team (http://www.monouml.org)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Cairo;

namespace MonoHotDraw.Util {

	public sealed class CairoFigures {
	
		private CairoFigures () {
		}

		public static void CurvedRectangle (Cairo.Context c, RectangleD rect, double radius) {
		
			if (rect.Width < (radius * 2.0) ) {
				radius = rect.Width/2.0;
			}

			if (rect.Height < (radius * 2.0) ) {
				radius = rect.Height/2.0;
			}
			
			c.MoveTo (rect.X, rect.Y+radius);
			c.LineTo (rect.X, rect.Y2-radius);
			c.CurveTo (rect.X, rect.Y2-radius, rect.X, rect.Y2, rect.X+radius, rect.Y2);
			c.LineTo (rect.X2-radius, rect.Y2);
			c.CurveTo (rect.X2-radius, rect.Y2, rect.X2, rect.Y2, rect.X2, rect.Y2-radius);
			c.LineTo (rect.X2, rect.Y+radius);
			c.CurveTo (rect.X2, rect.Y+radius, rect.X2, rect.Y, rect.X2-radius, rect.Y);
			c.LineTo (rect.X+radius, rect.Y);
			c.CurveTo (rect.X+radius, rect.Y, rect.X, rect.Y, rect.X, rect.Y+radius);
            c.ClosePath();
		}

        public static void AngleFrame(Cairo.Context c, Gdk.Rectangle gdkRect, double angleLeft, double angleTop, double angleRight, double angleBottom)
        {
            RectangleD rect = new RectangleD(gdkRect.X, gdkRect.Y, gdkRect.Width, gdkRect.Height);
            AngleFrame(c, rect, angleLeft, angleTop, angleRight, angleBottom);
        }

        public static void AngleFrame(Cairo.Context c, RectangleD rect, double angleLeft, double angleTop, double angleRight, double angleBottom)
        {
            //adjust position of frame accordingly to current position of rectangle
            double xAdj = rect.X;
            double yAdj = rect.Y;

            double middleX = rect.Width / 2;
            double middleY = rect.Height / 2;

            // Bottom-left corner
            PointD bottomLeftCorner = new PointD(angleLeft + xAdj, rect.Height - angleBottom + yAdj);
            
            PointD leftCenter = new PointD(0 + xAdj, middleY + yAdj);
            
            PointD topLeftCorner = new PointD(angleLeft + xAdj, angleTop + yAdj);
            
            // Top-middle
            PointD topMiddle = new PointD(middleX + xAdj, 0 + yAdj);
            
            // Top-right corner
            PointD topRightCorner = new PointD(rect.Width - angleRight + xAdj, angleTop + yAdj);
            
            // Middle, right side
            PointD middleRight = new PointD(rect.Width + xAdj, middleY + yAdj);
            
            // Bottom right corner
            PointD bottomRightCorner = new PointD(rect.Width - angleRight + xAdj, rect.Height - angleBottom + yAdj);
            
            // Bottom middle
            PointD bottomMiddle = new PointD(middleX + xAdj, rect.Height - angleBottom + yAdj);

            c.MoveTo(bottomLeftCorner);
            c.LineTo(leftCenter);
            c.LineTo(topLeftCorner);
            c.LineTo(topMiddle);
            c.LineTo(topRightCorner);
            c.LineTo(middleRight);
            c.LineTo(bottomRightCorner);
            c.LineTo(bottomMiddle);
            c.LineTo(bottomLeftCorner);
            c.ClosePath();
        }
	}
}

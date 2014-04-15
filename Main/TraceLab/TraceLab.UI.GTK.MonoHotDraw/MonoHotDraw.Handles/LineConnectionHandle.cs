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

using Cairo;
using MonoHotDraw.Figures;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {

	public class LineConnectionHandle: PolyLineHandle {
	
		public LineConnectionHandle (PolyLineFigure figure, ILocator loc, int index): base (figure, loc, index) {
		}

		public override void InvokeEnd (double x, double y, IDrawingView view) {
			PolyLineFigure figure = ((PolyLineFigure) Owner);
			// eliminates handles in the middle of a straight line
			if (figure.PointCount > 2 && Index != 0 && Index != (figure.PointCount - 1)) {
				PointD p1 = figure.PointAt (Index - 1);
				PointD p2 = figure.PointAt (Index + 1);
				if (Geometry.LineContainsPoint (p1.X, p1.Y, p2.X, p2.Y, x, y)) {
					figure.RemovePointAt (Index);
				}
			}
			base.InvokeEnd(x, y, view);
		}
	}
}

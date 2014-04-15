// MonoHotDraw. Diagramming Framework
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//	Mario Carrión <mario@monouml.org>
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
using System;
using MonoHotDraw.Figures;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {

	public class SouthHandle: ResizeHandle	{
	
		public SouthHandle (IFigure owner): base (owner, RelativeLocator.South)	{
		}
		
		public override Gdk.Cursor CreateCursor () {
			return CursorFactory.GetCursorFromType (Gdk.CursorType.BottomSide);
		}

		public override void InvokeStep (double x, double y, IDrawingView view) {
			RectangleD r = Owner.DisplayBox;

			PointD new_location = new PointD (r.X, r.Y);
			PointD new_corner   = new PointD (r.X + r.Width, Math.Max (r.Y, y));

			Owner.DisplayBox = new RectangleD (new_location, new_corner);
		}
	}
}

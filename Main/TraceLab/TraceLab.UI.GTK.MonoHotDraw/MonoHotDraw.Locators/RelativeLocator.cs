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
using MonoHotDraw.Util;

namespace MonoHotDraw.Locators {

	public class RelativeLocator: ILocator {

		public RelativeLocator (): this (0.0, 0.0) {
		}

		public RelativeLocator (double x, double y) {
			_relativeX = x;
			_relativeY = y;
		}

		public PointD Locate (IFigure owner) {
			if (owner != null) {
				RectangleD r = owner.DisplayBox;
				return new PointD (r.X + r.Width * _relativeX, r.Y + r.Height * _relativeY);
			}
			
			return new PointD (0, 0);
		}

		public static ILocator East {
			get { return new RelativeLocator (1.0, 0.5); }
		}

		public static ILocator North {
			get { return new RelativeLocator (0.5, 0.0); }
		}

		public static ILocator West {
			get { return new RelativeLocator (0.0, 0.5); }
		}

		public static ILocator NorthEast {
			get { return new RelativeLocator (1.0, 0.0); }
		}

		public static ILocator NorthWest {
			get { return new RelativeLocator (0.0, 0.0); }
		}

		public static ILocator South {
			get { return new RelativeLocator (0.5, 1.0); }
		}

		public static ILocator SouthEast {
			get { return new RelativeLocator (1.0, 1.0); }
		}

		public static ILocator SouthWest {
			get { return new RelativeLocator (0.0, 1.0); }
		}

		public static ILocator Center {
			get { return new RelativeLocator (0.5, 0.5); }
		}

		private double _relativeX;
		private double _relativeY;
	}
}

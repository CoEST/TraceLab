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

namespace MonoHotDraw.Util {

	[Serializable]
	public struct RectangleD : IEquatable<RectangleD> {
	
		public RectangleD (double x, double y): this (x, y, 0.0, 0.0) {
		}
		
		public RectangleD (PointD point): this (point.X, point.Y, 0.0, 0.0)	{
		}

		public RectangleD (RectangleD rect): this (rect.X, rect.Y, rect.Width, rect.Height)	{
		}

		public RectangleD (double x, double y, double width, double height)	{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public RectangleD (PointD location, PointD corner) {
			X = location.X;
			Y = location.Y;
			Width = corner.X - X;
			Height = corner.Y - Y;
		}

		public double X;
		public double Y;
		public double Width;
		public double Height;

		public double X2 {
			get { return X + Width; }
		}

		public double Y2  {
			get { return Y + Height; }
		}

		public double Left {
			get { return X; }
		}

		public double Right {
			get { return X + Width; }
		}

		public double Top {
			get { return Y; }
		}

		public double Bottom {
			get { return Y + Height; }
		}
		
		public PointD TopLeft {
			get { return new PointD (Left, Top); }
		}
		
		public PointD TopRight {
			get { return new PointD (Right, Top); }
		}
		
		public PointD BottomLeft {
			get { return new PointD (Left, Bottom); }
		}
		
		public PointD BottomRight {
			get { return new PointD (Right, Bottom); }
		}
		
		public PointD Center {
			get { return new PointD (X + (Width / 2.0), Y + (Height / 2.0)); }
		}
		
		public static RectangleD Empty {
			get { return new RectangleD (0.0, 0.0, 0.0, 0.0); }
		}
		
		public bool Contains (double x, double y) {
			return ((x >= Left) && (x < Right) && (y >= Top) && (y < Bottom));
		}
		
		public bool Contains (RectangleD r) {
			return ( Contains (r.X, r.Y) && Contains (r.X2, r.Y2) );
		}

		public void OffsetDot5 () {
			X = Math.Truncate (X) + 0.5;
			Y = Math.Truncate (Y) + 0.5;
			Width = Math.Truncate (Width);
			Height = Math.Truncate (Height);
		}

		public void Normalize () {
			if (Width > 0 && Height > 0) {
				return;
			}

			if (Width < 0 ) {
				X = X2;
				Width *= -1;
			}

			if (Height < 0 ) {
				Y = Y2;
				Height *= -1;
			}
		}

		public void Inflate (double w, double h) {
			X -= w;
			Y -= h;
			Width += w * 2.0;
			Height += h * 2.0;
		}

        public void Inflate (double left, double top, double right, double bottom) {
            X -= left;
            Y -= top;
            Width += left + right;
            Height += top + bottom;
        }
		
		public void Add (double newx, double newy) {
			double x1 = Math.Min (X, newx);
			double x2 = Math.Max (X + Width, newx);
			double y1 = Math.Min (Y, newy);
			double y2 = Math.Max (Y + Height, newy);

			X = x1;
			Y = y1;
			Width = x2 - x1;
			Height = y2 - y1;
		}

		public void Add (PointD point) {
			Add (point.X, point.Y);
		}

		public void Add (RectangleD r) {
			Add (r.X, r.Y);
			Add (r.X2, r.Y2);
		}

		public bool Equals (RectangleD rectangle) {
			return rectangle.X == this.X && rectangle.Y == this.Y 
				&& rectangle.Width == this.Width && rectangle.Height == this.Height;
		}
		
		public override bool Equals (object obj) {
			if (obj is RectangleD) {
				return Equals ((RectangleD) obj);
			}
			else {
				return false;
			}
		}
		
		public override int GetHashCode () {
			//TODO: This seems to be too tricky.
			return (int) (X + Y + Width + Height);
		}

		public override string ToString () {
			return String.Format ("x:{0} y:{1} w:{2} h:{3}", X, Y, Width, Height);
		}

		public static bool operator == (RectangleD rectangle, RectangleD other) {
			return rectangle.Equals (other); 
		}

		public static bool operator != (RectangleD rectangle, RectangleD other) {
			return !rectangle.Equals (other); 
		}
	}
}


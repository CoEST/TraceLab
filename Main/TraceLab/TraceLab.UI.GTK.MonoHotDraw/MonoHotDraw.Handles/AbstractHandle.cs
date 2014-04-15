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
using System;
using System.Runtime.Serialization;
using MonoHotDraw.Figures;
using MonoHotDraw.Commands;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {

	public abstract class AbstractHandle : IHandle {
	
		public static readonly double Size = 8.0;
	
		protected AbstractHandle (IFigure owner) {
			Owner     = owner;
			LineWidth = 1.0;
			FillColor = new Cairo.Color (0.2, 1.0, 0.2, 0.8);
			LineColor = new Cairo.Color (0.0, 0.0, 0.0, 1.0);
		}
		
		public virtual Gdk.Cursor CreateCursor () {
			return null;
		}
		
		public virtual double Width {
			get { return Size; }
			set {}
		}
		
		public virtual double Height {
			get { return Size; }
			set {}
		}

		public virtual IFigure Owner { get; set; }
		
		public virtual IUndoActivity UndoActivity { get; set; }
			
		public virtual double LineWidth {
			get { return _lineWidth; }
			set {
				if (value >= 0) {
					_lineWidth = value;
				}
			}
		}
		
		public Color FillColor { get; set; }	
		
		public Color LineColor { get; set; }

		public bool ContainsPoint (double x, double y) {
			RectangleD displayBox = new RectangleD(Locate());
			displayBox.Inflate(Width/2, Height/2);
			return displayBox.Contains(x, y);
		}

		public virtual void Draw (Context context, IDrawingView view) {
			RectangleD rect = ViewDisplayBox(view);
			context.Save();
			context.LineWidth = LineWidth;
			context.Rectangle (GdkCairoHelper.CairoRectangle (rect));
			context.Color = FillColor;
			context.FillPreserve ();
			context.Color = LineColor;
			context.Stroke ();
			context.Restore();
		}

		public virtual void InvokeStart (double x, double y, IDrawingView view) {
			CreateUndoActivity(view);
		}

		public virtual void InvokeStep (double x, double y, IDrawingView view) {
		}

		public virtual void InvokeEnd (double x, double y, IDrawingView view) {
			UpdateUndoActivity();
		}
		
		public abstract PointD Locate ();
		
		protected virtual void CreateUndoActivity(IDrawingView view) {
		}
		
		protected virtual void UpdateUndoActivity() {
		}
		
		protected virtual RectangleD ViewDisplayBox(IDrawingView view) {
			if (view == null)
				throw new ArgumentNullException("view");
			
			PointD center = Locate();
			center = view.DrawingToView(center.X, center.Y);
			RectangleD displayBox = new RectangleD(center);
			displayBox.Inflate(Width/2, Height/2);
			displayBox.OffsetDot5();
			
			return displayBox;
		}

		private double  _lineWidth;
	}
}

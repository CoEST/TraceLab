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

using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace MonoHotDraw {
	
	[Serializable]
	public class StandardDrawing : CompositeFigure, IDrawing {
	
		public event EventHandler <DrawingEventArgs> DrawingInvalidated;
		public event EventHandler <DrawingEventArgs> SizeAllocated;

		public StandardDrawing (): base () {
		}

		public void Draw (Context context, FigureCollection figures)	{
			foreach (IFigure fig in figures) {
				fig.Draw (context);
			}
		}
		
		public override RectangleD DisplayBox {
			get { return _displayBox; }
			set { _displayBox = value; }
		}
		
		public override void Add (IFigure figure)
		{
			base.Add (figure);
			figure.FigureChanged += FigureChangedHandler;
			RecalculateDisplayBox ();
		}
		
		public override void Remove (IFigure figure)
		{
			base.Remove (figure);
			figure.FigureChanged -= FigureChangedHandler;
			RecalculateDisplayBox ();
		}
 
		protected override void FigureInvalidatedHandler (object sender, FigureEventArgs args) {
			OnDrawingInvalidated (new DrawingEventArgs (this, args.Rectangle));
		}
		
		protected virtual void OnDrawingInvalidated (DrawingEventArgs args) {
			if (DrawingInvalidated != null) {
				DrawingInvalidated (this, args);
			}
		}
		
		protected virtual void OnSizeAllocated ()
		{
			if (SizeAllocated != null) {
				SizeAllocated (this, new DrawingEventArgs (this, DisplayBox) );
			}
		}
		
		public void RecalculateDisplayBox ()
		{
			_displayBox = new RectangleD (0.0, 0.0);
			bool first_flag = true;
			foreach (IFigure figure in FiguresEnumerator) {
				if (first_flag) {
					_displayBox = figure.DisplayBox;
					first_flag = false;
				} 
				else {
					_displayBox.Add (figure.DisplayBox);
				}
			}
			
			OnSizeAllocated ();
		}
		
		private void FigureChangedHandler (object sender, FigureEventArgs args) {
			if (_displayBox.Contains (args.Rectangle)) {
				return;
			}
			_displayBox.Add (args.Rectangle);
			//OnSizeAllocated ();
		}
		
		private RectangleD _displayBox;
	}
}

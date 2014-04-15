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
using System.Collections.Generic;
using MonoHotDraw.Handles;
using MonoHotDraw.Tools;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {

	[Serializable]
	public abstract class CompositeFigure : AttributeFigure {

		protected CompositeFigure () {
			Figures = new FigureCollection ();
		}

		public override bool ContainsPoint (double x, double y) {
			foreach (IFigure figure in FiguresEnumerator) {
				if (figure.ContainsPoint (x, y) ) {
					return true;
				}
			}
			return false;
		}

		// TODO replace with linq
		public IEnumerable <IFigure> FiguresEnumeratorReverse {
			get {
				for (int i=1; i <= Figures.Count; i++) {
					yield return Figures [Figures.Count - i];
				}
			}
		}

		public override IEnumerable <IFigure> FiguresEnumerator {
			get {
				foreach (IFigure fig in Figures) {
					yield return fig;
				}
			}
		}
		
		public override RectangleD BasicDisplayBox {
			get { 
				RectangleD r = new RectangleD (0.0, 0.0);
				bool first_flag = true;
				foreach (IFigure figure in FiguresEnumerator) {
					if (first_flag) {
						r = figure.DisplayBox;
						first_flag = false;
					} 
					else {
						r.Add (figure.DisplayBox);
					}
				}
			
				return r;
			}
			set {
				RectangleD r = DisplayBox;
				double dx = value.X - r.X;
				double dy = value.Y - r.Y;
				foreach (IFigure figure in FiguresEnumerator) {
					AbstractFigure af = figure as AbstractFigure;
					af.BasicMoveBy (dx, dy);
				}
			}
		}
		
		public override IEnumerable <IHandle> HandlesEnumerator {
			get {
				foreach (IFigure figure in FiguresEnumerator) {
					foreach (IHandle handle in figure.HandlesEnumerator) {
						yield return handle;
					}
				}
			}
		}

		public virtual void Add (IFigure figure) {
			if (Figures.Contains (figure)) {
				return;
			}

			Figures.Add (figure);
			figure.FigureInvalidated += FigureInvalidatedHandler;
			figure.Invalidate ();
		}

		public virtual void Remove (IFigure figure) {
			if (!Figures.Contains (figure)) {
				return;
			}

			Figures.Remove (figure);
			figure.FigureInvalidated -= FigureInvalidatedHandler;
			figure.Invalidate ();
		}

		public override bool Includes (IFigure figure) {
			if (base.Includes (figure)) {
				return true;
			}

			foreach (IFigure fig in FiguresEnumerator) {
				if (fig.Includes (figure)) {
					return true;
				}
			}
			return false;
		}

		public override void BasicDraw (Context context)	{
			foreach (IFigure fig in FiguresEnumerator) {
				fig.Draw (context);
			}
		}
			
		public override void BasicDrawSelected (Context context)	{
			foreach (IFigure fig in FiguresEnumerator) {
				fig.DrawSelected (context);
			}
		}

		public IFigure FindFigure (double x, double y) {
			foreach (IFigure figure in FiguresEnumeratorReverse) {
				if (figure.ContainsPoint (x, y)) {
					return figure;
				}
			}
			return null;
		}
		
		public override ITool CreateFigureTool (IDrawingEditor editor, ITool dt) {
			return new CompositeFigureTool (editor, this, dt);
		}
		
		public void SendToBack (IFigure figure) {
			if (Includes (figure)) {
				Figures.Remove (figure);
				Figures.Insert (0, figure);
				figure.Invalidate ();
			}
		}
		
		public void BringToFront (IFigure figure) {
			if (Includes (figure)) {
				Figures.Remove (figure);
				Figures.Add (figure);
				figure.Invalidate ();
			}
		}

		protected virtual void FigureInvalidatedHandler (object sender, FigureEventArgs args) {
			OnFigureInvalidated (new FigureEventArgs (this, args.Rectangle));
		}
		
		protected FigureCollection Figures { get; set; }
	}
}

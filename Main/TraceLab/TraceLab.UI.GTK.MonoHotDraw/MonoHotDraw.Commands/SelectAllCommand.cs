// MonoHotDraw. Diagramming Framework
//
// Authors:
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

using System;
using MonoHotDraw.Figures;

namespace MonoHotDraw.Commands {	
	
	public class SelectAllCommand : AbstractCommand {

		public SelectAllCommand (string name, IDrawingEditor editor) : base (name, editor) {
		}
		
		public override void Execute () {
			base.Execute ();

			UndoActivity = CreateUndoActivity ();
			UndoActivity.AffectedFigures = new FigureCollection (DrawingView.SelectionEnumerator);
			foreach (IFigure figure in DrawingView.Drawing.FiguresEnumerator) {
				DrawingView.AddToSelection (figure);
			}
		}

		protected override IUndoActivity CreateUndoActivity () {
			return new SelectAllUndoActivity (DrawingView);
		}

		class SelectAllUndoActivity : AbstractUndoActivity {
			public SelectAllUndoActivity (IDrawingView drawingView) : base (drawingView) {
				Undoable = true;
				Redoable = true;
			}

			public override bool Undo () {
				if (base.Undo () == false)
					return false;

				DrawingView.ClearSelection ();
				DrawingView.AddToSelection (AffectedFigures.ToFigures());

				return true;
			}

			public override bool Redo () {
				// do not call execute directly as the selection might has changed
				if (Redoable) {
					foreach (IFigure figure in DrawingView.Drawing.FiguresEnumerator) {
						DrawingView.AddToSelection (figure);
					}
					return true;
				}

				return false;
			}
		}

	}
}

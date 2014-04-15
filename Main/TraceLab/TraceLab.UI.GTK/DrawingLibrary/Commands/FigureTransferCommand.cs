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
using System.Collections.Generic;
using MonoHotDraw.Figures;

namespace MonoHotDraw.Commands {
	public abstract class FigureTransferCommand : AbstractCommand {

		protected FigureTransferCommand (string name, IDrawingEditor editor)  : base (name, editor) {
		}		
		
		protected void CopyFigures (IEnumerable<IFigure> figures) {
			Clipboard.GetInstance ().Contents = figures;
		}

		public void DeleteFigures (IEnumerable<IFigure> figures) {
			foreach (IFigure figure in figures) {
				DrawingView.Remove (figure);
			}
			DrawingView.ClearSelection ();
		}
		
		public FigureCollection InsertFigures (IEnumerable<IFigure> figures, double dx, double dy) {
			return DrawingView.InsertFigures (figures.ToFigures(), dx, dy, false);
		}
		
		// TODO: Refactor this. Maybe this could go to IDrawing and StandardDrawingView
		protected FigureCollection GetWithDependents (FigureCollection figures) {
			return figures;
		}
	}
}
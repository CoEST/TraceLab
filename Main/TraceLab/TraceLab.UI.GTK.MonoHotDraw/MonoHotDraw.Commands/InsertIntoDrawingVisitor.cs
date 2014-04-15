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
using MonoHotDraw.Handles;

namespace MonoHotDraw.Commands {
	public class InsertIntoDrawingVisitor : IFigureVisitor {
		
		public InsertIntoDrawingVisitor (IDrawing drawing) {
			Drawing       = drawing;
			_addedFigures = new FigureCollection ();
		}
		
		public IDrawing Drawing { get; set; }

		public FigureCollection GetAddedFigures () {
			return _addedFigures;
		}

		public void VisitFigure (IFigure hostFigure) {
			if (_addedFigures.Contains (hostFigure) == false 
				&& Drawing.Includes (hostFigure) == false) {
				Drawing.Add (hostFigure);
				_addedFigures.Add (hostFigure);
			}
		}

		public void VisitHandle (IHandle hostHandle) {
		}

		private FigureCollection _addedFigures;
	}
}
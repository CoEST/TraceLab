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

using Gdk;
using Gtk;
using MonoHotDraw.Figures;

namespace MonoHotDraw.Tools {
	// NOTE: Should this be inside CompositeFigure??
	public class CompositeFigureTool: FigureTool {
		public CompositeFigureTool (IDrawingEditor editor, IFigure fig, ITool dt): base (editor, fig, dt) {
		}

		public override ITool DefaultTool {
			get {
				if (DelegateTool != null) {
					return DelegateTool;
				}
				else {
					return base.DefaultTool;
				}
			}
			
			set { base.DefaultTool = value; }
		}

		public override void MouseDown (MouseEvent ev) {
			IFigure fig = ((CompositeFigure) Figure).FindFigure (ev.X, ev.Y);
			
			if (fig != null) {
				DelegateTool = fig.CreateFigureTool (Editor, DefaultTool);
			}
			else {
				DelegateTool = DefaultTool;
			}
			
			if (DelegateTool != null) {
				DelegateTool.MouseDown (ev);
			}
		}
		
		protected ITool DelegateTool {
			set { 
				if (_delegateTool != null) {
					_delegateTool.Deactivate ();
				}

				_delegateTool = value;
				if (_delegateTool != null) {
					_delegateTool.Activate ();
				}
			}
			get { return _delegateTool; }
		}

		private ITool _delegateTool;
	}
}

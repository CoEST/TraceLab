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
using System;
using MonoHotDraw.Figures;
using MonoHotDraw.Commands;

namespace MonoHotDraw.Tools {

	public class FigureTool: AbstractTool {
	
		public FigureTool (IDrawingEditor editor, IFigure fig, ITool dt): base (editor) {
			DefaultTool = dt;
			Figure = fig;
		}
		
		public virtual ITool DefaultTool { get; set; }		
		public IFigure Figure { get; set; }
		
		public override void MouseDown (MouseEvent ev) {
			if (DefaultTool != null) {
				DefaultTool.MouseDown (ev);
			}
		}

		public override void MouseUp (MouseEvent ev) {
			if (DefaultTool != null) {
				DefaultTool.MouseUp (ev);
			}
		}

		public override void MouseMove (MouseEvent ev) {
			if (DefaultTool != null) {
				DefaultTool.MouseMove (ev);
			}
		}

		public override void MouseDrag (MouseEvent ev) {
			if (DefaultTool != null) {
				DefaultTool.MouseDrag (ev);
			}
		}

		public override void KeyDown (KeyEvent ev) {
			if (DefaultTool != null) {
				DefaultTool.KeyDown (ev);
			}
		}

		public override void KeyUp (KeyEvent ev) {
			if (DefaultTool != null) {
				DefaultTool.KeyUp (ev);
			}
		}
		
		public override void Activate () {
			base.Activate ();
			if (DefaultTool != null) {
				DefaultTool.Activate();
			}
		}
		
		public override void Deactivate () {
			base.Deactivate ();
			if (DefaultTool != null) {
				DefaultTool.Deactivate();
			}
		}
	}
}

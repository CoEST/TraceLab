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
using Gdk;
using Gtk;
using System;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;
using MonoHotDraw.Commands;

namespace MonoHotDraw.Tools {

	public class TextTool: FigureTool	{

		public TextTool (IDrawingEditor editor, SimpleTextFigure fig, ITool dt) 
			: base (editor, fig, dt) {
		}
		
	    public override void Activate () {
			_showingWidget = false;
			base.Activate ();
		}

		public override void MouseDrag (MouseEvent ev) {
			if (!_showingWidget) {
				DefaultTool.MouseDrag (ev);
			}
		}
		
		public class TextToolUndoActivity: AbstractUndoActivity {
			public TextToolUndoActivity(IDrawingView view): base(view) {
				Undoable = true;
				Redoable = true;
			}
			
			public override bool Undo () {
				if (!base.Undo ()) {
					return false;
				}
				AffectedFigure.Text = OldText;
				return true;
			}
			
			public override bool Redo () {
				if (!base.Undo ()) {
					return false;
				}
				AffectedFigure.Text = NewText;
				return true;
			}
			
			public string OldText { get; set; }
			public string NewText { get; set; }
			public SimpleTextFigure AffectedFigure { get; set; }
		}
		
		protected void CreateUndoActivity() {
			TextToolUndoActivity activity = new TextToolUndoActivity(Editor.View);
			activity.AffectedFigure = Figure as SimpleTextFigure;
			activity.OldText = activity.AffectedFigure.Text;
			UndoActivity = activity;
		}

		protected void UpdateUndoActivity() {
			TextToolUndoActivity activity = UndoActivity as TextToolUndoActivity;
			if (activity == null)
				return;
			activity.NewText = activity.AffectedFigure.Text;
			if (activity.NewText == activity.OldText) {
				UndoActivity = null;
			}
		}
		
		protected bool _showingWidget = false;
	}
}

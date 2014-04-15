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
using MonoHotDraw.Figures;
using MonoHotDraw.Util;
using MonoHotDraw.Commands;

namespace MonoHotDraw.Tools {

	public class CreationTool: AbstractTool {
	
		public CreationTool (IDrawingEditor editor, IFigure ptype): base (editor) {
			Prototype = ptype;
		}
		
		public override void Activate () {
			base.Activate ();
			Gtk.Widget widget = Editor.View as Gtk.Widget;
			widget.GdkWindow.Cursor = CursorFactory.GetCursorFromType (CursorType.Crosshair);
		}
		
		public override void Deactivate () {
			base.Deactivate ();
			Gtk.Widget widget = Editor.View as Gtk.Widget;
			widget.GdkWindow.Cursor = null;
		}
		
		public override void MouseDown (MouseEvent ev)	{
			IDrawingView view = ev.View;
			base.MouseDown (ev);
			view.Drawing.Add (Prototype);
			Prototype.MoveTo (ev.X, ev.Y);
			view.ClearSelection ();
			view.AddToSelection (Prototype);
			CreateUndoActivity();
		}
		
		public override void MouseUp (MouseEvent ev) {
			Editor.Tool = new SelectionTool (Editor);
			PushUndoActivity();
		}
		
		public class CreationToolUndoActivity: AbstractUndoActivity {
			public CreationToolUndoActivity(IDrawingView view, IFigure prototype): base(view) {
				Undoable = true;
				Redoable = true;
				Prototype = prototype;
			}
			
			public override bool Undo () {
				if (!base.Undo()  )
					return false;
				DrawingView.Drawing.Remove(Prototype);
				DrawingView.RemoveFromSelection(Prototype);
				return true;
			}
			
			public override bool Redo () {
				if (!base.Redo() )
					return false;
				DrawingView.Drawing.Add(Prototype);
				return true;
			}
			
			public IFigure Prototype { set; get; }
		}
		
		protected void CreateUndoActivity(){
			UndoActivity = new CreationToolUndoActivity(Editor.View, Prototype);
		}
		
		protected IFigure Prototype { get; set; }
	}
}

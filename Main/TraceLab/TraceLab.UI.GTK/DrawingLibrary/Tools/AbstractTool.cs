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
using System;
using MonoHotDraw.Commands;

namespace MonoHotDraw.Tools {

	public abstract class AbstractTool: ITool {
	
		protected AbstractTool (IDrawingEditor editor) {
			Editor = editor;
			Undoable = true;
		}

		public IDrawingEditor Editor { get; set;}
		public IDrawingView View { get; set; }
		public bool Activated { get; protected set; }
		public virtual IUndoActivity UndoActivity { get; set; }
		public bool Undoable { get; set; }
		
		public virtual void Activate () {
			Activated = true;
		}
		
		public virtual void Deactivate () {
			Activated = false;
		}
			
		public virtual void KeyDown (KeyEvent ev) {
		}
			
		public virtual void KeyUp (KeyEvent ev) {
		}

		public virtual void MouseDown (MouseEvent ev) {
			SetAnchorCoords (ev.X, ev.Y);
			View = ev.View;
		}

		public virtual void MouseDrag (MouseEvent ev) {
		}
			
		public virtual void MouseMove (MouseEvent ev){
		}
			
		public virtual void MouseUp (MouseEvent ev) {
		}
		
		protected void PushUndoActivity() {
			if (!Undoable) {
				return;
			}
			
			IUndoActivity activity = UndoActivity;
			if (activity != null && activity.Undoable && Editor.UndoManager != null) {
				Editor.UndoManager.PushUndo(activity);
				Editor.UndoManager.ClearRedos();
			}
		}

		protected double AnchorX { get; set; }
		protected double AnchorY { get; set; }

		protected void SetAnchorCoords (double x, double y) {
			AnchorX = x;
			AnchorY = y;
		}
	}
}

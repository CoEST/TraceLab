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
using MonoHotDraw.Commands;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {
	
	public class UndoableHandle: IHandle {
		
		public UndoableHandle(IHandle wrappedHandle)	{
			WrappedHandle = wrappedHandle;
		}
		
		public bool ContainsPoint (double x, double y) {
			return WrappedHandle.ContainsPoint(x, y);
		}
		
		public double Width {
			get { return WrappedHandle.Width; }
		}
		
		public double Height {
			get { return WrappedHandle.Width; }
		}
		
		public PointD Locate () {
			return WrappedHandle.Locate();
		}
		
        public void InvokeStart (MouseEvent ev) {
			WrappedHandle.InvokeStart(ev);
		}
		
		public void InvokeStep(MouseEvent ev) {
			WrappedHandle.InvokeStep(ev);
		}
		
        public void InvokeEnd (MouseEvent ev) {
			WrappedHandle.InvokeEnd(ev);
			
			IUndoActivity activity = WrappedHandle.UndoActivity;
			
			if (activity != null && activity.Undoable && ev.View.Editor.UndoManager != null) {
				ev.View.Editor.UndoManager.PushUndo(activity);
				ev.View.Editor.UndoManager.ClearRedos();
			}
		}
			
		public void Draw (Context context, IDrawingView view) {
			WrappedHandle.Draw(context, view);
		}
		
		public Gdk.Cursor CreateCursor () {
			return WrappedHandle.CreateCursor();
		}
		
		public IFigure Owner {
			get { return WrappedHandle.Owner; }
		}
		
		public IUndoActivity UndoActivity {
			get { return WrappedHandle.UndoActivity; }
			set { WrappedHandle.UndoActivity = value; }
		}
		
		public IHandle WrappedHandle { get; private set; }
	}
}

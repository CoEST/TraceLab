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
using MonoHotDraw.Figures;
using MonoHotDraw.Commands;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {
	
	public abstract class ResizeHandle: LocatorHandle {
		
		public ResizeHandle(IFigure owner, ILocator locator): base (owner, locator) {
		}
		
		public class ResizeHandleUndoActivity: AbstractUndoActivity {
			public ResizeHandleUndoActivity(IDrawingView view, IFigure owner): base (view) {
				Undoable = true;
				Redoable = true;
				Owner = owner;
				OldDisplayBox = Owner.DisplayBox;
				NewDisplayBox = Owner.DisplayBox;
			}
			
			public override bool Undo () {
				if (!base.Undo()  )
					return false;
				Owner.DisplayBox = OldDisplayBox;
				return true;
			}
			
			public override bool Redo () {
				if (!base.Redo() )
					return false;
				Owner.DisplayBox = NewDisplayBox;
				return true;
			}
			
			public IFigure Owner { get; private set; }
			public RectangleD OldDisplayBox { get; set; }
			public RectangleD NewDisplayBox { get; set; }
		}
		
		protected override void CreateUndoActivity(IDrawingView view) {
			UndoActivity = new ResizeHandleUndoActivity(view, Owner);
		}
		
		protected override void UpdateUndoActivity() {
			ResizeHandleUndoActivity activity = UndoActivity as ResizeHandleUndoActivity;
			activity.NewDisplayBox = Owner.DisplayBox;
		}
	}
}

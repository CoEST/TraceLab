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
using System;
using MonoHotDraw.Figures;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;
using MonoHotDraw.Commands;

namespace MonoHotDraw.Handles {

	public class PolyLineHandle: LocatorHandle {
	
		public PolyLineHandle (PolyLineFigure figure, ILocator loc, int index): base (figure, loc) {
			Index = index;
		}
		
		public override Gdk.Cursor CreateCursor () {
			return CursorFactory.GetCursorFromType (Gdk.CursorType.Hand2);
		}
		
		public override void InvokeStep (double x, double y, IDrawingView view) {
			((PolyLineFigure) Owner).SetPointAt (Index, x, y);
		}

		public override void Draw (Context context, IDrawingView view) {
			RectangleD rect = ViewDisplayBox(view);
			
			double middle = rect.Width / 2.0;

			context.LineWidth = LineWidth;
			context.Save ();
			context.Translate (rect.X + middle, rect.Y + middle);
			context.Arc (0.0, 0.0, middle, 0.0, 2.0 * Math.PI);
			context.Restore ();
			context.Color = new Cairo.Color (0.2, 0.2, 1.0, 0.5);
			context.FillPreserve ();
			context.Color = new Cairo.Color (0.0, 0.0, 0.0, 1.0);
			context.Stroke ();
		}
		
		public class PolyLineHandleUndoActivity: AbstractUndoActivity {
			public PolyLineHandleUndoActivity(IDrawingView view): base (view) {
				Undoable = true;
				Redoable = true;
			}
			
			public override bool Undo () {
				if (!base.Undo()  )
					return false;
				Owner.SetPointAt(Index, OldPoint.X, OldPoint.Y);
				return true;
			}
			
			public override bool Redo () {
				if (!base.Redo() )
					return false;
				Owner.SetPointAt(Index, NewPoint.X, NewPoint.Y);
				return true;
			}
			
			public int Index { get; set; }
			public PointD OldPoint { get; set; }
			public PointD NewPoint { get; set; }
			public PolyLineFigure Owner { get; set; }
		}
		
		protected override void CreateUndoActivity(IDrawingView view) {
			PolyLineHandleUndoActivity activity = new PolyLineHandleUndoActivity(view);
			activity.Owner = Owner as PolyLineFigure;
			activity.Index = Index;
			activity.OldPoint = activity.Owner.PointAt(Index);
			UndoActivity = activity;
		}
		
		protected override void UpdateUndoActivity () {
			PolyLineHandleUndoActivity activity = UndoActivity as PolyLineHandleUndoActivity;
			activity.NewPoint = activity.Owner.PointAt(Index);
		}
		
		public int Index { get; protected set; }
	}
}

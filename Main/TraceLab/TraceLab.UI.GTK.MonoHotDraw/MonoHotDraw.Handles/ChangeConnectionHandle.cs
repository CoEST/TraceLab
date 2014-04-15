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
using MonoHotDraw.Connectors;
using MonoHotDraw.Commands;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {

	public abstract class ChangeConnectionHandle: AbstractHandle {
	
		protected ChangeConnectionHandle (IConnectionFigure owner): base (owner) {
			Connection = owner;
			TargetFigure = null;
		}
		
		public override Gdk.Cursor CreateCursor () {
			return CursorFactory.GetCursorFromType (Gdk.CursorType.Hand2);
		}

		public override void InvokeStart (double x, double y, IDrawingView view) {
			_oldTarget = Target;
			CreateUndoActivity(view);
			Disconnect ();
		}

		public override void InvokeStep (double x, double y, IDrawingView view) {
			TargetFigure = FindConnectableFigure (x, y, view.Drawing);
			_newTarget = FindConnectionTarget (x, y, view.Drawing);
			Point = _newTarget != null ? FindPoint(_newTarget) : new PointD(x, y);
			Connection.UpdateConnection ();
		}

		public override void InvokeEnd (double x, double y, IDrawingView view) {
			_newTarget = _newTarget ?? _oldTarget;
			UpdateUndoActivity();
			Connect (_newTarget);
			Connection.UpdateConnection ();
		}

		public override void Draw (Context context, IDrawingView view) {
			RectangleD rect = ViewDisplayBox(view);
			
			context.LineWidth = LineWidth;

			context.MoveTo (rect.Center.X, rect.Top);
			context.LineTo (rect.Right, rect.Center.Y);
			context.LineTo (rect.Center.X, rect.Bottom);
			context.LineTo (rect.Left, rect.Center.Y);
			context.LineTo (rect.Center.X, rect.Top);

			context.Color = new Cairo.Color (1.0, 0.0, 0.0, 0.8);
			context.FillPreserve ();
			context.Color = new Cairo.Color (0.0, 0.0, 0.0, 1.0);
			context.Stroke ();
		}
		
		public class ChangeConnectionHandleUndoActivity: AbstractUndoActivity {
			public ChangeConnectionHandleUndoActivity(IDrawingView view): base (view) {
				Undoable = true;
				Redoable = true;
			}
			
			public override bool Undo () {
				if (!base.Undo()  )
					return false;
				Owner.Disconnect();
				Owner.Connect (OldConnector);
				return true;
			}
			
			public override bool Redo () {
				if (!base.Redo() )
					return false;
				Owner.Disconnect ();
				Owner.Connect (NewConnector);
				return true;
			}
			
			public ChangeConnectionHandle Owner { get; set; }
			public IConnector OldConnector { get; set; }
			public IConnector NewConnector { get; set; }
		}
		
		protected override void CreateUndoActivity (IDrawingView view) {
			ChangeConnectionHandleUndoActivity activity = new ChangeConnectionHandleUndoActivity(view);
			activity.Owner = this;
			activity.OldConnector = _oldTarget;
			UndoActivity = activity;
		}
		
		protected override void UpdateUndoActivity () {
			ChangeConnectionHandleUndoActivity activity = UndoActivity as ChangeConnectionHandleUndoActivity;
			if (activity == null) {
				return;
			}
			if (activity.OldConnector.Owner == _newTarget.Owner) {
				UndoActivity = null;
			}
			else {
				activity.NewConnector = _newTarget;
			}
		}

		protected abstract PointD Point { set; }

		protected abstract IConnector Target { get; }

		protected abstract void Connect (IConnector connector);

		protected abstract void Disconnect ();

		protected abstract bool IsConnectionPossible (IFigure figure);
		
		protected abstract PointD FindPoint(IConnector connector);

		protected IConnectionFigure Connection { get; set; }

		protected IFigure TargetFigure { get; set; }
		
		private IFigure FindConnectableFigure (double x, double y, IDrawing drawing) {
			foreach (IFigure figure in drawing.FiguresEnumeratorReverse) {
				if (figure.ContainsPoint (x, y) && IsConnectionPossible (figure)) {
					return figure;
				}
			}
			return null;
		}

		private IConnector FindConnectionTarget (double x, double y, IDrawing drawing) {
			IFigure target = FindConnectableFigure (x, y, drawing);
			return target != null ? target.ConnectorAt (x, y) : null;
		}

		private IConnector _oldTarget;
		private IConnector _newTarget;
	}
}

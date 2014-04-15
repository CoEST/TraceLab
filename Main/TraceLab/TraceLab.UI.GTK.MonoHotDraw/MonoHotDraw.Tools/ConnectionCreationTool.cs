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
using MonoHotDraw.Commands;
using MonoHotDraw.Handles;
using MonoHotDraw.Connectors;
using MonoHotDraw.Util;

namespace MonoHotDraw.Tools {

	public class ConnectionCreationTool: CreationTool {
	
		public ConnectionCreationTool (IDrawingEditor editor, IConnectionFigure fig): base (editor, fig) {
			_connection = fig;
			_connection.DisconnectStart ();
			_connection.DisconnectEnd ();
		}
		
		public override void MouseDrag (MouseEvent ev) {
			if (_handle != null) {
				_handle.InvokeStep (ev.X, ev.Y, ev.View);
			}
		}
		
		public override void MouseDown (MouseEvent ev) {
			IDrawingView view = ev.View;
			IFigure figure = view.Drawing.FindFigure (ev.X, ev.Y);

			if (figure != null) {
				_connection.EndPoint = new PointD (ev.X, ev.Y);
				_connection.StartPoint = new PointD (ev.X, ev.Y);
				_connection.ConnectStart (figure.ConnectorAt (ev.X, ev.Y));
				_connection.UpdateConnection ();
				view.Drawing.Add (_connection);
				view.ClearSelection ();
				view.AddToSelection (_connection);
				_handle = _connection.EndHandle;
				CreateUndoActivity();
			}
			else {
				Editor.Tool = new SelectionTool(Editor);
			}
		}
		
		public override void MouseUp (MouseEvent ev) {
			if (_handle != null) {
				_handle.InvokeEnd (ev.X, ev.Y, ev.View);
			}
						
			if (_connection.EndConnector == null) {
				_connection.DisconnectStart ();
				_connection.DisconnectEnd ();
				ev.View.Drawing.Remove (_connection);
				ev.View.ClearSelection ();
				UndoActivity = null;
			}
			else {
				ConnectionCreationToolUndoActivity activity = UndoActivity as ConnectionCreationToolUndoActivity;
				activity.EndConnector = _connection.EndConnector;
			}
			base.MouseUp(ev);
		}
		
		public override void MouseMove (MouseEvent ev) {
			Widget widget = (Widget) ev.View;
			IFigure figure = ev.View.Drawing.FindFigure (ev.X, ev.Y);
			if (figure != null) {
				widget.GdkWindow.Cursor = CursorFactory.GetCursorFromType (Gdk.CursorType.SbHDoubleArrow);
			}
			else {
				widget.GdkWindow.Cursor = CursorFactory.GetCursorFromType (Gdk.CursorType.Crosshair);
			}
		}
		
		public class ConnectionCreationToolUndoActivity: AbstractUndoActivity {
			public ConnectionCreationToolUndoActivity(IDrawingView view): base(view) {
				Undoable = true;
				Redoable = true;
			}
			
			public override bool Undo () {
				if (!base.Undo()  )
					return false;
				Connection.DisconnectStart();
				Connection.DisconnectEnd();
				DrawingView.Drawing.Remove(Connection);
				DrawingView.RemoveFromSelection(Connection);
				return true;
			}
			
			public override bool Redo () {
				if (!base.Redo() )
					return false;
				DrawingView.Drawing.Add(Connection);
				Connection.ConnectStart(StartConnector);
				Connection.ConnectEnd(EndConnector);
				return true;
			}
			
			public IConnectionFigure Connection { set; get; }
			public IConnector StartConnector { set; get; }
			public IConnector EndConnector { set; get; }
		}
		
		protected void CreateUndoActivity(){
			ConnectionCreationToolUndoActivity activity = new ConnectionCreationToolUndoActivity(Editor.View);
			activity.Connection = _connection;
			activity.StartConnector = _connection.StartConnector;
			UndoActivity = activity;
		}
		
		private IHandle _handle;
		private IConnectionFigure _connection;
	}
}
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
using Pango;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace MonoHotDraw.Tools {

	public class MultiLineTextTool: TextTool {
	
		public MultiLineTextTool (IDrawingEditor editor, MultiLineTextFigure fig, ITool dt): base (editor, fig, dt) {	
			_textview = new Gtk.TextView ();
			_textview.Buffer.Changed += new System.EventHandler (ChangedHandler);
			_textview.ModifyFont (fig.PangoLayout.FontDescription.Copy ());
			_textview.RightMargin = 5;
			_textview.Justification = ConvertJustificaton ();
		}
		
		private Gtk.Justification ConvertJustificaton () {
			Pango.Alignment alignment = ((MultiLineTextFigure)Figure).FontAlignment;
			
			switch (alignment) {
				case Pango.Alignment.Center: 
					return Gtk.Justification.Center;
				case Pango.Alignment.Left: 
					return Gtk.Justification.Left;
				case Pango.Alignment.Right: 
					return Gtk.Justification.Right;
				default: 
					return Gtk.Justification.Left;
			}
		}

		private void ChangedHandler (object sender, EventArgs args)	{
			((MultiLineTextFigure)Figure).Text = _textview.Buffer.Text;
			CalculateTextViewSize ();
		}
		
		private void CalculateTextViewSize () {
			int paddingLeft = (int)(Figure as MultiLineTextFigure).PaddingLeft;
            int paddingTop = (int)(Figure as MultiLineTextFigure).PaddingTop;
            int paddingRight = (int)(Figure as MultiLineTextFigure).PaddingRight;
            int paddingBottom = (int)(Figure as MultiLineTextFigure).PaddingBottom;
			RectangleD r = Figure.DisplayBox;
			r.Inflate(-paddingLeft, -paddingTop, -paddingRight, -paddingBottom);
			
			// Drawing Coordinates must be translated to View's coordinates in order to 
			// Correctly put the widget in the DrawingView
			PointD point = View.DrawingToView(r.X, r.Y);

			int x = (int) point.X;
			int y = (int) point.Y;
			int w = (int) Math.Max (r.Width, 10.0) + _textview.RightMargin * 2;
			int h = (int) Math.Max (r.Height, 10.0);
			
			_textview.SetSizeRequest (w, h);
			View.MoveWidget (_textview, x, y);
		}

		public override void MouseDown (MouseEvent ev) {
			IDrawingView view = ev.View;
			SetAnchorCoords (ev.X, ev.Y);
			View = view;
			
			Gdk.EventType type = ev.GdkEvent.Type;
			if (type == EventType.TwoButtonPress) {
				CreateUndoActivity();
				_showingWidget = true;
				_textview.Buffer.Text = ((MultiLineTextFigure)Figure).Text;
				
				View.AddWidget(_textview, 0, 0);
				CalculateTextViewSize();
				
				_textview.Show();
				_textview.GrabFocus();
				
				//selects all
				_textview.Buffer.SelectRange(_textview.Buffer.StartIter, _textview.Buffer.EndIter);
				
				return;
			}
			DefaultTool.MouseDown (ev);
		}

		public override void Deactivate () {
			if (_showingWidget) {
				View.RemoveWidget (_textview);
				UpdateUndoActivity();
				PushUndoActivity();
			}
			base.Deactivate();
		}
		
		private Gtk.TextView _textview;
	}
}

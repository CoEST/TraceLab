// MonoHotDraw. Diagramming Framework
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//	Mario Carrión <mario@monouml.org>
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
using Gtk;
using Pango;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MonoHotDraw.Tools;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {

	[Serializable]
	public class SimpleTextFigure : AttributeFigure {
	
		public event EventHandler TextChanged;

		public SimpleTextFigure (string text): base () {
			TextEditable  = true;
			PaddingLeft       = 2.0;
            PaddingTop       = 2.0;
            PaddingRight       = 2.0;
            PaddingBottom       = 2.0;
			FontColor     = (Cairo.Color) AttributeFigure.GetDefaultAttribute (FigureAttribute.FontColor);
			FontAlignment = (Pango.Alignment) AttributeFigure.GetDefaultAttribute (FigureAttribute.FontAlignment);
			FontFamily    = (string) AttributeFigure.GetDefaultAttribute (FigureAttribute.FontFamily);
			FontSize      = (int) AttributeFigure.GetDefaultAttribute (FigureAttribute.FontSize);
			FontStyle     = (Pango.Style) AttributeFigure.GetDefaultAttribute (FigureAttribute.FontStyle);
			_text         = text;
			GenerateDummyContext ();
		}

		protected SimpleTextFigure (SerializationInfo info, StreamingContext context) : base (info, context) {
			FontColor      = (Cairo.Color) info.GetValue ("FontColor", typeof (Cairo.Color));
			FontAlignment  = (Pango.Alignment) info.GetValue ("FontAlignment", typeof (Pango.Alignment));
			FontFamily     = (string) info.GetValue ("FontFamily", typeof (string));
			FontSize       = info.GetInt32 ("FontSize");
			FontStyle      = (Pango.Style) info.GetValue ("FontStyle", typeof (Pango.Style));
			_displayBox    = (RectangleD) info.GetValue ("DisplayBox", typeof (RectangleD));
			_text          = (string) info.GetValue ("Text", typeof (string));
			_textEditable  = info.GetBoolean ("TextEditable");
			_paddingLeft   = info.GetDouble ("PaddingLeft");
            _paddingTop    = info.GetDouble ("PaddingTop");
            _paddingRight  = info.GetDouble ("PaddingRight");
            _paddingBottom = info.GetDouble ("PaddingBottom");
		}
		
		public Pango.Alignment FontAlignment {
			get { return _fontAlignment; }
			set { _fontAlignment = value; }
		}
		
		public Cairo.Color FontColor {
			get { return _fontColor; }
			set { _fontColor = value; }
		}

		public string FontFamily {
			get { return _fontFamily; }
			set {
				if (value != null && value != string.Empty)
					_fontFamily = value;
			}
		}	

		public int FontSize {
			get { return _fontSize; }
			set { _fontSize = value; }
		}

		public Pango.Style FontStyle {
			get { return _fontStyle; }
			set { _fontStyle = value; } 
		}

		public virtual string Text {
			get { return _text; }
			set {
				if (_text == value) {
					return;
				}
				
				_text = value;

				WillChange ();
				if (_text != null && _text.Length > 0)
					PangoLayout.SetText (value);
				RecalculateDisplayBox ();
				Changed ();	
								
				OnTextChanged ();
			}
		}

		public bool TextEditable {
			get { return _textEditable; }
			set { _textEditable = value; }
		}
		
		public virtual Pango.Layout PangoLayout	{
			protected set {	_pangoLayout = value; }
			get { return _pangoLayout; }
		}

        #region Padding
		
		public virtual double PaddingLeft {
			get { return _paddingLeft; }
			set {
				if (value >= 0) { 
					WillChange ();
					_paddingLeft = value;
					RecalculateDisplayBox ();
					Changed ();
				}
			}
		}

        public virtual double PaddingRight {
            get { return _paddingRight; }
            set {
                if (value >= 0) { 
                    WillChange ();
                    _paddingRight = value;
                    RecalculateDisplayBox ();
                    Changed ();
                }
            }
        }

        public virtual double PaddingTop {
            get { return _paddingTop; }
            set {
                if (value >= 0) { 
                    WillChange ();
                    _paddingTop = value;
                    RecalculateDisplayBox ();
                    Changed ();
                }
            }
        }

        public virtual double PaddingBottom {
            get { return _paddingBottom; }
            set {
                if (value >= 0) { 
                    WillChange ();
                    _paddingBottom = value;
                    RecalculateDisplayBox ();
                    Changed ();
                }
            }
        }

        #endregion Padding

		public override RectangleD BasicDisplayBox {
			get { return _displayBox; }
			set {
				WillChange ();
				_displayBox = value; 
				RecalculateDisplayBox ();
			}
		}

		public override void BasicDraw (Cairo.Context context) {
			SetupLayout (context);
			DrawText (context);
			if (_usingDummy) {
				RecalculateDisplayBox();
				Changed();
				_usingDummy = false;
			}
		}
		
		public override void BasicDrawSelected (Cairo.Context context)
		{
			context.LineWidth = 1.0;
			RectangleD rect = DisplayBox;
			rect.OffsetDot5();
			context.Rectangle(GdkCairoHelper.CairoRectangle(rect));
			context.Stroke();
		}
	
		public override ITool CreateFigureTool (IDrawingEditor editor, ITool dt) {
			return TextEditable ? new SimpleTextTool (editor, this, dt) : dt;
		}
		
		public override object GetAttribute (FigureAttribute attribute) {
			switch (attribute) {
				case FigureAttribute.FillColor:
					return FillColor;
				case FigureAttribute.FontAlignment:
					return FontAlignment;
				case FigureAttribute.FontColor:
					return FontColor;
				case FigureAttribute.FontSize:
					return FontSize;
				case FigureAttribute.FontStyle:
					return FontStyle;
				case FigureAttribute.LineColor:
					return LineColor;
			}
			return base.GetAttribute (attribute); 
		}

		public override void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue ("DisplayBox", _displayBox);
			info.AddValue ("FontAlignment", _fontAlignment);
			info.AddValue ("FontColor", FontColor);
			info.AddValue ("FontFamily", _fontFamily);
			info.AddValue ("FontSize", _fontSize);
			info.AddValue ("FontStyle", _fontStyle);
			info.AddValue ("PaddingLeft", _paddingLeft);
            info.AddValue ("PaddingTop", _paddingTop);
            info.AddValue ("PaddingRight", _paddingRight);
            info.AddValue ("PaddingBottom", _paddingBottom);
			info.AddValue ("Text", _text);
			info.AddValue ("TextEditable", _textEditable);

			base.GetObjectData (info, context);
		}

		public override void SetAttribute (FigureAttribute attribute, object value) {
			//FIXME: Improve this logic, because doesn't make any sense
			//invalidating when isn't needed (current value = new value) 
			WillChange ();
			switch (attribute) {
				case FigureAttribute.FillColor:
					FillColor = (Cairo.Color) value;
					break;
				case FigureAttribute.FontAlignment:
					FontAlignment = (Pango.Alignment) value; 
					break;
				case FigureAttribute.FontColor:
					FontColor = (Cairo.Color) value;
					break;
				case FigureAttribute.FontSize:
					FontSize = (int) value;
					break;
				case FigureAttribute.FontStyle:
					FontStyle = (Pango.Style) value;
					break;
				case FigureAttribute.LineColor:
					LineColor = (Cairo.Color) value;
					break;
				default:
					base.SetAttribute (attribute, value);
					break;
			}
			GenerateDummyContext (); 
			Changed ();
		}

		protected virtual void DrawText (Cairo.Context context) 
        {
			context.Color = FontColor;
			context.MoveTo (DisplayBox.X + PaddingLeft, DisplayBox.Y + PaddingTop);
			Pango.CairoHelper.ShowLayout (context, PangoLayout);
			context.Stroke ();
		}
		
		protected virtual void OnTextChanged () 
        {
			if (TextChanged != null) {
				TextChanged (this, new EventArgs ());
			}
		}
		
		protected virtual void SetupLayout (Cairo.Context context) {
			PangoLayout = Pango.CairoHelper.CreateLayout (context);
			PangoLayout.FontDescription = FontFactory.GetFontFromDescription (string.Format ("{0} {1}", FontFamily, FontSize));
			if (Text != null && Text.Length > 0)
				PangoLayout.SetText (Text);
			PangoLayout.Alignment = FontAlignment;
			PangoLayout.ContextChanged ();
		}

		protected void RecalculateDisplayBox() 
        {
			int w = 0;
			int h = 0;
			
			if (PangoLayout != null)
				PangoLayout.GetPixelSize(out w, out h);
			
			RectangleD r = new RectangleD(DisplayBox.X + PaddingLeft, 
                                           DisplayBox.Y + PaddingTop, 
									        (double) w, (double) h);

			r.Inflate(PaddingLeft, PaddingTop, PaddingRight, PaddingBottom);
			_displayBox = r; 
		}
		
		private void GenerateDummyContext () {
			// Generates a dummy Cairo.Context. This trick is necesary in order to get
			// a Pango.Layout before obtaining a valid Cairo Context, otherwise, we should
			// wait until Draw method is called. The Pango.Layout is neccesary for
			// RecalculateDisplayBox.
			ImageSurface surface = new ImageSurface (Cairo.Format.ARGB32, 100, 100);			
			using (Cairo.Context dummycontext =  new Cairo.Context (surface)) {
				SetupLayout (dummycontext);
				RecalculateDisplayBox ();
			}
		}

		private RectangleD      _displayBox;
		private Pango.Alignment _fontAlignment;
		private Cairo.Color     _fontColor;
		private string          _fontFamily;
		private int             _fontSize;
		private Pango.Style     _fontStyle;
		private double          _paddingLeft;
        private double          _paddingTop;
        private double          _paddingRight;
        private double          _paddingBottom;
		private Pango.Layout    _pangoLayout;
		private string          _text;
		private bool            _textEditable;
		private bool            _usingDummy = true;
	}
}

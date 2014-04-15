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
using Gdk;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {
	
	public class PixbufFigure: AbstractFigure {
		
		public PixbufFigure(Pixbuf pixbuf): base() {
			Image = pixbuf;
		}
		
		public Pixbuf Image {
			get {
				return _pixbuf;
			}
			
			set {
				_pixbuf = value;
				_image = GdkCairoHelper.PixbufToImageSurface(_pixbuf);
			}
		}
		
		public override RectangleD BasicDisplayBox {
			get {
				return new RectangleD {
					X = Position.X,
					Y = Position.Y,
					Width = _image.Width,
					Height = _image.Height,
				};
			}
			
			set {
				Position = value.TopLeft;
			}
		}
		
		public override void BasicDraw(Context context) {
			RectangleD r = DisplayBox;
			_image.Show (context, Math.Round (r.X), Math.Round (r.Y));
		}
		
		public override void BasicDrawSelected (Cairo.Context context)
		{
			context.Rectangle(GdkCairoHelper.CairoRectangle(DisplayBox));
		}

		
		protected PointD Position { get; set; }
		
		private Pixbuf _pixbuf;
		private ImageSurface _image;
	}
}

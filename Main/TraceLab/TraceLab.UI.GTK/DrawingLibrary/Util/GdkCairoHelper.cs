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

namespace MonoHotDraw.Util {

	public sealed class GdkCairoHelper {
	
		private GdkCairoHelper () {
		}
		
		public static Cairo.ImageSurface PixbufToImageSurface (Gdk.Pixbuf pixbuf) {
			Cairo.Format format = Cairo.Format.RGB24;
			if (pixbuf.HasAlpha) {
				format = Cairo.Format.ARGB32;
			}
			
			Cairo.ImageSurface surface = new ImageSurface (format, pixbuf.Width, pixbuf.Height);
			using (Cairo.Context context = new Cairo.Context (surface)) {
				Gdk.CairoHelper.SetSourcePixbuf (context, pixbuf, 0.0, 0.0);
				context.Paint ();
			}
			
			return surface;
		}
		
		public static Gdk.Color GdkColor (Cairo.Color color) {
			Gdk.Color gdk = new Gdk.Color ((byte) (color.R * System.Byte.MaxValue),
			                               (byte) (color.G * System.Byte.MaxValue),
			                               (byte) (color.B * System.Byte.MaxValue));
			return gdk;
		}
		
		public static Cairo.Color CairoColor (Gdk.Color color) {
			double blue = (double)color.Blue / System.UInt16.MaxValue;
			double green = (double)color.Green / System.UInt16.MaxValue;
			double red   = (double)color.Red / System.UInt16.MaxValue;
			
			blue  = Convert.ToDouble (decimal.Round ((decimal) blue, 2));
			green = Convert.ToDouble (decimal.Round ((decimal) green, 2));
			red   = Convert.ToDouble (decimal.Round ((decimal) red, 2));

			Cairo.Color cairo = new Cairo.Color (red, green, blue);
			return cairo;
		}
		
		public static bool RectangleInsideGdkRegion (RectangleD r, Gdk.Region region) {
			r.Inflate (1.0, 1.0);
			Gdk.Rectangle gdkRect = GdkRectangle (r);
			Gdk.OverlapType type = region.RectIn (gdkRect);

			return (type == Gdk.OverlapType.In || type == Gdk.OverlapType.Part);
		}
		
		public static Cairo.Rectangle CairoRectangle (RectangleD r) {
			return new Cairo.Rectangle (r.X, r.Y, r.Width, r.Height); 
		}
		
		public static Cairo.Rectangle CairoRectangle (Gdk.Rectangle r) {
			return new Cairo.Rectangle ((double)r.X, (double)r.Y, (double)r.Width, (double)r.Height); 
		}
		
		public static Gdk.Rectangle GdkRectangle (RectangleD r) {
			return new Gdk.Rectangle ((int) r.X, (int) r.Y, (int) r.Width, (int) r.Height);
		}
		
		public static PointD OffsetDot5(PointD point) {
			return new PointD {
				X = Math.Truncate(point.X) + 0.5,
				Y = Math.Truncate(point.Y) + 0.5
			};
		}
	}
}

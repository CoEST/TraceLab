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
using System.Collections.Generic;
using MonoHotDraw.Connectors;

namespace MonoHotDraw.Figures {

	[Serializable]
	public class EllipseFigure: BaseBoxFigure {
	
		public EllipseFigure (): base () {
		}

		public override void BasicDraw (Context context) {
			double midwidth  = DisplayBox.Width / 2.0;
			double midheight = DisplayBox.Height / 2.0;

			context.LineWidth = LineWidth;
			context.Save ();
			context.Translate (DisplayBox.X + midwidth, DisplayBox.Y + midheight);
			context.Scale (midwidth - 1.0, midheight - 1.0);
			context.Arc (0.0, 0.0, 1.0, 0.0, 2.0 * Math.PI);
			context.Restore ();
			context.Color = FillColor;
			context.FillPreserve ();
			context.Color = LineColor;
			context.Stroke ();
		}
		
		public override IConnector ConnectorAt (double x, double y) {
			return new ChopEllipseConnector (this);
		}
	}
}

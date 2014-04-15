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
using System.Collections.Generic;
using System.Linq;
using Cairo;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {
	
	public enum HStackAlignment {
		Center,
		Top,
		Bottom,
	}
	
	public class HStackFigure: StackFigure {
		
		public HStackFigure(): base() {
			Alignment = HStackAlignment.Center;
		}
		
		public HStackAlignment Alignment { get; set; }
		
		protected override double CalculateHeight()
		{
			if (Figures.Count() == 0)
				return 0.0;
			return (from IFigure fig in this.Figures
			        select fig.DisplayBox.Height).Max();
		}
		
		protected override double CalculateWidth() {
			int count = Figures.Count();
			
			if (count == 0)
				return 0.0;
			
			return (from IFigure fig in this.Figures
			        select fig.DisplayBox.Width).Sum() + Spacing * (count-1);
		}
		
		protected override void UpdateFiguresPosition() {
			double width = 0.0;
			foreach (IFigure figure in Figures) {
				RectangleD r = figure.DisplayBox;
				r.X = Position.X + width;
				r.Y = CalculateFigureY(figure);
				AbstractFigure af = figure as AbstractFigure;
				af.BasicDisplayBox = r;
				width += r.Width + Spacing;
			}
		}
		
		private double CalculateFigureY(IFigure figure) {
			switch (Alignment) {
			case HStackAlignment.Center:
				return Position.Y + (Height - figure.DisplayBox.Height)/2;
			case HStackAlignment.Top:
				return Position.Y;
			case HStackAlignment.Bottom:
				return Position.Y + (Height - figure.DisplayBox.Height);
			default:
				return Position.Y;
			}
		}
	}
}

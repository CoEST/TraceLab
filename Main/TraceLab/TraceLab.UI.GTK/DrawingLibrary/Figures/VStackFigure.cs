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
	
	public enum VStackAlignment {
		Center,
		Left,
		Right,
	}
	
	public class VStackFigure: StackFigure {
		
		public VStackFigure(): base() {
			Alignment = VStackAlignment.Left;
		}
		
		public VStackAlignment Alignment { get; set; }
		
		protected override double CalculateHeight()
		{
			int count = Figures.Count();
			
			if (count == 0)
				return 0.0;
			
			return (from IFigure fig in this.Figures
			        select fig.DisplayBox.Height).Sum() + Spacing * (count-1);
		}
		
		protected override double CalculateWidth() {
			if (Figures.Count() == 0)
				return 0.0;
			
			return (from IFigure fig in this.Figures
			        select fig.DisplayBox.Width).Max();
		}
		
		protected override void UpdateFiguresPosition() {
			double height = 0.0;
			foreach (IFigure figure in Figures) {
				RectangleD r = figure.DisplayBox;
				r.X = CalculateFigureX(figure);
				r.Y = Position.Y + height;
				AbstractFigure af = figure as AbstractFigure;
				af.BasicDisplayBox = r;
				height += r.Height + Spacing;
			}
		}
		
		private double CalculateFigureX(IFigure figure) {
			
			switch (Alignment) {
			case VStackAlignment.Center:
				return Position.X + (Width - figure.DisplayBox.Width)/2;
			case VStackAlignment.Left:
				return Position.X;
			case VStackAlignment.Right:
				return Position.X + (Width - figure.DisplayBox.Width);
			default:
				return Position.X;
			}
		}
	}
}

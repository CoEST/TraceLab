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
using MonoHotDraw.Figures;

namespace MonoHotDraw.Locators {
	
	public class AbsoluteLocator: ILocator {
		
        private AbsoluteTo m_absoluteTo;

        public AbsoluteLocator(): this(0.0, 0.0, AbsoluteTo.TopLeft) 
        {
		}
		
		public AbsoluteLocator(double x, double y, AbsoluteTo absoluteTo) 
        {
			this.x = x;
			this.y = y;
            m_absoluteTo = absoluteTo;
		}
		
		public PointD Locate(IFigure owner) 
        {
            if (m_absoluteTo == AbsoluteTo.TopRight) 
            {
                PointD topRight = owner.DisplayBox.TopRight;
                return new PointD {
                    X = topRight.X - x,
                    Y = topRight.Y + y,
                };
            } 
            else 
            {
                PointD topLeft = owner.DisplayBox.TopLeft;
                return new PointD {
                    X = topLeft.X + x,
                    Y = topLeft.Y + y,
                };
            }
		}
		
		double x;
		double y;

        public enum AbsoluteTo 
        {
            TopLeft,
            TopRight
        }
	}
}

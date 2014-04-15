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
using System.Runtime.Serialization;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace MonoHotDraw.Connectors {

	[Serializable]
	public class ChopBoxConnector: AbstractConnector {
	
		public ChopBoxConnector (IFigure figure): base (figure) {
		}

		protected ChopBoxConnector (SerializationInfo info, StreamingContext context) : base (info, context) {
		}

		public override PointD FindStart (IConnectionFigure connection) {
			if (connection == null) {
				return DisplayBox.Center;
			}
				
			PointD point = connection.PointAt (1);
	
			return Chop (Owner, point);
		}

		public override PointD FindEnd (IConnectionFigure connection) {
		
			if (connection == null) {
				return DisplayBox.Center;
			}

			PointD point = connection.PointAt (connection.PointCount - 2);

			return Chop (Owner, point);
		}

		protected virtual PointD Chop (IFigure target, PointD point) {	
			if (target == null) {
				return new PointD (0, 0);
			}
			
			else if (target.ContainsPoint (point.X, point.Y)) {
				return target.DisplayBox.Center;
			}

			double angle = Geometry.AngleFromPoint (DisplayBox, point);
			return Geometry.EdgePointFromAngle (DisplayBox, angle);
		}
	}
}

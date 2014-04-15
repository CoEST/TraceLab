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
using System;
using System.Runtime.Serialization;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {
	
	[Serializable]
	public class TriangleArrowLineTerminal : LineTerminal {

		public TriangleArrowLineTerminal (): this (10.0, 20.0) {
		}
	
		public TriangleArrowLineTerminal (double lDistance, double pDistance): base () {
			_lineDistance = lDistance;
			_pointDistance = pDistance;
		}

		protected TriangleArrowLineTerminal (SerializationInfo info, StreamingContext context) : base (info, context) {
			_lineDistance  = info.GetDouble ("LineDistance");
			_pointDistance = info.GetDouble ("PointDistance");
		}
		
		public override PointD Draw (Context context, PointD a, PointD b) {
			PointD leftPoint = new PointD ();
			PointD middlePoint = new PointD ();
			PointD rightPoint = new PointD ();
			Geometry.GetArrowPoints (a, b, _lineDistance, _pointDistance, 
									out leftPoint, out rightPoint, out middlePoint);
			
			context.MoveTo (middlePoint);
			context.LineTo (leftPoint);
			context.LineTo (a);
			context.LineTo (rightPoint);
			context.LineTo (middlePoint);
			context.Stroke ();
			
			return middlePoint;
		}
		
		public override void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue ("LineDistance", _lineDistance);
			info.AddValue ("PointDistance", _pointDistance);

			base.GetObjectData (info, context);
		}
		
		public override RectangleD InvalidateRect (PointD b) {
			double distance = Math.Max (_lineDistance*2, _pointDistance);
			RectangleD r = new RectangleD (b.X, b.Y, 0.0, 0.0);
			r.Inflate (distance, distance);
			return r;
		}

		private double _lineDistance;
		private double _pointDistance;
	}
}

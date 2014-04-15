
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
using System.Collections.Generic;
using System.Runtime.Serialization;
using MonoHotDraw.Handles;
using MonoHotDraw.Locators;
using MonoHotDraw.Tools;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {

	[Serializable]
	public class PolyLineFigure : AttributeFigure {
	
		public PolyLineFigure () {
			_points = new List <PointD> ();
		}
		
		protected PolyLineFigure (SerializationInfo info, StreamingContext context) : base (info, context) {
			_dashes  = (double[])  info.GetValue ("Dashes", typeof (double[]));
			_startTerminal = (LineTerminal) info.GetValue ("StartTerminal", typeof (LineTerminal));
			_endTerminal = (LineTerminal) info.GetValue ("EndTerminal", typeof (LineTerminal));
			_points = (List <PointD>) info.GetValue ("Points", typeof (List <PointD>));
		}

		public virtual double[] Dashes {
			get { return _dashes; }
			set { _dashes = value; }
		}

		public virtual int FindSegment (double x, double y) {
			for (int i = 0; i < _points.Count - 1; i++) {
				PointD p1 = PointAt (i);
				PointD p2 = PointAt (i + 1);
				if (Geometry.LineContainsPoint (p1.X, p1.Y, p2.X, p2.Y, x, y)) {
					return i + 1;
				}
			}
			return -1;
		}

		public virtual void AddPoint (double x, double y) {
			WillChange ();
			_points.Add (new PointD (x, y));
			Changed ();
		}

		public virtual void RemovePointAt (int i) {
			WillChange ();
			_points.RemoveAt (i);
			Changed ();
		}

		public virtual void ClearPoints () {
			_points.Clear ();
		}

		public virtual void SplitSegment (double x, double y) {
			int index = FindSegment (x, y);
			
			if (index != -1) {
				InsertPointAt (index, x, y);
			}
		}

		public virtual void InsertPointAt (int index, double x, double y) {
			WillChange ();
			_points.Insert (index, new PointD (x, y));
			Changed ();	
		}

		public virtual void SetPointAt (int index, double x, double y) {
			WillChange ();
			_points [index] = new PointD (x, y);
			Changed ();
		}

		public virtual int PointCount {
			get { return _points.Count; }
		}

		public virtual PointD PointAt (int index) {
			return _points [index];
		}
		
		public virtual PointD StartPoint {
			get { return PointAt (0); }
			set {
				WillChange ();
				if (PointCount == 0) {
					AddPoint (value.X, value.Y);
				}
				else {
					_points [0] = value;
				}
				Changed ();
			}
		}

		public virtual PointD EndPoint {
			get { return PointAt (PointCount - 1); }
			set {
				WillChange ();
				if (PointCount < 2) {
					AddPoint (value.X, value.Y);
				}
				else {
					_points [PointCount - 1] = value;
				}
				Changed ();
			}
		}
			
		public virtual LineTerminal StartTerminal {
			get { return _startTerminal; }
			set { _startTerminal = value; }
		}
			
		public virtual LineTerminal EndTerminal {
			get { return _endTerminal; }
			set { _endTerminal = value; }
		}
		
		public override IEnumerable <IHandle> HandlesEnumerator {
			get {
				for (int i=0; i<PointCount; i++) {
					yield return new PolyLineHandle (this, new PolyLineLocator (i), i);
				}
			}
		}
		
		public virtual List <PointD> Points {
			get { return _points; }
		}
		
        public ITool CreateFigureTool(IPrimaryToolDelegator mainTool, IDrawingEditor editor, ITool defaultTool, MouseEvent ev)
        {
            return new PolyLineFigureTool (editor, this, defaultTool);
		}
		
		public override RectangleD InvalidateDisplayBox {
			get {
				RectangleD rect = DisplayBox;
				if (StartTerminal != null) {
					rect.Add (StartTerminal.InvalidateRect (StartPoint));
				}
					
				if (EndTerminal != null) {
					rect.Add (EndTerminal.InvalidateRect (EndPoint));
				}
					
				rect.Inflate (6.0, 6.0);
					
				return rect;
			}
		}
		
		public override void BasicDraw (Context context) {
			if (_points.Count < 2) {
				return;
			}

			context.LineWidth = LineWidth;
			context.LineJoin  = LineJoin.Round;
			context.Color = LineColor;
			
			if (Dashes != null) {
				context.SetDash (Dashes, 0);
			}
				
			PointD start, end;
				
			if (StartTerminal != null) {
				start = StartTerminal.Draw (context, StartPoint, _points [1]);
			}
			else {
				start = StartPoint;
			}
				
			if (EndTerminal != null) {
				end = EndTerminal.Draw (context, EndPoint, _points [PointCount-2]);
			}
			else {
				end = EndPoint;
			}
			
			context.MoveTo (start);
				
			for (int i = 1; i < _points.Count-1; i++)
				context.LineTo (_points [i]);
				
			context.LineTo (end);
			
			context.Stroke ();
		}

		public override void BasicMoveBy (double x, double y) {
			PointD newpoint;

			for (int i = 0; i < _points.Count; i++) {
				newpoint = _points [i];
				newpoint.X += x;
				newpoint.Y += y;
				_points [i] = newpoint;
			}
		}

		public override RectangleD BasicDisplayBox {
			get {
				if (_points.Count < 2) {
					return RectangleD.Empty;
				}

				RectangleD rect = new RectangleD (_points [0]);
				
				foreach (PointD point in _points) {
					rect.Add (point);
				}

				return rect;
			}
			set { }
		}

		public override bool ContainsPoint (double x, double y) {
			RectangleD rect = DisplayBox;
			rect.Inflate (4.0, 4.0);
			if (!rect.Contains (x, y)) {
				return false;
			}

			for (int i = 0; i < _points.Count - 1; i++) {
				PointD p1 = _points [i];
				PointD p2 = _points [i + 1];
				if (Geometry.LineContainsPoint (p1.X, p1.Y, p2.X, p2.Y, x, y)) {
					return true;
				}
			}
			return false;
		}
		
		public override void GetObjectData (SerializationInfo info, StreamingContext context) {		
			info.AddValue ("Dashes", _dashes);
			info.AddValue ("StartTerminal", _startTerminal);
			info.AddValue ("EndTerminal", _endTerminal);
			info.AddValue ("Points", _points);
			
			base.GetObjectData (info, context);
		}
		
		private double[] _dashes;
		private List <PointD> _points;
		private LineTerminal _startTerminal;
		private LineTerminal _endTerminal;
	}
}

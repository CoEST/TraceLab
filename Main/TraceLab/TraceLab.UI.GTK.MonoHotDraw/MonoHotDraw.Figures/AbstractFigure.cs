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
using System.Runtime.Serialization;
using Cairo;
using MonoHotDraw.Commands;
using MonoHotDraw.Connectors;
using MonoHotDraw.Handles;
using MonoHotDraw.Tools;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {

	[Serializable]
	public abstract class AbstractFigure : IFigure {
	
		protected AbstractFigure () {
			FillColor = new Cairo.Color (1.0, 1.0, 0.2, 0.8);
			LineColor = (Color) AttributeFigure.GetDefaultAttribute (FigureAttribute.LineColor);
		}

		protected AbstractFigure (SerializationInfo info, StreamingContext context) {
			FillColor = (Cairo.Color) info.GetValue ("FillColor", typeof (Cairo.Color));
			LineColor = (Cairo.Color) info.GetValue ("LineColor", typeof (Cairo.Color));
		}

		public virtual RectangleD DisplayBox {
			get { return BasicDisplayBox; }
			set {
				if (value != DisplayBox) {
					WillChange ();
					BasicDisplayBox = value;
					Changed ();
				}
			}
		}
		
		//TODO: This needs to be protected
		public abstract RectangleD BasicDisplayBox { get; set; }
		
		public virtual bool CanConnect {
			get { return true; }
		}
		
		public virtual IEnumerable <IFigure> FiguresEnumerator {
			get { yield break; }
		}
		
		public virtual Color FillColor {
			get { return _fillColor; }
			set { _fillColor = value; }
		}
		
		public virtual Color LineColor {
			get { return _lineColor; }
			set { _lineColor = value; }
		}

		public virtual double LineWidth {
			get { return (double) GetAttribute (FigureAttribute.LineWidth); }
			set {
				if (value >= 0) {
					SetAttribute (FigureAttribute.LineWidth, value);
				}
			}
		}
		
		public virtual ITool CreateFigureTool (IDrawingEditor editor, ITool defaultTool) {
			return defaultTool;
		}
		
		public virtual void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue ("FillColor", FillColor);
			info.AddValue ("LineColor", LineColor);
		}

		public virtual IEnumerable <IHandle> HandlesEnumerator {
			get { yield break; }
		}

		public virtual void Draw (Context context) {
			context.Save ();
			BasicDraw (context);
			context.Restore ();
		}
		
		// TODO: This needs to be protected
		public virtual void BasicDraw (Context context) {
		}
		
		public virtual void DrawSelected (Context context) {
			context.Save ();
			BasicDrawSelected (context);
			context.Restore ();
		}
		
		public virtual void BasicDrawSelected (Context context)  {
		}
		
		public virtual bool Includes (IFigure figure) {
			return (this == figure);
		}

		public virtual object GetAttribute (FigureAttribute attribute) {
			switch (attribute) {
				case FigureAttribute.FillColor:
					return FillColor;
				case FigureAttribute.LineColor:
					return LineColor;
				default:
					return null;
			}
		}
		
		public virtual void SetAttribute (FigureAttribute attribute, object value) {
			switch (attribute) {
				case FigureAttribute.FillColor:
					FillColor = (Cairo.Color) value;
					break;
				case FigureAttribute.LineColor:
					LineColor = (Cairo.Color) value;
					break;
			}
		}

		public void MoveBy (double x, double y) {
			WillChange ();
			BasicMoveBy (x, y);
			Changed ();
		}
		
		public void MoveTo (double x, double y) {
			RectangleD r = DisplayBox;
			r.X = x;
			r.Y = y;
			DisplayBox = r;
		}

		public virtual void BasicMoveBy (double x, double y) {
			RectangleD r = BasicDisplayBox;
			r.X += x;
			r.Y += y;
			BasicDisplayBox = r;
		}

		public virtual bool ContainsPoint (double x, double y) {
			return DisplayBox.Contains (x, y);
		}
		
		public virtual object Clone () {
			return GenericCloner.Clone <AbstractFigure> (this);
		}

		public void Invalidate () {
			OnFigureInvalidated (new FigureEventArgs (this, InvalidateDisplayBox));
		}

		public virtual IConnector ConnectorAt (double x, double y) {
			return new ChopBoxConnector (this);
		}

		public virtual RectangleD InvalidateDisplayBox {
			get {
				RectangleD rect = DisplayBox;
				rect.Inflate (AbstractHandle.Size + 1.0 , AbstractHandle.Size + 1.0);
				return rect;
			}
		}

		public void Visit (IFigureVisitor visitor) {
			visitor.VisitFigure (this);

			foreach (IFigure figure in FiguresEnumerator) {
				figure.Visit (visitor);
			}
			
			foreach (IHandle handle in HandlesEnumerator) {
				visitor.VisitHandle (handle);
			}
		}
		
		public event EventHandler <FigureEventArgs> FigureInvalidated;
		public event EventHandler <FigureEventArgs> FigureChanged;

		protected virtual void OnFigureInvalidated (FigureEventArgs e) {
			if (FigureInvalidated != null) {
				FigureInvalidated (this, e);
			}
		}

		protected void WillChange () {
			Invalidate ();
		}

		protected void Changed () {
			Invalidate ();
			OnFigureChanged (new FigureEventArgs (this, DisplayBox));
		}

		protected virtual void OnFigureChanged (FigureEventArgs e) {
			if (FigureChanged != null) {
				FigureChanged (this, e);
			}
		}

		private List <IFigure> _dependentFigures;
		private Cairo.Color    _fillColor;
		private Cairo.Color    _lineColor;
	}
}

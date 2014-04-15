// TODO: Change nage to LineConnectionFigure

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
using MonoHotDraw.Connectors;
using MonoHotDraw.Handles;
using MonoHotDraw.Locators;

namespace MonoHotDraw.Figures {

	[Serializable]
	public class LineConnection : PolyLineFigure, IConnectionFigure, IDeserializationCallback {
	
		public LineConnection () : base () {
			AddPoint (0.0, 0.0);
			AddPoint (0.0, 0.0);
		}

		protected LineConnection (SerializationInfo info, StreamingContext context) : base (info, context) {
			_endConnector = (IConnector) info.GetValue ("EndConnector", typeof (IConnector));
			_startConnector = (IConnector) info.GetValue ("StartConnector", typeof (IConnector));
		}
		
		public event EventHandler ConnectionChanged;

		public LineConnection (IFigure fig1, IFigure fig2): this ()	{
			if (fig1 != null) {
				ConnectStart (fig1.ConnectorAt (0.0, 0.0));
			}

			if (fig2 != null) {
				ConnectEnd (fig2.ConnectorAt (0.0, 0.0));
			}
		}

		public virtual IConnector StartConnector {
			get { return _startConnector; }
			protected set { _startConnector = value; }
		}

		public virtual IConnector EndConnector {
			get { return _endConnector; }
			protected set {	_endConnector = value; }
		}

		public void OnDeserialization (Object sender) {
			ConnectFigure (_startConnector);
			StartConnector = _startConnector;
			
			ConnectFigure (_endConnector);
			EndConnector = _endConnector;
		}

		public virtual void ConnectStart (IConnector start)	{
			if (StartConnector == start) {
				return;
			}

			DisconnectStart ();
			StartConnector = start;
			ConnectFigure (StartConnector);
			OnConnectionChanged();
		}

		public virtual void ConnectEnd (IConnector end)	{
			if (EndConnector == end) {
				return;
			}
			
			DisconnectEnd ();
			EndConnector = end;
			ConnectFigure (EndConnector);
			OnConnectionChanged();
		}

		public virtual void DisconnectStart () {
			if (StartConnector == null) {
				return;
			}
			DisconnectFigure (StartConnector);
			StartConnector = null;
			OnConnectionChanged();
		}

		public virtual void DisconnectEnd () {
			if (EndConnector == null) {
				return;
			}
			DisconnectFigure (EndConnector);
			EndConnector = null;
			OnConnectionChanged();
		}

		public virtual bool CanConnectEnd (IFigure figure) {
			return true;
		}

		public virtual bool CanConnectStart (IFigure figure) {
			return true;
		}

		public virtual IFigure StartFigure {
			get {
				if (StartConnector != null) {
					return StartConnector.Owner;
				}
				return null;
			}
		}
		public virtual IFigure EndFigure {
			get {
				if (EndConnector != null) {
					return EndConnector.Owner;
				}
				return null;
			}
		}
		
		public virtual void UpdateConnection ()	{	
			if (StartConnector != null) {
				StartPoint = StartConnector.FindStart (this);
			}

			if (EndConnector != null) {
				EndPoint = EndConnector.FindEnd (this);
			}
		}
		
		public virtual IHandle StartHandle {
			get { return new ChangeConnectionStartHandle (this); }
		}
		
		public virtual IHandle EndHandle {
			get { return new ChangeConnectionEndHandle (this); }
		}

		public override void BasicMoveBy (double x, double y) {
			for (int i = 1; i < PointCount - 1; i++) {
				PointD newpoint = PointAt (i);
				newpoint.X += x;
				newpoint.Y += y;
				SetPointAt (i, newpoint.X, newpoint.Y);
			}
			UpdateConnection ();
		}
		
		public override bool CanConnect {
			get { return false; }
		}

		public override void SetPointAt (int index, double x, double y) {
			base.SetPointAt (index, x, y);
			UpdateConnection ();
		}
		
		public override void RemovePointAt (int i) {
			base.RemovePointAt (i);
			UpdateConnection();
		}
		
		public override void InsertPointAt (int index, double x, double y) {
			base.InsertPointAt (index, x, y);
			UpdateConnection();
		}

		public override IEnumerable <IHandle> HandlesEnumerator {
			get {
				if (PointCount < 2) {
					yield break;
				}

				yield return StartHandle;
				
				for (int i = 1; i < PointCount - 1; i++) {
					yield return new LineConnectionHandle (this, new PolyLineLocator (i), i);
				}

				yield return EndHandle;
			}
		}
		
		public override void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue ("EndConnector",   EndConnector);
			info.AddValue ("StartConnector", StartConnector);
			
			base.GetObjectData (info, context);
		}
		
		protected virtual void OnConnectionChanged() {
			if (ConnectionChanged != null) {
				ConnectionChanged(this, new EventArgs());
			}
		}
		
		private void ConnectFigure (IConnector connector) {
			if (connector != null) {
				connector.Owner.FigureChanged += FigureChangedHandler;
				UpdateConnection ();
			}
		}
		
		private void DisconnectFigure (IConnector connector) {
			if (connector != null) {
				connector.Owner.FigureChanged -= FigureChangedHandler;
			}
		}

		private void FigureChangedHandler (object sender, FigureEventArgs args)	{
			UpdateConnection ();
		}
		
		private IConnector _endConnector;
		private IConnector _startConnector;
	}
}

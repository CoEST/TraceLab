// TODO: Clean serialization stuff

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
using MonoHotDraw.Commands;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace MonoHotDraw.Connectors{

	[Serializable]
	public abstract class AbstractConnector : IConnector {
	
		protected AbstractConnector (IFigure owner) {
			Owner = owner;
		}

		protected AbstractConnector (SerializationInfo info, StreamingContext context) {
			_owner = (IFigure) info.GetValue ("Owner", typeof (IFigure));
		}

		public virtual IFigure Owner {
			get { return _owner; }
			protected set { _owner = value; }
		}

		public virtual RectangleD DisplayBox {
			get { return Owner.DisplayBox; }
		}
		
		public virtual object Clone () {
			return GenericCloner.Clone <AbstractConnector> (this);
		}

		public virtual bool ContainsPoint (double x, double y) {
			return Owner.ContainsPoint (x, y);
		}

		public virtual void Draw (Context context) {
		}

		public virtual PointD FindStart (IConnectionFigure connection) {
			return DisplayBox.Center;
		}

		public virtual PointD FindEnd (IConnectionFigure connection) {
			return DisplayBox.Center;
		}
		
		public virtual void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue ("Owner", Owner);
		}

		private IFigure _owner;
	}
}

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
using MonoHotDraw.Connectors;
using MonoHotDraw.Figures;

namespace MonoHotDraw.Handles {

	public class ChangeConnectionEndHandle: ChangeConnectionHandle {
	
		public ChangeConnectionEndHandle (IConnectionFigure owner): base (owner) {
		}

		public override PointD Locate () {
			return Connection.EndPoint;
		}

		protected override IConnector Target {
			get { return Connection.EndConnector; }
		}

		protected override void Connect (IConnector connector) {
			Connection.ConnectEnd (connector);
		}

		protected override void Disconnect () {
			Connection.DisconnectEnd ();
		}
		
		protected override bool IsConnectionPossible (IFigure figure) {
			if (!figure.Includes (Connection)
			    && figure.CanConnect
			    && Connection.CanConnectEnd (figure)) {
			    
				return true;
			}

			return false;
		}

		protected override PointD Point {
			set { Connection.EndPoint = value; }
		}
		
		protected override PointD FindPoint (IConnector connector) {
			return connector.FindEnd(Connection);
		}
	}
}

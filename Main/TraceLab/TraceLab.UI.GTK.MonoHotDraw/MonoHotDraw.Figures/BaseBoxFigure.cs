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
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures {

	[Serializable]
	public abstract class BaseBoxFigure : AttributeFigure {

		protected BaseBoxFigure () {
		}

		protected BaseBoxFigure (SerializationInfo info, StreamingContext context) : base (info, context) {
			DisplayBox = ((RectangleD) info.GetValue ("DisplayBox", typeof (RectangleD)));
		}
		
		public override RectangleD BasicDisplayBox {
			set { _displayBox = value; }
			get { return _displayBox; }
		}

		public override IEnumerable <IHandle> HandlesEnumerator {
			get {
				if (_handles == null) {
					InstantiateHandles ();
				}

				foreach (IHandle handle in _handles) {
					yield return handle;
				}
			}
		}
		
		public override void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue ("DisplayBox", _displayBox);

			base.GetObjectData (info, context);
		}
		
		private void InstantiateHandles () {
			_handles = new List <IHandle> ();
			_handles.Add (new SouthEastHandle (this));
			_handles.Add (new SouthWestHandle (this));
			_handles.Add (new NorthWestHandle (this));
			_handles.Add (new NorthEastHandle (this));
			_handles.Add (new NorthHandle (this));
			_handles.Add (new EastHandle (this));
			_handles.Add (new SouthHandle (this));
			_handles.Add (new WestHandle (this));
		}

		private RectangleD     _displayBox;
		private List <IHandle> _handles;
	}
}

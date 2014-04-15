// MonoHotDraw. Diagramming Framework
//
// Authors:
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

namespace MonoHotDraw.Figures {
	//TODO: Verify serialization. It seems that Dictionary<FigureAttribute,object> 
	//doesn't serialize completetly.

	[Serializable]
	public abstract class AttributeFigure : AbstractFigure {

		protected AttributeFigure () {
		}	

		protected AttributeFigure (SerializationInfo info, StreamingContext context) : base (info, context) {
			if (info.GetBoolean ("HasAttributes") == false)
				return;
 
			_attributes = (Dictionary<FigureAttribute, object>) info.GetValue ("Attributes", typeof (Dictionary<FigureAttribute, object>));
		}

		public static object GetDefaultAttribute (FigureAttribute attribute) {
			if (_defaultAttributes == null)
				InitializeDefaultAttributes ();

			object returnValue = null;
			_defaultAttributes.TryGetValue (attribute, out returnValue); 
			return returnValue;
		}
		
		public static void SetDefaultAttribute (FigureAttribute attribute, object value) {
			if (value == null)
				return;

			if (_defaultAttributes == null)
				InitializeDefaultAttributes ();

			_defaultAttributes [attribute] = value;
		}
		
		public override void GetObjectData (SerializationInfo info, StreamingContext context) {
			if (_attributes != null && _attributes.Count > 0) {
				info.AddValue ("HasAttributes", true);
				info.AddValue ("Attributes", _attributes);
			} else {
				info.AddValue ("HasAttributes", false);
			}
			
			base.GetObjectData (info, context);
		}
		
		public override object GetAttribute (FigureAttribute attribute) {
			if (_attributes == null)
				return AttributeFigure.GetDefaultAttribute (attribute);

			object returnValue = null;
			if (_attributes.TryGetValue (attribute, out returnValue) == false)
				return AttributeFigure.GetDefaultAttribute (attribute); 

			return returnValue;
		}

		public override void SetAttribute (FigureAttribute attribute, object value) {
			if (value == null)
				return;

			if (_attributes == null)
				_attributes = new Dictionary<FigureAttribute, object> ();

			_attributes [attribute] = value;
		}
		
		private static void InitializeDefaultAttributes () {
			_defaultAttributes = new Dictionary<FigureAttribute, object> ();
			_defaultAttributes.Add (FigureAttribute.FontAlignment, Pango.Alignment.Left);
			_defaultAttributes.Add (FigureAttribute.FontFamily, "Sans-Serif");
			_defaultAttributes.Add (FigureAttribute.FontSize, 9);
			_defaultAttributes.Add (FigureAttribute.FontStyle, Pango.Style.Normal);
			_defaultAttributes.Add (FigureAttribute.FontColor, new Cairo.Color (0.0, 0.0, 0.0, 1.0));
			_defaultAttributes.Add (FigureAttribute.FillColor, new Cairo.Color (1.0, 1.0, 1.0, 0.8));
			_defaultAttributes.Add (FigureAttribute.LineColor, new Cairo.Color (0.0, 0.0, 0.0, 1.0));
			_defaultAttributes.Add (FigureAttribute.LineWidth, 1.0);
			//TODO: Should FigureAttribute.Location be added?
		}
		
		private Dictionary<FigureAttribute, object>        _attributes;
		private static Dictionary<FigureAttribute, object> _defaultAttributes;
	}
}

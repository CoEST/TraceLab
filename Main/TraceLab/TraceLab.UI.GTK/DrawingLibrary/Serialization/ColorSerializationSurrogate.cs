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
using System.Runtime.Serialization;

namespace MonoHotDraw.Util {

	public sealed class ColorSerializationSurrogate : ISerializationSurrogate {

		public void GetObjectData (object obj, SerializationInfo info, StreamingContext context) {
			Color color = (Color) obj;

			info.AddValue ("Cairo.Color.A", color.A);
			info.AddValue ("Cairo.Color.R", color.R);
			info.AddValue ("Cairo.Color.G", color.G);
			info.AddValue ("Cairo.Color.B", color.B);
		}

		public object SetObjectData (object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
			Color color = (Color) obj;

			color.A = info.GetDouble ("Cairo.Color.A");
			color.R = info.GetDouble ("Cairo.Color.R");
			color.G = info.GetDouble ("Cairo.Color.G");
			color.B = info.GetDouble ("Cairo.Color.B");

			return color;
		}

	}

}

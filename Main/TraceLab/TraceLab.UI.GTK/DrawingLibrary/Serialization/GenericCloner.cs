// TODO: Review Cloner

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MonoHotDraw.Util;

namespace MonoHotDraw.Commands {

	public sealed class GenericCloner {
		private GenericCloner () { 
		}
		
		public static T Clone<T> (T reference) where T : class, ICloneable {
			if (reference == null)
				return null;
				
			T clone = default (T);
			BinaryFormatter formater = new BinaryFormatter ();
			MemoryStream stream = new MemoryStream ();
			SurrogateSelector surrogate = new SurrogateSelector ();
			
			/* Using surrogates because Cairo structures aren't serializable */
			surrogate.AddSurrogate (typeof (Cairo.PointD),
					new StreamingContext (StreamingContextStates.All),
					new PointDSerializationSurrogate ());
			surrogate.AddSurrogate (typeof (Cairo.Color),
					new StreamingContext (StreamingContextStates.All),
					new ColorSerializationSurrogate ());

			formater.SurrogateSelector = surrogate;
			formater.Serialize (stream, reference);
			stream.Flush ();
			stream.Position = 0;
			clone = ((T) formater.Deserialize (stream));

			return (T) clone;
		}
	}
}


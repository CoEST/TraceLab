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
using MonoHotDraw.Tools;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures { 

	public interface IFigure : ICloneable, ISerializable {
	
		void MoveBy (double x, double y);
		void MoveTo (double x, double y);
		bool Includes (IFigure figure);
		void Draw (Context context);
		void DrawSelected (Context context);
		bool ContainsPoint (double x, double y);
		void Invalidate ();
		IConnector ConnectorAt (double x, double y);
		ITool CreateFigureTool (IDrawingEditor editor, ITool defaultTool);
		
		RectangleD DisplayBox { get; set; }
		IEnumerable <IFigure> FiguresEnumerator { get; }
		IEnumerable <IHandle> HandlesEnumerator { get; }
		bool CanConnect { get; }
		
		object GetAttribute (FigureAttribute attribute);
		void SetAttribute (FigureAttribute attribute, object value);
		
		void Visit (IFigureVisitor visitor);

		event EventHandler <FigureEventArgs> FigureInvalidated;
		event EventHandler <FigureEventArgs> FigureChanged;
	}
}


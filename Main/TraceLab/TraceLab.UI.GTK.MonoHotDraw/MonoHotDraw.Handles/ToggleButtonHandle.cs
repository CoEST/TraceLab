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
using Cairo;
using MonoHotDraw.Figures;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;

namespace MonoHotDraw.Handles {
	
	public class ToggleEventArgs: EventArgs {
		
		public ToggleEventArgs(bool active): base() {
			Active = active;
		}
		
		public bool Active { get; private set; }
	}
	
	public class ToggleButtonHandle: LocatorHandle {
		
		public ToggleButtonHandle(IFigure owner, ILocator locator): base(owner, locator) {
			
			Width = 15.0;
			Height = 15.0;
		}
		
		public event EventHandler<ToggleEventArgs> Toggled;
		
		public override void Draw (Cairo.Context context, IDrawingView view)
		{
			context.Save();
			base.Draw(context, view);
			
			context.LineWidth = 1.0;
			
			if (Active) {
				DrawOn(context, view);
			}
			else {
				DrawOff(context, view);
			}
			context.Restore();
		}
		
		public bool Active {
			get { return active; }
			set {
				active = value;
				OnToggled();
			}
		}
		
		public override double Width  { get; set; }
		public override double Height { get; set; }
		
		public override void InvokeStart (double x, double y, IDrawingView view)
		{
			base.InvokeStart (x, y, view);
			
			clicked = true;
		}
		
		public override void InvokeEnd (double x, double y, IDrawingView view)
		{
			base.InvokeEnd (x, y, view);
			
			if (clicked) {
				Active = !Active;
			}
			
			clicked = false;
		}

		
		protected virtual void OnToggled()
		{
			if (Toggled != null) {
				Toggled(this, new ToggleEventArgs(Active));
			}
		}
		
		protected virtual void DrawOn(Cairo.Context context, IDrawingView view)
		{
            FillColor = new Color(0.5, 0.5, 0.5, 0.3);
			RectangleD rect = ViewDisplayBox(view);
			PointD center = rect.Center;
			
			double margin = Width * 0.2;
			
			context.MoveTo(rect.Left + margin, Dot5(center.Y));
			context.LineTo(rect.Right - margin, Dot5(center.Y));
			context.Stroke();
		}
		
		protected virtual void DrawOff(Cairo.Context context, IDrawingView view)
		{
            FillColor = new Color(1, 1, 0.0, 0.3);
			RectangleD rect = ViewDisplayBox(view);
			PointD center = rect.Center;
			
			double margin_w = Width * 0.2;
			double margin_h = Height * 0.2;
			
			context.MoveTo(rect.Left + margin_w, Dot5(center.Y));
			context.LineTo(rect.Right - margin_w, Dot5(center.Y));
			context.Stroke();
			
			context.MoveTo(Dot5(center.X), rect.Top + margin_h);
			context.LineTo(Dot5(center.X), rect.Bottom - margin_h);
			context.Stroke();
		}
		
		private double Dot5(double val)
		{
			return Math.Truncate(val) + 0.5;
		}

		private bool active;
		private bool clicked = false;
	}
}

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

using System.Collections.Generic;
using Gtk;
using Gdk;
using MonoHotDraw.Util;

namespace MonoHotDraw {
	
	public class ContainerCanvas: Container {
		
		public ContainerCanvas(): base () {
			_children = new Dictionary<Widget,ContainerCanvasChild>();
			CanFocus = true;
			Color = new Cairo.Color (1, 1, 1);
			
			Hadjustment = NewDefaultAdjustment();
			Vadjustment = NewDefaultAdjustment();
		}
		
		public class ContainerCanvasChild: Container.ContainerChild {
			
			public ContainerCanvasChild (ContainerCanvas parent, Widget widget, int x, int y): 
			    base(parent, widget) {
				widget.Parent = parent;
				Move (x, y);
			}
			
			public void Move (int x, int y) {
				_x = x;
				_y = y;
				Child.QueueResize();
			}
			
			public int X {
				get { return _x; }
			}
			
			public int Y {
				get { return _y; }
			}
			
			private int _x;
			private int _y;
		}
		
		public override ContainerChild this[Widget w] {
			get {
				ContainerCanvasChild child;
				if (_children.TryGetValue(w, out child) ) {
					return child;
				}
				return null;
			}
		}
		
		public void AddWidget(Gtk.Widget w, double x, double y) {
			_children[w] = new ContainerCanvasChild(this, w, (int)x, (int)y);
		}
		
		public void MoveWidget(Gtk.Widget w, double x, double y) {
			ContainerCanvasChild child = this[w] as ContainerCanvasChild;
			
			if (child != null) {
				child.Move ((int)x, (int)y);
				QueueResize ();
			}
		}
		
		public void RemoveWidget(Gtk.Widget w) {
			if (_children.ContainsKey (w) ) {
				Remove (w);
			}
		}
		
		public void ClearWidgets () {
			foreach (Gtk.Widget  child in _children.Keys) {
				Remove (child);
			}
		}
		
		public Adjustment Hadjustment {
			get { return _hadjustment; }
			
			set {
				if (_hadjustment != value) {
					if (_hadjustment != null) {
						_hadjustment.ValueChanged -= OnAdjustmentValueChanged;
					}
					_hadjustment = value;
					if (_hadjustment != null) {
						_hadjustment.ValueChanged += OnAdjustmentValueChanged;
					}
				}
			}
		}

		public Adjustment Vadjustment {
			get { return _vadjustment; }
			
			set {
				if (_vadjustment != value) {
					if (_vadjustment != null) { 
						_vadjustment.ValueChanged -= OnAdjustmentValueChanged;
					}
					_vadjustment = value;
					if (_vadjustment != null) { 
						_vadjustment.ValueChanged += OnAdjustmentValueChanged;
					}
				}
			}
		}
		
		public Cairo.Color Color {
			set {
				Gdk.Color color = GdkCairoHelper.GdkColor (value); 
				ModifyBg (StateType.Normal, color);
			}
		}
	
		protected override void ForAll (bool include_internals, Callback callback) {
			foreach (ContainerCanvasChild  child in _children.Values) {
				callback (child.Child);
			}
		}
		
		protected override void OnAdded (Widget w) {
			AddWidget (w, 0, 0);
		}
		
		protected override void OnRemoved (Gtk.Widget w) {
			ContainerCanvasChild child = this[w] as ContainerCanvasChild;
			
			if (child != null) {
				child.Child.Unparent ();
				_children.Remove (w);
				QueueResize();
			}
		}
		
		protected override void OnSizeRequested (ref Requisition req) {
			req.Width = 0;
			req.Height = 0;
			
			foreach (ContainerCanvasChild  child in _children.Values) {
				Requisition r = child.Child.SizeRequest ();
				int x2 = child.X + r.Width;
				int y2 = child.Y + r.Height;
				
				if (req.Width < x2)
					req.Width = x2;
				if (req.Height < y2)
					req.Height = y2;
			}
		}
		
		protected override void OnSizeAllocated (Rectangle allocation) {
			base.OnSizeAllocated (allocation);
			Requisition childReq;
			
			foreach (ContainerCanvasChild child in _children.Values) {
				Gtk.Widget widget = child.Child;
				childReq = widget.ChildRequisition;
				allocation = new Rectangle();
				allocation.X = child.X;
				allocation.Width = childReq.Width;
				allocation.Y = child.Y;
				allocation.Height = childReq.Height;
				widget.Allocation = allocation;
			}
		}
		
		protected override void OnRealized () {
			SetFlag(Gtk.WidgetFlags.Realized);
			
			Gdk.WindowAttr attributes = new Gdk.WindowAttr();
			attributes.WindowType = Gdk.WindowType.Child;
			attributes.X = Allocation.X;
			attributes.Y = Allocation.Y;
			attributes.Width = Allocation.Width;
			attributes.Height = Allocation.Height;
			attributes.Wclass = WindowClass.InputOutput;
			attributes.Visual = Visual;
			attributes.Colormap = Colormap;
			
			// This enables all events except PointerMotionHitMask which prevents correct behavior
			// of MotionNotifyEvent
			attributes.EventMask = (int) (Gdk.EventMask.AllEventsMask & (~Gdk.EventMask.PointerMotionHintMask));
			
			Gdk.WindowAttributesType mask = WindowAttributesType.X | WindowAttributesType.Y
				| WindowAttributesType.Visual | WindowAttributesType.Colormap;
			
			GdkWindow = new Gdk.Window(ParentWindow, attributes, (int) mask);
			GdkWindow.UserData = Handle;
			GdkWindow.SetBackPixmap(null, false);
			
			Style = Style.Attach(GdkWindow);
			Style.SetBackground(GdkWindow, StateType.Normal);
			
			Color = new Cairo.Color(1.0, 1.0, 1.0);
		}
		
		protected override void OnSetScrollAdjustments (Adjustment hadj, Adjustment vadj) {
			Hadjustment = hadj;
			Vadjustment = vadj;
		}
		
		protected virtual void OnAdjustmentValueChanged(object sender, System.EventArgs args) {	
		}
		
		private Adjustment NewDefaultAdjustment() {
			return new Adjustment(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
		}
		
		private Dictionary<Widget, ContainerCanvasChild> _children; 
		private Gtk.Adjustment _hadjustment;
		private Gtk.Adjustment _vadjustment;
	}
}

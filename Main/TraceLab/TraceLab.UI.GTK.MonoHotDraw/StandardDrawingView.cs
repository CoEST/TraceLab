// MonoHotDraw. Diagramming Framework
//
// Authors:
//    Manuel Cerón <ceronman@gmail.com>
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

// # define DEBUG_SHOW_FPS
// # define DEBUG_SHOW_VISIBLE_AREA

using Cairo;
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoHotDraw.Commands;
using MonoHotDraw.Figures;
using MonoHotDraw.Handles;
using MonoHotDraw.Util;

namespace MonoHotDraw {

    public class StandardDrawingView : ContainerCanvas, IDrawingView {
        
        public event EventHandler VisibleAreaChanged;
        
        public StandardDrawingView (IDrawingEditor editor): base () {
            Drawing = new StandardDrawing ();
            Editor = editor;
            Scale = 1.0;
            _selection = new FigureCollection ();
                    
            DebugCreateTimer ();
        }

        public IDrawing Drawing {
            set {
                if (value == _drawing) {
                    return;
                }

                if (_drawing != null) {
                    _drawing.DrawingInvalidated -= OnDrawingInvalidated;
                    _drawing.SizeAllocated -= OnDrawingSizeAllocated;
                }

                _drawing = value;
                _drawing.DrawingInvalidated += OnDrawingInvalidated;
                _drawing.SizeAllocated += OnDrawingSizeAllocated;
            }
            get { return  _drawing; }
        }

        public IDrawingEditor Editor { get; set; }
        
        public double Scale {
            get { return _scale; }
            set { 
                if(value != _scale) 
                {
                    _scale = value;
                    QueueDraw();
                }
            }
        }

        public IEnumerable <IFigure> SelectionEnumerator {
            get {
                foreach (IFigure fig in _selection) {
                    yield return fig;
                }
            }
        }
        
        public int SelectionCount {
            get { return _selection.Count; }
        }
        
        public void Add (IFigure figure) {
            Drawing.Add (figure);
        }

        public void AddToSelection (IFigure figure) {
            if (!IsFigureSelected (figure) && Drawing.Includes (figure)) {
                _selection.Add (figure);
                figure.Invalidate ();
            }
        }
        
        public void AddToSelection (FigureCollection collection) {
            foreach (IFigure figure in collection) {
                AddToSelection (figure);
            } 
        }
        
        public void Remove (IFigure figure) {
            Drawing.Remove (figure);
        }

        public void RemoveFromSelection (IFigure figure) {
            _selection.Remove (figure);
            figure.Invalidate ();
        }

        public void ToggleSelection (IFigure figure) {
            if (IsFigureSelected (figure)) {
                RemoveFromSelection (figure);
            } else {
                AddToSelection (figure);
            }
        }

        public void ClearSelection () {
            foreach (IFigure figure in _selection) {
                figure.Invalidate ();
            }

            _selection.Clear ();
        }

        public bool IsFigureSelected (IFigure figure) {
            return _selection.Contains (figure);
        }
        
        public IHandle FindHandle (double x, double y) {
            foreach (IHandle handle in SelectionHandles) {
                if (handle.ContainsPoint (x, y)) {
                    return handle;
                }
            }
            return null;
        }
        
        public PointD ViewToDrawing (double x, double y) {
            return new PointD {
                X = (x/Scale + VisibleArea.X),
                Y = (y/Scale + VisibleArea.Y)
            };
        }
        
        public PointD DrawingToView (double x, double y) {
            return new PointD {
                    X = (x - VisibleArea.X) * Scale,
                    Y = (y - VisibleArea.Y) * Scale
            };
        }
        
        public RectangleD VisibleArea {
            get {
                return new RectangleD {
                    X = Hadjustment.Value,
                    Y = Vadjustment.Value,
                    Width = Allocation.Width / Scale,
                    Height = Allocation.Height / Scale
                };
            }
        }
        
        public void ScrollToMakeVisible (PointD point) {
            RectangleD visible = VisibleArea;
            
            if (visible.Contains(point.X, point.Y) ) {
                return;
            }
            
            Hadjustment.Lower = Math.Min (Hadjustment.Lower, point.X);
            Hadjustment.Upper = Math.Max (Hadjustment.Upper, point.X);
            Hadjustment.Change ();
            
            Vadjustment.Lower = Math.Min (Vadjustment.Lower, point.Y);            
            Vadjustment.Upper = Math.Max (Vadjustment.Upper, point.Y);
            Vadjustment.Change ();
            
            if (point.X < visible.X) {
                Hadjustment.Value = Math.Round (point.X, 0);
            }
            else if (point.X > visible.X2 ) {
                Hadjustment.Value = Math.Round (point.X - visible.Width, 0);
            }
            
            if (point.Y < visible.Y) {
                Vadjustment.Value = Math.Round (point.Y, 0);
            }
            else if (point.Y > visible.Y2) {
                Vadjustment.Value = Math.Round (point.Y - visible.Height, 0);
            }
        }
        
        public void ScrollToMakeVisible (RectangleD rect) {
            RectangleD visible = VisibleArea;
            
            if (visible.Contains(rect) ) {
                return;
            }
            
            Hadjustment.Lower = Math.Min (Hadjustment.Lower, rect.X);            
            Hadjustment.Upper = Math.Max (Hadjustment.Upper, rect.X2);
            Hadjustment.Change ();
            
            Vadjustment.Lower = Math.Min (Vadjustment.Lower, rect.Y);            
            Vadjustment.Upper = Math.Max (Vadjustment.Upper, rect.Y2);
            Vadjustment.Change ();
            
            if (rect.X < visible.X) {
                Hadjustment.Value = Math.Round (rect.X, 0);
            }
            if (rect.X2 > visible.X2 ) {
                Hadjustment.Value = Math.Round (rect.X2 - visible.Width, 0);
            }
            
            if (rect.Y < visible.Y) {
                Vadjustment.Value = Math.Round (rect.Y, 0);
            }
            if (rect.Y2 > visible.Y2) {
                Vadjustment.Value = Math.Round (rect.Y2 - visible.Height, 0);
            }
        }
        
        public void ScrollCanvas (int dx, int dy)
        {
            double hadjustment = Hadjustment.Value + dx;
            double vadjustment = Vadjustment.Value + dy;
            Hadjustment.Lower = Math.Min (Hadjustment.Lower, hadjustment);
            Hadjustment.Upper = Math.Max (Hadjustment.Upper, hadjustment);
            Hadjustment.Change ();
            
            Vadjustment.Lower = Math.Min (Vadjustment.Lower, vadjustment);          
            Vadjustment.Upper = Math.Max (Vadjustment.Upper, vadjustment);
            Vadjustment.Change ();
            
            Hadjustment.Value = hadjustment;
            Vadjustment.Value = vadjustment;
        }

        public double ZoomToFit() 
        {
            this.Drawing.RecalculateDisplayBox();
            RectangleD displayBox = this.Drawing.DisplayBox;
            RectangleD viewport = new RectangleD {
                X = Allocation.X,
                Y = Allocation.Y,
                Width = Allocation.Width,
                Height = Allocation.Height
            };

            //The maximum scale width we could use
            double maxWidthScale = viewport.Width / displayBox.Width; 
            
            //The maximum scale height we could use
            double maxHeightScale = viewport.Height / displayBox.Height; 
            
            //Use the smaller of the 2 scales for the scaling
            double scale = Math.Min(maxHeightScale, maxWidthScale); 

            Scale = scale;

            ScrollToMakeVisible(displayBox);

            return scale;
        }

        protected IEnumerable <IHandle> SelectionHandles {
            get {
                foreach (IFigure figure in SelectionEnumerator) {
                    foreach (IHandle handle in figure.HandlesEnumerator) {
                        yield return handle;
                    }
                }
            }
        }    
        
        public FigureCollection InsertFigures (FigureCollection figures, double dx, double dy, bool check) {
            InsertIntoDrawingVisitor visitor = new InsertIntoDrawingVisitor (Drawing);
            foreach (IFigure figure in figures) {    
                figure.MoveBy (dx, dy);
                visitor.VisitFigure (figure);
            }
            AddToSelection (visitor.GetAddedFigures ());
            //TODO: Use check parameter
            return visitor.GetAddedFigures ();
        }
    
        protected override bool OnExposeEvent (EventExpose ev) {
            using (Cairo.Context context = CairoHelper.Create (ev.Window)) {
                
                context.Save();
                
                PointD translation = DrawingToView(0.0,  0.0);
                context.Translate (translation.X, translation.Y);
                context.Scale(Scale, Scale);
                
                foreach (IFigure figure in Drawing.FiguresEnumerator) {
                    // check region for update
                    bool shouldDraw = true;
                    if (shouldDraw) {
                        figure.Draw (context);
                    }
                }
                foreach (IFigure figure in SelectionEnumerator) {                    
                    figure.DrawSelected (context);
                }
                
                context.Restore();
                
                foreach (IHandle handle in SelectionHandles) {
                    handle.Draw (context, this);
                }
            }
            
            DebugUpdateFrame ();
            return base.OnExposeEvent(ev);
        }
        
        protected override bool OnMotionNotifyEvent (EventMotion gdk_event) {
            PointD point = ViewToDrawing(gdk_event.X, gdk_event.Y);
            MouseEvent ev = new MouseEvent(this, gdk_event, point);

            if (_drag) {
                // TODO: Move this to a Tool
                ScrollToMakeVisible (point); 
                Editor.Tool.MouseDrag (ev);
            } else {
                Editor.Tool.MouseMove (ev);
            }

            return base.OnMotionNotifyEvent(gdk_event);
        }

        protected override bool OnButtonReleaseEvent (EventButton gdk_event) {
            Drawing.RecalculateDisplayBox();
            PointD point = ViewToDrawing(gdk_event.X, gdk_event.Y);
            MouseEvent ev = new MouseEvent(this, gdk_event, point);
            Editor.Tool.MouseUp (ev);
            _drag = false;
            return base.OnButtonReleaseEvent(gdk_event);
        }

        protected override bool OnButtonPressEvent (Gdk.EventButton gdk_event) {
            PointD point = ViewToDrawing(gdk_event.X, gdk_event.Y);
            MouseEvent ev = new MouseEvent(this, gdk_event, point);
            Editor.Tool.MouseDown (ev);
            _drag = true;

            return base.OnButtonPressEvent(gdk_event);
        }

        protected override bool OnKeyPressEvent (Gdk.EventKey ev) {
            Editor.Tool.KeyDown (new KeyEvent(this, ev));
            return base.OnKeyPressEvent(ev);
        }
        
        protected override bool OnKeyReleaseEvent (Gdk.EventKey ev) {
            Editor.Tool.KeyUp (new KeyEvent(this, ev));
            return base.OnKeyReleaseEvent(ev);
        }
        
        protected override void OnSizeAllocated (Gdk.Rectangle allocation) {
            base.OnSizeAllocated (allocation);
            
            UpdateAdjustments ();
            OnVisibleAreaChaned();
        }

        protected override void OnAdjustmentValueChanged (object sender, EventArgs args) {
            QueueDraw();
            OnVisibleAreaChaned();
        }

        
        protected void OnDrawingInvalidated (object sender, DrawingEventArgs args) {
            RectangleD r = args.Rectangle;
            PointD p = DrawingToView(r.X, r.Y);
            r.X = p.X;
            r.Y = p.Y;
            r.Width = r.Width * Scale;
            r.Height = r.Height * Scale;
            QueueDrawArea ((int) r.X, (int) r.Y, (int) r.Width, (int) r.Height);
        }
        
        protected void OnDrawingSizeAllocated (object sender, DrawingEventArgs args) {
            UpdateAdjustments ();
            QueueDraw();
        }
        
        protected void OnVisibleAreaChaned() {
            if (VisibleAreaChanged != null) {
                VisibleAreaChanged(this, new EventArgs());
            }
        }
        
        private void UpdateAdjustments () {
            RectangleD drawing_box = Drawing.DisplayBox;
            drawing_box.Add (VisibleArea);
            
            Hadjustment.PageSize = Allocation.Width;
            Hadjustment.PageIncrement = Allocation.Width * 0.9;
            Hadjustment.StepIncrement = 1.0;
            Hadjustment.Lower = drawing_box.X;
            Hadjustment.Upper = drawing_box.X2;
            Hadjustment.Change ();
            
            Vadjustment.PageSize = Allocation.Height;
            Vadjustment.PageIncrement = Allocation.Height * 0.9;
            Vadjustment.StepIncrement = 1.0;
            Vadjustment.Lower = drawing_box.Y;
            Vadjustment.Upper = drawing_box.Y2;
            Vadjustment.Change ();
        }
        
        [ConditionalAttribute ("DEBUG_SHOW_FPS")]
        private void DebugCreateTimer () {
            GLib.Timeout.Add (1000, delegate() {
                System.Console.WriteLine ("FPS: {0}", _frameCount.ToString());
                _frameCount = 0;
                return true;
            });
        }
        
        [ConditionalAttribute ("DEBUG_SHOW_FPS")]
        private void DebugUpdateFrame () {
            _frameCount ++;
        }
        
        private bool _drag;        
        private IDrawing _drawing;
        private FigureCollection _selection;
        
        // used for debug purposes
        private int _frameCount = 0;
        private double _scale = 1.0;
    }
}









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

using System;
using Gdk;
using Cairo;
using MonoHotDraw.Figures;
using MonoHotDraw.Commands;

namespace MonoHotDraw.Tools {

    public class DragTool: AbstractTool {
    
        public DragTool (IDrawingEditor editor, IFigure anchor): base (editor) {
            AnchorFigure = anchor;
        }

        public IFigure AnchorFigure { get; set; }
        protected double LastX { get; set; }
        protected double LastY { get; set; }
        public bool HasMoved { get; protected set; }

        public event EventHandler DragCompleted;

        private void OnDragCompleted()
        {
            if(DragCompleted != null)
                DragCompleted(this, EventArgs.Empty);
        }
        
        protected void SetLastCoords (double x, double y) {
            LastX = x;
            LastY = y;
        }

        public override void MouseDown (MouseEvent ev) {
            base.MouseDown (ev);
            IDrawingView view = ev.View;
            
            SetLastCoords (ev.X, ev.Y);
            
            Gdk.ModifierType state = (ev.GdkEvent as EventButton).State;

            bool shift_pressed = (state & ModifierType.ShiftMask) != 0;

            if (shift_pressed) {
                view.ToggleSelection (AnchorFigure);
            }
                
            else if (!view.IsFigureSelected (AnchorFigure)) {
                view.ClearSelection ();
                view.AddToSelection (AnchorFigure);
            }
            CreateUndoActivity();
        }

        public override void MouseDrag (MouseEvent ev) {
            HasMoved = (Math.Abs (ev.X - AnchorX) > 4 || Math.Abs (ev.Y - AnchorX) > 4);

            if (HasMoved) {
                foreach (IFigure figure in ev.View.SelectionEnumerator) {
                    figure.MoveBy (ev.X - LastX, ev.Y - LastY);
                }
            }
            SetLastCoords (ev.X, ev.Y);
        }
        
        public override void MouseUp (MouseEvent ev) 
        {
            OnDragCompleted();
            UpdateUndoActivity();
            PushUndoActivity();
        }
        
        public class DragToolUndoActivity: AbstractUndoActivity {
            public DragToolUndoActivity(IDrawingView drawingView): base (drawingView) {
                Undoable = true;
                Redoable = true;
            }
            
            public override bool Undo () {
                if (!base.Undo()  )
                    return false;
                
                double deltaX = StartPoint.X - EndPoint.X;
                double deltaY = StartPoint.Y - EndPoint.Y;
            
                foreach (IFigure figure in AffectedFigures) {
                    figure.MoveBy(deltaX, deltaY);
                }
                return true;
            }
            
            public override bool Redo () {
                if (!base.Redo() )
                    return false;
                
                double deltaX = EndPoint.X - StartPoint.X;
                double deltaY = EndPoint.Y - StartPoint.Y;
                
                foreach (IFigure figure in AffectedFigures) {
                    figure.MoveBy(deltaX, deltaY);
                }
                return true;
            }
            
            public PointD StartPoint { get; set; }
            public PointD EndPoint { get; set; } 
        }
        
        protected void CreateUndoActivity() {
            IDrawingView view = Editor.View;
            DragToolUndoActivity activity = new DragToolUndoActivity(view);
            activity.AffectedFigures = view.SelectionEnumerator.ToFigures();
            activity.StartPoint = new PointD(AnchorX, AnchorY);
            UndoActivity = activity;
        }
        
        protected void UpdateUndoActivity() {
            if (HasMoved){
                DragToolUndoActivity activity = UndoActivity as DragToolUndoActivity;
                activity.EndPoint = new PointD(LastX, LastY);
            }
            
            else {
                UndoActivity = null;
            }
        }
    }
}

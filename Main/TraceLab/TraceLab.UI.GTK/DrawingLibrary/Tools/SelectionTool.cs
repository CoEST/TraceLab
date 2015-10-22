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

using Cairo;
using Gdk;
using Gtk;
using System.Collections.Generic;
using MonoHotDraw.Figures;
using MonoHotDraw.Commands;
using MonoHotDraw.Handles;
using MonoHotDraw.Util;

namespace MonoHotDraw.Tools {

    public class SelectionTool: AbstractTool, IPrimaryToolDelegator {
    
        public SelectionTool (IDrawingEditor editor): base (editor) {
        }

        // HERZUM SPRINT 2.0 TLAB-136-2
        // HERZUM SPRINT 2.5 TLAB-136
        // static double x = -1;
        // static double y = -1;
        static bool mouseDown = false;
        static bool mouseUp = true;
        // END // HERZUM SPRINT 2.5 TLAB-136
        // END HERZUM SPRINT 2.0 TLAB-136-2

        public override void MouseDown(MouseEvent ev)
        {
            base.MouseDown(ev);
            IDrawingView view = ev.View;

            // HERZUM SPRINT 2.5 TLAB-136
            if (!mouseUp){
                return;
            }
            else
            {
                mouseUp = false;
                mouseDown = true;
            }
            // HERZUM SPRINT 2.5 TLAB-136

            // HERZUM SPRINT 2.0 TLAB-136-2
            /*
            if (x!=-1 && (x != ev.X || y != ev.Y)){
                x = ev.X;
                y = ev.Y;
            } 
            else if (x == ev.X && y == ev.Y)
                return;
            else {
                x = ev.X;
                y = ev.Y;
            }
            */
            // END HERZUM SPRINT 2.5 TLAB-136
            // END HERZUM SPRINT 2.2 TLAB-136-2

            IFigure figure = view.Drawing.FindFigure(ev.X, ev.Y);

            if(ev.IsRightButtonPressed())
            {
                ITool defaultTool = new PanTool(Editor, CursorFactory.GetCursorFromType(Gdk.CursorType.Arrow));
                if (figure != null) 
                {
                    DelegateTool = figure.CreateFigureTool(this, Editor, defaultTool, ev);
                }
                else
                {
                    //if it didn't hit figure then allow panning
                    DelegateTool = defaultTool;
                }
            } 
            else
            {
                IHandle handle = view.FindHandle(ev.X, ev.Y);
                if (handle != null) 
                {
                    DelegateTool = new HandleTracker(Editor, new UndoableHandle(handle));
                }
                else 
                {
                    if(figure != null) 
                    {
                        ITool dragTool = new DragTool(Editor, figure);
                        DelegateTool = figure.CreateFigureTool(this, Editor, dragTool, ev);
                    } 
                    else
                    {
                        DelegateTool = new SelectAreaTool(Editor);
                    }
                }
            }

            if (DelegateTool != null) 
            {
                DelegateTool.MouseDown (ev);
            }
        }

        // HERZUM SPRINT 2.0 TLAB-136-2
        // HERZUM SPRINT 2.5 TLAB-136
        // static double x2 = -1;
        // static double y2 = -1;
        // END HERZUM SPRINT 2.5 TLAB-136
        // END HERZUM SPRINT 2.0 TLAB-136-2

        public override void MouseUp (MouseEvent ev) {

            // HERZUM SPRINT 2.5 TLAB-136
            if (!mouseDown){
                return;
            }
            else
            {
                mouseUp = true;
                mouseDown = false;
            }

            // END HERZUM SPRINT 2.5 TLAB-136

            // HERZUM SPRINT 2.0 TLAB-136-2
            // HERZUM SPRINT 2.5 TLAB-136
            /*
            if (x2!=-1 && (x2 != ev.X || y2 != ev.Y)){
                x2 = ev.X;
                y2 = ev.Y;
            } 
            else if (x2 == ev.X && y2 == ev.Y)
                return;
            else {
                x2 = ev.X;
                y2 = ev.Y;
            }
            */
            // END HERZUM SPRINT 2.5 TLAB-136
            // END HERZUM SPRINT 2.2 TLAB-136-2

            if (DelegateTool != null) {
                DelegateTool.MouseUp (ev);
            }
        }
        
        public override void MouseDrag (MouseEvent ev) {
            if (DelegateTool != null) {
                DelegateTool.MouseDrag (ev);
            }
        }
        
        public override void MouseMove (MouseEvent ev) {
            IDrawingView view = ev.View;
            Widget widget = (Widget) view;
            IHandle handle = view.FindHandle (ev.X, ev.Y);
            if (handle != null) {
                widget.GdkWindow.Cursor = handle.CreateCursor ();
            }
            else {
                IFigure figure = view.Drawing.FindFigure (ev.X, ev.Y);
                if (figure != null) {
                    widget.GdkWindow.Cursor = CursorFactory.GetCursorFromType (Gdk.CursorType.Fleur);
                }
                else { 
                    widget.GdkWindow.Cursor = null;
                }
            }
            
            if (DelegateTool != null) {
                DelegateTool.MouseMove (ev);
            }
        }
        
        public override void KeyDown (KeyEvent ev) {
            if (DelegateTool != null) {
                DelegateTool.KeyDown (ev);
            }
            if (ev.Key == Gdk.Key.Delete) {
                DeleteFigures (ev.View);
            }
        }

        public override void KeyUp (KeyEvent ev) {
            if (DelegateTool != null) {
                DelegateTool.KeyUp (ev);
            }
        }
        
        public ITool DelegateTool {
            set { 
                if (_delegateTool != null && _delegateTool.Activated) {
                    _delegateTool.Deactivate ();
                }

                _delegateTool = value;
                if (_delegateTool != null) {
                    _delegateTool.Activate ();
                }
            }
            get { return _delegateTool; }
        }
        
        private void DeleteFigures (IDrawingView view) {
            List <IFigure> figures = new List <IFigure> ();
            
            foreach (IFigure fig in view.SelectionEnumerator) {
                figures.Add (fig);
            }
            
            view.ClearSelection ();
            
            foreach (IFigure fig in figures) {
                view.Drawing.Remove (fig);
            }
        }
                
        private ITool _delegateTool;
    }
}

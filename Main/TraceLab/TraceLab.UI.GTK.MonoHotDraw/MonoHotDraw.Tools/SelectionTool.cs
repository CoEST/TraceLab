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

    public class SelectionTool: AbstractTool {
    
        public SelectionTool (IDrawingEditor editor): base (editor) {
        }

        public override void MouseDown (MouseEvent ev) {
            base.MouseDown (ev);
            IDrawingView view = ev.View;
            
            if(IsRightButtonPressed (ev))
            {
                DelegateTool = new PanTool(Editor, CursorFactory.GetCursorFromType(Gdk.CursorType.Arrow));
            } 
            else
            {
                IHandle handle = view.FindHandle (ev.X, ev.Y);
                if (handle != null) {
                    DelegateTool = new HandleTracker (Editor, new UndoableHandle(handle));
                }
                else {
                    IFigure figure = view.Drawing.FindFigure (ev.X, ev.Y);
                    if (figure != null) {
                        DelegateTool = figure.CreateFigureTool (Editor, new DragTool (Editor, figure));
                    } else {
                        DelegateTool = new SelectAreaTool (Editor);
                    }
                }
            }

            if (DelegateTool != null) {
                DelegateTool.MouseDown (ev);
            }

        }

        private static bool IsRightButtonPressed (MouseEvent ev)
        {
            Gdk.EventButton mouseButtonEvent = ev.GdkEvent as Gdk.EventButton;
            if (mouseButtonEvent != null) {
                if(mouseButtonEvent.Button == 3)
                {
                    return true;
                }
            }
            return false;
        }
        
        public override void MouseUp (MouseEvent ev) {
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
        
        protected ITool DelegateTool {
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

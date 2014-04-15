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
using Gtk;

using MonoHotDraw.Commands;
using MonoHotDraw.Tools;
using MonoHotDraw.Figures;

namespace MonoHotDraw
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SteticComponent : Gtk.Bin, IDrawingEditor
    {
        public SteticComponent()
        {
            this.Build();
            
            View = new StandardDrawingView (this);
            
            this.Add((Widget)View);

            m_selectionTool = new SelectionTool(this);
            SetSelectionTool();
            UndoManager = new UndoManager();
            UndoManager.StackChanged += delegate {
                OnUndoStackChanged();
            };
        }
        
        private SelectionTool m_selectionTool;

        public event EventHandler UndoStackChanged;
        
        public IDrawingView View { get; set; }
        public UndoManager UndoManager { get; private set; }
        
        public void SetSelectionTool() 
        {
            Tool = m_selectionTool;
        }

        public void SetPanTool() 
        {
            Tool = new PanTool(this, CustomCursorFactory.OpenHandGrabCursor);
        }

        public ITool Tool {
            get { return _tool; }
            set {
                if (_tool != null && _tool.Activated) {
                    _tool.Deactivate();
                }
                
                _tool = value;
                if (value != null) {
                    _tool.Activate();
                }
            }
        }
        
        public void Undo() {
            ICommand command = new UndoCommand("Undo", this);
            command.Execute();
        }
        
        public void Redo() {
            ICommand command = new RedoCommand("Redo", this);
            command.Execute();
        }
        
        public void AddWithDragging(IFigure figure) {
            Tool = new DragCreationTool(this, figure);
        }
        
        public void AddWithResizing(IFigure figure) {
            Tool = new ResizeCreationTool(this, figure);
        }
        
        public void AddConnection(IConnectionFigure figure) {
            Tool = new ConnectionCreationTool(this, figure);
        }
        
        protected void OnUndoStackChanged() {
            if (UndoStackChanged != null) {
                UndoStackChanged(this, new EventArgs());
            }
        }
        
        private ITool _tool;
    }
}

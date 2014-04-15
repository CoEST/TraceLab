// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.
//

using Cairo;
using Gdk;
using System.Collections.Generic;
using MonoHotDraw.Figures;
using MonoHotDraw.Commands;
using MonoHotDraw.Handles;
using MonoHotDraw.Util;
using MonoHotDraw;

namespace MonoHotDraw.Tools {
    
    public class PanTool: AbstractTool {

        private bool active;
        private PointD last_point;
        private Cursor m_defaultCursor;

        public PanTool(IDrawingEditor editor, Cursor defaultCursor): base (editor) 
        {
            m_defaultCursor = defaultCursor;
            Gtk.Widget widget = (Gtk.Widget) editor.View;
            widget.GdkWindow.Cursor = m_defaultCursor;
        }

        public override void MouseDown (MouseEvent ev) 
        {
            base.MouseDown(ev);

            // If we are already panning, ignore any additional mouse down events
            if (active)
                return;

            active = true;

            Gdk.EventButton eventButton = ev.GdkEvent as Gdk.EventButton;

            last_point = new PointD (eventButton.XRoot, eventButton.YRoot);

            Gtk.Widget widget = (Gtk.Widget) ev.View;

            widget.GdkWindow.Cursor = CustomCursorFactory.GrabbingHandCursor;
        }
        
        public override void MouseUp (MouseEvent ev) 
        {
            active = false;
            Gtk.Widget widget = (Gtk.Widget) ev.View;
            widget.GdkWindow.Cursor = m_defaultCursor;
        }
        
        public override void MouseDrag (MouseEvent ev) 
        {
            if(active) 
            {
                Gdk.EventMotion motion = ev.GdkEvent as Gdk.EventMotion;
                View.ScrollCanvas((int)(last_point.X - motion.XRoot), (int)(last_point.Y - motion.YRoot));
                last_point = new PointD (motion.XRoot, motion.YRoot);
            }
        }
    }
}

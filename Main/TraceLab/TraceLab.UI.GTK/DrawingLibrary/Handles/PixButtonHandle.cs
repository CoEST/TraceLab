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
using System;
using MonoHotDraw.Figures;
using MonoHotDraw.Locators;

//HERZUM SPRINT 5.5 TLAB-253
using Cairo;
using Gdk;
using MonoHotDraw.Util;
//END HERZUM SPRINT 5.5 TLAB-253

namespace MonoHotDraw.Handles
{
    public class PixButtonHandle : PixbufHandle
    {
        private bool clicked = false;
        private Action m_action;
        //HERZUM SPRINT 5.5 TLAB-253
        private int m_xAnchor, m_yAnchor;
        private double m_valueZoom=1;
        private double m_offsetPanX=0, m_offsetPanY=0;
        private bool is_resize=false;
        //END HERZUM SPRINT 5.5 TLAB-253

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoHotDraw.Handles.PixButtonHandle"/> class.
        /// Takes action to be invoked when button is clicked
        /// </summary>
        /// <param name="owner">Owner.</param>
        /// <param name="locator">Locator.</param>
        /// <param name="pixbuf">Pixbuf.</param>
        /// <param name="action">Action.</param>

        public PixButtonHandle(IFigure owner, ILocator locator, Gdk.Pixbuf pixbuf, Action action)
            : base (owner, locator, pixbuf) 
        {
            m_action = action;
        }

        public PixButtonHandle(IFigure owner, ILocator locator, Gdk.Pixbuf pixbuf, Action action, int xAnchor, int yAnchor, double valueZoom, double offsetPanX, double offsetPanY)
            : base (owner, locator, pixbuf) 
        {
            m_action = action;
            m_xAnchor = xAnchor;
            m_yAnchor = yAnchor;
            m_valueZoom = valueZoom;
            m_offsetPanX = offsetPanX;
            m_offsetPanY = offsetPanY;
            is_resize = true;

        }

        public override void InvokeStart (MouseEvent ev)
        {
            base.InvokeStart (ev);
            
            clicked = true;
        }
        
        public override void InvokeEnd (MouseEvent ev)
        {
            base.InvokeEnd (ev);

            if (clicked) 
            {
                m_action();
            }
   
            clicked = false;
            is_resize = false;
        }

        //HERZUM SPRINT 5.5 TLAB-253
        public override void InvokeStep (MouseEvent ev)
        {
            if (is_resize)
            {
                DrawSelectionRect ((Gtk.Widget) ev.View, ev.GdkEvent.Window);
                PointD anchor = new PointD ((m_xAnchor+m_offsetPanX)*m_valueZoom, (m_yAnchor+m_offsetPanY)*m_valueZoom);
                PointD corner = new PointD ((ev.X+m_offsetPanX)*m_valueZoom, (ev.Y+m_offsetPanY)*m_valueZoom);
                _selectionRect = new RectangleD (anchor, corner);
                DrawSelectionRect ((Gtk.Widget) ev.View, ev.GdkEvent.Window);
            }
        }

        private RectangleD _selectionRect;

        private void DrawSelectionRect (Gtk.Widget widget, Gdk.Window window) {
            Gdk.GC gc = (widget.Style.WhiteGC);
            gc.SetLineAttributes (1,LineStyle.OnOffDash, CapStyle.Butt, JoinStyle.Miter);
            gc.Function = Function.Xor;
            _selectionRect.Normalize ();
                      
            Gdk.Point[] points = new Gdk.Point[4];
            points [0] = new Gdk.Point((int)_selectionRect.X, (int)_selectionRect.Y);
            points [1] = new Gdk.Point((int)_selectionRect.X2, (int)_selectionRect.Y);
            points [2] = new Gdk.Point((int)_selectionRect.X2, (int)_selectionRect.Y2);
            points [3] = new Gdk.Point((int)_selectionRect.X, (int)_selectionRect.Y2);

            window.DrawPolygon(gc, false, points);
        }
        //END HERZUM SPRINT 5.5 TLAB-253 

    }
}


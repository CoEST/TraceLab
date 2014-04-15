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
using Gtk;
using Gdk;

namespace TraceLab.UI.GTK
{
    public class GtkMenuHelper
    {
        /// <summary> Draw the image to the image menu item. </summary>
        ///  The event source. <see cref="System.Object"/> 
        ///  The event args. <see cref="Gtk.ExposeEventArgs"/> 
        public static void DrawImageMenuItemImage(object o, Gtk.ExposeEventArgs args) 
        { 
            if (o as Gtk.ImageMenuItem == null) 
                return; 
            
            Gtk.Image image = (o as Gtk.ImageMenuItem).Image as Gtk.Image; 
            if (image == null || image.Pixbuf == null) 
                return; 
            
            Gdk.GC mainGC = ((Gtk.Widget)o).Style.ForegroundGCs[(int)Gtk.StateType.Normal]; 
            Gdk.Rectangle r = args.Event.Area; 
            
            args.Event.Window.DrawPixbuf(mainGC, image.Pixbuf, 0, 0, r.Left + 2, 
                                         r.Top + (r.Height - image.Pixbuf.Height) / 2, -1, -1, Gdk.RgbDither.None, 0, 0); 
            
        }
    }
}


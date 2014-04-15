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
using Cairo;
using MonoHotDraw.Figures;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;
using MonoHotDraw.Handles;
using MonoHotDraw;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.GTK
{
    public class PixToggleButtonHandle: ToggleButtonHandle
    {
        private ImageSurface m_imageOff;
        private ImageSurface m_imageOn;
        private bool clicked = false;

        public PixToggleButtonHandle(IComponentControl owner, ILocator locator, Gdk.Pixbuf iconOff, Gdk.Pixbuf iconOn)
            : base (owner, locator) 
        {
            m_imageOff = GdkCairoHelper.PixbufToImageSurface(iconOff);
            m_imageOn = GdkCairoHelper.PixbufToImageSurface(iconOn);
        }
                
        public override double Width 
        {
            get 
            {
                return m_imageOff.Width;
            }
            set { }
        }
        
        public override double Height 
        {
            get 
            {
                return m_imageOff.Height;
            }
            set { }
        }

        public override void Draw (Cairo.Context context, IDrawingView view)
        {
            context.Save();            
                        
            if (Active) 
            {
                DrawOn(context, view);
            }
            else 
            {
                DrawOff(context, view);
            }

            context.Restore();
        }

        protected override void DrawOn(Cairo.Context context, IDrawingView view)
        {
            RectangleD r = ViewDisplayBox(view);
            m_imageOn.Show(context, Math.Round (r.X), Math.Round (r.Y));
        }
        
        protected override void DrawOff(Cairo.Context context, IDrawingView view)
        {
            RectangleD r = ViewDisplayBox(view);
            m_imageOff.Show(context, Math.Round (r.X), Math.Round (r.Y));
        }
    }
}


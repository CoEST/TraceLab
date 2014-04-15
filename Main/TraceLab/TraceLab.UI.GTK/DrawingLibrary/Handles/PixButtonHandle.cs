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

namespace MonoHotDraw.Handles
{
    public class PixButtonHandle : PixbufHandle
    {
        private bool clicked = false;
        private Action m_action;

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
        }
    }
}


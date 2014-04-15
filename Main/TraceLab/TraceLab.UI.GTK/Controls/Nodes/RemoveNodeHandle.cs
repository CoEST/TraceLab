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
    public class RemoveNodeHandle: PixbufHandle
    {
        private static Gdk.Pixbuf s_trashIcon;
        private bool clicked = false;

        static RemoveNodeHandle() 
        {
            s_trashIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.trashBin.png");
        }

        public RemoveNodeHandle(IComponentControl owner, ILocator locator): base (owner, locator, s_trashIcon) 
        {
            m_ownerComponent = owner;
        }

        private IComponentControl m_ownerComponent;

        public override void InvokeStart (double x, double y, IDrawingView view)
        {
            base.InvokeStart (x, y, view);
            
            clicked = true;
        }
        
        public override void InvokeEnd (double x, double y, IDrawingView view)
        {
            base.InvokeEnd (x, y, view);
            
            if (clicked) 
            {
                IEditableExperiment experiment = m_ownerComponent.ExperimentNode.Owner as IEditableExperiment;
                if(experiment != null) 
                {
                    experiment.RemoveVertex(m_ownerComponent.ExperimentNode);
                }
            }
            
            clicked = false;
        }
    }
}


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
using System.Collections.Generic;
using MonoDevelop.Components.Docking;

namespace TraceLab.UI.GTK
{
    /// <summary>
    /// Info panel factory - helper class for Main Window responsible for managing
    /// and creating component info panels in the given Dock Frame.
    /// </summary>
    internal class InfoPanelFactory
    {
        private DockFrame m_mainWindowDockFrame;
        private ApplicationContext m_applicationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.InfoPanelFactory"/> class.
        /// </summary>
        /// <param name="dockFrame">Reference to main window dock frame.</param>
        internal InfoPanelFactory(ApplicationContext applicationContext, DockFrame dockFrame)
        {
            m_mainWindowDockFrame = dockFrame;
            m_applicationContext = applicationContext;
        }

        private Gtk.Widget CreateInfoWidget(BasicNodeControl basicComponentControl)
        {
            //case 1: Component Panel
            ComponentControl componentControl = basicComponentControl as ComponentControl;
            if(componentControl != null) 
            {
                ComponentInfoPanel panel = new ComponentInfoPanel();
                panel.Component = componentControl;
                return panel;
            } 
            else 
            {
                //case 2: decision panel
                DecisionNodeControl decisionControl = basicComponentControl as DecisionNodeControl;
                if(decisionControl != null) 
                {
                    DecisionInfoPanel panel = new DecisionInfoPanel(m_applicationContext);
                    panel.DecisionControl = decisionControl;
                    return panel;
                }
            }

            //invalid
            Gtk.Label errorLabel = new Gtk.Label("Not implemented. Panels not supported for the given component control.");
            return errorLabel;
        }

        internal void ShowComponentInfoPad(BasicNodeControl component) 
        {   
            DockItem infoDockItem = m_mainWindowDockFrame.GetItem(component.ExperimentNode.ID);
            if(infoDockItem == null) 
            {
                infoDockItem = m_mainWindowDockFrame.AddItem(component.ExperimentNode.ID);
                infoDockItem.Content = CreateInfoWidget(component);
            }
            
            infoDockItem.Label = component.ExperimentNode.Data.Metadata.Label;
            infoDockItem.DefaultHeight = 150;
            infoDockItem.DefaultWidth = 200;
            
            infoDockItem.DefaultLocation = GetLocation();
            
            m_mainWindowDockFrame.SetVisible(infoDockItem, true);
            
            infoPads.AddLast(infoDockItem);

            //TODO: floating info panels
            //there is still problem with below solution - if user resizes the window it frequently crashes (not always though)
            //however it crashes only when running from MONO Develop with attached debugger
            
            //            //this line allows setting window as floating automatically, to consider in future
            //            m_dockFrame.SetStatus(infoDockItem, DockItemStatus.Floating);
            //            Gdk.Rectangle floatRectangle = infoDockItem.FloatingPosition;
            //            floatRectangle.Width = 350;
            //            floatRectangle.Height = 180;
            //
            //            // TODO set location of info box next to the component node
            //            // to do this probably some translation will be needed of componentControl.DisplayBox on experiment canvas
            //            // to absolute x and y in relation to window
            //            floatRectangle.X = 600;
            //            floatRectangle.Y = 600;
            //            infoDockItem.SetFloatMode(floatRectangle);
        }
                
        private string GetLocation() 
        {
            string location = "ExperimentPad/Right";
            
            //check if there is any non floating info pad and add it next to it
            LinkedListNode<DockItem> n = infoPads.Last;
            while(n != null)
            {
                if(n.Value.Status == DockItemStatus.Dockable) 
                {
                    location = n.Value.Id + "/Bottom";
                    break;
                }
                
                n = n.Previous;
            }
            
            return location;
        }
        
        private LinkedList<DockItem> infoPads = new LinkedList<DockItem>(); 
    }
}


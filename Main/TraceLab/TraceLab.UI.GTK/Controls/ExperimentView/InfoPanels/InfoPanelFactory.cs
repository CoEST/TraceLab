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
using Gtk;

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
            //case 1: decision panel
            //must be check first, as it inherits from ComponentControl
            DecisionNodeControl decisionControl = basicComponentControl as DecisionNodeControl;
            if(decisionControl != null) 
            {
                DecisionInfoPanel panel = new DecisionInfoPanel(m_applicationContext);
                panel.DecisionControl = decisionControl;
                return panel;
            }
            else 
            {
                //case 2: Component Panel
                if(basicComponentControl is ComponentControl || basicComponentControl is CompositeComponentControl) 
                {
                    ComponentInfoPanel panel = new ComponentInfoPanel();
                    panel.Component = basicComponentControl;
                    return panel;
                }
            }

            //invalid
            Gtk.Label errorLabel = new Gtk.Label("Not implemented. Panels not supported for the given component control.");
            return errorLabel;
        }

        /// <summary>
        /// Shows the component info pad.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="defaultLocationX">Default location x for the floating window</param>
        /// <param name="defaultLocationY">Default location y for the floating window</param>
        /// <param name="onVisibleChanged">The action that is executed when visibility of window changes.</param>
        internal void ShowComponentInfoPad(BasicNodeControl component, 
                                           int defaultLocationX, int defaultLocationY, System.Action<Boolean> onVisibleChanged) 
        {   
            DockItem infoDockItem = m_mainWindowDockFrame.GetItem(component.ExperimentNode.ID);
            if(infoDockItem == null) 
            {
                infoDockItem = m_mainWindowDockFrame.AddItem(component.ExperimentNode.ID);
                infoDockItem.Content = CreateInfoWidget(component);

                infoDockItem.Label = component.ExperimentNode.Data.Metadata.Label;
                infoDockItem.DefaultHeight = 150;
                infoDockItem.DefaultWidth = 200;
                
                infoDockItem.DefaultLocation = GetLocation();

                infoDockItem.Visible = true;

                //attach action on visible changed, so that if window is closed then it toggles off the info icon
                infoDockItem.VisibleChanged += (object sender, EventArgs e) => 
                {
                    bool isVisible = ((DockItem)sender).Visible;
                    onVisibleChanged(isVisible);
                };

                AttachMouseOverHighlightHandlers (component, infoDockItem);

                infoPads.AddLast(infoDockItem);

                //Float window
                //this line allows setting window as floating automatically, to consider in future
                m_mainWindowDockFrame.SetStatus(infoDockItem, DockItemStatus.Floating);
                Gdk.Rectangle floatRectangle = infoDockItem.FloatingPosition;
                floatRectangle.Width = 350;
                floatRectangle.Height = 180;
                
                //location of info box next to the component node just sligthly below cursor click
                floatRectangle.X = defaultLocationX;
                floatRectangle.Y = defaultLocationY + 20;
                infoDockItem.SetFloatMode(floatRectangle);
            }
            else
            {
                //if already exists just set it visible
                infoDockItem.Visible = true;
            }
        }

        /// <summary>
        /// Attaches the handlers to mouse over and mouse leave event
        /// If mouse is over the panel it highlights the corresponding control.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="infoDockItem">Info dock item.</param>
        static void AttachMouseOverHighlightHandlers (BasicNodeControl component, DockItem infoDockItem)
        {
            var enter = new Action<object, EnterNotifyEventArgs>(
                delegate (object o, Gtk.EnterNotifyEventArgs args) {
                    component.IsHighlighted = true;
            });
            var leave =  new Action<object, LeaveNotifyEventArgs>(
                delegate (object o, Gtk.LeaveNotifyEventArgs args) {
                    component.IsHighlighted = false;
            });

            //attach listeners to enter and leave events of DockItemContainer header, and DockItem content
            //note that Content and that header are EventBoxes, thus the notification works
            foreach (Gtk.Widget widget in new Gtk.Widget[] { infoDockItem.Widget.Header, infoDockItem.Content }) 
            {
                widget.EnterNotifyEvent += new EnterNotifyEventHandler(enter);
                widget.LeaveNotifyEvent += new LeaveNotifyEventHandler(leave);
            }

            //attach to all children
            AttachHandler(infoDockItem.Content as Container, enter, leave);
        }

        static void AttachHandler(Container container, Action<object, EnterNotifyEventArgs> enterHandler, Action<object, LeaveNotifyEventArgs> leaveHandler) 
        {
            if(container != null)
            {
                foreach(Widget w in container.AllChildren) {
                    w.EnterNotifyEvent += new EnterNotifyEventHandler(enterHandler);
                    w.LeaveNotifyEvent += new LeaveNotifyEventHandler(leaveHandler);
                    Container subcontainer = w as Container;
                    if(subcontainer != null)
                        AttachHandler(subcontainer, enterHandler, leaveHandler);
                }
            }
        }

        internal void HideComponentInfoPad(BasicNodeControl component) 
        {
            DockItem infoDockItem = m_mainWindowDockFrame.GetItem(component.ExperimentNode.ID);
            if(infoDockItem != null) 
            {
                if(infoDockItem.Visible == true)
                {
                    //just set invisible
                    //no need for removing it, as it may be opened again
                    infoDockItem.Visible = false;
                }
            }
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


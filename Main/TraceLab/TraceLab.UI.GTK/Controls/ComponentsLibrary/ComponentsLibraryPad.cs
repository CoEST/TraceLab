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

using System;
using MonoDevelop.Components.Docking;
using Gtk;
using Mono.Unix;
using TraceLab.Core.ViewModels;
using TraceLab.Core.Components;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



namespace TraceLab.UI.GTK
{
    /// <summary>
    /// Components library pad.
    /// </summary>
    public class ComponentsLibraryPad : IDockPad
    {
        DockFrame dockFrame;
        ComponentsLibraryWidget widget;


        public ComponentsLibraryPad() 
        {
            widget = new ComponentsLibraryWidget();
        }


        #region IDockPad interface

        /// <summary>
        /// Initialize the window pad in the given dock frame.
        /// </summary>
        /// <param name='dockFrame'>
        /// Dock frame.
        /// </param>
        public void Initialize(DockFrame dockFrame)
        {
            if (this.dockFrame != null)
                throw new ApplicationException ("This DockPad has already been initialized");
            if (dockFrame == null)
                throw new ArgumentNullException ("Provided DockFrame is null");
            this.dockFrame = dockFrame;

            DockItem componentLibraryDock = dockFrame.AddItem("ComponentsLibrary");
            componentLibraryDock.Label = Catalog.GetString("Components Library");
            componentLibraryDock.Behavior |= DockItemBehavior.CantClose;
            componentLibraryDock.DefaultWidth = 200;
            componentLibraryDock.Content = widget;
        }

        /// <summary>
        /// Sets the application model on the given pad.
        /// Pad refreshes its information according to the given application model.
        /// </summary>
        /// <param name='applicationModel'>
        /// Application model.
        /// </param>
        public void SetApplicationModel(ApplicationViewModel applicationViewModel) 
        {
            if(dockFrame == null || dockFrame.GdkWindow == null) 
            {
                //GdkWindow is for each dock frame is assigned when windowShell calls ShowAll(). See DockContainer.OnRealize method
                throw new InvalidOperationException("ComponentsLibraryPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            widget.SetApplicationModel(applicationViewModel);
        }

        #endregion
    }
}

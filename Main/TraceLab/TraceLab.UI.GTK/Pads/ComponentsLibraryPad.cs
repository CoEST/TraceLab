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
    public class ComponentsLibraryPad : IDockPad
    {
        private bool m_initialized = false;
        private DockFrame m_dockFrame;
        private ComponentLibraryViewModel m_componentsLibraryViewModel;
        private Gtk.ListStore m_componentsListModel;

        /// <summary>
        /// The table of targets that the drag will support.
        /// </summary>
        public static Gtk.TargetEntry[] Targets {
            get;
            private set;
        }

        static ComponentsLibraryPad() 
        {
            //init drag supported targets.
            Gdk.Atom metadataDefAtom = Gdk.Atom.Intern("application/x-metadata_definition", false);
            Targets = new Gtk.TargetEntry[1] {
                new Gtk.TargetEntry(metadataDefAtom, TargetFlags.App, 0)
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.ComponentsLibraryPad"/> class.
        /// </summary>
        /// <param name='context'>
        /// Context.
        /// </param>
        public ComponentsLibraryPad() 
        {
        
        }
        
        /// <summary>
        /// Initialize the window pad in the given dock frame.
        /// </summary>
        /// <param name='dockFrame'>
        /// Dock frame.
        /// </param>
        public void Initialize(DockFrame dockFrame)
        {
            m_dockFrame = dockFrame;
            DockItem componentLibraryDockingWindow = m_dockFrame.AddItem("ComponentsLibrary");
            componentLibraryDockingWindow.Label = Catalog.GetString("Components Library");
            componentLibraryDockingWindow.Behavior |= DockItemBehavior.CantClose;
            componentLibraryDockingWindow.DefaultWidth = 200;

            componentLibraryDockingWindow.Content = CreateComponentsTreeView();

            //DockItemToolbar libraryToolbar = componentLibraryDockingWindow.GetToolbar(PositionType.Top);
            //libraryToolbar.Add(ApplicationUICore.Actions.ComponentLibrary.Rescan.CreateDockToolBarItem());

            EnableDrag();

            m_initialized = true;
        }

        /// <summary>
        /// Sets the application model on the given pad.
        /// Pad refreshes its information according to the given application model.
        /// </summary>
        /// <param name='applicationModel'>
        /// Application model.
        /// </param>
        public void SetApplicationModel(ApplicationViewModel appliactionViewModel) 
        {
            if(m_initialized == false || m_dockFrame.GdkWindow == null) 
            {
                //GdkWindow is for each dock frame is assigned when windowShell calls ShowAll(). See DockContainer.OnRealize method
                throw new InvalidOperationException("ComponentsLibraryPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            if(m_componentsLibraryViewModel != null) 
            {
                //detach handlers
                m_componentsLibraryViewModel.Rescanned -= ComponentsLibraryRescannedHandler;
                m_componentsLibraryViewModel.Rescanning -= ComponentsLibraryRescanningHandler;
            }

            m_componentsLibraryViewModel = appliactionViewModel.ComponentLibraryViewModel;

            // Don't build hierarchy unless the library is done scanning
            if (m_componentsLibraryViewModel.IsRescanning == false)
            {
                BuildTagsHierarchy();
            }
            
            //attach handlers
            m_componentsLibraryViewModel.Rescanned += ComponentsLibraryRescannedHandler;
            m_componentsLibraryViewModel.Rescanning += ComponentsLibraryRescanningHandler;
        }

        /// <summary>
        /// Creates the components tree view.
        /// </summary>
        /// <returns>
        /// The components tree view.
        /// </returns>
        private Gtk.Widget CreateComponentsTreeView() 
        {
            ScrolledWindow scroller = new ScrolledWindow();
            m_treeView = new Gtk.TreeView();
            m_treeView.HeadersVisible = false;

            //init model with two columns (label and hidden column for metadatadefinition)
            //note, that currently second column does not have renderer - it is there to allow quick lookup into
            //metadatadefinition (see HandleDragDataGet)
            //possibly custom renderer for metadatadefinition to pring out label would be nice
            //then one column would be enough.
            m_componentsListModel = new Gtk.ListStore(typeof(string), typeof(MetadataDefinition));
            m_treeView.Model = m_componentsListModel;

            // Create a column for the component name
            Gtk.TreeViewColumn componentColumn = new Gtk.TreeViewColumn();
            m_treeView.AppendColumn(componentColumn);

            // Create the text cell that will display the artist name
            Gtk.CellRendererText componentNameCell = new Gtk.CellRendererText();
            
            // Add the cell to the column
            componentColumn.PackStart(componentNameCell, true);
            
            // Tell the Cell Renderers which items in the model to display
            componentColumn.AddAttribute(componentNameCell, "text", 0);

            scroller.Add(m_treeView);
            scroller.ShowAll();

            return scroller;
        }

        #region Drag

        /// <summary>
        /// Enables the drag support on the tree view
        /// </summary>
        private void EnableDrag() 
        {
            Gtk.Drag.SourceSet(m_treeView, Gdk.ModifierType.Button1Mask, Targets, Gdk.DragAction.Move | Gdk.DragAction.Copy);
            m_treeView.DragDataGet += HandleDragDataGet;
        }

        /// <summary>
        /// Handler prepares drag data.
        /// See the EnableDrop and HandleDragDataReceived in the ExperimentCanvasPad
        /// where receiving of the data is done. 
        /// </summary>
        /// <param name='o'>
        /// O.
        /// </param>
        /// <param name='args'>
        /// Arguments.
        /// </param>
        private void HandleDragDataGet (object o, DragDataGetArgs args)
        {
            TreeView tv = o as TreeView;
            if(tv != null) {
                TreeIter item;
                if(tv.Selection.GetSelected(out item)) 
                {
                    //if dragged item in the tree is a metadatadefinition then signal drag data
                    if(tv.Model.GetValue(item, 1) is MetadataDefinition) 
                    {
                        //the byte array can be empty - previously definition was serialized, and deserialized by 
                        //DragDataReceived in experiment canvas, but we can avoid it 
                        //Also serialization didn't work for composite components definitions as its ComponentGraph is not serialized
                        args.SelectionData.Set(args.SelectionData.Target, args.SelectionData.Format, new byte[] {});
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Builds the tags hierarchy of components
        /// and adds it to the tree view.
        /// </summary>
        private void BuildTagsHierarchy()
        {
            //assure list is clear
            m_componentsListModel.Clear();

            // currently view is a simple list
            //TODO: Create tree view from tags. (also check out TreeModelDemo in GtkSharp examples (in program files)
            foreach (MetadataDefinition component in m_componentsLibraryViewModel.ComponentsCollection)
            {
                TreeIter item = m_componentsListModel.AppendValues(component.Label, component);
            }
        }

        private TreeView m_treeView;

        /// <summary>
        /// Handles the components library rescanned event.
        /// Builds tags hierarchy
        /// </summary>
        /// <param name='sender'>Sender</param>
        /// <param name='e'>E.</param>
        private void ComponentsLibraryRescannedHandler(object sender, EventArgs e)
        {
            BuildTagsHierarchy();
        }

        /// <summary>
        /// Handles the components library rescanning event.
        /// </summary>
        /// <param name='sender'>Sender.</param>
        /// <param name='e'>E.</param>
        private void ComponentsLibraryRescanningHandler(object sender, EventArgs e)
        {
            
        }
    }
}
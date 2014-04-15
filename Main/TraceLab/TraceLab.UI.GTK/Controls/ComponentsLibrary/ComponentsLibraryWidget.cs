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
using TraceLab.Core.ViewModels;
using TraceLab.Core.Components;
using System.Collections.Generic;


namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ComponentsLibraryWidget : Gtk.Bin
    {
        ApplicationViewModel applicationViewModel;
        ComponentLibraryViewModel viewModel;
        TreeModelFilter treeFilter;

        public ComponentsLibraryWidget()
        {
            this.Build();
            treeView.HeadersVisible = false;
            treeView.EnableTreeLines = true;
            treeView.EnableSearch = true;
            CellRendererText cellRenderer = new CellRendererText();
            treeView.AppendColumn("Label", cellRenderer);
            treeView.Columns[0].SetCellDataFunc(cellRenderer, new TreeCellDataFunc(RenderNode));
        }

        public void SetApplicationModel(ApplicationViewModel applicationViewModel)
        {
            this.applicationViewModel = applicationViewModel;
            if(viewModel != null) 
            {
                //detach handlers
                viewModel.Rescanned -= ComponentsLibraryRescannedHandler;
                viewModel.Rescanning -= ComponentsLibraryRescanningHandler;
            }
            
            viewModel = applicationViewModel.ComponentLibraryViewModel;

            //attach handlers
            viewModel.Rescanned += ComponentsLibraryRescannedHandler;
            viewModel.Rescanning += ComponentsLibraryRescanningHandler;
            
            // Don't build hierarchy unless the library is done scanning
            if (viewModel.IsRescanning == false)
                ComponentsLibraryRescannedHandler(this, EventArgs.Empty);

            EnableDrag();
        }
       
        #region Drag & Drop
        /// <summary>
        /// The table of targets that the drag will support.
        /// </summary>
        public static TargetEntry[] Targets 
        {
            get;
            private set;
        }
        
        static ComponentsLibraryWidget() 
        {
            //init drag supported targets.
            Gdk.Atom metadataDefAtom = Gdk.Atom.Intern("application/x-metadata_definition", false);
            Targets = new TargetEntry[1] {
                new TargetEntry(metadataDefAtom, TargetFlags.App, 0)
            };
        }
        
        /// <summary>
        /// Enables the drag support on the tree view
        /// </summary>
        private void EnableDrag() 
        {
            Gtk.Drag.SourceSet(treeView, Gdk.ModifierType.Button1Mask, Targets, Gdk.DragAction.Move | Gdk.DragAction.Copy);
            treeView.DragDataGet += HandleDragDataGet;
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
        private void HandleDragDataGet (object source, DragDataGetArgs args)
        {
            TreeView treeView = source as TreeView;
            TreeIter selectedItem;
            
            if(treeView != null && treeView.Selection.GetSelected(out selectedItem))
            {
                ComponentsLibraryNode selectedNode = treeView.Model.GetValue(selectedItem, 0) as ComponentsLibraryNode;
                if (selectedNode.Data != null)
                    args.SelectionData.Set(args.SelectionData.Target, args.SelectionData.Format, new byte[] {}); // we send empty byte array just to trigger a drag&drop event. Drop handler will access componentsLibrary once again to get selected node.
            }
        }
        #endregion

        #region Event Handlers
        void ComponentsLibraryRescanningHandler(object sender, EventArgs e)
        {
        }
        
        void ComponentsLibraryRescannedHandler(object sender, EventArgs e)
        {
            treeFilter = new TreeModelFilter(BuildTreeModel(), null);
            treeFilter.VisibleFunc = new TreeModelFilterVisibleFunc(FilterTree);
            treeView.Model = treeFilter;
        }
        
        bool FilterTree(TreeModel model, TreeIter iter)
        {
            ComponentsLibraryNode node = (ComponentsLibraryNode)model.GetValue(iter, 0);
            
            if (node == null)
                return true;
            else
                return FilterNode(node, filterEntry.Text.ToUpper());
        }
        
        bool FilterNode(ComponentsLibraryNode node, string filter)
        {
            if (node.Label.ToUpper().Contains(filter))
                return true;
            foreach (ComponentsLibraryNode child in node.GetChildren())
                if (FilterNode(child, filter))
                    return true;
            
            return false;
        }
        
        TreeModel BuildTreeModel()
        {
            TreeStore treeStore = new TreeStore(typeof(ComponentsLibraryNode));
            
            foreach (ComponentsLibraryNode node in NodesTreeBuilder.Build(viewModel)) 
            {
                TreeIter treeParentNode = treeStore.AppendValues(node);
                foreach (ComponentsLibraryNode nodeChild in node)
                    AddNodeToTreeModel(nodeChild, treeStore, treeParentNode);
            }
            
            return treeStore;
        }

        void AddNodeToTreeModel(ComponentsLibraryNode node, TreeStore tree, TreeIter parentNode)
        {
            parentNode = tree.AppendValues(parentNode, node);
            foreach (ComponentsLibraryNode childNode in node)
                AddNodeToTreeModel(childNode, tree, parentNode);
        }
        
        void OnFilterEntryTextChanged(object sender, EventArgs e)
        {
            if (treeFilter != null)
                treeFilter.Refilter();
        }
        
        void RenderNode(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ComponentsLibraryNode node = (ComponentsLibraryNode) model.GetValue(iter, 0);
            (cell as CellRendererText).Text = node.Label;
        }
        
        protected void RescanButtonClicked(object sender, EventArgs e)
        {
            treeView.Model = new TreeStore(typeof(ComponentsLibraryNode));
            viewModel.Rescan();
        }
        
        protected void PackageReferencesButtonClicked(object sender, EventArgs e)
        {
            PackageReferencesWindow window = new PackageReferencesWindow(applicationViewModel);
            window.SetPosition(WindowPosition.Center);
            window.Modal = true;
            window.Show ();
        }
        #endregion
    }
}


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
using TraceLab.Core.PackageSystem;
using TraceLabSDK.PackageSystem;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    public partial class PackageReferencesWindow : Window
    {
        ApplicationViewModel viewModel;

        #region Constructors
        public PackageReferencesWindow() : base(WindowType.Toplevel)
        {
            this.Build();
        }

        public PackageReferencesWindow(ApplicationViewModel viewModel) : this()
        {
            if (viewModel == null)
                throw new ArgumentNullException();
            this.viewModel = viewModel;

            TreeStore treeStore = new TreeStore(typeof(PackageReferenceNode));
            FormatTreeView(treeStore);
            BuildModel(treeStore);
            treeView.Model = treeStore;
        }
        #endregion

        void FormatTreeView(TreeStore treeStore)
        {
            treeView.HeadersVisible = false;
            treeView.EnableTreeLines = true;

            CellRendererToggle checkboxCellRenderer = new CellRendererToggle();
            checkboxCellRenderer.Activatable = true;
            checkboxCellRenderer.Toggled += delegate(object o, ToggledArgs args) 
            {
                TreeIter iter;      
                if (treeStore.GetIter(out iter, new TreePath(args.Path))) 
                {
                    PackageReferenceNode node = (PackageReferenceNode)treeStore.GetValue(iter, 0);              
                    node.State = !node.State;
                }
            };
            treeView.AppendColumn("CheckBox", checkboxCellRenderer);
            
            CellRendererText nameCellRenderer = new CellRendererText();
            treeView.AppendColumn("Name", nameCellRenderer);
            
            treeView.Columns[0].SetCellDataFunc(checkboxCellRenderer, new TreeCellDataFunc(RenderCheckBox));
            treeView.Columns[1].SetCellDataFunc(nameCellRenderer, new TreeCellDataFunc(RenderName));
        }

        void BuildModel(TreeStore treeStore)
        {
            foreach (IPackage package in PackageManager.Instance)
            {
                PackageReferenceNode node = new PackageReferenceNode(package);
                //foreach(IPackageFile file in package.Files)
                //    file.Path
                treeStore.AppendValues(node);
            }
        }

        void RenderCheckBox(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageReferenceNode node = (PackageReferenceNode) model.GetValue(iter, 0);
            CellRendererToggle toggle = (cell as CellRendererToggle);
            toggle.Active = node.State;
        }

        void RenderName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageReferenceNode node = (PackageReferenceNode) model.GetValue(iter, 0);
            (cell as CellRendererText).Text = node.Name;
        }
    }
}


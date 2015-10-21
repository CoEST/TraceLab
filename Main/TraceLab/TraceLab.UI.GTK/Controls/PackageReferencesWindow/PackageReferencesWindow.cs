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
using System.Linq;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.GTK
{
    public partial class PackageReferencesWindow : Window
    {
        #region Constructors
        public PackageReferencesWindow() : base(WindowType.Toplevel)
        {
            this.Build();
        }

        public PackageReferencesWindow(ApplicationViewModel viewModel) : this()
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            if(viewModel.Experiment == null)
                throw new InvalidOperationException("Package reference window cannot be used when there is no experiment opened");

            this.m_experiment = viewModel.Experiment;

            m_treeStore = new Gtk.TreeStore(typeof(PackageReferenceNode));
            FormatTreeView();
            BuildModel();
            treeView.Model = m_treeStore;
        }
        #endregion

        private void FormatTreeView()
        {
            treeView.HeadersVisible = false;
            treeView.EnableTreeLines = true;

            CellRendererToggle includeCheckBoxRenderer = new CellRendererToggle();
            TreeViewColumn includeColumn = treeView.AppendColumn("Include", includeCheckBoxRenderer);
            includeColumn.SetCellDataFunc(includeCheckBoxRenderer, new TreeCellDataFunc(RenderIncludeCheckBox));
            includeCheckBoxRenderer.Activatable = true;
            includeCheckBoxRenderer.Toggled += HandleToggled;;

            CellRendererText nameRenderer = new CellRendererText();
            TreeViewColumn nameColumn = treeView.AppendColumn("Name", nameRenderer);
            nameColumn.SetCellDataFunc(nameRenderer, new TreeCellDataFunc(RenderName));      
        }

        private void HandleToggled (object o, ToggledArgs args)
        {
            TreeIter iter;      
            if (m_treeStore.GetIter(out iter, new TreePath(args.Path))) 
            {
                PackageReferenceNode node = (PackageReferenceNode)m_treeStore.GetValue(iter, 0);              
                node.State = !node.State;

                var packageReference = new TraceLab.Core.PackageSystem.PackageReference(node.Package);

                if (node.State == false) 
                {
                    PackagesViewModelHelper.RemoveReference(m_experiment, packageReference);
                }
                else if(node.State && !m_experiment.References.Contains(packageReference))
                {
                    PackagesViewModelHelper.AddReference(m_experiment, packageReference);
                }
            }
        }

        private void BuildModel()
        {
            foreach (IPackage package in PackageManager.Instance)
            {
                bool initialState = m_experiment.References.Any( (p) => p.ID.Equals(package.ID));
                PackageReferenceNode node = new PackageReferenceNode(package, initialState);
                m_treeStore.AppendValues(node);
            }
        }

        private void RenderIncludeCheckBox(TreeViewColumn layout, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageReferenceNode node = (PackageReferenceNode)model.GetValue(iter, 0);
            (cell as CellRendererToggle).Active = node.State;
        }

        private void RenderName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageReferenceNode node = (PackageReferenceNode) model.GetValue(iter, 0);
            (cell as CellRendererText).Text = node.Name;
        }

        private IExperiment m_experiment;
        private Gtk.TreeStore m_treeStore;
    }
}


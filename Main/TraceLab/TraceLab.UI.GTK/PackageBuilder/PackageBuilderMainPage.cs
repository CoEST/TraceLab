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
using TraceLab.Core.PackageBuilder;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class PackageBuilderMainPage : Gtk.Bin
    {
        public PackageBuilderMainPage()
        {
            this.Build();

            /*
            1 -- Node Data
            */
            m_packageContentStore = new Gtk.TreeStore(typeof(PackageFileSourceInfo));
            this.treeView.Model = m_packageContentStore;

            //create columns with associated cell renderings
            CellRendererText nameRenderer = new CellRendererText();
            TreeViewColumn nameColumn = treeView.AppendColumn("Name", nameRenderer);
            nameColumn.SetCellDataFunc(nameRenderer, new TreeCellDataFunc(RenderName));

            CellRendererToggle hasComponentsCheckBoxRenderer = new CellRendererToggle();
            TreeViewColumn hasComponentsColumn = treeView.AppendColumn("Has Components", hasComponentsCheckBoxRenderer);
            hasComponentsColumn.SetCellDataFunc(hasComponentsCheckBoxRenderer, new TreeCellDataFunc(RenderHasComponentCheckBox));
            hasComponentsCheckBoxRenderer.Activatable = true;
            hasComponentsCheckBoxRenderer.Toggled += HasComponentCheckBoxToggled;

            CellRendererToggle hasTypesCheckBoxRenderer = new CellRendererToggle();
            TreeViewColumn hasTypesColumn = treeView.AppendColumn("Has Types", hasTypesCheckBoxRenderer);
            hasTypesColumn.SetCellDataFunc(hasTypesCheckBoxRenderer, new TreeCellDataFunc(RenderHasTypesCheckBox));
            hasTypesCheckBoxRenderer.Activatable = true;
            hasTypesCheckBoxRenderer.Toggled += HasTypesCheckBoxToggled;

            this.treeView.Selection.Mode = Gtk.SelectionMode.Multiple;

            EnableDrop();
        }

        private void EnableDrop()
        {
            //init drag supported targets.
            TargetEntry[] targets = new TargetEntry[1] {
                new TargetEntry("text/uri-list", 0, 0)
            };

            // set up label as a drop target
            Gtk.Drag.DestSet (this.treeView, DestDefaults.All, targets, Gdk.DragAction.Copy);
            this.treeView.DragDataReceived += HandleDragDataReceived;
        }

        private void HandleDragDataReceived (object o, DragDataReceivedArgs args)
        {
            TreeView treeView = (TreeView) o;
            bool success = false;

            //1. check if we can drop at the given position
            TreePath path;
            TreeViewDropPosition pos;
            if (!treeView.GetDestRowAtPos (args.X, args.Y, out path, out pos)) 
            {
                args.RetVal = false;
                Gtk.Drag.Finish(args.Context, success, false, args.Time);
                return;
            }

            //2. check if given position can find corresponding iter (iter holds value with PackageSourceInfo)
            Gtk.TreeIter folderIter;
            if (!m_packageContentStore.GetIter (out folderIter, path))
            {
                args.RetVal = false;
                Gtk.Drag.Finish(args.Context, success, false, args.Time);
                return;
            }

            //if user drop the item not on the folder then return false
            PackageHeirarchyItem folder = this.treeView.Model.GetValue(folderIter, 0) as PackageHeirarchyItem;
            if(folder == null) 
            {
                args.RetVal = false;
                Gtk.Drag.Finish(args.Context, success, false, args.Time);
                return;
            }

            if (pos == TreeViewDropPosition.Before)
                treeView.SetDragDestRow (path, TreeViewDropPosition.IntoOrBefore);
            else if (pos == TreeViewDropPosition.After)
                treeView.SetDragDestRow (path, TreeViewDropPosition.IntoOrAfter);

            Gdk.Drag.Status (args.Context, args.Context.SuggestedAction, args.Time);

            string data = System.Text.Encoding.UTF8.GetString(args.SelectionData.Data);
            List<string> paths = new List<string>(System.Text.RegularExpressions.Regex.Split(data, "\r\n"));
            paths.RemoveAll(string.IsNullOrEmpty);
            paths.RemoveAt(paths.Count-1); // I don't know what last object is, but I tested and noticed that it is not a path

            foreach (string uriPath in paths) 
            {
                string localPath = new Uri(uriPath).LocalPath;
                System.Console.WriteLine ("Got Path {0}", localPath);
                PackageFileSourceInfo newItem = m_viewModel.Add(folder, localPath);

                //update also a view
                if(newItem != null) 
                {
                    AddNodeToTreeModel(newItem, folderIter);
                }
            }

            success = true;
            Gtk.Drag.Finish(args.Context, success, false, args.Time);
        }

        public PackageBuilderMainPage(PackageBuilderViewModel viewModel) : this()
        {
            m_viewModel = viewModel;
            this.packageNameEntry.Text = viewModel.PackageSourceInfo.Name;
            BuildTree();
        }

        private void BuildTree()
        {
            PackageHeirarchyItem root = m_viewModel.PackageSourceInfo.Root;
            TreeIter treeParentNode = m_packageContentStore.AppendValues(root);

            foreach (PackageFileSourceInfo childNode in root.Children)
            {
                AddNodeToTreeModel(childNode, treeParentNode);
            }
        }

        private void AddNodeToTreeModel(PackageFileSourceInfo node, TreeIter parentNode)
        {
            parentNode = m_packageContentStore.AppendValues(parentNode, node);

            PackageHeirarchyItem hierarchyItem = node as PackageHeirarchyItem;
            if(hierarchyItem != null)
            {
                foreach (PackageFileSourceInfo childNode in hierarchyItem.Children)
                {
                    AddNodeToTreeModel(childNode, parentNode);
                }
            }
        }

        private void RenderName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageFileSourceInfo value = (PackageFileSourceInfo)model.GetValue(iter, 0);
            (cell as CellRendererText).Text = value.Name;
        }

        private void RenderHasComponentCheckBox(TreeViewColumn layout, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageHeirarchyItem hierarchyItem = model.GetValue(iter, 0) as PackageHeirarchyItem;
            if(hierarchyItem != null)
            {
                (cell as CellRendererToggle).Active = hierarchyItem.HasComponents;
                cell.Visible = true;
            } 
            else 
            {
                //if it is just a file not directory hide the checkbox
                cell.Visible = false;
            }
        }

        private void RenderHasTypesCheckBox(TreeViewColumn layout, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            PackageHeirarchyItem hierarchyItem = model.GetValue(iter, 0) as PackageHeirarchyItem;
            if(hierarchyItem != null)
            {
                (cell as CellRendererToggle).Active = hierarchyItem.HasTypes;
                cell.Visible = true;
            } 
            else 
            {
                //if it is just a file not directory hide the checkbox
                cell.Visible = false;
            }
        }
                
        private void HasComponentCheckBoxToggled(object o, ToggledArgs args)
        {
            TreeIter iter;
            if (m_packageContentStore.GetIterFromString(out iter, args.Path)) 
            {
                PackageHeirarchyItem hierarchyItem = m_packageContentStore.GetValue(iter, 0) as PackageHeirarchyItem;
                if(hierarchyItem != null)
                {
                    hierarchyItem.HasComponents = !hierarchyItem.HasComponents;
                }
            }
        }

        private void HasTypesCheckBoxToggled(object o, ToggledArgs args)
        {
            TreeIter iter;
            if (m_packageContentStore.GetIterFromString(out iter, args.Path)) 
            {
                PackageHeirarchyItem hierarchyItem = m_packageContentStore.GetValue(iter, 0) as PackageHeirarchyItem;
                if(hierarchyItem != null)
                {
                    hierarchyItem.HasTypes = !hierarchyItem.HasTypes;
                }
            }
        }

        private PackageBuilderViewModel m_viewModel;
        private Gtk.TreeStore m_packageContentStore;

        protected void HandleKeyPress(object o, KeyPressEventArgs args)
        {
            if(args.Event.Key == Gdk.Key.Delete)
            {
                foreach(TreePath treePath in treeView.Selection.GetSelectedRows())
                {
                    TreeIter iter;
                    if(treeView.Model.GetIter(out iter, treePath))
                    {
                        PackageFileSourceInfo info = treeView.Model.GetValue(iter, 0) as PackageFileSourceInfo;
                        PackageFileSourceInfo root = m_viewModel.PackageSourceInfo.Root;
                        //remove only if it is not a root
                        if(info != root && info != null)
                        {
                            info.Parent.Children.Remove(info);
                            m_packageContentStore.Remove(ref iter);
                        }
                    }
                }
            }
        }

        // HERZUM SPRINT 4.2: TLAB-215
        protected void OnPackageNameEntryChanged (object sender, EventArgs e)
        {
            m_viewModel.PackageSourceInfo.Name = this.packageNameEntry.Text;
        }
        // END HERZUM SPRINT 4.2: TLAB-215
    }
}


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
using TraceLab.Core.Experiments;
using Gtk;
using System.Collections;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    /// <summary>
    /// IO spec setup page in the wizard for defining composite component 
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class IOSpecSetupPage : Gtk.Bin
    {
        private IOSpecSetupPage()
        {
            this.Build();
        }

        public IOSpecSetupPage(DefiningCompositeComponentSetup setup) : this()
        {
            m_setup = setup;
            InitializeInputsListView();
            InitializeOutputsListView();
        }

        private void InitializeInputsListView()
        {
            m_inputsStore = new Gtk.ListStore(typeof(ItemSetting), typeof(ItemSetting), typeof(ItemSetting));
            InitializeListView(this.inputsView, m_inputsStore, "Input", 
                             m_setup.InputSettings.Values, InputIncludeCheckBoxToggled);
        }

        private void InitializeOutputsListView() 
        {
            m_outputsStore = new Gtk.ListStore(typeof(ItemSetting), typeof(ItemSetting), typeof(ItemSetting));
            InitializeListView(this.outputsView, m_outputsStore, "Output", 
                             m_setup.OutputSettings.Values, OutputsIncludeCheckBoxToggled);
        }

        private void InitializeListView(TreeView treeView, ListStore store, String nameColumnTitle, 
                                      IEnumerable<ItemSetting> data, ToggledHandler includeToggleHandler) 
        {
            treeView.Model = store;

            //create columns with associated cell renderings
            CellRendererText nameRenderer = new CellRendererText();
            TreeViewColumn nameColumn = treeView.AppendColumn(nameColumnTitle, nameRenderer);
            nameColumn.SetCellDataFunc(nameRenderer, new TreeCellDataFunc(RenderName));            

            CellRendererText typeRenderer = new CellRendererText();
            TreeViewColumn typeColumn = treeView.AppendColumn("Type", typeRenderer);
            typeColumn.SetCellDataFunc(typeRenderer, new TreeCellDataFunc(RenderType));   

            CellRendererToggle includeCheckBoxRenderer = new CellRendererToggle();
            TreeViewColumn includeColumn = treeView.AppendColumn("Include", includeCheckBoxRenderer);
            includeColumn.SetCellDataFunc(includeCheckBoxRenderer, new TreeCellDataFunc(RenderIncludeCheckBox));
            includeCheckBoxRenderer.Activatable = true;
            includeCheckBoxRenderer.Toggled += includeToggleHandler;

            //fill store with data
            foreach (ItemSetting item in data) 
            {
                store.AppendValues(item, item, item);
            }
        }

        private void InputIncludeCheckBoxToggled(object o, ToggledArgs args)
        {
            TreeIter iter;
            if (m_inputsStore.GetIterFromString (out iter, args.Path)) 
            {
                ItemSetting itemSetting = (ItemSetting)m_inputsStore.GetValue(iter, 2);
                itemSetting.Include = !itemSetting.Include;
            }
        }

        private void OutputsIncludeCheckBoxToggled(object o, ToggledArgs args)
        {
            TreeIter iter;
            if (m_outputsStore.GetIterFromString (out iter, args.Path)) 
            {
                ItemSetting itemSetting = (ItemSetting)m_outputsStore.GetValue(iter, 2);
                itemSetting.Include = !itemSetting.Include;
            }
        }

        private void RenderName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ItemSetting itemSetting = (ItemSetting)model.GetValue(iter, 0);
            (cell as CellRendererText).Text = itemSetting.ItemSettingName;
        }
        
        private void RenderType(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ItemSetting itemSetting = (ItemSetting)model.GetValue(iter, 1);
            (cell as CellRendererText).Text = itemSetting.FriendlyType;
        }

        private void RenderIncludeCheckBox(TreeViewColumn layout, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ItemSetting itemSetting = (ItemSetting)model.GetValue(iter, 2);
            (cell as CellRendererToggle).Active = itemSetting.Include;
        }

        private DefiningCompositeComponentSetup m_setup;
        private Gtk.ListStore m_inputsStore;
        private Gtk.ListStore m_outputsStore;
    }
}


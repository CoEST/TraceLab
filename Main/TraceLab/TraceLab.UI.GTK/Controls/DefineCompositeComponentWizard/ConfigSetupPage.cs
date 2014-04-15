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

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ConfigSetupPage : Gtk.Bin
    {
        private ConfigSetupPage ()
        {
            this.Build ();
        }

        public ConfigSetupPage(DefiningCompositeComponentSetup setup) : this()
        {
            m_setup = setup;
            InitializeConfigListView();
        }

        private void InitializeConfigListView()
        {
            m_configStore = new Gtk.ListStore(typeof(ConfigItemSetting), typeof(ConfigItemSetting), 
                                              typeof(ConfigItemSetting), typeof(ConfigItemSetting));

            this.configView.Model = m_configStore;
            
            //create columns with associated cell renderings
            CellRendererText nameRenderer = new CellRendererText();
            TreeViewColumn nameColumn = this.configView.AppendColumn("Config Parameter", nameRenderer);
            nameColumn.SetCellDataFunc(nameRenderer, new TreeCellDataFunc(RenderName));            

            CellRendererText aliasRenderer = new CellRendererText();
            TreeViewColumn aliasColumn = this.configView.AppendColumn("Alias", aliasRenderer);
            aliasColumn.SetCellDataFunc(aliasRenderer, new TreeCellDataFunc(RenderAlias));
            aliasRenderer.Editable = true;
            aliasRenderer.Edited += AliasEdited;

            CellRendererText typeRenderer = new CellRendererText();
            TreeViewColumn typeColumn = this.configView.AppendColumn("Type", typeRenderer);
            typeColumn.SetCellDataFunc(typeRenderer, new TreeCellDataFunc(RenderType));   
            
            CellRendererToggle includeCheckBoxRenderer = new CellRendererToggle();
            TreeViewColumn includeColumn = this.configView.AppendColumn("Include", includeCheckBoxRenderer);
            includeColumn.SetCellDataFunc(includeCheckBoxRenderer, new TreeCellDataFunc(RenderIncludeCheckBox));
            includeCheckBoxRenderer.Activatable = true;
            includeCheckBoxRenderer.Toggled += IncludeCheckBoxToggled;
            
            //fill store with data
            foreach (ItemSetting item in m_setup.ConfigSettings.Values) 
            {
                m_configStore.AppendValues(item, item, item, item);
            }
        }
                
        private void AliasEdited (object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_configStore.GetIterFromString (out iter, args.Path)) 
            {
                ConfigItemSetting itemSetting = (ConfigItemSetting)m_configStore.GetValue(iter, 2);
                itemSetting.Alias = args.NewText;
            }
        }

        private void IncludeCheckBoxToggled(object o, ToggledArgs args)
        {
            TreeIter iter;
            if (m_configStore.GetIterFromString (out iter, args.Path)) 
            {
                ConfigItemSetting itemSetting = (ConfigItemSetting)m_configStore.GetValue(iter, 3);
                itemSetting.Include = !itemSetting.Include;
            }
        }

        private void RenderName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ConfigItemSetting itemSetting = (ConfigItemSetting)model.GetValue(iter, 0);
            (cell as CellRendererText).Text = itemSetting.ItemSettingName;
        }

        private void RenderAlias(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ConfigItemSetting itemSetting = (ConfigItemSetting)model.GetValue(iter, 1);
            (cell as CellRendererText).Text = itemSetting.Alias;
        }
        
        private void RenderType(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ConfigItemSetting itemSetting = (ConfigItemSetting)model.GetValue(iter, 2);
            (cell as CellRendererText).Text = itemSetting.FriendlyType;
        }
        
        private void RenderIncludeCheckBox(TreeViewColumn layout, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            ConfigItemSetting itemSetting = (ConfigItemSetting)model.GetValue(iter, 3);
            (cell as CellRendererToggle).Active = itemSetting.Include;
        }

        private DefiningCompositeComponentSetup m_setup;
        private Gtk.ListStore m_configStore;
    }
}


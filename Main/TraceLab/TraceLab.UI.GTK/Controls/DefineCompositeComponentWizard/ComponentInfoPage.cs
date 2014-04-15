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
using TraceLab.Core.Settings;
using System.Collections.Generic;
using Gtk;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ComponentInfoPage : Gtk.Bin
    {
        private ComponentInfoPage()
        {
            this.Build();
        }

        public ComponentInfoPage(DefiningCompositeComponentSetup setup, 
                                 IEnumerable<SettingsPath> componentDirectories) : this()
        {
            m_setup = setup;
            this.descriptionTextView.Buffer.Changed += DescriptionEntryChanged;

            InitComponentDirsComboBox (componentDirectories);
        }

        private void InitComponentDirsComboBox (IEnumerable<SettingsPath> componentDirectories)
        {
            //fill combo box with components directories
            foreach (SettingsPath path in componentDirectories) 
            {
                this.componentsDirectoryComboBox.AppendText (path.Path);
            }

            //set first path as current selected path
            TreeIter iter;
            if (this.componentsDirectoryComboBox.Model.GetIterFirst (out iter)) 
            {
                this.componentsDirectoryComboBox.SetActiveIter (iter);
                m_selectedComponentDirectory = (string)this.componentsDirectoryComboBox.Model.GetValue (iter, 0);
                UpdateFinalComponentPath();
            }
        }

        protected void ComponentNameEntryChanged(object sender, EventArgs e)
        {
            m_setup.Name = this.componentNameEntry.Text;
            UpdateFinalComponentPath();
            OnComponentNameChanged();
        }

        protected void AuthorEntryChanged(object sender, EventArgs e)
        {
            m_setup.Author = this.authorEntry.Text;
        }

        protected void VersionEntryChanged(object sender, EventArgs e)
        {
            m_setup.Version = this.versionEntry.Text;
        }

        private void DescriptionEntryChanged (object sender, EventArgs e)
        {
            m_setup.Description = this.descriptionTextView.Buffer.Text;
        }
                
        protected void componentComboBoxChanged(object sender, EventArgs e)
        {
            TreeIter iter;
            if (this.componentsDirectoryComboBox.GetActiveIter(out iter))
            {
                m_selectedComponentDirectory = (string) this.componentsDirectoryComboBox.Model.GetValue(iter, 0);
                UpdateFinalComponentPath();
            }
        }

        private void UpdateFinalComponentPath()
        {
            string filename = (m_setup.Name != null) ? m_setup.Name.ToLower() + ".tcml" : String.Empty;
            string finalPath = System.IO.Path.Combine(m_selectedComponentDirectory, filename);

            //update model data
            m_setup.CompositeComponentLocationFilePath = finalPath;
            //update also text box (that is not editable)
            this.finalLocationEntry.Text = finalPath;
        }

        public event EventHandler ComponentNameChanged;
                
        private void OnComponentNameChanged()
        {
            if(ComponentNameChanged != null)
            {
                ComponentNameChanged(this, EventArgs.Empty);
            }
        }

        private DefiningCompositeComponentSetup m_setup;
        private string m_selectedComponentDirectory;
    }
}


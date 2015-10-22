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
using TraceLab.Core.PackageBuilder;
using TraceLab.Core;
using Gtk;
using TraceLab.UI.GTK.Extensions;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class PackageBuilderSettingsPage : Gtk.Bin
    {
        public PackageBuilderSettingsPage ()
        {
            this.Build ();

            //set all texts
            this.titleLabel.Markup = "<span size=\"x-large\" weight=\"bold\">Package experiment configuration</span>";

            this.CheckBoxIncludeIndependentFilesDirs.Label = Messages.PackageBuilderConfig1_IncludeIndependentFilesDirs;
            SetInfoNote(this.DecriptionIncludeIndependentFilesDirs.Buffer, Messages.PackageBuilderConfig1_IncludeIndependentFilesDirs_Descr);

            this.CheckBoxIncludeOtherPackagesFilesDirs.Label = Messages.PackageBuilderConfig2_IncludeOtherPackagesFilesDirs;
            SetInfoNote(this.DescriptionIncludeOtherPackagesFilesDirs.Buffer, Messages.PackageBuilderConfig2_IncludeOtherPackagesFilesDirs_Descr);

            this.CheckBoxIncludeOtherPackagesAssemblies.Label = Messages.PackageBuilderConfig3_IncludeOtherPackagesAssemblies;
            SetInfoNote(this.DescriptionIncludeOtherPackagesAssemblies.Buffer, Messages.PackageBuilderConfig3_IncludeOtherPackagesAssemblies_Descr);

            SetInfoNote(this.OtherNotes.Buffer, Messages.PackageBuilderNote);
        }

        private void SetInfoNote(TextBuffer buffer, string note)
        {
            buffer.InitTags();
            TextIter insertIter = buffer.StartIter;
            buffer.InsertWithTagsByName (ref insertIter, note, "italic");
        }

        public PackageBuilderSettingsPage(PackageBuilderViewModel viewModel) : this()
        {
            m_viewModel = viewModel;

            this.CheckBoxIncludeIndependentFilesDirs.Active = viewModel.ExperimentPackageConfig.IncludeIndependentFilesDirs;
            this.CheckBoxIncludeOtherPackagesFilesDirs.Active = viewModel.ExperimentPackageConfig.IncludeOtherPackagesFilesDirs;
            this.CheckBoxIncludeOtherPackagesAssemblies.Active = viewModel.ExperimentPackageConfig.IncludeOtherPackagesAssemblies;

            this.CheckBoxIncludeIndependentFilesDirs.Toggled += (object sender, EventArgs e) => 
            {
                viewModel.ExperimentPackageConfig.IncludeIndependentFilesDirs = !this.CheckBoxIncludeIndependentFilesDirs.Active;
            };
        
            this.CheckBoxIncludeOtherPackagesFilesDirs.Toggled += (object sender, EventArgs e) => 
            {
                viewModel.ExperimentPackageConfig.IncludeOtherPackagesFilesDirs = !this.CheckBoxIncludeOtherPackagesFilesDirs.Active;
            };
        
            this.CheckBoxIncludeOtherPackagesAssemblies.Toggled += (object sender, EventArgs e) => 
            {
                viewModel.ExperimentPackageConfig.IncludeOtherPackagesAssemblies = !this.CheckBoxIncludeOtherPackagesAssemblies.Active;
            };

        }

        private PackageBuilderViewModel m_viewModel;
    }
}


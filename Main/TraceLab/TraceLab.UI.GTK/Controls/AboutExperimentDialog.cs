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
using TraceLab.Core.ViewModels;

namespace TraceLab.UI.GTK
{
    public partial class AboutExperimentDialog : Gtk.Dialog
    {
        private ApplicationViewModel m_applicationViewModel;
    
        protected AboutExperimentDialog ()
        {
            this.Build ();
        }

        public AboutExperimentDialog (ApplicationViewModel appVM)
            : this()
        {
            m_applicationViewModel= appVM;

            tbx_experimentName.Text= m_applicationViewModel.Experiment.ExperimentInfo.Name;
            tbx_author.Text= m_applicationViewModel.Experiment.ExperimentInfo.Author;
            tbx_contributors.Text = m_applicationViewModel.Experiment.ExperimentInfo.Contributors;
            tbx_description.Buffer.Text = m_applicationViewModel.Experiment.ExperimentInfo.Description;
        }

        protected void buttonOKClickedHandler (object sender, EventArgs e)
        {
            m_applicationViewModel.Experiment.ExperimentInfo.Name=tbx_experimentName.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Author=tbx_author.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Contributors=tbx_contributors.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Description=tbx_description.Buffer.Text;

            this.Destroy();
        }

        protected void buttonCancelClickedHandler (object sender, EventArgs e)
        {
            this.Destroy();
        }

        protected void onChangeValidateHandler (object sender, EventArgs e)
        {
            bool isDataValid = true;

            isDataValid = isDataValid && !String.IsNullOrWhiteSpace(tbx_experimentName.Text);

            btn_ok.Sensitive = isDataValid;
        }
    }
}


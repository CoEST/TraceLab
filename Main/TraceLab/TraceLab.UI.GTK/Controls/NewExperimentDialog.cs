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
using System.IO;
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;
using Gtk;

namespace TraceLab.UI.GTK
{
    public partial class NewExperimentDialog : Gtk.Dialog
    {
        private Experiment _experiment;

        private static readonly string s_fileExtension = ".teml";

        public NewExperimentDialog (ref Experiment experiment)
        {
            _experiment = experiment;
            this.Build ();

        }

        protected void buttonCancelClickedHandler (object sender, EventArgs e)
        {
            this.Destroy();
        }

        protected void buttonSelectDirClickedHandler (object sender, EventArgs e)
        {
            string directoryPath;

            if (FileDialogs.SelectCatalogDialog (this, out directoryPath)) 
            {
                tbx_dirPath.Text = directoryPath;
            }
        }



        protected void buttonOKClickedHandler (object sender, EventArgs e)
        {
            string fileName = tbx_fileName.Text.Trim ();
            if (!fileName.EndsWith (s_fileExtension)) 
            {
                fileName+= s_fileExtension;
            }
            _experiment.ExperimentInfo.Name= tbx_experimentName.Text;

            //_experiment.ExperimentInfo.FilePath= flw_choseFile.CurrentFolder +"/"+ fileName;

            _experiment.ExperimentInfo.FilePath= tbx_dirPath.Text +"/"+ fileName;
            

            _experiment.ExperimentInfo.Author = tbx_author.Text;
            _experiment.ExperimentInfo.Description = tbx_description.Buffer.Text;

            Results = true;

            this.Destroy();

        }


        public void updateProjectName (object sender, EventArgs e)
        {
            tbx_fileName.Text=tbx_experimentName.Text.Replace(" ","_").Replace(".","-");

        }

        public Boolean Results
        {
            get;
            private set;
        }

        /// <summary>
        /// Validates the data entered in the TextBoxes. If all data is valid, then the CreateButton is enabled.
        /// </summary>
        protected void onChangeValidateHandler (object sender, EventArgs e)
        {
            bool isDataValid = true;

            isDataValid = isDataValid && !String.IsNullOrWhiteSpace(tbx_experimentName.Text);
            isDataValid = isDataValid && Directory.Exists(tbx_dirPath.Text);
            isDataValid = isDataValid && !String.IsNullOrWhiteSpace(tbx_fileName.Text);
            isDataValid = isDataValid && (tbx_fileName.Text.Trim() != s_fileExtension);

            btn_ok.Sensitive = isDataValid;

        }


    }
}
 

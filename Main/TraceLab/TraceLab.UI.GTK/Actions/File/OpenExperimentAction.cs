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

using System;
using Gtk;
using Mono.Unix;
using TraceLab.Core.ViewModels;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.GTK
{
    class OpenExperimentAction : IActionHandler
    {
        private ApplicationContext m_applicationContext;

        public OpenExperimentAction(ApplicationContext applicationWrapper)
        {
            m_applicationContext = applicationWrapper;
        }

        #region IActionHandler Members
        public void Initialize()
        {
            m_applicationContext.Actions.File.Open.Activated += Activated;
        }
        
        public void Uninitialize()
        {
            m_applicationContext.Actions.File.Open.Activated -= Activated;
        }
        #endregion

        private void Activated(object sender, EventArgs e)
        {
            string filename = FileDialogs.OpenExperimentDialog(m_applicationContext.MainWindow.WindowShell);
            if(filename != null) 
            {
                OpenExperiment(filename, m_applicationContext);
            }
        }

        /// <summary>
        /// Opens the experiment using given application window
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="applicationContext">Application context.</param>
        internal static void OpenExperiment(string filename, ApplicationContext applicationContext)
        {
            try
            {
                Experiment experiment = TraceLab.Core.Experiments.ExperimentManager.Load(filename, TraceLab.Core.Components.ComponentsLibrary.Instance);
                ApplicationViewModel newApplicationViewModel = ApplicationViewModel.CreateNewApplicationViewModel(applicationContext.Application, experiment);
                RecentExperimentsHelper.UpdateRecentExperimentList(TraceLab.Core.Settings.Settings.RecentExperimentsPath, experiment.ExperimentInfo.FilePath);
                applicationContext.OpenInWindow(newApplicationViewModel);
            }
            catch (TraceLab.Core.Exceptions.ExperimentLoadException ex)
            {
                string msg = String.Format("Unable to open the file {0}. Error: {1}", filename, ex.Message);
                NLog.LogManager.GetCurrentClassLogger().Error(msg);
            }
        }

    }
}


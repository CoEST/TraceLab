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
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;
using Gtk;

namespace TraceLab.UI.GTK
{
    class NewExperimentAction : IActionHandler
    {
        private ApplicationContext m_applicationContext;

        public NewExperimentAction(ApplicationContext applicationWrapper)
        {
            m_applicationContext = applicationWrapper;
        }
        
        #region IActionHandler Members
        public void Initialize()
        {
            m_applicationContext.Actions.File.New.Activated += Activated;
        }
        
        public void Uninitialize()
        {
            m_applicationContext.Actions.File.New.Activated -= Activated;
        }
        #endregion
        
        private void Activated (object sender, EventArgs eargs)
        {
            Experiment experiment = ExperimentManager.New ();
            bool success = FileDialogs.NewExperimantDialog (m_applicationContext.MainWindow.WindowShell, ref experiment);

            if (success) 
            {
                ApplicationViewModel newApplicationViewModel = ApplicationViewModel.CreateNewApplicationViewModel(m_applicationContext.Application, experiment);
                RecentExperimentsHelper.UpdateRecentExperimentList(TraceLab.Core.Settings.Settings.RecentExperimentsPath, experiment.ExperimentInfo.FilePath);
                m_applicationContext.OpenInWindow(newApplicationViewModel);

                String file = experiment.ExperimentInfo.FilePath;
                try
                {
                    ExperimentManager.Save(experiment, file);
                }
                catch (System.IO.IOException e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                    FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell,"Failed to Save File", file);

                }
                catch (UnauthorizedAccessException e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                    FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell,"Failed to Save File", file);

                }
                catch (Exception e)
                {
                    NLog.LogManager.GetCurrentClassLogger().ErrorException(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                  FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell,"Failed to Save File", file);
              }
            }

         }
    }
}


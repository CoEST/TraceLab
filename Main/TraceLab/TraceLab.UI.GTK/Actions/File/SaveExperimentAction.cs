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
using Gtk;

namespace TraceLab.UI.GTK
{
    class SaveExperimentAction : IActionHandler
    {
        private ApplicationContext m_applicationContext;

        public SaveExperimentAction(ApplicationContext applicationWrapper)
        {
            m_applicationContext = applicationWrapper;
        }
        
        #region IActionHandler Members
        public void Initialize()
        {
            m_applicationContext.Actions.File.Save.Activated += Activated;
        }
        
        public void Uninitialize()
        {
            m_applicationContext.Actions.File.Save.Activated -= Activated;
        }
        #endregion

        /// <summary>
        /// When activated saves current experiment from associated application view model.
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='args'>
        /// Arguments.
        /// </param>
        private void Activated(object sender, EventArgs args)
        {
            //save the experiment of this application view model
            Experiment experiment = m_applicationContext.Application.Experiment;
            
            if (experiment != null)
            {
                string file = null;
                if (String.IsNullOrEmpty(experiment.ExperimentInfo.FilePath))
                {
                    // if file path is not set show save as dialog
                    file = FileDialogs.ShowSaveAsDialog(m_applicationContext.MainWindow.WindowShell);
                }
                else
                {
                    file = experiment.ExperimentInfo.FilePath;
                }
                
                if (file != null)
                {
                    try
                    {
                        ExperimentManager.Save(experiment, file);
                    }
                    catch (System.IO.IOException e)
                    {
                        FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell, e.Message, file);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell, e.Message, file);
                    }
                    catch (Exception e)
                    {
                        FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell, e.Message, file);
                    }
                }
            }
        }
    }
}


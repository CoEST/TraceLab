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

namespace TraceLab.UI.GTK
{
    class SaveExperimentAsAction : IActionHandler
    {
        private ApplicationContext m_applicationContext;

        public SaveExperimentAsAction(ApplicationContext applicationWrapper)
        {
            m_applicationContext = applicationWrapper;
        }
        
        #region IActionHandler Members
        public void Initialize()
        {
            m_applicationContext.Actions.File.SaveAs.Activated += Activated;
        }
        
        public void Uninitialize()
        {
            m_applicationContext.Actions.File.SaveAs.Activated -= Activated;
        }
        #endregion
        
        private void Activated(object sender, EventArgs args)
        {
            //save the experiment of this application view model
            Experiment experiment = m_applicationContext.Application.Experiment;
            
            if (experiment != null)
            {
                string file = FileDialogs.ShowSaveAsDialog(m_applicationContext.MainWindow.WindowShell, 
                                                           experiment.ExperimentInfo.FilePath);

                if (file != null)
                {
                    //remember old guid in case save fails
                    Guid oldId = experiment.ExperimentInfo.GuidId;
                    bool successfulSave = false;
                    try
                    {
                        experiment.ExperimentInfo.GuidId = Guid.NewGuid();

                        //TODO: allow user to set whether referenced files should be copied or not
                        ReferencedFiles referencedFilesProcessing = ReferencedFiles.IGNORE;
                        //try save
                        successfulSave = ExperimentManager.SaveAs(experiment, file, referencedFilesProcessing);
                    }
                    catch (System.IO.IOException e)
                    {
                        FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell, e.Message, file);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell, e.Message, file);
                    }
                    catch (TraceLab.Core.Exceptions.FilesCopyFailuresException e)
                    {
                        //some referenced files failed to be copied... note the experiment still might have been saved as correctly
                        NLog.LogManager.GetCurrentClassLogger().Warn(String.Format("Failed to Save File {0}. {1}", file, e.Message));

                        //TODO
                        //DisplayCopyErrorsWindow(e);
                    }
                    catch (Exception e)
                    {
                        FileDialogs.ShowSaveErrorDialog(m_applicationContext.MainWindow.WindowShell, e.Message, file);
                    }
                    
                    if (successfulSave == true)
                    {
                        //reset the workspace view and logView to the new experiment id
                        //so that both workspace and log view shows the data of the experiment with new id
                        ApplicationViewModel.CreateNewApplicationViewModel(m_applicationContext.Application, experiment);

                        //create new workspace view for the new experiment id
                        m_applicationContext.Application.WorkspaceViewModel =
                            new WorkspaceViewModel((TraceLab.Core.Workspaces.Workspace)m_applicationContext.Application.WorkspaceViewModel,
                                                   experiment.ExperimentInfo.Id);

                        m_applicationContext.Application.LogViewModel = new LogViewModel(experiment.ExperimentInfo.Id, 
                                                                                                  m_applicationContext.Application.LogViewModel);
                    }
                    else
                    {
                        //otherwise don't change the views, but reset the experiment id back to old ID
                        experiment.ExperimentInfo.GuidId = oldId;
                    }
                }
            }
        }
    }
}
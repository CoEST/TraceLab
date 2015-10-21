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
using TraceLab.Core.ViewModels;

namespace TraceLab.UI.GTK
{
    /// <summary>
    /// Application context wraps TraceLab Core ApplicationViewModel.
    /// Wrapper have additional UI specific behavior.
    /// There is one application context per window. Each window have assigned one experiment
    /// with view models to ComponentsLibrary, Workspace itp for that particular experiment.
    /// Although behind the scene there is one ComponentLibrary and one Workspace, the application view 
    /// model provides view into it per experiment.
    /// </summary>
    public class ApplicationContext
    {
        public MainWindow MainWindow { get; private set; }
        public ActionManager Actions { get; private set; }
        public ApplicationViewModel Application { get; private set; }
        public NodeConnectionControlFactory NodeConnectionControlFactory { get; private set; }
        public NodeControlFactory NodeControlFactory { get; private set; }

        static ApplicationContext()
        {
            //init additional WPF extension to check for UI
            TraceLab.Core.Workspaces.WorkspaceUIAssemblyExtensions.Extensions = 
                new string[] { ".UI.GTK.dll", 
                TraceLab.Core.Workspaces.WorkspaceUIAssemblyExtensions.DEFAULT_EXTENSION 
            };
        }

        public ApplicationContext(ApplicationViewModel applicationViewModel)
        {
            Application = applicationViewModel;
            Actions = new ActionManager(this);
            NodeConnectionControlFactory = new NodeConnectionControlFactory(this);
            NodeControlFactory = new NodeControlFactory(this);
        }

        public void SetApplicationModel(ApplicationViewModel newApplicationViewModel) 
        {
            if(MainWindow == null) 
            {
                throw new InvalidOperationException("Application Context has not initialized its Window, thus the application view model cannot be set!");
            }
            Application = newApplicationViewModel;
            MainWindow.SetApplicationViewModel(newApplicationViewModel);
        }

        public void InitializeWindow()
        {
            Actions.RegisterHandlers ();
            MainWindow = new MainWindow(this);
        }

        #region OpenExperiment
                
        /// <summary>
        /// Opens the given applicationViewModel in the current window IF (and only if) the current window has not been used yet. 
        /// Otherwise, opens it in a new window.
        /// </summary>
        /// <param name="newApplicationViewModel">The new application view model.</param>
        public void OpenInWindow(ApplicationViewModel newApplicationViewModel)
        {
            var experiment = this.Application.Experiment;

            if (experiment == null)
            {
                OpenInCurrentWindow(newApplicationViewModel);
            }
            else
            {
                OpenNewExperimentWindow(newApplicationViewModel);
            }
        }

        /// <summary>
        /// Opens the experiment in the current window.
        /// All pads refresh their information according to new application view model.
        /// </summary>
        /// <param name='newApplicationViewModel'>
        /// New application view model.
        /// </param>
        private void OpenInCurrentWindow(ApplicationViewModel newApplicationViewModel)
        {
            this.SetApplicationModel(newApplicationViewModel);
        }
        
        /// <summary>
        /// Opens the new window based on the given application view model.
        /// </summary>
        /// <param name="newApplicationViewModel">The new application view model.</param>
        private static void OpenNewExperimentWindow(ApplicationViewModel newApplicationViewModel)
        {
            ApplicationContext app = new ApplicationContext(newApplicationViewModel);
            app.InitializeWindow();
        }

        #endregion
    }
}


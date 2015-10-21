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
using MonoDevelop.Components.Docking;
using TraceLab.Core.ViewModels;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    public class MainWindow
    {
        // keeps track of how many main windows are open
        private static int m_mainWindowCounter = 0;
        private DockFrame m_dockFrame;
        private ApplicationContext m_applicationContext;
        private InfoPanelFactory m_infoPanelFactory;
        private static string CLOSE_MAIN_WINDOW_WARN_MSG = "Experiment has been modified. Are you sure you want to close without saving?";

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.MainWindow"/> class.
        /// </summary>
        public MainWindow (ApplicationContext dataContext)
        {
            m_applicationContext = dataContext;

            // Init dock frame
            m_dockFrame = new DockFrame ();
            m_dockFrame.CompactGuiLevel = 5;

            // Init component info panel
            m_infoPanelFactory = new InfoPanelFactory (m_applicationContext, m_dockFrame);

            //build the window
            CreateWindow ();
            
            //set window application view model to all UI panels in current chrome
            //note, that application vm should not be set before m_windowShell.ShowAll is called
            SetApplicationViewModel (m_applicationContext.Application);
            
            // initiate action handlers for all actions in application
            new ActionHandlers (dataContext);
            
            //attach handler to closing event of a main window
            this.WindowShell.DeleteEvent += MainWindow_DeleteEvent;
        
            m_mainWindowCounter++;
        }

        /// <summary>
        /// Sets the application view model to all UI panels and elements in current window
        /// </summary>
        /// <param name='applicationViewModel'>
        /// Application view model.
        /// </param>
        public void SetApplicationViewModel (ApplicationViewModel applicationViewModel)
        {
            // change application view model in all current pads
            foreach (IDockPad pad in ApplicationPads) {
                pad.SetApplicationModel (applicationViewModel);
            }

            //set experiment name
            if (applicationViewModel.Experiment != null) {
                m_applicationContext.Application.PropertyChanged += HandleApplicationPropertyChanged;
                this.WindowShell.Title = applicationViewModel.ExperimentName;
            }
        }

        /// <summary>
        /// Handles the application property changed 
        /// -> in particular update window title if experiment changed
        /// For example when user updates the name using AboutExperimentDialog
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void HandleApplicationPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals ("ExperimentName")) {
                this.WindowShell.Title = (sender as ApplicationViewModel).ExperimentName;
            }
        }

        #region Create Main Window

        /// <summary>
        /// Creates the window.
        /// </summary>
        private void CreateWindow ()
        {
            int width = 1000;
            int height = 750;
            bool maximize = false;

            this.WindowShell = new WindowShell ("TraceLab.GenericWindow", "TraceLab", width, height, maximize);
            
            CreateMainToolBar ();

            CreateMainStatusBar ();

            CreatePanels ();
            
            this.WindowShell.ShowAll ();
        }

        /// <summary>
        /// Creates the main tool bar.
        /// </summary>
        /// <param name='shell'>
        /// Shell.
        /// </param>
        private void CreateMainToolBar ()
        {
            Toolbar main_toolbar = this.WindowShell.CreateToolBar ("main_toolbar"); 
            main_toolbar.IconSize = IconSize.SmallToolbar;
            main_toolbar.ToolbarStyle = ToolbarStyle.Icons;

            m_applicationContext.Actions.CreateToolBar (main_toolbar);
        }

        private void CreateMainStatusBar ()
        {
            this.WindowShell.CreateProgressBar ("main_progress_bar");
        }

        /// <summary>
        /// Creates the panels.
        /// </summary>
        /// <param name='shell'>
        /// Shell.
        /// </param>
        private void CreatePanels ()
        {
            HBox panelContainer = this.WindowShell.CreateLayout ();
            
            CreateDockAndPads (panelContainer);
            panelContainer.ShowAll ();
        }

        /// <summary>
        /// Creates the dock frame and pads.
        /// </summary>
        /// <param name='container'>
        /// Container.
        /// </param>
        private void CreateDockAndPads (HBox container)
        {
            var componentsLibraryPad = new ComponentsLibraryPad ();
            var workspaceWindowPad = new WorkspaceWindowPad ();
            var experimentCanvasPad = new ExperimentCanvasPad (m_applicationContext);
            var outputWindowPad = new OutputWindowPad ();

            IDockPad[] pads = new IDockPad[] {  
                componentsLibraryPad,
                workspaceWindowPad,
                experimentCanvasPad,
                outputWindowPad
            };

            foreach (IDockPad pad in pads) {
                pad.Initialize (m_dockFrame);
            }

            this.ApplicationPads = pads;
            this.ComponentsLibraryPad = componentsLibraryPad;
            this.WorkspaceWindowPad = workspaceWindowPad;
            this.ExperimentCanvasPad = experimentCanvasPad;
            this.OutputWindowPad = outputWindowPad;

            container.PackStart (m_dockFrame, true, true, 0);

            //TODO: save last layout - see how Pinta has done it
//            string layout_file = System.IO.Path.Combine (PintaCore.Settings.GetUserSettingsDirectory(), "layouts.xml");
//            
//            if (System.IO.File.Exists(layout_file))
//            {
//                try
//                {
//                    dock.LoadLayouts(layout_file);
//                }
//                // If parsing layouts.xml fails for some reason, proceed to create the default layout.
//                catch (Exception e)
//                {
//                    System.Console.Error.WriteLine ("Error reading layouts.xml: " + e.ToString());
//                }
//            }
            
            if (!m_dockFrame.HasLayout ("Default"))
                m_dockFrame.CreateLayout ("Default", false);
            
            m_dockFrame.CurrentLayout = "Default";
        }

        #endregion Create Main Window

        /// <summary>
        /// Shows the component info pad.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="defaultLocationX">Default location x for the floating window</param>
        /// <param name="defaultLocationY">Default location y for the floating window</param>
        /// <param name="onVisibleChanged">The action that is executed when visibility of window changes.</param>
        internal void ShowComponentInfoPad (BasicNodeControl component, 
                                            int defaultLocationX, int defaultLocationY, System.Action<Boolean> onVisibleChanged)
        {  
            //delegate creation of component info panel to panel factory
            m_infoPanelFactory.ShowComponentInfoPad (component, defaultLocationX, defaultLocationY, onVisibleChanged);
        }

        internal void HideComponentInfoPad (BasicNodeControl component)
        {
            m_infoPanelFactory.HideComponentInfoPad (component);
        }

        #region UI Elements

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <value>
        /// The main window.
        /// </value>
        public WindowShell WindowShell { 
            get;
            private set;
        }

        public ComponentsLibraryPad ComponentsLibraryPad {
            get;
            private set;
        }

        public WorkspaceWindowPad WorkspaceWindowPad {
            get;
            private set;
        }

        public ExperimentCanvasPad ExperimentCanvasPad {
            get;
            private set;
        }

        public OutputWindowPad OutputWindowPad {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the application pads.
        /// The array is a container for all application window pads such as ComponentLibraryPad
        /// WorkspaceWindowPad, OutputWindowPad, and ExperimentCanvasPad
        /// </summary>
        /// <value>
        /// The application pads.
        /// </value>
        public IDockPad[] ApplicationPads {
            get;
            private set;
        }

        #endregion UI Elements

        /// <summary>
        /// Handler for closing main window event.
        /// The handler activates exit action.
        /// </summary>
        /// <param name='o'>
        /// O.
        /// </param>
        /// <param name='args'>
        /// Arguments.
        /// </param>
        private void MainWindow_DeleteEvent (object o, DeleteEventArgs args)
        {
            bool quitApp = true;


            if (m_applicationContext.Application.Experiment != null && (m_applicationContext.Application.Experiment.IsModified || m_applicationContext.Application.Experiment.ExperimentInfo.IsModified)) {
                //show a new dialog to warn the user he's closing the program without saving
                ResponseType response = ResponseType.None;
    
                Dialog dialog = new Dialog (
                    "Modified Experiment Not Saved",
                    null,
                    DialogFlags.Modal,
                    "Yes", ResponseType.Yes, 
                    "No", ResponseType.No
                );

                dialog.VBox.Add (new Label (CLOSE_MAIN_WINDOW_WARN_MSG)); 
                dialog.ShowAll ();

                response = (ResponseType)dialog.Run ();

                if ((response == ResponseType.Yes)) {
                    m_mainWindowCounter--;

                } else {
                    args.RetVal = true;     
                    quitApp = false;               
                }
                dialog.Destroy ();
              
            } else {
                m_mainWindowCounter--;
            }


            if (m_mainWindowCounter == 0 && quitApp) {
                Application.Quit ();
                args.RetVal = true;

                //destroys all nlog rule targets
                LogViewModel.DestroyLogTargets ();

            }            
        }
    }
}
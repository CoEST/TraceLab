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
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;
using TraceLab.UI.WPF.Views;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TraceLab.Core.Workspaces;

namespace TraceLab.UI.WPF.ViewModels
{
    internal class ApplicationViewModelWrapper : INotifyPropertyChanged
    {
        ApplicationViewModel m_viewModel;

        static ApplicationViewModelWrapper()
        {
            //init additional WPF extension to check for UI
            WorkspaceUIAssemblyExtensions.Extensions = new string[] { ".UI.WPF.dll", WorkspaceUIAssemblyExtensions.DEFAULT_EXTENSION };
        }

        public ApplicationViewModelWrapper(ApplicationViewModel appModel)
        {
            if (appModel == null)
                throw new ArgumentNullException("appModel");

            New = new TraceLab.UI.WPF.Commands.DelegateCommand(NewFunc);
            Open = new TraceLab.UI.WPF.Commands.DelegateCommand(OpenFunc, CanOpenFunc);
            Save = new TraceLab.UI.WPF.Commands.DelegateCommand(SaveFunc, CanSaveFunc);
            SaveAs = new TraceLab.UI.WPF.Commands.DelegateCommand(SaveAsFunc, CanSaveFunc);

            OpenSettings = new TraceLab.UI.WPF.Commands.DelegateCommand(OpenSettingsFunc);

            SetModel(appModel);
        }

        private void SetModel(ApplicationViewModel appModel)
        {
            if (m_viewModel != null)
            {
                m_viewModel.PropertyChanged -= appModel_PropertyChanged;
                m_viewModel.Dispose();
            }

            m_viewModel = appModel;
            m_viewModel.PropertyChanged += appModel_PropertyChanged;

            SetComponentLibrary(m_viewModel);
            SetWorkspace(m_viewModel);
            SetLog(m_viewModel);
            SetSettings(m_viewModel);
            SetBenchmarkWizard(m_viewModel);

            if (m_viewModel.Experiment != null)
            {
                SetExperimentViewModel(m_viewModel.Experiment);
            }
            else
            {
                ExperimentDocumentWrapper = new object[] { new StartPageModel(this) };
            }

            NotifyPropertyChanged("");
        }

        private void SetExperimentViewModel(Experiment experiment)
        {
            var experimentViewModel = new ExperimentViewModel(experiment);
            ExperimentDocumentWrapper = new object[] { experimentViewModel };
        }

        private void SetBenchmarkWizard(ApplicationViewModel appModel)
        {
            if (appModel == null)
                throw new ArgumentNullException("appModel");

            BenchmarkWizardViewModel = new BenchmarkWizardViewModel(appModel.BenchmarkWizard, 
                                                                    appModel.Workspace, 
                                                                    (TraceLab.Core.Components.ComponentsLibrary)appModel.ComponentLibraryViewModel);
        }

        private void SetSettings(ApplicationViewModel appModel)
        {
            if (appModel == null)
                throw new ArgumentNullException("appModel");

            SettingsViewModel = new SettingsViewModel(appModel.Settings, ComponentLibraryViewModel);
        }

        private void SetLog(ApplicationViewModel appModel)
        {
            if (appModel == null)
                throw new ArgumentNullException("appModel");

            if(appModel.LogViewModel != null) 
                LogViewModel = new LogViewModelWrapper(appModel.LogViewModel);
        }

        private void SetWorkspace(ApplicationViewModel appModel)
        {
            if (appModel == null)
                throw new ArgumentNullException("appModel");

            if (appModel.WorkspaceViewModel != null)
            {
                if (WorkspaceViewModel != null)
                {
                    WorkspaceViewModel.Dispose();
                }
                WorkspaceViewModel = new WorkspaceViewModelWrapper(appModel.WorkspaceViewModel);
            }
        }

        private void SetComponentLibrary(ApplicationViewModel appModel)
        {
            if (appModel == null)
                throw new ArgumentNullException("appModel");
 
            ComponentLibraryViewModel = new ComponentsLibraryViewModelWrapper(appModel.ComponentLibraryViewModel);

            if (ComponentLibraryViewModel.IsRescanning == false)
            {
                LoadUserTags();
            }
        }

        private void ComponentLibraryViewModel_Rescanned(object sender, System.EventArgs e)
        {
            if (ExperimentViewModel != null)
            {
                string currentSubLevelID = string.Empty;
                double zoom = 0, translateX = 0, translateY = 0;

                // get the current "path" of open composite components
                var currentExperimentVM = (ExperimentViewModel)ExperimentDocumentWrapper[0];
                var currentSubLevel = currentExperimentVM.CurrentView as SubLevelExperimentViewModel;
                if (currentSubLevel != null)
                    currentSubLevelID = currentSubLevel.GraphIdPath;
                translateX = currentExperimentVM.CurrentView.TranslateX;
                translateY = currentExperimentVM.CurrentView.TranslateY;
                zoom = currentExperimentVM.CurrentView.Zoom;

                // Get the current XY translation and zoom of currently visible level
                var refreshedExperiment = ExperimentManager.ReloadExperiment((Experiment)ExperimentViewModel.TopLevel, (TraceLab.Core.Components.ComponentsLibrary)ComponentLibraryViewModel);

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var newModel = ApplicationViewModel.CreateNewApplicationViewModel(m_viewModel, refreshedExperiment);
                    OpenInCurrentWindow(newModel);

                    // reopen the previously open "path" of composite components
                    currentExperimentVM = (ExperimentViewModel)ExperimentDocumentWrapper[0];
                    if (!string.IsNullOrEmpty(currentSubLevelID))
                        currentExperimentVM.FindSubLevel(currentSubLevelID);

                    currentExperimentVM.CurrentView.Zoom = zoom;
                    currentExperimentVM.CurrentView.TranslateX = translateX;
                    currentExperimentVM.CurrentView.TranslateY = translateY;

                    // set the XY translation and zoom to the previous values.
                }), System.Windows.Threading.DispatcherPriority.Normal, null);
            }
        }

        private void ComponentLibraryViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsRescanning" && ComponentLibraryViewModel.IsRescanning == false)
            {
                LoadUserTags();
            }
        }

        private void appModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplicationViewModel appModel = sender as ApplicationViewModel;
            switch (e.PropertyName)
            {
                case "ComponentLibraryViewModel":
                    SetComponentLibrary(appModel);
                    break;
                case "WorkspaceViewModel":
                    SetWorkspace(appModel);
                    break;
                case "LogViewModel":
                    SetLog(appModel);
                    break;
                case "Settings":
                    SetSettings(appModel);
                    break;
                case "BenchmarkWizard":
                    SetBenchmarkWizard(appModel);
                    break;
                case "ExperimentName":
                    NotifyPropertyChanged("ExperimentName");
                    break;
                case "Experiment":
                    throw new InvalidOperationException("The Experiment of the application wrapper cannot be changed.");
                default:
                    throw new InvalidOperationException("Application wrapper does not properly wrap all properties.");
            }
        }

        public event UnhandledExceptionEventHandler UnhandledException
        {
            add
            {
                m_viewModel.UnhandledException += value;
            }
            remove
            {
                m_viewModel.UnhandledException -= value;
            }
        }

        ComponentsLibraryViewModelWrapper m_componentLibrary;
        public ComponentsLibraryViewModelWrapper ComponentLibraryViewModel
        {
            get { return m_componentLibrary; }
            private set
            {
                if (m_componentLibrary != value)
                {
                    if (m_componentLibrary != null)
                    {
                        m_componentLibrary.PropertyChanged -= ComponentLibraryViewModel_PropertyChanged;
                        m_componentLibrary.Rescanned -= ComponentLibraryViewModel_Rescanned;
                    }  

                    m_componentLibrary = value;

                    if (m_componentLibrary != null)
                    {
                        ComponentLibraryViewModel.PropertyChanged += ComponentLibraryViewModel_PropertyChanged;
                        ComponentLibraryViewModel.Rescanned += ComponentLibraryViewModel_Rescanned;
                    }

                    NotifyPropertyChanged("ComponentLibraryViewModel");
                }
            }
        }

        private WorkspaceViewModelWrapper m_workspaceView;
        public WorkspaceViewModelWrapper WorkspaceViewModel
        {
            get { return m_workspaceView; }
            private set
            {
                if (m_workspaceView != value)
                {
                    m_workspaceView = value;
                    NotifyPropertyChanged("WorkspaceViewModel");
                }
            }
        }

        private SettingsViewModel m_settings;
        public SettingsViewModel SettingsViewModel
        {
            get { return m_settings; }
            private set
            {
                if (m_settings != value)
                {
                    m_settings = value;
                    NotifyPropertyChanged("SettingsViewModel");
                }
            }
        }

        private LogViewModelWrapper m_logs;
        public LogViewModelWrapper LogViewModel
        {
            get { return m_logs; }
            private set
            {
                if (m_logs != value)
                {
                    m_logs = value;
                    NotifyPropertyChanged("LogViewModel");
                }
            }
        }

        private BenchmarkWizardViewModel m_benchmarkWizardViewModel;
        public BenchmarkWizardViewModel BenchmarkWizardViewModel
        {
            get { return m_benchmarkWizardViewModel; }
            private set
            {
                if (m_benchmarkWizardViewModel != value)
                {
                    m_benchmarkWizardViewModel = value;
                    NotifyPropertyChanged("BenchmarkWizardViewModel");
                }
            }
        }

        public void SaveUserTags()
        {
            m_viewModel.SaveUserTags();
        }

        public void LoadUserTags()
        {
            m_viewModel.LoadUserTags();
        }

        public void SaveSettings()
        {
            TraceLab.Core.Settings.Settings.SaveSettings(m_viewModel.Settings);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #region New & Open

        #region New

        private ICommand m_new;
        public ICommand New
        {
            get
            {
                return m_new;
            }
            private set
            {
                m_new = value;
                NotifyPropertyChanged("New");
            }
        }

        private void NewFunc(object param)
        {
            var experiment = ExperimentManager.New();
            Window parentWindow = param as Window;
            NewExperimentDialog dialog = new NewExperimentDialog(parentWindow, this.SettingsViewModel.DefaultExperimentsDirectory);
            dialog.DataContext = experiment.ExperimentInfo;

            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                ApplicationViewModel newApplicationViewModel = ApplicationViewModel.CreateNewApplicationViewModel(m_viewModel, experiment);
                RecentExperimentsHelper.UpdateRecentExperimentList(TraceLab.Core.Settings.Settings.RecentExperimentsPath, experiment.ExperimentInfo.FilePath);
                OpenInWindow(newApplicationViewModel);

                string file = experiment.ExperimentInfo.FilePath;
                try
                {
                    ExperimentManager.Save(experiment, file);
                }
                catch (System.IO.IOException e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                    System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                }
                catch (UnauthorizedAccessException e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                    System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                }
                catch (Exception e)
                {
                    NLog.LogManager.GetCurrentClassLogger().ErrorException(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                    System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                }
            }
        }

        #endregion

        #region Open

        private ICommand m_open;
        public ICommand Open
        {
            get
            {
                return m_open;
            }
            private set
            {
                m_open = value;
                NotifyPropertyChanged("Open");
            }
        }

        private void OpenFunc(object param)
        {
            string file = param as string;
            if (String.IsNullOrWhiteSpace(file))
            {
                file = FileDialogs.GetExperimentFilename<Ookii.Dialogs.Wpf.VistaOpenFileDialog>();
            }

            if (file != null && CanOpenFunc(file))
            {
                OpenExperiment(file);
            }
        }

        private bool CanOpenFunc(object param)
        {
            bool retVal = true;

            if (ExperimentViewModel != null)
            {
                if (ExperimentViewModel.TopLevel.IsExperimentRunning == true)
                {
                    retVal = false;
                }
                else
                {
                    string file = param as string;
                    if (file != null && !System.IO.File.Exists(file))
                    {
                        retVal = false;
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region Save

        private ICommand m_save;
        public ICommand Save
        {
            get
            {
                return m_save;
            }
            private set
            {
                m_save = value;
                NotifyPropertyChanged("Save");
            }
        }

        private void SaveFunc(object param)
        {
            //save the experiment of this application view model
            Experiment experiment = m_viewModel.Experiment;

            if (experiment != null)
            {
                string file;
                if (experiment.ExperimentInfo.FilePath == String.Empty)
                {
                    file = FileDialogs.GetExperimentFilename<Ookii.Dialogs.Wpf.VistaSaveFileDialog>();
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
                        NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                        System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                        System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                    }
                    catch (Exception e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().ErrorException(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                        System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                    }
                }
            }

        }

        private ICommand m_saveAs;
        public ICommand SaveAs
        {
            get
            {
                return m_saveAs;
            }
            private set
            {
                m_saveAs = value;
                NotifyPropertyChanged("SaveAs");
            }
        }

        private void SaveAsFunc(object param)
        {
            //save the experiment of this application view model
            Experiment experiment = m_viewModel.Experiment;

            if (experiment != null)
            {
                ReferencedFiles referencedFilesProcessing;
                string rootDrive = System.IO.Path.GetPathRoot(experiment.ExperimentInfo.FilePath);
                string file = FileDialogs.GetExperimentFilenameSaveAsDialog(rootDrive, out referencedFilesProcessing);
                if (file != null)
                {
                    //remember old guid in case save fails
                    Guid oldId = experiment.ExperimentInfo.GuidId;
                    bool successfulSave = false;
                    try
                    {
                        experiment.ExperimentInfo.GuidId = Guid.NewGuid();
                        //try save
                        successfulSave = ExperimentManager.SaveAs(experiment, file, referencedFilesProcessing);
                    }
                    catch (System.IO.IOException e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                        System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                        System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                    }
                    catch (TraceLab.Core.Exceptions.FilesCopyFailuresException e)
                    {
                        //some referenced files failed to be copied... note the experiment still might have been saved as correctly
                        NLog.LogManager.GetCurrentClassLogger().Warn(String.Format("Failed to Save File {0}. {1}", file, e.Message));

                        DisplayCopyErrorsWindow(e);
                    }
                    catch (Exception e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().ErrorException(String.Format("Failed to Save File {0}. {1}", file, e.Message), e);
                        System.Windows.MessageBox.Show(e.Message, "Failed To Save", MessageBoxButton.OK);
                    }

                    if (successfulSave == true)
                    {
                        //reset the workspace view and logView to the new experiment id
                        //so that both workspace and log view shows the data of the experiment with new id
                        m_viewModel.WorkspaceViewModel = new WorkspaceViewModel((TraceLab.Core.Workspaces.Workspace)m_viewModel.WorkspaceViewModel, experiment.ExperimentInfo.Id);
                        m_viewModel.LogViewModel = new LogViewModel(experiment.ExperimentInfo.Id, m_viewModel.LogViewModel);
                    }
                    else
                    {
                        //otherwise don't change the views, but reset the experiment id back to old ID
                        experiment.ExperimentInfo.GuidId = oldId;
                    }
                }
            }

        }

        private static void DisplayCopyErrorsWindow(TraceLab.Core.Exceptions.FilesCopyFailuresException ex)
        {
            Window errorWindow = new Window();
            var errorControl = new ComponentLibraryErrorDisplay();
            errorWindow.Content = errorControl;
            errorControl.HeaderText = "Some referenced files failed to be copied: ";
            errorControl.Errors = ex.CopyErrors;
            errorWindow.Height = 500;
            errorWindow.Width = 800;
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.IsKeyboardFocusWithin)
                {
                    errorWindow.Owner = window;
                }
            }
            errorWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            errorWindow.Title = "Referenced Files Copy Errors";
            errorWindow.ShowDialog();
        }

        private bool CanSaveFunc(object param)
        {
            if (ExperimentViewModel != null && ExperimentViewModel.TopLevel.IsExperimentRunning == false)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Settings

        private ICommand m_openSettings;
        public ICommand OpenSettings
        {
            get { return m_openSettings; }
            private set
            {
                m_openSettings = value;
                NotifyPropertyChanged("OpenSettings");
            }
        }

        private void OpenSettingsFunc(object param)
        {
            var settingsView = new TraceLab.UI.WPF.Views.SettingsPage();
            settingsView.DataContext = SettingsViewModel;

            var settingsWindow = new System.Windows.Window();
            settingsWindow.Content = settingsView;
            SetWindowOwner(settingsWindow);
            settingsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            settingsWindow.ShowActivated = true;
            settingsWindow.Title = "Settings";
            settingsWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/TraceLab.UI.WPF;component/Resources/Icon_Settings16.png"));
            settingsWindow.ResizeMode = ResizeMode.NoResize;
            settingsWindow.SizeToContent = SizeToContent.WidthAndHeight;

            bool? result = settingsWindow.ShowDialog();
            if (result == true)
            {
                SettingsViewModel.ApplyChanges();
            }
        }

        private static void SetWindowOwner(Window settingsWindow)
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.IsKeyboardFocusWithin)
                {
                    settingsWindow.Owner = window;
                }
            }
        }

        #endregion

        internal void OpenExperiment(string file)
        {
            try
            {
                var experiment = ExperimentManager.Load(file, (TraceLab.Core.Components.ComponentsLibrary)m_viewModel.ComponentLibraryViewModel);
                ApplicationViewModel newApplicationViewModel = ApplicationViewModel.CreateNewApplicationViewModel(m_viewModel, experiment);
                RecentExperimentsHelper.UpdateRecentExperimentList(TraceLab.Core.Settings.Settings.RecentExperimentsPath, file);
                OpenInWindow(newApplicationViewModel);
            }
            catch(TraceLab.Core.Exceptions.ExperimentLoadException ex) 
            {
                string msg = String.Format("Unable to open the file {0}. Error: {1}", file, ex.Message);
                NLog.LogManager.GetCurrentClassLogger().ErrorException(msg, ex);
                MessageBox.Show(msg, "Failed to load experiment", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        /// <summary>
        /// Opens the given applicationViewModel in the current window IF (and only if) the current window has not been used yet.  Otherwise, opens it in a new window.
        /// </summary>
        /// <param name="newApplicationViewModel">The new application view model.</param>
        private void OpenInWindow(ApplicationViewModel newApplicationViewModel)
        {
            if (m_viewModel.Experiment == null || (m_viewModel.Experiment != null && m_viewModel.Experiment.IsModified == false && m_viewModel.Experiment.VertexCount == 2))
            {
                OpenInCurrentWindow(newApplicationViewModel);
            }
            else
            {
                OpenNewExperimentWindow(newApplicationViewModel);
            }
        }

        private void OpenInCurrentWindow(ApplicationViewModel newApplicationViewModel)
        {
            SetModel(newApplicationViewModel);
        }

        /// <summary>
        /// Opens the new window based on the given application view model.
        /// </summary>
        /// <param name="newApplicationViewModel">The new application view model.</param>
        private static void OpenNewExperimentWindow(ApplicationViewModel newApplicationViewModel)
        {
            var wrapper = new ApplicationViewModelWrapper(newApplicationViewModel);
            System.Windows.Window window = new TraceLab.UI.WPF.Views.MainWindow(wrapper);
            window.Show();
        }

        #endregion

        private object[] m_experimentDocumentWrapper;
        /// <summary>
        /// Essentially it ExperimentViewModel of the presented Experiment OR Start Page.
        /// This is the wrapper collection for DockingManager.DocumentsSource, with one experiment document in it.
        /// Normally DockingManager allows several documents opened as tabs, but TraceLab allow one Experiment Document per window.
        /// Decision has been made this way as not to confuse users with subexperiments for composite components presented with crumbs.
        /// </summary>
        public object[] ExperimentDocumentWrapper
        {
            get { return m_experimentDocumentWrapper; }
            private set
            {
                if (m_experimentDocumentWrapper != value)
                {
                    m_experimentDocumentWrapper = value;
                    NotifyPropertyChanged("ExperimentDocumentWrapper");
                    NotifyPropertyChanged("ExperimentViewModel");
                }
            }
        }

        /// <summary>
        /// Gets the experiment associated with this application view model
        /// </summary>
        public ExperimentViewModel ExperimentViewModel
        {
            get { return ExperimentDocumentWrapper[0] as ExperimentViewModel; }
        }

        /// <summary>
        /// Gets the name of the experiment. 
        /// </summary>
        /// <value>
        /// The name of the experiment.
        /// </value>
        public string ExperimentName
        {
            get
            {
                return m_viewModel.ExperimentName;
            }
        }

        /// <summary>
        /// List of experiments recently opened sorted by date
        /// </summary>
        public RecentExperimentList RecentExperiments
        {
            get { return m_viewModel.RecentExperiments; }
        }

        /// <summary>
        /// List of video links
        /// </summary>
        public List<WebsiteLink> Videos
        {
            get { return m_viewModel.Videos; }
        }

        /// <summary>
        /// List of website links
        /// </summary>
        public List<WebsiteLink> Links
        {
            get { return m_viewModel.Links; }
        }
    }
}

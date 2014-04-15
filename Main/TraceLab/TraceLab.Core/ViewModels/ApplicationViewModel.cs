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
using System.Xml;
using TraceLab.Core.Experiments;
using TraceLab.Core.Workspaces;
using TraceLab.Core.Benchmarks;
using System.Collections.Generic;
using System.Reflection;

namespace TraceLab.Core.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged, IDisposable
    {
        static ApplicationViewModel()
        {
            WebsiteLink.LoadLinksFromXML(null, out s_videos, out s_links);
            s_recentExperiments = RecentExperimentsHelper.LoadRecentExperimentListFromXML(TraceLab.Core.Settings.Settings.RecentExperimentsPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationViewModel"/> class.
        /// Each application view model needs to have access to the actual workspace (not a wrapper), so that
        /// other application models could be created.
        /// The WorkspaceViewModel wrapper exists only if application view model contains the experiment.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="username">The name of the user from the license file (to be displayed by the application).</param>
        public ApplicationViewModel(Workspace workspace)
        {
            Version = Assembly.GetCallingAssembly().GetName().Version.ToString();
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Workspace = workspace;
        }

        public static ApplicationViewModel CreateNewApplicationViewModel(ApplicationViewModel context, Experiment experiment)
        {
            ApplicationViewModel newApplicationViewModel = new ApplicationViewModel(context.Workspace);

            newApplicationViewModel.ComponentLibraryViewModel = context.ComponentLibraryViewModel.Clone();
            newApplicationViewModel.Settings = context.Settings.Clone();
            newApplicationViewModel.BenchmarkWizard = context.BenchmarkWizard;

            newApplicationViewModel.Experiment = experiment;
            newApplicationViewModel.WorkspaceViewModel = new WorkspaceViewModel(newApplicationViewModel.Workspace, experiment.ExperimentInfo.Id);
            newApplicationViewModel.LogViewModel = new LogViewModel(newApplicationViewModel.Experiment.ExperimentInfo.Id, context.LogViewModel);

            return newApplicationViewModel;
        }

        public event UnhandledExceptionEventHandler UnhandledException;

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (UnhandledException != null)
            {
                UnhandledException(sender, e);
            }
        }

        private ComponentLibraryViewModel m_componentLibrary;
        public ComponentLibraryViewModel ComponentLibraryViewModel
        {
            get { return m_componentLibrary; }
            set
            {
                if (m_componentLibrary != value)
                {
                    m_componentLibrary = value;
                    NotifyPropertyChanged("ComponentLibraryViewModel");
                }
            }
        }

        private WorkspaceViewModel m_workspaceViewModel;
        /// <summary>
        /// Gets or sets the workspace view model for the current experiment.
        /// </summary>
        /// <value>
        /// The workspace view model.
        /// </value>
        public WorkspaceViewModel WorkspaceViewModel
        {
            get { return m_workspaceViewModel; }
            set
            {
                if (m_workspaceViewModel != value)
                {
                    m_workspaceViewModel = value;
                    NotifyPropertyChanged("WorkspaceViewModel");
                }
            }
        }

        private Workspace m_workspace;
        public Workspace Workspace
        {
            get { return m_workspace; }
            set
            {
                if (m_workspace != value)
                {
                    m_workspace = value;
                    NotifyPropertyChanged("Workspace");
                }
            }
        }

        private TraceLab.Core.Settings.Settings m_settings;
        public TraceLab.Core.Settings.Settings Settings
        {
            get { return m_settings; }
            set
            {
                if (m_settings != value)
                {
                    m_settings = value;
                    NotifyPropertyChanged("Settings");

                    if (Experiment != null)
                        Experiment.Settings = Settings;
                }
            }
        }

        private LogViewModel m_log;
        public LogViewModel LogViewModel
        {
            get { return m_log; }
            set
            {
                if (m_log != value)
                {
                    m_log = value;
                    NotifyPropertyChanged("LogViewModel");
                }
            }
        }

        /// <summary>
        /// Gets the name of the user read from the key file.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public static string RegisteredUser;

        /// <summary>
        /// Gets or sets the current version of TraceLab.
        /// </summary>
        /// <value>
        /// Version of TraceLab.
        /// </value>
        public static string Version
        {
            get;
            private set;
        }

        #region BenchmarkWizard

        private BenchmarkWizard m_benchmarkWizard;
        public BenchmarkWizard BenchmarkWizard
        {
            get { return m_benchmarkWizard; }
            set
            {
                if (m_benchmarkWizard != value)
                {
                    m_benchmarkWizard = value;
                    NotifyPropertyChanged("BenchmarkWizard");
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private Experiment m_experiment;
        public Experiment Experiment
        {
            get { return m_experiment; }
            set
            {
                if (m_experiment != null)
                    throw new InvalidOperationException("Experiment can be set only once on ApplicationViewModel");

                if (m_experiment != value)
                {
                    DetachExperimentEventsHandlers();

                    m_experiment = value;

                    AttachEventHandlers();

                    NotifyPropertyChanged("Experiment");
                    NotifyPropertyChanged("ExperimentName");

                    if (m_experiment != null)
                    {
                        var settings = TraceLab.Core.Settings.Settings.GetSettings(m_experiment.ExperimentInfo.Id);
                        Settings = settings;
                        settings.ExperimentSettings.LastOpened = DateTime.Now;

                        //also update componentlibraryviewmodel reference to current experiment
                        ComponentLibraryViewModel.Experiment = m_experiment;
                    }
                }
            }
        }

        public string ExperimentName
        {
            get
            {
                string name = "TraceLab - <No experiment selected>";
                if (Experiment != null)
                {
                    string file = string.IsNullOrEmpty(Experiment.ExperimentInfo.FilePath) ? "<Not saved>" : Experiment.ExperimentInfo.FilePath;
                    name = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} : {1}", Experiment.ExperimentInfo.Name, file);

                    if (Experiment.IsModified)
                    {
                        name = "(*) " + name;
                    }
                }

                return name;
            }
        }

        /// <summary>
        /// List of experiments recently opened sorted by date - common across all application view models
        /// </summary>
        private static RecentExperimentList s_recentExperiments;
        public RecentExperimentList RecentExperiments
        {
            get { return s_recentExperiments; }
        }

        /// <summary>
        /// List of video links - common across all application view models
        /// </summary>
        private static List<WebsiteLink> s_videos;
        public List<WebsiteLink> Videos
        {
            get { return s_videos; }
        }

        /// <summary>
        /// List of website links - common across all application view models
        /// </summary>
        private static List<WebsiteLink> s_links;
        public List<WebsiteLink> Links
        {
            get { return s_links; }
        }

        private void AttachEventHandlers()
        {
            // Attach a change listener to the new m_experiment, if it exists.
            if (m_experiment != null)
            {
                m_experiment.PropertyChanged += experiment_PropertyChanged;
                m_experiment.ExperimentInfo.PropertyChanged += SelectedWorkFlowPropertyChanged;

            }
        }

        void experiment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                NotifyPropertyChanged("ExperimentName");
            }
        }

        private void DetachExperimentEventsHandlers()
        {
            // Remove the old event handler from the previous m_experiment
            if (m_experiment != null)
            {
                m_experiment.PropertyChanged -= experiment_PropertyChanged;
                m_experiment.ExperimentInfo.PropertyChanged -= SelectedWorkFlowPropertyChanged;
            }
        }

        // If properties of the m_experiment change that we use to build the selection's name, then we
        // want to fire a notification that the name has changed.
        private void SelectedWorkFlowPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilePath" || e.PropertyName == "Name")
            {
                NotifyPropertyChanged("ExperimentName");
            }
        }

        public void SaveUserTags()
        {
            m_componentLibrary.SaveUserTags(TraceLab.Core.Settings.Settings.UserTagsPath);
        }

        public void LoadUserTags()
        {
            m_componentLibrary.LoadUserTags(TraceLab.Core.Settings.Settings.UserTagsPath);
        }

        #region IDisposable Pattern
        
        ~ApplicationViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                System.AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
                DetachExperimentEventsHandlers();
                if (WorkspaceViewModel != null)
                {
                    WorkspaceViewModel.Dispose();
                }
            }
        }

        #endregion
    }
}

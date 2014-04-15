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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Benchmarks;
using System.ComponentModel;
using TraceLab.Core.Experiments;
using System.Windows.Input;
using TraceLab.UI.WPF.Commands;
using TraceLab.Core.WebserviceAccess;
using TraceLab.Core;

namespace TraceLab.UI.WPF.ViewModels
{
    /// <summary>
    /// The view model is a wrapper around DefiningBenchmark model data. In addition to wrapped properties it contains
    /// view specific commands.
    /// </summary>
    class DefiningBenchmarkViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefiningBenchmarkViewModel"/> class.
        /// </summary>
        /// <param name="baseExperiment">The base experiment represents experiment based on which the benchmark is being defined.</param>
        public DefiningBenchmarkViewModel(Experiment baseExperiment, IEnumerable<string> benchmarksDirectories,
                                          TraceLab.Core.Components.ComponentsLibrary library, 
                                          TraceLab.Core.Workspaces.Workspace workspace,
                                          IEnumerable<string> workspaceTypeDirectories, string webserviceAddress)
        {
            //initialize the model data based on base experiment
            m_definingBenchmark = new DefiningBenchmark(baseExperiment, library, workspace, TraceLab.Core.PackageSystem.PackageManager.Instance, workspaceTypeDirectories, webserviceAddress);
            Define = new DelegateCommand(ExecuteDefine, CanExecuteDefine);
            SelectBenchmarkPath = new DelegateCommand(DoSelectBenchmarkPath);
            m_benchmarksDirectories = benchmarksDirectories;

            //intiate authentication view model
            if (m_definingBenchmark.WebService != null)
            {
                m_authenticationViewModel = new AuthenticationAndUploadViewModel<ContestPublishedResponse>(m_definingBenchmark.WebService,
                                                    m_definingBenchmark.ExecutePublishContest,
                                                    Messages.UploadingContestToWebsite, Messages.ContestPublished);
            }
        }

        private DefiningBenchmark m_definingBenchmark;
        private IEnumerable<string> m_benchmarksDirectories;

        #region Properties

        private AuthenticationAndUploadViewModel<ContestPublishedResponse> m_authenticationViewModel;

        /// <summary>
        /// Gets or sets the authentication view model, that is needed for AuthenticationAndUploadControl 
        /// </summary>
        /// <value>
        /// The authentication view model.
        /// </value>
        public AuthenticationAndUploadViewModel<ContestPublishedResponse> AuthenticationViewModel
        {
            get { return m_authenticationViewModel; }
            set
            {
                if (m_authenticationViewModel != value)
                {
                    m_authenticationViewModel = value;
                    NotifyPropertyChanged("AuthenticationAndUpload");
                }
            }
        }

        public BenchmarkInfo BenchmarkInfo
        {
            get
            {
                return m_definingBenchmark.BenchmarkInfo;
            }
        }

        public List<ExperimentNode> TemplatizableComponents
        {
            get
            {
                return m_definingBenchmark.TemplatizableComponents;
            }
        }

        public ExperimentNode SelectedTemplateNode
        {
            get
            {
                return m_definingBenchmark.SelectedTemplateNode;
            }
            set
            {
                if (m_definingBenchmark.SelectedTemplateNode != value)
                {
                    m_definingBenchmark.SelectedTemplateNode = value;
                    NotifyPropertyChanged("SelectedTemplateNode");
                }
            }
        }

        public List<string> PublishableExperimentResults
        {
            get
            {
                return m_definingBenchmark.PublishableExperimentResults;
            }
        }

   
        /// <summary>
        /// Gets or sets the selected experiment results unitname, which later is loaded from Workspace and is used for
        /// creating metrics definitions, as well, when contest results are published, these variable is going to be uploaded
        /// to webserver.
        /// </summary>
        /// <value>
        /// The selected experiment results unitname.
        /// </value>
        public string SelectedExperimentResultsUnitname
        {
            get { return m_definingBenchmark.SelectedExperimentResultsUnitname; }
            set 
            {
                if (m_definingBenchmark.SelectedExperimentResultsUnitname != value)
                {
                    m_definingBenchmark.SelectedExperimentResultsUnitname = value;
                    m_definingBenchmark.LoadExperimentResultsFromWorkspace(m_definingBenchmark.SelectedExperimentResultsUnitname);
                    ValidateExperimentResults();

                    NotifyPropertyChanged("SelectedExperimentResultsUnitname");
                    NotifyPropertyChanged("SelectedExperimentResults");
                }
            }
        }

        public TraceLabSDK.Types.Contests.TLExperimentResults SelectedExperimentResults
        {
            get { return m_definingBenchmark.SelectedExperimentResults; }
        }

        private void ValidateExperimentResults() 
        {
            ErrorMessage = m_definingBenchmark.ValidateExperimentResults();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contest should be published.
        /// </summary>
        /// <value>
        ///   <c>true</c> publish contest; otherwise, don't publish contest <c>false</c>.
        /// </value>
        public bool PublishContest
        {
            get 
            {
                return m_definingBenchmark.PublishContest;
            }
            set 
            {
                if (m_definingBenchmark.PublishContest != value) 
                {
                    m_definingBenchmark.PublishContest = value;
                    if (value == false)
                    {
                        //reset also publish baseline
                        PublishBaseline = false;
                        //reset experiment results
                        SelectedExperimentResultsUnitname = null;
                        //and potential error
                        ErrorMessage = null;
                    }
                    NotifyPropertyChanged("PublishContest");
                }
            }
        }

        public bool CanPublishContest
        {
            get
            {
                return (m_authenticationViewModel != null);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the baseline resutls should be published.
        /// </summary>
        /// <value>
        ///   <c>true</c> publish baseline; otherwise, don't publish baseline <c>false</c>.
        /// </value>
        public bool PublishBaseline
        {
            get
            {
                return m_definingBenchmark.PublishBaseline;
            }
            set
            {
                if (m_definingBenchmark.PublishBaseline != value)
                {
                    m_definingBenchmark.PublishBaseline = value;
                    if (value == false)
                    {
                        //reset benchmark description
                        BaselineTechniqueDescription = String.Empty;
                    }
                    NotifyPropertyChanged("PublishBaseline");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the baseline resutls should be published.
        /// </summary>
        /// <value>
        ///   <c>true</c> publish baseline; otherwise, don't publish baseline <c>false</c>.
        /// </value>
        public string BaselineTechniqueDescription
        {
            get
            {
                return m_definingBenchmark.BaselineTechniqueDescription;
            }
            set
            {
                if (m_definingBenchmark.BaselineTechniqueDescription != value)
                {
                    m_definingBenchmark.BaselineTechniqueDescription = value;
                    NotifyPropertyChanged("BaselineTechniqueDescription");
                }
            }
        }

        private DefiningBenchmarkDialogState m_currentState;
        /// <summary>
        /// Gets the state of the dialog. Determines which screen is currently displayed in the dialog
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public DefiningBenchmarkDialogState CurrentState
        {
            get
            {
                return m_currentState;
            }
            private set
            {
                if (m_currentState != value)
                {
                    m_currentState = value;
                    NotifyPropertyChanged("CurrentState");
                }
            }
        }

        private string m_errorMessage;

        /// <summary>
        /// Gets or sets the error message (particularly it is used when user does not provide correct Experiment Results)
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage
        {
            get 
            { 
                return m_errorMessage; 
            }
            set 
            {
                if (m_errorMessage != value)
                {
                    m_errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }
                
        #endregion

        #region Commands

        #region Define 

        public ICommand Define
        {
            get;
            private set;
        }

        private void ExecuteDefine(object param)
        {
            var dialog = param as TraceLab.UI.WPF.Views.DefineBenchmarkDialog;
            if (dialog != null)
            {
                if (PublishContest == true)
                {
                    //continue to next screen - authenticate
                    CurrentState = DefiningBenchmarkDialogState.Authentication;
                }
                else
                {
                    //otherwise simply define benchmark, save it on disk and close dialog
                    m_definingBenchmark.Define();
                    dialog.Close();
                }
            }
        }

        private bool CanExecuteDefine(object param)
        {
            bool canExecuteDefine = (String.IsNullOrEmpty(BenchmarkInfo.FilePath) == false && SelectedTemplateNode != null);
            //if contest is to be published, it is required to select also experiment results
            if (PublishContest == true)
            {
                canExecuteDefine = canExecuteDefine && SelectedExperimentResults != null;
            }

            return canExecuteDefine;
        }

        #endregion

        #region Select Benchmark Path

        public ICommand SelectBenchmarkPath
        {
            get;
            private set;
        }

        private void DoSelectBenchmarkPath(object param)
        {
            bool tryagain;
            do
            {
                tryagain = false;
                string filePath = GetBenchmarkFilePathDialog();
                if (filePath != null)
                {
                    tryagain = Utilities.FileOutsideDirectoryWarningBox.ShowWarningBox(filePath, 
                                                                                  m_benchmarksDirectories, 
                                                                                  TraceLab.Core.Messages.DefineBenchmarkOutsideBenchmarkDirectoryWarning);

                    if (tryagain == false)
                    {
                        BenchmarkInfo.FilePath = filePath;
                    }
                }

            } while (tryagain);
        }
        
        private static string GetBenchmarkFilePathDialog()
        {
            var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            fileDialog.Title = "Select benchmark file path";
            fileDialog.CheckFileExists = false;
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".tbml";
            fileDialog.Filter = "Benchmark Files|*.tbml";

            string filePath = null;
            bool? success = fileDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                filePath = fileDialog.FileName;
            }
            return filePath;
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }

    public enum DefiningBenchmarkDialogState : int
    {
        DefineBenchmark,
        Authentication,
        UploadingContest
    }
}

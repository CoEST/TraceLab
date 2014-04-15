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
using TraceLab.Core.Experiments;
using System.ComponentModel;
using System.Windows.Input;
using TraceLab.UI.WPF.Commands;
using TraceLab.Core.Components;
using TraceLabSDK;
using System.Threading;
using System.Windows;
using TraceLab.Core.WebserviceAccess;
using TraceLab.Core;
using TraceLab.Core.Workspaces;

namespace TraceLab.UI.WPF.ViewModels
{
    class BenchmarkWizardViewModel : INotifyPropertyChanged
    {
        private BenchmarkWizard m_benchmarkWizard;
        private Workspace m_workspace;
        private ComponentsLibrary m_library;

        public BenchmarkWizardViewModel(BenchmarkWizard benchmarkWizard, Workspace workspace, ComponentsLibrary library)
        {
            if (benchmarkWizard == null)
                throw new ArgumentNullException("benchmarkWizard", "Wrapped benchmark wizard cannot be null");
            if (workspace == null)
                throw new ArgumentNullException("workspace");
            if (library == null)
                throw new ArgumentNullException("library");

            AdvanceState = new DelegateCommand(DoAdvanceState, CanAdvanceState);
            BacktrackState = new DelegateCommand(DoBacktrackState, CanBacktrackState);
            Process = new DelegateCommand(DoProcess, DoCanProcess);
            DownloadContestPackage = new DelegateCommand(ExecuteDownloadContestPackage);
           
            m_benchmarkWizard = benchmarkWizard;
            m_workspace = workspace;
            m_library = library;

            m_benchmarkWizard.PropertyChanged += OnWrappedBenchmarkWizardPropertyChanged;
        }

        void OnWrappedBenchmarkWizardPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //propagate Property changed
            NotifyPropertyChanged(e.PropertyName);
        }

        public List<Benchmark> Benchmarks
        {
            get { return m_benchmarkWizard.Benchmarks; }
        }

        private AuthenticationAndUploadViewModel<ContestResultsPublishedResponse> m_authenticationViewModel;

        /// <summary>
        /// Gets or sets the authentication view model, that is needed for AuthenticationAndUploadControl 
        /// </summary>
        /// <value>
        /// The authentication view model.
        /// </value>
        public AuthenticationAndUploadViewModel<ContestResultsPublishedResponse> AuthenticationViewModel
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

        public Benchmark SelectedBenchmark
        {
            get { return m_benchmarkWizard.SelectedBenchmark; }
            set
            {
                if (m_benchmarkWizard.SelectedBenchmark != value)
                {
                    if (m_benchmarkWizard.SelectedBenchmark != null)
                    {
                        DetachHandlers();
                        m_benchmarkWizard.SelectedBenchmark.PropertyChanged -= OnSelectedBenchmarkPropertyChanged;
                    }

                    m_benchmarkWizard.SelectedBenchmark = value;
                    //check if component template is not null... it might be null in case if benchmark has not been downloaded yet
                    if (m_benchmarkWizard.SelectedBenchmark != null) 
                    {
                        if (m_benchmarkWizard.SelectedBenchmark.ComponentTemplate != null)
                        {
                            m_benchmarkWizard.SelectedBenchmark.PrepareMatchingIOByType(m_benchmarkWizard.ExperimentToBeBenchmarked);
                            AttachHandlers();
                        }
                        else
                        {
                            //listen to componenttemplate property change (in case it has been downloaded
                            m_benchmarkWizard.SelectedBenchmark.PropertyChanged += OnSelectedBenchmarkPropertyChanged;
                        }
                    }

                    NotifyPropertyChanged("SelectedBenchmark");
                }
            }
        }

        private void OnSelectedBenchmarkPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ComponentTemplate"))
            {
                m_benchmarkWizard.SelectedBenchmark.PrepareMatchingIOByType(m_benchmarkWizard.ExperimentToBeBenchmarked);
                AttachHandlers();

                //also notify property CanProcess change, so that the Buttons can reevalute their state
                NotifyPropertyChanged("CanProcess");
            }
        }

        public string BenchmarksDirectory
        {
            get { return m_benchmarkWizard.BenchmarksDirectory; }
        }

        public void StartWizard(Experiment experimentToBeBenchmarked)
        {
            CurrentState = BenchmarkWizardState.SelectBenchmark;
            try
            {
                m_benchmarkWizard.StartWizard(experimentToBeBenchmarked);
            }
            catch (ArgumentException e)
            {
                NLog.LogManager.GetCurrentClassLogger().ErrorException("Failed to open wizard because ", e);
                System.Windows.MessageBox.Show(e.Message, "Failed to open wizard.", MessageBoxButton.OK);
            }
        }


        private BenchmarkWizardState m_currentState;
        public BenchmarkWizardState CurrentState
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
                    NotifyPropertyChanged("CanProcess");
                }
            }
        }

        private bool m_benchmarkExperimentCompleted;

        /// <summary>
        /// Gets or sets a value indicating whether the benchmark experiment has been executed and completed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if benchmark experiment has been completed; otherwise, <c>false</c>.
        /// </value>
        public bool BenchmarkExperimentCompleted
        {
            get { return m_benchmarkExperimentCompleted; }
            set 
            {
                if (m_benchmarkExperimentCompleted != value)
                {
                    m_benchmarkExperimentCompleted = value;
                    NotifyPropertyChanged("BenchmarkExperimentCompleted");
                }
            }
        }

        private bool m_publishResults;

        /// <summary>
        /// Gets or sets a value indicating whether the results should be published to website
        /// </summary>
        /// <value>
        ///   <c>true</c> if [publish results]; otherwise, <c>false</c>.
        /// </value>
        public bool PublishResults
        {
            get { return m_publishResults; }
            set 
            {
                if (m_publishResults != value)
                {
                    m_publishResults = value;
                    NotifyPropertyChanged("PublishResults");
                }
            }
        }

        public string TechniqueName
        {
            get { return m_benchmarkWizard.TechniqueName; }
            set 
            {
                m_benchmarkWizard.TechniqueName = value;
                //do not notify property changed, as wrapped property already notifies, and the change is propagated
                //see OnWrappedBenchmarkWizardPropertyChanged
            }
        }

        public string TechniqueDescription
        {
            get { return m_benchmarkWizard.TechniqueDescription; }
            set 
            {
                m_benchmarkWizard.TechniqueDescription = value;
                //do not notify property changed, as wrapped property already notifies, and the change is propagated
                //see OnWrappedBenchmarkWizardPropertyChanged
            }
        }
        
        public string ErrorMessage
        {
            get { return m_benchmarkWizard.ErrorMessage; }
            set
            {
                m_benchmarkWizard.ErrorMessage = value;
                //do not notify property changed, as wrapped property already notifies, and the change is propagated
                //see OnWrappedBenchmarkWizardPropertyChanged
            }
        }

        public ICommand AdvanceState
        {
            get;
            private set;
        }

        public ICommand BacktrackState
        {
            get;
            private set;
        }

        public ICommand Process
        {
            get;
            private set;
        }

        public IProgress Progress
        {
            get;
            set;
        }

        private ExperimentViewModel m_benchmarkExperiment;
        public ExperimentViewModel BenchmarkExperiment
        {
            get { return m_benchmarkExperiment; }
            private set
            {
                if (m_benchmarkExperiment != value)
                {
                    m_benchmarkExperiment = value;
                    NotifyPropertyChanged("BenchmarkExperiment");
                }
            }
        }

        public bool CanProcess
        {
            get
            {
                bool canProcess = CurrentState == BenchmarkWizardState.Configuration;
                if (SelectedBenchmark != null)
                {
                    foreach (BenchmarkItemSetting<IOItem> setting in SelectedBenchmark.BenchmarkInputSetting)
                    {
                        canProcess &= setting.SelectedSetting != null;
                    }

                    foreach (BenchmarkItemSetting<IOItem> setting in SelectedBenchmark.BenchmarkOutputsSetting)
                    {
                        canProcess &= setting.SelectedSetting != null;
                    }
                }

                return canProcess;
            }
        }

        private bool CanAdvanceState(object param)
        {
            //if state is process allow advancing only when experiment is completed and if contest is online
            if (CurrentState == BenchmarkWizardState.Process)
                return BenchmarkExperimentCompleted && SelectedBenchmark.IsOnlineContest;

            if (CurrentState == BenchmarkWizardState.QuestionToPublishResults)
                return !String.IsNullOrEmpty(TechniqueName) && !String.IsNullOrEmpty(TechniqueDescription);

            if (CurrentState == BenchmarkWizardState.AuthenticationAndUpload)
                return false; //cannot advance anymore so disable button

            return SelectedBenchmark != null && SelectedBenchmark.ComponentTemplate != null && CurrentState != BenchmarkWizardState.Process;
        }

        private void DoAdvanceState(object param)
        {
            var state = CurrentState;
            
            if (state == BenchmarkWizardState.QuestionToPublishResults)
            {
                //intiate authentication view model
                AuthenticationViewModel = new AuthenticationAndUploadViewModel<ContestResultsPublishedResponse>(m_benchmarkWizard.WebService,
                                                m_benchmarkWizard.ExecutePublishResults,
                                                Messages.UploadingContestResultsToWebsite, Messages.ContestResultsPublished);
            }

            if (state != BenchmarkWizardState.AuthenticationAndUpload)
            {
                CurrentState = ++state;
            }
        }

        private bool CanBacktrackState(object param)
        {
            //don't allow backtrace once you are in the authentication screen
            return CurrentState != BenchmarkWizardState.SelectBenchmark && CurrentState != BenchmarkWizardState.AuthenticationAndUpload;
        }

        private void DoBacktrackState(object param)
        {
            var state = CurrentState;
            if (state != BenchmarkWizardState.SelectBenchmark)
            {
                CurrentState = --state;
            }
        }

        private bool DoCanProcess(object param)
        {
            return CanProcess;
        }

        public ICommand DownloadContestPackage
        {
            get;
            private set;
        }

        private void ExecuteDownloadContestPackage(object param)
        {
            //the parameters are expected to be the Benchmark, and the progress bar, and clicked button
            var parameters = param as List<object>;
            var benchmark = parameters[0] as Benchmark;
            var downloadProgressControl = parameters[1] as TraceLab.UI.WPF.Controls.ProgressControl;
            var downloadButton = parameters[2] as System.Windows.Controls.Button;
            if (benchmark != null && downloadProgressControl != null && downloadButton != null)
            {
                //change visibility of a button
                downloadButton.Visibility = Visibility.Collapsed;
                downloadProgressControl.Visibility = Visibility.Visible;
                try
                {
                    m_benchmarkWizard.DownloadContestPackage(downloadProgressControl, benchmark);
                }
                catch (ArgumentNullException)
                {
                    //although UI should prevent user trying to download contest by not showing the download button, catch exceptions just in case
                    benchmark.ErrorMessage = Messages.WebserviceUrlEmptyError; 
                }
                catch (UriFormatException)
                {
                    //although UI should prevent user trying to download contest by not showing the download button, catch exceptions just in case
                    benchmark.ErrorMessage = Messages.WebserviceUrlMalformedError;
                }
                catch (Exception ex)
                {
                    benchmark.ErrorMessage = String.Format(Messages.WebserviceAccessError, ex.Message);
                }
            }
        }

        class FakeProgress : IProgress
        {
            #region IProgress Members

            public bool IsIndeterminate
            {
                get
                {
                    return false;
                }
                set
                {
                }
            }

            public double NumSteps
            {
                get
                {
                    return 0;
                }
                set
                {
                }
            }

            public double CurrentStep
            {
                get
                {
                    return 0;
                }
                set
                {
                }
            }

            public string CurrentStatus
            {
                get
                {
                    return string.Empty;
                }
                set
                {
                }
            }

            public void Reset()
            {
            }

            public void Increment()
            {
            }

            public void SetError(bool hasError)
            {
            }

            #endregion
        }


        private void DoProcess(object param)
        {
            try
            {
                BenchmarkExperimentCompleted = false;
                IProgress progress = param as IProgress;
                CurrentState = BenchmarkWizardState.Process;
                m_benchmarkWizard.PrepareBenchmarkExperiment(SelectedBenchmark);
                ExperimentViewModel model = new ExperimentViewModel(SelectedBenchmark.BenchmarkExperiment);
                BenchmarkExperiment = model;
                var baseline = SelectedBenchmark.LoadBaseline();
                SelectedBenchmark.BenchmarkExperiment.ExperimentCompleted += OnExperimentCompleted;
                SelectedBenchmark.BenchmarkExperiment.RunExperiment(progress, m_workspace, m_library, baseline);
            }
            catch (TraceLab.Core.Exceptions.ExperimentLoadException ex)
            {
                NLog.LogManager.GetCurrentClassLogger().ErrorException("Unable to load the benchmark file.", ex);
                MessageBox.Show("Unable to load the benchmark file. Error: " + ex.Message);
            }
        }

        void OnExperimentCompleted(object sender, ExperimentEventArgs e)
        {
            //update property BenchmarkExperiment completed using main gui dispatcher so 
            //that it enables button immediately
            Application.Current.Dispatcher.Invoke(
                new Action(() => { BenchmarkExperimentCompleted = true; })
            );

            SelectedBenchmark.BenchmarkExperiment.ExperimentCompleted -= OnExperimentCompleted;
        }

        void AttachHandlers()
        {
            foreach (BenchmarkItemSetting<IOItem> setting in SelectedBenchmark.BenchmarkInputSetting)
            {
                setting.PropertyChanged += new PropertyChangedEventHandler(setting_PropertyChanged);
            }

            foreach (BenchmarkItemSetting<IOItem> setting in SelectedBenchmark.BenchmarkOutputsSetting)
            {
                setting.PropertyChanged += new PropertyChangedEventHandler(setting_PropertyChanged);
            }
        }

        void DetachHandlers()
        {
            foreach (BenchmarkItemSetting<IOItem> setting in SelectedBenchmark.BenchmarkInputSetting)
            {
                setting.PropertyChanged -= new PropertyChangedEventHandler(setting_PropertyChanged);
            }

            foreach (BenchmarkItemSetting<IOItem> setting in SelectedBenchmark.BenchmarkOutputsSetting)
            {
                setting.PropertyChanged -= new PropertyChangedEventHandler(setting_PropertyChanged);
            }
        }

        void setting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((DelegateCommand)Process).RaiseCanExecuteChanged();
            NotifyPropertyChanged("CanProcess");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            // Trigger re-evaluation of the commands whenever a property changes.
            ((DelegateCommand)BacktrackState).RaiseCanExecuteChanged();
            ((DelegateCommand)AdvanceState).RaiseCanExecuteChanged();
            ((DelegateCommand)Process).RaiseCanExecuteChanged();

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }

    public enum BenchmarkWizardState : int
    {
        SelectBenchmark,
        Configuration,
        Process, //state while experiment is processing
        QuestionToPublishResults,
        AuthenticationAndUpload
    }
}

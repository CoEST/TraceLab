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
using TraceLab.Core.Experiments;
using TraceLab.Core.Components;
using System.Xml;
using TraceLab.Core.Workspaces;
using TraceLabSDK;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.WebserviceAccess;
using System.ComponentModel;
using System.IO;

namespace TraceLab.Core.Benchmarks
{
    public class BenchmarkWizard : INotifyPropertyChanged
    {
        public BenchmarkWizard(string benchmarkDirectory, ComponentsLibrary library, Workspace workspace,
                               List<string> workspaceTypeDirectories, string dataRoot, TraceLab.Core.Settings.Settings settings)
        {
            m_workspace = workspace;
            m_componentsLibrary = library;
            m_dataRoot = dataRoot;
            m_settings = settings;
            BenchmarksDirectory = benchmarkDirectory;

        }

        private Workspace m_workspace;
        private ComponentsLibrary m_componentsLibrary;
        private string m_dataRoot;
        private TraceLab.Core.Settings.Settings m_settings;
        
        private WebserviceAccessor m_webService;
        public WebserviceAccessor WebService
        {
            get
            {
                return m_webService;
            }
        }
        
        public void StartWizard(Experiment experimentToBeBenchmarked)
        {
            if (experimentToBeBenchmarked == null)
                throw new InvalidOperationException("Benchmark cannot be run with null experiment.");

            //if webservice is set access webservice, and load benchmarks from there... on OnRetrieveListOfBenchmarksCallCompleted will load load also local benchmarks
            //and discover those that are online contests
            if (m_settings.WebserviceAddress != null)
            {
                m_webService = new WebserviceAccessor(m_settings.WebserviceAddress, true);

                if (m_webService != null)
                {
                    var listOfContestsCallback = new Callback<ListOfContestsResponse>();
                    listOfContestsCallback.CallCompleted += OnRetrieveListOfBenchmarksCallCompleted;
                    m_webService.RetrieveListOfContests(listOfContestsCallback);
                }
                else
                {
                    //load only local benchmarks
                    Benchmarks = BenchmarkLoader.LoadBenchmarksInfo(BenchmarksDirectory);
                }
            }
            else
            {
                //load only local benchmarks
                Benchmarks = BenchmarkLoader.LoadBenchmarksInfo(BenchmarksDirectory);
            }
            ExperimentToBeBenchmarked = experimentToBeBenchmarked;
        }

        private string m_techniqueName;

        public string TechniqueName
        {
            get { return m_techniqueName; }
            set
            {
                if (m_techniqueName != value)
                {
                    m_techniqueName = value;
                    NotifyPropertyChanged("TechniqueName");
                }
            }
        }

        private string m_techniqueDescription;

        public string TechniqueDescription
        {
            get { return m_techniqueDescription; }
            set
            {
                if (m_techniqueDescription != value)
                {
                    m_techniqueDescription = value;
                    NotifyPropertyChanged("TechniqueDescription");
                }
            }
        }

        private string m_errorMessage;

        public string ErrorMessage
        {
            get { return m_errorMessage; }
            set
            {
                if (m_errorMessage != value)
                {
                    m_errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }

            }
        }


        /// <summary>
        /// Called when [retrieve list of benchmarks call has been completed].
        /// It adds online contests to the list of Benchmarks.
        /// The contests that has been already downloaded and thus were loaded from local directery
        /// get their link to website updated and are marked as the online contests (so that user can compete in them)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="responseArgs">The <see cref="TraceLab.Core.WebserviceAccess.CallCompletedEventArgs&lt;TraceLab.Core.WebserviceAccess.ListOfContestsResponse&gt;"/> instance containing the event data.</param>
        private void OnRetrieveListOfBenchmarksCallCompleted(object sender, CallCompletedEventArgs<ListOfContestsResponse> responseArgs)
        {
            //local Benchmarks has been loaded already
            var localBenchmarks = BenchmarkLoader.LoadBenchmarksInfo(BenchmarksDirectory); ;
            var newOnlineBenchmarks = new List<Benchmark>();

            if (responseArgs.Response.Status == ResponseStatus.STATUS_SUCCESS)
            {
                List<Contest> contests = responseArgs.Response.ListOfContests;

                foreach (Contest contest in contests)
                {
                    //assuming the joomla com_jtracelab components is used, determine the link to contest website
                    string joomlaSiteRootUrl = m_settings.WebserviceAddress.Substring(0, m_settings.WebserviceAddress.IndexOf("administrator"));
                    string linkToContest = JoomlaLinks.GetContestWebpageLink(joomlaSiteRootUrl, contest.ContestIndex);
                    
                    //check if there is already local benchmark with the same guid
                    var benchmark = localBenchmarks.Find((b) => { return b.BenchmarkInfo.Id.Equals(contest.ContestGUID); });
                    if (benchmark == null)
                    {
                        //if benchmark has not been found create new benchmark (note, its template component is empty at this moment)
                        var newOnlineBenchmark = new Benchmark(contest, linkToContest, BenchmarksDirectory);
                        newOnlineBenchmarks.Add(newOnlineBenchmark);
                    }
                    else
                    {
                        //if found, only update the link to website
                        benchmark.BenchmarkInfo.WebPageLink = new Uri(linkToContest);
                        //mark it as online contest so that wizard now if it should give option to user to publish results
                        benchmark.IsOnlineContest = true;
                    }
                }

                //all new benchmarks add to total list of benchmarks
                localBenchmarks.AddRange(newOnlineBenchmarks);
            }
            else
            {
                //log the error, and continue
                string error = String.Format(Messages.RetrievingListOfContestsFailedWarning, responseArgs.Response.ErrorMessage);
                NLog.LogManager.GetCurrentClassLogger().Warn(error);
                ErrorMessage = error;
            }

            Benchmarks = localBenchmarks; 
        }
        
        public void DownloadContestPackage(IProgress progress, Benchmark benchmark)
        {
            var contestPackageResponseCallback = new BenchmarkDownloadCallback(benchmark);
            contestPackageResponseCallback.CallCompleted += OnContestPackageDownloadCompleted;
            contestPackageResponseCallback.Progress = progress;
            WebService.DownloadContestPackage(benchmark.BenchmarkInfo.Id, contestPackageResponseCallback);
        }

        internal void OnContestPackageDownloadCompleted(object sender, CallCompletedEventArgs<ContestPackageResponse> responseArgs)
        {
            BenchmarkDownloadCallback callback = sender as BenchmarkDownloadCallback;
            var benchmark = callback.Benchmark;

            if (responseArgs.Response.Status == ResponseStatus.STATUS_SUCCESS)
            {
                //do error handling of file writing and deserialization
                try
                {
                    //save the file to benchmark directory into its filepath
                    File.WriteAllBytes(benchmark.BenchmarkInfo.FilePath, Convert.FromBase64String(responseArgs.Response.ContestPackage));

                    //load benchmark info to update its experiment results unitname
                    string resultsUnitname = BenchmarkLoader.ReadExperimentResultsUnitname(benchmark.BenchmarkInfo.FilePath);
                    if (resultsUnitname != null)
                    {
                        benchmark.BenchmarkInfo.ExperimentResultsUnitname = resultsUnitname;

                        //read the file to determine ComponentTemplate
                        ComponentTemplateMetadata template = BenchmarkLoader.FindTemplateComponentMetadata(benchmark.BenchmarkInfo.FilePath);

                        //set the benchmark 
                        if (template != null)
                        {
                            benchmark.ComponentTemplate = template;
                        }
                        else
                        {
                            callback.Progress.Reset();
                            callback.Progress.SetError(true);
                            callback.Progress.CurrentStatus = "Error!";
                            benchmark.ErrorMessage = Messages.ComponentTemplateNotFoundInContestError;
                        }
                    }
                    else
                    {
                        callback.Progress.Reset();
                        callback.Progress.SetError(true);
                        callback.Progress.CurrentStatus = "Error!";
                        benchmark.ErrorMessage = Messages.ExperimentResultsUnitnameNotDefined;
                    }
                }
                catch (System.Xml.XmlException)
                {
                    callback.Progress.Reset();
                    callback.Progress.SetError(true);
                    callback.Progress.CurrentStatus = "Error!";
                    benchmark.ErrorMessage = Messages.ContestFileDeserializationError;
                }
                catch (UnauthorizedAccessException ex)
                {
                    callback.Progress.Reset();
                    callback.Progress.SetError(true);
                    callback.Progress.CurrentStatus = "Error!";
                    benchmark.ErrorMessage = String.Format(Messages.ContestFileSaveError, ex.Message);
                }
                catch (IOException ex)
                {
                    callback.Progress.Reset();
                    callback.Progress.SetError(true);
                    callback.Progress.CurrentStatus = "Error!";
                    benchmark.ErrorMessage = String.Format(Messages.ContestFileSaveError, ex.Message);
                }
                catch (System.Security.SecurityException ex)
                {
                    callback.Progress.Reset();
                    callback.Progress.SetError(true);
                    callback.Progress.CurrentStatus = "Error!";
                    benchmark.ErrorMessage = String.Format(Messages.ContestFileSaveError, ex.Message);
                }
                catch (System.Exception ex)
                {
                    callback.Progress.Reset();
                    callback.Progress.SetError(true);
                    callback.Progress.CurrentStatus = "Error!";
                    benchmark.ErrorMessage = String.Format(Messages.ContestDownloadError, ex.Message);
                }
            }
            else //response status different than SUCCESS
            {
                //propagate error from the response args
                callback.Progress.Reset();
                callback.Progress.SetError(true);
                callback.Progress.CurrentStatus = "Error!";
                benchmark.ErrorMessage = responseArgs.Response.ErrorMessage;
            }
        }
        
        private List<Benchmark> m_benchmarks;
        public List<Benchmark> Benchmarks
        {
            get { return m_benchmarks; }
            private set
            {
                if (m_benchmarks != value)
                {
                    m_benchmarks = value;
                    NotifyPropertyChanged("Benchmarks");
                }
            }
        }

        private Benchmark m_selectedBenchmark;

        public Benchmark SelectedBenchmark
        {
            get { return m_selectedBenchmark; }
            set { m_selectedBenchmark = value; }
        }
        

        private string m_benchmarksDirectory;
        public string BenchmarksDirectory
        {
            get { return m_benchmarksDirectory; }
            private set
            {
                if (m_benchmarksDirectory != value)
                {
                    m_benchmarksDirectory = value;
                    NotifyPropertyChanged("BenchmarksDirectory");
                }
            }
        }

        private Experiment m_experimentToBeBenchmarked;
        public Experiment ExperimentToBeBenchmarked
        {
            get { return m_experimentToBeBenchmarked; }
            private set 
            {
                if (m_experimentToBeBenchmarked != value)
                {
                    m_experimentToBeBenchmarked = value;
                    NotifyPropertyChanged("ExperimenToBeBenchmarked");
                }
            }
        }

        /// <summary>
        /// Prepares the benchmark experiment.
        /// 1. First it deserialize the benchmark experiment
        /// 2. Secondly replaces template component node with provided composite component node
        /// </summary>
        /// <param name="selectedBenchmarkFile">The selected benchmark file.</param>
        /// <exception cref="TraceLab.Core.Exceptions.ExperimentLoadException">throws if benchmark experiment load fails</exception>
        /// <param name="experimentToBeBenchmarked">The experiment to be benchmarked.</param>
        /// <returns></returns>
        public void PrepareBenchmarkExperiment(Benchmark selectedBenchmark)
        {
            if (ExperimentToBeBenchmarked == null)
                throw new InvalidOperationException("The wizard has not been started. Benchmark cannot be run with null experiment.");

            selectedBenchmark.PrepareBenchmarkExperiment(ExperimentToBeBenchmarked, m_componentsLibrary);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        public void ExecutePublishResults(string ticket, string username, Callback<ContestResultsPublishedResponse> publishingResultsCallback)
        {   
            //get the unitname to be loaded from workspace
            string experimentResultsUnitname = SelectedBenchmark.BenchmarkInfo.ExperimentResultsUnitname;

            //create an experiment workspace wrapper with experiment info
            var benchmarkWorkspaceWrapper = new ExperimentWorkspaceWrapper(m_workspace, SelectedBenchmark.BenchmarkInfo.Id);

            var experimentResults = benchmarkWorkspaceWrapper.Load(experimentResultsUnitname) as TraceLabSDK.Types.Contests.TLExperimentResults;

            if (experimentResults != null)
            {
                ContestResults results = BenchmarkResultsHelper.PrepareBaselineContestRestults(SelectedBenchmark.BenchmarkInfo.Id,
                                                                                                experimentResults,
                                                                                                TechniqueName, TechniqueDescription);

                WebService.PublishContestResults(ticket, username, results, publishingResultsCallback);
            }
            else
            {
                throw new InvalidOperationException("The experiment results outputed in the contest are empty! Empty results cannot be publish! ");
            }
        }
    }
}

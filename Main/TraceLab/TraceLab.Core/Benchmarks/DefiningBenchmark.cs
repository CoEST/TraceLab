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
using System.Xml.Serialization;
using TraceLab.Core.Components;
using TraceLab.Core.WebserviceAccess;
using TraceLab.Core.Exceptions;
using TraceLab.Core.WebserviceAccess.Metrics;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.Benchmarks
{
    public class DefiningBenchmark
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefiningBenchmark"/> class.
        /// </summary>
        /// <param name="baseExperiment">The base experiment represents experiment based on which the benchmark is being defined.</param>
        public DefiningBenchmark(Experiment baseExperiment, ComponentsLibrary library,
                                 Workspace workspace, PackageSystem.PackageManager manager, 
                                 IEnumerable<string> workspaceTypeDirectories, string webserviceAddress)
        {
            if (baseExperiment == null)
                throw new ArgumentNullException("baseExperiment");
            if (library == null)
                throw new ArgumentNullException("library");
            if (workspace == null)
                throw new ArgumentNullException("workspace");
            if (workspaceTypeDirectories == null)
                throw new ArgumentNullException("workspaceTypeDirectories");

            // these are needed to create experiment serializing in Define method
            m_packageManager = manager;
            m_library = library;
            WebserviceAddress = webserviceAddress;
            if (webserviceAddress != null)
            {
                m_webService = new WebserviceAccessor(webserviceAddress, true);
            }

            m_baseExperiment = baseExperiment;

            m_workspace = workspace;
            //wrap the workspace into experiment workspace wrapper, so that we can load units only from experiment namespace
            m_experimentWorkspaceWrapper = new ExperimentWorkspaceWrapper(workspace, m_baseExperiment.ExperimentInfo.Id);

            //prefil benchmark info
            PrefillBenchmarkInfo();

            //get nodes that can be selected as template 
            GetTemplatizableComponents();

            //get the ExperimentResults variables that can be selected as publishable results
            //from the ExperimentResults all metric names, their descriptions and dataset names are going to be extracted 
            //when contest is going to be published
            GetPublishableExperimentResults();
        }

        private PackageSystem.PackageManager m_packageManager;
        private IComponentsLibrary m_library;
        private Workspace m_workspace;
        private ExperimentWorkspaceWrapper m_experimentWorkspaceWrapper;
        private string m_dataRoot;
        
        private void PrefillBenchmarkInfo()
        {
            m_benchmarkInfo = new BenchmarkInfo();
            m_benchmarkInfo.Name = m_baseExperiment.ExperimentInfo.Name;
            m_benchmarkInfo.Author = m_baseExperiment.ExperimentInfo.Author;
            m_benchmarkInfo.Contributors = m_baseExperiment.ExperimentInfo.Contributors;
            m_benchmarkInfo.Description = m_baseExperiment.ExperimentInfo.Description;
            m_benchmarkInfo.FilePath = null; //assure that original path is null
        }

        private void GetTemplatizableComponents()
        {
            m_templatizableNodes = new List<ExperimentNode>();
            foreach (ExperimentNode node in m_baseExperiment.Vertices)
            {
                if (node is CompositeComponentNode)
                {
                    m_templatizableNodes.Add(node);
                }
            }
        }

        private void GetPublishableExperimentResults()
        {
            m_publishableExperimentResults = new List<string>();
            foreach (ExperimentNode node in m_baseExperiment.Vertices)
            {
                IConfigurableAndIOSpecifiable metadata = node.Data.Metadata as IConfigurableAndIOSpecifiable;
                if (metadata != null)
                {
                    foreach (IOItem outputItem in metadata.IOSpec.Output.Values)
                    {
                        //if output item type is an ExperimentResult it can be published
                        if (outputItem.IOItemDefinition.Type.Equals(typeof(TraceLabSDK.Types.Contests.TLExperimentResults).FullName))
                        {
                            //add it to the list of potential publishable results
                            m_publishableExperimentResults.Add(outputItem.MappedTo);
                        }
                    }
                }
            }
        }


        private Experiment m_baseExperiment;

        private BenchmarkInfo m_benchmarkInfo;
        public BenchmarkInfo BenchmarkInfo
        {
            get
            {
                return m_benchmarkInfo;
            }
        }

        private List<ExperimentNode> m_templatizableNodes;
        public List<ExperimentNode> TemplatizableComponents
        {
            get
            {
                return m_templatizableNodes;
            }
        }

        private ExperimentNode m_selectedTemplateNode;
        public ExperimentNode SelectedTemplateNode
        {
            get
            {
                return m_selectedTemplateNode;
            }
            set
            {
                m_selectedTemplateNode = value;
            }
        }

        private List<string> m_publishableExperimentResults;
        public List<string> PublishableExperimentResults
        {
            get
            {
                return m_publishableExperimentResults;
            }
        }

        /// <summary>
        /// Gets or sets the selected experiment results unitname. 
        /// </summary>
        /// <value>
        /// The selected experiment results unitname.
        /// </value>
        public string SelectedExperimentResultsUnitname
        {
            get { return BenchmarkInfo.ExperimentResultsUnitname; }
            set { BenchmarkInfo.ExperimentResultsUnitname = value; }
        }

        private bool m_publishContest;
        public bool PublishContest 
        {
            get { return m_publishContest; }
            set { m_publishContest = value; }
        }

        private bool m_publishBaseline;
        public bool PublishBaseline
        {
            get { return m_publishBaseline; }
            set { m_publishBaseline = value; }
        }

        private string m_baselineTechniqueDescription;
        public string BaselineTechniqueDescription
        {
            get { return m_baselineTechniqueDescription; }
            set { m_baselineTechniqueDescription = value; }
        }

        /// <summary>
        /// Defines the benchmark and saves the benchmark file to provided in BenchmarkInfo filepath.
        /// 
        /// </summary>
        public void Define()
        {
            if (string.IsNullOrEmpty(BenchmarkInfo.FilePath))
                throw new ArgumentException("Benchmark File Path has not been provided. It cannot be null or empty");
            if (System.IO.Path.IsPathRooted(BenchmarkInfo.FilePath) == false)
                throw new ArgumentException("Benchmark File Path has to be absolute.");
            if (SelectedTemplateNode == null)
                throw new ArgumentNullException("Missing selected template node.");

            Experiment benchmark = (Experiment)m_baseExperiment.Clone();
            
            //save original experiment path
            string originalExperimentPath = benchmark.ExperimentInfo.FilePath;

            //replace experiment info with benchmark info
            benchmark.ExperimentInfo = BenchmarkInfo;
            
            var experimentSerializer = new BenchmarkSerializer(m_library, m_dataRoot);

            //find the node that has to be replaced with ComponentTemplateMetadata
            foreach(ExperimentNode node in benchmark.Vertices) 
            {
                if (node.Equals(SelectedTemplateNode))
                {
                    //get current metadata
                    var currentMetadata = node.Data.Metadata as IConfigurableAndIOSpecifiable;
                    if (currentMetadata != null)
                    {
                        //replace the current metadata with the ComponentTemplateMetadata
                        //provide the same IOSpec that was in current metadata. 
                        //note that configuration is ignored
                        node.Data.Metadata = new ComponentTemplateMetadata(currentMetadata.IOSpec, currentMetadata.Label);
                    }
                }
            }

            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(benchmark.ExperimentInfo.FilePath, settings))
            {
                SelectedExperimentResults.TechniqueName = "BASELINE";
                experimentSerializer.SerializeExperiment(benchmark, SelectedExperimentResults, writer);
            }
        }

        private string m_webserviceAddress;

        /// <summary>
        /// Gets or sets the webservice address. Required for publishing the contest to website
        /// </summary>
        /// <value>
        /// The webservice address.
        /// </value>
        public string WebserviceAddress
        {
            get { return m_webserviceAddress; }
            private set 
            { 
                if (!String.IsNullOrEmpty(value))
                {
                    m_webserviceAddress = value;
                }
            }
        }

        private WebserviceAccessor m_webService;
        public WebserviceAccessor WebService
        {
            get
            {
                return m_webService;
            }
        }

        /// <summary>
        /// Authenticates the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="System.UriFormatException">throws when webservice url is malformed</exception>
        /// <exception cref="System.ArgumentNullException">throws when webservice url is null</exception>
        public void Authenticate(Credentials credentials, Callback<AuthenticationResponse> callback)
        {
            WebService.Authenticate(credentials, callback);
        }

        /// <summary>
        /// Executes the publish contest.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="username">The username.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="System.UriFormatException">throws when webservice url is malformed</exception>
        /// <exception cref="System.ArgumentNullException">throws when webservice url is null</exception>
        public void ExecutePublishContest(string ticket, string username, Callback<ContestPublishedResponse> callback)
        {
            //first execute creation of benchmark file! (it saves the file locally too)
            Define();

            //extract metrics definitions out of selected Experiment Results
            List<MetricDefinition> metrics;
            List<string> datasets;
            BenchmarkResultsHelper.ExtractDatasetsAndMetricsDefinitions(SelectedExperimentResults, out metrics, out datasets);

            //define method creates the benchmark file
            Contest newContest = new Contest(BenchmarkInfo.Id, 
                                             BenchmarkInfo.Name, 
                                             BenchmarkInfo.Author, 
                                             BenchmarkInfo.Contributors, 
                                             BenchmarkInfo.ShortDescription, 
                                             BenchmarkInfo.Description,
                                             BenchmarkInfo.Deadline,
                                             metrics,
                                             datasets,
                                             BenchmarkInfo.FilePath);

            //prepare baseline out of selected Experiment Results if publish baseline is checked
            if (PublishBaseline == true)
            {
                newContest.BaselineResults = BenchmarkResultsHelper.PrepareBaselineContestRestults(newContest.ContestGUID, 
                                                    SelectedExperimentResults, "Baseline", BaselineTechniqueDescription);
            }
            WebService.PublishContest(ticket, username, newContest, callback);
        }

        /// <summary>
        /// Loads the experiment results from workspace.
        /// </summary>
        /// <param name="selectedExperimentResultsUnitname">The selected experiment results unitname.</param>
        public void LoadExperimentResultsFromWorkspace(string selectedExperimentResultsUnitname)
        {
            SelectedExperimentResults = m_experimentWorkspaceWrapper.Load(selectedExperimentResultsUnitname) 
                            as TraceLabSDK.Types.Contests.TLExperimentResults;
        }

        private TraceLabSDK.Types.Contests.TLExperimentResults m_selectedExperimentResults;

        public TraceLabSDK.Types.Contests.TLExperimentResults SelectedExperimentResults
        {
            get { return m_selectedExperimentResults; }
            set { m_selectedExperimentResults = value; }
        }

        public string ValidateExperimentResults()
        {
            string error = null;
            if (SelectedExperimentResults == null)
            {
                error = Messages.ExperimentResultsNotFoundOrNull;
            }
            else
            {
                //if different than null, check if there is at least one dataset and one metric definition
                using (IEnumerator<TraceLabSDK.Types.Contests.DatasetResults> enumerator 
                        = SelectedExperimentResults.DatasetsResults.GetEnumerator())
                {
                    //check just first dataset, as there has to be at least one valid dataset
                    if (enumerator.MoveNext())
                    {
                        do
                        {
                            var datasetResult = enumerator.Current;
                            using (IEnumerator<TraceLabSDK.Types.Contests.Metric> metricEnumerator = datasetResult.Metrics.GetEnumerator())
                            {
                                //check only first metric in each dataset (if there is at least one metric, then it is correct)
                                if (metricEnumerator.MoveNext() == false)
                                {
                                    //no metrics
                                    error = Messages.ExperimentResultsWithNoMetric;
                                }
                            }
                        } while (enumerator.MoveNext());
                    }
                    else
                    {
                        //no datasets
                        error = Messages.ExperimentResultsWithNoDatasets;
                    }
                }
            }

            return error;
        }
    }
}

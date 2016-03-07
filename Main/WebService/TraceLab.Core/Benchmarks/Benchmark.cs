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
using TraceLabSDK;
using TraceLab.Core.WebserviceAccess;
using System.ComponentModel;
using TraceLabSDK.Types.Contests;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.Benchmarks
{
    public class Benchmark : INotifyPropertyChanged
    {
        public Benchmark(BenchmarkInfo benchmarkInfo, ComponentTemplateMetadata componentTemplate)
        {
            if (benchmarkInfo == null)
                throw new ArgumentNullException("benchmarkInfo");
            if (String.IsNullOrEmpty(benchmarkInfo.FilePath))
                throw new ArgumentException("Benchmark file path cannot be null or empty.");
            if (componentTemplate == null)
                throw new ArgumentNullException("componentTemplate");

            BenchmarkInfo = benchmarkInfo;
            ComponentTemplate = componentTemplate;
            IsOnlineContest = false;
        }

        public Benchmark(ExperimentInfo experimentInfo, ComponentTemplateMetadata componentTemplate)
        {
            if (experimentInfo == null)
                throw new ArgumentNullException("benchmarkInfo");
            if (String.IsNullOrEmpty(experimentInfo.FilePath))
                throw new ArgumentException("Benchmark file path cannot be null or empty.");
            if (componentTemplate == null)
                throw new ArgumentNullException("componentTemplate");

            BenchmarkInfo = new BenchmarkInfo(experimentInfo);
            ComponentTemplate = componentTemplate;
            IsOnlineContest = false;
        }

        public Benchmark(Contest contest, string linkToContest, string benchmarksDirectory)
        {
            //determine the full benchmark file path... although file might not have been downloaded yet
            string benchmarkPath = System.IO.Path.Combine(benchmarksDirectory, contest.PackageFileName);

            BenchmarkInfo = new BenchmarkInfo(contest.ContestGUID, contest.Name, 
                                                contest.Author, contest.Contributors, 
                                                contest.Description, contest.ShortDescription,
                                                benchmarkPath, contest.Deadline, linkToContest);
            ComponentTemplate = null;
            IsOnlineContest = true;
        }

        private Experiment m_benchmarkExperiment = null;
        public Experiment BenchmarkExperiment
        {
            get
            {
                return m_benchmarkExperiment;
            }
            private set 
            {
                if (m_benchmarkExperiment != value)
                {
                    m_benchmarkExperiment = value;
                    NotifyPropertyChanged("BenchmarkExperiment");
                }
            }
        }

        private BenchmarkInfo m_benchmarkInfo;
        public BenchmarkInfo BenchmarkInfo
        {
            get { return m_benchmarkInfo; }
            private set
            {
                if (m_benchmarkInfo != value)
                {
                    m_benchmarkInfo = value;
                    NotifyPropertyChanged("BenchmarkInfo");
                }
            }
        }

        private ComponentTemplateMetadata m_componentTemplate;
        /// <summary>
        /// Gets or sets the component template.
        /// Template represents the component to be replaced by the user.
        /// </summary>
        /// <value>
        /// The component template.
        /// </value>
        public ComponentTemplateMetadata ComponentTemplate
        {
            get { return m_componentTemplate; }
            set
            {
                if (m_componentTemplate != value)
                {
                    m_componentTemplate = value;
                    NotifyPropertyChanged("ComponentTemplate");
                }
            }
        }

        private TraceLabSDK.Types.Contests.TLExperimentResults m_baseline;

        /// <summary>
        /// Gets or sets the baseline.
        /// </summary>
        /// <value>
        /// The baseline.
        /// </value>
        public TraceLabSDK.Types.Contests.TLExperimentResults Baseline
        {
            get { return m_baseline; }
            set 
            {
                if (m_baseline != value)
                {
                    m_baseline = value;
                    NotifyPropertyChanged("Baseline");
                }
            }
        }

        private bool m_isOnlineContest;
        /// <summary>
        /// Gets or sets a value indicating whether this benchmark is an online contest.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is online contest; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnlineContest {
            get { return m_isOnlineContest; }
            set
            {
                if (m_isOnlineContest != value)
                {
                    m_isOnlineContest = value;
                    NotifyPropertyChanged("IsOnlineContest");
                }
            }
        }

        private string m_errorMessage;

        /// <summary>
        /// Gets or sets the error message, that can be displayed if downloading the benchmark failed
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

        private BenchmarkSettingCollection<IOItem> m_benchmarkInputSetting = new BenchmarkSettingCollection<IOItem>();
        /// <summary>
        /// Gets the input settings for the experiment that is being benchmarked.
        /// </summary>
        public BenchmarkSettingCollection<IOItem> BenchmarkInputSetting
        {
            get { return m_benchmarkInputSetting; }
        }

        private BenchmarkSettingCollection<IOItem> m_benchmarkOutputsSetting = new BenchmarkSettingCollection<IOItem>();
        /// <summary>
        /// Gets the output settings for the experiment that is being benchmarked.
        /// </summary>
        public BenchmarkSettingCollection<IOItem> BenchmarkOutputsSetting
        {
            get { return m_benchmarkOutputsSetting; }
        }

        public void ClearIO()
        {
            BenchmarkInputSetting.Clear();
            BenchmarkOutputsSetting.Clear();
        }

        private DefiningCompositeComponentSetup m_setup;

        public DefiningCompositeComponentSetup Setup
        {
            get { return m_setup; }
        }


        /// <summary>
        /// Builds the list of candidate inputs and outputs used by the experiment that is being benchmarked
        /// </summary>
        /// <param name="experimentToBeBenchmarked">The experiment to be benchmarked.</param>
        public void PrepareMatchingIOByType(Experiment experimentToBeBenchmarked)
        {
            // Make sure to reset and previous state.
            ClearIO();

            m_setup = new DefiningCompositeComponentSetup(experimentToBeBenchmarked);

            //PROCESS INPUTS
            ProcessIO(ComponentTemplate.IOSpec.Input, m_setup.InputSettings, BenchmarkInputSetting);

            //PROCESS OUTPUTS
            ProcessIO(ComponentTemplate.IOSpec.Output, m_setup.OutputSettings, BenchmarkOutputsSetting);

            ProcessConfig(m_setup.ConfigSettings);
        }

        /// <summary>
        /// Processes the IO.
        /// </summary>
        /// <param name="componentTemplateIODictionary">The io dictionary of either Inputs of Outputs of the component template.</param>
        /// <param name="settings">input or output settings for the composite component that is being defined</param>
        /// <param name="benchmarkMappingSettingsCollection">The benchmark setting collection .</param>
        private static void ProcessIO(IDictionary<string, IOItem> componentTemplateIODictionary, SortedDictionary<string, ItemSetting> settings, BenchmarkSettingCollection<IOItem> benchmarkMappingSettingsCollection)
        {
            MultiDictionary<string, ItemSetting> lookupByType = new MultiDictionary<string, ItemSetting>();

            //first prepare lookup by type 
            foreach (KeyValuePair<string, ItemSetting> pair in settings)
            {
                ItemSetting item = pair.Value;
                //as default don't include it
                item.Include = false;
                lookupByType.Add(item.Type, item);
            }

            foreach (string itemKey in componentTemplateIODictionary.Keys)
            {
                IOItem item = componentTemplateIODictionary[itemKey];

                //check if there are any item settings with matching type
                IEnumerable<ItemSetting> matchingItemSettings;
                if (lookupByType.TryGetValue(item.IOItemDefinition.Type, out matchingItemSettings))
                {
                    ItemSettingCollection list = new ItemSettingCollection(matchingItemSettings);

                    //add all to the candidate matching items - these are items that user can choose from
                    var setting = new BenchmarkItemSetting<IOItem>(item, list);
                    benchmarkMappingSettingsCollection.Add(setting);
                    setting.SelectedSetting = setting.CandidateSettings[0];
                }
                else
                {
                    //add empty list - no matching items - benchmarking cannot be executed
                    var setting = new BenchmarkItemSetting<IOItem>(item, new ItemSettingCollection());
                    benchmarkMappingSettingsCollection.Add(setting);
                }
            }
        }

        private static void ProcessConfig(SortedDictionary<string, ConfigItemSetting> configSettings)
        {
            //don't include any of the config values
            foreach (KeyValuePair<string, ConfigItemSetting> pair in configSettings)
            {
                ConfigItemSetting item = pair.Value;
                //as default don't include it
                item.Include = false;
            }
        }

        /// <summary>
        /// Prepares the benchmark experiment.
        /// </summary>
        /// <param name="experimentToBeBenchmarked">The experiment to be benchmarked.</param>
        /// <exception cref="TraceLab.Core.Exceptions.ExperimentLoadException">throws if experiment load fails</exception>
        /// <param name="library">The library.</param>
        public void PrepareBenchmarkExperiment(Experiment experimentToBeBenchmarked, ComponentsLibrary library)
        {
            //load benchmark experiment
            Experiment benchmarkExperiment = ExperimentManager.Load(BenchmarkInfo.FilePath, library);
            //update benchmark experiment info, so that it refers to the same benchmark info (LoadExperiment would not read BenchmarkInfo, as it only reads ExperimentInfo)
            benchmarkExperiment.ExperimentInfo = BenchmarkInfo; 

            //2. find template node to be replaced
            ExperimentNode templateNode = null;
            foreach (ExperimentNode node in benchmarkExperiment.Vertices)
            {
                if (node.Data.Metadata is ComponentTemplateMetadata)
                {
                    templateNode = node;
                    break;
                }
            }
            if (templateNode == null)
                throw new TraceLab.Core.Exceptions.BenchmarkException("Template node has not been found in the benchmark experiment. The benchmark experiment is corrupted.");
            
            //3. export current experiment into composite component
            foreach (BenchmarkItemSetting<IOItem> item in BenchmarkInputSetting)
            {
                item.SelectedSetting.Include = true;
            }

            foreach (BenchmarkItemSetting<IOItem> item in BenchmarkOutputsSetting)
            {
                item.SelectedSetting.Include = true;
            }

            //create temporary component source - note it is just a assembly source name file to satisfy Metadata Assembly requirement
            //the file is not going to be created (this is UGLY - do refactoring)
            string tempSource = System.IO.Path.Combine(BenchmarkInfo.FilePath, "temporary");
            m_setup.CompositeComponentLocationFilePath = tempSource;

            CompositeComponentMetadataDefinition definition = m_setup.GenerateCompositeComponentDefinition();

            //4. Replace template node by removing it from graph, adding the new node, and reconnecting new not

            //Add composite component node
            ExperimentNode replacementNode = benchmarkExperiment.AddComponentFromDefinition(definition, templateNode.Data.X, templateNode.Data.Y);
            
            //connect replacement node to the same outgoing nodes as template node
            foreach (ExperimentNodeConnection nodeConnection in benchmarkExperiment.OutEdges(templateNode))
            {
                benchmarkExperiment.AddConnection(replacementNode, nodeConnection.Target);
            }
            
            //connect replacement node to the same incoming nodes as template node
            foreach (ExperimentNodeConnection nodeConnection in benchmarkExperiment.InEdges(templateNode))
            {
                
                benchmarkExperiment.AddConnection(nodeConnection.Source, replacementNode);

                //if the predecessor is a decision node update its decision code so that it matches new label
                FixDecisionCode(templateNode.Data.Metadata.Label, replacementNode.Data.Metadata.Label, nodeConnection.Source as ExperimentDecisionNode);
            }

            //finally remove the template node
            benchmarkExperiment.RemoveVertex(templateNode);

            //now remap io according to settings
            CompositeComponentMetadata compositeComponentDefinition = (CompositeComponentMetadata)replacementNode.Data.Metadata;
            foreach (BenchmarkItemSetting<IOItem> item in BenchmarkInputSetting)
            {
                item.SelectedSetting.Include = true;
                compositeComponentDefinition.IOSpec.Input[item.SelectedSetting.ItemSettingName].MappedTo = item.Item.MappedTo;
            }

            foreach (BenchmarkItemSetting<IOItem> item in BenchmarkOutputsSetting)
            {
                item.SelectedSetting.Include = true;
                compositeComponentDefinition.IOSpec.Output[item.SelectedSetting.ItemSettingName].MappedTo = item.Item.MappedTo;
            }

            BenchmarkExperiment = benchmarkExperiment;
        }

        public TLExperimentResults LoadBaseline()
        {
            TLExperimentResults baseline = BenchmarkLoader.ReadBaseline(BenchmarkInfo.FilePath);
            return baseline;
        }

        /// <summary>
        /// If the given decision node is not null, the method updates its code by replacing templateNodeLabel with replacementNodeLabel
        /// of all Select(" label ") statements in the decision code.
        /// </summary>
        /// <param name="templateNodeLabel">The template node label.</param>
        /// <param name="replacementNodeLabel">The replacement node label.</param>
        /// <param name="decisionNode">The decision node.</param>
        private static void FixDecisionCode(string templateNodeLabel, string replacementNodeLabel, ExperimentDecisionNode decisionNode)
        {
            if (decisionNode != null)
            {
                string decisionCode = ((DecisionMetadata)decisionNode.Data.Metadata).DecisionCode;
                string selectStatementOldLabel = string.Format("Select(\"{0}\")", templateNodeLabel);
                if (decisionCode.Contains(selectStatementOldLabel))
                {
                    //replace old label with new label of Select("...") statements;
                    string selectStatementNewLabel = string.Format("Select(\"{0}\")", replacementNodeLabel);
                    decisionCode = decisionCode.Replace(selectStatementOldLabel, selectStatementNewLabel);
                    ((DecisionMetadata)decisionNode.Data.Metadata).DecisionCode = decisionCode;
                }
            }
        }
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}

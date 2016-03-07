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
using System.ComponentModel;
using TraceLab.Core.Components;
using System.Collections.ObjectModel;
using System.IO;


namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Model for the wizard, that allows defining composite component
    /// </summary>
    public class DefiningCompositeComponentSetup : INotifyPropertyChanged
    {
        #region Constructor

        // HERZUM SPRINT 3.1 TLAB-180
        private void ConstructGraphFromSelectedScopeNodes(BaseExperiment experiment)
        {
            if (experiment == null)
                return;
            CompositeComponentGraph.CopyAndAdd (CompositeComponentGraph.ConstructGraphFromSelectedNodes (experiment), 200.0, 200.0);  
            foreach (ExperimentNode node in experiment.Vertices) 
                if (!node.IsSelected){
                    ScopeBaseMetadata scopeBaseMetadata = node.Data.Metadata as ScopeBaseMetadata;
                    if (scopeBaseMetadata!= null && scopeBaseMetadata.ComponentGraph!=null)
                        ConstructGraphFromSelectedScopeNodes(scopeBaseMetadata.ComponentGraph);
                }
        }

        private void ConstructGraphFromSelectedScopeNodes(CompositeComponentGraph experiment)
        {
            if (experiment == null)
                return;
            CompositeComponentGraph.CopyAndAdd (CompositeComponentGraph.ConstructGraphFromSelectedNodes (experiment), 200.0, 200.0);  
            foreach (ExperimentNode node in experiment.Vertices) {
                if (!node.IsSelected){
                    ScopeBaseMetadata scopeBaseMetadata = node.Data.Metadata as ScopeBaseMetadata;
                    if (scopeBaseMetadata!= null && scopeBaseMetadata.ComponentGraph!=null)
                        ConstructGraphFromSelectedScopeNodes(scopeBaseMetadata.ComponentGraph);
                }
            }
        }
        // END HERZUM SPRINT 3.1 TLAB-180

        /// <summary>
        /// Initializes a new instance of the <see cref="DefiningCompositeComponentSetup"/> class.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        public DefiningCompositeComponentSetup(Experiment experiment)
        {

            // HERZUM SPRINT 2.4 TLAB-157
            // CompositeComponentGraph = CompositeComponentGraph.ConstructGraphFromSelectedNodes(experiment);
            CompositeComponentGraph = CompositeComponentGraph.ConstructEmptyGraph();
            // HERZUM SPRINT 3.1 TLAB-180
            // CompositeComponentGraph.CopyAndAdd (CompositeComponentGraph.ConstructGraphFromSelectedNodes (experiment), 200.0, 200.0);  
            ConstructGraphFromSelectedScopeNodes (experiment);
            // END HERZUM SPRINT 3.1 TLAB-180
            CompositeComponentGraph.ConnectNodesToStartAndEndNode(CompositeComponentGraph);
            // END HERZUM SPRINT 2.4 TLAB-157

            InputSettings = new SortedDictionary<string, ItemSetting>();
            OutputSettings = new SortedDictionary<string, ItemSetting>();
            ConfigSettings = new SortedDictionary<string, ConfigItemSetting>();

            TraceLab.Core.Utilities.ExperimentHelper.BFSTraverseExperiment(CompositeComponentGraph, CompositeComponentGraph.StartNode, RetrieveIOSpecAndConfig);
        }

        public DefiningCompositeComponentSetup(Experiment experiment, Action<TraceLab.Core.Components.CompositeComponentMetadataDefinition> howToAddToComponentsLibrary)
            : this(experiment)
        {
            addToComponentLibrary = howToAddToComponentsLibrary;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the composite component graph of composite component that is being defined
        /// </summary>
        public CompositeComponentGraph CompositeComponentGraph
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input settings for the composite component that is being defined, ie. Inputs that are visible and required to be provided from higher level experiment
        /// </summary>
        public SortedDictionary<string, ItemSetting> InputSettings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the output settings for the composite component that is being defined, ie. Outputs that are visible and outputted by composite component to higher level experiment
        /// </summary>
        public SortedDictionary<string, ItemSetting> OutputSettings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the config settings for the composite component that is being defined
        /// </summary>
        public SortedDictionary<string, ConfigItemSetting> ConfigSettings
        {
            get;
            private set;
        }

        private string m_name = String.Empty;
        /// <summary>
        /// Gets or sets the name for the composite component that is being defined
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string m_author = String.Empty;
        /// <summary>
        /// Gets or sets the author for the composite component that is being defined
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author
        {
            get { return m_author; }
            set
            {
                if (m_author != value)
                {
                    m_author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

        private string m_description = String.Empty;
        /// <summary>
        /// Gets or sets the description for the composite component that is being defined
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return m_description; }
            set
            {
                if (m_description != value)
                {
                    m_description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private string m_version = String.Empty;
        /// <summary>
        /// Gets or sets the version for the composite component that is being defined
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get
            {
                return m_version;
            }
            set
            {
                if (m_version != value)
                {
                    m_version = value;
                    NotifyPropertyChanged("Version");
                }
            }
        }

        private string m_compositeComponentLocationFilePath;
        /// <summary>
        /// Gets or sets the composite component location file path.
        /// </summary>
        /// <value>
        /// The composite component location file path.
        /// </value>
        public string CompositeComponentLocationFilePath
        {
            get { return m_compositeComponentLocationFilePath; }
            set 
            {
                if (value != m_compositeComponentLocationFilePath)
                {
                    m_compositeComponentLocationFilePath = value;
                    NotifyPropertyChanged("CompositeComponentLocationFilePath");
                }
            }
        }

        #endregion

        #region Private Helper Methods For IOSpec and Config retrieval

        /// <summary>
        /// The dictionary contains corresponding nodes for each mapping name. Note, it is concatenated list of corresponding nodes from input and outputs mappings. 
        /// For example, if there is the same mapping for input and output, they have mutual list. 
        /// </summary>
        private Dictionary<string, List<IConfigurableAndIOSpecifiable>> m_corespondingNodesForEachMapping = new Dictionary<string,List<IConfigurableAndIOSpecifiable>>();

        /// <summary>
        /// Collects corresponding nodes for given mapping name.
        /// </summary>
        /// <param name="mappingName">Name of the mapping.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>list of nodes metadata for the mapping name.</returns>
        internal List<IConfigurableAndIOSpecifiable> CollectCorrespondingNodesForMapping(string mappingName, IConfigurableAndIOSpecifiable metadata)
        {
            List<IConfigurableAndIOSpecifiable> correspondingNodes;
            if (m_corespondingNodesForEachMapping.TryGetValue(mappingName, out correspondingNodes) == false)
            {
                correspondingNodes = new List<IConfigurableAndIOSpecifiable>();
                m_corespondingNodesForEachMapping.Add(mappingName, correspondingNodes);
            }

            if (correspondingNodes.Contains(metadata) == false)
            {
                correspondingNodes.Add(metadata);
            }

            return correspondingNodes;
        }

        private void RetrieveIOSpecAndConfig(ExperimentNode node)
        {
            if (node != null)
            {
                IConfigurableAndIOSpecifiable metadata = node.Data.Metadata as IConfigurableAndIOSpecifiable;
                if (metadata != null)
                {
                    RetrieveIOSpecFromComponent(metadata);
                    RetrieveConfigValuesFromComponent(node.ID, metadata);   
                }
            }
        }

        /// <summary>
        /// Retrieves the IO spec from component and updates local IOSpec.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        private void RetrieveIOSpecFromComponent(IConfigurableAndIOSpecifiable metadata)
        {
            CollectIO(metadata, metadata.IOSpec.Input, InputSettings);
            CollectIO(metadata, metadata.IOSpec.Output, OutputSettings);

            MatchOutputsAndInputsPairs();
        }

        /// <summary>
        /// Matches the outputs and inputs pairs of the same setting name. 
        /// </summary>
        private void MatchOutputsAndInputsPairs()
        {
            foreach (string itemSettingName in OutputSettings.Keys)
            {
                ItemSetting inputSetting;
                if (InputSettings.TryGetValue(itemSettingName, out inputSetting))
                {
                    ItemSetting outputSetting = OutputSettings[itemSettingName];
                    //match the two item settings (so that they can highlight each other)
                    outputSetting.PairSetting = inputSetting;
                    inputSetting.PairSetting = outputSetting;
                }
            }
        }

        private void CollectIO(IConfigurableAndIOSpecifiable metadata, IDictionary<string, IOItem> ioDictionary, SortedDictionary<string, ItemSetting> compositeComponentSettings)
        {
            foreach (IOItem item in ioDictionary.Values)
            {
                var correspondingNodes = CollectCorrespondingNodesForMapping(item.MappedTo, metadata);
                if (compositeComponentSettings.ContainsKey(item.MappedTo) == false)
                {
                    var itemSetting = new ItemSetting(item.MappedTo, item.IOItemDefinition.Type, item.IOItemDefinition.Description);
                    //set the reference to the list of corresponding nodes that either input or output the same mapping name
                    itemSetting.CorrespondingNodes = correspondingNodes;
                    compositeComponentSettings.Add(item.MappedTo, itemSetting);
                }
            }
        }
        
        /// <summary>
        /// Retrieves the config values from component's config wrapper.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        private void RetrieveConfigValuesFromComponent(string nodeIdentifier, IConfigurableAndIOSpecifiable metadata)
        {
            foreach (ConfigPropertyObject configParameter in metadata.ConfigWrapper.ConfigValues.Values)
            {
                string displayParameterName = String.Format("{0} {1}", metadata.Label, configParameter.DisplayName);
                string extendedParameterName = String.Format("{0}:{1}", nodeIdentifier, configParameter.Name);

                //create property object based on the component's property object
                ConfigPropertyObject propertyObject = new ConfigPropertyObject(configParameter);

                //but change the name - it will be resolved in Nodes factory based on extended parameter name which retNode config parameter is supposed to be overridden
                propertyObject.DisplayName = displayParameterName;
                propertyObject.Name = extendedParameterName;
                
                //add it to the compound configWrapperDefinition
                displayParameterName = GetUniqueDisplayName(displayParameterName);
                ConfigSettings.Add(displayParameterName, new ConfigItemSetting(displayParameterName, configParameter.Type, propertyObject));
            }   
        }

        /// <summary>
        /// Method assures that display name is unique. If it already exists in the config, it adds the number to the display name. 
        /// </summary>
        /// <param name="displayParameterName">Display name of the parameter.</param>
        /// <returns></returns>
        private string GetUniqueDisplayName(string displayParameterName)
        {
            int count = 2;
            string tmpDisplayName = displayParameterName;
            while (ConfigSettings.ContainsKey(tmpDisplayName))
            {
                tmpDisplayName = String.Format("{0} {1}", displayParameterName, count);
                count++;
            }
            displayParameterName = tmpDisplayName;
            return displayParameterName;
        }

        #endregion

        #region INotifyPropertyChanged

        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
            }
            remove
            {
                m_propertyChanged -= value;
            }
        }
        protected void NotifyPropertyChanged(string prop)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region Define Component

        private Action<TraceLab.Core.Components.CompositeComponentMetadataDefinition> addToComponentLibrary;

        /// <summary>
        /// Defines the component.
        /// StreamWriter throws several exceptions. For each of those there should be proper test
        /// </summary>
        public void DefineComponent()
        {
            TraceLab.Core.Components.CompositeComponentMetadataDefinition componentDefinition = GenerateCompositeComponentDefinition();

            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TraceLab.Core.Components.CompositeComponentMetadataDefinition), null);
            using (TextWriter writer = new StreamWriter(CompositeComponentLocationFilePath))
            {
                serializer.Serialize(writer, componentDefinition);
                writer.Close();
            }

            if (addToComponentLibrary != null)
            {
                addToComponentLibrary(componentDefinition);
            }
        }

        /// <summary>
        /// Generates the final composite component definition.
        /// </summary>
        /// <returns></returns>
        internal TraceLab.Core.Components.CompositeComponentMetadataDefinition GenerateCompositeComponentDefinition()
        {
            var componentId = Guid.NewGuid().ToString();
            //first create CompositeComponentMetadefinition based on ExportSettings
            TraceLab.Core.Components.CompositeComponentMetadataDefinition componentDefinition =
                new TraceLab.Core.Components.CompositeComponentMetadataDefinition(
                    componentId,
                    CompositeComponentGraph,
                    CompositeComponentLocationFilePath,
                    Name,
                    Name,
                    Version,
                    Description,
                    Author,
                    new Components.ComponentTags(componentId),
                    new List<DocumentationLink>());

            foreach (string itemName in InputSettings.Keys)
            {
                ItemSetting item = InputSettings[itemName];
                if (item.Include == true)
                {
                    if (componentDefinition.IOSpecDefinition.Input.ContainsKey(itemName) == false)
                    {
                        componentDefinition.IOSpecDefinition.Input.Add(itemName, new TraceLab.Core.Components.IOItemDefinition(itemName, item.Type, item.Description, TraceLabSDK.IOSpecType.Input));
                    }
                }
            }

            foreach (string itemName in OutputSettings.Keys)
            {
                ItemSetting item = OutputSettings[itemName];
                if (item.Include == true)
                {
                    if (componentDefinition.IOSpecDefinition.Output.ContainsKey(itemName) == false)
                    {
                        componentDefinition.IOSpecDefinition.Output.Add(itemName, new TraceLab.Core.Components.IOItemDefinition(itemName, item.Type, item.Description, TraceLabSDK.IOSpecType.Output));
                    }
                }
            }

            foreach (ConfigItemSetting item in ConfigSettings.Values)
            {
                //note here we add all config values, but only some of them are visible at the top level in the property grid
                if (componentDefinition.ConfigurationWrapperDefinition.Properties.ContainsKey(item.Alias) == false)
                {
                    componentDefinition.ConfigurationWrapperDefinition.AddProperty(item.PropertyObject);
                }
            }
            return componentDefinition;
        }

        #endregion
    }
}

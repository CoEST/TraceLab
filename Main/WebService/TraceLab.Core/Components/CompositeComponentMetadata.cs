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
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Experiments;
using TraceLab.Core.Settings;
using System.Security.Permissions;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Composite Component Metadata represents metadata of the compound node - node that contains another subgraph.
    /// </summary>
    [XmlRoot("Metadata")]
    [Serializable]
    public class CompositeComponentMetadata : CompositeComponentBaseMetadata, IConfigurableAndIOSpecifiable
    {        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentMetadata"/> class.
        /// </summary>
        protected CompositeComponentMetadata() : base() { }

        /// <summary>
        /// Initializes a new s_instance of the <see cref="CompositeComponentMetadata"/> class.
        /// </summary>
        /// <param name="compositeComponentMetadataDefinition">The composite component metadata definition.</param>
        internal CompositeComponentMetadata(CompositeComponentMetadataDefinition compositeComponentMetadataDefinition, string experimentLocationRoot)
            : this()
        {
            m_experimentLocationRoot = experimentLocationRoot;
            ComponentMetadataDefinition = compositeComponentMetadataDefinition;
            InitDefaultComponentMetadata(true);
        }

        /// <summary>
        /// Inits the default component metadata based on its definition
        /// </summary>
        /// <param name="enforce">if set to <c>true</c> [enforce].</param>
        private void InitDefaultComponentMetadata(bool enforce)
        {
            if (IOSpec == null || enforce)
            {
                IOSpec = new IOSpec(ComponentMetadataDefinition.IOSpecDefinition);
            }
            if (Label == null || enforce)
            {
                Label = ComponentMetadataDefinition.Label;
            }
            if (ConfigWrapper == null || enforce)
            {
                ConfigWrapper = new ConfigWrapper(ComponentMetadataDefinition.ConfigurationWrapperDefinition);
                if (m_experimentLocationRoot != null) //location may be null if metadata definition does not belong to any graph
                {
                    ConfigWrapper.SetExperimentLocationRoot(m_experimentLocationRoot, true);
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes the component graph from ComponentMetadataDefinition graph
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="settings">The settings.</param>
        public override void InitializeComponentGraph(CompositeComponentNode node, TraceLab.Core.Settings.Settings settings)
        {
            //each composite node gets its own copy of ComponentMetadataDefinition.ComponentGraph
            m_compositeComponentGraph = new CompositeComponentGraph(node, ComponentMetadataDefinition.ComponentGraph);
            m_compositeComponentGraph.Settings = settings;
        }

        #region Properties

        private CompositeComponentGraph m_compositeComponentGraph;
        /// <summary>
        /// Gets the component graph.
        /// </summary>
        public override CompositeComponentGraph ComponentGraph
        {
            get
            {
                return m_compositeComponentGraph;
            }
        }

        private string m_componentMetadataDefinitionID;
        /// <summary>
        /// Gets or sets the component metadata definition ID.
        /// </summary>
        /// <value>
        /// The component metadata definition ID.
        /// </value>
        public string ComponentMetadataDefinitionID
        {
            get
            {
                return m_componentMetadataDefinitionID;
            }
            set
            {
                if (m_componentMetadataDefinitionID != value)
                {
                    m_componentMetadataDefinitionID = value;
                    NotifyPropertyChanged("ComponentMetadataDefinitionID");
                    IsModified = true;
                }
            }
        }

        private CompositeComponentMetadataDefinition m_compositeComponentMetadataDefinition;
        /// <summary>
        /// Gets or sets the composite component metadata definition.
        /// </summary>
        /// <value>
        /// The composite component metadata definition.
        /// </value>
        public CompositeComponentMetadataDefinition ComponentMetadataDefinition
        {
            get
            {
                return m_compositeComponentMetadataDefinition;
            }
            private set
            {
                if (m_compositeComponentMetadataDefinition != value)
                {
                    m_compositeComponentMetadataDefinition = value;

                    if (value != null)
                    {
                        ComponentMetadataDefinitionID = value.ID;

                        if (Label == null)
                        {
                            Label = value.Label;
                        }
                    }
                    NotifyPropertyChanged("ComponentMetadataDefinition");
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets the metadata definition - IConfigurableAndIOSpecifiable member.
        /// </summary>
        /// <value>The metadata definition.</value>
        public MetadataDefinition MetadataDefinition
        {
            get { return m_compositeComponentMetadataDefinition; }
        }

        private IOSpec m_IOSpec;
        /// <summary>
        /// Gets or sets the compound IO spec of all nodes that are in the subgraph
        /// </summary>
        /// <value>
        /// The compound/merged IO spec.
        /// </value>
        public IOSpec IOSpec
        {
            get
            {
                return m_IOSpec;
            }
            set
            {
                if (m_IOSpec != null)
                {
                    m_IOSpec.PropertyChanged -= (IOSpecConfig_PropertyChanged);
                }

                m_IOSpec = value;

                if (m_IOSpec != null)
                {
                    m_IOSpec.PropertyChanged += (IOSpecConfig_PropertyChanged);
                }

                NotifyPropertyChanged("IOSpec");
                IsModified = true;
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the IOSpec or ConfigWrapper.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void IOSpecConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                IsModified = true;
            }
        }

        private ConfigWrapper m_configWrapper;
        /// <summary>
        /// Gets or sets the config wrapper.
        /// </summary>
        /// <value>
        /// The config wrapper.
        /// </value>
        public ConfigWrapper ConfigWrapper
        {
            get
            {
                return m_configWrapper;
            }
            set
            {
                if (m_configWrapper != value)
                {
                    if (m_configWrapper != null)
                        m_configWrapper.PropertyChanged -= IOSpecConfig_PropertyChanged;

                    m_configWrapper = value;

                    if (m_configWrapper != null)
                        m_configWrapper.PropertyChanged += IOSpecConfig_PropertyChanged;

                    NotifyPropertyChanged("ConfigWrapper");
                    IsModified = true;
                }
            }
        }
        
        #endregion

        #region Modification

        /// <summary>
        /// Calculates the modification - if sth is modified the experiment is marked as not saved
        /// </summary>
        /// <returns></returns>
        protected override bool CalculateModification()
        {
            bool isModified = base.CalculateModification();
            isModified |= m_IOSpec.IsModified;
            isModified |= m_configWrapper.IsModified;

            return isModified;
        }

        /// <summary>
        /// Resets the modified flag.
        /// </summary>
        public override void ResetModifiedFlag()
        {
            base.ResetModifiedFlag();

            if (m_IOSpec != null)
            {
                m_IOSpec.ResetModifiedFlag();
            }

            if (m_configWrapper != null)
            {
                m_configWrapper.ResetModifiedFlag();
            }

            IsModified = false;
        }

        #endregion

        #region IXmlSerializable

        private IOSpec m_tempIoSpec;
        private ConfigWrapper m_tempConfigWrapper;
        private string m_tempLabel;
        private bool m_tempWaitsForAllPredecessors;

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            IsInitialized = false;

            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/Metadata");
            //read component metadata definition id 
            ComponentMetadataDefinitionID = iter.GetAttribute("ComponentMetadataDefinitionID", String.Empty);
            m_tempLabel = iter.GetAttribute("Label", String.Empty);

            //read attribute indicating if component should wait for all predecessors
            var wait = iter.GetAttribute("WaitsForAllPredecessors", String.Empty);
            if (String.IsNullOrEmpty(wait) || (wait != Boolean.TrueString && wait != Boolean.FalseString)) //if value has not been found set it to true
                m_tempWaitsForAllPredecessors = true; //default value
            else
                m_tempWaitsForAllPredecessors = Convert.ToBoolean(wait);
            
            //read iospec and config wrapper from xml
            m_tempIoSpec = IOSpec.ReadIOSpec(nav.SelectSingleNode("/Metadata/IOSpec"));
            m_tempConfigWrapper = ConfigWrapper.ReadConfig(nav);
        }

        public override void PostProcessReadXml(Components.IComponentsLibrary library, string experimentLocationRoot)
        {
            GetDefinitionAndSet(library, experimentLocationRoot);

            IsInitialized = true;
            IsModified = false;
        }

        public override void UpdateFromDefinition(IComponentsLibrary library)
        {
            HasDeserializationError = false;
            m_tempConfigWrapper = ConfigWrapper;
            m_tempIoSpec = IOSpec;
            m_tempLabel = Label;
            m_tempWaitsForAllPredecessors = WaitsForAllPredecessors;
            ComponentMetadataDefinition = null;

            GetDefinitionAndSet(library, m_experimentLocationRoot);
        }

        /// <summary>
        /// Gets the definition for this Metadata and sets up the configuration based on it.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        private void GetDefinitionAndSet(Components.IComponentsLibrary library, string experimentLocationRoot)
        {
            m_experimentLocationRoot = experimentLocationRoot;

            //reload component metadata definition
            MetadataDefinition metadataDefinition;
            if (library.TryGetComponentDefinition(ComponentMetadataDefinitionID, out metadataDefinition))
            {
                ComponentMetadataDefinition = metadataDefinition as CompositeComponentMetadataDefinition;
            }

            //if definition has been found override matching parameters (but don't override definition)
            if (ComponentMetadataDefinition != null)
            {
                InitDefaultComponentMetadata(true);
                IOSpec.UpdateMappingsBasedOn(m_tempIoSpec);
                ConfigWrapper.UpdateConfigValuesBasedOn(m_tempConfigWrapper);
                Label = m_tempLabel;
                WaitsForAllPredecessors = m_tempWaitsForAllPredecessors;
                //clear error - there might have been errors regarding existence of composite component, but referencing new package might fix this problem.
                DeserializationErrorMessage = null;
                HasDeserializationError = false;
            }
            else
            {
                //otherwise set values to temporary IOSpec and configWrapper (just not to lose data in case user resave experiment), and rethrow exception
                IOSpec = m_tempIoSpec;
                ConfigWrapper = m_tempConfigWrapper;
                HasDeserializationError = true;
                Label = m_tempLabel;
                WaitsForAllPredecessors = m_tempWaitsForAllPredecessors;
                DeserializationErrorMessage = String.Format(System.Globalization.CultureInfo.CurrentCulture, "Component library does not contain any Composite Component of the given ID {0}", ComponentMetadataDefinitionID);
            }

            if (experimentLocationRoot != null) //it may be null if it is a component residing in the graph of composite component that is the library (was not added to any experiment)
            {
                ConfigWrapper.SetExperimentLocationRoot(experimentLocationRoot, true);
            }

            m_tempConfigWrapper = null;
            m_tempIoSpec = null;
            m_tempLabel = null;
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());
            writer.WriteAttributeString("Label", Label);
            writer.WriteAttributeString("ComponentMetadataDefinitionID", ComponentMetadataDefinitionID);
            writer.WriteAttributeString("WaitsForAllPredecessors", WaitsForAllPredecessors.ToString());
            IOSpec.WriteXml(writer);
            ConfigWrapper.WriteXml(writer);
        }

        #endregion

        #region Clone & Copy

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Metadata Clone()
        {
            var clone = new CompositeComponentMetadata();
            clone.CopyFrom(this);
            return clone;
        }

        /// <summary>
        /// Performs a deep copy of the data in this object to another instance of the Metadata.
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(Metadata other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            
            base.CopyFrom(other);

            CompositeComponentMetadata metadata = (CompositeComponentMetadata)other;
            m_IOSpec = metadata.m_IOSpec.Clone();
            m_configWrapper = metadata.m_configWrapper.Clone();
            m_experimentLocationRoot = metadata.m_experimentLocationRoot;
            m_compositeComponentMetadataDefinition = metadata.m_compositeComponentMetadataDefinition;
            m_componentMetadataDefinitionID = metadata.m_componentMetadataDefinitionID;

            HasDeserializationError = metadata.HasDeserializationError;
            if (HasDeserializationError)
            {
                DeserializationErrorMessage = metadata.DeserializationErrorMessage;
            } 

            // if composite component was not present in the library, then corresponding component graph may be null.
            // the deserialization has already set the corresponding error above, nevertheless subgraph cannot be cloned obviously
            if (HasDeserializationError == false && metadata.m_compositeComponentGraph != null)
            {
                m_compositeComponentGraph = (CompositeComponentGraph)(metadata.m_compositeComponentGraph.Clone());
            }

            m_tempConfigWrapper = metadata.m_tempConfigWrapper;
            m_tempIoSpec = metadata.m_tempIoSpec;
            m_tempLabel = metadata.m_tempLabel;
        }

        #endregion
    }
}

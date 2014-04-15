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
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents component metadata. 
    /// Component metadata is used for the component instances based on its component metadata definition.
    /// In other words component metadata definition consist of all defining elements such as IOSpecDefinition, ConfigurationDefnition, Default Label, etc,
    /// while ComponentMetadata has IOSpec refering to IOSpecDefinitoin, ConfigWrapper refering to ConfigurationDefinition. 
    /// IOSpec and ConfigWrapper consist also of actual values for the component, that are serialized into experiment file when experiment is saved.
    /// 
    /// Developer can think of the relation between ComponentMetadataDefinition and ComponentMetadata as a similar relation between class and object.
    /// 
    /// There is only one ComponentMetadataDefinition to define each component, but there can be several instances of component with their ComponentMetadata in the experiment.
    /// </summary>
    [XmlRoot("Metadata")]
    [Serializable]
    public class ComponentMetadata : Metadata, IConfigurableAndIOSpecifiable, ILoggable, INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMetadata"/> class.
        /// </summary>
        protected ComponentMetadata() 
        {
            InitLoggingNodeSettings();
        }

        /// <summary>
        /// Initializes a new s_instance of the <see cref="ComponentMetadata"/> class.
        /// </summary>
        /// <param name="componentMetadataDefinition">The component metadata definition.</param>
        internal ComponentMetadata(ComponentMetadataDefinition componentMetadataDefinition, string experimentLocationRoot)
        {
            m_experimentLocationRoot = experimentLocationRoot;
            ComponentMetadataDefinition = componentMetadataDefinition;
            InitDefaultComponentMetadata(true);
            InitLoggingNodeSettings();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ComponentMetadata"/> class from being created.
        /// </summary>
        /// <param name="other">The other.</param>
        private ComponentMetadata(ComponentMetadata other)
        {
            m_experimentLocationRoot = other.m_experimentLocationRoot;
            m_IOSpec = other.m_IOSpec.Clone();
            m_componentMetadataDefinition = other.m_componentMetadataDefinition;
            m_componentMetadataDefinitionID = other.m_componentMetadataDefinitionID;
            m_configWrapper = other.m_configWrapper.Clone();
            m_tempConfigWrapper = other.m_tempConfigWrapper;
            m_tempIoSpec = other.m_tempIoSpec;
            m_tempLabel = other.m_tempLabel;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Metadata Clone()
        {
            var clone = new ComponentMetadata(this);
            clone.CopyFrom(this);

            return clone;
        }

        protected override void CopyFrom(Metadata other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            base.CopyFrom(other);

            ComponentMetadata metadata = (ComponentMetadata)other;
            m_IOSpec = metadata.m_IOSpec.Clone();
            m_configWrapper = metadata.m_configWrapper.Clone();
            m_experimentLocationRoot = metadata.m_experimentLocationRoot;
            m_componentMetadataDefinition = metadata.m_componentMetadataDefinition;
            m_componentMetadataDefinitionID = metadata.m_componentMetadataDefinitionID;

            HasDeserializationError = metadata.HasDeserializationError;
            if (HasDeserializationError)
            {
                DeserializationErrorMessage = metadata.DeserializationErrorMessage;
            }

            m_tempConfigWrapper = metadata.m_tempConfigWrapper;
            m_tempIoSpec = metadata.m_tempIoSpec;
            m_tempLabel = metadata.m_tempLabel;
        }

        /// <summary>
        /// Inits the default component metadata based on its component metadata definition
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
                ConfigWrapper.SetExperimentLocationRoot(m_experimentLocationRoot, true);
            }
        }

        #endregion

        #region Properties

        private IOSpec m_IOSpec;
        /// <summary>
        /// Gets or sets the IO spec.
        /// </summary>
        /// <value>
        /// The IO spec.
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
        /// Handles the PropertyChanged event of the IOSpecConfig control.
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

        /// <summary>
        /// Calculates the modification to indicate that experiment has been modified
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

                    if(m_configWrapper != null)
                        m_configWrapper.PropertyChanged += IOSpecConfig_PropertyChanged;

                    NotifyPropertyChanged("ConfigWrapper");
                    IsModified = true;
                }
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
        
        private ComponentMetadataDefinition m_componentMetadataDefinition;
        /// <summary>
        /// Gets or sets the component metadata definition.
        /// ComponentMetadataDefinition should match ComponentMetadataDefinitionID and vice verse. 
        /// Lazy update - when ID changes, the component metadata is updated when getter is called. 
        /// 
        /// Ignored in experiment xml - in other words it is not saved, but reloaded in deserialization.
        /// </summary>
        /// <value>
        /// The component metadata definition.
        /// </value>
        public ComponentMetadataDefinition ComponentMetadataDefinition
        {
            get
            {
                return m_componentMetadataDefinition;
            }
            set
            {
                if (m_componentMetadataDefinition != value)
                {
                    m_componentMetadataDefinition = value;

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
            get { return m_componentMetadataDefinition; }
        }

        #endregion

        #region ILoggable Members

        /// <summary>
        /// Gets the classname to display in the log message. The classname can be displayed in the log information, especially when exception occurs.
        /// </summary>
        public string Classname
        {
            get { return m_componentMetadataDefinition.Classname; }
        }

        /// <summary>
        /// Gets the source assembly to display in the log message. The classname can be displayed in the log information, especially when exception occurs.
        /// </summary>
        public string SourceAssembly
        {
            get { return m_componentMetadataDefinition.Assembly; }
        }

        #endregion

        #region Equals_HashCode

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this s_instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this s_instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this s_instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            ComponentMetadata other = obj as ComponentMetadata;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(IOSpec, other.IOSpec);
            isEqual &= object.Equals(ComponentMetadataDefinitionID, other.ComponentMetadataDefinitionID);
            isEqual &= object.Equals(ConfigWrapper, other.ConfigWrapper);
            isEqual &= object.Equals(Label, other.Label);
            isEqual &= object.Equals(ComponentMetadataDefinition, other.ComponentMetadataDefinition);
            
            return isEqual;
        }

        /// <summary>
        /// Returns a hash code for this s_instance.
        /// </summary>
        /// <returns>
        /// A hash code for this s_instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int iospecHash = IOSpec.GetHashCode();
            int componentMetadataDefinitionIDHash = ComponentMetadataDefinitionID.GetHashCode();
            int configWrapperHash = ConfigWrapper.GetHashCode();
            int labelHash = Label.GetHashCode();
            int componentMetadataDefinitionHash = ComponentMetadataDefinition.GetHashCode();

            return iospecHash ^ componentMetadataDefinitionIDHash ^ configWrapperHash ^ labelHash ^ componentMetadataDefinitionHash;
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

        /// <summary>
        /// Post process after xml reading is completed.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        public override void PostProcessReadXml(Components.IComponentsLibrary library, string experimentLocationRoot)
        {
            GetDefinitionAndSet(library, experimentLocationRoot);

            IsInitialized = true;
            IsModified = false;
        }

        /// <summary>
        /// Updates the metadata from component metadata definiton - component metadata definition is located in the library
        /// </summary>
        /// <param name="library">The library.</param>
        public override void UpdateFromDefinition(IComponentsLibrary library)
        {
            DeserializationErrorMessage = null;
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
                ComponentMetadataDefinition = metadataDefinition as ComponentMetadataDefinition;
            }

            //if definition has been found override matching parameters (but don't override definition)
            if (ComponentMetadataDefinition != null)
            {
                InitDefaultComponentMetadata(true);
                IOSpec.UpdateMappingsBasedOn(m_tempIoSpec);
                ConfigWrapper.UpdateConfigValuesBasedOn(m_tempConfigWrapper);
                Label = m_tempLabel;
                WaitsForAllPredecessors = m_tempWaitsForAllPredecessors;
            }
            else
            {
                //otherwise set values to temporary IOSpec and configWrapper (just not to lose data in case user resaves experiment), and rethrow exception
                IOSpec = m_tempIoSpec;
                ConfigWrapper = m_tempConfigWrapper;
                HasDeserializationError = true;
                Label = m_tempLabel;
                WaitsForAllPredecessors = m_tempWaitsForAllPredecessors;
                DeserializationErrorMessage = String.Format(CultureInfo.CurrentCulture, "Component library does not contain any Component of the given ID {0}", ComponentMetadataDefinitionID);
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

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        #endregion

        #region ISerialization Implementation

        /// <summary>
        /// Gets the object data.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_IOSpec", m_IOSpec);
            info.AddValue("m_configWrapper", m_configWrapper);
            info.AddValue("m_componentMetadataDefinitionID", m_componentMetadataDefinitionID);
            info.AddValue("m_componentMetadataDefinition", m_componentMetadataDefinition);
        }

        #endregion

        #region Deserialization Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMetadata"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected ComponentMetadata(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_IOSpec = (IOSpec)info.GetValue("m_IOSpec", typeof(IOSpec));
            m_configWrapper = (ConfigWrapper)info.GetValue("m_configWrapper", typeof(ConfigWrapper));
            m_componentMetadataDefinitionID = (string)info.GetValue("m_componentMetadataDefinitionID", typeof(string));
            m_componentMetadataDefinition = (ComponentMetadataDefinition)info.GetValue("m_componentMetadataDefinition", typeof(ComponentMetadataDefinition));
        }

        #endregion

        private string m_experimentLocationRoot;
    }
}

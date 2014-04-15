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
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// ConfigWrapper is instantiated as part of the ComponentMetadata. 
    /// Based on given ConfigWrapperDefinition created a dictionary of Property keys, and their values. 
    /// 
    /// ConfigWrapper implements ICustomTypeDescriptor interface, that normally allows an object to provide type information about itself. 
    /// In this case we use it to provide type information about config object described by list of properties. 
    /// 
    /// See related links:
    /// http://msdn.microsoft.com/en-us/library/system.componentmodel.icustomtypedescriptor.aspx
    /// http://msdn.microsoft.com/en-us/magazine/cc163816.aspx
    /// http://zcoder.blogspot.com/2007/11/icustomtypedescriptor.html
    /// </summary>
    [Serializable]
    public sealed class ConfigWrapper : ICustomTypeDescriptor, INotifyPropertyChanged, IXmlSerializable, ISerializable, IModifiable
    {
        private static readonly int Version = 1;

        #region Constructor

        /// <summary>
        /// Needed for XML serialization
        /// </summary>
        internal ConfigWrapper()
        {
        }

        internal ConfigWrapper(ConfigWrapperDefinition configDefinition)
        {
            m_configWrapperDefinition = configDefinition;
            InitDefaultConfigWrapper();
        }

        protected ConfigWrapper(ConfigWrapper other)
        {
            if (other == null)
                throw new ArgumentNullException();

            m_configWrapperDefinition = other.m_configWrapperDefinition;
            InitDefaultConfigWrapper();

            foreach (KeyValuePair<string, ConfigPropertyObject> pair in other.m_configValues)
            {
                var property = new ConfigPropertyObject(pair.Value);
                m_configValues[pair.Key] = property;
                property.PropertyChanged += property_PropertyChanged;
            }
        }

        private ConfigWrapperDefinition m_configWrapperDefinition;

        private void InitDefaultConfigWrapper()
        {
            ConfigValues.Clear();
            if (m_configWrapperDefinition != null && m_configWrapperDefinition.Properties.Count > 0)
            {
                //init config values
                foreach (string propertyName in m_configWrapperDefinition.Properties.Keys)
                {
                    var property = new ConfigPropertyObject(m_configWrapperDefinition.Properties[propertyName]);
                    ConfigValues.Add(propertyName, property);
                    property.PropertyChanged += property_PropertyChanged;
                }

                IsJava = m_configWrapperDefinition.IsJava;
            }
        }

        void property_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                IsModified = true;
            }
        }

        #endregion

        #region Methods

        internal void AddProperty(ConfigPropertyObject propertyObject)
        {
            if (ConfigValues.ContainsKey(propertyObject.Name) == false)
            {
                //add Property to ConfigValues
                ConfigValues.Add(propertyObject.Name, propertyObject);

                //update also definition just for consistency
                if (m_configWrapperDefinition != null)
                {
                    m_configWrapperDefinition.AddProperty(propertyObject);
                }
            }
        }

        internal void RemoveProperty(string propertyName)
        {
            if (ConfigValues.ContainsKey(propertyName) == true)
            {
                //remove Property to ConfigValues
                ConfigValues.Remove(propertyName);

                //update also definition just for consistency 
                if (m_configWrapperDefinition != null)
                {
                    m_configWrapperDefinition.RemoveProperty(propertyName);
                }
            }
        }

        internal void Clear()
        {
            ConfigValues.Clear();
            if (m_configWrapperDefinition != null)
            {
                m_configWrapperDefinition.Clear();
            }
        }

        internal void UpdateConfigValuesBasedOn(ConfigWrapper tmpConfigWrapper)
        {
            foreach (ConfigPropertyObject tmpPropertyObject in tmpConfigWrapper.ConfigValues.Values)
            {
                ConfigPropertyObject propertyObject;
                if (ConfigValues.TryGetValue(tmpPropertyObject.Name, out propertyObject))
                {
                    propertyObject.Value = tmpPropertyObject.Value;
                }
            }
        }

        internal ConfigWrapper Clone()
        {
            return new ConfigWrapper(this);
        }

        /// <summary>
        /// Sets the experiment location root, which all paths should be relative to.
        /// </summary>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        internal void SetExperimentLocationRoot(string experimentLocationRoot, bool transformRelative)
        {
            foreach (KeyValuePair<string, ConfigPropertyObject> pair in m_configValues)
            {
                pair.Value.SetExperimentLocationRoot(experimentLocationRoot, transformRelative);
            }
        }

        /// <summary>
        /// Sets the experiment location root, which all paths should be relative to.
        /// Obsolete because it still support all FilePath that were relative to the dataroot.
        /// 
        /// Note, that method will update FilePaths and DirectoryPaths of config values.
        /// if transform relative is set to true then
        /// Relative properties will be updated.
        /// Absolue properties won't change.
        /// Dataroot will be updated to new location.
        /// 
        /// if transform relative is set to false then
        /// Relative properties won't change.
        /// Absolue properties will be updated to new location
        /// Dataroot will be updated to new location.
        /// 
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        /// <param name="dataRoot">The data root to which old path were related to. Not used in new version of FilePath</param>
        /// <param name="transformRelative">if set to <c>true</c> [transform relative].</param>
        [Obsolete]
        internal void SetExperimentLocationRoot(string experimentLocationRoot, string dataRoot, bool transformRelative)
        {
            foreach (KeyValuePair<string, ConfigPropertyObject> pair in m_configValues)
            {
                pair.Value.SetExperimentLocationRoot(experimentLocationRoot, dataRoot, transformRelative);
            }
        }

        /// <summary>
        /// Copies the referenced files by FilePath configs.
        /// Also it creates referenced directories, but don't copy files in the directories.
        /// Note, that method will update FilePaths and DirectoryPaths of config values.
        /// Relative properties won't change.
        /// Absolue properties will be updated to new location.
        /// Dataroot will be updated to new location.
        /// </summary>
        /// <param name="newExperimentLocation">The new experiment location.</param>
        /// <param name="oldExperimentLocation">The old experiment location.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite] the method will attempt to overwrite files. If not successful it will report failure of copying, but will continue to copy rest of files.</param>
        /// <exception cref="TraceLab.Core.Exceptions.FilesCopyFailuresException">Throws exception if there were any errors during copying process. The exception contains all reported errors during copy process.</exception>
        internal void CopyReferencedFiles(string newExperimentLocation, string oldExperimentLocation, bool overwrite)
        {
            List<string> copyErrors = new List<string>();

            foreach (KeyValuePair<string, ConfigPropertyObject> pair in m_configValues)
            {
                ConfigPropertyObject configObj = pair.Value;
                pair.Value.SetExperimentLocationRoot(newExperimentLocation, false); //change file path root and absolute path, but don't transform relative paths

                TraceLabSDK.Component.Config.FilePath filePath = configObj.Value as TraceLabSDK.Component.Config.FilePath;
                TraceLabSDK.Component.Config.DirectoryPath dirPath = configObj.Value as TraceLabSDK.Component.Config.DirectoryPath;
                if (filePath != null)
                {   
                    //if new directories do not exists create them
                    if (System.IO.Directory.Exists(oldExperimentLocation) == false)
                    {
                        System.IO.Directory.CreateDirectory(oldExperimentLocation);
                    }
                    //old absolute path to the referenced file
                    string oldAbsolute = System.IO.Path.GetFullPath(System.IO.Path.Combine(oldExperimentLocation, filePath.Relative));
                    
                    //copy from old location to new location
                    try
                    {
                        if (System.IO.File.Exists(filePath.Absolute))
                        {
                            //don't copy file if overwrite is false and file at new location already exists
                            if (overwrite) System.IO.File.Copy(oldAbsolute, filePath.Absolute, overwrite);
                        }
                        else
                        {
                            //if file at new location does not exists always copy
                            System.IO.File.Copy(oldAbsolute, filePath.Absolute, overwrite);
                        }
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        //if the file at the new location already existed and was readonly
                        var message = string.Format("Unable to copy file '{0}' to the new location. {1}", oldAbsolute, ex.Message);
                        copyErrors.Add(message);
                    }
                    catch (System.IO.IOException ex)
                    {
                        //for example if original file was locked (used by another process)
                        var message = string.Format("Unable to copy file '{0}' to the new location. {1}", oldAbsolute, ex.Message);
                        copyErrors.Add(message);
                    }
                }
                else if (dirPath != null)
                {
                    //if new directories do not exists create them
                    if (System.IO.Directory.Exists(dirPath) == false)
                    {
                        System.IO.Directory.CreateDirectory(dirPath);
                    }
                }
            }

            if (copyErrors.Count > 0)
            {
                throw new TraceLab.Core.Exceptions.FilesCopyFailuresException(Messages.FailedToCopyFiles, copyErrors);
            }
        }

        /// <summary>
        /// Creates a new instances of the ConfigWrapper with references to the ConfigPropertiesObjects in this ConfigWrapper,
        /// that within their name they contain the given id.
        /// </summary>
        /// <param name="id">The node id.</param>
        /// <param name="ignoreVisible">if set to <c>true</c> the ConfigPropertyObjects visibility parameters are ignored in the returned ConfigWrapper. All ConfigPropertyObjects are going to be visible.</param>
        /// <returns>
        /// New instance of ConfigWrapper with references to ConfigValues for the given node.
        /// </returns>
        public ConfigWrapper CreateViewForId(string id, bool ignoreVisible) 
        {
            ConfigWrapper localConfigWrapper = new ConfigWrapper();

            foreach (KeyValuePair<string, ConfigPropertyObject> pair in ConfigValues)
            {
                if (pair.Value.Name.Contains(id))
                {
                    localConfigWrapper.ConfigValues.Add(pair.Key, pair.Value);
                }
            }

            // ignore the ConfigPropertyObjectsVisibility
            localConfigWrapper.m_ignoreVisibility = ignoreVisible;

            return localConfigWrapper;
        }

        #endregion

        #region Properties

        private bool m_isJava;
        /// <summary>
        /// Gets or sets a value indicating whether this config wrapper was constructed for java component.
        /// </summary>
        /// <value>
        ///   <c>true</c> this config wrapper was constructed; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        public bool IsJava
        {
            get
            {
                return m_isJava;
            }
            set
            {
                if (m_isJava != value)
                {
                    m_isJava = value;
                    OnPropertyChanged("IsJava");
                }
            }
        }

        private ObservableDictionary<string, ConfigPropertyObject> m_configValues = new ObservableDictionary<string, ConfigPropertyObject>();
        /// <summary>
        /// Gets or sets the config values dictionary.
        /// The keys in the dictionary represent ConfigProperty name, and ConfigPropertyObject is actual value for the config property.
        /// In other words it is a lookup from Config parameters to their values.
        /// </summary>
        /// <value>
        /// The config values.
        /// </value>
        public ObservableDictionary<string, ConfigPropertyObject> ConfigValues
        {
            get
            {
                return m_configValues;
            }
            set
            {
                if (m_configValues != value)
                {
                    m_configValues = value;
                    OnPropertyChanged("ConfigValues");
                }
            }
        }

        #endregion

        #region ICustomTypeDescriptor

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection m_PropertyDescriptorCollection = null;
            if (m_PropertyDescriptorCollection == null)
            {
                List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
                
                foreach (ConfigPropertyObject property in ConfigValues.Values)
                {
                    //if ignore visibility flag is set to true, return all properties regardless their visibility 
                    if (m_ignoreVisibility == true)
                    {
                        properties.Add(new ConfigPropertyDescriptor(property, null));
                    }
                    else
                    {
                        //otherwise, check PropertyObject Visibility, and return only these that are set to be visible
                        if (property.Visible == true)
                        {
                            properties.Add(new ConfigPropertyDescriptor(property, null));
                        }
                    }
                }
                m_PropertyDescriptorCollection = new PropertyDescriptorCollection(properties.ToArray());
            }
            return m_PropertyDescriptorCollection;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        #endregion

        #region Equals_HashCode

        public override bool Equals(object obj)
        {
            ConfigWrapper other = obj as ConfigWrapper;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(IsJava, other.IsJava);
            isEqual &= TraceLab.Core.Utilities.CollectionsHelper.DictionaryContentEquals<string, ConfigPropertyObject>(ConfigValues, other.ConfigValues);

            return isEqual;
        }

        public override int GetHashCode()
        {
            int javaHash = IsJava.GetHashCode();
            int configValuesHash = ConfigValues.GetHashCode();

            return javaHash ^ configValuesHash;
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
        internal void OnPropertyChanged(string prop)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region IXmlSerializable

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("topReader");

            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();
            XPathNavigator iter = nav.SelectSingleNode("/Metadata/ConfigWrapper");
            if (iter != null)
            {
                String version = iter.GetAttribute("Version", String.Empty);
                if (string.IsNullOrEmpty(version))
                {
                    //read non versioned old format
                    ReadNonVersioned(iter);
                }
                else
                {
                    ReadVersionOne(iter);
                }
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        private void ReadNonVersioned(XPathNavigator nav)
        {
            XPathNavigator iter = nav.SelectSingleNode("/Metadata/ConfigWrapper/IsJava");
            string isJavaString = iter.Value;
            bool tmpIsJava;
            if (isJavaString != null && bool.TryParse(isJavaString, out tmpIsJava))
            {
                IsJava = tmpIsJava;
            }

            XPathNodeIterator propertyObjectsIterator = nav.Select("/Metadata/ConfigWrapper//PropertyObject");
            while (propertyObjectsIterator.MoveNext())
            {
                ConfigPropertyObject propertyObject = new ConfigPropertyObject();
                propertyObject.ReadXml(propertyObjectsIterator.Current.ReadSubtree());
                propertyObject.PropertyChanged += property_PropertyChanged;
                ConfigValues.Add(propertyObject.Name, propertyObject);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        private void ReadVersionOne(XPathNavigator nav)
        {
            XPathNavigator iter = nav.SelectSingleNode("/Metadata/ConfigWrapper");
            string isJavaString = iter.GetAttribute("IsJava", String.Empty);
            bool tmpIsJava;
            if (isJavaString != null && bool.TryParse(isJavaString, out tmpIsJava))
            {
                IsJava = tmpIsJava;
            }

            XPathNodeIterator propertyObjectsIterator = nav.Select("/Metadata/ConfigWrapper//PropertyObject");
            while (propertyObjectsIterator.MoveNext())
            {
                ConfigPropertyObject propertyObject = new ConfigPropertyObject();
                propertyObject.ReadXml(propertyObjectsIterator.Current.ReadSubtree());
                propertyObject.PropertyChanged += property_PropertyChanged;
                ConfigValues.Add(propertyObject.Name, propertyObject);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("ConfigWrapper");

            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteAttributeString("Version", Version.ToString(CultureInfo.CurrentCulture));

            writer.WriteAttributeString("IsJava", IsJava.ToString(CultureInfo.CurrentCulture));
            writer.WriteStartElement("ConfigValues");
            foreach (ConfigPropertyObject propertyObject in ConfigValues.Values)
            {
                propertyObject.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion

        #region Deserialization Constructor

        protected ConfigWrapper(SerializationInfo info, StreamingContext context)
        {
            m_configWrapperDefinition = (ConfigWrapperDefinition)info.GetValue("m_configWrapperDefinition", typeof(ConfigWrapperDefinition));
            m_isJava = (bool)info.GetValue("m_isJava", typeof(bool));
            m_configValues = (ObservableDictionary<string, ConfigPropertyObject>)info.GetValue("m_configValues", typeof(ObservableDictionary<string, ConfigPropertyObject>));
        }

        #endregion

        #region ISerializable Implementation

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_configWrapperDefinition", m_configWrapperDefinition);
            info.AddValue("m_isJava", m_isJava);
            info.AddValue("m_configValues", m_configValues);
        }

        #endregion

        #region Static Helper methods

        /// <summary>
        /// Reads the config.
        /// </summary>
        /// <param name="nav">A cursor in the xml data</param>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        internal static ConfigWrapper ReadConfig(XPathNavigator nav)
        {
            ConfigWrapper tmpConfigWrapper = new ConfigWrapper();
            tmpConfigWrapper.ReadXml(nav.ReadSubtree());
            return tmpConfigWrapper;
        }

        #endregion

        #region IModifiable Members

        private bool m_isModified;
        private bool m_deferModification;
        public bool IsModified
        {
            get
            {
                if (m_deferModification)
                {
                    m_isModified = CalculateModification();
                    m_deferModification = false;
                }

                return m_isModified;
            }
            set
            {
                if (m_isModified != value)
                {
                    m_deferModification = true;
                    OnPropertyChanged("IsModified");
                }
            }
        }

        private bool CalculateModification()
        {
            bool isModified = false;
            foreach (KeyValuePair<string, ConfigPropertyObject> obj in m_configValues)
            {
                isModified |= obj.Value.IsModified;
            }

            return isModified;
        }

        public void ResetModifiedFlag()
        {
            foreach (KeyValuePair<string, ConfigPropertyObject> obj in m_configValues)
            {
                obj.Value.ResetModifiedFlag();
            }

            IsModified = false;
        }

        #endregion

        /// <summary>
        /// If set to true, then the visibility of ConfigPropertyObjects is ignored.
        /// All ConfigObjects are going to be visible. As default it is set to false. 
        /// </summary>
        private bool m_ignoreVisibility = false;

        internal void Merge(string idPrefix, ConfigWrapper configWrapper)
        {
            foreach (KeyValuePair<string, ConfigPropertyObject> pair in configWrapper.m_configValues)
            {
                string extendedParameterName = String.Format("{0}:{1}", idPrefix, pair.Key);
                if (ConfigValues.ContainsKey(extendedParameterName) == false)
                {
                    var configValue = pair.Value;
                    var property = new ConfigPropertyObject(extendedParameterName, configValue.Value, configValue.Type, configValue.AssemblyQualifiedName, configValue.IsEnum, configValue.DisplayName, configValue.Description);
                    ConfigValues.Add(extendedParameterName, property);
                    property.PropertyChanged += property_PropertyChanged;
                }
            }
        }
    }
}

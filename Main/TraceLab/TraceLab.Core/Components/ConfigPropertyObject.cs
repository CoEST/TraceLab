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
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Schema;
using TraceLab.Core.Utilities;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Serializable property allows it to be stored in xml. Used in saving and loading the experiment.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ConfigPropertyConverter))]
    public class ConfigPropertyObject : INotifyPropertyChanged, IXmlSerializable, ISerializable, IModifiable
    {
        private const long CurrentVersion = 3;

        #region Constructor

        public ConfigPropertyObject() { }

        public ConfigPropertyObject(string name, object value, string type, string assemblyQualifiedName, bool isEnum, string displayName, string description)
            : this(name, value, type, assemblyQualifiedName, isEnum, displayName, description, true)
        {
        }

        public ConfigPropertyObject(string name, object value, string type, string assemblyQualifiedName, bool isEnum, string displayName, string description, bool visible)
        {
            Name = name;
            Value = value;
            Type = type;
            AssemblyQualifiedName = assemblyQualifiedName;
            DisplayName = displayName ?? name;
            Description = description ?? name;
            Visible = visible;
            IsEnum = isEnum;
        }

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public ConfigPropertyObject(string name, object value, Type type, string displayName = null, string descripton = null) :
            this(name, value, type.FullName, type.GetTraceLabQualifiedName(), type.IsEnum, displayName, descripton, true)
        {
            if (IsEnum)
            {
                m_value = string.Empty;
                var enumInfo = new EnumValueCollection(type);
                EnumInfo = enumInfo;
                TypeDescriptor.AddProvider(new EnumValueDescriptionProvider(EnumInfo), EnumInfo);
                Value = value.ToString();
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public ConfigPropertyObject(ConfigPropertyObject propertyObject)
        {
            Name = String.Copy(propertyObject.Name);
            Value = ObjectCopier.Clone(propertyObject.Value);
            Type = String.Copy(propertyObject.Type);
            AssemblyQualifiedName = String.Copy(propertyObject.AssemblyQualifiedName);
            DisplayName = String.Copy(propertyObject.DisplayName);
            Description = String.Copy(propertyObject.Description);
            Visible = propertyObject.Visible;
            IsEnum = propertyObject.IsEnum;

            if (IsEnum)
            {
                var enumInfo = new EnumValueCollection(propertyObject.EnumInfo);
                EnumInfo = enumInfo;
                TypeDescriptor.AddProvider(new EnumValueDescriptionProvider(EnumInfo), EnumInfo);
                Value = propertyObject.Value.ToString();
            }
        }

        #endregion

        #region Properties

        private string m_name;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private object m_value;
        public object Value
        {
            get
            {
                if (IsEnum)
                {
                    return m_enumInfo;
                }

                return m_value;
            }
            set
            {
                if (IsEnum)
                {
                    if (value == null)
                    {
                        return;
                    }

                    if (value.GetType() == typeof(EnumValueCollection))
                    {
                        m_enumInfo = (EnumValueCollection)value;
                    }
                    else 
                    {
                        TypeDescriptionProvider typeDescriptorProvider = TypeDescriptor.GetProvider(m_enumInfo);
                        var converter = typeDescriptorProvider.GetTypeDescriptor(m_enumInfo).GetConverter();
                        if(converter.CanConvertFrom(value.GetType())) 
                        {
                            m_enumInfo = (EnumValueCollection)converter.ConvertFrom(value);
                        }
                    }
                   
                    OnPropertyChanged("Value");
                    IsModified = true;
                }
                else if (m_value != value)
                {
                    m_value = value;
                    OnPropertyChanged("Value");
                    IsModified = true;
                }
            }
        }

        private string m_type;
        public string Type
        {
            get
            {
                return m_type;
            }
            private set
            {
                if (m_type != value)
                {
                    m_type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private string m_actualValueTypeName; // This is the underlying Type of the value, if it differs from what the Config specifies.  Eg. a derived class.
        private string m_assemblyQualifiedName;
        public string AssemblyQualifiedName
        {
            get
            {
                return m_assemblyQualifiedName;
            }
            set
            {
                if (m_assemblyQualifiedName != value)
                {
                    m_assemblyQualifiedName = value;
                    OnPropertyChanged("AssemblyQualifiedName");
                }
            }
        }

        private string m_displayName;
        public string DisplayName
        {
            get
            {
                return m_displayName;
            }
            set
            {
                if (m_displayName != value)
                {
                    m_displayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }

        private string m_description;
        public string Description
        {
            get
            {
                return m_description;
            }
            set
            {
                if (m_description != value)
                {
                    m_description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        
        private bool m_visible;
        public bool Visible
        {
            get
            {
                return m_visible;
            }
            set
            {
                if (m_visible != value)
                {
                    m_visible = value;
                    OnPropertyChanged("Visible");
                }
            }
        }

        private bool m_isEnum;
        public bool IsEnum
        {
            get
            {
                return m_isEnum;
            }
            private set
            {
                m_isEnum = value;
            }
        }

        private EnumValueCollection m_enumInfo;
        public EnumValueCollection EnumInfo
        {
            get { return m_enumInfo; }
            private set { m_enumInfo = value; }
        }

        /// <summary>
        /// Sets the experiment location root, which all relative paths should be relative to.
        /// </summary>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        public void SetExperimentLocationRoot(string experimentLocationRoot, bool transformRelative)
        {
            TraceLabSDK.Component.Config.BasePath filePath = Value as TraceLabSDK.Component.Config.BasePath;
            TraceLabSDK.Component.Config.DirectoryPath dirPath = Value as TraceLabSDK.Component.Config.DirectoryPath;
            if (filePath != null)
            {
                filePath.SetDataRoot(experimentLocationRoot, transformRelative);
            }
            else if (dirPath != null)
            {
                dirPath.SetDataRoot(experimentLocationRoot, transformRelative);
            }
        }

        /// <summary>
        /// Sets the experiment location root. In place as long as there might exists old filepaths that
        /// are relative to the base root (and not to experiment location).
        /// Eventually should be removed and above method SetExperimentLocationRoot(string experimentLocationRoot) should be used instead.
        /// </summary>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        /// <param name="dataRoot">The data root.</param>
        [Obsolete]
        public void SetExperimentLocationRoot(string experimentLocationRoot, string dataRoot, bool transformRelative)
        {
            TraceLabSDK.Component.Config.FilePath filePath = Value as TraceLabSDK.Component.Config.FilePath;
            TraceLabSDK.Component.Config.DirectoryPath dirPath = Value as TraceLabSDK.Component.Config.DirectoryPath;
            if (filePath != null)
            {
                if (filePath.relativeToDataRoot == true)
                {
                    //transform Relative path to be relative to experiment location, and not to dataroot
                    filePath.SetDataRoot(dataRoot, true); //set it first to data root (old relation)
                    filePath.SetDataRoot(experimentLocationRoot, true); //and reset to new data root - it will transform relative path so that it is related to new location
                    filePath.relativeToDataRoot = false;
                }
                else
                {
                    filePath.SetDataRoot(experimentLocationRoot, transformRelative);
                }
            }
            else if(dirPath != null)
            {
                dirPath.SetDataRoot(experimentLocationRoot, transformRelative);
            }
        }
        
        #endregion

        #region Equals_HashCode

        public override bool Equals(object obj)
        {
            ConfigPropertyObject other = obj as ConfigPropertyObject;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(Name, other.Name);
            isEqual &= object.Equals(DisplayName, other.DisplayName);
            isEqual &= object.Equals(Description, other.Description);
            isEqual &= object.Equals(Value, other.Value);
            isEqual &= object.Equals(Type, other.Type);

            return isEqual;
        }

        public override int GetHashCode()
        {
            int hash = Name.GetHashCode();
            if (Value != null)
            {
                hash = hash ^ Value.GetHashCode();
            }
            hash = hash ^ Type.GetHashCode();
            hash = hash ^ DisplayName.GetHashCode();
            hash = hash ^ Description.GetHashCode();

            return hash;
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
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public void ReadXml(System.Xml.XmlReader reader)
        {
            var doc = new XPathDocument(reader);
            var nav = doc.CreateNavigator();

            var iter = nav.SelectSingleNode("/PropertyObject/Version");

            if (iter == null)
            {
                //no version
                ReadNonVersioned(nav); //version zero
            }
            else
            {
                long version = iter.ValueAsLong;

                if (version == CurrentVersion)
                {
                    ReadCurrentVersion(nav);
                }
                else if (version == 2)
                {
                    ReadVersion2(nav);
                }
            }

        }

        private void ReadNonVersioned(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("/PropertyObject");
            if (iter == null)
                throw new XmlSchemaException("Property object does not have the correct xml.");

            if (iter.HasAttributes)
            {
                Name = iter.GetAttribute("name", "");
                Type = iter.GetAttribute("type", "");

                Type valueType = System.Type.GetType(Type);
                if (valueType != null)
                {
                    AssemblyQualifiedName = valueType.GetTraceLabQualifiedName();
                }
                else
                {
                    //TODO: search for types... probably not needed in any old experiment
                }

                //read value, if exists (it may be that config value was not set, and thus it is not in xml file
                XPathNavigator valueNav = iter.SelectSingleNode("/PropertyObject/Value");

                if (valueNav != null)
                {
                    XmlReader dataReader = valueNav.ReadSubtree();
                    dataReader.MoveToContent();
                    dataReader.Read();
                    XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(valueType, null);
                    Value = serializer.Deserialize(dataReader);
                }
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        private void ReadCurrentVersion(XPathNavigator nav)
        {
            ReadVersion3(nav);
        }

        private void ReadVersion3(XPathNavigator nav)
        {
            //read data type
            var iter = nav.SelectSingleNode("/PropertyObject/ActualValueType");
            if (iter != null)
            {
                m_actualValueTypeName = TraceLabSDK.TypeHelper.ConvertOldTypeName(iter.Value);
            }

            ReadVersion2(nav);
        }

        private void ReadVersion2(XPathNavigator nav)
        {
            //read name
            var iter = nav.SelectSingleNode("/PropertyObject/Name");
            if (iter == null)
                throw new XmlSchemaException("Property Object does not contain the proper xml.");

            Name = iter.Value;

            //read display name
            iter = nav.SelectSingleNode("/PropertyObject/DisplayName");
            if (iter != null && String.IsNullOrEmpty(iter.Value) == false)
            {
                DisplayName = iter.Value;
            }
            else
            {
                DisplayName = Name;
            }

            iter = nav.SelectSingleNode("/PropertyObject/Description");
            if (iter != null && String.IsNullOrEmpty(iter.Value) == false)
            {
                Description = iter.Value;
            }
            else
            {
                Description = String.Empty;
            }

            //read if visible
            iter = nav.SelectSingleNode("/PropertyObject/Visible");
            Visible = true; //default
            if (iter != null && String.IsNullOrEmpty(iter.Value) == false)
            {
                string visibleString = iter.Value;
                bool tmpVisible;
                if (bool.TryParse(visibleString, out tmpVisible))
                {
                    Visible = tmpVisible;
                }
            }

            //read enum information
            iter = nav.SelectSingleNode("/PropertyObject/IsEnum");
            if (iter != null)
            {
                IsEnum = bool.Parse(iter.Value);
                if (IsEnum)
                {
                    iter = nav.SelectSingleNode("/PropertyObject/EnumInfo");
                    if (iter == null)
                        throw new XmlSchemaException("Property Object does not contain the proper xml.");

                    var dataReader = iter.ReadSubtree();
                    dataReader.MoveToContent();
                    dataReader.Read();

                    var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(EnumValueCollection), null);
                    EnumInfo = (EnumValueCollection)serializer.Deserialize(dataReader);
                    TypeDescriptor.AddProvider(new EnumValueDescriptionProvider(EnumInfo), EnumInfo);

                    AssemblyQualifiedName = EnumInfo.SourceEnum;
                    Type = AssemblyQualifiedName.Remove(AssemblyQualifiedName.IndexOf(','));
                }
            }

            if (!IsEnum)
            {
                //read data type
                iter = nav.SelectSingleNode("/PropertyObject/ValueType");
                if (iter == null)
                    throw new XmlSchemaException("Property Object does not contain the proper xml.");

                Type valueType = System.Type.GetType(TraceLabSDK.TypeHelper.ConvertOldTypeName(iter.Value));
                if (valueType == null)
                    throw new InvalidOperationException(string.Format("Type {0} does not exist.", iter.Value));

                AssemblyQualifiedName = valueType.GetTraceLabQualifiedName();
                Type = valueType.FullName;

                //read value
                iter = nav.SelectSingleNode("/PropertyObject/Value");
                if (iter == null)
                    throw new XmlSchemaException("Property Object does not contain the proper xml.");
                if (iter.GetAttribute("IsNull", "") != "True")
                {
                    var typeToDeserialize = valueType;
                    if (!string.IsNullOrWhiteSpace(m_actualValueTypeName))
                    {
                        typeToDeserialize = System.Type.GetType(TraceLabSDK.TypeHelper.ConvertOldTypeName(m_actualValueTypeName));
                    }

                    var dataReader = iter.ReadSubtree();
                    dataReader.MoveToContent();
                    dataReader.Read();
                    var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeToDeserialize, null);

                    Value = serializer.Deserialize(dataReader);
                }
                else
                {
                    Value = null;
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation. Always writes the latest version.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("PropertyObject");
            writer.WriteElementString("Version", CurrentVersion.ToString(CultureInfo.CurrentCulture));
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("DisplayName", DisplayName);
            writer.WriteElementString("Description", Description);
            if (Value != null)
            {
                var traceLabName = Value.GetType().GetTraceLabQualifiedName();
                if(string.Equals(AssemblyQualifiedName, traceLabName) == false)
                {
                    writer.WriteElementString("ActualValueType", traceLabName);
                }
            }
            writer.WriteElementString("ValueType", AssemblyQualifiedName);
            writer.WriteElementString("Visible", Visible.ToString(System.Globalization.CultureInfo.CurrentCulture));
            writer.WriteElementString("IsEnum", IsEnum.ToString());
            if (IsEnum)
            {
                writer.WriteStartElement("EnumInfo");
                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(EnumValueCollection), null);
                serializer.Serialize(writer, EnumInfo);
                writer.WriteEndElement();
            }
            else
            {
                // Serialize the data.
                writer.WriteStartElement("Value");

                if (Value == null)
                {
                    writer.WriteAttributeString("IsNull", "True");
                }
                else
                {
                    writer.WriteAttributeString("IsNull", "False");
                    var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(System.Type.GetType(AssemblyQualifiedName), null);
                    serial.Serialize(writer, Value);
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        #endregion

        #region Deserialization Constructor

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        protected ConfigPropertyObject(SerializationInfo info, StreamingContext context)
        {
            m_name = (string)info.GetValue("m_name", typeof(string));
            m_displayName = (string)info.GetValue("m_displayName", typeof(string));
            m_description = (string)info.GetValue("m_description", typeof(string));
            m_value = (object)info.GetValue("m_value", typeof(object));
            m_type = (string)info.GetValue("m_type", typeof(string));
            m_assemblyQualifiedName = (string)info.GetValue("m_assemblyQualifiedName", typeof(string));
            m_visible = (bool)info.GetValue("m_visible", typeof(bool));
            m_isEnum = (bool)info.GetValue("m_isEnum", typeof(bool));
            if (m_isEnum)
            {
                m_enumInfo = (EnumValueCollection)info.GetValue("m_enumInfo", typeof(EnumValueCollection));
                TypeDescriptor.AddProvider(new EnumValueDescriptionProvider(EnumInfo), EnumInfo);
            }
        }

        #endregion

        #region ISerializable Implementation

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_name", m_name);
            info.AddValue("m_displayName", m_displayName);
            info.AddValue("m_description", m_description);
            info.AddValue("m_value", m_value);
            info.AddValue("m_type", m_type);
            info.AddValue("m_assemblyQualifiedName", m_assemblyQualifiedName);
            info.AddValue("m_visible", m_visible);
            info.AddValue("m_isEnum", m_isEnum);
            if (m_isEnum)
            {
                info.AddValue("m_enumInfo", m_enumInfo);
            }
        }

        #endregion

        #region IModifiable Members

        private bool m_isModified;
        public bool IsModified
        {
            get
            {
                return m_isModified;
            }
            set
            {
                if (m_isModified != value)
                {
                    m_isModified = value;
                    OnPropertyChanged("IsModified");
                }
                
            }
        }

        public void ResetModifiedFlag()
        {
            IsModified = false;
        }

        #endregion
    }
}

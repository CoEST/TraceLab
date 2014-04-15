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
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Components;
using System.Collections.Generic;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Represents node data that is serialized to xml
    /// </summary>
    [Serializable]
    public class SerializedVertexData : INotifyPropertyChanged, IXmlSerializable, ISerializable, IModifiable
    {
        #region Properties

        private const int Version = 1;
        protected bool m_isInitialized;

        /// <summary>
        /// Node X coordinate
        /// </summary>
        private double m_x;
        [XmlAttribute("X")]
        public double X
        {
            get { return m_x; }
            set
            {
                if (m_x != value)
                {
                    m_x = ConstrainDouble(value);
                    NotifyPropertyChanged("X");
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Node Y coordinate
        /// </summary>
        private double m_y;
        [XmlAttribute("Y")]
        public double Y
        {
            get { return m_y; }
            set
            {
                if (m_y != value)
                {
                    m_y = ConstrainDouble(value);
                    NotifyPropertyChanged("Y");
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Metadata represents specific node data. 
        /// </summary>
        private Metadata m_metadata;
        [XmlElement("Metadata")]
        public Metadata Metadata
        {
            get
            {
                return m_metadata;
            }
            set
            {
                if (m_metadata != value)
                {
                    if(m_metadata != null)
                        m_metadata.PropertyChanged -= MetadataPropertyChanged;

                    m_metadata = value;
                    NotifyPropertyChanged("Metadata");
                    NotifyPropertyChanged("HasError");
                    NotifyPropertyChanged("ErrorMessage");

                    if (m_metadata != null)
                        m_metadata.PropertyChanged += MetadataPropertyChanged;
                }
            }
        }

        private bool m_isModified;
        [XmlIgnore]
        public bool IsModified
        {
            get { return m_isModified || Metadata.IsModified; }
            set
            {
                if (m_isModified != value)
                {
                    m_isModified = value;
                    NotifyPropertyChanged("IsModified");
                }
            }
        }

        [XmlIgnore]
        public bool HasError
        {
            get { return m_errorMessage != null && m_errorMessage.Length != 0; }
        }

        private string m_errorMessage;
        [XmlIgnore]
        public string ErrorMessage
        {
            get { return m_errorMessage; }
            private set
            {
                if (m_errorMessage != value)
                {
                    m_errorMessage = value;
                    NotifyPropertyChanged("HasError");
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }


        #endregion

        public void ResetModifiedFlag()
        {
            Metadata.ResetModifiedFlag();
            IsModified = false;
        }

        #region Private_HelperMethods

        protected static double ConstrainDouble(double baseValue)
        {
            double val = System.Math.Round(baseValue, 3);

            return val;
        }

        void MetadataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                NotifyPropertyChanged("IsModified");
            }
            else if (e.PropertyName == "DeserializationErrorMessage")
            {
                ErrorMessage = Metadata.DeserializationErrorMessage;
            }
        }

        #endregion

        #region Equals_HashCode

        public override bool Equals(object obj)
        {
            SerializedVertexData other = obj as SerializedVertexData;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(X, other.X);
            isEqual &= object.Equals(Y, other.Y);

            return isEqual;
        }

        public override int GetHashCode()
        {
            int xHash = X.GetHashCode();
            int yHash = Y.GetHashCode();

            return xHash ^ yHash;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region IXmlSerializable

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            ReadXmlVertexData(reader, (r) =>
            {
                X = double.Parse(r.GetAttribute("X"), CultureInfo.CurrentCulture);
                Y = double.Parse(r.GetAttribute("Y"), CultureInfo.CurrentCulture);
            });
        }

        protected void ReadXmlVertexData(System.Xml.XmlReader reader, Action<XmlReader> readAttributes)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            m_isInitialized = false;

            bool wasEmpty = reader.IsEmptyElement;

            if (wasEmpty)
                return;

            String version = reader.GetAttribute("Version");

            var types = new Type[] 
            {
                typeof(StartNodeMetadata),
                typeof(EndNodeMetadata),
                typeof(ComponentMetadata),
                typeof(DecisionMetadata),
                typeof(ComponentTemplateMetadata),
                typeof(ExitDecisionMetadata),
                typeof(ScopeMetadata),
                typeof(CompositeComponentMetadata)
            };

            //based on version read old or new type of serialized data 
            if (version == null)
            {
                //read old non versioned data
                X = double.Parse(reader.GetAttribute("X"), CultureInfo.CurrentCulture);
                Y = double.Parse(reader.GetAttribute("Y"), CultureInfo.CurrentCulture);
                string xsiNamespace = reader.GetAttribute("xmlns:xsi");
                string xsdNamespace = reader.GetAttribute("xmlns:xsd");
                XmlNamespaceManager nsMan = new XmlNamespaceManager(reader.NameTable);
                nsMan.AddNamespace("xsi", xsiNamespace);
                nsMan.AddNamespace("xsd", xsdNamespace);

                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();
                XPathNavigator iter = nav.SelectSingleNode("/SerializedVertexData/Metadata", nsMan);
                string type = iter.GetAttribute("type", xsiNamespace);

                //try to find datatype
                type = "TraceLab.Core.Components." + type;
                Type dataType = Type.GetType(type);
                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(dataType, types);
                Metadata = (Metadata)serializer.Deserialize(iter.ReadSubtree());
            }
            else
            {
                readAttributes(reader);

                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();
                XPathNavigator iter = nav.SelectSingleNode("//Metadata");

                string type = iter.GetAttribute("type", "");

                Type dataType = Type.GetType(TraceLabSDK.TypeHelper.ConvertOldTypeName(type));

                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(dataType, types);
                Metadata = (Metadata)serializer.Deserialize(iter.ReadSubtree());
            }
        }

        public void PostProcessReadXml(Components.IComponentsLibrary library, string experimentLocationRoot)
        {
            if (Metadata != null)
                Metadata.PostProcessReadXml(library, experimentLocationRoot);

            m_isInitialized = true;
            IsModified = false;
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            WriteVertexData(writer, (w) =>
            {
                writer.WriteAttributeString("X", X.ToString(CultureInfo.CurrentCulture));
                writer.WriteAttributeString("Y", Y.ToString(CultureInfo.CurrentCulture));
            });
        }

        protected void WriteVertexData(System.Xml.XmlWriter writer, Action<XmlWriter> writeAttributes)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteAttributeString("Version", Version.ToString(CultureInfo.CurrentCulture));
            writeAttributes(writer);

            //write Metadata
            var types = new Type[] 
            {
                typeof(StartNodeMetadata),
                typeof(EndNodeMetadata),
                typeof(ComponentMetadata),
                typeof(DecisionMetadata),
                typeof(CompositeComponentMetadata),
                typeof(ExitDecisionMetadata)
            };

            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(Metadata.GetType(), types);
            serializer.Serialize(writer, Metadata);
        }

        #endregion

        #region Constructor

        public SerializedVertexData()
        {
        }

        #endregion
        
        #region Deserialization Constructor

        private SerializedVertexData(SerializationInfo info, StreamingContext context)
        {
            m_x = (double)info.GetValue("m_x", typeof(double));
            m_y = (double)info.GetValue("m_y", typeof(double));
            m_metadata = (Metadata)info.GetValue("m_metadata", typeof(Metadata));
        }

        #endregion

        #region ISerializable Implementation

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue("m_x", m_x);
            info.AddValue("m_y", m_y);
            info.AddValue("m_metadata", m_metadata);
        }

        #endregion

        public virtual SerializedVertexData Clone()
        {
            SerializedVertexData clone = new SerializedVertexData();
            clone.X = X;
            clone.Y = Y;
            clone.Metadata = Metadata.Clone();

            clone.m_isInitialized = m_isInitialized;

            return clone;
        }
    }
}

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
using System.Xml.Serialization;
using System.Xml.XPath;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// ImportItemDefinition defines import item for the component. It is a part of IOSpecDefinition of ComponentMetadataDefinition.  
    /// 
    /// Required to be marked as serializable because it is passed between AppDomains, as well as it is being serialized by XMLSerializer
    /// </summary>
    [Serializable]
    public class IOItemDefinition : IXmlSerializable
    {
         #region Constructor

        /// <summary>
        /// Initializes a new s_instance of the <see cref="IOItemDefinition"/> class.
        /// </summary>
        public IOItemDefinition()
        {
        }

        /// <summary>
        /// Initializes a new s_instance of the <see cref="IOItemDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of input item.</param>
        /// <param name="type">The type of input item.</param>
        /// <param name="description">The description.</param>
        public IOItemDefinition(string name, string type, string description, TraceLabSDK.IOSpecType ioType)
        {
            Name = name;
            Type = type;
            Description = description;
            IOType = ioType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the input item
        /// </summary>
        /// <value>
        /// The name of the input item.
        /// </value>
        [XmlAttribute("Name")]
        public string Name
        {
            get;
            set;
        }

        private string m_type;
        /// <summary>
        /// System Type of the input item
        /// </summary>
        /// <value>
        /// The type of the input item
        /// </value>
        [XmlAttribute("Type")]
        public string Type
        {
            get { return m_type; }
            set
            {
                m_type = value;
                m_friendlyType = TraceLab.Core.Utilities.TypesHelper.GetFriendlyName(value);
            }
        }

        private string m_friendlyType;
        /// <summary>
        /// Gets or sets the user friendly type name
        /// </summary>
        /// <value>
        /// the user friendly type name
        /// </value>
        [XmlIgnore]
        public string FriendlyType
        {
            get { return m_friendlyType; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlIgnore]
        public string Description
        {
            get;
            set;
        }

        public TraceLabSDK.IOSpecType IOType
        {
            get;
            set;
        }

        #endregion

        #region Equals & HashCode

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this s_instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this s_instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this s_instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            IOItemDefinition other = obj as IOItemDefinition;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(Name, other.Name);
            isEqual &= object.Equals(Type, other.Type);

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
            int nameHash = Name.GetHashCode();
            int typeHash = Type.GetHashCode();

            return nameHash ^ typeHash;
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
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();
            
            var def = nav.SelectSingleNode("/IOItemDefinition");
            if (def == null)
            {
                ReadOldVersion(nav);
            }
            else
            {
                Name = def.GetAttribute("Name", String.Empty);
                Type = def.GetAttribute("Type", String.Empty);
                IOType = (TraceLabSDK.IOSpecType)Enum.Parse(typeof(TraceLabSDK.IOSpecType), def.GetAttribute("IOType", String.Empty));
            }
        }

        private void ReadOldVersion(XPathNavigator nav)
        {
            var def = nav.SelectSingleNode("/InputItemDefinition");
            if (def != null)
            {
                IOType = TraceLabSDK.IOSpecType.Input;
            }
            else
            {
                def = nav.SelectSingleNode("/OutputItemDefinition");
                IOType = TraceLabSDK.IOSpecType.Output;
            }

            Name = def.GetAttribute("Name", String.Empty);
            Type = def.GetAttribute("Type", String.Empty);
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("IOItemDefinition");
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Type", Type);
            writer.WriteAttributeString("IOType", IOType.ToString());
            writer.WriteEndElement();
        }

        #endregion
    }
}

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
using System.Xml.Serialization;
using System.ComponentModel;
using TraceLab.Core.Utilities;
using System.Xml.XPath;

namespace TraceLab.Core.Components
{
    [Serializable]
    public class IOItem : IXmlSerializable, INotifyPropertyChanged, IModifiable
    {
        #region Constructor

        /// <summary>
        /// Initializes a new s_instance of the <see cref="IOItem"/> class.
        /// </summary>
        public IOItem() { }

        /// <summary>
        /// Initializes a new s_instance of the <see cref="IOItem"/> class.
        /// </summary>
        /// <param name="ioItemDefinition">The input item definition.</param>
        /// <param name="mappedTo">What input item is going to be mapped to</param>
        public IOItem(IOItemDefinition ioItemDefinition, string mappedTo)
        {
            IOItemDefinition = ioItemDefinition;
            MappedTo = mappedTo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the input item definition.
        /// </summary>
        /// <value>
        /// The input item definition.
        /// </value>
        [XmlElement]
        public IOItemDefinition IOItemDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets what input item is going to be mapped to
        /// </summary>
        /// <value>
        /// What input item is going to be mapped to
        /// </value>
        private string m_mappedTo;
        [XmlAttribute("MappedTo")]
        public string MappedTo
        {
            get
            {
                return m_mappedTo;
            }
            set
            {
                if (m_mappedTo != value)
                {
                    m_mappedTo = value;
                    // HERZUM SPRINT 4.2: TLAB-191
                    if (value == null)
                        m_mappedTo = "";
                    // END HERZUM SPRINT 4.2: TLAB-191
                    NotifyPropertyChanged("MappedTo");
                    IsModified = true;
                }
            }
        }


        private bool m_isHighlighted = false;
        [XmlIgnore]
        public bool IsHighlighted
        {
            get { return m_isHighlighted; }
            set
            {
                if (m_isHighlighted != value)
                {
                    m_isHighlighted = value;
                    NotifyPropertyChanged("IsHighlighted");
                }
            }
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
            IOItem other = obj as IOItem;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(IOItemDefinition, other.IOItemDefinition);
            isEqual &= object.Equals(MappedTo, other.MappedTo);

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
            int inputDefinitionHash = IOItemDefinition.GetHashCode();
            int mappedToHash = MappedTo.GetHashCode();

            return inputDefinitionHash ^ mappedToHash;
        }

        #endregion

        #region IXmlSerializable

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
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

            var itemnav = nav.SelectSingleNode("/IOItem");
            if (itemnav == null)
            {
                ReadOldVersion(nav);
            }
            else
            {
                XPathNavigator iter = nav.SelectSingleNode("/IOItem/IOItemDefinition");

                IOItemDefinition = new IOItemDefinition();
                IOItemDefinition.ReadXml(iter.ReadSubtree());

                iter = nav.SelectSingleNode("/IOItem/MappedTo");
                MappedTo = iter.Value;
            }
        }

        private void ReadOldVersion(XPathNavigator nav)
        {
            var itemnav = nav.SelectSingleNode("/InputItem");
            if (itemnav != null)
            {
                XPathNavigator iter = nav.SelectSingleNode("/InputItem/InputItemDefinition");

                IOItemDefinition = new IOItemDefinition();
                IOItemDefinition.ReadXml(iter.ReadSubtree());

                iter = nav.SelectSingleNode("/InputItem/MappedTo");
                MappedTo = iter.Value;
            }
            else
            {
                itemnav = nav.SelectSingleNode("/OutputItem");
                if(itemnav != null) 
                {
                    XPathNavigator iter = nav.SelectSingleNode("/OutputItem/OutputItemDefinition");

                    IOItemDefinition = new IOItemDefinition();
                    IOItemDefinition.ReadXml(iter.ReadSubtree());

                    iter = nav.SelectSingleNode("/OutputItem/OutputAs");
                    MappedTo = iter.Value;
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("IOItem");

            IOItemDefinition.WriteXml(writer);

            writer.WriteElementString("MappedTo", MappedTo);

            writer.WriteEndElement();
        }

        #endregion

        #region INotifyPropertyChanged Members

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

        private void NotifyPropertyChanged(string property)
        {
            NotifyPropertyChanged(new PropertyChangedEventArgs(property));
        }

        private void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, e);
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
                    NotifyPropertyChanged("IsModified");
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

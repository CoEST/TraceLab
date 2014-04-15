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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    [Serializable]
    //[TypeConverter(typeof(EnumValueTypeConverter))]
    //[TypeDescriptionProvider(typeof(EnumValueDescriptionProvider))]
    public class EnumValueCollection : ISerializable, IXmlSerializable
    {
        public EnumValueCollection()
        {
        }

        internal EnumValueCollection(Type enumType)
        {
            if (enumType.IsEnum)
            {
                m_possibleValues = new List<EnumValue>();
                foreach (string val in enumType.GetEnumNames())
                {
                    EnumValue newVal = new EnumValue();
                    newVal.Value = val;
                    m_possibleValues.Add(newVal);
                }
                m_currentValue = m_possibleValues[0];
                m_sourceEnum = enumType.GetTraceLabQualifiedName();
            }
        }

        internal EnumValueCollection(EnumValueCollection other)
        {
            m_possibleValues = new List<EnumValue>();
            foreach(EnumValue val in other.m_possibleValues)
            {
                EnumValue newVal = new EnumValue();
                newVal.Value = val.Value;
                m_possibleValues.Add(newVal);
            }
            m_currentValue = GetValue(other.m_currentValue);
            m_sourceEnum = other.m_sourceEnum;
        }

        public EnumValue Set(EnumValue value)
        {
            m_currentValue = GetValue(value);
            return m_currentValue;
        }

        private EnumValue GetValue(EnumValue value)
        {
            foreach (EnumValue myValue in PossibleValues.Where(myValue => myValue.Equals(value)))
            {
                return myValue;
            }

            throw new ArgumentException("EnumValue is not contained in this enum.");
        }

        private EnumValue GetValue(string value)
        {
            foreach (EnumValue myValue in PossibleValues.Where(myValue => myValue.Equals(value)))
            {
                return myValue;
            }

            throw new ArgumentException("EnumValue is not contained in this enum.");
        }

        public override string ToString()
        {
            return m_currentValue.Value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EnumValueCollection;
            if (other != null)
            {
                if (string.Equals(SourceEnum, other.SourceEnum))
                {
                    if (CurrentValue.Equals(other.CurrentValue))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
            //return (int)(0x0000f00d) ^ m_currentValue.GetHashCode() ^ m_sourceEnum.GetHashCode();
        }

        #region Deserialization Constructor

        protected EnumValueCollection(SerializationInfo info, StreamingContext context)
        {
            m_possibleValues = (List<EnumValue>)info.GetValue("m_possibleValues", typeof(List<EnumValue>));
            m_currentValue = (EnumValue)info.GetValue("m_currentValue", typeof(EnumValue));
            m_sourceEnum = (string)info.GetValue("m_sourceEnum", typeof(string));
        }

        #endregion

        #region ISerializable Implementation

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_possibleValues", m_possibleValues);
            info.AddValue("m_currentValue", m_currentValue);
            info.AddValue("m_sourceEnum", m_sourceEnum);
        }

        #endregion

        private List<EnumValue> m_possibleValues;
        public EnumValue[] PossibleValues
        {
            get
            {
                return m_possibleValues.ToArray();
            }
        }

        private EnumValue m_currentValue;
        public EnumValue CurrentValue
        {
            get
            {
                return m_currentValue;
            }
            set
            {
                m_currentValue = value;
            }
        }

        private string m_sourceEnum;
        public string SourceEnum
        {
            get
            {
                return m_sourceEnum;
            }
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var doc = new XPathDocument(reader);
            var nav = doc.CreateNavigator();

            //read name
            var iter = nav.SelectSingleNode("./EnumValueCollection/PossibleValues");
            if (iter == null)
                throw new XmlSchemaException("EnumValueCollection does not contain the proper xml.");

            var dataReader = iter.ReadSubtree();
            dataReader.MoveToContent();
            dataReader.Read();
            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(List<EnumValue>), null);

            m_possibleValues = (List<EnumValue>)serializer.Deserialize(dataReader);

            iter = nav.SelectSingleNode("/EnumValueCollection/Value");
            if (iter == null)
                throw new XmlSchemaException("EnumValueCollection does not contain the proper xml.");

            CurrentValue = GetValue(iter.Value);

            iter = nav.SelectSingleNode("/EnumValueCollection/SourceEnum");
            if (iter == null)
                throw new XmlSchemaException("EnumValueCollection does not contain the proper xml.");

            m_sourceEnum = iter.Value;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("SourceEnum", m_sourceEnum);
            writer.WriteElementString("Value", CurrentValue.Value);

            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(List<EnumValue>), null);
            writer.WriteStartElement("PossibleValues");
            serializer.Serialize(writer, m_possibleValues);
            writer.WriteEndElement();
        }

        #endregion
    }
}
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
using System.Xml.XPath;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    [XmlRoot("Metadata")]
    [Serializable]
    public class ComponentTemplateMetadata : Metadata, IConfigurableAndIOSpecifiable, IXmlSerializable
    {
        private ComponentTemplateMetadata() { }

        public ComponentTemplateMetadata(IOSpec ioSpec, string label)
        {
            IOSpec = ioSpec;
            Label = label;
        }

        #region IConfigurableAndIOSpecifiable Members

        private IOSpec m_IOSpec;
        public IOSpec IOSpec
        {
            get
            {
                return m_IOSpec;
            }
            set
            {
                m_IOSpec = value;
            }
        }

        public ConfigWrapper ConfigWrapper
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public MetadataDefinition MetadataDefinition
        {
            get 
            {
                throw new NotSupportedException();
            }
        }
        
        #endregion

        public override Metadata Clone()
        {
            var clone = new ComponentTemplateMetadata();
            clone.CopyFrom(this);
            return clone;
        }

        protected override void CopyFrom(Metadata other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            base.CopyFrom(other);

            ComponentTemplateMetadata metadata = (ComponentTemplateMetadata)other;
            m_IOSpec = metadata.m_IOSpec.Clone();
                        
            HasDeserializationError = metadata.HasDeserializationError;
            if (HasDeserializationError)
            {
                DeserializationErrorMessage = metadata.DeserializationErrorMessage;
            }
        }

        #region IXmlSerializable Members

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/Metadata");
            Label = iter.GetAttribute("Label", String.Empty);

            //read attribute indicating if component should wait for all predecessors
            var wait = iter.GetAttribute("WaitsForAllPredecessors", String.Empty);
            if (String.IsNullOrEmpty(wait) || (wait != Boolean.TrueString && wait != Boolean.FalseString)) //if value has not been found set it to true
                WaitsForAllPredecessors = true; //default value
            else
                WaitsForAllPredecessors = Convert.ToBoolean(wait);

            //read iospec
            m_IOSpec = IOSpec.ReadIOSpec(nav.SelectSingleNode("/Metadata/IOSpec"));
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());
            writer.WriteAttributeString("Label", Label);
            writer.WriteAttributeString("WaitsForAllPredecessors", WaitsForAllPredecessors.ToString());
            IOSpec.WriteXml(writer);
        }

        #endregion
    }
}

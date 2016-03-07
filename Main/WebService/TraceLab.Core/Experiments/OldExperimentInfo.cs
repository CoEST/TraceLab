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
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace TraceLab.Core.Experiments
{
    [Obsolete]
    [Serializable]
    [XmlRoot(ElementName = "WorkflowInfo")]
    public class WorkflowInfo : IXmlSerializable
    {

        public string Id
        {
            get
            {
                return GuidId.ToString();
            }
            private set
            {
                GuidId = Guid.Parse(value);
            }
        }

        /// <summary>
        /// The Guid version of the Id for this m_workflow.
        /// </summary>
        [XmlIgnore]
        public Guid GuidId
        {
            get;
            private set;
        }

        private string m_name;
        [XmlElement("Name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("LayoutName")]
        public string LayoutName
        {
            get;
            set;
        }

        [XmlElement("Author")]
        public string Author
        {
            get;
            set;
        }

        [XmlElement("Contributors")]
        public string Contributors
        {
            get;
            set;
        }

        [XmlElement("Description")]
        public string Description
        {
            get;
            set;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.Name == "WorkflowInfo")
            {
                ReadVersion1(reader);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void ReadVersion1(XmlReader topReader)
        {
            XmlReader reader = topReader.ReadSubtree();
            XPathDocument doc = new XPathDocument(reader);
            var nav = doc.CreateNavigator();
            XPathNavigator iter = nav.SelectSingleNode("/WorkflowInfo/Version");
            if (iter == null)
            {
                ReadVersion0(nav);
            }
            else
            {
                long ver = iter.ValueAsLong;
                if (ver == 1)
                {
                    Id = ReadString(nav, "/WorkflowInfo/Id");
                    Name = ReadString(nav, "/WorkflowInfo/Name");
                    LayoutName = ReadString(nav, "/WorkflowInfo/LayoutName");
                    Author = ReadString(nav, "/WorkflowInfo/Author");
                    Contributors = ReadString(nav, "/WorkflowInfo/Contributors");
                    Description = ReadString(nav, "/WorkflowInfo/Description");
                }
                else
                {
                    throw new InvalidOperationException("Experiment Info has an invalid version number");
                }
            }
        }

        private void ReadVersion0(XPathNavigator nav)
        {
            Name = ReadString(nav, "/WorkflowInfo/Name");
            LayoutName = ReadString(nav, "/WorkflowInfo/LayoutName");
            Author = ReadString(nav, "/WorkflowInfo/Author");
            Contributors = ReadString(nav, "/WorkflowInfo/Contributors");
            Description = ReadString(nav, "/WorkflowInfo/Description");
        }

        private string ReadString(XPathNavigator nav, string path)
        {
            string value;
            var iter = nav.SelectSingleNode(path);
            if (iter != null)
            {
                value = iter.Value;
            }
            else
            {
                value = string.Empty;
            }

            return value;
        }


        public TraceLab.Core.Experiments.ExperimentInfo Convert()
        {
            var info = new TraceLab.Core.Experiments.ExperimentInfo(GuidId);
            info.Name = Name;
            info.LayoutName = LayoutName;
            info.Author = Author;
            info.Contributors = Contributors;
            info.Description = Description;

            return info;
        }
    }
}

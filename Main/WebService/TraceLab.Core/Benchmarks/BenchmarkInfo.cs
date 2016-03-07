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
using TraceLab.Core.Experiments;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

namespace TraceLab.Core.Benchmarks
{
    [Serializable]
    public class BenchmarkInfo : ExperimentInfo
    {
        public BenchmarkInfo()
        {
            //set default deadline
            Deadline = DateTime.Now;
        }

        public BenchmarkInfo(ExperimentInfo experimentInfo)
        {
            GuidId = experimentInfo.GuidId;
            Name = experimentInfo.Name;
            LayoutName = experimentInfo.LayoutName;
            Author = experimentInfo.Author;
            Contributors = experimentInfo.Contributors;
            Description = experimentInfo.Description;
            FilePath = experimentInfo.FilePath;
            //set default deadline
            Deadline = DateTime.Now;
        }

        public BenchmarkInfo(string guidId, string name, string author, string contributors, 
                            string description, string shortDescription, string filePath, 
                            string deadline, string webpageLink)
        {
            Id = guidId;
            Name = name;
            LayoutName = "EfficientSugiyama";
            Author = author;
            Contributors = contributors;
            Description = description;
            ShortDescription = shortDescription;
            FilePath = filePath;
            //set default deadline
            Deadline = DateTime.Parse(deadline);
            if (String.IsNullOrEmpty(webpageLink))
            {
                WebPageLink = null;
            }
            else
            {
                WebPageLink = new Uri(webpageLink);
            }
        }

        private string m_shortDescription;
        public string ShortDescription
        {
            get { return m_shortDescription; }
            set
            {
                if (m_shortDescription != value)
                {
                    m_shortDescription = value;
                    OnPropertyChanged("ShortDescription");
                }
            }
        }

        private DateTime m_deadline;
        public DateTime Deadline
        {
            get { return m_deadline; }
            set
            {
                if (m_deadline != value)
                {
                    m_deadline = value;
                    OnPropertyChanged("Deadline");
                }
            }
        }

        private Uri m_webpageLink;
        public Uri WebPageLink
        {
            get
            {
                return m_webpageLink;
            }
            set
            {
                if (m_webpageLink != value)
                {
                    m_webpageLink = value;
                    OnPropertyChanged("WebPageLink");
                }
            }
        }

        private string m_experimentResultsUnitname;

        public string ExperimentResultsUnitname
        {
            get { return m_experimentResultsUnitname; }
            set 
            {
                if (m_experimentResultsUnitname != value)
                {
                    m_experimentResultsUnitname = value;
                    OnPropertyChanged("ExperimentResultsUnitname");
                }
            }
        }

        public override void ReadXml(XmlReader topReader)
        {
            string infoName = "BenchmarkInfo";
            XmlReader reader = topReader.ReadSubtree();
            XPathDocument doc = new XPathDocument(reader);
            var nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode(String.Format("/{0}/Version", infoName));
            if (iter == null)
            {
                throw new XmlException("BenchmarkInfo is missing version information.");
            }
            else
            {
                long ver = iter.ValueAsLong;
                if (ver == CurrentVersion)
                {
                    ReadCurrentVersion(nav, infoName);
                }
                else
                {
                    ReadVersion1(nav, infoName);
                }
            }
        }

        protected override void ReadCurrentVersion(XPathNavigator nav, string infoName)
        {
            ReadVersion1(nav, infoName);
        }

        protected override void ReadVersion1(XPathNavigator nav, string infoName)
        {
            base.ReadVersion1(nav, infoName);

            ShortDescription = ReadString(nav, String.Format("/{0}/ShortDescription", infoName));
            ExperimentResultsUnitname = ReadString(nav, String.Format("/{0}/ExperimentResultsUnitname", infoName));

            string webpageLink = ReadString(nav, String.Format("/{0}/WebPageLink", infoName));
            if (String.IsNullOrEmpty(webpageLink))
            {
                WebPageLink = null;
            }
            else
            {
                WebPageLink = new Uri(webpageLink);
            }

            string deadlineString = ReadString(nav, String.Format("/{0}/Deadline", infoName));
            if (!String.IsNullOrEmpty(deadlineString))
            {
                Deadline = DateTime.Parse(deadlineString);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            if (!string.IsNullOrEmpty(ShortDescription))
                writer.WriteElementString("ShortDescription", ShortDescription);

            if (Deadline != null)
                writer.WriteElementString("Deadline", Deadline.ToString());

            if (!string.IsNullOrEmpty(ExperimentResultsUnitname))
                writer.WriteElementString("ExperimentResultsUnitname", ExperimentResultsUnitname);

            if (WebPageLink != null)
                writer.WriteElementString("WebPageLink", WebPageLink.ToString());
        }
    }
}

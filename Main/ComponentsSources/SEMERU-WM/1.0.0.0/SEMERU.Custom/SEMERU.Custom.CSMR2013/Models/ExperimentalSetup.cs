using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SEMERU.Types.Custom;

/// SEMERU Component Library Extension - Custom additions to the SEMERU Component Library
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library Extension.
/// 
/// The SEMERU Component Library Extension is free software: you can redistribute it
/// and/or modify it under the terms of the GNU General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or (at your
/// option) any later version.
/// 
/// The SEMERU Component Library Extension is distributed in the hope that it will
/// be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
/// License for more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library Extension.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Custom.CSMR2012.Models
{
    public static class ExperimentalSetup
    {
        public static List<CSMR13DataSet> Import(string filename)
        {
            XmlDocument XMLdoc = new XmlDocument();
            XMLdoc.Load(filename);

            string[] s = filename.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder path = new StringBuilder();
            for (long i = 0; i < s.Length - 1; i++)
            {
                path.Append(s[i]);
                path.Append("\\");
            }
            path.Append(XMLdoc.GetElementsByTagName("Datasets")[0].Attributes["Directory"].Value.Replace('/', '\\'));

            List<CSMR13DataSet> datasets = new List<CSMR13DataSet>();
            XmlNodeList nodelist = XMLdoc.GetElementsByTagName("Dataset");

            foreach (XmlNode node in nodelist)
            {
                CSMR13DataSet dataset = new CSMR13DataSet();
                dataset.SourceArtifacts = path + node["Corpus"].Attributes["Directory"].Value + node["Corpus"]["SourceArtifacts"].InnerText
                    + "#" + node["Corpus"]["SourceArtifacts"].Attributes["Extension"].Value;
                dataset.TargetArtifacts = path + node["Corpus"].Attributes["Directory"].Value + node["Corpus"]["TargetArtifacts"].InnerText
                    + "#" + node["Corpus"]["TargetArtifacts"].Attributes["Extension"].Value;
                dataset.Oracle = path + node["Corpus"].Attributes["Directory"].Value + node["Corpus"]["Oracle"].InnerText;
                dataset.Name = node.Attributes["Name"].Value;
                dataset.Relationships = path + node["Corpus"].Attributes["Directory"].Value + node["Corpus"]["Relationships"].InnerText;
                dataset.Stopwords = path + node["Corpus"].Attributes["Directory"].Value + node["Corpus"]["Stopwords"].InnerText;
                datasets.Add(dataset);
            }

            return datasets;
        }
    }
}

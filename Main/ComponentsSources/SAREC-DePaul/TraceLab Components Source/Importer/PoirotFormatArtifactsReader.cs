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
using System.Xml.XPath;
using TraceLabSDK;
using TraceLabSDK.Types;

namespace Importer
{
    /// <summary>
    /// Functions for reading artifacts from XML file in Poirot format
    /// </summary>
    public class PoirotFormatArtifactsReader
    {
        public static ComponentLogger Logger { get; set; }

        /// <summary>
        /// Reads artifacts from given XML file
        /// </summary>
        public static TLArtifactsCollection ReadXMLFile(string filepath, bool trimValues)
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();

            XPathDocument doc = new XPathDocument(filepath);
            XPathNavigator nav = doc.CreateNavigator();
            
            string art_id, art_text, art_content;

            XPathNodeIterator nodeItor = nav.Select("/artifacts/artifact");

            while (nodeItor.MoveNext())
            {
                // Only reading xml tags: art_id, art_title, art_content
                art_id = ReadSingleItem(filepath, nodeItor.Current, "art_id");
                art_text = ReadSingleItem(filepath, nodeItor.Current, "art_title");
                art_content = ReadSingleItem(filepath, nodeItor.Current, "art_content");

                if (trimValues)
                {
                    art_id = art_id.Trim();
                    art_text = art_text.Trim();
                    art_content = art_content.Trim();
                }

                art_text = art_text + " " + art_content;
                
                // Checking if ID is already in Artifacts List
                if (!artifacts.ContainsKey(art_id))
                {
                    TLArtifact artifact = new TLArtifact(art_id, art_text);
                    artifacts.Add(art_id, artifact);
                }
                else
                {
                    PoirotFormatArtifactsReader.Logger.Warn(
                        String.Format("Repeated artifact ID '{0}' found in file '{1}'.", art_id, filepath)
                        );
                }
            }

            return artifacts;
        }

        /// <summary>
        /// Reads one xml element from the given node according to the given expression
        /// </summary>
        private static string ReadSingleItem(string filepath, XPathNavigator xpNav, string exp)
        {
            XPathNavigator elemNav = xpNav.SelectSingleNode(exp);
            if (elemNav != null)
            {
                return elemNav.InnerXml;
            }
            else
            {
                throw new XmlException(String.Format("The format of the given file {0} is not correct. The xml node {1} has not been found in the file.", filepath, exp));
            }
        }
    }
}

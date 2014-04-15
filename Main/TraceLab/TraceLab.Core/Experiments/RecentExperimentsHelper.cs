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

namespace TraceLab.Core.Experiments
{
    public class RecentExperimentsHelper
    {
        public static void UpdateRecentExperimentList(string pListLocation, string pExperimentFile)
        {
            RecentExperimentList list = LoadRecentExperimentListFromXML(pListLocation);

            RecentExperimentReference newRef = new RecentExperimentReference(pExperimentFile);

            if (list.Contains(newRef))
            {
                list.Remove(newRef);
            }
            list.Add(newRef);

            SaveRecentExperimentListToXML(list, pListLocation);
        }

        public static RecentExperimentList LoadRecentExperimentListFromXML(string pFilepath)
        {
            RecentExperimentList list = new RecentExperimentList();

            if (System.IO.File.Exists(pFilepath))
            {
                try
                {
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(pFilepath);
                    System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

                    System.Xml.XPath.XPathNodeIterator itemIterator = nav.Select("/RecentExperiments/RecentExperimentItem");
                    int numItems = itemIterator.Count;

                    string fullpath, time;
                    while (itemIterator.MoveNext())
                    {
                        fullpath = itemIterator.Current.GetAttribute("FullPath", String.Empty);
                        time = itemIterator.Current.GetAttribute("LastAccessTime", String.Empty);

                        RecentExperimentReference item = RecentExperimentReference.CreateRecentExperimentItem(fullpath, time);
                        if (item != null)
                        {
                            list.AddLast(item);
                        }
                    }

                    if (list.Count != numItems)
                    {
                        RecentExperimentsHelper.SaveRecentExperimentListToXML(list, pFilepath);
                    }
                }
                catch (XmlException)
                {
                    System.IO.File.Delete(pFilepath);
                    list.Clear();
                }
            }

            return list;
        }

        public static void SaveRecentExperimentListToXML(RecentExperimentList pList, string pFilepath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.CheckCharacters = true;

            // Create file
            using (System.Xml.XmlWriter writer = XmlWriter.Create(pFilepath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RecentExperiments");

                foreach (var item in pList)
                {
                    writer.WriteStartElement("RecentExperimentItem");
                    writer.WriteAttributeString("FullPath", item.FullPath);
                    //writer.WriteAttributeString("Filename", item.Filename);
                    writer.WriteAttributeString("LastAccessTime", item.LastAccessTime.ToString());
                    writer.WriteEndElement(); // RecentExperimentItem
                }

                writer.WriteEndElement(); // RecentExperiments
                writer.WriteEndDocument();
                writer.Close();
            }
        }
    }
}

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
using System.Xml;
using System.Xml.XPath;

namespace TraceLab.Core.ViewModels
{
    /// <summary>
    /// Data structure for holding links used in the toolbar and start page
    /// </summary>
    public class WebsiteLink
    {
        #region Members

        /// <summary>
        /// Title of the link
        /// </summary>
        private string m_title;
        public string Title
        {
            get { return this.m_title; }
        }

        /// <summary>
        /// Description of the link
        /// </summary>
        private string m_description;
        public string Description
        {
            get { return this.m_description; }
        }

        /// <summary>
        /// URL of the link
        /// </summary>
        private string m_linkURL;
        public string LinkURL
        {
            get { return this.m_linkURL; }
        }

        #endregion

        #region Methods

        public WebsiteLink(string pTitle, string pDescription, string pLinkURL)
        {
            this.m_title = pTitle;
            this.m_description = pDescription;
            this.m_linkURL = pLinkURL;
        }

        public void OpenLink()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(this.LinkURL));
        }

        #endregion

        #region XML

        /// <summary>
        /// Loads links from an XML file
        /// </summary>
        /// <param name="filepath">The path of the XML file.</param>
        public static void LoadLinksFromXML(string pFilepath, out List<WebsiteLink> pVideos, out List<WebsiteLink> pLinks)
        {
            XPathDocument doc;
            if (pFilepath == null)
            {
                doc = new XPathDocument(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TraceLab.Core.Resources.OnlineContent.xml"));
            }
            else
            {
                doc = new XPathDocument(pFilepath);
            }
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator linkIterator = nav.Select("/StartPage/Videos/VideoItem");
            pVideos = new List<WebsiteLink>(linkIterator.Count);
            string title, description, url;
            while (linkIterator.MoveNext())
            {
                title = linkIterator.Current.GetAttribute("Title", String.Empty);
                description = linkIterator.Current.GetAttribute("Description", String.Empty);
                url = linkIterator.Current.GetAttribute("URL", String.Empty);
                pVideos.Add(new WebsiteLink(title, description, url));
            }

            linkIterator = nav.Select("/StartPage/Links/LinkItem");
            pLinks = new List<WebsiteLink>(linkIterator.Count);
            while (linkIterator.MoveNext())
            {
                title = linkIterator.Current.GetAttribute("Title", String.Empty);
                description = linkIterator.Current.GetAttribute("Description", String.Empty);
                url = linkIterator.Current.GetAttribute("URL", String.Empty);
                pLinks.Add(new WebsiteLink(title, description, url));
            }
        }

        /// <summary>
        /// Saves links to an XML file.
        /// </summary>
        /// <param name="filepath">The path of the XML file.</param>
        private static void SaveLinksToXML(string filepath, List<WebsiteLink> pVideos, List<WebsiteLink> pLinks)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.CheckCharacters = true;

            // Create file
            using (System.Xml.XmlWriter writer = XmlWriter.Create(filepath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("OnlineContent");

                writer.WriteStartElement("Videos");
                if (pVideos != null)
                {
                    foreach (var item in pVideos)
                    {
                        writer.WriteStartElement("VideoItem");
                        writer.WriteAttributeString("Title", item.Title);
                        writer.WriteAttributeString("Description", item.Description);
                        writer.WriteAttributeString("URL", item.LinkURL);
                        writer.WriteEndElement(); // VideoItem
                    }
                }
                writer.WriteEndElement(); // Videos

                writer.WriteStartElement("Links");
                if (pLinks != null)
                {
                    foreach (var item in pLinks)
                    {
                        writer.WriteStartElement("LinkItem");
                        writer.WriteAttributeString("Title", item.Title);
                        writer.WriteAttributeString("Description", item.Description);
                        writer.WriteAttributeString("URL", item.LinkURL);
                        writer.WriteEndElement(); // LinkItem
                    }
                }
                writer.WriteEndElement(); // Links

                writer.WriteEndElement(); // StartPage
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        #endregion
    }
}
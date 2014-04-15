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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Globalization;

namespace TraceLabSDK.Component.Config
{
    /// <summary>
    /// Used to represent a DirectoryPath in a Configuration object
    /// 
    /// Any DirectoryPaths will be displayed with a special editor when displaying the configuration of a component,
    /// that will allow selecting the directory.
    /// </summary>
    [Serializable]
    [XmlRoot("DirectoryPath")]
    public class DirectoryPath : BasePath
    {
        private const int CurrentVersion = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPath"/> class.
        /// </summary>
        public DirectoryPath() { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="TraceLabSDK.Component.Config.FilePath"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(DirectoryPath path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return path.Absolute;
        }

        #region IXmlSerializable Members

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (reader.LocalName == "DirectoryPath")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/DirectoryPath/Version");
                if (iter != null)
                {
                    long ver = iter.ValueAsLong;

                    if (ver == CurrentVersion)
                    {
                        ReadCurrentVersion(nav);
                    }
                    else
                    {
                        throw new NotSupportedException("Version not supported.");
                    }
                }
                else
                {
                    throw new NotSupportedException("Could not read version of DirectoryPath. ");
                }
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("/DirectoryPath/Relative");
            if (iter == null)
                throw new XmlSchemaException("DirectoryPath elements do not match the required elements for this version.");

            Relative = iter.Value;
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteElementString("Version", CurrentVersion.ToString(CultureInfo.CurrentCulture));
            writer.WriteElementString("Relative", Relative);
        }

        #endregion
    }
}

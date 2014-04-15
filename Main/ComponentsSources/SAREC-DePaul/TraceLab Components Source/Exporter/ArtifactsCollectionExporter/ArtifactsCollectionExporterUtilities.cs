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
using TraceLabSDK.Types;

namespace Exporter.ArtifactsCollectionExporter
{
    public class ArtifactsCollectionExporterUtilities
    {
        public static void Export(TLArtifactsCollection artifactsCollection, string outputPath, string collectionId, string name, string version, string description)
        {
            if (artifactsCollection == null)
            {
                throw new TraceLabSDK.ComponentException("Received null artifacts collection.");
            }

            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.CheckCharacters = true;

            //create file
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputPath, settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("artifacts_collection");

                WriteCollectionInfo(writer, collectionId, name, version, description);

                WriteArtifacts(artifactsCollection, writer);

                writer.WriteEndElement(); //artifacts_collection

                writer.WriteEndDocument();

                writer.Close();
            }

            System.Diagnostics.Trace.WriteLine("File created , you can find the file " + outputPath);
        }

        private static void WriteCollectionInfo(System.Xml.XmlWriter writer, string collectionId, string name, string version, string description)
        {
            writer.WriteStartElement("collection_info");

            writer.WriteElementString("id", collectionId.Trim());
            writer.WriteElementString("name", name.Trim());
            writer.WriteElementString("version", version.Trim());
            writer.WriteElementString("description", description.Trim());

            writer.WriteEndElement();
        }

        private static void WriteArtifacts(TLArtifactsCollection artifactsCollection, System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("artifacts");

            foreach (KeyValuePair<string, TLArtifact> artifact in artifactsCollection)
            {
                writer.WriteStartElement("artifact");

                writer.WriteElementString("id", artifact.Value.Id.Trim());
                writer.WriteElementString("content", artifact.Value.Text.Trim());
                writer.WriteElementString("parent_id", String.Empty);

                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // artifacts
        }
    }
}

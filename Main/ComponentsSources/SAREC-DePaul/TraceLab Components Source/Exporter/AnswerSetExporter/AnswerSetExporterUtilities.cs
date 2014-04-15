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

namespace Exporter.AnswerSetExporter
{
    public class AnswerSetExporterUtilities
    {
        public static void Export(TLSimilarityMatrix answerSet, string sourceId, string targetId, string outputPath)
        {
            if (answerSet == null)
            {
                throw new TraceLabSDK.ComponentException("Received null answer similarity matrix");
            }

            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.CheckCharacters = true;

            //create file
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputPath, settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("answer_set");

                WriteAnswerSetInfo(writer, sourceId, targetId);

                WriteLinks(answerSet, writer);

                writer.WriteEndElement(); //answer_set

                writer.WriteEndDocument();

                writer.Close();
            }

            System.Diagnostics.Trace.WriteLine("File created , you can find the file " + outputPath);
        }

        private static void WriteAnswerSetInfo(System.Xml.XmlWriter writer, string sourceId, string targetId)
        {
            writer.WriteStartElement("answer_info");

            writer.WriteElementString("source_artifacts_collection", sourceId.Trim());
            writer.WriteElementString("target_artifacts_collection", targetId.Trim());

            writer.WriteEndElement();
        }

        private static void WriteLinks(TLSimilarityMatrix answerSet, System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("links");

            foreach (TLSingleLink link in answerSet.AllLinks)
            {
                writer.WriteStartElement("link");

                writer.WriteElementString("source_artifact_id", link.SourceArtifactId.Trim());
                writer.WriteElementString("target_artifact_id", link.TargetArtifactId.Trim());
                writer.WriteElementString("confidence_score", link.Score.ToString().Trim());

                writer.WriteEndElement();
            }

            writer.WriteEndElement(); // artifacts
        }
    }
}

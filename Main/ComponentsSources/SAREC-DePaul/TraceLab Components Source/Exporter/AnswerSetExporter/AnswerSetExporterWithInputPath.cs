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
using TraceLabSDK;
using TraceLabSDK.Types;

namespace Exporter.AnswerSetExporter
{
    [Component(GuidIDString = "fc7a2699-1654-4368-a370-b7f5225f13aa",
        Name = "Answer Set Exporter to XML With Input config",
        DefaultLabel = "Answer Set CoEST XML Exporter (with path as input)",
        Description = "The components exports the similarity matrix answer set to the xml file in standard CoEST format.",
        Author = "DePaul RE Team",
        Version = "1.0")]
    [IOSpec(IOSpecType.Input, "answerSet", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "sourceArtifactsCollectionId", typeof(System.String))]
    [IOSpec(IOSpecType.Input, "targetArtifactsCollectionId", typeof(System.String))]
    [IOSpec(IOSpecType.Input, "outputPath", typeof(System.String))]
    [Tag("Exporters.TLSimilarityMatrix.To XML")]
    public class AnswerSetExporterWithInputPath : BaseComponent
    {
        public AnswerSetExporterWithInputPath(ComponentLogger log)
            : base(log)
        {
        }

        public override void Compute()
        {
            TLSimilarityMatrix answerSet = (TLSimilarityMatrix)Workspace.Load("answerSet");

            string sourceArtifactsCollectionId = (string)Workspace.Load("sourceArtifactsCollectionId");
            string targetArtifactsCollectionId = (string)Workspace.Load("targetArtifactsCollectionId");
            string path = (string)Workspace.Load("outputPath");
            path = path.Remove(path.IndexOf(".") + 1);
            path += "xml";

            AnswerSetExporterUtilities.Export(answerSet, sourceArtifactsCollectionId, targetArtifactsCollectionId, path);
        }
    }
}

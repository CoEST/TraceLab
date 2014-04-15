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
using System.IO;

namespace Exporter.AnswerSetExporter
{
    [Component(GuidIDString = "56b9771d-1a42-4890-9451-f7cab3fdd22e",
        Name = "Answer Set Exporter to XML",
        DefaultLabel = "Answer Set CoEST XML Exporter",
        Description = "The components exports the similarity matrix answer set to the xml file in standard CoEST format.",
        Author = "DePaul RE Team",
        Version = "1.0",
        ConfigurationType = typeof(AnswerSetExporterConfig))]
    [IOSpec(IOSpecType.Input, "answerSet", typeof(TLSimilarityMatrix))]
    [Tag("Exporters.TLSimilarityMatrix.To XML")]
    public class AnswerSetExporter : BaseComponent
    {
        private AnswerSetExporterConfig Config;

        public AnswerSetExporter(ComponentLogger log)
            : base(log)
        {
            Config = new AnswerSetExporterConfig();
            Configuration = Config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix answerSet = (TLSimilarityMatrix)Workspace.Load("answerSet");

            AnswerSetExporterUtilities.Export(answerSet, Config.SourceArtifactsCollectionId, Config.TargetArtifactsCollectionId, Config.Path);
        }
    }
}

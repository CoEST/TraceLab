using System.Collections.Generic;
using SEMERU.Core.IO;
using SEMERU.Types.Custom;
using SEMERU.Types.Dataset;
using TraceLabSDK;
using TraceLabSDK.Types;

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

namespace SEMERU.Custom.ICSM2011.Components
{
    [Component(Name = "SEMERU - ICSM'11 - (Loop) Artifacts Importer",
               Description = "Imports artifacts in a loop design.",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Input, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Output, "SourceArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "TargetArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "Oracle", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "RTM_NumTopics", typeof(int))]
    [IOSpec(IOSpecType.Output, "RTM_Alpha", typeof(double))]
    [IOSpec(IOSpecType.Output, "RTM_Beta", typeof(double))]
    [IOSpec(IOSpecType.Output, "RTM_Eta", typeof(double))]
    [IOSpec(IOSpecType.Output, "VSM_JS_lambda", typeof(double))]
    [IOSpec(IOSpecType.Output, "VSM_RTM_lambda", typeof(double))]
    [IOSpec(IOSpecType.Output, "JS_RTM_lambda", typeof(double))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class LoopArtifactsImporter : BaseComponent
    {
        public LoopArtifactsImporter(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            int curr = ((int)Workspace.Load("CurrentDataset")) - 1;
            List<ICSM11DataSet> list = (List<ICSM11DataSet>)Workspace.Load("ListOfDatasets");
            ICSM11DataSet dataset = list[curr];
            string[] sourceInfo = dataset.SourceArtifacts.Split(new char[] { '#' });
            string[] targetInfo = dataset.TargetArtifacts.Split(new char[] { '#' });
            TLArtifactsCollection targetArtifacts = Artifacts.ImportDirectory(targetInfo[0], targetInfo[1]);
            TLArtifactsCollection sourceArtifacts = Artifacts.ImportDirectory(sourceInfo[0], sourceInfo[1]);
            Workspace.Store("SourceArtifacts", sourceArtifacts);
            Workspace.Store("TargetArtifacts", targetArtifacts);
            TLSimilarityMatrix answer = Oracle.Import(dataset.Oracle);
            Workspace.Store("Oracle", answer);
            Workspace.Store("RTM_NumTopics", list[curr].RTM.NumTopics);
            Workspace.Store("RTM_Alpha", list[curr].RTM.Alpha);
            Workspace.Store("RTM_Beta", list[curr].RTM.Beta);
            Workspace.Store("RTM_Eta", list[curr].RTM.Eta);
            Workspace.Store("VSM_JS_lambda", list[curr].PCA.VSM_JS);
            Workspace.Store("VSM_RTM_lambda", list[curr].PCA.VSM_RTM);
            Workspace.Store("JS_RTM_lambda", list[curr].PCA.JS_RTM);
        }
    }
}
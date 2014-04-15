using System.Collections.Generic;
using SEMERU.Core.IO;
using SEMERU.Types.Custom;
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

namespace SEMERU.Custom.CSMR2012.Components
{
    [Component(Name = "SEMERU - CSMR'13 - Loop Artifacts Importer",
        Description = "Imports artifacts in a loop for the CSMR'13 experiment.",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<CSMR13DataSet>))]
    [IOSpec(IOSpecType.Input, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Output, "SourceArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "TargetArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "Oracle", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "Relationships", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "Stopwords", typeof(TLStopwords))]
    [Tag("SEMERU.Custom.CSMR'13")]
    public class ExperimentalLoopArtifactsImporter : BaseComponent
    {
        public ExperimentalLoopArtifactsImporter(ComponentLogger log)
            : base(log)
        {

        }

        public override void Compute()
        {
            List<CSMR13DataSet> ListOfDatasets = (List<CSMR13DataSet>)Workspace.Load("ListOfDatasets");
            CSMR13DataSet CurrentDataset = ListOfDatasets[(int)Workspace.Load("CurrentDataset")];
            
            // import artifacts
            string[] sourceInfo = CurrentDataset.SourceArtifacts.Split(new char[] { '#' });
            string[] targetInfo = CurrentDataset.TargetArtifacts.Split(new char[] { '#' });
            Workspace.Store("SourceArtifacts", Artifacts.ImportDirectory(sourceInfo[0], sourceInfo[1]));
            Workspace.Store("TargetArtifacts", Artifacts.ImportDirectory(targetInfo[0], targetInfo[1]));

            // import oracle
            Workspace.Store("Oracle", Oracle.Import(CurrentDataset.Oracle));

            // import relationships
            Workspace.Store("Relationships", Oracle.Import(CurrentDataset.Relationships));

            // import stopwords
            Workspace.Store("Stopwords", Stopwords.Import(CurrentDataset.Stopwords));
        }
    }
}

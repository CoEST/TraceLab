using System.Collections.Generic;
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

namespace SEMERU.Custom.CSMR2012.Components
{
    [Component(Name = "SEMERU - CSMR'13 - Results retrieval",
        Description = "Retrieves the results from the CSMR'13 experiment.",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<CSMR13DataSet>))]
    [IOSpec(IOSpecType.Input, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Input, "VSM_Metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "VSM_Similarities", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "VSM_OCSTI_Metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "VSM_OCSTI", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "VSM_UDCSTI_Metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "VSM_UDCSTI", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "JS_Metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_Similarities", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "JS_OCSTI_Metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_OCSTI", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "JS_UDCSTI_Metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_UDCSTI", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "ListOfDatasets", typeof(List<CSMR13DataSet>))]
    [Tag("SEMERU.Custom.CSMR'13")]
    public class ResultsRetrievalComponent : BaseComponent
    {
        public ResultsRetrievalComponent(ComponentLogger log)
            : base(log)
        {

        }

        public override void Compute()
        {
            List<CSMR13DataSet> ListOfDatasets = (List<CSMR13DataSet>)Workspace.Load("ListOfDatasets");
            int CurrentDataset = (int)Workspace.Load("CurrentDataset");
            ListOfDatasets[CurrentDataset].Metrics = new DataSetPairsCollection();
            ListOfDatasets[CurrentDataset].Similarities = new List<TLSimilarityMatrix>();
            string[] IRmodels = new string[] { "VSM", "JS" };
            string[] StructuralModels = new string[] { "OCSTI", "UDCSTI" };
            foreach (string IRmodel in IRmodels)
            {
                TLSimilarityMatrix baseSims = (TLSimilarityMatrix)Workspace.Load(IRmodel + "_Similarities");
                baseSims.Name = IRmodel;
                ListOfDatasets[CurrentDataset].Similarities.Add(baseSims);
                foreach (DataSetPairs basePairs in (DataSetPairsCollection)Workspace.Load(IRmodel + "_Metrics"))
                {
                    basePairs.Name = IRmodel + " @" + basePairs.Name;
                    ListOfDatasets[CurrentDataset].Metrics.Add(basePairs);
                }
                foreach (string StructuralModel in StructuralModels)
                {
                    TLSimilarityMatrix structuralSims = (TLSimilarityMatrix)Workspace.Load(IRmodel + "_" + StructuralModel);
                    structuralSims.Name = IRmodel + "_" + StructuralModel;
                    ListOfDatasets[CurrentDataset].Similarities.Add(structuralSims);
                    foreach (DataSetPairs structuralPairs in (DataSetPairsCollection)Workspace.Load(IRmodel + "_" + StructuralModel + "_Metrics"))
                    {
                        structuralPairs.Name = IRmodel + " " + StructuralModel + " @" + structuralPairs.Name;
                        ListOfDatasets[CurrentDataset].Metrics.Add(structuralPairs);
                    }
                }
            }
            Workspace.Store("ListOfDatasets", ListOfDatasets);
        }
    }
}

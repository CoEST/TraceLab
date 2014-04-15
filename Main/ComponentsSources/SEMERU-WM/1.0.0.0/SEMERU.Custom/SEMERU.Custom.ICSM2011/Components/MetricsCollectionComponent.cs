using SEMERU.Types.Dataset;
using TraceLabSDK;
using TraceLabSDK.Types;
using System.Collections.Generic;
using SEMERU.Types.Custom;

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
    [Component(Name = "SEMERU - ICSM'11 - Metrics Collection",
               Description = "Collects the computed metrics and stores them in the associated dataset.",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Input, "VSM_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "JS_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "RTM_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "VSM_JS_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "VSM_JS_PCA_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "VSM_RTM_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "VSM_RTM_PCA_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "JS_RTM_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Input, "JS_RTM_PCA_Metrics", typeof(DataSetPairs))]
    [IOSpec(IOSpecType.Output, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class MetricsCollectionComponent : BaseComponent
    {
        public MetricsCollectionComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            string[] models = { "VSM", "JS", "RTM" };
            int CurrentDataset = (int)Workspace.Load("CurrentDataset") - 1;
            List<ICSM11DataSet> Datasets = (List<ICSM11DataSet>)Workspace.Load("ListOfDatasets");
            Datasets[CurrentDataset].Metrics = new DataSetPairsCollection();

            // loop over every IR pair
            DataSetPairs pair;
            for (int i = 0; i < models.Length; i++)
            {
                // base IR method
                pair = (DataSetPairs)Workspace.Load(models[i] + "_Metrics");
                pair.Name = models[i];
                Datasets[CurrentDataset].Metrics.Add(pair);
                // combos
                for (int j = i + 1; j < models.Length; j++)
                {
                    // lambda = 0.5
                    pair = (DataSetPairs)Workspace.Load(models[i] + "_" + models[j] + "_Metrics");
                    pair.Name = models[i] + "+" + models[j];
                    Datasets[CurrentDataset].Metrics.Add(pair);
                    // PCA lambda
                    pair = (DataSetPairs)Workspace.Load(models[i] + "_" + models[j] + "_PCA_Metrics");
                    pair.Name = models[i] + "+" + models[j] + "(PCA)";
                    Datasets[CurrentDataset].Metrics.Add(pair);
                }
            }

            Workspace.Store("ListOfDatasets", Datasets);
        }
    }
}
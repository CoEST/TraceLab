using System;
using TraceLabSDK;
using SEMERU.Types.Dataset;
using System.Collections.Generic;

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

namespace SEMERU.Custom.TSE2013
{
    [Component(Name = "SEMERU - TSE'13 - Metrics Collection",
                Description = "Collects the metrics from the experiment and converts them into the proper format for viewing.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "VSM_raw_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "VSM_noun_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "VSM_smooth_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "VSM_both_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_raw_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_noun_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_smooth_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Input, "JS_both_metrics", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Output, "ListOfDatasets", typeof(List<DataSet>))]
    [Tag("SEMERU.Custom.TSE'13")]
    public class MetricsCollectionComponent : BaseComponent
    {
        public MetricsCollectionComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            string[] models = new string[] { "VSM", "JS" };
            string[] processes = new string[] { "raw", "noun", "smooth", "both" };

            List<DataSet> list = new List<DataSet>();
            DataSet ds = new DataSet
            {
                Name = "Dataset",
                Metrics = new DataSetPairsCollection(),
                SourceArtifacts = "",
                TargetArtifacts = "",
                Oracle = "",
            };

            foreach (string model in models)
            {
                foreach (string process in processes)
                {
                    foreach (DataSetPairs dsp in (DataSetPairsCollection)Workspace.Load(model + "_" + process + "_metrics"))
                    {
                        dsp.Name = model + " (" + process + ")@" + dsp.Name;
                        ds.Metrics.Add(dsp);
                    }
                }
            }

            list.Add(ds);
            Workspace.Store("ListOfDatasets", list);
        }
    }
}
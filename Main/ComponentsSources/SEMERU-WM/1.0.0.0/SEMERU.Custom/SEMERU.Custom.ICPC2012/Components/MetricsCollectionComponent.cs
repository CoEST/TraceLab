using System;
using TraceLabSDK;
using SEMERU.Types.Metrics;
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

namespace SEMERU.Custom.ICPC2012.Components
{
    [Component(Name = "SEMERU - ICPC'12 - Metrics Collection",
                Description = "",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "VSM_Metrics", typeof(DatasetResults))]
    [IOSpec(IOSpecType.Input, "VSMTrace_Metrics", typeof(DatasetResults))]
    [IOSpec(IOSpecType.Input, "LSI_Metrics", typeof(DatasetResults))]
    [IOSpec(IOSpecType.Input, "LSITrace_Metrics", typeof(DatasetResults))]
    [IOSpec(IOSpecType.Output, "DatasetResults", typeof(List<DatasetResults>))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class MetricsCollectionComponent : BaseComponent
    {
        public MetricsCollectionComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            string[] models = new string[] { "VSM", "LSI" };
            List<Metric> metrics = new List<Metric>();
            foreach (string model in models)
            {
                foreach (Metric metric in ((DatasetResults)Workspace.Load(model + "_Metrics")).Metrics)
                {
                    metrics.Add(metric);
                }
                foreach (Metric metric in ((DatasetResults)Workspace.Load(model + "Trace_Metrics")).Metrics)
                {
                    metrics.Add(metric);
                }
            }
            List<DatasetResults> results = new List<DatasetResults>();
            results.Add(new DatasetResults("jEdit4.3", metrics));
            Workspace.Store("DatasetResults", results);
        }
    }
}
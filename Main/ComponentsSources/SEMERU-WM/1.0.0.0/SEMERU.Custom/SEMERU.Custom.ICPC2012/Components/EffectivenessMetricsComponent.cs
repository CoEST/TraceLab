using System;
using TraceLabSDK;
using SEMERU.Types.Metrics;
using TraceLabSDK.Types;
using System.Collections.Generic;
using SEMERU.ICPC12.Exporters;

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
    [Component(Name = "SEMERU - ICPC'12 - Effectiveness Metrics",
                Description = "Computes Best and All effectiveness measures for a similarity ranked-list",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType = typeof(EffectivenessMetricsConfig))]
    [IOSpec(IOSpecType.Input, "Similarities", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "GoldSet", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "QueryMap", typeof(Dictionary<int, string>))]
    [IOSpec(IOSpecType.Output, "EffectivenessMeasures", typeof(DatasetResults))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class EffectivenessMetricsComponent : BaseComponent
    {
        private EffectivenessMetricsConfig _config;

        public EffectivenessMetricsComponent(ComponentLogger log)
            : base(log)
        {
            _config = new EffectivenessMetricsConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix sims = (TLSimilarityMatrix)Workspace.Load("Similarities");
            TLSimilarityMatrix goldset = (TLSimilarityMatrix)Workspace.Load("GoldSet");
            Dictionary<int, string> qmap = (Dictionary<int, string>)Workspace.Load("QueryMap");
            Workspace.Store("EffectivenessMeasures", Effectiveness.Calculate(ref sims, ref goldset, qmap, _config.ModelName));
        }
    }

    public class EffectivenessMetricsConfig
    {
        public string ModelName { get; set; }
    }
}
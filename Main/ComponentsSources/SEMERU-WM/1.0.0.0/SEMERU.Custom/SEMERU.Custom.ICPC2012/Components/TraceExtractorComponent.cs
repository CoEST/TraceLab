using SEMERU.ICPC12.Tools;
using TraceLabSDK;
using TraceLabSDK.Types;
using System.Collections.Generic;
using TraceLabSDK.Component.Config;

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

namespace SEMERU.ICPC12.Components
{
    [Component(Name = "SEMERU - ICPC'12 - Trace Extractor",
                Description = "Removes non-executed methods for each source artifact in a TLSimilarityMatrix.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(TraceExtractorConfig))]
    [IOSpec(IOSpecType.Input, "Similarities", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "QueryMap", typeof(Dictionary<int, string>))]
    [IOSpec(IOSpecType.Output, "Similarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class TraceExtractorComponent : BaseComponent
    {
        private TraceExtractorConfig _config;

        public TraceExtractorComponent(ComponentLogger log)
            : base(log)
        {
            _config = new TraceExtractorConfig();
            Configuration = _config;
        }

        public TraceExtractorComponent(ComponentLogger log, TraceExtractorConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix sims = (TLSimilarityMatrix)Workspace.Load("Similarities");
            Dictionary<int, string> qmap = (Dictionary<int, string>) Workspace.Load("QueryMap");
            TLSimilarityMatrix newsims = Trace.Extract(ref sims, _config.TraceDirectory.Absolute, FeatureSet.All, qmap);
            Workspace.Store("Similarities", newsims);
        }
    }

    public class TraceExtractorConfig
    {
        public DirectoryPath TraceDirectory { get; set; }
        //public FeatureSet Subset { get; set; }
    }
}
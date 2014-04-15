using System;
using TraceLabSDK;
using TraceLabSDK.Types;
using SEMERU.ICPC12.Importers;
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
    [Component(Name = "SEMERU - ICPC'12 - Similarities Importer",
                Description = "Imports pre-computed similarities.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(SimilaritiesImporterConfig))]
    [IOSpec(IOSpecType.Input, "DocumentMap", typeof(List<string>))]
    [IOSpec(IOSpecType.Output, "Similarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class SimilaritiesImporter : BaseComponent
    {
        private SimilaritiesImporterConfig _config;

        public SimilaritiesImporter(ComponentLogger log)
            : base(log)
        {
            _config = new SimilaritiesImporterConfig();
            Configuration = _config;
        }

        public SimilaritiesImporter(ComponentLogger log, SimilaritiesImporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            List<string> map = (List<string>)Workspace.Load("DocumentMap");
            TLSimilarityMatrix sims = Similarities.Import(_config.Directory.Absolute, map);
            Workspace.Store("Similarities", sims);
        }
    }

    public class SimilaritiesImporterConfig
    {
        public DirectoryPath Directory { get; set; }
    }
}
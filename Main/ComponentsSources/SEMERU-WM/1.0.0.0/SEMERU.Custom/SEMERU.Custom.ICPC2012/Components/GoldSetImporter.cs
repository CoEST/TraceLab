using System;
using TraceLabSDK;
using TraceLabSDK.Types;
using SEMERU.ICPC12.Importers;
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
    [Component(Name = "SEMERU - ICPC'12 - Gold Set Importer",
                Description = "Imports a gold set.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(GoldSetImporterConfig))]
    [IOSpec(IOSpecType.Output, "GoldSet", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class GoldSetImporter : BaseComponent
    {
        private GoldSetImporterConfig _config;

        public GoldSetImporter(ComponentLogger log)
            : base(log)
        {
            _config = new GoldSetImporterConfig();
            Configuration = _config;
        }

        public GoldSetImporter(ComponentLogger log, GoldSetImporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            Workspace.Store("GoldSet", GoldSet.Import(_config.Directory.Absolute));
        }
    }

    public class GoldSetImporterConfig
    {
        public DirectoryPath Directory { get; set; }
    }
}
using SEMERU.Core.IO;
using TraceLabSDK;
using TraceLabSDK.Component.Config;
using TraceLabSDK.Types;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Components
{
    [Component(Name = "SEMERU - Artifacts Importer",
                Description = "Imports corpus artifacts",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(ArtifactsImporterConfig))]
    [IOSpec(IOSpecType.Output, "Artifacts", typeof(TLArtifactsCollection))]
    [Tag("SEMERU.Importers")]
    [Tag("Importers.TLArtifactsCollection.From TXT")]
    public class ArtifactsImporter : BaseComponent
    {
        private ArtifactsImporterConfig _config;

        public ArtifactsImporter(ComponentLogger log) : base(log)
        {
            _config = new ArtifactsImporterConfig();
            Configuration = _config;
        }

        public ArtifactsImporter(ComponentLogger log, ArtifactsImporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            Workspace.Store("Artifacts", Artifacts.Import(_config.Artifacts.Absolute));
        }
    }

    public class ArtifactsImporterConfig
    {
        public FilePath Artifacts { get; set; }
    }
}
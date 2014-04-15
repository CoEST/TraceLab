using System.Collections.Generic;
using SEMERU.ICPC12.Importers;
using TraceLabSDK;
using TraceLabSDK.Types;
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
    [Component(Name = "SEMERU - ICPC'12 - Queries Importer",
                Description = "Imports queries.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(QueriesImporterConfig))]
    [IOSpec(IOSpecType.Output, "QueryMap", typeof(Dictionary<int, string>))]
    [IOSpec(IOSpecType.Output, "Queries", typeof(TLArtifactsCollection))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class QueriesImporter : BaseComponent
    {
        private QueriesImporterConfig _config;

        public QueriesImporter(ComponentLogger log)
            : base(log)
        {
            _config = new QueriesImporterConfig();
            Configuration = _config;
        }

        public QueriesImporter(ComponentLogger log, QueriesImporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            Workspace.Store("Queries", Queries.Import(_config.QueriesDirectory.Absolute));
            Workspace.Store("QueryMap", Queries.CreateMap(_config.BugsIDFile.Absolute, _config.FeaturesIDFile.Absolute, _config.PatchesIDFile.Absolute));
        }
    }

    public class QueriesImporterConfig
    {
        public DirectoryPath QueriesDirectory { get; set; }
        public FilePath FeaturesIDFile { get; set; }
        public FilePath BugsIDFile { get; set; }
        public FilePath PatchesIDFile { get; set; }
    }
}
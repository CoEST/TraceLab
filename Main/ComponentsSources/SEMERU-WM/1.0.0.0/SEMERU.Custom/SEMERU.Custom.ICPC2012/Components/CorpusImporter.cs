using System.Collections.Generic;
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
    [Component(Name = "SEMERU - ICPC'12 - Corpus Importer",
                Description = "Imports a corpus.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(CorpusImporterConfig))]
    [IOSpec(IOSpecType.Output, "ListOfArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "DocumentMap", typeof(List<string>))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class CorpusImporter : BaseComponent
    {
        private CorpusImporterConfig _config;

        public CorpusImporter(ComponentLogger log)
            : base(log)
        {
            _config = new CorpusImporterConfig();
            Configuration = _config;
        }

        public CorpusImporter(ComponentLogger log, CorpusImporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            TLArtifactsCollection artifacts = Importers.Corpus.Import(_config.Identifiers.Absolute, _config.Documents.Absolute);
            List<string> map = Importers.Corpus.Map(_config.Identifiers.Absolute);
            Workspace.Store("ListOfArtifacts", artifacts);
            Workspace.Store("DocumentMap", map);
        }
    }

    public class CorpusImporterConfig
    {
        // doc1 - method identifiers
        public FilePath Identifiers { get; set; }
        // doc2 - BOW documents
        public FilePath Documents { get; set; }
    }
}
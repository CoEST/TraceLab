using System;
using System.ComponentModel;
using System.IO;
using TraceLabSDK;
using TraceLabSDK.Component.Config;
using TraceLabSDK.Types;
using SEMERU.Core.IO;

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
    [Component(Name = "SEMERU - Similarities Importer",
                Description = "Imports a TLSimilarityMatrix from file formatted by the SEMERU Similarities Exporter.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(SimilaritiesImporterConfig))]
    [IOSpec(IOSpecType.Output, "Similarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Importers")]
    [Tag("Importers.TLSimilarityMatrix.From TXT")]
    public class SimilaritiesImporter : BaseComponent
    {
        private SimilaritiesImporterConfig _config;

        public SimilaritiesImporter(ComponentLogger log)
            : base(log)
        {
            _config = new SimilaritiesImporterConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            Workspace.Store("Similarities", (TLSimilarityMatrix)Similarities.Import(_config.File.Absolute));
        }
    }

    public class SimilaritiesImporterConfig
    {
        [DisplayName("Similarities file")]
        [Description("Similarities file location")]
        public FilePath File { get; set; }
    }
}
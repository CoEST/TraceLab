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
    [Component(Name = "SEMERU - Similarities Exporter",
                Description = "Exports a TLSimilarityMatrix to the designated file.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(SimilaritiesExporterConfig))]
    [IOSpec(IOSpecType.Input, "Similarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Exporters")]
    [Tag("Exporters.TLSimilarityMatrix.To TXT")]
    public class SimilaritiesExporter : BaseComponent
    {
        private SimilaritiesExporterConfig _config;

        public SimilaritiesExporter(ComponentLogger log) : base(log)
        {
            _config = new SimilaritiesExporterConfig();
            Configuration = _config;
        }

        public SimilaritiesExporter(ComponentLogger log, SimilaritiesExporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            Logger.Trace("Writing similarities to " + _config.File.Absolute);
            Similarities.Export((TLSimilarityMatrix)Workspace.Load("Similarities"), _config.File.Absolute);
            Logger.Trace("Write complete.");
        }
    }

    public class SimilaritiesExporterConfig
    {
        [DisplayName("File location")]
        [Description("File to import")]
        public FilePath File { get; set; }
    }
}
using System.ComponentModel;
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
    [Component(Name = "SEMERU - Oracle Importer",
                Description = "Imports an answer matrix in the form of {SOURCE TARGET1, TARGET2, ...\\n}",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(OracleImporterConfig))]
    [IOSpec(IOSpecType.Output, "Oracle", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Importers")]
    [Tag("Importers.TLSimilarityMatrix.From TXT")]
    public class OracleImporter : BaseComponent
    {
        private OracleImporterConfig _config;

        public OracleImporter(ComponentLogger log) : base(log)
        {
            _config = new OracleImporterConfig();
            Configuration = _config;
        }

        public OracleImporter(ComponentLogger log, OracleImporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            Workspace.Store("Oracle", Oracle.Import(_config.OracleDocument.Absolute));

        }
    }

    public class OracleImporterConfig
    {
        [DisplayName("Oracle file")]
        [Description("Oracle file location")]
        public FilePath OracleDocument { get; set; }
    }
}
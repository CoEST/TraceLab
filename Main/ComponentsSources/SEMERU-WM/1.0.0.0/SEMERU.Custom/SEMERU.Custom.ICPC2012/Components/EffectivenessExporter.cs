using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SEMERU.ICPC12.Exporters;
using TraceLabSDK;
using TraceLabSDK.Component.Config;
using TraceLabSDK.Types;

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
    [Component(Name = "SEMERU - ICPC'12 - Effectiveness Exporter",
                Description = "Calculates and exports effectiveness measures to disk.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(EffectivenessExporterConfig))]
    [IOSpec(IOSpecType.Input, "Similarities", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "GoldSet", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "QueryMap", typeof(Dictionary<int, string>))]
    [Tag("SEMERU.Custom.ICPC'12")]
    public class EffectivenessExporter : BaseComponent
    {
        private EffectivenessExporterConfig _config;

        public EffectivenessExporter(ComponentLogger log)
            : base(log)
        {
            _config = new EffectivenessExporterConfig();
            Configuration = _config;
        }

        public EffectivenessExporter(ComponentLogger log, EffectivenessExporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix sims = (TLSimilarityMatrix)Workspace.Load("Similarities");
            TLSimilarityMatrix goldset = (TLSimilarityMatrix)Workspace.Load("GoldSet");
            Dictionary<int, string> qmap = (Dictionary<int, string>)Workspace.Load("QueryMap");
            Effectiveness.Export(ref sims, ref goldset, qmap, _config.Directory.Absolute, _config.Prefix);
        }
    }

    public class EffectivenessExporterConfig
    {
        public DirectoryPath Directory { get; set; }

        private string _prefix;
        public string Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                if (value == String.Empty)
                {
                    throw new ArgumentException("Prefix cannot be empty.");
                }
                _prefix = Regex.Replace(value, "[^a-zA-Z0-9]", "");
            }
        }
    }
}
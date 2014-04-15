using System.ComponentModel;
using System.IO;
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
    [Component(Name = "SEMERU - Artifacts Exporter",
                Description = "Exports a TLArtifactsCollection to a tab-delimited text file.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(ArtifactsExporterConfig))]
    [IOSpec(IOSpecType.Input, "Artifacts", typeof(TLArtifactsCollection))]
    [Tag("SEMERU.Exporters")]
    [Tag("Exporters.TLArtifactsCollection.To TXT")]
    public class ArtifactsExporter : BaseComponent
    {
        private ArtifactsExporterConfig _config;

        public ArtifactsExporter(ComponentLogger log) : base(log)
        {
            _config = new ArtifactsExporterConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLArtifactsCollection artifacts = (TLArtifactsCollection)Workspace.Load("Artifacts");
            TextWriter tw = new StreamWriter(_config.File.Absolute + ".txt");
            foreach (TLArtifact artifact in artifacts.Values)
            {
                tw.WriteLine(artifact.Id + "\t" + artifact.Text);
            }

            tw.Close();
        }
    }

    public class ArtifactsExporterConfig
    {
        [DisplayName("File location")]
        [Description("File name to export to")]
        public FilePath File { get; set; }
    }
}
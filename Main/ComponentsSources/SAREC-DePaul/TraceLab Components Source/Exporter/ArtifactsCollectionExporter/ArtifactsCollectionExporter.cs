// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK;
using TraceLabSDK.Types;
using System.IO;

namespace Exporter.ArtifactsCollectionExporter
{
    [Component(GuidIDString = "c55e7015-f7d4-41b8-a19c-5385a1fa16c6",
        Name = "Artifacts Collection Exporter to XML",
        DefaultLabel = "Artifacts Collection CoEST XML Exporter",
        Description = "The components exports the artifacts collection to the xml file in standard CoEST format.",
        Author = "DePaul RE Team",
        Version = "1.0",
        ConfigurationType = typeof(ArtifactsCollectionExporterConfig))]
    [IOSpec(IOSpecType.Input, "artifactsCollection", typeof(TLArtifactsCollection))]
    [Tag("Exporters.TLArtifactsCollection.To XML")]
    public class ArtifactsCollectionExporter : BaseComponent
    {
        private ArtifactsCollectionExporterConfig Config;

        public ArtifactsCollectionExporter(ComponentLogger log)
            : base(log)
        {
            Config = new ArtifactsCollectionExporterConfig();
            Configuration = Config;
        }

        public override void Compute()
        {
            TLArtifactsCollection artifactsCollection = (TLArtifactsCollection)Workspace.Load("artifactsCollection");

            ArtifactsCollectionExporterUtilities.Export(artifactsCollection, Config.Path, Config.CollectionId, Config.CollectionName, Config.CollectionVersion, Config.CollectionDescription);

            Logger.Info(String.Format("Artifacts Collection has been saved into xml file '{0}'", Config.Path.Absolute));
        }
    }
}

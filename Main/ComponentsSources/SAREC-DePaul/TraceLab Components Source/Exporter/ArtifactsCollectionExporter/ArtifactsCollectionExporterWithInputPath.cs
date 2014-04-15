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

namespace Exporter.ArtifactsCollectionExporter
{
    [Component(GuidIDString = "ffdac867-b8a8-4de5-b251-ca9258be371c",
                Name = "Artifacts Collection Exporter to XML With Input",
                DefaultLabel = "Artifacts Collection CoEST XML Exporter (with path as input)",
                Description = "The components exports the artifacts collection to the xml file in standard CoEST format. It returns generated guid for the artifacts type.",
                Author = "DePaul RE Team",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "artifactsCollection", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "outputPath", typeof(System.String))]
    [IOSpec(IOSpecType.Output, "collectionId", typeof(System.String))]
    [Tag("Exporters.TLArtifactsCollection.To XML")]
    public class ArtifactsCollectionExporterWithInputPath : BaseComponent
    {
        public ArtifactsCollectionExporterWithInputPath(ComponentLogger log)
            : base(log)
        {
        }

        public override void Compute()
        {
            TLArtifactsCollection artifactsCollection = (TLArtifactsCollection)Workspace.Load("artifactsCollection");

            string path, collectionName, id;
            
            path = (string)Workspace.Load("outputPath");
            if (path.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase))
            {
                collectionName = path.Remove(path.LastIndexOf(".") + 1);
            } 
            else 
            {
                collectionName = path;
                path = path + ".xml";
            }

            collectionName = collectionName.Substring(collectionName.LastIndexOf('\\') + 1);
            id = collectionName;
            
            ArtifactsCollectionExporterUtilities.Export(artifactsCollection, path, id, collectionName, "1.1", collectionName);

            Workspace.Store("collectionId", id);

            Logger.Info(String.Format("Artifacts Collection has been saved into xml file '{0}'", path));
        }
    }
}

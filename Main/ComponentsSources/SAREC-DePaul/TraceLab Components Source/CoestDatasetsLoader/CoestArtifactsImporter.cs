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
// Located in c:\Program Files (x86)\COEST\TraceLab\lib\TraceLabSDK.dll
using TraceLabSDK;
using TraceLabSDK.Types;
using System.ComponentModel;
using TraceLabSDK.Component.Config;

namespace CoestDatasetsLoader
{
    [Component(GuidIDString = "7f24cee1-6036-4e08-9904-c6f9c41356ff",
                Name = "CoestArtifactsImporter",
                DefaultLabel = "Coest Artifacts Collection Importer",
                Description = "Component imports artifact collection from xml Coest format. \n" +
                              "Sample files can be found in Package 'Importers Sample Files' in 'Coest Format' directory \n" +
                              "Coest format allows two variation for artifacts content location: \n" +
                              "1. External content location: 'Coest XML ArtifactsCollection Format External.xml' \n" +
                              "2. Internal content location: 'Coest XML ArtifactsCollection Format Internal.xml' \n", 
                Author = "ReLab",
                Version = "1.0", 
                ConfigurationType=typeof(ImporterConfig))]
    [IOSpec(IOSpecType.Output, "listOfArtifacts", typeof(TLArtifactsCollection), "The list that will contain imported artifacts")]
    [Tag("Importers.TLArtifactsCollection.From XML")]
    public class CoestArtifactsImporter : BaseComponent
    {
        public CoestArtifactsImporter(ComponentLogger log) : base(log) 
        {
            m_config = new ImporterConfig();
            Configuration = m_config;
        }

        private ImporterConfig m_config;

        public override void Compute()
        {
            string error;
            //do validation
            if (CoestDatasetImporterHelper.ValidatePath(m_config.FilePath, "Artifacts File", out error))
            {
                CoestDatasetImporterHelper.Logger = this.Logger;
                var artifacts = CoestDatasetImporterHelper.ImportArtifacts(m_config.FilePath, m_config.TrimElementValues);
                Workspace.Store("listOfArtifacts", artifacts);
                Logger.Info(String.Format("Collection of artifacts with id '{0}' imported from {1}.", artifacts.CollectionId, m_config.FilePath));
            }
            else
            {
                throw new ComponentException(error);
            }
        }
    }
}

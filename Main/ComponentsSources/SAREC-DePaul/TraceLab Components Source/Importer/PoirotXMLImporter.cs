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
using System.IO;
using TraceLabSDK.Types;

namespace Importer
{

    [Component(GuidIDString = "D98BD1E6-1DB5-11E0-BFA9-3EE4DFD72085", //GuidIDString is retained for backwards experiment that may refer to component by old guid
               Name = "PoirotXMLImporter", 
               DefaultLabel = "Poirot XML Importer",
               Author = "DePaul RE Lab Team", 
               Version = "1.0", 
               Description = "Imports list of artifacts from XML in Poirot format. \n" +
                             "Sample xml file can be found in Package 'Importers Sample Files', file 'Poirot XML Importer Format.xml'", 
               ConfigurationType=typeof(ImporterConfig))]
    [IOSpec(IOSpecType.Output, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [Tag("Importers.TLArtifactsCollection.From XML")]
    public class PoirotXMLImporter : BaseComponent
    {
        public PoirotXMLImporter(ComponentLogger log) : base(log)
        {
            this.Configuration = new ImporterConfig();
        }

        public override void Compute()
        {
            ImporterConfig config = (ImporterConfig)this.Configuration;
            if (config.Path == null)
            {
                throw new ComponentException("Path has not been specified.");
            }
            if (!File.Exists(config.Path))
            {
                throw new ComponentException(String.Format("File does not exist '{0}'.", config.Path.Absolute));
            }

            PoirotFormatArtifactsReader.Logger = this.Logger;
            TLArtifactsCollection targetArtifacts = PoirotFormatArtifactsReader.ReadXMLFile(config.Path, config.TrimElementValues);
            Workspace.Store("listOfArtifacts", targetArtifacts);

            Logger.Info("Artifacts has been imported from " + config.Path);
        }
    }
}

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

namespace Importer
{
    [Component(GuidIDString = "B450DC72-1DB6-11E0-BB91-FBE4DFD72085", //GuidIDString is retained for backwards experiment that may refer to component by old guid
           Name = "StopwordsImporter",
           DefaultLabel = "Stopwords Importer",
           Author = "DePaul RE Lab Team",
           Version = "1.0",
           Description = "Imports list of stopwords from text file. Each stopword should be in new line. \n" +
                         "Sample file can be found in Package 'Importers Sample Files', file 'Stopwords Importer Format.txt'",
           ConfigurationType = typeof(StopwordsImporterConfig))]
    [IOSpec(IOSpecType.Output, "stopwords", typeof(TraceLabSDK.Types.TLStopwords))]
    [Tag("Importers.TLStopwords.From TXT")]
    public class StopwordsImporter : BaseComponent
    {
        public StopwordsImporter(ComponentLogger log) : base(log)
        {
            this.Configuration = new StopwordsImporterConfig();
        }

        public override void Compute()
        {
            StopwordsImporterConfig config = (StopwordsImporterConfig)this.Configuration;
            if (config.Path == null)
            {
                throw new ComponentException("Path has not been specified.");
            }
            if (!File.Exists(config.Path))
            {
                throw new ComponentException(String.Format("File does not exist '{0}'.", config.Path.Absolute));
            }

            TLStopwords stopwords = StopwordsReader.ReadStopwords(config.Path);
            Workspace.Store("stopwords", stopwords);

            Logger.Info("Stopwords has been imported from "+config.Path);
        }
    }
}

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
    [Component(GuidIDString = "7364cfbc-49eb-4d2d-9f80-ce6e45246c81",
                Name = "CoestAnswerSetImporter",
                DefaultLabel = "Coest Answer Set Importer",
                Description = "Imports answer from xml Coest format." +
                               "Sample file can be found in Package 'Importers Sample Files', file 'Coest Format/Coest XML SimilarityMatrix Format.xml'",
                Author = "Re Lab",
                Version = "1.0",
                ConfigurationType = typeof(ImporterConfig))]
    [IOSpec(IOSpecType.Output, "answerMatrix", typeof(TLSimilarityMatrix))]
    [Tag("Importers.TLSimilarityMatrix.From XML")]
    public class CoestAnswerSetImporter : BaseComponent
    {
        public CoestAnswerSetImporter(ComponentLogger log) : base(log) 
        {
            m_config = new ImporterConfig();
            Configuration = m_config;
        }

        private ImporterConfig m_config;

        public override void Compute()
        {
            string error;
            //do validation
            if (CoestDatasetImporterHelper.ValidatePath(m_config.FilePath, "Answer Set File", out error))
            {
                var answerMatrix = CoestDatasetImporterHelper.ImportAnswerSet(m_config.FilePath, Logger, m_config.TrimElementValues);
                Workspace.Store("answerMatrix", answerMatrix);
                Logger.Trace(String.Format("Answer matrix imported from {0}.", m_config.FilePath));
            }
            else
            {
                throw new ArgumentException(error);
            }
        }
    }
}
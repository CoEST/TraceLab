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

namespace CoestDatasetsLoader
{
    [Component(GuidIDString = "7154278c-8c68-4f1d-9ad1-98df56f19e3b",
                Name = "CoestAnswerSetImporterWithValidation",
                DefaultLabel = "Coest Answer Set Importer With Validation",
                Description = "Imports answer from xml Coest format. It validates if answer matrix references correct artifacts ids existing in source and target artifacts collections. " +
                              "It may print out various warning about the dataset, if there are some mismatches. " +
                              "Component works well together with Coest Artifacts Importer.",
                Author = "Re Lab",
                Version = "1.0",
                ConfigurationType = typeof(ImporterConfig))]
    [IOSpec(IOSpecType.Input, "sourceArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "targetArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "answerMatrix", typeof(TLSimilarityMatrix))]
    [Tag("Importers.TLSimilarityMatrix.From XML")]
    public class CoestAnswerSetImporterWithValidation : BaseComponent
    {
        public CoestAnswerSetImporterWithValidation(ComponentLogger log)
            : base(log) 
        {
            m_config = new ImporterConfig();
            Configuration = m_config;
        }

        private ImporterConfig m_config;

        public override void Compute()
        {
            TLArtifactsCollection sourceArtifacts = (TLArtifactsCollection)Workspace.Load("sourceArtifacts");
            TLArtifactsCollection targetArtifacts = (TLArtifactsCollection)Workspace.Load("targetArtifacts");
            if (sourceArtifacts == null)
                throw new ArgumentException("Source artifacts cannot be null.");

            if (targetArtifacts == null)
                throw new ArgumentException("Target artifacts cannot be null.");
            
            string error;
            //do validation
            if (CoestDatasetImporterHelper.ValidatePath(m_config.FilePath, "Answer Set File", out error))
            {
                var answerMatrix = CoestDatasetImporterHelper.ImportAnswerSet(m_config.FilePath, sourceArtifacts, targetArtifacts, Logger, m_config.TrimElementValues);
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

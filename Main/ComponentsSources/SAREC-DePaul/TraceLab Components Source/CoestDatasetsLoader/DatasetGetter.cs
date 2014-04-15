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
using System.Collections.Generic;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;

namespace CoestDatasetsLoader
{
    [Component(GuidIDString = "88c78e57-5584-4b7f-a751-33209083bf5a",
                Name = "DatasetGetter",
                DefaultLabel = "Coest Dataset getter",
                Description = "The components retrieves the dataset from the given list of datasets at the given index. It converts dataset into seperate collection of source and target artifacts and their answer set.",
                Author = "Re Lab",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "listOfDatasets", typeof(TLDatasetsList))]
    [IOSpec(IOSpecType.Input, "indexOfDataset", typeof(int))]
    [IOSpec(IOSpecType.Output, "sourceArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "targetArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "answerSet", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "datasetName", typeof(string))]
    [Tag("Helper components")]
    [Tag("Contest utilities")]
    public class DatasetGetter : BaseComponent
    {
        public DatasetGetter(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            TLDatasetsList listOfDatasets = (TLDatasetsList)Workspace.Load("listOfDatasets");
            if (listOfDatasets == null)
            {
                throw new ComponentException("Received null listOfDatasets");
            }
            if (listOfDatasets.Count == 0)
            {
                throw new ComponentException("Received empty listOfDatasets");
            }

            int index = (int)Workspace.Load("indexOfDataset");
            if (index < 0)
            {
                throw new ComponentException("Received negative indexOfDataset");
            }
            if (index > listOfDatasets.Count)
            {
                throw new ComponentException("Received indexOfDataset is higher that number of datasets in the received list.");
            }

            TLDataset dataset = listOfDatasets[index];

            Workspace.Store("sourceArtifacts", dataset.SourceArtifacts);
            Workspace.Store("targetArtifacts", dataset.TargetArtifacts);
            Workspace.Store("answerSet", dataset.AnswerSet);
            Workspace.Store("datasetName", dataset.Name);

            Logger.Info(String.Format("Loaded dataset '{0}' source artifacts collection (id:{1}), and target artifacts collection (id:{2})", dataset.Name, dataset.SourceArtifacts.CollectionId, dataset.TargetArtifacts.CollectionId));
        }
    }
}
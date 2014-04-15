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
using TraceLabSDK;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;

namespace MetricComputationEngine
{
    [Component(Name = "Metric Computation for Single Dataset",
                Description = "Metric computation component computes metric results given two similarity matrix and one corresponding dataset.",
                Author = "ReLab",
                Version = "1.0",
                ConfigurationType=typeof(MetricComputationComponentConfig))]

    // Input/Output
    [IOSpec(IOSpecType.Input, "sourceArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "targetArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "answerMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "similarityMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "results", typeof(TLExperimentsResultsCollection))]

    // Tags
    [Tag("Metrics")]

    public class MetricComputationComponentForSingleDataset : BaseComponent
    {
        private MetricComputationComponentConfig m_config;

        public MetricComputationComponentForSingleDataset(ComponentLogger log) : base(log)
        {
            m_config = new MetricComputationComponentConfig();
            Configuration = m_config;
        }

        public override void Compute()
        {
            // Loading artifacts & datasets from workspace
            TLArtifactsCollection sourceArtifacts = (TLArtifactsCollection)Workspace.Load("sourceArtifacts");
            TLArtifactsCollection targetArtifacts = (TLArtifactsCollection)Workspace.Load("targetArtifacts");
            TLSimilarityMatrix answerMatrix = (TLSimilarityMatrix)Workspace.Load("answerMatrix");
            TLSimilarityMatrix similarityMatrix = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");

            // Checking for null arguments
            if (sourceArtifacts == null)
            {
                throw new ComponentException("The loaded source artifacts cannot be null!");
            }
            if (targetArtifacts == null)
            {
                throw new ComponentException("The loaded target artifacts cannot be null!");
            }
            if (answerMatrix == null)
            {
                throw new ComponentException("The loaded answer matrix cannot be null!");
            }
            if (similarityMatrix == null)
            {
                throw new ComponentException("The loaded similarity matrix cannot be null!");
            }

            // Results calculation
            TLDatasetsList datasets = new TLDatasetsList();
            var dataset = new TLDataset("Experiment results");
            dataset.AnswerSet = answerMatrix;
            dataset.SourceArtifacts = sourceArtifacts;
            dataset.TargetArtifacts = targetArtifacts;
            datasets.Add(dataset);

            TLSimilarityMatricesCollection similarityMatrices = new TLSimilarityMatricesCollection();
            similarityMatrix.Name = "Experiment results";
            similarityMatrices.Add(similarityMatrix);

            MetricComputationEngine engine = new MetricComputationEngine(datasets, Logger, m_config);

            //wrap result similarity matrix into TracingResult
            var tracingResults = GroupOfTracingResults<SingleTracingResults>.Adapt(similarityMatrices, "Experiment results");
            engine.AddTracingResults(tracingResults);
            var results = engine.ComputeResults();

            // Store the results in the workspace
            Workspace.Store("results", results);
        }
    }
}

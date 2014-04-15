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
    [Component(GuidIDString = "39fcc87e-68b5-4609-b7a7-90dba51af983",
                Name = "MetricComputationComponent",
                DefaultLabel = "Metric Computation",
                Description = "Metric computation component computes metric results for given two similarity matrix and one corresponding dataset. "+
                                "The component also computes statistical significance of the improvement from one techqnique to another. "+
                                "If baseline results are not provided, then statistical comparison is omitted and metrics are computed only for one provided technique."+
                                "If resultSimilarityMatrices are not provided, the Experiment Results are empty but include Dataset Names, Metrics Names and their Descriptions, and Metrics Types. "+
                                "Results matrices has to be named by the dataset name for which the matrix was computed for." +
                                "Configuration:  Component configuration allows selecting which metrics are going to be computed from the list of:" +
                                "Average Precision, Recall, Precision, F-2 Measure, RecallPrecisionCurve." + 
                                "It also allows setting what metric is used in the score computation. Note, it is computed independently from actual metrics.",
                Author = "ReLab",
                Version = "1.0",
                ConfigurationType=typeof(MetricComputationComponentConfig))]
    [IOSpec(IOSpecType.Input, "BASELINE", typeof(TLExperimentResults))]
    [IOSpec(IOSpecType.Input, "resultSimilarityMatrices", typeof(TLSimilarityMatricesCollection))]
    [IOSpec(IOSpecType.Input, "datasets", typeof(TLDatasetsList))]
    [IOSpec(IOSpecType.Output, "currentResults", typeof(TLExperimentResults))]
    [IOSpec(IOSpecType.Output, "comparedResults", typeof(TLExperimentsResultsCollection))]
    [Tag("Contest utilities")]
    [Tag("Metrics")]
    public class MetricComputationComponent : BaseComponent
    {
        public MetricComputationComponent(ComponentLogger log) : base(log) 
        {
            m_config = new MetricComputationComponentConfig();
            Configuration = m_config;
        }

        private MetricComputationComponentConfig m_config;

        public override void Compute()
        {
            //load dataset from workspace
            TLDatasetsList datasets = (TLDatasetsList)Workspace.Load("datasets");
            TLSimilarityMatricesCollection resultSimilarityMatrices = (TLSimilarityMatricesCollection)Workspace.Load("resultSimilarityMatrices");

            //wrap result similarity matrix into TracingResult
            var tracingResults = Adapt(resultSimilarityMatrices, "Current technique");

            MetricComputationEngine engine = new MetricComputationEngine(datasets, Logger, m_config);
            engine.AddTracingResults(tracingResults);
            
            TLExperimentsResultsCollection allExperimentResults = engine.ComputeResults();

            //set base data from which all metrics were computed from
            allExperimentResults["Current technique"].BaseData = resultSimilarityMatrices;

            //check if baseline results are in the workspace
            var baseline = (TLExperimentResults)Workspace.Load("BASELINE");
            if (baseline != null)
            {
                //if so, add them to collection of experiment results, so that it can be viewed by GUI component
                allExperimentResults.Add(baseline);

                TLSimilarityMatricesCollection baselineMatrices = baseline.BaseData as TLSimilarityMatricesCollection;

                //compute the score
                allExperimentResults["Current technique"].Score = ScoreComputation.ComputeScore(baselineMatrices, resultSimilarityMatrices, datasets, m_config);
            }
            else
            {
                allExperimentResults["Current technique"].Score = 0.0;
            }

            Workspace.Store("comparedResults", allExperimentResults);
            Workspace.Store("currentResults", allExperimentResults["Current technique"]);

        }

        /// <summary>
        /// Adapts the specified collection of matrices into group of tracing results to comply with engine api
        /// </summary>
        /// <param name="matrices">The matrices.</param>
        /// <returns></returns>
        private GroupOfTracingResults<SingleTracingResults> Adapt(TLSimilarityMatricesCollection matrices, string techniqueName)
        {
            GroupOfTracingResults<SingleTracingResults> results = new GroupOfTracingResults<SingleTracingResults>(techniqueName);
            if (matrices != null)
            {
                foreach (TLSimilarityMatrix matrix in matrices)
                {
                    results.Add(new SingleTracingResults(matrix));
                }
            }
            return results;
        }
    }
}
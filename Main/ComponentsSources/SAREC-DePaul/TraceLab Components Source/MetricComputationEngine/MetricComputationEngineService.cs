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
using TraceLabSDK.Types.Contests;

namespace MetricComputationEngine
{
    public abstract class MetricComputationEngineService<T> where T : ITracingResults
    {
        public MetricComputationEngineService(TLDatasetsList datasets)
        {
            m_datasets = datasets;
            m_allTracingResults = new List<GroupOfTracingResults<T>>();
        }

        private List<MetricComputationForSingleDataset<T>> metricComputationsPerDataset = new List<MetricComputationForSingleDataset<T>>();
        protected void RegisterMetricComputation(MetricComputationForSingleDataset<T> metricComputation)
        {
            metricComputationsPerDataset.Add(metricComputation);
        }

        private List<GroupOfTracingResults<T>> m_allTracingResults;
        private TLDatasetsList m_datasets;

        public void AddTracingResults(GroupOfTracingResults<T> tracingResults)
        {
            m_allTracingResults.Add(tracingResults);
        }

        public TLExperimentsResultsCollection ComputeResults()
        {
            TLExperimentsResultsCollection allResults = new TLExperimentsResultsCollection();
            foreach (GroupOfTracingResults<T> tracingResults in m_allTracingResults)
            {
                var datasetResults = ComputeMetricResultsForDataset(tracingResults);
                allResults.Add(datasetResults);
            }

            return allResults;
        }

        private TLExperimentResults ComputeMetricResultsForDataset(GroupOfTracingResults<T> tracingResults)
        {
            //create experiment results container for this technique
            TLExperimentResults experimentResult = new TLExperimentResults(tracingResults.TechniqueName);

            foreach (TLDataset dataset in m_datasets)
            {
                DatasetResults datasetResults = new DatasetResults(dataset.Name);

                //iterate through all computation and calculate metric results for this dataset
                foreach (MetricComputationForSingleDataset<T> computation in metricComputationsPerDataset)
                {
                    T tracingResult = default(T);
                    if (tracingResults.Contains(dataset.Name))
                    {
                        tracingResult = tracingResults[dataset.Name];
                    }

                    var metric = computation.Compute(tracingResult, dataset);
                    if (metric == null)
                    {
                        throw new InvalidOperationException("The metric computation method failed to return the metric. " +
                                                            "Even if tracing results are empty computation must return metric with name and description, although it may have empty data");
                    }
                    if (computation is IStatisticallyComparableMetric<T>)
                    {
                        //collect results, so that statistical comparison can be computed afterwards, if there are two techniques
                    }

                    datasetResults.AddMetric(metric);
                }

                experimentResult.AddDatasetResult(datasetResults);
            }

            //compute also set of metrics across all datasets combined
            //experimentResult.AcrossAllDatasetsResults = ComputeMetricResultsAcrossAllDatasets(tracingResults);

            return experimentResult;
        }

        //private List<MetricComputationAcrossGroupOfDatasets<T>> metricComputationsAcrossAllDatasets = new List<MetricComputationAcrossGroupOfDatasets<T>>();
        //protected void RegisterMetricComputation(MetricComputationAcrossGroupOfDatasets<T> metricComputation)
        //{
        //    metricComputationsAcrossAllDatasets.Add(metricComputation);
        //}

        //private DatasetResults ComputeMetricResultsAcrossAllDatasets(GroupOfTracingResults<T> tracingResults)
        //{
        //    DatasetResults datasetResults = new DatasetResults("Across all datasets");

        //    //iterate through all computation and calculate metric results across all datasets
        //    foreach (MetricComputationAcrossGroupOfDatasets<T> computation in metricComputationsAcrossAllDatasets)
        //    {
        //        var metric = computation.Compute(tracingResults, m_datasets);
        //        datasetResults.AddMetric(metric);
        //    }

        //    return datasetResults;
        //}
    }
}

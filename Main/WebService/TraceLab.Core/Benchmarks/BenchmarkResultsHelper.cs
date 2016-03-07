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
using TraceLab.Core.WebserviceAccess;
using TraceLab.Core.WebserviceAccess.Metrics;

namespace TraceLab.Core.Benchmarks
{
    public class BenchmarkResultsHelper
    {
        public static ContestResults PrepareBaselineContestRestults(string contestId, 
                                TraceLabSDK.Types.Contests.TLExperimentResults experimentResults,
                                string techniqueName,                    
                                string techniqueDescription)
        {
            List<DatasetResultsDTO> results = new List<DatasetResultsDTO>();
            foreach (TraceLabSDK.Types.Contests.DatasetResults dataset in experimentResults.DatasetsResults)
            {
                results.Add(new DatasetResultsDTO(dataset));
            }

            var baselineResults = new ContestResults(contestId, techniqueName, techniqueDescription, results, experimentResults.Score, experimentResults.BaseData);
            return baselineResults;
        }

        public static void ExtractDatasetsAndMetricsDefinitions(TraceLabSDK.Types.Contests.TLExperimentResults experimentResults,
                                                                out List<MetricDefinition> metrics, out List<string> datasets)
        {
            datasets = new List<string>();
            metrics = new List<MetricDefinition>();

            foreach (TraceLabSDK.Types.Contests.DatasetResults dataset in experimentResults.DatasetsResults)
            {
                datasets.Add(dataset.DatasetName);

                //if metrics has not been yet assigned
                if (metrics.Count == 0)
                {
                    //iterate through first dataset for all metrics 
                    //(assumption is that all datasets are using the same metrics
                    foreach (TraceLabSDK.Types.Contests.Metric metric in dataset.Metrics)
                    {
                        metrics.Add(new MetricDefinition(metric));
                    }
                }
            }
        }
    }
}

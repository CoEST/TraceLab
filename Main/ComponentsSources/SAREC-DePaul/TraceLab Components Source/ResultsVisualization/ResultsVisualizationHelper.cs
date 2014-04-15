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
using TraceLabSDK.Types.Contests;
using ResultsVisualization.Charts;

namespace ResultsVisualization
{
    class ResultsVisualizationHelper
    {
        /// <summary>
        /// Processes the results and stores them in a collection were results are grouped by dataset first
        /// and metric second.
        /// </summary>
        /// <param name="allResults">Results across all datasets and metrics.</param>
        /// <returns>
        /// Collection of results categorized by datasets and metrics.
        /// </returns>
        public static SortedDictionary<string, Dictionary<string, Chart>> ProcessResults(TLExperimentsResultsCollection allResults)
        {
            SortedDictionary<string, Dictionary<string, Chart>> AllDatasetMetricCharts = new SortedDictionary<string, Dictionary<string, Chart>>();

            foreach (TLExperimentResults expResults in allResults)
            {
                // Technique Name
                string techniqueName = expResults.TechniqueName;

                foreach (DatasetResults result in expResults.DatasetsResults)
                {
                    // Dataset Name
                    string datasetName = result.DatasetName;

                    foreach (Metric metric in result.Metrics)
                    {
                        StoreTechnique(AllDatasetMetricCharts, datasetName, metric, techniqueName);
                    }
                }
            }

            return AllDatasetMetricCharts;
        }

        /// <summary>
        /// Creates a new chart or adds data to an existing one.
        /// </summary>
        /// <param name="allDatasetMetricCharts">All results calculated so far.</param>
        /// <param name="dataset">The dataset name.</param>
        /// <param name="metric">The metric data.</param>
        /// <param name="technique">The technique name.</param>
        private static void StoreTechnique(SortedDictionary<string, Dictionary<string, Chart>> allDatasetMetricCharts,
                                            string dataset, Metric metric, string technique)
        {
            // Does this dataset have already been added?
            Dictionary<string, Chart> charts;
            bool newDataset = (allDatasetMetricCharts.TryGetValue(dataset, out charts) == false);
            if (newDataset)
            {
                charts = new Dictionary<string, Chart>();
            }

            // Does this dataset have already been added?
            Chart chart = null;
            charts.TryGetValue(metric.MetricName, out chart);
            if (metric is BoxSummaryData)
            {
                BoxPlot bp = null;
                if (chart is BoxPlot)
                {
                    bp = (BoxPlot)chart;
                }
                else
                {
                    bp = new BoxPlot(metric.MetricName, metric.Description);
                }

                bool boxAdded = bp.AddBox((BoxSummaryData)metric, technique);
                if (chart == null && boxAdded)
                {
                    charts.Add(metric.MetricName, bp);
                }
            }
            else if (metric is LineSeries)
            {
                LineChart lc = null;
                if (chart is LineChart)
                {
                    lc = (LineChart)chart;
                }
                else
                {
                    lc = new LineChart(metric.MetricName, metric.Description);
                }

                bool lineAdded = lc.AddLine((LineSeries)metric, technique);
                if (chart == null && lineAdded)
                {
                    charts.Add(metric.MetricName, lc);
                }
            }

            if (newDataset && charts.Count > 0)
            {
                allDatasetMetricCharts.Add(dataset, charts);
            }
        }
    }
}

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
using TraceLabSDK.Types.Contests;
using TraceLabSDK.Types;

namespace MetricComputationEngine
{
    class ScoreComputation
    {
        /// <summary>
        /// Computes the score for the given current results.
        /// </summary>
        /// <param name="baselineMatrices">The baseline matrices.</param>
        /// <param name="currentResults">The current results.</param>
        /// <returns></returns>
        public static double ComputeScore(TLSimilarityMatricesCollection baselineMatrices, TLSimilarityMatricesCollection currentResultsMatrices, 
                                          TLDatasetsList datasets, MetricComputationComponentConfig config)
        {
            double score = 0.0;

            double sum = 0;
            double counts = 0;

            IMetricComputation metricComputation = GetMetricComputation(config);

            foreach (TLDataset dataset in datasets)
            {
                TLSimilarityMatrix baseline = baselineMatrices[dataset.Name];
                TLSimilarityMatrix current = currentResultsMatrices[dataset.Name];

                //score is computed based on delta between two techniques from metric computation
                SortedDictionary<string, double> baselineValues = metricComputation.Calculate(baseline, dataset);
                SortedDictionary<string, double> currentValues = metricComputation.Calculate(current, dataset);

                var deltas = ScoreComputationHelper.Delta(baselineValues, currentValues);

                //now compute average of computed deltas, and that's the score

                foreach (double delta in deltas.Values)
                {
                    sum += delta;
                }

                counts += deltas.Count;
            }

            score = sum / counts;

            return score;
        }

        /// <summary>
        /// Gets the metric computation.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        private static IMetricComputation GetMetricComputation(MetricComputationComponentConfig config)
        {
            IMetricComputation metricComputation;
            if (config.ScoreBaseMetric == ScoreBaseMetric.PrecisionAtRecall100)
            {
                metricComputation = new PrecisionAtRecall100();
            }
            else if (config.ScoreBaseMetric == ScoreBaseMetric.Precision)
            {
                metricComputation = new Precision(config.Threshold);
            }
            else if (config.ScoreBaseMetric == ScoreBaseMetric.Recall)
            {
                metricComputation = new Recall(config.Threshold);
            }
            else
            {
                metricComputation = new AveragePrecision();
            }
            return metricComputation;
        }
    }
}

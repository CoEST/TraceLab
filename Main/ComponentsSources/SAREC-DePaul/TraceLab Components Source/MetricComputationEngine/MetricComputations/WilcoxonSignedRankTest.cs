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

namespace MetricComputationEngine
{
    /// <summary>
    /// Class is delegate for the any metric that wants to use this test for comparing
    /// </summary>
    public class WilcoxonSignedRankTest : IStatisticallyComparableMetric<SingleTracingResults>
    {
        private IMetricComputation m_baseMetricComputation;

        public WilcoxonSignedRankTest(IMetricComputation baseMetricComputation)
        {
            m_baseMetricComputation = baseMetricComputation;
        }

        #region IStatisticallyComparableMetric Members

        public double Compare(SingleTracingResults techniqueOneResults, SingleTracingResults techniqueTwoResults, TLDataset dataset)
        {
            List<double> differencesResults = PrepareDataForWilcox(techniqueOneResults, techniqueTwoResults, dataset);

            return Wilcox(differencesResults.ToArray());
        }

        public string StatisticalTestName
        {
            get { return "Two sided Wilcoxon Signed Rank Test"; }
        }

        #endregion
        
        private double Wilcox(double[] results)
        {
            double pvalue_bothtails = 0;
            double pvalue_lefttail = 0;
            double pvalue_righttail = 0;
            double e = 0;
            alglib.wilcoxonsignedranktest(results, results.Length, e, out pvalue_bothtails, out pvalue_lefttail, out pvalue_righttail);

            return pvalue_bothtails;
        }

        private List<double> PrepareDataForWilcox(SingleTracingResults techniqueOneResults, SingleTracingResults techniqueTwoResults, TLDataset dataset)
        {
            var avePrec1 = (SortedDictionary<string, double>)m_baseMetricComputation.Calculate(techniqueOneResults.ResultMatrix, dataset);
            var avePrec2 = (SortedDictionary<string, double>)m_baseMetricComputation.Calculate(techniqueTwoResults.ResultMatrix, dataset);

            var deltas = ScoreComputationHelper.Delta(avePrec1, avePrec2);

            return deltas.Values.ToList();
        }


    }
}

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
using TraceLabSDK;

namespace MetricComputationEngine
{
    class RecallMetricComputation : MetricComputationForSingleDataset<SingleTracingResults>, IStatisticallyComparableMetric<SingleTracingResults>
    {
        private const string MetricName = "Recall";
        private const string MetricDescription = "Recall measures the fraction of correctly retrieved documents among the total correct documents. " +
                                                 "Recall is measured at a certain threshold such as a certain relevance score or the number of "+
                                                 "retrieved documents given as a parameter.";

        private double m_threshold;
        private static WilcoxonSignedRankTest s_rankTest;
        private TraceLabSDK.ComponentLogger m_logger;

        public RecallMetricComputation(double threshold, TraceLabSDK.ComponentLogger logger)
        {
            m_logger = logger;
            m_threshold = threshold;
            s_rankTest = new WilcoxonSignedRankTest(new Recall(threshold));
        }

        public override Metric Compute(SingleTracingResults tracingResults, TLDataset dataset)
        {
            SummaryStatistics recallComputation = new SummaryStatistics(MetricName, MetricDescription, new Recall(m_threshold), m_logger);

            return recallComputation.Calculate(tracingResults, dataset);
        }

        #region IStatisticallyComparableMetric Members

        public double Compare(SingleTracingResults techniqueOneResults, SingleTracingResults techniqueTwoResults, TLDataset dataset)
        {
            return s_rankTest.Compare(techniqueOneResults, techniqueTwoResults, dataset);
        }

        public string StatisticalTestName
        {
            get { return s_rankTest.StatisticalTestName; }
        }

        #endregion
    }
}

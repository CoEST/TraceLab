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
    class AveragePrecisionMetricComputation : MetricComputationForSingleDataset<SingleTracingResults>, IStatisticallyComparableMetric<SingleTracingResults>
    {
        private const string MetricName = "Average Precision";
        private const string MetricDescription = "Average precision measures how well a traceability technique retrieves relevant documents " +
                                                 "at the top of the retrieved document list when the documents are listed in descending order of relevance score. " +
                                                 "Average precision is defined as the sum of the precision divided by the number of the total relevant documents, " +
                                                 "where precision is computed after correct retrieval of each document.";

        //private const string MetricDescription = "Average precision measures the extent to which relevant documents are retrieved towards the top of the ranked list.";

        private static WilcoxonSignedRankTest s_rankTest;
        private TraceLabSDK.ComponentLogger m_logger;

        public AveragePrecisionMetricComputation(TraceLabSDK.ComponentLogger logger)
        {
            m_logger = logger;
            s_rankTest = new WilcoxonSignedRankTest(new AveragePrecision());
        }

        public override Metric Compute(SingleTracingResults tracingResults, TLDataset dataset)
        {
            SummaryStatistics averagePrecisionComputation = new SummaryStatistics(MetricName, MetricDescription, new AveragePrecision(), m_logger);

            return averagePrecisionComputation.Calculate(tracingResults, dataset);
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

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
    class PrecisionAtRecall100MetricComputation : MetricComputationForSingleDataset<SingleTracingResults>, IStatisticallyComparableMetric<SingleTracingResults>
    {
        private const string MetricName = "Precision at recall 100%";
        private const string MetricDescription = "Precision at recall 100% measures precision at the earliest point that recall reaches 1 "+
                                                 "when the retrieved documents are listed in descending order of relevance score.";

        private static WilcoxonSignedRankTest s_rankTest;
        private TraceLabSDK.ComponentLogger m_logger;

        public PrecisionAtRecall100MetricComputation(TraceLabSDK.ComponentLogger logger)
        {
            m_logger = logger;
            s_rankTest = new WilcoxonSignedRankTest(new PrecisionAtRecall100());
        }

        public override Metric Compute(SingleTracingResults tracingResults, TLDataset dataset)
        {
            SummaryStatistics precisionComputation = new SummaryStatistics(MetricName, MetricDescription, new PrecisionAtRecall100(), m_logger);

            return precisionComputation.Calculate(tracingResults, dataset);
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

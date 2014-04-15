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
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;
using TraceLabSDK;

namespace MetricComputationEngine
{
    /// <summary>
    /// Summary statistics is a wrapper class around several metrics computations.
    /// The summary statistics compute data to be display by box plots and summary tables.
    /// </summary>
    public class SummaryStatistics
    {
        private IMetricComputation m_metricComputation;
        private string m_metricName;
        private string m_metricDescription;
        private ComponentLogger m_logger;

        public SummaryStatistics(string metricName, string metricDescription, IMetricComputation baseMetricComputation, ComponentLogger logger)
        {
            m_metricComputation = baseMetricComputation;
            m_metricName = metricName;
            m_metricDescription = metricDescription;
            m_logger = logger;
        }
        
        public BoxSummaryData Calculate(SingleTracingResults singleTechniqueResults, TLDataset dataset)
        {
            SortedDictionary<string, double> intermediateResults = m_metricComputation.Calculate(singleTechniqueResults.ResultMatrix, dataset);

            double[] dataPoints = intermediateResults.Values.ToArray();
            if (dataPoints.Length == 0 && m_logger != null)
            {
                m_logger.Warn("Metric computation of '" + m_metricName + "' returned zero matching results for " + dataset.Name + " for one of the techniques. It may be valid results, but it may also mean that there is mismatch of ids in the answer matrix and corresponding artifacts." );
            }

            var summaryData = new BoxSummaryData(m_metricName, m_metricDescription);
            summaryData.AddPoint(new BoxPlotPoint(dataPoints));

            return summaryData;
        }
    }
}

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
using TraceLabSDK.Types;

namespace MetricComputationEngine
{
    /// <summary>
    /// Metric Computation Engine is responsible for managing computation of metric results for the given group of tracing results.
    /// </summary>
    public class MetricComputationEngine : MetricComputationEngineService<SingleTracingResults>
    {
        public MetricComputationEngine(TLDatasetsList datasets, ComponentLogger logger, MetricComputationComponentConfig config)
            : base(datasets)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            m_logger = logger;
            InitMetricComputationsPerDataset(config);
        }

        //public MetricComputationEngine(GroupOfTracingResults<SingleTracingResults> allTracingResults, TLDatasetsList datasets, ComponentLogger logger)
        //    : this(allTracingResults, datasets, logger, 0.01)
        //{
        //}

        //public MetricComputationEngine(GroupOfTracingResults<SingleTracingResults> allTracingResults, TLDatasetsList datasets, ComponentLogger logger, double threshold)
        //    : base(allTracingResults, datasets)
        //{
        //    m_logger = logger;
        //    InitMetricComputationsPerDataset(threshold);
        //    InitMetricComputationAcrossAllDatasets();
        //}

        private void InitMetricComputationsPerDataset(MetricComputationComponentConfig config)
        {
            if (config.AveragePrecision == true)
            {
                RegisterMetricComputation(new AveragePrecisionMetricComputation(m_logger));
            }
            if (config.Recall == true)
            {
                RegisterMetricComputation(new RecallMetricComputation(config.Threshold, m_logger));
            }
            if (config.Precision == true)
            {
                RegisterMetricComputation(new PrecisionMetricComputation(config.Threshold, m_logger));
            }
            if (config.PrecisionAtRecall100 == true)
            {
                RegisterMetricComputation(new PrecisionAtRecall100MetricComputation(m_logger));
            }
            if (config.PrecisionRecallCurve == true)
            {
                RegisterMetricComputation(new PrecisionRecallCurveMetricComputation(m_logger));
            }
            //RegisterMetricComputation(new DataStatisticsMetricSingleDatasetComputation<SingleTracingResults>());
        }

        private void InitMetricComputationAcrossAllDatasets() 
        {
            //RegisterMetricComputation(new AverageRankMetricComputation(new AveragePrecision(), "Mean Rank of Average Precision"));
            //RegisterMetricComputation(new AverageRankMetricComputation(new PrecisionAtRecall100(), "Mean Rank of Precision At Recall 100%"));
            //RegisterMetricComputation(new DataStatisticsMetricComputation<SingleTracingResults>(m_logger));
        }

        private ComponentLogger m_logger;
    }
}

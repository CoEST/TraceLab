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
using System.Runtime.Serialization;

namespace TraceLab.Core.WebserviceAccess.Metrics
{
    /// <summary>
    /// DatasetResults can contain results for metrics for all techniques
    /// </summary>
    [DataContract]
    public class DatasetResultsDTO
    {
        public DatasetResultsDTO(string datasetName) 
        {
            m_datasetName = datasetName;
            m_metrics = new List<IMetricResult>();
        }
        
        public DatasetResultsDTO(TraceLabSDK.Types.Contests.DatasetResults datasetResult)
        {
            m_datasetName = datasetResult.DatasetName;
            m_metrics = new List<IMetricResult>();
            foreach (TraceLabSDK.Types.Contests.Metric metric in datasetResult.Metrics)
            {
                var boxSummaryMetric = metric as TraceLabSDK.Types.Contests.BoxSummaryData;
                var lineSeriesMetric = metric as TraceLabSDK.Types.Contests.LineSeries;
                
                if (boxSummaryMetric != null)
                {
                    m_metrics.Add(new BoxSummaryDataDTO(boxSummaryMetric));
                }
                else if (lineSeriesMetric != null)
                {
                    m_metrics.Add(new LineSeriesDTO(lineSeriesMetric));
                }
                else
                {
                    throw new InvalidOperationException("Experiment results cannot have any metrics that are not compatible with TraceLabSDK.Types.Contests.LineSeries, or TraceLabSDK.Types.Contests.BoxSummaryData");
                }
            }
        }

        private string m_datasetName;
        /// <summary>
        /// Gets or sets the name of the dataset, the identifier for dataset for which results were computed.
        /// </summary>
        /// <value>
        /// The name of the dataset.
        /// </value>
        [DataMember]
        public string DatasetName
        {
            get
            {
                return m_datasetName;
            }
            set
            {
                m_datasetName = value;
            }
        }

        private List<IMetricResult> m_metrics;
        [DataMember]
        public List<IMetricResult> Metrics
        {
            get
            {
                return m_metrics;
            }
            set
            {
                m_metrics = value;
            }
        }
    }
}

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
    [DataContract]
    public class MetricDefinition
    {
        protected MetricDefinition() { }

        public MetricDefinition(TraceLabSDK.Types.Contests.Metric metric)
        {
            Name = metric.MetricName;
            Description = metric.Description;

            if(metric.GetType().Equals(typeof(TraceLabSDK.Types.Contests.LineSeries))) 
            {
                MetricType = typeof(LineSeriesDTO).Name;
            }
            else if (metric.GetType().Equals(typeof(TraceLabSDK.Types.Contests.BoxSummaryData)))
            {
                MetricType = typeof(BoxSummaryDataDTO).Name;
            }
            else
            {
                throw new ArgumentException("Provided metric is not of valid type. Only LineSeries and BoxSummaryData from TraceLabSDK are allowed.");
            }
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        private string m_metricType;

        [DataMember]
        public string MetricType 
        { 
            get { return m_metricType; }
            set 
            {
                if (MetricTypesManager.GetMetricTypes.Any(t => t.Name.Equals(value)))
                {
                    m_metricType = value;
                }
                else
                {
                    throw new InvalidOperationException(String.Format("The metric type needs to be one of available metric s_metricTypes. The {0} is not a valid metric type.", value));
                }
            }
        }
    }
}

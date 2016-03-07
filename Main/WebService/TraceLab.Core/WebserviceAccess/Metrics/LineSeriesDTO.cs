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
    public sealed class LineSeriesDTO : IMetricResult
    {
        public LineSeriesDTO(string metricName) 
        {
            m_metricName = metricName;
            m_points = new List<PointDTO>();
        }

        public LineSeriesDTO(string metricName, List<PointDTO> points)
        {
            m_metricName = metricName;
            m_points = points;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeriesDTO"/> class
        /// based on original line series coming from the user data.
        /// </summary>
        /// <param name="lineSeries">The line series.</param>
        public LineSeriesDTO(TraceLabSDK.Types.Contests.LineSeries lineSeries)
        {
            m_metricName = lineSeries.MetricName;
            m_points = new List<PointDTO>();
            foreach (TraceLabSDK.Types.Contests.Point point in lineSeries.Points)
            {
                m_points.Add(new PointDTO(point.X, point.Y));
            }
        }

        private List<PointDTO> m_points;
        [DataMember]
        public List<PointDTO> Points
        {
            get { return m_points; }
            set { m_points = value; }
        }

        private string m_metricName;
        [DataMember]
        public string MetricName
        {
            get 
            {
                return m_metricName;
            }
            set
            {
                m_metricName = value;
            }
        }
    }

}

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
    public class BoxPlotPointDTO
    {
        public BoxPlotPointDTO(TraceLabSDK.Types.Contests.BoxPlotPoint boxPlotPoint)
        {
            Min = boxPlotPoint.Min;
            Q1 = boxPlotPoint.Q1;
            Median = boxPlotPoint.Median;
            Mean = boxPlotPoint.Mean;
            Q3 = boxPlotPoint.Q3;
            Max = boxPlotPoint.Max;
            StdDev = boxPlotPoint.StdDev;
            N = boxPlotPoint.N;
        }

        public BoxPlotPointDTO(double min, double q1, double median, double mean, double q3, double max, double stdDev, double n)
        {
            Min = min;
            Q1 = q1;
            Median = median;
            Mean = mean;
            Q3 = q3;
            Max = max;
            StdDev = stdDev;
            N = n;
        }
        
        [DataMember]
        public double Min
        {
            get;
            set;
        }

        [DataMember]
        public double Q1
        {
            get;
            set;
        }

        [DataMember]
        public double Median
        {
            get;
            set;
        }

        [DataMember]
        public double Mean
        {
            get;
            set;
        }

        [DataMember]
        public double Q3
        {
            get;
            set;
        }

        [DataMember]
        public double Max
        {
            get;
            set;
        }

        [DataMember]
        public double StdDev
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Number of Datapoints.
        /// </summary>
        /// <value>
        /// The N.
        /// </value>
        [DataMember]
        public double N
        {
            get;
            set;
        }
    }
}

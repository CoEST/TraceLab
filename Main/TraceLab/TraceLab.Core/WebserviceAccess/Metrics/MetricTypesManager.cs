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

namespace TraceLab.Core.WebserviceAccess.Metrics
{
    /// <summary>
    /// The metric types manager manages available metric, so that they can be used in the webservice
    /// </summary>
    class MetricTypesManager
    {
        static MetricTypesManager()
        {
            //note that all types has to extend abstract MetricResult class
            m_availableMetricTypes = new List<Type>() {
                typeof(LineSeriesDTO),
                typeof(BoxSummaryDataDTO)
            };
        }

        private static List<Type> m_availableMetricTypes;

        public static IEnumerable<Type> GetMetricTypes
        {
            get
            {
                return m_availableMetricTypes;
            }
        }

    }
}

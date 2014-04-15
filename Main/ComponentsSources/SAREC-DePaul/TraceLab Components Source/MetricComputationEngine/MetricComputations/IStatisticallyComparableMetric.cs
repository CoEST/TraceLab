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
    /// <summary>
    /// If Metric computation implements the interface, means that provided computed data metric can do
    /// statistical analysis of the difference significance between two techniques. 
    /// </summary>
    interface IStatisticallyComparableMetric<T>
    {
        /// <summary>
        /// Compares the specified two techniques
        /// </summary>
        /// <param name="techniqueOneResults">The technique one results.</param>
        /// <param name="techniqueTwoResults">The technique two results.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns>pvalue of the comparison</returns>
        double Compare(T techniqueOneResults, T techniqueTwoResults, TLDataset dataset);

        /// <summary>
        /// Gets the name of the statistical test.
        /// </summary>
        /// <value>
        /// The name of the statistical test.
        /// </value>
        string StatisticalTestName
        {
            get;
        }
    }
}

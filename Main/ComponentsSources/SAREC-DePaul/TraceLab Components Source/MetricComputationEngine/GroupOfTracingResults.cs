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
using System.Collections.ObjectModel;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;

namespace MetricComputationEngine
{
    /// <summary>
    /// The class represents the collection of the results from one techniques for multiple datasets.
    /// </summary>
    public class GroupOfTracingResults<T> : KeyedCollection<string, T> where T : ITracingResults
    {
        public GroupOfTracingResults(string techniqueName) : base()
        {
            m_techniqueName = techniqueName;
        }

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        protected override string GetKeyForItem(T item)
        {
            return item.DatasetName;
        }

        private string m_techniqueName;

        public string TechniqueName
        {
            get { return m_techniqueName; }
            private set { m_techniqueName = value; }
        }

        /// <summary>
        /// Adapts the specified collection of matrices into group of tracing results to comply with engine api
        /// </summary>
        /// <param name="matrices">The matrices.</param>
        /// <returns></returns>
        public static GroupOfTracingResults<SingleTracingResults> Adapt(TLSimilarityMatricesCollection matrices, string techniqueName)
        {
            GroupOfTracingResults<SingleTracingResults> results = new GroupOfTracingResults<SingleTracingResults>(techniqueName);
            if (matrices != null)
            {
                foreach (TLSimilarityMatrix matrix in matrices)
                {
                    results.Add(new SingleTracingResults(matrix));
                }
            }
            return results;
        }
    }
}

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
using System.Collections;

namespace TraceLab.UI.WPF.ViewModels
{
    class NodeComparer : IComparer
    {
        #region IComparer Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero <paramref name="x"/> is less than <paramref name="y"/>. Zero <paramref name="x"/> equals <paramref name="y"/>. Greater than zero <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception>
        public int Compare(object x, object y)
        {
            var baseX = x as CLVBaseNode;
            var baseY = y as CLVBaseNode;
            if (baseX == null)
                return 1;
            if (baseY == null)
                return -1;
            if (baseX == baseY)
                return 0;
            if (baseX.Equals(baseY))
                return 0;

            var referencesX = x as CLVReferenceContainerNode;
            var referencesY = y as CLVReferenceContainerNode;
            if (referencesX != null)
                return -1;
            if (referencesY != null)
                return 1;

            int comparison = baseX.Label.CompareTo(baseY.Label);

            if (comparison == 0)
            {
                baseX.IsDuplicateLabel = true;
                baseY.IsDuplicateLabel = true;
            }

            return comparison;
        }

        #endregion
    }
}

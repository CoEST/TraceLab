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
using System.Windows.Forms;

namespace TraceabilityUserFeedbackGUI
{
    class WeightsSorter : IComparer
    {
        public int Column = 1;
        public System.Windows.Forms.SortOrder Order = SortOrder.Descending;
        
        public int Compare(object x, object y)
        {
            if (!(x is ListViewItem))
                return (0);
            if (!(y is ListViewItem))
                return (0);

            try
            {
                double dbl1 = double.Parse(((ListViewItem)x).SubItems[Column].Text);
                double dbl2 = double.Parse(((ListViewItem)y).SubItems[Column].Text);

                if (Order == SortOrder.Ascending)
                {
                    return dbl1.CompareTo(dbl2);
                }
                else
                {
                    return dbl2.CompareTo(dbl1);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

    }
}

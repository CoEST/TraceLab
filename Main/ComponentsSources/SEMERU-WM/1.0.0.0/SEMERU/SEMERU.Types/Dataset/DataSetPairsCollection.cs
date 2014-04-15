using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Types.Dataset
{
    [Serializable]
    public class DataSetPairsCollection : List<DataSetPairs>
    {
        public DataSetPairsCollection() : base() { }

        public string ToOutputString()
        {
            StringBuilder txt = new StringBuilder("                 P    R    AvgP MeanAvgP\n");

            foreach (DataSetPairs metric in this)
            {
                txt.Append(metric.Name);
                for (int i = metric.Name.Length; i <= 16; i++)
                {
                    txt.Append(" ");
                }
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.PrecisionData)));
                txt.Append(" ");
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.RecallData)));
                txt.Append(" ");
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.AveragePrecisionData)));
                txt.Append(" ");
                txt.Append(String.Format("{0:0.00}", CalculateAverage(metric.MeanAveragePrecisionData)));
                txt.AppendLine();
            }

            return txt.ToString();
        }

        public static double CalculateAverage(TLKeyValuePairsList metrics)
        {
            double val = 0.0;
            double non = 0.0;

            foreach (KeyValuePair<string, double> metric in metrics)
            {
                if (!Double.IsNaN(metric.Value))
                    val += metric.Value;
                else
                    non++;
            }

            return val / (metrics.Count - non);
        }
    }
}

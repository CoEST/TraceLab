using System;
using System.Collections.Generic;
using TraceLabSDK.Types;

/// SEMERU Component Library Extension - Custom additions to the SEMERU Component Library
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library Extension.
/// 
/// The SEMERU Component Library Extension is free software: you can redistribute it
/// and/or modify it under the terms of the GNU General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or (at your
/// option) any later version.
/// 
/// The SEMERU Component Library Extension is distributed in the hope that it will
/// be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
/// License for more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library Extension.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Custom.CSMR2012.Models
{
    public static class SharedUtils
    {
        /// <summary>
        /// Compute the delta value for each source artifact, then return the median of the delta values
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double ComputeDelta(TLSimilarityMatrix matrix)
        {
            List<double> DeltaLookup = new List<double>();
            foreach (string source in matrix.SourceArtifactsIds)
            {
                DeltaLookup.Add(ComputeDeltaForSourceArtifact(matrix, source));
            }
            DeltaLookup.Sort();
            if (DeltaLookup.Count % 2 == 0)
            {
                return (DeltaLookup[DeltaLookup.Count / 2] + DeltaLookup[(DeltaLookup.Count / 2) + 1]) / 2.0;
            }
            else
            {
                return DeltaLookup[Convert.ToInt32(Math.Ceiling(DeltaLookup.Count / 2.0))];
            }
        }

        public static double ComputeDeltaForSourceArtifact(TLSimilarityMatrix matrix, string source)
        {
            matrix.Threshold = double.MinValue;
            double min = Double.MaxValue;
            double max = Double.MinValue;
            foreach (TLSingleLink link in matrix.GetLinksAboveThresholdForSourceArtifact(source))
            {
                if (link.Score < min)
                {
                    min = link.Score;
                }
                if (link.Score > max)
                {
                    max = link.Score;
                }
            }
            double delta = (max - min) / 2.0;
            // according to R scripts
            if (delta < 0.05)
            {
                delta = Math.Pow(delta, 4) / 4;
            }
            return delta;
        }
    }
}

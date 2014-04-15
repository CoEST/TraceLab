using System;
using SEMERU.Core.Models;
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

namespace SEMERU.Core.Preprocessors
{
    public static class SmoothingFilter
    {
        /// <summary>
        /// Smoothing filter from ICPC'11 paper "Improving IR-based Traceability Recovery Using Smoothing Filters"
        /// </summary>
        /// <param name="artifacts">Artifacts</param>
        /// <returns>Smoothed artifacts</returns>
        public static TermDocumentMatrix Compute(TLArtifactsCollection artifacts)
        {
            return Compute(new TermDocumentMatrix(artifacts));
        }

        /// <summary>
        /// Smoothing filter from ICPC'11 paper "Improving IR-based Traceability Recovery Using Smoothing Filters"
        /// </summary>
        /// <param name="artifacts">Artifacts</param>
        /// <returns>Smoothed artifacts</returns>
        public static TermDocumentMatrix Compute(TermDocumentMatrix matrix)
        {
            double[] avg = ComputeAverageVector(matrix);

            if (avg.Length != matrix.NumTerms)
                throw new ArgumentException("Average vector does not have the correct number of terms.");

            for (int i = 0; i < matrix.NumDocs; i++)
            {
                for (int j = 0; j < matrix.NumTerms; j++)
                {
                    matrix[i, j] -= avg[j];
                    if (matrix[i, j] < 0.0)
                    {
                        matrix[i, j] = 0.0;
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Computes the average term vector of the matrix
        /// </summary>
        /// <param name="matrix">Artifacts</param>
        /// <returns>Average vector</returns>
        private static double[] ComputeAverageVector(TermDocumentMatrix matrix)
        {
            double[] avg = new double[matrix.NumTerms];
            for (int j = 0; j < matrix.NumTerms; j++)
            {
                for (int i = 0; i < matrix.NumDocs; i++)
                {
                    avg[j] += matrix[i, j];
                }
                avg[j] = avg[j] / matrix.NumDocs;
            } 
            return avg;
        }
    }
}

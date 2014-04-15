using SEMERU.Core.IO;
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

namespace SEMERU.Core.Models
{
    /// <summary>
    /// Responsible for computing affine transformations of two distributions.
    /// </summary>
    public static class Affine
    {
        /// <summary>
        /// Performs an affine transformation on two similarity matrices.
        /// </summary>
        /// <param name="large">Large expert</param>
        /// <param name="small">Small expert</param>
        /// <param name="lambda">Weight given to large expert</param>
        /// <returns>Transformed similarities</returns>
        public static TLSimilarityMatrix Transform(TLSimilarityMatrix large, TLSimilarityMatrix small, double lambda)
        {
            TLSimilarityMatrix largeNormal = Normalize(large);
            TLSimilarityMatrix smallNormal = Normalize(small);
            TLSimilarityMatrix combined = new TLSimilarityMatrix();

            foreach (TLSingleLink largeLink in largeNormal.AllLinks)
            {
                double smallLink = smallNormal.GetScoreForLink(largeLink.SourceArtifactId, largeLink.TargetArtifactId);
                combined.AddLink(largeLink.SourceArtifactId, largeLink.TargetArtifactId, Combine(largeLink.Score, smallLink, lambda));
            }

            return combined;
        }

        /// <summary>
        /// Combines two similarity scores
        /// </summary>
        /// <param name="large">Large expert</param>
        /// <param name="small">Small expert</param>
        /// <param name="lambda">Weight given to large expert</param>
        /// <returns>Transformed similarity</returns>
        public static double Combine(double large, double small, double lambda)
        {
            return (lambda * large) + ((1 - lambda) * small);
        }

        /// <summary>
        /// Normalizes a similarity matrix
        /// </summary>
        /// <param name="matrix">Similarity matrix</param>
        /// <returns>Normalized similarity matrix</returns>
        public static TLSimilarityMatrix Normalize(TLSimilarityMatrix matrix)
        {
            TLSimilarityMatrix norm = new TLSimilarityMatrix();
            double mean = Similarities.AverageSimilarity(matrix);
            double stdDev = Similarities.SimilarityStandardDeviation(matrix);

            foreach (TLSingleLink link in matrix.AllLinks)
            {
                norm.AddLink(link.SourceArtifactId, link.TargetArtifactId, (link.Score - mean) / stdDev);
            }

            return norm;
        }
    }
}

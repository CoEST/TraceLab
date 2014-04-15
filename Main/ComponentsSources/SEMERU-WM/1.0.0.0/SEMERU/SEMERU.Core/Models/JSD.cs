using System;
using System.Collections.Generic;
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
    /// Responsible for computing Jensen-Shannon divergence
    /// </summary>
    public static class JSD
    {
        /// <summary>
        /// Computes Jensen-Shannon divergence on two TLArtifactsCollections
        /// </summary>
        /// <param name="source">Source artifacts collection</param>
        /// <param name="target">Target artifacts collection</param>
        /// <returns>Similarity matrix</returns>
        public static TLSimilarityMatrix Compute(TLArtifactsCollection source, TLArtifactsCollection target)
        {
            return Compute(new TermDocumentMatrix(source), new TermDocumentMatrix(target));
        }

        /// <summary>
        /// Computes Jensen-Shannon divergence on two TermDocumentMatrices
        /// </summary>
        /// <param name="source">Source artifacts collection</param>
        /// <param name="target">Target artifacts collection</param>
        /// <returns>Similarity matrix</returns>
        public static TLSimilarityMatrix Compute(TermDocumentMatrix source, TermDocumentMatrix target)
        {
            List<TermDocumentMatrix> matrices = TermDocumentMatrix.Equalize(source, target);
            TLSimilarityMatrix sims = new TLSimilarityMatrix();
            for (int i = 0; i < matrices[0].NumDocs; i++)
            {
                TLLinksList list = new TLLinksList();
                for (int j = 0; j < matrices[1].NumDocs; j++)
                {
                    list.Add(new TLSingleLink(matrices[0].GetDocumentName(i), matrices[1].GetDocumentName(j),
                        DocumentSimilarity(matrices[0].GetDocument(i), matrices[1].GetDocument(j))));
                }
                list.Sort();
                foreach (TLSingleLink link in list)
                {
                    sims.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                }
            }
            return sims;
        }

        /// <summary>
        /// Computes similarity between two documents.
        /// </summary>
        /// <param name="document1"></param>
        /// <param name="document2"></param>
        /// <returns></returns>
        private static double DocumentSimilarity(double[] document1, double[] document2)
        {
            double similarity;
            //1. Transform documents in two probability distributions
            double[] distribution1 = new double[document1.Length];
            double[] distribution2 = new double[document2.Length];
            double sum1 = 0, sum2 = 0;
            for (int i = 0; i < document1.Length; i++)
            {
                sum1 = sum1 + document1[i];
                sum2 = sum2 + document2[i];
            }
            for (int i = 0; i < document1.Length; i++)
            {
                distribution1[i] = document1[i] / sum1;
                distribution2[i] = document2[i] / sum2;
            }
            //2. Compute Jensen-Shannon divergence between probability distribution
            double[] temp;
            temp = sumDocument(distribution1, distribution2);
            temp = mulDocument(0.5, temp);
            similarity = entropy(temp);
            similarity = similarity - (entropy(distribution1) + entropy(distribution2)) / 2;
            //3. Compute Jensen Shannon similarity
            similarity = 1 - similarity;
            return similarity;
        }

        /// <summary>
        /// Computes the entropy of a document distribution
        /// </summary>
        /// <param name="docDistrib">document distribution</param>
        /// <returns>entropy</returns>
        private static double entropy(double[] docDistrib)
        {
            int i;
            double entropia = 0;
            for (i = 0; i < docDistrib.Length; i++)
            {
                if (docDistrib[i] > 0)
                {
                    entropia = entropia - docDistrib[i] * Math.Log(docDistrib[i], 2);
                }
            }

            return entropia;
        }

        /// <summary>
        /// Computes the sums of two document vectors.
        /// </summary>
        /// <param name="document1">document1 vector</param>
        /// <param name="document2">document2 vector</param>
        /// <returns>vector sum</returns>
        private static double[] sumDocument(double[] document1, double[] document2)
        {
            double[] sum = new double[document1.Length];

            for (int i = 0; i < sum.Length; i++)
            {
                sum[i] = document1[i] + document2[i];
            }

            return sum;
        }

        /// <summary>
        /// Multplies a vector by a scalar.
        /// </summary>
        /// <param name="pScalar">scalar value</param>
        /// <param name="pVector">vector</param>
        /// <returns>Multiplied vector</returns>
        private static double[] mulDocument(double pScalar, double[] pVector)
        {
            double[] mul = new double[pVector.Length];
            for (int i = 0; i < mul.Length; i++)
                mul[i] = pScalar * pVector[i];
            return mul;
        }
    }
}

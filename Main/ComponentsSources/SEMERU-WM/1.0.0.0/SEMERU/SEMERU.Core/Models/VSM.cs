using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Responsible for computing VSM similarities
    /// </summary>
    public static class VSM
    {
        /// <summary>
        /// Computes similarities between documents via the Vector Space Model
        /// using a tf-idf weighting scheme and cosine similarity.
        /// </summary>
        /// <param name="source">Source artifacts</param>
        /// <param name="target">Target artifacts</param>
        /// <returns>Similarity matrix</returns>
        public static TLSimilarityMatrix Compute(TLArtifactsCollection source, TLArtifactsCollection target)
        {
            return Compute(new TermDocumentMatrix(source), new TermDocumentMatrix(target));
        }

        /// <summary>
        /// Computes similarities between term-by-document matrices via the Vector Space Model
        /// using a tf-idf weighting scheme and cosine similarity.
        /// </summary>
        /// <param name="source">Source matrix</param>
        /// <param name="target">Target matrix</param>
        /// <returns>Similarity matrix</returns>
        public static TLSimilarityMatrix Compute(TermDocumentMatrix source, TermDocumentMatrix target)
        {
            TermDocumentMatrix IDs = ComputeIdentities(source);
            TermDocumentMatrix TF = ComputeTF(target);
            double[] IDF = ComputeIDF(ComputeDF(target), target.NumDocs);
            TermDocumentMatrix TFIDF = ComputeTFIDF(TF, IDF);
            return ComputeSimilarities(IDs, TFIDF);
        }

        #region private

        /// <summary>
        /// Computes boolean (0|1) terms in documents.
        /// </summary>
        /// <param name="matrix">Term-by-document matrix</param>
        /// <returns>Term-by-document matrix with 1s for terms that are in the document and 0s for terms that are not.</returns>
        private static TermDocumentMatrix ComputeIdentities(TermDocumentMatrix matrix)
        {
            for (int i = 0; i < matrix.NumDocs; i++)
            {
                for (int j = 0; j < matrix.NumTerms; j++)
                {
                    matrix[i,j] = (matrix[i,j] > 0.0) ? 1.0 : 0.0;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Computes the term frequencies of each document.
        /// </summary>
        /// <param name="matrix">Term-by-document matrix</param>
        /// <returns>tf-weighted term-by-document matrix</returns>
        private static TermDocumentMatrix ComputeTF(TermDocumentMatrix matrix)
        {
            for (int i = 0; i < matrix.NumDocs; i++)
            {
                double max = matrix.GetDocument(i).Max();
                for (int j = 0; j < matrix.NumTerms; j++)
                {
                    matrix[i,j] = matrix[i,j] / max;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Computes the document frequencies of each term
        /// </summary>
        /// <param name="matrix">Term-by-document matrix</param>
        /// <returns>df-weighted term distribution</returns>
        private static double[] ComputeDF(TermDocumentMatrix matrix)
        {
            double[] df = new double[matrix.NumTerms];
            for (int j = 0; j < matrix.NumTerms; j++)
            {
                df[j] = 0.0;
                for (int i = 0; i < matrix.NumDocs; i++)
                {
                    df[j] += (matrix[i,j] > 0.0) ? 1.0 : 0.0;
                }
            }
            return df;
        }

        /// <summary>
        /// Computes the inverse document frequencies of a document frequencies vector
        /// </summary>
        /// <param name="df">Document frequencies vector</param>
        /// <returns>Inverse document frequencies vector</returns>
        private static double[] ComputeIDF(double[] df, int numDocs)
        {
            double[] idf = new double[df.Length];
            for (int i = 0; i < df.Length; i++)
            {
                if (df[i] <= 0.0)
                {
                    idf[i] = 0.0;
                }
                else
                {
                    idf[i] = Math.Log(numDocs / df[i]);
                }
            }
            return idf;
        }

        /// <summary>
        /// Computes tf-idf weights
        /// </summary>
        /// <param name="tf">Term-frequency weighted matrix</param>
        /// <param name="idf">Inverse document frequencies vector</param>
        /// <returns></returns>
        private static TermDocumentMatrix ComputeTFIDF(TermDocumentMatrix tf, double[] idf)
        {
            for (int i = 0; i < tf.NumDocs; i++)
            {
                for (int j = 0; j < tf.NumTerms; j++)
                {
                    tf[i,j] = tf[i,j] * idf[j];
                }
            }
            return tf;
        }

        /// <summary>
        /// Computes cosine similarities between a set of boolean document vectors and a tfidf weighted corpus
        /// </summary>
        /// <param name="ids">Boolean document vectors</param>
        /// <param name="tfidf">tf-idf weighted document vectors</param>
        /// <returns>Similarity matrix</returns>
        private static TLSimilarityMatrix ComputeSimilarities(TermDocumentMatrix ids, TermDocumentMatrix tfidf)
        {
            TLSimilarityMatrix sims = new TLSimilarityMatrix();
            List<TermDocumentMatrix> matrices = TermDocumentMatrix.Equalize(ids, tfidf);
            for (int i = 0; i < ids.NumDocs; i++)
            {
                TLLinksList links = new TLLinksList();
                for (int j = 0; j < tfidf.NumDocs; j++)
                {
                    double product = 0.0;
                    double asquared = 0.0;
                    double bsquared = 0.0;
                    for (int k = 0; k < matrices[0].NumTerms; k++)
                    {
                        double a = matrices[0][i, k];
                        double b = matrices[1][j, k];
                        product += (a * b);
                        asquared += Math.Pow(a, 2);
                        bsquared += Math.Pow(b, 2);
                    }
                    double cross = Math.Sqrt(asquared) * Math.Sqrt(bsquared);
                    if (cross == 0.0)
                    {
                        links.Add(new TLSingleLink(ids.GetDocumentName(i), tfidf.GetDocumentName(j), 0.0));
                    }
                    else
                    {
                        links.Add(new TLSingleLink(ids.GetDocumentName(i), tfidf.GetDocumentName(j), product / cross));
                    }
                }
                links.Sort();
                foreach (TLSingleLink link in links)
                {
                    sims.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                }
            }
            return sims;
        }

        #endregion
    }
}

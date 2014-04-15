using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;
using AdvSoftEng.Types;

namespace AdvSoftEng.Models
{
    public static class CosineSimilarity
    {
        public static TLSimilarityMatrix Compute(NormalizedVectorCollection docs, NormalizedVector lengths, DocumentVectorCollection queries)
        {
            TLSimilarityMatrix sims = new TLSimilarityMatrix();

            foreach (KeyValuePair<string, DocumentVector> QueryKVP in queries)
            {
                /*
                 * Since tf in queries are all 1,
                 * we can assume this term is the sqrt of the size of the dictionary
                 */
                double qVal = Math.Sqrt(QueryKVP.Value.Count);
                foreach (KeyValuePair<string, NormalizedVector> DocKVP in docs)
                {
                    double dVal = lengths[DocKVP.Key];
                    double qdVec = ComputeProduct(QueryKVP.Value, DocKVP.Value);
                    sims.AddLink(QueryKVP.Key, DocKVP.Key, qdVec / (qVal * dVal));
                }
            }

            return sims;
        }

        /*
         * Instead of looking at every term across all documents,
         * only look at the terms in the query, because all other terms
         * will be 0, resulting in q*d=0.
         * Typically the number of terms in a query is less than
         * the number of terms in a document.
         */
        private static double ComputeProduct(DocumentVector query, NormalizedVector doc)
        {
            double val = 0;
            foreach (KeyValuePair<string, int> term in query)
            {
                double d;
                doc.TryGetValue(term.Key, out d);
                val += term.Value * d;
            }
            return val;
        }
    }
}

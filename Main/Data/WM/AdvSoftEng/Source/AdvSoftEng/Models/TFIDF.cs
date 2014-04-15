using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvSoftEng.Types;

namespace AdvSoftEng.Models
{
    public static class TFIDF
    {
        public static NormalizedVectorCollection Compute(NormalizedVectorCollection tf, NormalizedVector idf)
        {
            NormalizedVectorCollection tfidf = new NormalizedVectorCollection();

            foreach (KeyValuePair<string, NormalizedVector> doc in tf)
            {
                NormalizedVector vec = new NormalizedVector(doc.Key);
                foreach (KeyValuePair<string, double> term in doc.Value)
                {
                    vec.Add(term.Key, term.Value * idf[term.Key]);
                }
                tfidf.Add(vec);
            }

            return tfidf;
        }
    }
}

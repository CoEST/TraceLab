using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvSoftEng.Types;

namespace AdvSoftEng.Models
{
    public class Normalizer
    {
        private NormalizedVectorCollection vectors;
        public NormalizedVectorCollection Vectors
        {
            get
            {
                return vectors;
            }
        }

        private NormalizedVector lengths;
        public NormalizedVector Lengths
        {
            get
            {
                return lengths;
            }
        }

        public Normalizer(DocumentVectorCollection documents)
        {
            lengths = new NormalizedVector("DocumentVectorLengths");
            vectors = new NormalizedVectorCollection();

            foreach (KeyValuePair<string, DocumentVector> kvp in documents)
            {
                String id = kvp.Key;
                NormalizedVector vec = Normalize(id, kvp.Value);
                vectors.Add(vec);
            }
        }


        private NormalizedVector Normalize(String id, DocumentVector doc)
        {
            NormalizedVector vec = new NormalizedVector(id);

            // find max value
            int max = 0;
            foreach (KeyValuePair<string, int> term in doc)
            {
                if (term.Value > max)
                {
                    max = term.Value;
                }
            }

            lengths.Add(id, 0);

            // add normalized frequencies
            foreach (KeyValuePair<string, int> term in doc)
            {
                double tf = term.Value / (double)max;
                vec.Add(term.Key, tf);
                lengths[id] += Math.Pow(tf, 2);
            }

            lengths[id] = Math.Sqrt(lengths[id]);

            return vec;
        }

        /*
         * Return the square root of the sum of the squares of the vector
         * sqrt(sum(v_i^2))
         */
        private double ComputeLength(NormalizedVector vec)
        {
            double val = 0;
            foreach (double term in vec.Values)
            {
                val += Math.Pow(term, 2);
            }
            return Math.Sqrt(val);
        }
    }
}

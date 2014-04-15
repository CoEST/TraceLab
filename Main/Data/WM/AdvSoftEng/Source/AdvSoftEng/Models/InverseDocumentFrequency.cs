using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvSoftEng.Types;

namespace AdvSoftEng.Models
{
    public static class InverseDocumentFrequency
    {
        public static NormalizedVector Compute(DocumentVector df, int numDocs)
        {
            NormalizedVector idf = new NormalizedVector("InverseDocumentFrequencies");

            foreach (KeyValuePair<string, int> kvp in df)
            {
                idf.Add(kvp.Key, Math.Log(numDocs / (double) kvp.Value, 2));
            }

            return idf;
        }
    }
}

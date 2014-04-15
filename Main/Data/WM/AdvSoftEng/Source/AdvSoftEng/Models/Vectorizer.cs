using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvSoftEng.Types;
using TraceLabSDK.Types;

namespace AdvSoftEng.Models
{
    public class Vectorizer
    {
        private DocumentVectorCollection vectors;
        public DocumentVectorCollection Vectors
        {
            get
            {
                return vectors;
            }
        }

        private DocumentVector freq;
        public DocumentVector Frequencies
        {
            get
            {
                return freq;
            }
        }

        public Vectorizer(TLArtifactsCollection artifacts, String representation)
        {
            vectors = new DocumentVectorCollection();
            freq = new DocumentVector("DocumentFrequencies");

            foreach (KeyValuePair<string, TLArtifact> kvp in artifacts)
            {
                // vars
                String docID = kvp.Value.Id;
                String[] words = kvp.Value.Text.Split(' ');

                // create new document representation
                DocumentVector vec = new DocumentVector(docID);
                List<String> addedWords = new List<String>();

                // loop over each word and update its frequency
                foreach (String word in words)
                {
                    // update term-doc frequency only ONCE per document
                    if (!freq.ContainsKey(word))
                    {
                        freq.Add(word, 1);
                        addedWords.Add(word);
                    }
                    else if (!addedWords.Contains(word))
                    {
                        freq[word]++;
                        addedWords.Add(word);
                    }

                    // update word freqency
                    if (!vec.ContainsKey(word))
                    {
                        vec.Add(word, 1);
                    }
                    else
                    {
                        if (representation == "Ordinal")
                        {
                            vec[word]++;
                        }
                    }
                    // update MaxFreq
                    if (vec[word] > vec.MaxFreq.Value)
                    {
                        vec.MaxFreq = new KeyValuePair<string, int>(word, vec[word]);
                    }
                }

                // add document to vector collection
                vectors.Add(vec);
            }
        }
    }
}

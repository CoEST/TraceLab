using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TraceLabSDK.Types;

namespace AdvSoftEng.Importers
{
    public static class AnswerMapping
    {
        public static TLSimilarityMatrix Import(String directory)
        {
            TLSimilarityMatrix matrix = new TLSimilarityMatrix();
            /* Each file is a link
             * filename.map - feature
             * Each line in file is a methodID
             */
            foreach (String file in Directory.GetFiles(directory))
            {
                String feature = ExtractFeatureID(file);
                StreamReader links = new StreamReader(file);
                String link;

                while ((link = links.ReadLine()) != null)
                {
                    matrix.AddLink(feature, link, 1);
                }
            }
            return matrix;
        }

        private static String ExtractFeatureID(String file)
        {
            String[] parts = (new FileInfo(file).Name).Split('.');
            if (parts.Length == 1)
            {
                return file;
            }
            String[] copy = new String[parts.Length - 1];
            Array.Copy(parts, copy, parts.Length - 1);
            return String.Join(".", copy);
        }
    }
}

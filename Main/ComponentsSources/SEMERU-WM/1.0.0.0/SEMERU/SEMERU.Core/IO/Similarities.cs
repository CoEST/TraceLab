using System;
using System.IO;
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

namespace SEMERU.Core.IO
{
    /// <summary>
    /// Responsible for TLSimilarityMatrix I/O
    /// </summary>
    public static class Similarities
    {
        /// <summary>
        /// Imports a file in the form (each line):
        /// SOURCE TARGET SCORE
        /// </summary>
        /// <param name="filename">Similarities file</param>
        /// <returns>Similarity matrix</returns>
        public static TLSimilarityMatrix Import(String filename)
        {
            StreamReader file = new StreamReader(filename);
            TLSimilarityMatrix answer = new TLSimilarityMatrix();
            String line;
            int num = 0;
            while ((line = file.ReadLine()) != null)
            {
                num++;
                if (String.IsNullOrWhiteSpace(line))
                    continue;
                try
                {
                    String[] artifacts = line.Split();
                    String source = artifacts[0];
                    String target = artifacts[1];
                    double score = Convert.ToDouble(artifacts[2]);
                    answer.AddLink(source, target, score);
                }
                catch (IndexOutOfRangeException e)
                {
                    file.Close();
                    throw new InvalidDataException("Invalid data format on line " + num + " of file:" + filename, e);
                }
            }
            file.Close();
            return answer;
        }

        /// <summary>
        /// Exports TLSimilarityMatrix to file in the form (each line):
        /// SOURCE TARGET SCORE
        /// </summary>
        /// <param name="matrix">Similarity matrix</param>
        public static void Export(TLSimilarityMatrix matrix, string filename)
        {
            TextWriter file = new StreamWriter(filename);
            foreach (TLSingleLink link in matrix.AllLinks)
            {
                file.WriteLine("{0} {1} {2}", link.SourceArtifactId, link.TargetArtifactId, link.Score);
            }
            file.Flush();
            file.Close();
        }

        public static double AverageSimilarity(TLSimilarityMatrix matrix)
        {
            return AverageSimilarity(matrix.AllLinks);
        }

        public static double AverageSimilarity(TLLinksList list)
        {
            double sum = 0;

            foreach (TLSingleLink link in list)
            {
                sum += link.Score;
            }

            return sum / list.Count;
        }

        public static double SimilarityStandardDeviation(TLSimilarityMatrix matrix)
        {
            return SimilarityStandardDeviation(matrix.AllLinks);
        }

        public static double SimilarityStandardDeviation(TLLinksList list)
        {
            double average = AverageSimilarity(list);
            double sumOfDerivation = 0;

            foreach (TLSingleLink link in list)
            {
                sumOfDerivation += link.Score * link.Score;
            }

            double sumOfDerivationAverage = sumOfDerivation / list.Count;
            return Math.Sqrt(sumOfDerivationAverage - (average * average));
        }

        public static TLSimilarityMatrix CreateMatrix(TLLinksList list)
        {
            TLSimilarityMatrix matrix = new TLSimilarityMatrix();
            foreach (TLSingleLink link in list)
            {
                matrix.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
            }
            return matrix;
        }
    }
}

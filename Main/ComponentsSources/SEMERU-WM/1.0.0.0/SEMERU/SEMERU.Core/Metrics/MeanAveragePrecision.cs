using System;
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

namespace SEMERU.Core.Metrics
{
    /// <summary>
    /// Taken from TraceLab's AveragePrecisionsMetricComponent v0.4.0.0
    /// </summary>
    public static class MeanAveragePrecision
    {
        public static double Compute(TLSimilarityMatrix resultSimilarityMatrix, TLSimilarityMatrix answerMatrix)
        {
            double tmpAveragePrecision = 0.0;
            int totalCountOfTrueLinks = answerMatrix.Count;

            foreach (string sourceArtifact in answerMatrix.SourceArtifactsIds)
            {
                var traceLinks = resultSimilarityMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact);
                tmpAveragePrecision += Calculate(sourceArtifact, traceLinks, answerMatrix);
            }

            double finalAverageAveragePrecision = 0.0;
            if (totalCountOfTrueLinks > 0)
            {
                finalAverageAveragePrecision = tmpAveragePrecision / totalCountOfTrueLinks;
            }

            return finalAverageAveragePrecision;
        }

        private static double Calculate(string sourceArtifactId, TLLinksList resultList, TLSimilarityMatrix answerMatrix)
        {
            resultList.Sort();

            int correct = 0;
            Double totalAvgPrecision = 0.0;
            int totalDocumentsRead = 0;

            foreach (TLSingleLink link in resultList)
            {
                totalDocumentsRead++;
                //check if this is relevant link
                if (answerMatrix.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                {
                    correct++;
                    Double precisionAtCurrentIteration = (double)correct / totalDocumentsRead;
                    totalAvgPrecision += precisionAtCurrentIteration;
                }
            }

            //int numberOfRelevant = answerMatrix.GetCountOfLinksAboveThresholdForSourceArtifact(sourceArtifactId);

            return totalAvgPrecision;
        }
    }
}

using System;
using System.Collections.Generic;
using SEMERU.Core.IO;
using SEMERU.Types.Dataset;
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
    public static class MetricsPerSourceArtifact
    {
        public static DataSetPairs Compute(TLSimilarityMatrix sims, TLSimilarityMatrix oracle, RecallLevel recall)
        {
            TLSimilarityMatrix matrix = Similarities.CreateMatrix(MetricsUtil.GetLinksAtRecall(sims, oracle, recall));
            matrix.Threshold = double.MinValue;
            DataSetPairs pairs = new DataSetPairs();

            foreach (string sourceArtifact in oracle.SourceArtifactsIds)
            {
                TLLinksList links = matrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact);
                links.Sort();
                int totalCorrect = oracle.GetLinksAboveThresholdForSourceArtifact(sourceArtifact).Count;
                int numCorrect = 0;
                int totalRead = 0;
                double totalAvgPrecision = 0.0;
                foreach (TLSingleLink link in links)
                {
                    totalRead++;
                    if (oracle.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                    {
                        numCorrect++;
                        totalAvgPrecision += numCorrect / (double)totalRead;
                    }
                }
                pairs.PrecisionData.Add(new KeyValuePair<string, double>(sourceArtifact, numCorrect / Convert.ToDouble(links.Count)));
                pairs.RecallData.Add(new KeyValuePair<string, double>(sourceArtifact, Convert.ToDouble(numCorrect) / totalCorrect));
                pairs.AveragePrecisionData.Add(new KeyValuePair<string, double>(sourceArtifact, totalAvgPrecision / totalCorrect));
            }

            pairs.MeanAveragePrecisionData.Add(new KeyValuePair<string, double>("#TOTAL", DataSetPairsCollection.CalculateAverage(pairs.AveragePrecisionData)));
            return pairs;
        }
    }
}

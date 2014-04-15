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
    public static class OverallMetricsComputation
    {
        public static DataSetPairsCollection ComputeAll(TLSimilarityMatrix sims, TLSimilarityMatrix oracle)
        {
            DataSetPairsCollection dspc = new DataSetPairsCollection();
            foreach (RecallLevel level in Enum.GetValues(typeof(RecallLevel)))
            {
                dspc.Add(Compute(sims, oracle, level));
            }
            return dspc;
        }

        public static DataSetPairs Compute(TLSimilarityMatrix sims, TLSimilarityMatrix oracle, RecallLevel level)
        {
            TLKeyValuePairsList precision;
            TLKeyValuePairsList recall;
            TLKeyValuePairsList avgPrecision;
            TLKeyValuePairsList meanAvgPrecision;
            ComputeMetrics(sims, oracle, level, out precision, out recall, out avgPrecision, out meanAvgPrecision);
            return new DataSetPairs
            {
                Name = RecallLevelUtil.ShortRecallString(level),
                PrecisionData = precision,
                RecallData = recall,
                AveragePrecisionData = avgPrecision,
                MeanAveragePrecisionData = meanAvgPrecision,
            };
        }

        public static void ComputeMetrics(TLSimilarityMatrix sims, TLSimilarityMatrix oracle, RecallLevel level,
            out TLKeyValuePairsList precision, out TLKeyValuePairsList recall, out TLKeyValuePairsList avgPrecision, out TLKeyValuePairsList meanAvgPrecision)
        {
            TLLinksList links = MetricsUtil.GetLinksAtRecall(sims, oracle, level);
            int numCorrect = 0;
            int totalRead = 0;
            double totalAvgPrecision = 0.0;
            foreach (TLSingleLink link in links)
            {
                totalRead++;
                if (oracle.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                {
                    numCorrect++;
                    totalAvgPrecision += numCorrect / (double) totalRead;
                }
            }
            // temporary
            precision = new TLKeyValuePairsList();
            precision.Add(new KeyValuePair<string, double>("#TOTAL", numCorrect / Convert.ToDouble(links.Count)));
            recall = new TLKeyValuePairsList();
            recall.Add(new KeyValuePair<string, double>("#TOTAL", Math.Ceiling(oracle.Count * RecallLevelUtil.RecallValue(level)) / oracle.Count));
            avgPrecision = new TLKeyValuePairsList();
            avgPrecision.Add(new KeyValuePair<string, double>("#TOTAL", totalAvgPrecision / oracle.Count));
            meanAvgPrecision = new TLKeyValuePairsList();
            meanAvgPrecision.Add(new KeyValuePair<string,double>("#TOTAL", MeanAveragePrecision.Compute(Similarities.CreateMatrix(links), oracle)));
        }
    }
}

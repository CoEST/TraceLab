// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;

namespace MetricComputationEngine
{
    public class AveragePrecision : IMetricComputation
    {
        public SortedDictionary<string, double> Calculate(TLSimilarityMatrix resultMatrix, TLDataset dataset)
        {
            var answerSet = dataset.AnswerSet;
            var sourceArtifacts = dataset.SourceArtifacts;

            SortedDictionary<string, double> metricValues = new SortedDictionary<string, double>();

            foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
            {
                int numberOfRelevant = answerSet.GetCountOfLinksAboveThresholdForSourceArtifact(sourceArtifact.Id); //??
                
                double averagePrecision = 0.0;
                
                //do calculation only if there are relevant links
                if (numberOfRelevant > 0)
                {
                    TLLinksList resultsListForArtifact = resultMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);
                    resultsListForArtifact.Sort();

                    int numRetrieved = 0;
                    int numCorrectlyRetrieved = 0;
                    double sumPrecision = 0;
                    int numSameRankPosition = 1;
                    int sumSameRankPosition = 0;
                    bool hasCorrectlyRetrieved = false;
                    double lastSimilarityScore = -1;
                    foreach (TLSingleLink link in resultsListForArtifact)
                    {
                        numRetrieved++;
                        if (link.Score != lastSimilarityScore)
                        {
                            if (hasCorrectlyRetrieved)
                            {
                                double averageRankPosition = (double)sumSameRankPosition / numSameRankPosition;
                                sumPrecision += (double)numCorrectlyRetrieved / averageRankPosition;
                            }
                            numSameRankPosition = 1;
                            sumSameRankPosition = numRetrieved;
                            hasCorrectlyRetrieved = false;
                        }
                        else
                        {
                            numSameRankPosition++;
                            sumSameRankPosition += numRetrieved;
                        }
                        if (answerSet.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                        {
                            numCorrectlyRetrieved++;
                            hasCorrectlyRetrieved = true;
                        }
                        lastSimilarityScore = link.Score;
                    }
                    if (hasCorrectlyRetrieved)
                    {
                        double averageRankPosition = sumSameRankPosition / numSameRankPosition;
                        sumPrecision += (double)numCorrectlyRetrieved / averageRankPosition;
                    }

                    averagePrecision = (double)sumPrecision / numberOfRelevant;
                    metricValues.Add(sourceArtifact.Id, averagePrecision);
                }
            }

            return metricValues;
        }
    }
}

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
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;

namespace MetricComputationEngine
{
    class Recall : IMetricComputation
    {
        public Recall(double threshold)
        {
            m_threshold = threshold;
        }

        private double m_threshold;

        public SortedDictionary<string, double> Calculate(TLSimilarityMatrix resultMatrix, TLDataset dataset)
        {
            var answerSet = dataset.AnswerSet;
            var sourceArtifacts = dataset.SourceArtifacts;

            SortedDictionary<string, double> metricValues = new SortedDictionary<string, double>();

            resultMatrix.Threshold = m_threshold;

            foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
            {
                int numberOfRelevant = answerSet.GetCountOfLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);

                double recall = 0.0;
                if (numberOfRelevant > 0)
                {
                    TLLinksList resultsListForArtifact = resultMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);
                    resultsListForArtifact.Sort();

                    int numberOfCorrectlyRetrieved = 0;

                    foreach (TLSingleLink link in resultsListForArtifact)
                    {
                        //check if this is relevant link
                        if (answerSet.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                        {
                            numberOfCorrectlyRetrieved++;
                        }
                    }

                    recall = (double)numberOfCorrectlyRetrieved / numberOfRelevant;
                    metricValues.Add(sourceArtifact.Id, recall);
                }
            }

            resultMatrix.Threshold = 0.0;

            return metricValues;
        }
    }
}

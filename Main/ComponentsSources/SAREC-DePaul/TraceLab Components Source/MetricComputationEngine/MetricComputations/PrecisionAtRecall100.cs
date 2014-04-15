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
    public class PrecisionAtRecall100 : IMetricComputation
    {
        public PrecisionAtRecall100()
        {
        }

        public SortedDictionary<string, double> Calculate(TLSimilarityMatrix resultMatrix, TLDataset dataset)
        {
            var answerSet = dataset.AnswerSet;
            var sourceArtifacts = dataset.SourceArtifacts;

            SortedDictionary<string, double> metricValues = new SortedDictionary<string, double>();
            
            foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
            {
                int totalNumberOfCorrectLinks = answerSet.GetCountOfLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);

                double precision = 0.0;
                resultMatrix.Threshold = 0.0;
                TLLinksList resultsListForArtifact = resultMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);
                resultsListForArtifact.Sort();

                int numberOfCorrectlyRetrieved = 0;
                int numberOfRetrieved = 0;
                double scoreOfLastCorrectLink = 0;
                bool foundLastCorrectLink = false;
                foreach (TLSingleLink link in resultsListForArtifact)
                {
                    numberOfRetrieved++;
                    
                    //if all correct links has not been found yet
                    if (foundLastCorrectLink == false)
                    {
                        //check if this is relevant link
                        if (answerSet.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                        {
                            numberOfCorrectlyRetrieved++;
                            if (numberOfCorrectlyRetrieved == totalNumberOfCorrectLinks)
                            {
                                foundLastCorrectLink = true;
                                scoreOfLastCorrectLink = answerSet.GetScoreForLink(link.SourceArtifactId, link.TargetArtifactId);
                            }
                        }
                    } 
                    else if (foundLastCorrectLink)
                    {
                        //if all correct link were found
                        // retrieve all the documents that have the same relevance score as the document with the last correct link
                        double score = answerSet.GetScoreForLink(link.SourceArtifactId, link.TargetArtifactId);
                        if (!score.Equals(scoreOfLastCorrectLink)) break;
                    }
                }

                if (numberOfCorrectlyRetrieved != totalNumberOfCorrectLinks)
                {
                    //if number of correctly retrieved links is not equal once results list was exhausted,
                    //it means there are some links not retrieved with probability zero. the precision is calculated by taking all target documents count 
                    //because then also all documents with probability zero would have to be retrieved
                    precision = (double)totalNumberOfCorrectLinks / dataset.TargetArtifacts.Count;
                    metricValues.Add(sourceArtifact.Id, precision);

                } else if (numberOfRetrieved > 0)
                {
                    precision = (double)numberOfCorrectlyRetrieved / numberOfRetrieved;
                    metricValues.Add(sourceArtifact.Id, precision);
                }
            }
         
            return metricValues;
        }

        public string NameOfMetric
        {
            get { return "Precision At Recall 100%"; }
        }
    }
}

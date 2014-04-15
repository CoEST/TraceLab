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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;
using TraceLabSDK;

namespace MetricComputationEngine
{
    class PrecisionRecallCurveMetricComputation : MetricComputationForSingleDataset<SingleTracingResults>
    {
        private const string MetricName = "Precision Recall Curve";
        private const string MetricDescription = "A precision-recall curve shows a curve that consists of points of precision"+
                                                 " and recall measured after retrieval of each documents.";

        private TraceLabSDK.ComponentLogger m_logger;

        public PrecisionRecallCurveMetricComputation(TraceLabSDK.ComponentLogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");
            m_logger = logger;
        }

        /// <summary>
        /// Computes the specified tracing results.
        /// </summary>
        /// <param name="tracingResults">The tracing results.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        public override Metric Compute(SingleTracingResults tracingResults, TLDataset dataset)
        {
            LineSeries precisionRecallCurve = new LineSeries(MetricName, MetricDescription);

            //only if tracing results are not null... 
            if (tracingResults != null)
            {
                var resultMatrix = tracingResults.ResultMatrix;
                var answerSet = dataset.AnswerSet;
                var sourceArtifacts = dataset.SourceArtifacts;

                TLLinksList resultLinks = resultMatrix.AllLinks;
                resultLinks.Sort();

                int numberOfRelevant = 0;
                foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
                {
                    numberOfRelevant += answerSet.GetCountOfLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);
                }

                //add point only if number of relevant and number of retrieved links are greater than 0
                //basically don't allow division by 0 
                if (numberOfRelevant == 0)
                {
                    m_logger.Warn("Number of relevant links is 0, thus the recall value cannot be computed for the Precision Recall Curve");
                }
                else
                {
                    int numberOfCorrectlyRetrieved = 0;
                    int numberOfRetrieved = 0;

                    foreach (TLSingleLink link in resultLinks)
                    {
                        numberOfRetrieved++;
                        //check if this is a relevant link
                        if (answerSet.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                        {
                            numberOfCorrectlyRetrieved++;
                        }
                        
                        double recall = (double)numberOfCorrectlyRetrieved / numberOfRelevant;

                        //don't need to check if number of retrieved is greater than 0 as it is always the case
                        double precision = (double)numberOfCorrectlyRetrieved / numberOfRetrieved;
                        precisionRecallCurve.AddPoint(new Point(recall, precision));
                    }
                }
            }

            return precisionRecallCurve;
        }
    }
}

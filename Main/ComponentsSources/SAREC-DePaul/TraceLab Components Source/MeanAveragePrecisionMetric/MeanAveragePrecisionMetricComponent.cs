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
using TraceLabSDK;
using TraceLabSDK.Types;

namespace ResultMetrics
{
    [Component(GuidIDString = "6D869F0E-46B8-11E0-9506-4C9ADFD72085",
        Name = "Average average precision Metric Computation",
        DefaultLabel = "Mean Average Precision Metric",
        Description = "This is the component that computes Mean Average Precision of all queries. "+
                      "In other words it computes average value of all average precisions od each source artifact. ",
        Author = "DePaul RE Team",
        Version = "1.0")]

    [IOSpec(IOSpecType.Input, "sourceArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "answerMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "similarityMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "averageAveragePrecision", typeof(double))]

    [Tag("Metrics")]
    public class AveragePrecisionsMetricsComponent : BaseComponent
    {
        public AveragePrecisionsMetricsComponent(ComponentLogger log) : base(log) 
        { 
        }

        public override void Compute()
        {
            TLArtifactsCollection sourceArtifacts = (TLArtifactsCollection)Workspace.Load("sourceArtifacts");
            TLSimilarityMatrix resultSimilarityMatrix = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            TLSimilarityMatrix answerMatrix = (TLSimilarityMatrix)Workspace.Load("answerMatrix");

            double finalAverageAveragePrecision = ComputeMeanAveragePrecision(sourceArtifacts, resultSimilarityMatrix, answerMatrix);

            Workspace.Store("averageAveragePrecision", finalAverageAveragePrecision);
        }

        private static double ComputeMeanAveragePrecision(TLArtifactsCollection sourceArtifacts, TLSimilarityMatrix resultSimilarityMatrix, TLSimilarityMatrix answerMatrix)
        {
            if (sourceArtifacts == null)
            {
                throw new ComponentException("Received null sourceArtifacts");
            }
            if (resultSimilarityMatrix == null)
            {
                throw new ComponentException("Received null similarityMatrix");
            }
            if (answerMatrix == null)
            {
                throw new ComponentException("Received null answerMatrix");
            }

            double tmpAveragePrecision = 0.0;
            int totalCountOfTrueLinks = answerMatrix.Count;

            foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
            {
                var traceLinks = resultSimilarityMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact.Id);

                double intermediateAvgPrec = 0.0;

                intermediateAvgPrec = Calculate(sourceArtifact.Id, traceLinks, answerMatrix);

                tmpAveragePrecision += intermediateAvgPrec;

            }

            double finalAverageAveragePrecision = 0.0;
            if (totalCountOfTrueLinks > 0)
            {
                finalAverageAveragePrecision = tmpAveragePrecision / totalCountOfTrueLinks;
            }
            return finalAverageAveragePrecision;
        }

        public static double Calculate(string sourceArtifactId, TLLinksList resultList, TLSimilarityMatrix answerMatrix)
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

            int numberOfRelevant = answerMatrix.GetCountOfLinksAboveThresholdForSourceArtifact(sourceArtifactId);

            return totalAvgPrecision;
        }
    }
}

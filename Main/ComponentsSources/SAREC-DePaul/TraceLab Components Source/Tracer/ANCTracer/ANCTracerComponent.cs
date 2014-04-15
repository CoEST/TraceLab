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
using TraceLabSDK;
using TraceLabSDK.Types;

namespace Tracer
{
    [Component(GuidIDString = "AB30EAD2-46AC-11E0-8AD8-3A8EDFD72085",
        Name = "ANC Tracer Component",
        DefaultLabel = "ANC Tracer Component",
        Description = "This is the basic tracer engine. It calculates the dot product of the two vectors and delegates " +
                        "the actual scores to the similarity metric method. The available similarity metrics are: " +
                        "Dice, Jaccard, Cosine, and Simple matching. Currently the metric is set to Cosine Similarity method. " +
                        "This searcher assumes that the query has already been pre-processed." +
                        "In addition to using tf-idf dictionary index it also uses American National Corpus",
        Author = "DePaul RE Team",
        Version = "1.0",
        ConfigurationType = typeof(TracerConfig))]
    [IOSpec(IOSpecType.Input, "sourceArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "targetArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "dictionaryIndex", typeof(TraceLabSDK.Types.TLDictionaryIndex))]
    [IOSpec(IOSpecType.Input, "ancTermsWeights", typeof(TraceLabSDK.Types.TLKeyValuePairsList))]
    [IOSpec(IOSpecType.Output, "similarityMatrix", typeof(TraceLabSDK.Types.TLSimilarityMatrix))]
    [Tag("Tracers")]
    public class ANCTracerComponent : BaseComponent
    {
        public ANCTracerComponent(ComponentLogger logger)
            : base(logger)
        {
            this.Configuration = new TracerConfig();
        }

        public override void Compute()
        {
            Logger.Trace("Start component ANCTracerComponent");

            TLArtifactsCollection sourceArtifacts = (TLArtifactsCollection)Workspace.Load("sourceArtifacts");
            TLArtifactsCollection targetArtifacts = (TLArtifactsCollection)Workspace.Load("targetArtifacts");
            TLDictionaryIndex dict = (TLDictionaryIndex)Workspace.Load("dictionaryIndex");
            TLKeyValuePairsList ancTermsWeights = (TLKeyValuePairsList)Workspace.Load("ancTermsWeights");

            TracerConfig config = (TracerConfig)this.Configuration;

            TLSimilarityMatrix similarityMatrix = ComputeTraceability(sourceArtifacts, targetArtifacts, dict, ancTermsWeights, config);

            Workspace.Store("similarityMatrix", similarityMatrix);

            Logger.Trace("Completed component ANCTracerComponent");
        }

        /// <summary>
        /// Computes the traceability between source and target artifacts using dictionary and American Corpus Term weigths.
        /// </summary>
        /// <param name="sourceArtifacts">The source artifacts.</param>
        /// <param name="targetArtifacts">The target artifacts.</param>
        /// <param name="dict">The dict.</param>
        /// <param name="ancTermsWeights">The anc terms weights.</param>
        /// <param name="config">The config.</param>
        /// <returns>Similarity matrix with links between source and target artifacts</returns>
        private static TLSimilarityMatrix ComputeTraceability(TLArtifactsCollection sourceArtifacts, 
                                                              TLArtifactsCollection targetArtifacts, 
                                                              TLDictionaryIndex dict, 
                                                              TLKeyValuePairsList ancTermsWeights, 
                                                              TracerConfig config)
        {
            if (sourceArtifacts == null)
            {
                throw new ComponentException("Received source artifacts are null!");
            }

            if (targetArtifacts == null)
            {
                throw new ComponentException("Received target artifacts are null!");
            }

            if (dict == null)
            {
                throw new ComponentException("Received dictionary index is null!");
            }

            if (ancTermsWeights == null)
            {
                throw new ComponentException("Received 'ancTermsWeights' is null!");
            }

            TLSimilarityMatrix similarityMatrix = new TLSimilarityMatrix();

            
            ANCSearcher searcher = new ANCSearcher(SimilarityMetricFactory.GetSimiliarityMetric(config.SimilarityMetric));

            // Iterates over all the source artifacts to determine the probabilities to target artifacts - by executing a search
            foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
            {

                String query = sourceArtifact.Text;

                // Executes the query
                List<Result> results;
                results = searcher.search(query, dict, PrepareANCData(ancTermsWeights));

                // Iterates over the results and stores them in the matrix
                foreach (Result r in results)
                {
                    string targetArtifactId = r.ArtifactId;
                    similarityMatrix.AddLink(sourceArtifact.Id, targetArtifactId, r.Ranking);
                }
            }
            return similarityMatrix;
        }

        private static Dictionary<string, double> PrepareANCData(TLKeyValuePairsList metricContainer)
        {
            Dictionary<string, double> retCol = new Dictionary<string, double>();

            foreach (KeyValuePair<string, double> metric in metricContainer)
            {
                if (retCol.ContainsKey(metric.Key) == true)
                {
                    if (retCol[metric.Key] < metric.Value)
                    {
                        retCol[metric.Key] = metric.Value;
                    }
                }
                else
                {
                    retCol.Add(metric.Key, metric.Value);
                }
            }

            return retCol;
        }
    }
}

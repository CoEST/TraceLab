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
    [Component(GuidIDString = "AD26CC51-5234-4B1A-ABFA-B631BC0F2382",
            Name = "Tracer Component",
            DefaultLabel = "VSM Tracer Component",
            Description = "This is the basic tracer engine. It calculates the dot product of the two vectors and delegates " +
                            "the actual scores to the similarity metric method. The available similarity metrics are: " +
                            "Dice, Jaccard, Cosine, and Simple matching. Currently the metric is set to Cosine Similarity method. " +
                            "This searcher assumes that the query has already been pre-processed.",
            Author = "DePaul RE Team",
            Version = "1.0",
            ConfigurationType=typeof(TracerConfig))]
    [IOSpec(IOSpecType.Input, "sourceArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection), Description="Source artifacts - queries that are used to search for similar documents in the given dictionary.")]
    [IOSpec(IOSpecType.Input, "dictionaryIndex", typeof(TraceLabSDK.Types.TLDictionaryIndex), Description="Dictionary Index should be build on the target artifacts.")]
    [IOSpec(IOSpecType.Output, "similarityMatrix", typeof(TraceLabSDK.Types.TLSimilarityMatrix), Description="Similarity matrix that contains results from source artifacts to target artifacts.")]
    [Tag("Tracers.TFIDF VSM Helpers")]
    public class TracerComponent : BaseComponent
    {
        public TracerComponent(ComponentLogger logger) : base(logger)
        {
            this.Configuration = new TracerConfig();
        }

        public override void Compute()
        {
            TLArtifactsCollection sourceArtifacts = (TLArtifactsCollection)Workspace.Load("sourceArtifacts");
            TLDictionaryIndex dict = (TLDictionaryIndex)Workspace.Load("dictionaryIndex");
            TracerConfig config = (TracerConfig)this.Configuration;

            TLSimilarityMatrix similarityMatrix = Process(sourceArtifacts, dict, config);

            Workspace.Store("similarityMatrix", similarityMatrix);
        }

        private static TLSimilarityMatrix Process(TLArtifactsCollection sourceArtifacts, TLDictionaryIndex dict, TracerConfig config)
        {
            if (sourceArtifacts == null)
            {
                throw new ComponentException("Received null sourceArtifacts");
            }

            if (dict == null)
            {
                throw new ComponentException("Received null dictionaryIndex");
            }

            TLSimilarityMatrix similarityMatrix = new TLSimilarityMatrix();

            Searcher searcher = new Searcher(SimilarityMetricFactory.GetSimiliarityMetric(config.SimilarityMetric));

            // Iterates over all the source artifacts to determine the probabilities to target artifacts - by executing a search
            foreach (TLArtifact sourceArtifact in sourceArtifacts.Values)
            {

                String query = sourceArtifact.Text;

                // Executes the query
                List<Result> results;
                results = searcher.search(query, dict);

                // Iterates over the results and stores them in the matrix
                foreach (Result r in results)
                {
                    string targetArtifactId = r.ArtifactId;
                    similarityMatrix.AddLink(sourceArtifact.Id, targetArtifactId, r.Ranking);
                }
            }

            return similarityMatrix;
        }
    }
}

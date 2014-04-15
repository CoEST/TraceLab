using System;
using TraceLabSDK;
using TraceLabSDK.Types;
using AdvSoftEng.Types;
using System.Collections.Generic;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "82c70245-5224-4831-9fa3-840489a483de",
                Name = "AdvSoftEng.Components.CosineSimilarityComponent",
                DefaultLabel = "AdvSoftEng - Cosine Similarity",
                Description = "Computes cosine similarity between queries and documents",
                Author = "Evan Moritz",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "Documents", typeof(NormalizedVectorCollection))]
    [IOSpec(IOSpecType.Input, "DocumentLengths", typeof(NormalizedVector))]
    [IOSpec(IOSpecType.Input, "Queries", typeof(DocumentVectorCollection))]
    [IOSpec(IOSpecType.Output, "SimilarityMatrix", typeof(TLSimilarityMatrix))]
    public class CosineSimilarityComponent : BaseComponent
    {
        public CosineSimilarityComponent(ComponentLogger log) : base(log) {  }

        public override void Compute()
        {
            DocumentVectorCollection queries = (DocumentVectorCollection)Workspace.Load("Queries");
            NormalizedVector lengths = (NormalizedVector)Workspace.Load("DocumentLengths");
            NormalizedVectorCollection docs = (NormalizedVectorCollection)Workspace.Load("Documents");
            TLSimilarityMatrix sims = Models.CosineSimilarity.Compute(docs, lengths, queries);
            Workspace.Store("SimilarityMatrix", sims);
        }
    }
}
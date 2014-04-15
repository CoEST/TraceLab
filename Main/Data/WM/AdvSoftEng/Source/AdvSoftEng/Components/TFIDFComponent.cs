using System;
using TraceLabSDK;
using AdvSoftEng.Types;
using TraceLabSDK.Types;
using System.Collections.Generic;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "63058b8a-5f0a-4460-adb3-56b139ac1642",
                Name = "AdvSoftEng.Components.TFIDFTransformationComponent",
                DefaultLabel = "AdvSoftEng - TFIDF Component",
                Description = "Computes tf-idf on a DocumentVectorCollection",
                Author = "Evan Moritz",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "NormalizedVectors", typeof(NormalizedVectorCollection))]
    [IOSpec(IOSpecType.Input, "InverseDocumentFrequencies", typeof(NormalizedVector))]
    [IOSpec(IOSpecType.Output, "WeightedVectors", typeof(NormalizedVectorCollection))]
    public class TFIDFComponent : BaseComponent
    {
        public TFIDFComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            NormalizedVectorCollection tf = (NormalizedVectorCollection)Workspace.Load("NormalizedVectors");
            NormalizedVector idf = (NormalizedVector)Workspace.Load("InverseDocumentFrequencies");
            NormalizedVectorCollection tfidf = Models.TFIDF.Compute(tf, idf);
            Workspace.Store("WeightedVectors", tfidf);
        }
    }
}
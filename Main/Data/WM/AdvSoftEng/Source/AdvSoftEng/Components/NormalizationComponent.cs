using System;
using TraceLabSDK;
using AdvSoftEng.Types;
using System.Collections.Generic;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "0c5bb0a2-f0cd-4cb4-9006-a2c90b9dd600",
                Name = "AdvSoftEng.Components.NormalizationComponent",
                DefaultLabel = "AdvSoftEng - Normalization",
                Description = "Normalizes each document in a DocumentVectorCollection.\nEach frequency is divided by the maximum frequency of a term inside a document.",
                Author = "Evan Moritz",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "DocumentVectors", typeof(DocumentVectorCollection))]
    [IOSpec(IOSpecType.Output, "NormalizedVectors", typeof(NormalizedVectorCollection))]
    [IOSpec(IOSpecType.Output, "NormalizedVectorLengths", typeof(NormalizedVector))]
    public class NormalizationComponent : BaseComponent
    {
        public NormalizationComponent(ComponentLogger log) : base(log) {  }

        public override void Compute()
        {
            DocumentVectorCollection documents = (DocumentVectorCollection)Workspace.Load("DocumentVectors");
            Models.Normalizer norm = new Models.Normalizer(documents);
            Workspace.Store("NormalizedVectors", norm.Vectors);
            Workspace.Store("NormalizedVectorLengths", norm.Lengths);
        }
    }
}
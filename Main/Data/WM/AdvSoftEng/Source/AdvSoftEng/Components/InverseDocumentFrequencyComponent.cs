using System;
using TraceLabSDK;
using AdvSoftEng.Types;
using System.Collections.Generic;
using TraceLabSDK.Types;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "d3d9e709-2448-4ee8-b124-01c5b4f8f94a",
                Name = "AdvSoftEng.Components.InverseDocumentFrequencyComponent",
                DefaultLabel = "AdvSoftEng - IDF Component",
                Description = "Calculates inverse document frequencies",
                Author = "Evan Moritz",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "NumberOfDocuments", typeof(int))]
    [IOSpec(IOSpecType.Input, "DocumentFrequencies", typeof(DocumentVector))]
    [IOSpec(IOSpecType.Output, "InverseDocumentFrequencies", typeof(NormalizedVector))]
    public class InverseDocumentFrequencyComponent : BaseComponent
    {
        public InverseDocumentFrequencyComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            int numDocs = (int)Workspace.Load("NumberOfDocuments");
            DocumentVector df = (DocumentVector)Workspace.Load("DocumentFrequencies");
            NormalizedVector idf = Models.InverseDocumentFrequency.Compute(df, numDocs);
            Workspace.Store("InverseDocumentFrequencies", idf);
        }
    }
}
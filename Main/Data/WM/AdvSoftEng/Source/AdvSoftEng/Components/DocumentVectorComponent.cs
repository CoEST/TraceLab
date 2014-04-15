using System;
using TraceLabSDK;
using AdvSoftEng.Types;
using TraceLabSDK.Types;
using System.Collections.Generic;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "b4193270-7cb9-4814-b7b4-33a6758b3041",
                Name = "AdvSoftEng.Components.DocumentVectorComponent",
                DefaultLabel = "AdvSoftEng - Document Vectorizer",
                Description = "Converts a TLArtifactsCollection into a set of document by term vectors (concurrency matrix)",
                Author = "Evan Moritz",
                Version = "1.0",
                ConfigurationType=typeof(DocumentVectorConfig))]
    [IOSpec(IOSpecType.Input, "ListOfArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "DocumentVectors", typeof(DocumentVectorCollection))]
    [IOSpec(IOSpecType.Output, "DocumentFrequencies", typeof(DocumentVector))]
    public class DocumentVectorComponent : BaseComponent
    {
        private DocumentVectorConfig _config;

        public DocumentVectorComponent(ComponentLogger log) : base(log)
        {
            _config = new DocumentVectorConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLArtifactsCollection artifacts = (TLArtifactsCollection)Workspace.Load("ListOfArtifacts");
            Models.Vectorizer vec = new Models.Vectorizer(artifacts, _config.Representation);
            Workspace.Store("DocumentVectors", vec.Vectors);
            Workspace.Store("DocumentFrequencies", vec.Frequencies);
        }
    }
}
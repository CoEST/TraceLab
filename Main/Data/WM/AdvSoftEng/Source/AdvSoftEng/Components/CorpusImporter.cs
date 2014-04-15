using System;
using TraceLabSDK;
using TraceLabSDK.Types;
using System.IO;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "ca563e24-06f4-4dd0-bc62-544ca1737d4e",
                Name = "AdvSoftEng.Components.CorpusImporter",
                DefaultLabel = "AdvSoftEng - CorpusImporter",
                Description = "Imports a corpus in the format of two separate documents:\ndoc1 - Each line is a method identifier\ndoc2 - Each line is BOW document that corresponds with the method in doc1",
                Author = "Evan Moritz",
                Version = "1.0",
                ConfigurationType=typeof(CorpusImporterConfig))]
    [IOSpec(IOSpecType.Output, "ListOfArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "NumberOfDocuments", typeof(int))]
    public class CorpusImporter : BaseComponent
    {
        private CorpusImporterConfig _config;

        public CorpusImporter(ComponentLogger log) : base(log)
        {
            _config = new CorpusImporterConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLArtifactsCollection artifacts = Importers.Corpus.Import(_config.Identifiers.Absolute, _config.Documents.Absolute);
            Workspace.Store("ListOfArtifacts", artifacts);
            Workspace.Store("NumberOfDocuments", artifacts.Count);
        }
    }
}
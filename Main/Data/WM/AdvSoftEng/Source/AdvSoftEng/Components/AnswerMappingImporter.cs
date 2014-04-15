using System;
using TraceLabSDK;
using TraceLabSDK.Types;
using System.IO;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "6743f551-a058-4b86-b335-764a7d9d4771",
                Name = "AdvSoftEng.Components.AnswerMappingImporter",
                DefaultLabel = "AdvSoftEng - AnswerMappingImporter",
                Description = "Imports a mapping of the gold set",
                Author = "Evan Moritz",
                Version = "1.0",
                ConfigurationType=typeof(AnswerMappingConfig))]
    [IOSpec(IOSpecType.Output, "AnswerMapping", typeof(TLSimilarityMatrix))]
    public class AnswerMappingImporter : BaseComponent
    {
        private AnswerMappingConfig _config;
        public AnswerMappingImporter(ComponentLogger log) : base(log)
        {
            _config = new AnswerMappingConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix matrix = Importers.AnswerMapping.Import(_config.Directory);
            Workspace.Store("AnswerMapping", matrix);
        }
    }
}
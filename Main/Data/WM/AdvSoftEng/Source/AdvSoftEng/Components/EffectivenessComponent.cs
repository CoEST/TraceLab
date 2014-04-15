using System;
using TraceLabSDK;
using TraceLabSDK.Types;
using AdvSoftEng.Exporters;

namespace AdvSoftEng.Components
{
    [Component(GuidIDString = "f75f9452-121f-4619-95cd-1de81fc6d625",
                Name = "AdvSoftEng.Components.EffectivenessComponent",
                DefaultLabel = "AdvSoftEng - Effectiveness",
                Description = "Calculates the effectiveness measures for the feature location technique.",
                Author = "Evan Moritz",
                Version = "1.0",
                ConfigurationType=typeof(EffectivenessConfig))]
    [IOSpec(IOSpecType.Input, "SimilarityMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "GoldSet", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "Queries", typeof(TLArtifactsCollection))]
    public class EffectivenessComponent : BaseComponent
    {
        private EffectivenessConfig _config;

        public EffectivenessComponent(ComponentLogger log) : base(log)
        {
            _config = new EffectivenessConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix sims = (TLSimilarityMatrix)Workspace.Load("SimilarityMatrix");
            TLSimilarityMatrix gold = (TLSimilarityMatrix)Workspace.Load("GoldSet");
            TLArtifactsCollection queries = (TLArtifactsCollection)Workspace.Load("Queries");
            Effectiveness.Export(queries, sims, gold, _config.AllMethodsFile, _config.BestMethodsFile);
        }
    }
}
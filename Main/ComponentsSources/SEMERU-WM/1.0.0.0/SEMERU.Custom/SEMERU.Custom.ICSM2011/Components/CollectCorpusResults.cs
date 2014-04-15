using System;
using TraceLabSDK;

namespace SEMERU.Custom.ICSM2011.Components
{
    [Component(Name = "SEMERU - ICSM'11 - Collect corpus results",
                Description = "",
                Author = "Evan Moritz",
                Version = "1.0")]
    //[IOSpec(IOSpecType.Import, "ListOfResults",
    [Tag("SEMERU - ICSM'11")]
    public class CollectCorpusResults : BaseComponent
    {
        public CollectCorpusResults(ComponentLogger log) : base(log) { }

        public override void Compute()
        {

        }
    }
}
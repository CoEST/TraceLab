using SEMERU.Custom.CSMR2012.Models;
using TraceLabSDK;
using TraceLabSDK.Types;

/// SEMERU Component Library Extension - Custom additions to the SEMERU Component Library
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library Extension.
/// 
/// The SEMERU Component Library Extension is free software: you can redistribute it
/// and/or modify it under the terms of the GNU General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or (at your
/// option) any later version.
/// 
/// The SEMERU Component Library Extension is distributed in the hope that it will
/// be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
/// License for more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library Extension.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Custom.CSMR2012.Components
{
    [Component(Name = "SEMERU - CSMR'13 - UD-CSTI",
        Description = "Performs \"User-Driven Combination of Structural and Textual Information\"",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "Similarities", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "StructuralRelationships", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "DeveloperFeedback", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "UD-CSTI_Similarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Custom.CSMR'13")]
    public class UDCSTIComponent : BaseComponent
    {
        public UDCSTIComponent(ComponentLogger log) : base(log) {  }

        public override void Compute()
        {
            TLSimilarityMatrix sims = (TLSimilarityMatrix)Workspace.Load("Similarities");
            TLSimilarityMatrix relationships = (TLSimilarityMatrix)Workspace.Load("StructuralRelationships");
            TLSimilarityMatrix feedback = (TLSimilarityMatrix)Workspace.Load("DeveloperFeedback");
            Workspace.Store("UD-CSTI_Similarities", UDCSTI.Compute(sims, relationships, feedback));
        }
    }
}

using System.ComponentModel;
using SEMERU.Core.Models;
using TraceLabSDK;
using TraceLabSDK.Types;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Components
{
    [Component(Name = "SEMERU - (Loop) Affine Tranformation",
                Description = "Performs an affine transformation combining two distributions. Imports lambda from Workspace for use in loop.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "LargeExpert", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "SmallExpert", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "Lambda", typeof(double))]
    [IOSpec(IOSpecType.Output, "CombinedSimilarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Tracers")]
    [Tag("Tracers")]
    public class AffineTransformationLoop : BaseComponent
    {
        public AffineTransformationLoop(ComponentLogger log) : base(log)
        {

        }

        public override void Compute()
        {
            TLSimilarityMatrix large = (TLSimilarityMatrix)Workspace.Load("LargeExpert");
            TLSimilarityMatrix small = (TLSimilarityMatrix)Workspace.Load("SmallExpert");
            double lambda = (double)Workspace.Load("Lambda");
            TLSimilarityMatrix combined = Affine.Transform(large, small, lambda);
            Workspace.Store("CombinedSimilarities", combined);
        }
    }
}
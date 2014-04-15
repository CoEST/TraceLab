using System;
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
    [Component(Name = "SEMERU - Affine Tranformation",
                Description = "Performs an affine transformation combining two distributions.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(AffineTransformationConfig))]
    [IOSpec(IOSpecType.Input, "LargeExpert", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "SmallExpert", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Output, "CombinedSimilarities", typeof(TLSimilarityMatrix))]
    [Tag("SEMERU.Tracers")]
    [Tag("Tracers")]
    public class AffineTransformation : BaseComponent
    {
        private AffineTransformationConfig _config;

        public AffineTransformation(ComponentLogger log) : base(log)
        {
            _config = new AffineTransformationConfig();
            Configuration = _config;
        }

        public AffineTransformation(ComponentLogger log, AffineTransformationConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix large = (TLSimilarityMatrix)Workspace.Load("LargeExpert");
            TLSimilarityMatrix small = (TLSimilarityMatrix)Workspace.Load("SmallExpert");
            TLSimilarityMatrix combined = Affine.Transform(large, small, _config.Lambda);
            Workspace.Store("CombinedSimilarities", combined);
        }
    }

    public class AffineTransformationConfig
    {
        private double _lambda;

        [DisplayName("Lambda")]
        [Description("Weight percentage given to LargeExpert")]
        public double Lambda
        {
            get
            {
                return _lambda;
            }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentException("Lambda must be between 0 and 1");
                }
                _lambda = value;
            }
        }
    }
}
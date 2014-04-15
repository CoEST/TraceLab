using System;
using System.Collections.Generic;
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

namespace SEMERU.ICPC12.Tools
{
    public static class SimilaritySplitter
    {
        public static void Split(ref TLSimilarityMatrix original, Dictionary<int, string> qmap, ref TLSimilarityMatrix bugs, ref TLSimilarityMatrix features, ref TLSimilarityMatrix patch)
        {
            foreach (TLSingleLink link in original.AllLinks)
            {
                string feature = qmap[Convert.ToInt32(link.SourceArtifactId)];
                if (feature == Trace.GetFeatureSetType(FeatureSet.Bugs))
                    bugs.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                else if (feature == Trace.GetFeatureSetType(FeatureSet.Features))
                    features.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                else if (feature == Trace.GetFeatureSetType(FeatureSet.Patch))
                    patch.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
            }
        }
    }
}

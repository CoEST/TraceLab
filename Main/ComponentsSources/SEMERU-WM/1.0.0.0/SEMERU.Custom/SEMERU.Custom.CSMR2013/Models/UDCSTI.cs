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

namespace SEMERU.Custom.CSMR2012.Models
{
    public static class UDCSTI
    {
        public static TLSimilarityMatrix Compute(TLSimilarityMatrix sims, TLSimilarityMatrix relationships, TLSimilarityMatrix feedback)
        {
            // new matrix
            TLSimilarityMatrix newMatrix = new TLSimilarityMatrix();
#if UseDelta
            // compute delta
            double delta = SharedUtils.ComputeDelta(sims);
#endif
            // make sure the entire list is sorted
            TLLinksList links = sims.AllLinks;
            links.Sort();
            // end condition
            int correct = 0;
            // iterate over each source-target pair
            while (links.Count > 0 && correct < feedback.Count)
            {
                // get link at top of list
                TLSingleLink link = links[0];
                // check feedback
                if (feedback.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                {
                    correct++;
                    // update related links
                    for (int i = 1; i < links.Count; i++)
                    {
                        if (link.SourceArtifactId.Equals(links[i].SourceArtifactId)
                            && relationships.IsLinkAboveThreshold(link.TargetArtifactId, links[i].TargetArtifactId))
                        {
#if UseDelta
                            links[i].Score += links[i].Score * delta;
#else
                            links[i].Score += links[i].Score * 0.1;
#endif
                        }
                    }
                }
                // remove link
                newMatrix.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                links.RemoveAt(0);
                // reorder links
                links.Sort();
            }
            return newMatrix;
        }
    }
}

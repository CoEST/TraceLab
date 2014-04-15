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

namespace SEMERU.Custom.CSMR2012.Models
{
    public static class OCSTI
    {
        public static TLSimilarityMatrix Compute(TLSimilarityMatrix matrix, TLSimilarityMatrix relationships)
        {
            // create pseudo matrix for easy lookup
            // Dictionary<sourceID, Dictionary<targetID, score>>
            Dictionary<string, Dictionary<string, double>> storage = new Dictionary<string, Dictionary<string, double>>();
            foreach (TLSingleLink link in matrix.AllLinks)
            {
                if (!storage.ContainsKey(link.SourceArtifactId))
                {
                    storage.Add(link.SourceArtifactId, new Dictionary<string, double>());
                }
                storage[link.SourceArtifactId].Add(link.TargetArtifactId, link.Score);
            }
#if UseDelta
            // compute delta
            double delta = SharedUtils.ComputeDelta(matrix);
#endif
            // iterate over every (source, target) pair
            TLLinksList links = matrix.AllLinks;
            links.Sort();
            foreach (TLSingleLink link in links)
            {
                // get the set of target artifacts related to link.TargetArtifactId
                // then update the value of (link.SourceArtifactId, relatedArtifact) by delta
                foreach (string relatedArtifact in relationships.GetSetOfTargetArtifactIdsAboveThresholdForSourceArtifact(link.TargetArtifactId))
                {
#if UseDelta
                    storage[link.SourceArtifactId][relatedArtifact] += storage[link.SourceArtifactId][relatedArtifact] * delta;
#else
                    storage[link.SourceArtifactId][relatedArtifact] += storage[link.SourceArtifactId][relatedArtifact] * 0.1;
#endif
                }
            }
            // build new matrix
            TLLinksList newLinks = new TLLinksList();
            foreach (string source in storage.Keys)
            {
                foreach (string target in storage[source].Keys)
                {
                    newLinks.Add(new TLSingleLink(source, target, storage[source][target]));
                }
            }
            newLinks.Sort();
            TLSimilarityMatrix newMatrix = new TLSimilarityMatrix();
            foreach (TLSingleLink link in newLinks)
            {
                newMatrix.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
            }
            return newMatrix;
        }
    }
}

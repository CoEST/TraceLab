using NUnit.Framework;
using SEMERU.Core.IO;
using SEMERU.Core.Models;
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

namespace SEMERU.Tests.Models
{
    [TestFixture]
    public class VSMTest
    {
        [Test]
        public void ComputeTest()
        {
            string data = @"../../Data/SimpleCorpus.";
            TLArtifactsCollection source = Artifacts.Import(data + "input.source.txt");
            TLArtifactsCollection target = Artifacts.Import(data + "input.target.txt");
            TLSimilarityMatrix testsims = VSM.Compute(source, target);
            TLSimilarityMatrix realsims = Similarities.Import(data + "output.VSM.txt");
            Assert.AreEqual(testsims.Count, realsims.Count);
            TLLinksList testlinks = testsims.AllLinks;
            TLLinksList reallinks = realsims.AllLinks;
            testlinks.Sort();
            reallinks.Sort();
            for (int i = 0; i < reallinks.Count; i++)
            {
                Assert.AreEqual(testlinks[i].SourceArtifactId, reallinks[i].SourceArtifactId);
                Assert.AreEqual(testlinks[i].TargetArtifactId, reallinks[i].TargetArtifactId);
                Assert.AreEqual(testlinks[i].Score, reallinks[i].Score, 0.000000001);
            }
        }
    }
}

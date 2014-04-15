// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracer;
using TraceLabSDK.Types;
using TraceLabSDK;

namespace ComponentTest
{
    [TestClass]
    public class TracerComponentTest : ComponentTest
    {
        new public TracerComponent TestComponent
        {
            get { return (TracerComponent)base.TestComponent; }
            set { base.TestComponent = value; }
        }

        protected override void CreateImporter(TraceLabSDK.ComponentLogger logger)
        {
            TestComponent = new TracerComponent(logger);
        }

        [TestMethod]
        public void EmptySourceArtifactsTest()
        {
            TLArtifactsCollection sourceArtifacts = new TLArtifactsCollection();
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            targetArtifacts.Add(new TLArtifact("id", "text"));
            TLDictionaryIndex dictionary = new TLDictionaryIndex();
            dictionary.AddTermEntry("term", 1, 1, 1);

            Workspace.Store("sourceArtifacts", sourceArtifacts);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", dictionary);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();

            TLSimilarityMatrix simMat = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            if (simMat == null || simMat.Count != 0)
            {
                Assert.Fail("Similarity Matrix should still be created but have nothing in it");
            }
        }

        [TestMethod]
        public void EmptyTargetArtifactsTest()
        {
            TLArtifactsCollection sourceArtifacts = new TLArtifactsCollection();
            sourceArtifacts.Add(new TLArtifact("id", "text"));
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            TLDictionaryIndex dictionary = new TLDictionaryIndex();
            dictionary.AddTermEntry("term", 1, 1, 1);

            Workspace.Store("sourceArtifacts", sourceArtifacts);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", dictionary);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();

            TLSimilarityMatrix simMat = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            if (simMat == null || simMat.Count != 0)
            {
                Assert.Fail("Similarity Matrix should still be created but have nothing in it");
            }
        }

        [TestMethod]
        public void EmptyDictionaryIndexTest()
        {
            TLArtifactsCollection sourceArtifacts = new TLArtifactsCollection();
            sourceArtifacts.Add(new TLArtifact("id", "text"));
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            targetArtifacts.Add(new TLArtifact("id", "text"));
            TLDictionaryIndex dictionary = new TLDictionaryIndex();

            Workspace.Store("sourceArtifacts", sourceArtifacts);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", dictionary);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();

            TLSimilarityMatrix simMat = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            if (simMat == null || simMat.Count != 0)
            {
                Assert.Fail("Similarity Matrix should still be created but have nothing in it");
            }
        }

        [TestMethod]
        public void TestTracingOfComponent()
        {
            TLArtifactsCollection sourceArtifacts = new TLArtifactsCollection();
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            TLDictionaryIndex dictionary = new TLDictionaryIndex();

            // TODO: add inputs that matter
            sourceArtifacts.Add(new TLArtifact("id1", "first text"));
            sourceArtifacts.Add(new TLArtifact("id2", "words to do stuff with"));
            sourceArtifacts.Add(new TLArtifact("id3", "some more text"));

            targetArtifacts.Add(new TLArtifact("id1", "hello world"));
            targetArtifacts.Add(new TLArtifact("id2", "very very random yes indeed"));
            targetArtifacts.Add(new TLArtifact("id3", "yep"));
            targetArtifacts.Add(new TLArtifact("id4", "chickens in the coop"));

            dictionary.AddTermEntry("term", 3, 3, 0.2);

            Workspace.Store("sourceArtifacts", sourceArtifacts);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", dictionary);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();

            TLSimilarityMatrix simMat = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            // TODO: add tests to make sure the output is correctly formatted
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullSourceArtifactsTest()
        {
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            TLDictionaryIndex dictionary = new TLDictionaryIndex();

            Workspace.Store("sourceArtifacts", null);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", dictionary);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();
        }
        
        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullDictionaryIndexTest()
        {
            TLArtifactsCollection sourceArtifacts = new TLArtifactsCollection();
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();

            Workspace.Store("sourceArtifacts", sourceArtifacts);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", null);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void IncorrectInputSourceArtifactsType()
        {
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            targetArtifacts.Add(new TLArtifact("id1", "text2"));
            TLDictionaryIndex dictionary = new TLDictionaryIndex();
            dictionary.AddTermEntry("term", 1,1,1);

            Workspace.Store("sourceArtifacts", "incorrect type");
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", dictionary);

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void IncorrectInputDictonaryType()
        {
            TLArtifactsCollection sourceArtifacts = new TLArtifactsCollection();
            sourceArtifacts.Add(new TLArtifact("id1", "text"));
            TLArtifactsCollection targetArtifacts = new TLArtifactsCollection();
            targetArtifacts.Add(new TLArtifact("id1", "text2"));

            Workspace.Store("sourceArtifacts", sourceArtifacts);
            Workspace.Store("targetArtifacts", targetArtifacts);
            Workspace.Store("dictionaryIndex", "incorrect type");

            ((TracerConfig)TestComponent.Configuration).SimilarityMetric = SimilarityMetricMethod.SimpleMatching;

            TestComponent.Compute();
        }
    }
}

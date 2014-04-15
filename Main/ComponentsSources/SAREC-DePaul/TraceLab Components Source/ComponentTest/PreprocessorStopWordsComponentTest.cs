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
using Preprocessor;
using TraceLabSDK.Types;
using Importer;
using TraceLabSDK;

namespace ComponentTest
{
    [TestClass]
    public class PreprocessorStopWordsComponentTest : ComponentTest
    {
        new public PreprocessorStopWordsComponent TestComponent
        {
            get { return (PreprocessorStopWordsComponent)base.TestComponent; }
            set { base.TestComponent = value; }
        }

        protected override void CreateImporter(TraceLabSDK.ComponentLogger logger)
        {
            TestComponent = new PreprocessorStopWordsComponent(logger);
        }

        [TestMethod]
        public void EmptyArtifactListTest()
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            Workspace.Store("listOfArtifacts", artifacts);

            TLStopwords stopwords = new TLStopwords();
            stopwords.Add("one");
            stopwords.Add("word");
            stopwords.Add("to");
            stopwords.Add("add");
            Workspace.Store("stopwords", stopwords);

            TestComponent.Compute();
        }

        [TestMethod]
        public void EmptyStopWordsListTest()
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            artifacts.Add(new TLArtifact("id", "text 1"));
            artifacts.Add(new TLArtifact("id1", "text two"));
            artifacts.Add(new TLArtifact("id2", "text is three"));
            artifacts.Add(new TLArtifact("id3", "text has a the stop word"));
            Workspace.Store("listOfArtifacts", artifacts);

            TLStopwords stopwords = new TLStopwords();
            Workspace.Store("stopwords", stopwords);

            TestComponent.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullArtifactsTest()
        {
            Workspace.Store("listOfArtifacts", null);

            TLStopwords stopwords = new TLStopwords();
            Workspace.Store("stopwords", stopwords);

            TestComponent.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullStopwordsTest()
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            artifacts.Add(new TLArtifact("id", "word to clean"));
            Workspace.Store("listOfArtifacts", artifacts);

            TLStopwords stopwords = new TLStopwords();
            Workspace.Store("stopwords", null);

            TestComponent.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void IncorrectArtifactsType()
        {
            Workspace.Store("listOfArtifacts", "incorrect type");

            TLStopwords stopwords = new TLStopwords();
            Workspace.Store("stopwords", stopwords);

            TestComponent.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void IncorrectStopwordsType()
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            Workspace.Store("listOfArtifacts", artifacts);

            Workspace.Store("stopwords", "incorrect type");

            TestComponent.Compute();
        }

        [TestMethod]
        public void CleanArtifactsWithStopwords()
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            artifacts.Add(new TLArtifact("id1", "clean these words"));
            artifacts.Add(new TLArtifact("id2", "this has a stopword"));
            artifacts.Add(new TLArtifact("id3", "expression"));
            Workspace.Store("listOfArtifacts", artifacts);

            TLStopwords stopwords = new TLStopwords();
            stopwords.Add("these");
            stopwords.Add("has");
            stopwords.Add("an");
            stopwords.Add("a");
            Workspace.Store("stopwords", stopwords);

            TestComponent.Compute();

            artifacts = (TLArtifactsCollection)Workspace.Load("listOfArtifacts");
            stopwords = (TLStopwords)Workspace.Load("stopwords");

            Assert.AreEqual(artifacts["id1"].Text, "clean words");
            Assert.AreEqual(artifacts["id2"].Text, "this stopword");
            Assert.AreEqual(artifacts["id3"].Text, "expression");
        }
    }
}

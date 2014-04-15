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
using TraceLabSDK;

namespace ComponentTest
{
    [TestClass]
    public class PreprocessorEnglishStemmerComponentTest : ComponentTest
    {
        new public PreprocessorStemmerComponent TestComponent
        {
            get { return (PreprocessorStemmerComponent)base.TestComponent; }
            set { base.TestComponent = value; }
        }

        protected override void CreateImporter(TraceLabSDK.ComponentLogger logger)
        {
            TestComponent = new PreprocessorStemmerComponent(logger);
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullStemTest()
        {
            Workspace.Store("listOfArtifacts", null);
            TestComponent.Compute();
        }

        /// <summary>
        /// Attempts to clean an object from the Workspace that is not of the correct type
        /// for the component to handle.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void IncorrectTypeTest()
        {
            Workspace.Store("listOfArtifacts", "incorrect type");
            TestComponent.Compute();
        }

        [TestMethod]
        public void EmptyCollectionTest()
        {
            TLArtifactsCollection collection = new TLArtifactsCollection();
            Workspace.Store("listOfArtifacts", collection);

            TestComponent.Compute();
        }

        //TODO: fill with better data that will fully test what the component is supposed to do.
        [TestMethod]
        public void CleanListOfArtifacts()
        {
            TLArtifactsCollection collection = new TLArtifactsCollection();
            collection.Add(new TLArtifact("id", "addition"));
            collection.Add(new TLArtifact("id2", "works all arounds"));
            collection.Add(new TLArtifact("id3", "the world is nothing but a huge sphere"));
            Workspace.Store("listOfArtifacts", collection);

            TestComponent.Compute();
            collection = (TLArtifactsCollection)Workspace.Load("listOfArtifacts");

            if (collection["id"].Text != "addit")
            {
                Assert.Fail("id got '" + collection["id"].Text + "' when 'addit' was expected");
            }
            if (collection["id2"].Text != "work all around")
            {
                Assert.Fail("id2 got '" + collection["id2"].Text + "' when 'work all around' was expected");
            }
            if (collection["id3"].Text != "the world is noth but a huge sphere")
            {
                Assert.Fail("id3 got '" + collection["id3"].Text + "' when 'the world is noth but a huge sphere' was expected");
            }
        }

        /// <summary>
        /// Tests rare phrases and awkward values that may be put into the 
        /// pre processor such as ç,Ä,ì
        /// </summary>
        [TestMethod]
        public void RarePhrasesTest()
        {
            TLArtifactsCollection collection = new TLArtifactsCollection();
            collection.Add(new TLArtifact("blank", ""));
            collection.Add(new TLArtifact("space", " "));
            collection.Add(new TLArtifact("accents", "à ì")); // à = 133 ì = 141
            collection.Add(new TLArtifact("unrecognized", "╣")); // the debugger sees this value as a square ascii is 441
            Workspace.Store("listOfArtifacts", collection);
            TestComponent.Compute();

            collection = (TLArtifactsCollection)Workspace.Load("listOfArtifacts");
            if (collection["blank"].Text != "")
            {
                Assert.Fail("blank got '" + collection["blank"].Text + "' when '' was expected");
            }
            if (collection["space"].Text != "")
            {
                Assert.Fail("space got '" + collection["space"].Text + "' when '' was expected");
            }
            if (collection["accents"].Text != "à ì")
            {
                Assert.Fail("accents got '" + collection["accents"].Text + "' when 'à ì' was expected");
            }
            if (collection["unrecognized"].Text != "╣")
            {
                Assert.Fail("unrecognized got '" + collection["unrecognized"].Text + "' when '╣' was expected");
            }
        }
    }
}

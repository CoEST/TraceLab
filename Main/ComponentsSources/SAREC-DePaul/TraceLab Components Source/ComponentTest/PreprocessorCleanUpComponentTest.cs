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
using System.Reflection;
using TraceLabSDK;
using Preprocessor;
using TraceLabSDK.Types;

namespace ComponentTest
{
    /// <summary>
    /// Summary description for PreprocessorCleanUpComponentTest
    /// </summary>
    [TestClass]
    public class PreprocessorCleanUpComponentTest : ComponentTest
    {
        new public PreprocessorCleanUpComponent TestComponent
        {
            get { return (PreprocessorCleanUpComponent)base.TestComponent; }
            set { base.TestComponent = value; }
        }

        protected override void CreateImporter(ComponentLogger logger)
        {
            TestComponent = new PreprocessorCleanUpComponent(logger);
        }

        [TestMethod]
        public void CleanListOfArtifacts()
        {
            TLArtifactsCollection collection = new TLArtifactsCollection();
            collection.Add(new TLArtifact("id", "this is text"));
            collection.Add(new TLArtifact("id2", "this is text"));
            collection.Add(new TLArtifact("id3", "this is more text"));
            Workspace.Store("listOfArtifacts", collection);

            TestComponent.Compute();
            collection = (TLArtifactsCollection) Workspace.Load("listOfArtifacts");

            if (collection["id"].Text != "this is text")
            {
                Assert.Fail("id got '" + collection["id"].Text + "' when 'this is text' was expected");
            }
            if (collection["id2"].Text != "this is text")
            {
                Assert.Fail("id2 got '" + collection["id2"].Text + "' when 'this is text' was expected");
            }
            if (collection["id3"].Text != "this is more text")
            {
                Assert.Fail("id3 got '" + collection["id3"].Text + "' when 'this is more text' was expected");
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
            collection.Add(new TLArtifact("unrecognized", "╣")); // the debugger sees this value as a square, ascii is 441
            Workspace.Store("listOfArtifacts", collection);
            TestComponent.Compute();

            collection = (TLArtifactsCollection) Workspace.Load("listOfArtifacts");
            if (collection["blank"].Text != "")
            {
                Assert.Fail("blank got '" + collection["blank"].Text + "' when '' was expected");
            }
            if (collection["space"].Text != "")
            {
                Assert.Fail("space got '" + collection["space"].Text + "' when '' was expected");
            }
            if (collection["accents"].Text != "")
            {
                Assert.Fail("accents got '" + collection["accents"].Text + "' when 'à ì' was expected");
            }
            if (collection["unrecognized"].Text != "")
            {
                Assert.Fail("unrecognized got '" + collection["unrecognized"].Text + "' when '╣' was expected");
            }
        }

        [TestMethod]
        public void EmptyCollectionTest()
        {
            TLArtifactsCollection collection = new TLArtifactsCollection();
            Workspace.Store("listOfArtifacts", collection);

            TestComponent.Compute();
        }

        /// <summary>
        /// Component tries to clean up a null value returned from the Workspace
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullCleanUpTest()
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
    }
}

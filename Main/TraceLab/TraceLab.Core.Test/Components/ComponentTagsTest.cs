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

using TraceLab.Core.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TraceLab.Core.Test
{
    
    
    /// <summary>
    ///This is a test class for ComponentTagsTest and is intended
    ///to contain all ComponentTagsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ComponentTagsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ComponentTags Constructor
        ///</summary>
        [TestMethod()]
        public void ComponentTagsConstructorTest()
        {
            string definitionId = Guid.NewGuid().ToString();
            ComponentTags target = new ComponentTags(definitionId);
            Assert.AreEqual(definitionId, target.ComponentDefinitionId);
            Assert.AreEqual(0, target.Values.Count);
        }

        /// <summary>
        ///A test for SetTag
        ///</summary>
        [TestMethod()]
        public void SetTagTest()
        {
            string definitionId = Guid.NewGuid().ToString();
            ComponentTags target = new ComponentTags(definitionId);
            string tag = "+Test";
            bool isUserTag = false;
            target.SetTag(tag, isUserTag);

            Assert.AreEqual(1, target.Values.Count);
            Assert.AreEqual("Test", target.Values[0]);

            target.SetTag("-Test", false);
            Assert.AreEqual(0, target.Values.Count);

            target.SetTag("-Test", false);
            Assert.AreEqual(0, target.Values.Count);
        }

        [TestMethod()]
        public void ApplyOverridesTest()
        {
            string definitionId = Guid.NewGuid().ToString();
            ComponentTags baseTag = new ComponentTags(definitionId);

            List<string> tags = new List<string>(new string[]{"+Test", "+Foo", "+Bar"});
            foreach(string tag in tags)
            {
                baseTag.SetTag(tag, false);
            }

            ComponentTags overrideTag = new ComponentTags(definitionId);
            List<string> overrideTags = new List<string>(new string[] { "+AnotherTestValue", "+Foo", "-Foo" });
            foreach (string tag in overrideTags)
            {
                overrideTag.SetTag(tag, false);
            }

            baseTag.ApplyOverrides(overrideTag);
            Assert.AreEqual(3, baseTag.Values.Count);
            Assert.IsTrue(baseTag.Values.Contains("Test"));
            Assert.IsTrue(baseTag.Values.Contains("Bar"));
            Assert.IsTrue(baseTag.Values.Contains("AnotherTestValue"));
            Assert.IsFalse(baseTag.Values.Contains("Foo"));
        }
    }
}

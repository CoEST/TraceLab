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
using System.Xml.Schema;
using System.Xml;
using TraceLab.Core.Test.Setup;
using System.Xml.Serialization;
using System.IO;

namespace TraceLab.Core.Test
{
    /// <summary>
    ///This is a test class for TagValueTest and is intended
    ///to contain all TagValueTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TagValueTest
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

        internal TraceLabTestApplication AppContext
        {
            get;
            set;
        }


        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
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
        ///A test for TagValue Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TagEmptyValueConstructorTest()
        {
            TagValue target = new TagValue(String.Empty, TagType.Additive, false);
        }

        /// <summary>
        ///A test for TagValue Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TagNullValueConstructorTest()
        {
            TagValue target = new TagValue(null, TagType.Additive, false);
        }
        /// <summary>
        ///A test for TagValue Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TagWhitespaceValueConstructorTest()
        {
            TagValue target = new TagValue(" ", TagType.Additive, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyValueTest()
        {
            TagValue target = new TagValue(String.Empty, TagType.Additive, false); 
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void WhitespaceValueTest()
        {
            TagValue target = new TagValue("Test", TagType.Additive, false);
            target.Value = " ";
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void NullValueTest()
        {
            TagValue target = new TagValue(null, TagType.Additive, false);
        }

        /// <summary>
        ///A test for ReadXml
        ///</summary>
        [TestMethod()]
        public void DeserializationVersion1Test()
        {
            string xml = @"<?xml version='1.0' encoding='utf-16'?>
<TagValue Version=""1"">
    <Value>+Test</Value>
</TagValue>";

            var serial = new XmlSerializer(typeof(TagValue));
            var stringReader = new System.IO.StringReader(xml);
            var reader = XmlReader.Create(stringReader);

            var tag = (TagValue)serial.Deserialize(reader);
            Assert.AreEqual(TagType.Additive, tag.Type);
            Assert.AreEqual("Test", tag.Value);
            Assert.AreEqual(false, tag.IsUserTag);

            xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<TagValue Version=""1"">
    <Value>-Test</Value>
</TagValue>";

            stringReader = new System.IO.StringReader(xml);
            reader = XmlReader.Create(stringReader);

            tag = (TagValue)serial.Deserialize(reader);
            Assert.AreEqual(TagType.Subtractive, tag.Type);
            Assert.AreEqual("Test", tag.Value);
            Assert.AreEqual(false, tag.IsUserTag);
        }

        /// <summary>
        ///A test for WriteXml
        ///</summary>
        [TestMethod()]
        public void SerializationCurrentVersion()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            
            string value = "Test";
            TagType type = TagType.Additive; 
            bool isUser = false; 
            TagValue target = new TagValue(value, type, isUser);

            StringWriter stringWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter, settings);
            var serial = new XmlSerializer(typeof(TagValue));

            serial.Serialize(writer, target);

            string xml = @"<?xml version=""1.0"" encoding=""utf-16""?><TagValue Version=""1""><Value>+Test</Value></TagValue>";
            Assert.AreEqual(xml, stringWriter.ToString());
        }

        /// <summary>
        ///A test for IsUserTag
        ///</summary>
        [TestMethod()]
        public void IsUserTagTest()
        {
            TagValue target = new TagValue("Test", TagType.Additive, false); 
            Assert.AreEqual(false, target.IsUserTag);
            target.IsUserTag = true;
            Assert.AreEqual(true, target.IsUserTag);
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        public void CookedValueTest()
        {
            TagValue target = new TagValue("Test", TagType.Additive, false);
            target.SetCookedValue("-Test");
            Assert.AreEqual("Test", target.Value);
            Assert.AreEqual(TagType.Subtractive, target.Type);

            target.SetCookedValue("Test");
            Assert.AreEqual("Test", target.Value);
            Assert.AreEqual(TagType.Additive, target.Type);

            target.SetCookedValue("-Test");
            target.SetCookedValue("+Test");
            Assert.AreEqual("Test", target.Value);
            Assert.AreEqual(TagType.Additive, target.Type);
        }
    }
}

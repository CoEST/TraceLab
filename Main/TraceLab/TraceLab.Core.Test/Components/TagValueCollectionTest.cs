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
using System.Xml.Serialization;
using System.IO;

namespace TraceLab.Core.Test
{
    /// <summary>
    ///This is a test class for TagValueCollectionTest and is intended
    ///to contain all TagValueCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TagValueCollectionTest
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
        ///A test for TagValueCollection Constructor
        ///</summary>
        [TestMethod()]
        public void TagValueCollectionConstructorTest()
        {
            TagValueCollection target = new TagValueCollection();
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        ///A test for GetKeyForItem
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLab.Core.dll")]
        public void GetKeyForItemTest()
        {
            TagValueCollection_Accessor target = new TagValueCollection_Accessor();
            TagValue item = new TagValue("Test", TagType.Additive, false);
            string expected = "Test";
            string actual;
            actual = target.GetKeyForItem(item);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ReadXml
        ///</summary>
        [TestMethod()]
        public void DeserializationTest()
        {
            var serial = new XmlSerializer(typeof(TagValueCollection));
            var stringReader = new System.IO.StringReader(m_1ItemXml);
            var reader = XmlReader.Create(stringReader);

            var tag = (TagValueCollection)serial.Deserialize(reader);
            Assert.AreEqual(1, tag.Count);
            Assert.AreEqual(TagType.Additive, tag[0].Type);
            Assert.AreEqual("Test", tag[0].Value);

            stringReader = new System.IO.StringReader(m_2ItemXml);
            reader = XmlReader.Create(stringReader);

            tag = (TagValueCollection)serial.Deserialize(reader);
            Assert.AreEqual(2, tag.Count);
            Assert.AreEqual(TagType.Additive, tag["Test"].Type);
            Assert.AreEqual("Test", tag["Test"].Value);
            Assert.AreEqual(TagType.Additive, tag["AnotherTestValue"].Type);
            Assert.AreEqual("AnotherTestValue", tag["AnotherTestValue"].Value);
        }

        /// <summary>
        ///A test for WriteXml
        ///</summary>
        [TestMethod()]
        public void WriteXmlTest()
        {
            XmlWriterSettings settings = new XmlWriterSettings();

            string value = "Test";
            TagType type = TagType.Additive;
            bool isUser = false;
            TagValue firstItem = new TagValue(value, type, isUser);

            StringWriter stringWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter, settings);
            var serial = new XmlSerializer(typeof(TagValueCollection));

            var coll = new TagValueCollection();
            coll.Add(firstItem);

            serial.Serialize(writer, coll);
            Assert.AreEqual(m_1ItemXml, stringWriter.ToString());

            coll.Add(new TagValue("AnotherTestValue", TagType.Additive, false));
            stringWriter = new StringWriter();
            writer = XmlWriter.Create(stringWriter, settings);
            serial.Serialize(writer, coll);
            Assert.AreEqual(m_2ItemXml, stringWriter.ToString());
        }

        private const string m_1ItemXml = @"<?xml version=""1.0"" encoding=""utf-16""?><TagValueCollection Version=""1""><TagValue Version=""1""><Value>+Test</Value></TagValue></TagValueCollection>";
        private const string m_2ItemXml = @"<?xml version=""1.0"" encoding=""utf-16""?><TagValueCollection Version=""1""><TagValue Version=""1""><Value>+Test</Value></TagValue><TagValue Version=""1""><Value>+AnotherTestValue</Value></TagValue></TagValueCollection>";
    }
}

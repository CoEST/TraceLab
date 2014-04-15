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

using TraceLab.Core.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TraceLab.Core.Test
{
    /// <summary>
    ///This is a test class for WorkspaceDataTest and is intended
    ///to contain all WorkspaceDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WorkspaceDataTest
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
        ///A test for WorkspaceUnitData Constructor
        ///</summary>
        [TestMethod()]
        public void WorkspaceDataConstructorTest()
        {
            object data = new TraceLab.Core.Test.Workspaces.TestObject(); 
            WorkspaceUnitData target = new WorkspaceUnitData(data);

            Assert.AreEqual(data, target.Data);
        }

        /// <summary>
        ///A test for WorkspaceUnitData Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WorkspaceDataConstructorNullTest()
        {
            object data = null; 
            WorkspaceUnitData target = new WorkspaceUnitData(data);
        }

        /// <summary>
        ///A test for Data
        ///</summary>
        [TestMethod()]
        public void DataTest()
        {
            WorkspaceUnitData target = new WorkspaceUnitData(new TraceLab.Core.Test.Workspaces.TestObject()); // TODO: Initialize to an appropriate value
            object expected = new TraceLab.Core.Test.Workspaces.TestObject();
            object actual = null;
            target.Data = expected;
            actual = target.Data;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsTemporary
        ///</summary>
        [TestMethod()]
        public void IsTemporaryTest()
        {
            WorkspaceUnitData target = new WorkspaceUnitData(new TraceLab.Core.Test.Workspaces.TestObject());
            bool expected = true;
            bool actual;
            target.IsTemporary = expected;
            actual = target.IsTemporary;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.IsTemporary = expected;
            actual = target.IsTemporary;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BinarySerializeTest()
        {
            var data = new TraceLab.Core.Test.Workspaces.TestObject();
            data.Value = "this is a test";
            WorkspaceUnitData target = new WorkspaceUnitData(data);

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var stream = new System.IO.MemoryStream();

            Assert.IsTrue(stream.Length == 0);
            formatter.Serialize(stream, target);
            Assert.IsTrue(stream.Length > 0);
            stream.Position = 0;

            var result = (WorkspaceUnitData)formatter.Deserialize(stream);
            var resultData = (TraceLab.Core.Test.Workspaces.TestObject)result.Data;

            Assert.AreEqual(data.Value, resultData.Value);
        }

        [TestMethod]
        public void RawBinarySerializeTest()
        {
            var data = new TraceLab.Core.Test.Workspaces.TestObject();
            data.Value = "this is a test";
            WorkspaceUnitData target = new WorkspaceUnitData(data);

            var stream = new System.IO.MemoryStream();
            var writer = new System.IO.BinaryWriter(stream);

            Assert.IsTrue(stream.Length == 0);
            target.WriteData(writer);
            Assert.IsTrue(stream.Length > 0);
            stream.Position = 0;


            var result = (WorkspaceUnitData)Activator.CreateInstance(typeof(WorkspaceUnitData), true);
            result.ReadData(new System.IO.BinaryReader(stream));

            var resultData = (TraceLab.Core.Test.Workspaces.TestObject)result.Data;

            Assert.AreEqual(data.Value, resultData.Value);
        }

        [TestMethod]
        public void XmlSerializeTest()
        {
            var data = new TraceLab.Core.Test.Workspaces.TestObject();
            data.Value = "this is a test";
            WorkspaceUnitData target = new WorkspaceUnitData(data);

            var stream = new System.IO.MemoryStream();
            var formatter = new System.Xml.Serialization.XmlSerializer(typeof(WorkspaceUnitData));

            Assert.IsTrue(stream.Length == 0);
            formatter.Serialize(stream, target);
            Assert.IsTrue(stream.Length > 0);
            stream.Position = 0;

            var result = (WorkspaceUnitData)formatter.Deserialize(stream);
            var resultData = (TraceLab.Core.Test.Workspaces.TestObject)result.Data;

            Assert.AreEqual(data.Value, resultData.Value);
        }

        [TestMethod]
        public void SchemaIsNull()
        {
            var data = new TraceLab.Core.Test.Workspaces.TestObject();
            data.Value = "this is a test";
            System.Xml.Serialization.IXmlSerializable target = new WorkspaceUnitData(data);

            Assert.IsNull(target.GetSchema());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InvalidXmlWriterThrows()
        {
            var data = new TraceLab.Core.Test.Workspaces.TestObject();
            data.Value = "this is a test";
            System.Xml.Serialization.IXmlSerializable target = new WorkspaceUnitData(data);
            target.WriteXml(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InvalidXmlReaderThrows()
        {
            var data = new TraceLab.Core.Test.Workspaces.TestObject();
            data.Value = "this is a test";
            System.Xml.Serialization.IXmlSerializable target = new WorkspaceUnitData(data);
            target.ReadXml(null);
        }

        [TestMethod]
        [DeploymentItem(@"TraceLab.Core.Test/Workspaces/WorkspaceDataVersion1.xml")]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadVersion1()
        {
            string data = "this is a test";

            var reader = System.Xml.XmlReader.Create("WorkspaceDataVersion1.xml");
            var formatter = new System.Xml.Serialization.XmlSerializer(typeof(WorkspaceUnitData));
            WorkspaceUnitData workspaceData = null;

            try
            {
                workspaceData = (WorkspaceUnitData)formatter.Deserialize(reader);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    throw e.InnerException;
                else
                    throw;
            }

            Assert.AreEqual(typeof(TraceLab.Core.Test.Workspaces.TestObject), workspaceData.Data.GetType());
            var testData = (TraceLab.Core.Test.Workspaces.TestObject)workspaceData.Data;
            Assert.AreEqual(data, testData.Value);
        }
    }
}

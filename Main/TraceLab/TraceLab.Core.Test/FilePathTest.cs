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

using TraceLabSDK.Component.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace TraceLab.Core.Test
{
    
    
    /// <summary>
    ///This is a test class for FilePathTest and is intended
    ///to contain all FilePathTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilePathTest
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
        ///A test for FilePath Constructor
        ///</summary>
        [TestMethod()]
        public void DataRootDoesNotHaveTrailingSlash()
        {
            string absolutePath = @"C:\Foo\Stuff.xml"; // TODO: Initialize to an appropriate value
            string dataRoot = @"C:\Foo";
            FilePath target = new FilePath();
            target.Init(absolutePath, dataRoot);

            // Internally, DataRoot needs a directory separator appended to it.
            Assert.AreEqual(dataRoot + System.IO.Path.DirectorySeparatorChar, target.DataRoot);
            Assert.AreEqual(absolutePath, target.Absolute);
            Assert.AreEqual(absolutePath.Remove(0, dataRoot.Length + 1), target.Relative);
        }

        /// <summary>
        /// Tests whether the trailing slash of the data root is ignored.
        /// </summary>
        [TestMethod()]
        public void DataRootDoesHaveTrailingSlash()
        {
            string absolutePath = @"C:\Foo\Stuff.xml"; // TODO: Initialize to an appropriate value
            string dataRoot = @"C:\Foo\";
            FilePath target = new FilePath();
            target.Init(absolutePath, dataRoot);
            Assert.AreEqual(dataRoot, target.DataRoot);
            Assert.AreEqual(absolutePath, target.Absolute);
            Assert.AreEqual(absolutePath.Remove(0, dataRoot.Length), target.Relative);
        }

        /// <summary>
        /// Tests whether the trailing slash of the data root is ignored.
        /// </summary>
        [TestMethod()]
        public void CanMixAndMatchDirectorySeperators()
        {
            string absolutePath = @"C:\Foo\Stuff.xml";
            string dataRoot = @"C:\Foo/";
            string realRoot = dataRoot.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            FilePath target = new FilePath();
            target.Init(absolutePath, dataRoot);
            Assert.AreEqual(realRoot, target.DataRoot);
            Assert.AreEqual(absolutePath, target.Absolute);
            Assert.AreEqual(absolutePath.Remove(0, realRoot.Length), target.Relative);
        }



        /// <summary>
        ///A test for FilePath Constructor
        ///</summary>
        [TestMethod()]
        public void FilePathConstructorTest1()
        {
            string absolutePath = @"C:\Foo\Stuff.xml";
            FilePath target = new FilePath();
            target.Init(absolutePath);
            Assert.AreEqual(absolutePath, target.Absolute);
            Assert.AreEqual(null, target.Relative);
            Assert.AreEqual(null, target.DataRoot);
        }

        /// <summary>
        ///A test for FilePath Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        public void FilePathConstructorTest2()
        {
            FilePath_Accessor target = new FilePath_Accessor();
            Assert.AreEqual(null, target.DataRoot);
            Assert.AreEqual(null, target.Absolute);
            Assert.AreEqual(null, target.Relative);
        }

        /// <summary>
        ///A test for FilePath Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        public void DataRootSetsRelativePathIfAbsoluteIsAlreadySet()
        {
            FilePath_Accessor target = new FilePath_Accessor();

            string absolute = @"C:\Foo\Stuff.xml";
            string dataRoot = @"C:\Foo";
            string relative = @"Stuff.xml";
            target.Absolute = absolute;
            target.SetDataRoot(dataRoot, true);

            Assert.AreEqual(relative, target.Relative);
        }

        /// <summary>
        ///A test for FilePath Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        public void DataRootSetsAbsolutePathIfRelativeIsAlreadySet()
        {
            FilePath_Accessor target = new FilePath_Accessor();

            string absolute = @"C:\Foo\Stuff.xml";
            string dataRoot = @"C:\Foo";
            string relative = @"Stuff.xml";
            target.Relative = relative;
            target.SetDataRoot(dataRoot, true);

            Assert.AreEqual(absolute, target.Absolute);
        }

        /// <summary>
        ///A test for ReadCurrentVersion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        [DeploymentItem(@"TraceLab.Core.Test\TestResources\Version2FilePath.xml")]
        public void ReadCurrentVersionTest()
        {
            FilePath_Accessor target = new FilePath_Accessor(); // TODO: Initialize to an appropriate value

            using (var reader = System.Xml.XmlReader.Create("Version2FilePath.xml"))
            {
                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();
                target.ReadCurrentVersion(nav);
            }

            Assert.AreEqual(@"Stuff.xml", target.Relative);
            Assert.AreEqual(null, target.Absolute);
            Assert.AreEqual(null, target.DataRoot);
        }

        /// <summary>
        ///A test for ReadNonVersioned
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        [DeploymentItem(@"TraceLab.Core.Test\TestResources\NonVersionedFilePath.xml")]
        public void ReadNonVersionedTest()
        {
            FilePath_Accessor target = new FilePath_Accessor(); // TODO: Initialize to an appropriate value

            using (var reader = System.Xml.XmlReader.Create("NonVersionedFilePath.xml"))
            {
                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();
                target.ReadNonVersioned(nav);
            }

            Assert.AreEqual(@"C:\Foo\Stuff.xml", target.Absolute);
            Assert.AreEqual(null, target.Relative);
            Assert.AreEqual(null, target.DataRoot);
        }

        /// <summary>
        ///A test for System.Xml.Serialization.IXmlSerializable.GetSchema
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        public void GetSchemaTest()
        {
            IXmlSerializable target = new FilePath_Accessor();
            XmlSchema expected = null;
            XmlSchema actual;
            actual = target.GetSchema();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.Xml.Serialization.IXmlSerializable.ReadXml
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        [DeploymentItem(@"TraceLab.Core.Test\TestResources\NonVersionedFilePath.xml")]
        [DeploymentItem(@"TraceLab.Core.Test\TestResources\Version2FilePath.xml")]
        public void ReadXmlTest()
        {
            var formatter = new System.Xml.Serialization.XmlSerializer(typeof(FilePath));
            FilePath filePath = null;
            using (var reader = System.Xml.XmlReader.Create("NonVersionedFilePath.xml"))
            {
                filePath = (FilePath)formatter.Deserialize(reader);
            }

            Assert.AreEqual(@"C:\Foo\Stuff.xml", filePath.Absolute);
            Assert.AreEqual(null, filePath.Relative);
            Assert.AreEqual(null, filePath.DataRoot);

            using (var reader = System.Xml.XmlReader.Create("Version2FilePath.xml"))
            {
                filePath = (FilePath)formatter.Deserialize(reader);
            }

            Assert.AreEqual(@"Stuff.xml", filePath.Relative);
            Assert.AreEqual(null, filePath.Absolute);
            Assert.AreEqual(null, filePath.DataRoot);
        }

        /// <summary>
        ///A test for System.Xml.Serialization.IXmlSerializable.WriteXml
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TraceLabSDK.dll")]
        [DeploymentItem(@"TraceLab.Core.Test\TestResources\Version2FilePath.xml")]
        public void WriteXmlTest()
        {
            string absolute = @"C:\Foo\Stuff.xml";
            string dataRoot = @"C:\Foo";
            FilePath path = new FilePath();
            path.Init(absolute, dataRoot);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(@"FilePathXmlWriterTest.xml", settings))
            {
                var formatter = new System.Xml.Serialization.XmlSerializer(typeof(FilePath));
                formatter.Serialize(writer, path);
                writer.Close();
            }

            using (System.IO.FileStream stream1 = new System.IO.FileStream("Version2FilePath.xml", System.IO.FileMode.Open), stream2 = new System.IO.FileStream("FilePathXmlWriterTest.xml", System.IO.FileMode.Open))
            {
                //Assert.AreEqual(stream1.Length, stream2.Length);
                string expected;
                System.IO.StreamReader reader = new System.IO.StreamReader(stream1);
                expected = reader.ReadToEnd();

                reader = new System.IO.StreamReader(stream2);
                string actual = reader.ReadToEnd();

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        public void op_ImplicitTest()
        {
            string expected = @"C:\Foo\Stuff";
            FilePath path = new FilePath();
            path.Init(expected);
            string actual;
            actual = path;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestDetermineRelative1()
        {
            string root = @"C:\packageMain\";
            string file = @"C:\stuff.txt";

            FilePath path = new FilePath();
            path.Init(file, root);

            string expectedRelative = @"..\stuff.txt";

            Assert.AreEqual(expectedRelative, path.Relative);
        }

        [TestMethod()]
        public void TestDetermineRelative2()
        {
            string root = @"C:\packageMain\";
            string file = @"C:\packageMain\foo\stuff.txt";

            FilePath path = new FilePath();
            path.Init(file, root);

            string expectedRelative = @"foo\stuff.txt";

            Assert.AreEqual(expectedRelative, path.Relative);
        }

        [TestMethod()]
        public void TestDetermineRelative3()
        {
            string root = @"C:\packageMain\";
            string file = @"C:\foo\stuff.txt";

            FilePath path = new FilePath();
            path.Init(file, root);

            string expectedRelative = @"..\foo\stuff.txt";

            Assert.AreEqual(expectedRelative, path.Relative);
        }


        [TestMethod()]
        public void TestChangeDataRoot()
        {
            string root = @"C:\packageMain\";
            string file = @"C:\stuff.txt";

            FilePath path = new FilePath();
            path.Init(file, root);
            string expectedRelative = @"..\stuff.txt";
            Assert.AreEqual(expectedRelative, path.Relative);

            //change data root
            string newRoot = @"C:\packageMain\foo";
            path.SetDataRoot(newRoot, true);
            //it should update relative
            expectedRelative = @"..\..\stuff.txt";
            Assert.AreEqual(expectedRelative, path.Relative);
        }
    }
}

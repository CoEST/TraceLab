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

using InstallerActions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using System.Xml;

namespace InstallerActions.Test
{
    /// <summary>
    ///This is a test class for LayoutUpgradesTest and is intended
    ///to contain all LayoutUpgradesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LayoutUpgradesTest
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
        ///A test for UpgradeLayoutTo0320
        ///</summary>
        [TestMethod()]
        [DeploymentItem("InstallerActions.dll")]
        public void UpgradeLayoutTo0320Test()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var pre0320 = assembly.GetManifestResourceStream("InstallerActions.Test.LayoutFiles.0000.xml");
            var post0320 = assembly.GetManifestResourceStream("InstallerActions.Test.LayoutFiles.0320.xml");

            Assert.IsNotNull(pre0320);
            Assert.IsNotNull(post0320);

            MemoryStream pre0320MemStream = new MemoryStream();
            pre0320MemStream.SetLength(pre0320.Length);
            pre0320.Read(pre0320MemStream.GetBuffer(), 0, (int)pre0320MemStream.Length);

            XPathDocument pre0320Document;
            using (XmlReader reader = XmlReader.Create(pre0320MemStream))
            {
                pre0320Document = new XPathDocument(reader);
            }
            pre0320MemStream.Position = 0;
            Assert.IsNull(pre0320Document.CreateNavigator().SelectSingleNode("//DocumentPane/@ShowHeader"));

            MemoryStream post0320MemStream = new MemoryStream();
            post0320MemStream.SetLength(post0320.Length);
            post0320.Read(post0320MemStream.GetBuffer(), 0, (int)post0320MemStream.Length);

            LayoutUpgrades_Accessor.UpgradeLayoutTo0320(pre0320MemStream);

            {
                StreamReader strmreader = new StreamReader(pre0320MemStream);
                string str = strmreader.ReadToEnd();
                pre0320MemStream.Position = 0;

                System.Console.Write(str);
                StringReader strReader = new StringReader(str);
                using (XmlReader reader = XmlReader.Create(strReader))
                {
                    pre0320MemStream.Position = 0;
                    XPathDocument upgraded0320Document = new XPathDocument(reader);

                    Assert.IsNotNull(upgraded0320Document.CreateNavigator().Select("//DocumentPane/@ShowHeader"));
                }
            }

        }
    }
}

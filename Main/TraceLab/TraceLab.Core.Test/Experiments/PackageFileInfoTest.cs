using TraceLab.Core.Experiments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TraceLabSDK.Component.Config;
using System.Collections.Generic;

namespace TraceLab.Core.Test
{
    /// <summary>
    ///This is a test class for PackageFileInfoTest and is intended
    ///to contain all PackageFileInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PackageFileInfoTest
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

        /// <summary>
        ///A test for PackageFileInfo Constructor
        ///</summary>
        [TestMethod()]
        public void PackageFileInfoDetermineRelativePathToSubDir_Test1()
        {
            //case 1. Relative path stays the same 
            LinkedList<string> folders = new LinkedList<string>();
            string input = @"Input\A.xml";
            string output = PackageFileInfo_Accessor.DetermineRelativePathToSubDir(input, out folders);
            Assert.AreEqual(@"Input\A.xml", output);
            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual("Input", folders.First.Value); 
        }


        /// <summary>
        ///A test for PackageFileInfo Constructor
        ///</summary>
        [TestMethod()]
        public void PackageFileInfoDetermineRelativePathToSubDir_Test2()
        {
            //case 2. Removes '..\'
            LinkedList<string> folders = new LinkedList<string>();
            string input = @"..\Input\A.xml";
            string output = PackageFileInfo_Accessor.DetermineRelativePathToSubDir(input, out folders);
            Assert.AreEqual(@"Input\A.xml", output);
            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual("Input", folders.First.Value);
        }


        /// <summary>
        ///A test for PackageFileInfo Constructor
        ///</summary>
        [TestMethod()]
        public void PackageFileInfoDetermineRelativePathToSubDir_Test3()
        {
            //case 3. Removes '..\..\'
            LinkedList<string> folders = new LinkedList<string>();
            string input = @"..\..\A.xml";
            string output = PackageFileInfo_Accessor.DetermineRelativePathToSubDir(input, out folders);
            Assert.AreEqual(@"A.xml", output);
            Assert.AreEqual(0, folders.Count);
        }
    }
}

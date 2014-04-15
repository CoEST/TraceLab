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

using TraceLab.Core.PackageSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TraceLab.Core.Test.PackageSystem;
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.Test
{
    /// <summary>
    ///This is a test class for PackageFileTest and is intended
    ///to contain all PackageFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PackageFileTest
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

        private static IPackage TempPackage
        {
            get;
            set;
        }

        private static string TempFile
        {
            get;
            set;
        }

        private static string TempFileRelative
        {
            get;
            set;
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            string temp = System.IO.Path.GetTempPath();
            temp = System.IO.Path.Combine(temp, "PackageFileTestTemp");
            System.IO.Directory.CreateDirectory(temp);

            string filename = "TestFile.txt";
            TempFileRelative = filename;
            TempFile = System.IO.Path.Combine(temp, filename);

            TempPackage = new MockPackage(temp);

            using (var stream = new System.IO.StreamWriter(TempFile))
            {
                stream.WriteLine("New file FTW!");
            }
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            System.IO.File.Delete(TempFile);
            System.IO.Directory.Delete(TempPackage.Location, true);
            TempPackage = null;
        }
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
        ///A test for PackageFile Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void PackageFileConstructorNullPackage()
        {
            IPackage ownerPackage = null;
            string path = "sfjahsd/dsfe.data";
            PackageFile target = new PackageFile(ownerPackage, path);
            Assert.Fail("A ArgumentNullException exception should have been thrown");
        }

        /// <summary>
        ///A test for PackageFile Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void PackageFileConstructorNullPath()
        {
            IPackage ownerPackage = new MockPackage("someLocation");
            string path = string.Empty;
            PackageFile target = new PackageFile(ownerPackage, path);
            Assert.Fail("A ArgumentNullException exception should have been thrown");
        }

        /// <summary>
        ///A test for PackageFile Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void PackageFileConstructorFileDoesNotExist()
        {
            IPackage ownerPackage = TempPackage; // TODO: Initialize to an appropriate value
            string path = "fdsjhfsadjk.fdf"; // TODO: Initialize to an appropriate value
            PackageFile target = new PackageFile(ownerPackage, path);
            Assert.Fail("A FileNotFound exception should have been thrown");
        }

        /// <summary>
        ///A test for PackageFile Constructor
        ///</summary>
        [TestMethod()]
        public void PackageFileConstructorTest()
        {
            IPackage ownerPackage = TempPackage;
            string path = TempFileRelative; // TODO: Initialize to an appropriate value
            PackageFile target = new PackageFile(ownerPackage, path);
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.ID), "Created file should have an ID");
            Assert.AreEqual(path, target.Path, "Path does not equal what was used to create the file");
            Assert.IsFalse(target.IsCompressed, "Created file should not be compressed");
            Assert.AreNotEqual(Int64.MinValue, target.UncompressedSize, "File must have a non-negative size");
            Assert.AreEqual(Int64.MinValue, target.CompressedSize, "Compressed size isn't valid, as the file hasn't been compressed yet.");
            Assert.IsNotNull(target.UncompressedHash, "Uncompressed has should have been computed");
            Assert.IsNull(target.CompressedHash, "Compressed hash shouldn't have been computed");
            Assert.IsNull(target.Data, "Data shouldn't be valid yet.");
        }

        /// <summary>
        ///A test for PackageFile Constructor
        ///</summary>
        [TestMethod()]
        public void PackageFileConstructorTest1()
        {
            Guid id = Guid.NewGuid();
            IPackage ownerPackage = TempPackage;
            string path = TempFileRelative; // TODO: Initialize to an appropriate value
            long uncompSize = 500;
            long compSize = 156;
            string uncomHash = "AAAFC";
            string compHash = "FFCAB";
            string data = "FFgK8sfee";

            PackageFile target = new PackageFile(ownerPackage, path, id, uncompSize, uncomHash, compSize, compHash, data);
            Assert.AreEqual(id.ToString("D"), target.ID, "Created file should have the ID it was created with.");
            Assert.AreEqual(path, target.Path, "Path does not equal what was used to create the file");
            Assert.AreEqual(true, target.IsCompressed, "Created with compressed data, file should be compressed");
            Assert.AreEqual(uncompSize, target.UncompressedSize, "File must have a non-negative size");
            Assert.AreEqual(compSize, target.CompressedSize, "Compressed size was passed in.");
            Assert.AreEqual(uncomHash, target.UncompressedHash, "Uncompressed hash was passed in");
            Assert.AreEqual(compHash, target.CompressedHash, "Compressed hash was passed in");
            Assert.AreEqual(data, target.Data, "Data shouldn't be valid yet.");
        }

        /// <summary>
        ///A test for PackFile
        ///</summary>
        [TestMethod()]
        public void PackAndUnpackFileTest()
        {
            // TODO: Pack file, unpack file, verify file is same.
            IPackage ownerPackage = TempPackage;
            string path = TempFileRelative; // TODO: Initialize to an appropriate value
            PackageFile target = new PackageFile(ownerPackage, path);

            target.PackFile();
            Assert.IsTrue(target.IsCompressed);
            Assert.IsNotNull(target.CompressedHash, "Compressed hash should have been computed");
            Assert.IsNotNull(target.Data, "Data should be valid by now.");
            Assert.AreNotEqual(Int64.MinValue, target.CompressedSize, "Compressed size should be valid, since the file has been compressed.");

            System.IO.File.Delete(TempFile + ".bak");
            System.IO.File.Copy(TempFile, TempFile + ".bak");
            try
            {
                System.IO.File.Delete(TempFile);
                Assert.IsFalse(System.IO.File.Exists(TempFile));
                target.Unpack();
                Assert.IsTrue(System.IO.File.Exists(TempFile));
                var backup = System.IO.File.ReadAllBytes(TempFile + ".bak");
                var unpacked = System.IO.File.ReadAllBytes(TempFile);

                Assert.IsTrue(backup.SequenceEqual(unpacked));

                Assert.IsFalse(target.IsCompressed, "File has been unpacked.");
                Assert.IsNotNull(target.CompressedHash, "Compressed hash should have been computed");
                Assert.AreNotEqual(Int64.MinValue, target.CompressedSize, "Compressed size should be valid, since the file has been compressed at least once.");
                Assert.IsNull(target.Data, "Data is no longer valid - file has been dumped to disk.");

            }
            finally
            {
                System.IO.File.Delete(TempFile + ".bak");
            }
        }
    }
}

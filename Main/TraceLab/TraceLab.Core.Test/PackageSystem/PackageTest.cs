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
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace TraceLab.Core.Test
{


    /// <summary>
    ///This is a test class for PackageTest and is intended
    ///to contain all PackageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PackageTest
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

        public static string PackageTestRoot
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
            string packageMain = System.IO.Path.GetTempPath();
            packageMain = System.IO.Path.Combine(packageMain, "PackageTestTemp");
            PackageTestRoot = packageMain;

            WriteTestPackage("TestPackage");
        }

        private static string WriteTestPackage(string packageName)
        {
            var packageMain = System.IO.Path.Combine(PackageTestRoot, packageName);

            string components = System.IO.Path.Combine(packageMain, "Components");
            string data = System.IO.Path.Combine(packageMain, "Data");
            string types = System.IO.Path.Combine(packageMain, "Types");
            System.IO.Directory.CreateDirectory(packageMain);
            System.IO.Directory.CreateDirectory(data);
            System.IO.Directory.CreateDirectory(components);
            System.IO.Directory.CreateDirectory(types);

            var manifestFile = System.IO.Path.Combine(packageMain, "TestPackage.manifest");
            WriteFile(manifestFile, "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.TestPackage.manifest.xml");
            WriteFile(System.IO.Path.Combine(components, "Importer.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Components.Importer.dll");
            WriteFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Types.DictionaryTermWeights.dll");
            WriteFile(System.IO.Path.Combine(data, "coest.xml"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Data.coest.xml");
            WriteFile(System.IO.Path.Combine(data, "coest1.xml"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Data.coest1.xml");
            WriteFile(System.IO.Path.Combine(data, "randomfile.something"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Data.randomfile.something");
            WriteFile(System.IO.Path.Combine(packageMain, "somerandomfile.xml"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.somerandomfile.xml");

            var newManifestFile = System.IO.Path.Combine(packageMain, packageName + ".manifest");
            // Update the manifest to reflect the given name.
            System.IO.File.Move(manifestFile, newManifestFile);

            // Change any references to the default package name in the manifest file to the input name.
            var manifestData = System.IO.File.ReadAllText(newManifestFile);
            manifestData = manifestData.Replace("TestPackage", packageName);
            System.IO.File.WriteAllText(newManifestFile, manifestData);

            return packageMain;
        }

        private static void WriteFile(string filePath, string source)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (var manifestFile = System.IO.File.OpenWrite(filePath))
            {
                using (var manifestSource = assembly.GetManifestResourceStream(source))
                {
                    manifestSource.CopyTo(manifestFile);
                }
            }
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            System.IO.Directory.Delete(PackageTestRoot, true);
        }
        
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
        ///A test for Package Constructor
        ///</summary>
        [TestMethod()]
        public void PackageConstructorTest()
        {
            string packageName = "AnEmptyPackage";
            string packageDirectory = System.IO.Path.Combine(PackageTestRoot, packageName);
            System.IO.Directory.CreateDirectory(packageDirectory);
            Package target = new Package(packageDirectory, false);

            Guid id;
            Assert.IsTrue(Guid.TryParse(target.ID, out id));
            Assert.AreEqual(packageName, target.Name);
            Assert.AreEqual(System.IO.Path.Combine(PackageTestRoot, packageName), target.Location);
            Assert.AreEqual(0, target.Files.Count());
            Assert.AreEqual(0, target.References.Count());
            Assert.AreEqual(0, target.TypeLocations.Count());
            Assert.AreEqual(0, target.ComponentLocations.Count());
        }

        /// <summary>
        ///A test for loading the information for an existing package.
        ///</summary>
        [TestMethod()]
        public void PackageLoadTest()
        {
            string packageName = "TestPackage";
            string packageDirectory = System.IO.Path.Combine(PackageTestRoot, packageName);
            Package target = new Package(packageDirectory, true);

            Assert.AreEqual("A939607B-7159-4466-88C4-A869044F81C6", target.ID);
            Assert.AreEqual("TestPackage", target.Name);

            Assert.AreEqual(2, target.References.Count());
            Assert.AreEqual("1886E5F6-27CA-4D42-AB51-7B7BBD9E97C1", target.References.ElementAt(0).ID);
            Assert.AreEqual("E6D01E22-7C9E-4B75-B7C3-6FC8B33A9593", target.References.ElementAt(1).ID);
            Assert.AreEqual(1, target.ComponentLocations.Count());
            Assert.AreEqual("./Components", target.ComponentLocations.ElementAt(0));
            Assert.AreEqual(1, target.TypeLocations.Count());
            Assert.AreEqual("./Types", target.TypeLocations.ElementAt(0));

            Assert.AreEqual(6, target.Files.Count());
            foreach (PackageFile file in target.Files)
            {
                Assert.AreEqual(target, file.Owner);

                string filePath = target.GetAbsolutePath(file);
                using (System.IO.FileStream reader = System.IO.File.OpenRead(filePath))
                {
                    Assert.AreEqual(reader.Length, file.UncompressedSize, "File does not match expected uncompressed size: " + file.Path);
                    Assert.AreEqual(Package.ComputeHash(reader), file.UncompressedHash, "File does not match expected uncompressed hash: " + file.Path);
                    reader.Seek(0, System.IO.SeekOrigin.Begin);

                    // We can't validate the specific size of the compressed data or the hash of the compressed data
                    // because the compression algorithm can change based on the runtime.  And we don't need to validate the round-trip since
                    // PackageFile already tests that.
                }
            }
        }

        [TestMethod()]
        public void AddFileTest()
        {
            string packageName = "AddFileTestPackage";
            string packageDirectory = System.IO.Path.Combine(PackageTestRoot, packageName);
            System.IO.Directory.CreateDirectory(packageDirectory);
            Package target = new Package(packageDirectory, false);

            string components = System.IO.Path.Combine(target.Location, "Components");
            string types = System.IO.Path.Combine(target.Location, "Types");
            string data = System.IO.Path.Combine(target.Location, "Files");

            System.IO.Directory.CreateDirectory(components);
            System.IO.Directory.CreateDirectory(types);
            System.IO.Directory.CreateDirectory(data);

            WriteFile(System.IO.Path.Combine(data, "coest.xml"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Data.coest.xml");
            WriteFile(System.IO.Path.Combine(target.Location, "randomfile.something"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Data.randomfile.something");
            WriteFile(System.IO.Path.Combine(components, "Importer.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Components.Importer.dll");
            WriteFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Types.DictionaryTermWeights.dll");

            target.AddFile(System.IO.Path.Combine(data, "coest.xml"));
            target.AddFile(System.IO.Path.Combine(target.Location, "randomfile.something"));
            target.AddFile(System.IO.Path.Combine(components, "Importer.dll"));
            target.AddFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"));

            // TODO: Verify that files are added correctly
            Assert.AreEqual(4, target.Files.Count());
        }

        [TestMethod]
        public void SetDirectoryHasComponents()
        {
            string packageName = "SetDirectoryHasComponentsPackage";
            string packageDirectory = System.IO.Path.Combine(PackageTestRoot, packageName);
            System.IO.Directory.CreateDirectory(packageDirectory);
            Package target = new Package(packageDirectory, false);

            string components = System.IO.Path.Combine(target.Location, "Components");
            string types = System.IO.Path.Combine(target.Location, "Types");
            string data = System.IO.Path.Combine(target.Location, "Files");

            System.IO.Directory.CreateDirectory(components);
            System.IO.Directory.CreateDirectory(types);
            System.IO.Directory.CreateDirectory(data);

            WriteFile(System.IO.Path.Combine(components, "Importer.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Components.Importer.dll");
            WriteFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Types.DictionaryTermWeights.dll");

            target.AddFile(System.IO.Path.Combine(components, "Importer.dll"));
            target.AddFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"));

            Assert.AreEqual(0, target.ComponentLocations.Count());
            Assert.AreEqual(0, target.TypeLocations.Count());

            target.SetDirectoryHasComponents(components, true);
            Assert.AreEqual(1, target.ComponentLocations.Count());
            Assert.AreEqual(components, target.ComponentLocations.ElementAtOrDefault(0));

            // Now test that they turn off as well.
            target.SetDirectoryHasComponents(components, false);
            Assert.AreEqual(0, target.ComponentLocations.Count());
            Assert.AreEqual(0, target.TypeLocations.Count());
        }

        [TestMethod]
        public void SetDirectoryHasTypes()
        {
            string packageName = "SetDirectoryHasTypesPackage";
            string packageDirectory = System.IO.Path.Combine(PackageTestRoot, packageName);
            System.IO.Directory.CreateDirectory(packageDirectory);
            Package target = new Package(packageDirectory, false);

            string components = System.IO.Path.Combine(target.Location, "Components");
            string types = System.IO.Path.Combine(target.Location, "Types");
            string data = System.IO.Path.Combine(target.Location, "Files");

            System.IO.Directory.CreateDirectory(components);
            System.IO.Directory.CreateDirectory(types);
            System.IO.Directory.CreateDirectory(data);

            WriteFile(System.IO.Path.Combine(components, "Importer.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Components.Importer.dll");
            WriteFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"), "TraceLab.Core.Test.PackageSystem.PackageSystemTestResources.Types.DictionaryTermWeights.dll");

            target.AddFile(System.IO.Path.Combine(components, "Importer.dll"));
            target.AddFile(System.IO.Path.Combine(types, "DictionaryTermWeights.dll"));

            Assert.AreEqual(0, target.ComponentLocations.Count());
            Assert.AreEqual(0, target.TypeLocations.Count());

            target.SetDirectoryHasTypes(types, true);
            Assert.AreEqual(1, target.TypeLocations.Count());
            Assert.AreEqual(types, target.TypeLocations.ElementAtOrDefault(0));

            target.SetDirectoryHasTypes(types, false);
            Assert.AreEqual(0, target.TypeLocations.Count());
            Assert.AreEqual(0, target.ComponentLocations.Count());
        }

        /// <summary>
        ///A test for Pack
        ///</summary>
        [TestMethod()]
        public void PackRoundTripTest()
        {
            // TODO: Create new package based on a directory (See the Load test)
            var packageRoot = WriteTestPackage("RoundTripPackage");
            Package target = new Package(packageRoot, true);

            Package unpackedTarget = null;
            using (System.IO.MemoryStream packedStream = new System.IO.MemoryStream())
            {
                target.Pack(packedStream);
                packedStream.Seek(0, System.IO.SeekOrigin.Begin);

                System.IO.Directory.Delete(System.IO.Path.Combine(PackageTestRoot, "RoundTripPackage"), true);

                unpackedTarget = new Package(packedStream);
                unpackedTarget.Unpack(PackageTestRoot);
            }

            Assert.IsNotNull(unpackedTarget);

            Assert.AreEqual(target.ID, unpackedTarget.ID);
            Assert.AreEqual(target.Name, unpackedTarget.Name);

            Assert.AreEqual(target.Files.Count(), unpackedTarget.Files.Count());
            Assert.AreEqual(target.References.Count(), unpackedTarget.References.Count());
            Assert.AreEqual(target.ComponentLocations.Count(), unpackedTarget.ComponentLocations.Count());
            Assert.AreEqual(target.TypeLocations.Count(), unpackedTarget.TypeLocations.Count());

            for (int i = 0; i < target.Files.Count(); ++i)
            {
                var targetFile = target.Files.ElementAt(i);
                var unpackedFile = unpackedTarget.Files.ElementAt(i);

                Assert.AreEqual(targetFile.ID, unpackedFile.ID);
            }

            for (int i = 0; i < target.References.Count(); ++i)
            {
                Assert.AreEqual(target.References.ElementAt(i), unpackedTarget.References.ElementAt(i));
            }

            for (int i = 0; i < target.ComponentLocations.Count(); ++i)
            {
                Assert.AreEqual(target.ComponentLocations.ElementAt(i), unpackedTarget.ComponentLocations.ElementAt(i));
            }

            for (int i = 0; i < target.TypeLocations.Count(); ++i)
            {
                Assert.AreEqual(target.TypeLocations.ElementAt(i), unpackedTarget.TypeLocations.ElementAt(i));
            }

            // and lets make sure the files came through too.
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "RoundTripPackage.manifest")));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "Data", "coest.xml")));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "Data", "coest1.xml")));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "Data", "randomfile.something")));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "Components", "Importer.dll")));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "Types", "DictionaryTermWeights.dll")));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(packageRoot, "somerandomfile.xml")));
        }

        [TestMethod]
        public void SaveManifestTest()
        {
            var name = "TestPackage";
            var packageRoot = WriteTestPackage(name);

            // Load the package
            Package target = new Package(packageRoot, true);

            var manifestFile = System.IO.Path.Combine(PackageTestRoot, name, "TestPackage.manifest");
            var newManifestFile = System.IO.Path.Combine(PackageTestRoot, name, name + ".manifest");

            // Move the manifest to a backup 
            System.IO.File.Move(manifestFile, manifestFile + ".bak");

            // Compare the written manifest with the m anifest that is saved in the test resources
            Assert.IsFalse(System.IO.File.Exists(newManifestFile));

            // Force the save of the manifest
            target.SaveManifest();

            // Compare the written manifest with the m anifest that is saved in the test resources
            Assert.IsTrue(System.IO.File.Exists(newManifestFile));

            var backup = System.IO.File.ReadAllText(manifestFile + ".bak", Encoding.UTF8);
            var manifest = System.IO.File.ReadAllText(newManifestFile, Encoding.UTF8);
            Assert.IsTrue(backup.SequenceEqual(manifest));
        }
    }
}

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
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Components;
using System.IO;

namespace TraceLab.Core.Test.Components
{
    [TestClass]
    public class ComponentsLibraryCacheTest
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        internal TraceLabTestApplication AppContext
        {
            get;
            set;
        }

        public ComponentsLibrary Library
        {
            get;
            set;
        }

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
            this.Library = AppContext.Components;
        }
        
        [TestCleanup]
        public void TestTeardown()
        {
            Library.Clear();
        }

        /// <summary>
        /// This test loads the cache from it's regular location, then clears its content and checks that the cache
        /// is empty and the old cache file has been deleted
        /// </summary>
        [TestMethod]
        public void EmptyLibraryCacheTest()
        {
            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(Library));
            var libraryCache = libraryAccessor.m_componentFilesCache;
            var componentFiles = libraryCache.m_componentFiles;

            Assert.IsTrue(libraryCache.WasModified);
            Assert.AreEqual(libraryCache.AppExecutable, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Assert.IsTrue(componentFiles.Size() > 0);

            libraryCache.Clear();

            Assert.IsTrue(libraryCache.WasModified);
            Assert.AreEqual(componentFiles.Size(), 0);
            Assert.IsFalse(File.Exists(ComponentsLibraryCacheHelper_Accessor.s_defaultCacheLocation));
        }

        /// <summary>
        /// This test tries to load the cache from a file location that doesn't exists. Therefore, it creates a new
        /// empty cache.
        /// </summary>
        [TestMethod]
        public void WrongCacheFilePathTest()
        {
            ComponentsLibraryCacheHelper_Accessor.s_defaultCacheLocation = "c:\\ComponentsLibrary.cache";

            ComponentsLibraryCache cache = ComponentsLibraryCacheHelper.LoadCacheFile();

            Assert.IsNull(cache);

            ComponentsLibraryCacheHelper_Accessor.s_defaultCacheLocation = null;

            cache = ComponentsLibraryCacheHelper.LoadCacheFile();

            Assert.IsNull(cache);
        }

        /// <summary>
        /// Tests the different functionalities of the ComponentsLibraryCacheHelper static library of functions.
        /// </summary>
        [TestMethod]
        public void FileCacheHelperTest()
        {
            string cachePath = System.IO.Path.Combine(AppContext.BaseTestDirectory, "ComponentsLibrary.cache");

            ComponentsLibraryCacheHelper_Accessor.s_defaultCacheLocation = cachePath;
            ComponentsLibraryCache cache = ComponentsLibraryCacheHelper.LoadCacheFile();

            Assert.IsNull(cache);

            cache = new ComponentsLibraryCache(cachePath);

            Assert.IsTrue(cache.ComponentFiles != null);
            Assert.AreEqual(cache.AppExecutable, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            ComponentsLibraryCacheHelper.StoreCacheFile(cache);

            Assert.IsFalse(cache.WasModified);
            Assert.IsTrue(ComponentsLibraryCacheHelper.CacheFileExists());

            ComponentsLibraryCacheHelper.DeleteCacheFile();

            Assert.IsFalse(ComponentsLibraryCacheHelper.CacheFileExists());
        }

        /// <summary>
        /// FileDescriptors should never be initialized with a null or incorrect file path, otherwise an
        /// exception will be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void FileDescriptorNullTest()
        {
            FileDescriptor assembly = new AssemblyFileDescriptor(null);
        }

        /// <summary>
        /// Testing the functionality of class FileDescriptor and adding metadata to the Library Cache.
        /// </summary>
        [TestMethod]
        public void FileDescriptorTest()
        {
            string componentsDir = System.IO.Path.Combine(AppContext.BaseTestDirectory, "TestComponents");
            string componentFile = System.IO.Path.Combine(componentsDir, "Importer.dll");

            AssemblyFileDescriptor assembly = new AssemblyFileDescriptor(componentFile);

            Assert.IsTrue(assembly.isUpToDate());

            string id = "MockComponent";
            string classname = "MockComponents.MockComponent"; //class exists and should be loaded
            ComponentMetadataDefinition compMetaDef = new ComponentMetadataDefinition(id, assembly.AbsolutePath, classname);

            Assert.AreEqual("MockComponents.MockComponent", compMetaDef.Classname);

            assembly.MetadataCollection.Add(compMetaDef);

            IList<FileDescriptor> files = new List<FileDescriptor>();
            files.Add(assembly);

            ComponentsLibraryCache cache = new ComponentsLibraryCache(AppContext.BaseTestDirectory);
            cache.AddComponentFiles(files);

            Assert.IsTrue(cache.ComponentFiles.Contains(assembly.AbsolutePath));

            ISet<string> filepaths = cache.GetUpToDateFiles();

            Assert.AreEqual(filepaths.Count, 1);
        }
    }
}

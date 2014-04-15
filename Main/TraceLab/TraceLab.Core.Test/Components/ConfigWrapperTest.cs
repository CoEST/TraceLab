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
using TraceLab.Core.Components;
using TraceLabSDK.Component.Config;
using TraceLab.Core.Test.Setup;
using System.IO;

namespace TraceLab.Core.Test.Components
{
    /// <summary>
    /// Summary description for ConfigWrapper
    /// </summary>
    [TestClass]
    public class ConfigWrapperTest
    {
        public ConfigWrapperTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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

            //set current location
            currentLocation = AppContext.BaseTestDirectory;

            //create config definition using createwrapper method of the ComponentMetadataDefinition class
            var configDef = ComponentScannerHelper_Accessor.CreateConfigWrapperDefinition(typeof(MockConfig));
            testConfig = new ConfigWrapper(configDef);

            //set the values of MockConfig
            mockFileAbsolutePath = Path.Combine(currentLocation, mockFile);
            Stream file = File.Create(mockFileAbsolutePath);
            file.Close(); //close file so that it is not being used by this process anymore

            configFilePath = new FilePath();
            configFilePath.Init(mockFileAbsolutePath, currentLocation);
            //the key matches property name in the MockConfig
            testConfig.ConfigValues["MockFile"].Value = configFilePath;

            mockDirAbsolutePath = Path.Combine(currentLocation, mockDir);
            Directory.CreateDirectory(mockDirAbsolutePath);
            Assert.IsTrue(Directory.Exists(mockDirAbsolutePath));

            configDirPath = new DirectoryPath();
            configDirPath.Init(mockDirAbsolutePath, currentLocation);
            //the key matches property name in the MockConfig
            testConfig.ConfigValues["MockDirectory"].Value = configDirPath;
        }

        [TestCleanup]
        public void TestTearDown()
        {
            File.Delete(mockFileAbsolutePath);
            Directory.Delete(mockDirAbsolutePath);

            AppContext.Components.Clear();
            AppContext = null;
        }

        private string currentLocation;
        private string mockFile = "MockFileTestCopyReferenceFiles.txt";
        private string mockFileAbsolutePath;
        private string mockDir = "MockDirectoryTestCopyReferenceFiles";
        private string mockDirAbsolutePath;
        private ConfigWrapper testConfig;
        private FilePath configFilePath;
        private DirectoryPath configDirPath;

        [TestMethod]
        public void TestCopyReferencedFiles()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(newLocation);

            string newMockAbsoluteFilePath = Path.Combine(newLocation, mockFile);
            string newMockDirAbsolutePath = Path.Combine(newLocation, mockDir);

            Assert.IsFalse(File.Exists(newMockAbsoluteFilePath));
            Assert.IsFalse(Directory.Exists(newMockDirAbsolutePath));

            testConfig.CopyReferencedFiles(newLocation, currentLocation, true);

            Assert.IsTrue(File.Exists(newMockAbsoluteFilePath));
            Assert.IsTrue(Directory.Exists(newMockDirAbsolutePath));

            Assert.AreEqual(mockFile, configFilePath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockAbsoluteFilePath, configFilePath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            Assert.AreEqual(mockDir, configDirPath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockDirAbsolutePath, configDirPath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location

            //cleanup new files after test
            File.Delete(newMockAbsoluteFilePath);
            Directory.Delete(newMockDirAbsolutePath);
            Directory.Delete(newLocation);
        }

        [TestMethod]
        public void TestCopyReferencedFilesOverride()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(newLocation);

            string newMockAbsoluteFilePath = Path.Combine(newLocation, mockFile);
            string newMockDirAbsolutePath = Path.Combine(newLocation, mockDir);

            Assert.IsFalse(File.Exists(newMockAbsoluteFilePath));
            Assert.IsFalse(Directory.Exists(newMockDirAbsolutePath));

            //create the file at the new location before copying
            Stream file = File.Create(newMockAbsoluteFilePath);
            file.Close(); //close file so that it is not being used by this process anymore

            testConfig.CopyReferencedFiles(newLocation, currentLocation, true);
            
            Assert.IsTrue(File.Exists(newMockAbsoluteFilePath));
            Assert.IsTrue(Directory.Exists(newMockDirAbsolutePath));

            Assert.AreEqual(mockFile, configFilePath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockAbsoluteFilePath, configFilePath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            Assert.AreEqual(mockDir, configDirPath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockDirAbsolutePath, configDirPath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location

            //cleanup new files after test
            File.Delete(newMockAbsoluteFilePath);
            Directory.Delete(newMockDirAbsolutePath);
            Directory.Delete(newLocation);
        }

        [TestMethod]
        public void TestCopyReferencedFilesDoNotOverride()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(newLocation);

            string newMockAbsoluteFilePath = Path.Combine(newLocation, mockFile);
            string newMockDirAbsolutePath = Path.Combine(newLocation, mockDir);

            Assert.IsFalse(File.Exists(newMockAbsoluteFilePath));
            Assert.IsFalse(Directory.Exists(newMockDirAbsolutePath));

            //create the file at the new location before copying
            Stream file = File.Create(newMockAbsoluteFilePath);
            file.Close(); //close file so that it is not being used by this process anymore

            testConfig.CopyReferencedFiles(newLocation, currentLocation, false); //should not throw exception, just skip the file
            
            Assert.IsTrue(File.Exists(newMockAbsoluteFilePath));
            Assert.IsTrue(Directory.Exists(newMockDirAbsolutePath));

            Assert.AreEqual(mockFile, configFilePath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockAbsoluteFilePath, configFilePath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            Assert.AreEqual(mockDir, configDirPath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockDirAbsolutePath, configDirPath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location

            //cleanup new files after test
            File.Delete(newMockAbsoluteFilePath);
            Directory.Delete(newMockDirAbsolutePath);
            Directory.Delete(newLocation);
        }

        [TestMethod]
        public void TestCopyReferencedOverrideReadOnlyFile()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(newLocation);

            string newMockAbsoluteFilePath = Path.Combine(newLocation, mockFile);
            string newMockDirAbsolutePath = Path.Combine(newLocation, mockDir);

            Assert.IsFalse(File.Exists(newMockAbsoluteFilePath));
            Assert.IsFalse(Directory.Exists(newMockDirAbsolutePath));

            //create the file at the new location before copying
            Stream file = File.Create(newMockAbsoluteFilePath);
            file.Close(); //close file so that it is not being used by this process anymore
            // set it to read only
            System.IO.FileInfo finfo = new System.IO.FileInfo(newMockAbsoluteFilePath);
            finfo.IsReadOnly = true;

            try
            {
                testConfig.CopyReferencedFiles(newLocation, currentLocation, true);
            }
            catch (TraceLab.Core.Exceptions.FilesCopyFailuresException ex)
            {
                Assert.AreEqual(ex.CopyErrors.Count, 1);  //there should be one error message, about one file that failed to be overwritten
            }

            Assert.IsTrue(File.Exists(newMockAbsoluteFilePath));
            Assert.IsTrue(Directory.Exists(newMockDirAbsolutePath));

            Assert.AreEqual(mockFile, configFilePath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockAbsoluteFilePath, configFilePath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            Assert.AreEqual(mockDir, configDirPath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockDirAbsolutePath, configDirPath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location

            //cleanup new files after test
            finfo.IsReadOnly = false;
            File.Delete(newMockAbsoluteFilePath);
            Directory.Delete(newMockDirAbsolutePath);
            Directory.Delete(newLocation);
        }

        [TestMethod]
        public void TestCopyReferencedFilesLockedFile()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(newLocation);

            string newMockAbsoluteFilePath = Path.Combine(newLocation, mockFile);
            string newMockDirAbsolutePath = Path.Combine(newLocation, mockDir);

            Assert.IsFalse(File.Exists(newMockAbsoluteFilePath));
            Assert.IsFalse(Directory.Exists(newMockDirAbsolutePath));

            Stream file = File.Open(mockFileAbsolutePath, FileMode.Append); //open file so that it is used, and can't be copied

            try
            {
                testConfig.CopyReferencedFiles(newLocation, currentLocation, false);
            }
            catch (TraceLab.Core.Exceptions.FilesCopyFailuresException ex)
            {
                Assert.AreEqual(ex.CopyErrors.Count, 1);  //there should be one error message, about one filed that could not have been copied
            }
            Assert.IsFalse(File.Exists(newMockAbsoluteFilePath)); //because copying failed
            Assert.IsTrue(Directory.Exists(newMockDirAbsolutePath));

            Assert.AreEqual(mockFile, configFilePath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockAbsoluteFilePath, configFilePath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            Assert.AreEqual(mockDir, configDirPath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockDirAbsolutePath, configDirPath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location

            //cleanup new files after test
            File.Delete(newMockAbsoluteFilePath);
            Directory.Delete(newMockDirAbsolutePath);
            Directory.Delete(newLocation);

            file.Close();//now it can be closed
        }

        [TestMethod]
        public void TestSetExperimentLocationRootTransformRelative()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            
            testConfig.SetExperimentLocationRoot(newLocation, true);

            string expectedRelative = ".." + Path.DirectorySeparatorChar + mockFile;
            Assert.AreEqual(expectedRelative, configFilePath.Relative); //relative should have changed
            Assert.AreEqual(mockFileAbsolutePath, configFilePath.Absolute); //absolute should have remained the same
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            string expectedRelativeDir = ".." + Path.DirectorySeparatorChar + mockDir;
            Assert.AreEqual(expectedRelativeDir, configDirPath.Relative); //relative should have changed
            Assert.AreEqual(mockDirAbsolutePath, configDirPath.Absolute); //absolute should have remained the same
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location
        }

        /// <summary>
        /// Tests the set experiment location root do not transform relative. (IGNORE)
        /// </summary>
        [TestMethod]
        public void TestSetExperimentLocationRootDoNotTransformRelative()
        {
            //once set let's call CopyReferencedFile and change location to new location (add last trailing slash because DataRoot includes last trailing slash)
            string newLocation = Path.Combine(AppContext.BaseTestDirectory, "TestCopyReferenceFiles") + Path.DirectorySeparatorChar;
            string newMockAbsoluteFilePath = Path.Combine(newLocation, mockFile);
            string newMockDirAbsolutePath = Path.Combine(newLocation, mockDir);

            testConfig.SetExperimentLocationRoot(newLocation, false);

            Assert.AreEqual(mockFile, configFilePath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockAbsoluteFilePath, configFilePath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configFilePath.DataRoot); //data root should have been changed to new location

            Assert.AreEqual(mockDir, configDirPath.Relative); //relative should have remained the same
            Assert.AreEqual(newMockDirAbsolutePath, configDirPath.Absolute); //absolute should have changed to new path
            Assert.AreEqual(newLocation, configDirPath.DataRoot); //data root should have been changed to new location
        }


        class MockConfig
        {
            public FilePath MockFile
            {
                get;
                set;
            }

            public DirectoryPath MockDirectory
            {
                get;
                set;
            }
        }
    }
}
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Workspaces.Serialization;
using TraceLab.Core.Test.Setup;

namespace TraceLab.Serialization.Test.StreamSystem
{
    [TestClass]
    public class FileWrapperTests
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

        private string m_tempfilename;
        public string TemporaryFile
        {
            get
            {
                return m_tempfilename;
            }
        }

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext, false);
            m_tempfilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Foo.xml");

            if (System.IO.File.Exists(TemporaryFile))
                System.IO.File.Delete(TemporaryFile);
        }

        [TestCleanup]
        public void TestTeardown()
        {
            if (System.IO.File.Exists(TemporaryFile))
                System.IO.File.Delete(TemporaryFile);
            AppContext.Dispose();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PathEmptyConstructionTest()
        {
            var wrapper = new FileWrapperStream(string.Empty);
            wrapper.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PathNullConstructionTest()
        {
            var wrapper = new FileWrapperStream(null);
            wrapper.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PathRelativeConstructionTest()
        {
            var wrapper = new FileWrapperStream(System.IO.Path.Combine("..", "..", "Place", "Foo"));
            wrapper.Dispose();
        }

        [TestMethod]
        public void ConstructionCreatesFileIfNotExistTest()
        {
            var wrapper = new FileWrapperStream(TemporaryFile);
            wrapper.Dispose();
            Assert.IsTrue(System.IO.File.Exists(TemporaryFile));
        }

        [TestMethod]
        public void CanSetLength()
        {
            var wrapper = new FileWrapperStream(TemporaryFile);
            Assert.AreEqual(0, wrapper.Length);
            wrapper.SetLength(200);
            Assert.AreEqual(200, wrapper.Length);
            wrapper.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.IOException))]
        public void ConstructionFailsIfFileIfCannotOpenFile()
        {
            using(System.IO.BinaryWriter writer = new System.IO.BinaryWriter(new System.IO.FileStream(TemporaryFile, System.IO.FileMode.OpenOrCreate)))
            {
                var wrapper = new FileWrapperStream(TemporaryFile);
                wrapper.Dispose();
                Assert.IsTrue(System.IO.File.Exists(TemporaryFile));
            }
        }

        [TestMethod]
        public void ConstructionDoesNotEraseFileIfExistTest()
        {
            System.IO.StreamWriter writer = System.IO.File.CreateText(TemporaryFile);
            writer.Write("blah");
            writer.Close();

            System.IO.FileInfo preInfo = new System.IO.FileInfo(TemporaryFile);
            Assert.IsTrue(preInfo.Length != 0);

            var wrapper = new FileWrapperStream(TemporaryFile);
            wrapper.Dispose();
            Assert.IsTrue(System.IO.File.Exists(TemporaryFile));

            System.IO.FileInfo postInfo = new System.IO.FileInfo(TemporaryFile);
            Assert.IsTrue(postInfo.Length != 0);
        }

        [TestMethod]
        public void LoadExistingFileTest()
        {
            System.IO.StreamWriter writer = System.IO.File.CreateText(TemporaryFile);
            writer.Write("blah");
            writer.Close();

            var wrapper = new FileWrapperStream(TemporaryFile);

            // Ensure that new FileWrappers start at the beginning
            Assert.AreEqual(0, wrapper.Position);

            Assert.AreEqual(4, wrapper.Length);
            wrapper.Position = 0;
            System.IO.StreamReader reader = new System.IO.StreamReader(wrapper);
            
            string data = reader.ReadToEnd();
            Assert.AreEqual("blah", data);

            wrapper.Dispose();
        }

        [TestMethod]
        public void WriteSetsDiskFlushFlag()
        {
            using (var wrapper = new FileWrapperStream(TemporaryFile))
            {
                wrapper.Seek(0, System.IO.SeekOrigin.Begin);
                Assert.IsFalse(wrapper.IsAwaitingDiskFlush);
                wrapper.WriteByte(4);
                Assert.IsTrue(wrapper.IsAwaitingDiskFlush);
            }
        }

        [TestMethod]
        public void WriteToExistingFileTest()
        {
            using (System.IO.StreamWriter writer = System.IO.File.CreateText(TemporaryFile))
            {
                writer.Write("blah");
                writer.Close();
            }

            using (var wrapper = new FileWrapperStream(TemporaryFile))
            {
                wrapper.Seek(0, System.IO.SeekOrigin.End);

                Assert.IsFalse(wrapper.IsAwaitingDiskFlush);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(wrapper);
                writer.AutoFlush = true;
                writer.Write("test");
            }

            using (System.IO.StreamReader reader = new System.IO.StreamReader(TemporaryFile))
            {
                string data = reader.ReadToEnd();
                Assert.AreEqual("blahtest", data);
            }
        }

        [TestMethod]
        public void WriteToNonExistingFileTest()
        {
            Assert.IsFalse(System.IO.File.Exists(TemporaryFile));

            using (var wrapper = new FileWrapperStream(TemporaryFile))
            {
                Assert.IsFalse(wrapper.IsAwaitingDiskFlush);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(wrapper);
                writer.AutoFlush = true;
                writer.Write("test");
            }

            using (System.IO.StreamReader reader = new System.IO.StreamReader(TemporaryFile))
            {
                string data = reader.ReadToEnd();
                Assert.AreEqual("test", data);
            }
        }
    }
}

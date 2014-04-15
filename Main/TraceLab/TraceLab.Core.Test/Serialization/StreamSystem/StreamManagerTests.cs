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
using System.IO;
using TraceLab.Core.Workspaces.Serialization;
using TraceLab.Core.Test.Setup;

namespace TraceLab.Serialization.Test.StreamSystem
{
    [TestClass]
    public class StreamManagerTests
    {
        public StreamManagerTests() {}

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

        [TestInitialize]
        public void Setup()
        {
            AppContext = new TraceLabTestApplication(TestContext, false);
            AppContext.StreamManager.Clear();
            TmpPath = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Foo.xml");
        }

        [TestCleanup]
        public void TearDown()
        {
            new FileInfo(TmpPath).Delete();
        }

        public string TmpPath
        {
            get;
            set;
        }

        [TestMethod]
        public void ClearTest()
        {
            Assert.IsTrue(AppContext.StreamManager.IsEmpty());
            Stream stream = AppContext.StreamManager.GetStream(TmpPath);
            Assert.IsFalse(AppContext.StreamManager.IsEmpty());
            AppContext.StreamManager.Clear();
            Assert.IsTrue(AppContext.StreamManager.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetStreamTestNotAbsolutePath()
        {
            Stream stream = AppContext.StreamManager.GetStream("notAbsolutePath");
        }

        [TestMethod]
        public void GetStreamTestStreamIsNotNull()
        {
            Stream stream = AppContext.StreamManager.GetStream(TmpPath);
            Assert.IsNotNull(stream);
        }

        [TestMethod]
        public void GetStreamTestReadWrite()
        {
            Stream stream = AppContext.StreamManager.GetStream(TmpPath);
            StreamWriter writer = new StreamWriter(stream);
            string w = "workspace knows all";
            writer.WriteLine(w);
            writer.Flush();
            AppContext.StreamManager.Flush(TmpPath);

            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string r = reader.ReadLine();

            Assert.AreEqual(w, r);
        }

        [TestMethod]
        public void GetStreamTemporary()
        {
            Stream stream = AppContext.StreamManager.GetStream(TmpPath, true);
            StreamWriter writer = new StreamWriter(stream);
            string w = "workspace knows all";
            writer.WriteLine(w);
            writer.Flush();

            // Close out the stream, we're done with it.
            AppContext.StreamManager.Flush(TmpPath);
            AppContext.StreamManager.CloseStream(TmpPath);

            // After closing a temporary stream, nothing remains on disk.
            Assert.IsFalse(File.Exists(TmpPath));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ObjectDisposedException))]
        public void CloseStreamTest()
        {
            Stream stream = AppContext.StreamManager.GetStream(TmpPath);

            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanWrite);
            stream.Write(new byte[4096], 0, 100);

            AppContext.StreamManager.CloseStream(TmpPath);

            Assert.IsFalse(stream.CanRead);
            Assert.IsFalse(stream.CanWrite);
            stream.Write(new byte[4096], 0, 100); //should throw exception - stream should have been disposed
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FlushTestNoStream()
        {
            AppContext.StreamManager.Flush(TmpPath); //throws exception - path has not been created
        }

        [TestMethod]
        public void FlushTest()
        {
            FileWrapperStream stream = (FileWrapperStream) AppContext.StreamManager.GetStream(TmpPath);
            Assert.IsFalse(stream.IsAwaitingDiskFlush);

            string dataString = "Blah";
            byte[] data = StrToByteArray(dataString);
            stream.Write(data, 0, data.Length);

            Assert.IsTrue(stream.IsAwaitingDiskFlush);
            AppContext.StreamManager.Flush(TmpPath); //this should pass... file exists
            Assert.IsFalse(stream.IsAwaitingDiskFlush);
        }

        // C# to convert a string to a byte array.
        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        [TestMethod]
        public void FlushAllTest()
        {
            int no = 4;
            string path;
            FileWrapperStream[] streams = new FileWrapperStream[no];
            string dataString = "Blah";
            byte[] data = StrToByteArray(dataString);
            for (int i = 0; i < no; i++)
            {
                path = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Foo" + i + ".xml");
                streams[i] = (FileWrapperStream)AppContext.StreamManager.GetStream(path);
                Assert.IsFalse(streams[i].IsAwaitingDiskFlush);
                
                streams[i].Write(data, 0, data.Length);
                Assert.IsTrue(streams[i].IsAwaitingDiskFlush);
            }

            AppContext.StreamManager.FlushAll();
            for (int i = 0; i < no; i++)
            {
                Assert.IsFalse(streams[i].IsAwaitingDiskFlush);
            }

            //delete files after test
            for (int i = 0; i < no; i++)
            {
                path = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Foo" + i + ".xml");
                (new FileInfo(path)).Delete();
            }
        }

        [TestMethod]
        public void MoveStreamNotTemporaryTest()
        {
            StreamManager_Accessor streamManager = new StreamManager_Accessor();

            Assert.IsTrue(streamManager.IsEmpty());

            string path1 = System.IO.Path.Combine(AppContext.BaseTestDirectory, "testPath1.cache");
            string path2 = System.IO.Path.Combine(AppContext.BaseTestDirectory, "testPath2.cache");
            
            //creates two streams in the stream manager
            Stream stream1 = streamManager.GetStream(path1);
            Stream stream2 = streamManager.GetStream(path2);

            //both streams exists
            Assert.AreEqual(2, streamManager.m_streams.Count);

            //stream2 is opened before moving
            Assert.IsTrue(stream2.CanWrite);
            Assert.IsTrue(stream2.CanRead);

            //file of the path1 and path2 should exist before move
            Assert.IsTrue(File.Exists(path1));
            Assert.IsTrue(File.Exists(path2));

            //move path1 over path2
            streamManager.MoveStream(path1, path2);

            //file of the old path1 should not exist anymore
            Assert.IsFalse(File.Exists(path1));

            //path2 should still exists, although it is different file
            Assert.IsTrue(File.Exists(path2));
            
            //stream manager should now have just one stream and it should be path2. 
            Assert.AreEqual(1, streamManager.m_streams.Count);
            Assert.IsFalse(streamManager.m_streams.ContainsKey(path1));
            Assert.IsTrue(streamManager.m_streams.ContainsKey(path2));

            //the current stream for path2 should be equal to stream1
            Stream stream3 = streamManager.GetStream(path2);
            Assert.AreEqual(stream1, stream3);
            
            //the stream2 should be close
            Assert.IsFalse(stream2.CanWrite);
            Assert.IsFalse(stream2.CanRead);
        }

        [TestMethod]
        public void MoveStreamTemporaryTest()
        {
            StreamManager_Accessor streamManager = new StreamManager_Accessor();

            Assert.IsTrue(streamManager.IsEmpty());

            string path1 = System.IO.Path.Combine(AppContext.BaseTestDirectory, "testPath1.cache");
            string path2 = System.IO.Path.Combine(AppContext.BaseTestDirectory, "testPath2.cache");

            //creates two streams in the stream manager
            Stream stream1 = streamManager.GetStream(path1, true);
            Stream stream2 = streamManager.GetStream(path2, true);

            //both streams exists
            Assert.AreEqual(2, streamManager.m_streams.Count);

            //stream2 is opened before moving
            Assert.IsTrue(stream2.CanWrite);
            Assert.IsTrue(stream2.CanRead);

            //move path1 over path2
            streamManager.MoveStream(path1, path2);

            //stream manager should now have just one stream and it should be path2. 
            Assert.AreEqual(1, streamManager.m_streams.Count);
            Assert.IsFalse(streamManager.m_streams.ContainsKey(path1));
            Assert.IsTrue(streamManager.m_streams.ContainsKey(path2));

            //the current stream for path2 should be equal to stream1
            Stream stream3 = streamManager.GetStream(path2);
            Assert.AreEqual(stream1, stream3);

            //the stream2 should be close
            Assert.IsFalse(stream2.CanWrite);
            Assert.IsFalse(stream2.CanRead);
        }
    }
}

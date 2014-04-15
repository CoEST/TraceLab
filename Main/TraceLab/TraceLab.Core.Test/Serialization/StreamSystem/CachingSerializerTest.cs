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
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Workspaces;
using TraceLab.Core.Workspaces.Serialization;
using System.Xml;
using System.IO;
using TraceLab.Core.Test.Setup;

namespace TraceLab.Serialization.Test.StreamSystem
{
    [TestClass]
    public class CachingSerializerTest
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

        public Type[] SupportedTypes
        {
            get;
            set;
        }

        public CachingSerializerTest()
        {
            TestSerializableObject obj = new TestSerializableObject();
            Type[] supportedTypes = new Type[1];
            supportedTypes[0] = obj.GetType();
            this.SupportedTypes = supportedTypes;
        }

        private string TmpCachePath
        {
            get;
            set;
        }

        private string TmpDataPath
        {
            get;
            set;
        }

        private CachingSerializer TestCacheSerializer
        {
            get;
            set;
        }

        [TestInitialize]
        public void SetUp()
        {
            AppContext = new TraceLabTestApplication(TestContext, false);

            //setPath
            this.TmpCachePath = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Foo.cache");
            this.TmpDataPath = System.IO.Path.Combine(AppContext.BaseTestDirectory, "DataFoo.xml");

            TestCacheSerializer = new CachingSerializer(AppContext.StreamManager, TmpDataPath, TmpCachePath, SupportedTypes, true, true);
        }

        [TestCleanup]
        public void TearDown()
        {
            new FileInfo(TmpDataPath).Delete();
            new FileInfo(TmpCachePath).Delete();

            AppContext.Dispose();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNullPath()
        {
            CachingSerializer cachingSerial = new CachingSerializer(AppContext.StreamManager, null, TmpCachePath, SupportedTypes, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNullCachePath()
        {
            CachingSerializer cachingSerial = new CachingSerializer(AppContext.StreamManager, TmpDataPath, null, SupportedTypes, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNullStreamMgr()
        {
            CachingSerializer cachingSerial = new CachingSerializer(null, TmpDataPath, TmpCachePath, SupportedTypes, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestConstructorNotAbsolutePath()
        {
            CachingSerializer cachingSerial = new CachingSerializer(AppContext.StreamManager, "Foo.xml", TmpCachePath, SupportedTypes, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestConstructorNotAbsoluteCachePath()
        {
            CachingSerializer cachingSerial = new CachingSerializer(AppContext.StreamManager, TmpDataPath, "Foo.cache", SupportedTypes, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSerializeNullObj()
        {
            TestSerializableObject obj = new TestSerializableObject();

            obj.Test = "test";
            TestCacheSerializer.Serialize(null); //should throw null exception
        }


        /// <summary>
        /// TODO: reads xml file, but is that enough?
        /// </summary>
        [TestMethod]
        public void TestSerialize()
        {
            TestSerializableObject obj = new TestSerializableObject();

            obj.Test = "test";
            TestCacheSerializer.Serialize(obj); //should pass

            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(TmpDataPath);
            XmlNode node = myXmlDocument.DocumentElement;

            //find Test node
            bool nodeFound = false;
            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name.Equals("Test"))
                {
                    nodeFound = true;
                    Assert.AreEqual("test", node1.InnerText);
                }
            }
            Assert.IsTrue(nodeFound);
        }


        [TestMethod]
        [ExpectedException(typeof(System.Runtime.Serialization.SerializationException))]
        public void TestSerializeNotSerializableObject()
        {
            TestNotSerializableObject obj = new TestNotSerializableObject();

            TestCacheSerializer.Serialize(obj); //should not pass, because object is not serializable
        }




        [TestMethod]
        public void TestDeserialize()
        {
            TestSerializableObject obj = new TestSerializableObject();

            obj.Test = "test123";
            TestCacheSerializer.Serialize(obj);

            TestSerializableObject desObj = TestCacheSerializer.Deserialize<TestSerializableObject>();

            Assert.IsTrue(desObj.Equals(obj));
        }

        /// <summary>
        /// cache is older than data
        /// 1. Create object with Test property = "original value 123"
        /// 2. Serialize object - this saves serialized object to disk in xml format 'Foo.xml' and binary file GUID.membinary, that represents cache
        /// 3. Modify xml file and change Test property to "modified value 987!" - then save the file, which changes the timestamp - the cache is not up to date
        /// 4. Deserialize object - it should not return the cached value, but the data from xml file. 
        /// </summary>
        [TestMethod]
        public void TestDeserializeUseData()
        {
            TestSerializableObject obj = new TestSerializableObject();

            obj.Test = "original value 123";
            TestCacheSerializer.Serialize(obj);

            //1. should change timestampt of the file - Data should be newer than Cache
            string newTestValue = "modified value 987!";
            ModifyXMLAndSave(TmpDataPath, newTestValue);

            //2. assure that Data is newer than cache - update lastwritetime for newer moment
            FileInfo pathFileInfo = new FileInfo(TmpDataPath);
            FileInfo cachedFileInfo = new FileInfo(TmpCachePath);

            //3. just make Data LastWriteTime 5 minutes newer than LastWriteTime of Cache
            pathFileInfo.LastWriteTime = new DateTime(cachedFileInfo.LastWriteTime.Ticks).Add(new System.TimeSpan(0, 0, 5, 0));
            //double check that it is newer
            Assert.IsTrue(pathFileInfo.LastWriteTime > cachedFileInfo.LastWriteTime);

            //4. Deserialize object - it should use data from xml... cache is old! We expect to get new value
            TestSerializableObject deseralizedObj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.AreEqual(newTestValue, deseralizedObj.Test);
        }


        /// <summary>
        /// This test checks if cache has been updated when deserialization used Data.
        /// The beginning of the test is the same as previous test... the actual new test starts from 5.
        /// </summary>
        [TestMethod]
        public void TestDeserializeCheckIfCacheHasBeenUpdated()
        {
            //------ 
            TestSerializableObject obj = new TestSerializableObject();

            obj.Test = "original value 123";
            TestCacheSerializer.Serialize(obj);

            //1. should change timestampt of the file - Data should be newer than Cache
            string newTestValue = "modified value 987!";
            ModifyXMLAndSave(TmpDataPath, newTestValue);

            //2. assure that Data is newer than cache - update lastwritetime for newer moment
            FileInfo pathFileInfo = new FileInfo(TmpDataPath);
            FileInfo cachedFileInfo = new FileInfo(TmpCachePath);

            //3. just make Data LastWriteTime 5 minutes newer than LastWriteTime of Cache
            pathFileInfo.LastWriteTime = new DateTime(cachedFileInfo.LastWriteTime.Ticks).Add(new System.TimeSpan(0, 0, 5, 0));
            //double check that it is newer
            Assert.IsTrue(pathFileInfo.LastWriteTime > cachedFileInfo.LastWriteTime);

            //4. Deserialize object - it should use data from xml... cache is old! We expect to get new value
            TestSerializableObject deseralizedObj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.AreEqual(newTestValue, deseralizedObj.Test);

            //-------

            //5. but cache should be updated as well
            //so change Cache LastWriteTime to be newer than Data LastWriteTime  
            cachedFileInfo.LastWriteTime = new DateTime(pathFileInfo.LastWriteTime.Ticks).Add(new System.TimeSpan(0, 0, 5, 0));

            //double check that Data is older than Cache, just in case
            Assert.IsTrue(pathFileInfo.LastWriteTime < cachedFileInfo.LastWriteTime);

            //6. Deserialize object this time it should use cache - and if cache it updated then it will return newTestValue
            //   otherwise it would be old original value, which would be wrong.
            TestSerializableObject deseralizedObj2 = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.AreEqual(newTestValue, deseralizedObj2.Test);
        }

        /// <summary>
        /// cache is newer than data
        /// 1. Create object with Test property = "original value 123"
        /// 2. Serialize object - this saves serialized object to disk in xml format 'Foo.xml' and binary file GUID.membinary, that represents cache
        /// 3. Modify xml file and change Test property to "modified value 987!" - then save the file, which changes the timestamp - the cache is not up to date
        /// 4. In this test we change the xml file timestamp so that Data is older than Cache
        /// 5. Deserialize object - it should return the cached value.
        /// </summary>
        [TestMethod]
        public void TestDeserializeUseCache()
        {
            TestSerializableObject obj = new TestSerializableObject();
            obj.Test = "original value 123";

            TestCacheSerializer.Serialize(obj);

            //changes timestampt of the file - Data is newer than Cache - we don't want it in this test
            string newTestValue = "modified value 987!";
            ModifyXMLAndSave(TmpDataPath, newTestValue);

            //so change the timestamp of the xml file to old
            FileInfo pathFileInfo = new FileInfo(TmpDataPath);
            FileInfo cachedFileInfo = new FileInfo(TmpCachePath);

            pathFileInfo.LastWriteTime = new DateTime(cachedFileInfo.LastWriteTime.Ticks).Subtract(new System.TimeSpan(0, 0, 5, 0)); //just make it 5 minutes older
            //double check that it is older
            Assert.IsTrue(pathFileInfo.LastWriteTime < cachedFileInfo.LastWriteTime);

            //it should use cache... cache is newer!
            TestSerializableObject deseralizedObj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.AreEqual("original value 123", deseralizedObj.Test);
        }

        /// <summary>
        /// cache = data
        /// 1. Create object with Test property = "original value 123"
        /// 2. Serialize object - this saves serialized object to disk in xml format 'Foo.xml' and binary file GUID.membinary, that represents cache
        /// 3. Modify xml file and change Test property to "modified value 987!" - then save the file, which changes the timestamp - the cache is not up to date
        /// 4. In this test we change the xml file timestamp so that Data is older than Cache
        /// 5. Deserialize object - it should return the cached value.
        /// </summary>
        [TestMethod]
        public void TestDeserializeUseCacheIfTimestampsAreEqual()
        {
            TestSerializableObject obj = new TestSerializableObject();
            obj.Test = "original value 123";

            TestCacheSerializer.Serialize(obj);

            //changes timestampt of the file - Data is newer than Cache - we don't want it in this test
            string newTestValue = "modified value 987!";
            ModifyXMLAndSave(TmpDataPath, newTestValue);

            //so change the timestamp of the xml file to old
            FileInfo pathFileInfo = new FileInfo(TmpDataPath);
            FileInfo cachedFileInfo = new FileInfo(TmpCachePath);

            pathFileInfo.LastWriteTime = cachedFileInfo.LastWriteTime; //cache = data

            //it should use cache... cache is newer!
            TestSerializableObject deseralizedObj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.AreEqual("original value 123", deseralizedObj.Test);
        }

        /// <summary>
        /// Test checks that Deserialize works correctly when cache didn't exist while the new Data file appeared.
        /// </summary>
        [TestMethod]
        public void TestDeserializeCreateCacheWhenCacheDidNotExist()
        {
            TestSerializableObject obj = new TestSerializableObject();

            string originalValue = "original value 123";
            obj.Test = originalValue;
            TestCacheSerializer.Serialize(obj);

            //2. Delete the cachePath
            AppContext.StreamManager.CloseStream(TmpCachePath); //disposes and removes cache stream
            (new FileInfo(TmpCachePath)).Delete();

            //3. Currently there is only Data file

            //4. Deserialize object - it should use data from xml... cache does not exists... it should still work
            TestSerializableObject deseralizedObj = TestCacheSerializer.Deserialize<TestSerializableObject>();

            //5. cache should be have been created as well
            //so lets change Data
            string newTestValue = "modified value 987!";
            ModifyXMLAndSave(TmpDataPath, newTestValue);

            //and change Cache LastWriteTime to be newer than Data LastWriteTime  
            FileInfo cachedFileInfo = new FileInfo(TmpCachePath);
            FileInfo pathFileInfo = new FileInfo(TmpDataPath);
            cachedFileInfo.LastWriteTime = new DateTime(pathFileInfo.LastWriteTime.Ticks).Add(new System.TimeSpan(0, 0, 5, 0));

            //double check that Data is older than Cache, just in case
            Assert.IsTrue(pathFileInfo.LastWriteTime < cachedFileInfo.LastWriteTime);

            //6. Deserialize object this time it should use cache... so it should have original value
            TestSerializableObject deseralizedObj2 = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.AreEqual(originalValue, deseralizedObj2.Test);
        }

        [TestMethod]
        public void DeserializeGracefullyFailsOnDataError()
        {
            TestSerializableObject obj = new TestSerializableObject();

            string originalValue = "original value 123";
            obj.Test = originalValue;
            TestCacheSerializer.Serialize(obj);

            //2. Delete the cachePath
            AppContext.StreamManager.CloseStream(TmpCachePath); //disposes and removes cache stream

            using (BinaryWriter writer = new BinaryWriter(new FileStream(TmpCachePath, FileMode.Open)))
            {
                string foo = "foo";
                writer.Write(foo);
                writer.Flush();
            }

            // Now that it is closed, we should be able to repeat the process and successfully delete the bad cache file.
            obj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.IsNotNull(obj);
            Assert.AreEqual(originalValue, obj.Test);
            Assert.IsTrue(File.Exists(TmpCachePath));

            // Deserializing it should have forced the cache file back to the correct data.
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (Stream stream = new FileStream(TmpCachePath, FileMode.Open))
            {
                obj = (TestSerializableObject)formatter.Deserialize(stream);
            }

            Assert.AreEqual(originalValue, obj.Test);
        }

        [TestMethod]
        public void DeserializeFailsWhenDataFileDoesntExist()
        {
            TestSerializableObject obj = new TestSerializableObject();

            string originalValue = "original value 123";
            obj.Test = originalValue;
            TestCacheSerializer.Serialize(obj);

            //2. Delete the cachePath
            AppContext.StreamManager.CloseStream(TmpCachePath); //disposes and removes cache stream
            File.Delete(TmpDataPath);

            // After deleting the data file, attempting to deserialize the object should return
            // a null object.
            obj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void DeserializeBadDataFails()
        {
            TestSerializableObject obj = new TestSerializableObject();

            string originalValue = "original value 123";
            obj.Test = originalValue;
            TestCacheSerializer.Serialize(obj);

            //2. Delete the cachePath
            AppContext.StreamManager.CloseStream(TmpCachePath); //disposes and removes cache stream

            // Sleep, just to cause the file write times to be different.
            System.Threading.Thread.Sleep(15);

            using (XmlWriter writer = XmlWriter.Create(TmpDataPath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Foo");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            // Attempting to deserialize the invalidated data file should delete the file
            obj = TestCacheSerializer.Deserialize<TestSerializableObject>();
            Assert.IsNull(obj);
            Assert.IsFalse(File.Exists(TmpDataPath));
        }

        [TestMethod]
        public void GetByteRepresentationTest()
        {
            TestSerializableObject obj = new TestSerializableObject();

            string originalValue = "original value 123";
            obj.Test = originalValue;
            TestCacheSerializer.Serialize(obj);

            // Attempting to deserialize the invalidated data file should delete the file
            var bytes = TestCacheSerializer.GetByteRepresentation();

            //attempt to deserialize from bytes back to original object
            var binaryFormatter = new BinaryFormatter();

            TestSerializableObject obj2 = binaryFormatter.Deserialize(new MemoryStream(bytes)) as TestSerializableObject;

            Assert.AreEqual(obj, obj2);
        }

        [TestMethod]
        public void GetByteRepresentationEmptyStreamTest()
        {
            // GetByteRepresentationEmptyStream shall return null if the byte stream is empty
            var bytes = TestCacheSerializer.GetByteRepresentation();
            Assert.IsNull(bytes);
        }
        
        /// <summary>
        /// Works for xml file of serialized TestSerializableObject 
        /// </summary>
        /// <param name="path">path to xml file</param>
        /// <param name="newTestValue">new value for the Test property of TestSerializableObject</param>
        private void ModifyXMLAndSave(string path, string newTestValue)
        {
            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(path);
            XmlNode node = myXmlDocument.DocumentElement;

            //find Test node
            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.Name.Equals("Test"))
                {
                    node1.InnerText = newTestValue;
                }
            }

            myXmlDocument.Save(path);
        }

        /// <summary>
        /// Supporting class... this class should not be serializable
        /// </summary>
        public class TestNotSerializableObject
        {
            public string Test
            {
                get;
                set;
            }

            public TestNotSerializableObject()
            {

            }
        }

        /// <summary>
        /// Supporting class... this class should be serializable
        /// </summary>
        [Serializable]
        public class TestSerializableObject
        {
            public string Test
            {
                get;
                set;
            }

            public TestSerializableObject()
            {
            }

            public override bool Equals(object obj)
            {
                TestSerializableObject other = obj as TestSerializableObject;
                if (other == null)
                    return false;

                bool isEqual = true;
                isEqual &= object.Equals(Test, other.Test);

                return isEqual;
            }

            public override int GetHashCode()
            {
                int typeHash = Test != null ? Test.GetHashCode() : 0;
                return typeHash;
            }

        }
    }
}

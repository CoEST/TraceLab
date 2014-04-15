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
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLabSDK;
using System.IO;
using System.Xml.Serialization;
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using TraceLab.Core.Exceptions;

namespace TraceLab.Core.Test.Workspaces
{
    [TestClass]
    public class WorkspaceTest
    {
        private Workspace TestWorkspace
        {
            get;
            set;
        }

        private Guid CurrentWorkFlow
        {
            get;
            set;
        }

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

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
        }

        [ClassCleanup]
        public static void Cleanup()
        {
        }

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);

            TmpDataDir = Path.Combine(AppContext.BaseTestDirectory, "workspace");
            TmpCacheDir = Path.Combine(AppContext.BaseTestDirectory, "cache");
            TmpTypeDir = Path.Combine(AppContext.BaseTestDirectory, "s_metricTypes");

            CurrentWorkFlow = Guid.NewGuid();
            ResetWorkspace();
        }

        private void ResetWorkspace()
        {
            TestWorkspace = AppContext.WorkspaceInstance;
            TestWorkspace.Reset();
            TestWorkspace.RegisterType(typeof(TestObject));
        }

        [TestCleanup]
        public void TestTeardown()
        {
            //clear directories
            if (Directory.Exists(TmpDataDir))
                Directory.Delete(TmpDataDir, true);
            if (Directory.Exists(TmpCacheDir))
                Directory.Delete(TmpCacheDir, true);

            AppContext.Dispose();
            AppContext = null;
        }

        private string Unitname = "testname";
        private static string TmpDataDir;
        private static string TmpCacheDir;
        private static string TmpTypeDir;

        [TestMethod]
        public void TestWorkspaceConstructorDirectoryCreation()
        {
            //assure path does not exist
            if (Directory.Exists(TmpDataDir))
                Directory.Delete(TmpDataDir, true);
            if (Directory.Exists(TmpCacheDir))
                Directory.Delete(TmpCacheDir, true);

            Assert.IsFalse(Directory.Exists(TmpDataDir));
            Assert.IsFalse(Directory.Exists(TmpCacheDir));

            new Workspace(TmpDataDir, TmpCacheDir, TraceLab.Core.Workspaces.Serialization.StreamManager.CreateNewManager());

            Assert.IsTrue(Directory.Exists(TmpDataDir));
            Assert.IsTrue(Directory.Exists(TmpCacheDir));
        }

        #region Store Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoreTestNullUnitname()
        {
            TestWorkspace.Store(null, new TestObject());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreTestEmptyUnitname()
        {
            TestWorkspace.Store("", new TestObject());
        }

        [TestMethod]
        [ExpectedException(typeof(WorkspaceException))]
        public void StoreTestNotSerializableObject()
        {
            TestWorkspace.Store(Unitname, new NotSerializableObject());
        }

        [TestMethod]
        [ExpectedException(typeof(WorkspaceException))]
        public void StoreTestSerializableObjectButNoParametlessConstructor()
        {
            TestWorkspace.Store(Unitname, new SerializableObjectButNoParametlessConstructor("test"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreTestNullObject()
        {
            TestWorkspace.Store(Unitname, null);
        }

        #endregion

        #region Load Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadTestNullUnitname()
        {
            TestWorkspace.Load(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadTestEmptyUnitname()
        {
            TestWorkspace.Load("");
        }

        /// <summary>
        /// Tests load in situation when data is not yet in memory but Data xml file exists in the workspace directory
        /// </summary>
        [TestMethod]
        public void LoadTestFromXML()
        {
            //set workspace so that it writes units to disc, otherwise files would not be written to disc,
            //so the persistence the units would not be persistent after resetting the workspace
            TestWorkspace.WriteUnitsToDisc = true;

            TestObject obj = new TestObject();
            obj.Value = "value";

            string tmp_unitname = "testobject";

            //string tmpDataPath = Path.Combine(TmpDataDir, TestWorkspace.CurrentGroupId.ToString() + "." + tmp_unitname + ".xml");
            string tmpDataPath = Path.Combine(TmpDataDir, tmp_unitname + ".xml");

            WorkspaceUnit tmpUnit = TestWorkspace.CreateWorkspaceUnit(tmp_unitname);
            tmpUnit.Data = obj;

            // Reset the workspace - this will cause it to realize that
            // the workspace unit exists, even though it's not loaded yet.
            ResetWorkspace();

            TestObject obj2 = (TestObject)TestWorkspace.Load(tmp_unitname);
            Assert.AreEqual(obj, obj2);
        }


        [TestMethod]
        public void LoadTestUnitDoesNotExist()
        {
            //if does not exists in the workspace returns null
            TestObject obj2 = (TestObject)TestWorkspace.Load("FakeName");
            Assert.IsNull(obj2);
        }

        #endregion

        [TestMethod]
        public void StoreAndLoadTest()
        {
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspace.RegisterType(typeof(TestObject));
            TestWorkspace.Store(Unitname, obj);

            TestObject obj2 = (TestObject)TestWorkspace.Load(Unitname);
            Assert.AreEqual(obj, obj2);
        }

        #region GetDateTimeStamp Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDateTimestampNullUnitname()
        {
            TestWorkspace.GetDateTimeStamp(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDateTimestampEmptyUnitname()
        {
            TestWorkspace.GetDateTimeStamp("");
        }

        #endregion

        #region RenameUnit Tests

        [TestMethod]
        public void TestRenameUnit()
        {
            string oldUnitName = Unitname;
            string newUnitName = "mockUnitname";

            //store test object in the workspace using old name
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspace.RegisterType(typeof(TestObject));
            TestWorkspace.Store(oldUnitName, obj);

            //rename unit
            TestWorkspace.RenameUnit(oldUnitName, newUnitName);

            Assert.IsFalse(TestWorkspace.Exists(oldUnitName));
            Assert.IsTrue(TestWorkspace.Exists(newUnitName));
            
            //check if loading obj using old name returns null
            TestObject oldObj = (TestObject)TestWorkspace.Load(Unitname);
            Assert.IsNull(oldObj);

            //check if we can load object by new name
            TestObject obj2 = (TestObject)TestWorkspace.Load(newUnitName);
            Assert.AreEqual(obj, obj2);
        }

        [TestMethod]
        public void TestRenameUnitIfUnitWithNewNameAlreadyExists()
        {
            string oldUnitName = Unitname;
            string newUnitName = "mockUnitname";

            //store test object in the workspace using old name
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspace.RegisterType(typeof(TestObject));
            TestWorkspace.Store(oldUnitName, obj);

            //store another test object in the workspace using new name
            TestObject anotherObj = new TestObject();
            anotherObj.Value = "different object";
            TestWorkspace.Store(newUnitName, anotherObj);
            
            //rename the old unitname object to the new unit name - it should override previous unit
            TestWorkspace.RenameUnit(oldUnitName, newUnitName);

            Assert.IsFalse(TestWorkspace.Exists(oldUnitName));
            Assert.IsTrue(TestWorkspace.Exists(newUnitName));
            
            //check if loading obj using old name returns null
            TestObject oldObj = (TestObject)TestWorkspace.Load(Unitname);
            Assert.IsNull(oldObj);

            //check if we can load object by new name
            TestObject obj2 = (TestObject)TestWorkspace.Load(newUnitName);
            Assert.AreEqual(obj, obj2);
        }

        [TestMethod]
        public void TestRenameUnitWriteToDisc()
        {
            AppContext.WorkspaceInstance.WriteUnitsToDisc = true;

            string oldUnitName = Unitname;
            string newUnitName = "mockUnitname";

            string cacheOldNameFile = System.IO.Path.Combine(AppContext.CacheDirectory, oldUnitName + ".cache");
            string cacheNewNameFile = System.IO.Path.Combine(AppContext.CacheDirectory, newUnitName + ".cache");

            //store test object in the workspace using old name
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspace.RegisterType(typeof(TestObject));
            TestWorkspace.Store(oldUnitName, obj);

            //check if the cache file exists
            Assert.IsTrue(System.IO.File.Exists(cacheOldNameFile));

            //rename unit
            TestWorkspace.RenameUnit(oldUnitName, newUnitName);

            Assert.IsFalse(TestWorkspace.Exists(oldUnitName));
            Assert.IsTrue(TestWorkspace.Exists(newUnitName));

            //check if old file does not exist anymore, and new exists
            Assert.IsFalse(System.IO.File.Exists(cacheOldNameFile));
            Assert.IsTrue(System.IO.File.Exists(cacheNewNameFile));

            //check if loading obj using old name returns null
            TestObject oldObj = (TestObject)TestWorkspace.Load(Unitname);
            Assert.IsNull(oldObj);

            //check if we can load object by new name
            TestObject obj2 = (TestObject)TestWorkspace.Load(newUnitName);
            Assert.AreEqual(obj, obj2);
        }

        [TestMethod]
        public void TestRenameUnitIfUnitWithNewNameAlreadyExistsWriteCacheToDisc()
        {
            AppContext.WorkspaceInstance.WriteUnitsToDisc = true;

            string oldUnitName = Unitname;
            string newUnitName = "mockUnitname";

            string cacheOldNameFile = System.IO.Path.Combine(AppContext.CacheDirectory, oldUnitName + ".cache");
            string cacheNewNameFile = System.IO.Path.Combine(AppContext.CacheDirectory, newUnitName + ".cache");

            //store test object in the workspace using old name
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspace.RegisterType(typeof(TestObject));
            TestWorkspace.Store(oldUnitName, obj);

            //store another test object in the workspace using new name
            TestObject anotherObj = new TestObject();
            anotherObj.Value = "different object";
            TestWorkspace.Store(newUnitName, anotherObj);

            //check if the cache file exists
            Assert.IsTrue(System.IO.File.Exists(cacheOldNameFile));
            Assert.IsTrue(System.IO.File.Exists(cacheNewNameFile));

            //rename the old unitname object to the new unit name - it should override previous unit
            TestWorkspace.RenameUnit(oldUnitName, newUnitName);

            Assert.IsFalse(TestWorkspace.Exists(oldUnitName));
            Assert.IsTrue(TestWorkspace.Exists(newUnitName));

            //check if old file does not exist anymore, and new exists
            Assert.IsFalse(System.IO.File.Exists(cacheOldNameFile));
            Assert.IsTrue(System.IO.File.Exists(cacheNewNameFile));

            //check if loading obj using old name returns null
            TestObject oldObj = (TestObject)TestWorkspace.Load(Unitname);
            Assert.IsNull(oldObj);

            //check if we can load object by new name
            TestObject obj2 = (TestObject)TestWorkspace.Load(newUnitName);
            Assert.AreEqual(obj, obj2);
        }

        #endregion

        #region CopyUnit Tests

        [TestMethod]
        public void TestCopyUnit()
        {
            string fromUnitName = Unitname;
            string toUnitName = "mockUnitname";

            //store test object in the workspace using old name
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspace.RegisterType(typeof(TestObject));
            TestWorkspace.Store(fromUnitName, obj);

            //copy unit
            TestWorkspace.CopyUnit(fromUnitName, toUnitName);

            //both units should exists
            Assert.IsTrue(TestWorkspace.Exists(fromUnitName));
            Assert.IsTrue(TestWorkspace.Exists(toUnitName));

            //load old object
            TestObject oldObj = (TestObject)TestWorkspace.Load(Unitname);
            Assert.IsNotNull(oldObj);
            Assert.AreEqual(obj, oldObj);

            //check if we can load object by new name
            TestObject obj2 = (TestObject)TestWorkspace.Load(toUnitName);
            Assert.AreEqual(obj, obj2);

            //change one object, store it and test again
            oldObj.Value = "another value";
            TestWorkspace.Store(fromUnitName, oldObj);

            //check if new object has not been affected
            obj2 = (TestObject)TestWorkspace.Load(toUnitName);
            Assert.AreEqual(obj, obj2); //it should still be equal to original object
        }

        #endregion

        /// <summary>
        /// Converts the return paramter into an object of the appropriate ParameterType.
        /// If it can not find an appropriate conversion returns null
        /// </summary>
        /// <param name="pInfo">The parameterInfo to convert into an object.</param>
        /// <returns>A random object that is of type ParameterInfo.ParameterType</returns>
        private object CreateParameterObj(ParameterInfo pInfo)
        {
            object returnParamObj = null;

            if (pInfo.ParameterType == typeof(bool))
            {
                returnParamObj = true;
            }
            else if (pInfo.ParameterType == typeof(int))
            {
                returnParamObj = 1;
            }
            else if (pInfo.ParameterType == typeof(string))
            {
                returnParamObj = "test";
            }
            else if (pInfo.ParameterType.IsClass)
            {
                returnParamObj = null;
            }
            else if (pInfo.ParameterType == typeof(Type))
            {
                returnParamObj = typeof(Type);
            }
            else if (pInfo.ParameterType == typeof(object))
            {
                returnParamObj = 55;
            }

            return returnParamObj;
        }
    }

    [Serializable]
    public class TestObject
    {
        public TestObject() { }

        public string Value
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            TestObject other = obj as TestObject;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(Value, other.Value);

            return isEqual;
        }

        public override int GetHashCode()
        {
            int typeHash = Value != null ? Value.GetHashCode() : 0;
            return typeHash;
        }
    }

    public class NotSerializableObject
    {
        public NotSerializableObject() { }

        public string Value
        {
            get;
            set;
        }
    }

    [Serializable]
    public class SerializableObjectButNoParametlessConstructor
    {
        public SerializableObjectButNoParametlessConstructor(string test)
        {
            this.Value = test;
        }

        public string Value
        {
            get;
            set;
        }
    }
}

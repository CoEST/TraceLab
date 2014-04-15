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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Components;
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.Test.Workspaces
{
    [TestClass]
    public class WorkspaceWrapperTest
    {
        private WorkspaceWrapper TestWorkspaceWrapper
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
            ResetWorkspaceWrapper();
        }

        private static string TmpDataDir;
        private static string TmpCacheDir;
        private static string TmpTypeDir;

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

        private static string Unitname = "testobject";
        private static string Unitname2 = "testinteger";

        private void ResetWorkspaceWrapper()
        {
            AppContext.WorkspaceInstance.Reset();
            AppContext.WorkspaceInstance.RegisterType(typeof(TestObject));

            IOSpec mockIoSpec = new IOSpec();
            mockIoSpec.Input.Add(Unitname, new IOItem(new IOItemDefinition(Unitname, typeof(TestObject).FullName, "mockdescription", TraceLabSDK.IOSpecType.Input), Unitname));
            mockIoSpec.Output.Add(Unitname, new IOItem(new IOItemDefinition(Unitname, typeof(TestObject).FullName, "mockdescription", TraceLabSDK.IOSpecType.Output), Unitname));
            mockIoSpec.Input.Add(Unitname2, new IOItem(new IOItemDefinition(Unitname2, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Input), Unitname2));
            mockIoSpec.Output.Add(Unitname2, new IOItem(new IOItemDefinition(Unitname2, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Output), Unitname2));

            TestWorkspaceWrapper = new WorkspaceWrapper(mockIoSpec, AppContext.WorkspaceInstance);
        }

        /// <summary>
        /// Tests storing the and loading through the workspace wrapper.
        /// </summary>
        [TestMethod]
        public void StoreAndLoadTest()
        {
            TestObject obj = new TestObject();
            obj.Value = "value";
            TestWorkspaceWrapper.Store(Unitname, obj);

            TestObject obj2 = (TestObject)TestWorkspaceWrapper.Load(Unitname);
            Assert.AreEqual(obj, obj2);

            int test = 5;
            TestWorkspaceWrapper.Store(Unitname2, test);
            int test2 = (int)TestWorkspaceWrapper.Load(Unitname2);
            Assert.AreEqual(test, test2);

            test2++;

            TestWorkspaceWrapper.Store(Unitname2, test2);
            
            int test3 = (int)TestWorkspaceWrapper.Load(Unitname2);
            Assert.AreEqual(test2, test3);
        }
    }
}

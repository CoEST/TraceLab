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
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;
using System.IO;
using TraceLab.Core.Test.ExperimentExecution;
using TraceLab.Core.Experiments;

namespace TraceLab.Core.Test.Workspaces
{
    [TestClass]
    public class NestedWorkspaceWrapperTest
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

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
            TmpDataDir = Path.Combine(AppContext.BaseTestDirectory, "workspace");
            TmpCacheDir = Path.Combine(AppContext.BaseTestDirectory, "cache");
            TmpTypeDir = Path.Combine(AppContext.BaseTestDirectory, "s_metricTypes");
            
            //assure that workspace is empty
            AppContext.WorkspaceInstance.Clear();
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

        private const string DOT = Workspace.NAMESPACE_DELIMITER;

        [TestMethod]
        public void NestedWorkspaceWrapperSetupAndTearDownTest()
        {
            string experimentNamespace = "experiment_namespace";
            ExperimentWorkspaceWrapper experimentWorkspaceWrapper = new ExperimentWorkspaceWrapper(AppContext.WorkspaceInstance, experimentNamespace);

            // let store some objects in the workspace using experiment wrapper
            // note that experiment wrapper does not have any restriction and can store anything in workspace - it has direct access to Workspace
            string unitname1 = "testunitname1";
            string unitname2 = "testunitname2";

            TestObject object1 = new TestObject();
            object1.Value = "some value";

            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(unitname1));
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname1));
            experimentWorkspaceWrapper.Store(unitname1, object1);
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(unitname1));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname1));

            int testInt = 5;
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(unitname2));
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname2));
            experimentWorkspaceWrapper.Store(unitname2, testInt);
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(unitname2));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname2));

            //prepare io spec for first nested workspace
            IOSpec mockIoSpec1 = new IOSpec();
            string nest1unitname1 = "nest1unitname1"; //input nest1unitname1 from unitname1
            string nest1unitname2 = "nest1unitname2"; //input nest1unitname2 from unitname2

            mockIoSpec1.Input.Add(nest1unitname1, new IOItem(new IOItemDefinition(nest1unitname1, typeof(TestObject).FullName, "mockdescription", TraceLabSDK.IOSpecType.Input), unitname1));
            mockIoSpec1.Input.Add(nest1unitname2, new IOItem(new IOItemDefinition(nest1unitname2, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Input), unitname2));
            
            string nestedWorkspace1Namespace = "nested_workspace1";
            NestedWorkspaceWrapper nestedWorkspace1 = new NestedWorkspaceWrapper(mockIoSpec1, experimentWorkspaceWrapper, nestedWorkspace1Namespace);

            //prepare io spec for 2nd nested workspace
            IOSpec mockIoSpec2 = new IOSpec();
            string nest2unitname1 = "nest2unitname1"; //input nest2unitname1 from nest1unitname1
            string nest2unitname2 = "nest2unitname2"; //input nest2unitname2 from nest1unitname2

            //output matters only in teardown
            string unitname3 = "unitname3";
            string unitname3_OutputAS = "nest2unitname3";
            string localScopeUnitname = "localScope";

            mockIoSpec2.Input.Add(nest2unitname1, new IOItem(new IOItemDefinition(nest2unitname1, typeof(TestObject).FullName, "mockdescription", TraceLabSDK.IOSpecType.Input), nest1unitname1));
            mockIoSpec2.Input.Add(nest2unitname2, new IOItem(new IOItemDefinition(nest2unitname2, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Input), nest1unitname2));
            mockIoSpec2.Output.Add(unitname3, new IOItem(new IOItemDefinition(unitname3, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Output), unitname3_OutputAS));

            string nestedWorkspace2Namespace = "nested_workspace2";
            NestedWorkspaceWrapper nestedWorkspace2 = new NestedWorkspaceWrapper(mockIoSpec2, nestedWorkspace1, nestedWorkspace2Namespace);

            IOSpec mockIoSpec3 = new IOSpec();
            mockIoSpec3.Output.Add(unitname3, new IOItem(new IOItemDefinition(unitname3, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Output), unitname3));
            mockIoSpec3.Output.Add(localScopeUnitname, new IOItem(new IOItemDefinition(localScopeUnitname, typeof(int).FullName, "mockdescription", TraceLabSDK.IOSpecType.Output), localScopeUnitname));
            WorkspaceWrapper workspaceWrapper = new WorkspaceWrapper(mockIoSpec3, nestedWorkspace2);

            // call in order 
            // 1. setup nested workspace 1 
            // 2. setup nested workspace 2 
            // 3. store a object with unitname3 using workspace wrapper, that is output to the higher level scope
            // 4. store another object, that is NOT output to higher level scope and should be discarded in teardown
            // 4. tear down workspace 2 
            // 5. tear down workspace 1

            nestedWorkspace1.Setup();
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname1)); //it actually should stay in experiment scope
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname2));
            Assert.IsTrue(experimentWorkspaceWrapper.Exists(unitname1));
            Assert.IsTrue(experimentWorkspaceWrapper.Exists(unitname2));

            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname1));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname2));
            Assert.IsTrue(nestedWorkspace1.Exists(nest1unitname1));
            Assert.IsTrue(nestedWorkspace1.Exists(nest1unitname2));

            nestedWorkspace2.Setup();
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname1));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname2));
            Assert.IsTrue(nestedWorkspace1.Exists(nest1unitname1));
            Assert.IsTrue(nestedWorkspace1.Exists(nest1unitname2));

            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + nest2unitname1));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + nest2unitname2));
            Assert.IsTrue(nestedWorkspace2.Exists(nest2unitname1));
            Assert.IsTrue(nestedWorkspace2.Exists(nest2unitname2));

            //store unit 3
            int unit3 = 10;
            workspaceWrapper.Store(unitname3, unit3);
            //it should store it with namespace
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + unitname3));

            //store localscope variable
            int localScopeUnit = 25;
            workspaceWrapper.Store(localScopeUnitname, localScopeUnit);
            //it should store it with namespace
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + localScopeUnitname));

            nestedWorkspace2.TearDown();
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + nest2unitname1));
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + nest2unitname2));
            Assert.IsFalse(nestedWorkspace2.Exists(nest2unitname1));
            Assert.IsFalse(nestedWorkspace2.Exists(nest2unitname2));
            
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname1));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname2));
            Assert.IsTrue(nestedWorkspace1.Exists(nest1unitname1));
            Assert.IsTrue(nestedWorkspace1.Exists(nest1unitname2));

            //unitname 3 should be tear down and renamed to unitnameOutpusAs with proper namespace
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + unitname3));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + unitname3_OutputAS));

            //local scope should also be cleared, although it was not output to higher level scope
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nestedWorkspace2Namespace + DOT + localScopeUnitname));

            nestedWorkspace1.TearDown();
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname1));
            Assert.IsFalse(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + nestedWorkspace1Namespace + DOT + nest1unitname2));
            Assert.IsFalse(nestedWorkspace1.Exists(nest1unitname1));
            Assert.IsFalse(nestedWorkspace1.Exists(nest1unitname2));
            
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname1));
            Assert.IsTrue(AppContext.WorkspaceInstance.Exists(experimentNamespace + DOT + unitname2));
            Assert.IsTrue(experimentWorkspaceWrapper.Exists(unitname1));
            Assert.IsTrue(experimentWorkspaceWrapper.Exists(unitname2));
        }

        /// <summary>
        /// Nesteds the workspace wrapper test for the bug 180.
        /// After running the experiment, there should be only three units in the workspace
        /// a = 10
        /// b = 5
        /// y = 6
        /// 
        /// If the output is wrong, or there are more than three units the test fails.
        /// The experiment uses 'incrementer of two values.tcml' and 'wrapped incrementer of two values.tcml' composite components.
        /// </summary>
        [TestMethod]
        public void NestedWorkspaceWrapperTest_Bug180()
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = ExperimentManager.Load("compositecomponentbug180.teml", AppContext.Components);

            using (var dispatcher = ExperimentRunnerHelper.CreateExperimentRunner(experiment, AppContext.WorkspaceInstance, AppContext.Components))
            {
                dispatcher.ExecuteExperiment(progress);
            }

            Assert.AreEqual(3, AppContext.WorkspaceInstance.Units.Count);
            foreach (WorkspaceUnit unit in AppContext.WorkspaceInstance.Units)
            {
                bool isCorrect = false;
                if(unit.FriendlyUnitName.Equals("a") && (int)unit.Data == 10) isCorrect = true;
                else if(unit.FriendlyUnitName.Equals("b") && (int)unit.Data == 5) isCorrect = true;
                else if(unit.FriendlyUnitName.Equals("y") && (int)unit.Data == 6) isCorrect = true;

                Assert.IsTrue(isCorrect);
            }
        }
    }
}

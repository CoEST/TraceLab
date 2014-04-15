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
    public class ScopeNestedWorkspaceWrapperTest
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

        /// <summary>
        /// Scope nested workspace wrapper need to do setup and teardown differently than standard NestedWorkspaceWrapper.
        /// In setup phase, all currently available items in the workspace should be copied into the lower level workspace.
        /// In teardown phase, units that have been modified should be renamed into upper level workspace.
        /// 
        /// After running the test experiment, the workspace should contain only following units:
        /// a = 1
        /// b = 7
        /// 
        /// If the output is different wrong, the test fails.
        /// </summary>
        [TestMethod]
        public void ScopeNestedWorkspaceWrapperTearDownTest()
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = ExperimentManager.Load("DecisionScopeTest.teml", AppContext.Components);

            using (var dispatcher = ExperimentRunnerHelper.CreateExperimentRunner(experiment, AppContext.WorkspaceInstance, AppContext.Components))
            {
                dispatcher.ExecuteExperiment(progress);
            }

            Assert.AreEqual(2, AppContext.WorkspaceInstance.Units.Count);
            foreach (WorkspaceUnit unit in AppContext.WorkspaceInstance.Units)
            {
                bool isCorrect = false;
                if (unit.FriendlyUnitName.Equals("a") && (int)unit.Data == 1) isCorrect = true;
                else if (unit.FriendlyUnitName.Equals("b") && (int)unit.Data == 7) isCorrect = true;

                Assert.IsTrue(isCorrect);
            }
        }

        /// <summary>
        /// Testing correct behavior of scope nested workspace wrapper tear down,
        /// in case there is composite component inside scope.
        /// </summary>
        [TestMethod]
        public void ScopeWithCompositeComponent_ScopeNestedWorkspaceWrapperTearDownTest()
        {
            MockProgress progress = new MockProgress();

            string testExperimentFilepath = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Decision Scope with composite component.teml");
            Experiment experiment = ExperimentManager.Load(testExperimentFilepath, AppContext.Components);

            using (var dispatcher = ExperimentRunnerHelper.CreateExperimentRunner(experiment, AppContext.WorkspaceInstance, AppContext.Components))
            {
                dispatcher.ExecuteExperiment(progress);
            }

            Assert.AreEqual(3, AppContext.WorkspaceInstance.Units.Count);

            HashSet<string> expectedUnitNames = new HashSet<string>();
            expectedUnitNames.Add("targetArtifacts");
            expectedUnitNames.Add("sourceArtifacts");
            expectedUnitNames.Add("similarityMatrix");

            foreach (WorkspaceUnit unit in AppContext.WorkspaceInstance.Units)
            {
                bool isCorrect = false;
                if (expectedUnitNames.Contains(unit.FriendlyUnitName)) isCorrect = true;
                Assert.IsTrue(isCorrect);
            }
        }
    }
}

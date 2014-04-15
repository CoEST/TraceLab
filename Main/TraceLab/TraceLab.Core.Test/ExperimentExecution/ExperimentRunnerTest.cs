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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Experiments;
using TraceLab.Core.Test.Setup;
using TraceLabSDK;
using TraceLab.Core.Workspaces;
using TraceLab.Core.Components;

namespace TraceLab.Core.Test.ExperimentExecution
{
    [TestClass]
    public class DispatcherTest
    {
        public DispatcherTest()
        {
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
        public static void DispatcherSetup(TestContext context)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            AppContext.Dispose();
        }
        
        /// <summary>
        /// Loads the test experiment file
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private Experiment LoadExperiment(string filename)
        {
            filename = System.IO.Path.Combine(AppContext.BaseTestDirectory, filename);
            if (System.IO.File.Exists(filename) == false)
            {
                throw new System.IO.FileNotFoundException("Graph file not found", filename);
            }

            Experiment currentExperiment = ExperimentManager.Load(filename, AppContext.Components);

            return currentExperiment;
        }

        /// <summary>
        /// Runs the experiment asynchronously... it will wait till the experiment runner thread completes before returning.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <param name="progress">The progress.</param>
        private void RunExperimentAsync(Experiment experiment, IProgress progress, Workspace workspace, ComponentsLibrary library)
        {
            using (var waiter = new System.Threading.ManualResetEventSlim())
            {
                experiment.RunExperiment(progress, workspace, library);
                experiment.ExperimentCompleted += (sender, args) =>
                {
                    waiter.Set();
                };

                waiter.Wait();
            }
        }

        /// <summary>
        /// Positive the most basic example - just to check if running experiment works with no errors.
        /// </summary>
        [TestMethod]
        public void TestExecuteExperiment() 
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("basic_graph.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.AreEqual(4, progress.NumSteps);
            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
        }

        /// <summary>
        /// Check that the experiment nodes can log information.
        /// </summary>
        [TestMethod]
        public void TestExecuteExperimentLogs()
        {
            var target = new MockLogTarget();
            var logConfig = new NLog.Config.LoggingConfiguration();
            logConfig.AddTarget("MockLogger", target);
            NLog.LogManager.Configuration = logConfig;

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("basic_graph.teml");

            using (var dispatcher = ExperimentRunnerHelper.CreateExperimentRunner(experiment, AppContext.WorkspaceInstance, AppContext.Components))
            {
                dispatcher.ExecuteExperiment(progress);
            }
            // only two nodes log, because the other two are start/end
            Assert.AreEqual(2, target.NumEvents); 
        }


        /// <summary>
        /// Error because one of the components cannot be initialized, since it is not in the components library.
        /// </summary>
        [TestMethod]
        public void TestExecuteExperimentErrorCannotInitializeComponent()
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("basic_graph_with_non_existing_component.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsTrue(progress.HasError);

            Assert.AreEqual(Messages.ExperimentRunnerErrorMessage, progress.CurrentStatus);
            
        }

        /// <summary>
        /// Testing decision loops
        /// 
        /// Graph is following:
        ///                                                   test<5              test>5
        ///   Start ----> WriterNode -------> ReaderNode <----------- Decision ----------> end
        ///               (testoutput         (testinput                 ^
        ///                =>> test)           => test)                  |         
        ///                                       |                      | 
        ///                                       |                      |
        ///                                       ----- Incrementer -----
        ///                                             (testinput
        ///                                              => test
        ///                                              
        /// Legend
        /// => means mappedTo
        /// =>> means outputAs
        /// 
        /// In this graph ReaderNode should NOT await for the Decision. 
        /// If this is the case, the experiment will hold on ReaderNode, and will never proceed further.
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public void TestExecuteExperiment_Basic_Loop()
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_basic_loop.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
        }

        /// <summary>
        /// Testing decision loops.
        /// 
        /// Graph is following:
        ///                                                                       test>5
        ///   Start ----> WriterNode -------> ReaderNode -----------> Decision ----------> end
        ///               (testoutput         (testinput                 |
        ///                =>> test)           => test)                  |         
        ///                                       ^                      | test<5
        ///                                       |                      |
        ///                                       ----- Incrementer <----
        ///                                             (testinput
        ///                                              => test
        ///                                              
        /// Legend
        /// => means mappedTo
        /// =>> means outputAs
        /// 
        /// In this graph ReaderNode should NOT await for Incrementer, beacuse it is coming from path starting from Decision. 
        /// If this is the case, the experiment will hold on ReaderNode, and will never proceed further.
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public void TestExecuteExperiment_Complex_Loop()
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_complex_loop.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
        }

        /// <summary>
        /// Testing decision loops.
        /// 
        /// Graph is following:
        ///                                                test<5                             
        ///   Start ----> WriterNode -------> Decision -----------> ReaderNode ----------> END
        ///               (testoutput             |                 (testinput              ^
        ///                =>> test)              |test<5            => test                |
        ///                                       V                                         |
        ///                                 Incrementer ----------> ReaderNode -------------
        ///                                 (testinput              (testinput
        ///                                  => test                 => test
        ///                                              
        /// Legend
        /// => means mappedTo
        /// =>> means outputAs
        /// 
        /// In this graph END should wait only for one node. 
        /// If END nodes waits for both ReaderNodes, the experiment will hold on END, and will never finish.
        /// Notice that instead of END there could be a component node there too...
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestExecuteExperiment_Decision()
        {
            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_decision.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
        }

        /// <summary>
        /// Tests the execution of the experiment with more complex loop, where paths after decision node are splitted
        /// and where one node is supposed to wait for another completion before executing.
        /// It this is not a case then the final results of the experiment is incorrect.
        /// 
        /// The final results is supposed to be 7 = ((0 * 2 + 1) * 2 + 1) * 2 + 1
        /// 
        /// If the 'Adder Node' is not waiting for 'Multiplier Node' the order of addition and multiplication is different and then the result is incorrect.
        /// 
        /// See the graph file itself for more details... You need MockComponent.dll in your Components library to run experiment.
        /// </summary>
        [TestMethod]
        [Timeout(6000)]
        public void TestExecuteExperimentComplexLoopWithManyOutGoingPaths()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();
            
            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_complex_loop_with_many_outgoing_paths.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);

            //the test value after experiment is expected to be equal 7
            int test = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id+".test"); //all values are prefixed with experiment id
            Assert.AreEqual(test, 7);
        }

        /// <summary>
        /// Refer to the test experiment file.
        /// Incrementer 4 has to wait until Incrementer 3 is completed. Note Multiplier is sleeping for 500ms to assure that Incrementer 3 is not being active for a moment.
        /// If Increment 4 is not waiting for Incrementer 3, the final results may be incorrect. 
        /// </summary>
        [TestMethod]
        [Timeout(6000)]
        public void TestExecuteExperimentBasicGraph2()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("basic_graph_2.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
            
            //the test value after experiment is expected to be equal 6
            int test = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".testoutput");
            Assert.AreEqual(test, 6);
        }

        /// <summary>
        /// Refer to the test experiment file. Test for bug #71
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestExecuteExperimentDecisionGraphBug71()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_decision_bug_71.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
            
            //all counters should be equal 3
            int counter1 = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".counter1");
            int counter2 = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".counter2");
            int counter3 = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".counter3");
            Assert.AreEqual(3, counter1);
            Assert.AreEqual(3, counter2);
            Assert.AreEqual(3, counter3);
        }

        /// <summary>
        /// Refer to the test experiment file. Test for bug #72
        /// Test checks if node properly awaits the execution of previous nodes in the 2nd iteration
        /// of the loop. In this case there are two paths incoming from loop, so if the node is not waiting,
        /// then the final result is going to be incorrect.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestExecuteExperimentDecisionGraphTwoNodesComingToDecision()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_loop_two_nodes_coming_to_decision_bug_72.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);

            int test_a = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".test_a");
            Assert.AreEqual(9, test_a);
        }

        /// <summary>
        /// Test for bug #56
        /// Test checks if the Experiment Runner prevents executing graph with 
        /// the decision node that connects to two different components with the same label.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestExecuteExperimentDecisionConnectsToTwoNodesWithSameLabel()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("graph_decision_bug_56.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsTrue(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerErrorMessage, progress.CurrentStatus);
        }

        /// <summary>
        /// Refer to the test experiment file 'experiment_with_composite_component.gml'
        /// Note uses a composite component, that can be found at GenericTestData/TestComponents/composite_component.tcml
        /// If the component is not in library test fails.
        /// The experiment has modified config values, that differ from default values in the composite_component.tcml
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestExecuteExperimentWithCompositeComponent()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("experiment_with_composite_component.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);
            
            // the composite component defined that
            // 'test1' is outputted as test_x
            // 'test2' was not visible in top component, so it should stay with name including composite component node id
            // 'test3' was not visible in top component, so it should stay with name including composite component node id
            // 'test4' is outputted as test_y

            int test_x = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".test_x");
            Assert.AreEqual(10, test_x);

            int test_y = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".test_y");
            Assert.AreEqual(6, test_y);

            string compositeComponentNodeId = "46b30cf2-28e8-40ef-8fae-797255c09dc3";

            object test_1 = AppContext.WorkspaceInstance.Load(compositeComponentNodeId + ".test1");
            Assert.IsNull(test_1); //assure that test_1 is not in workspace, because it should have been renamed as test_x

            object test_2 = AppContext.WorkspaceInstance.Load(compositeComponentNodeId + ".test2");
            Assert.IsNull(test_2);
            
            object test_3 = AppContext.WorkspaceInstance.Load(compositeComponentNodeId + ".test3");
            Assert.IsNull(test_3);

            object test_4 = AppContext.WorkspaceInstance.Load(compositeComponentNodeId + ".test4");
            Assert.IsNull(test_4); //assure that test_1 is not in workspace, because it should have been renamed as test_y
        }

        /// <summary>
        /// Test executing experiment with loop scope node.
        /// Loop is repeated 5 times, thus final integer should be equal to 5 in the workspace
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestExecuteExperimentWithLoopNode()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment experiment = LoadExperiment("LoopExperimentTest.teml");
            RunExperimentAsync(experiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsFalse(progress.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progress.CurrentStatus);

            int integer = (int)AppContext.WorkspaceInstance.Load(experiment.ExperimentInfo.Id + ".integer");
            Assert.AreEqual(5, integer);
        }

        /// <summary>
        /// Test termination of the experiment. 
        /// The experiment has infinite loop, so the test will finish only if the terminate call works.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestTerminateExperiment()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment currentExperiment = LoadExperiment("infinite_loop_terminate_experiment_test.teml");

            RunExperimentAndTerminate(currentExperiment, progress, AppContext.WorkspaceInstance, AppContext.Components);

            Assert.IsTrue(progress.HasError);
            Assert.AreEqual(Messages.ExperimentExecutionTerminated, progress.CurrentStatus);
        }

        /// <summary>
        /// Test termination of the experiment that has composite components. 
        /// The experiment has infinite loop with two composite components, so there is high probability that termination will happen
        /// while executing composite component. Repeat execution twice to higher that probability.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public void TestTerminateExperimentWithCompositeComponents()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            MockProgress progress = new MockProgress();

            Experiment currentExperiment = LoadExperiment("infinite_loop_terminate_experiment_test.teml");
            RunExperimentAndTerminate(currentExperiment, progress, AppContext.WorkspaceInstance, AppContext.Components);
            Assert.IsTrue(progress.HasError);
            Assert.AreEqual(Messages.ExperimentExecutionTerminated, progress.CurrentStatus);

            RunExperimentAndTerminate(currentExperiment, progress, AppContext.WorkspaceInstance, AppContext.Components);
            Assert.IsTrue(progress.HasError);
            Assert.AreEqual(Messages.ExperimentExecutionTerminated, progress.CurrentStatus);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestRunTwoExperimentsSimultanouslyAndTerminateOneExperiment()
        {
            //assure workspace is empty
            AppContext.WorkspaceInstance.Clear();

            var done = new System.Threading.CountdownEvent(2);

            //start experiment one (takes more than 2 seconds to finish successfully)
            Experiment experimentOne = LoadExperiment("long-experiment.teml");
            MockProgress progressOne = new MockProgress();
            experimentOne.RunExperiment(progressOne, AppContext.WorkspaceInstance, AppContext.Components);
            experimentOne.ExperimentCompleted += (sender, args) =>
            {
                done.Signal();
            };

            //start experiment two and terminate it
            MockProgress progressTwo = new MockProgress();
            Experiment experimentTwo = LoadExperiment("infinite_loop_terminate_experiment_test.teml");
            experimentTwo.RunExperiment(progressTwo, AppContext.WorkspaceInstance, AppContext.Components);
            experimentTwo.ExperimentStarted += (sender, args) =>
            {
                experimentTwo.StopRunningExperiment();
            };
            experimentTwo.ExperimentCompleted += (sender, args) =>
            {
                done.Signal();
            };

            //wait for both experiment to end
            done.Wait(5000);

            //the experiment one should have ended successfully (it SHOULD NOT have been terminated)
            Assert.IsFalse(progressOne.HasError);
            Assert.AreEqual(Messages.ExperimentRunnerSuccessMessage, progressOne.CurrentStatus);

            //while exepriment two should be terminated
            Assert.IsTrue(progressTwo.HasError);
            Assert.AreEqual(Messages.ExperimentExecutionTerminated, progressTwo.CurrentStatus);
        }

        private void RunExperimentAndTerminate(Experiment experiment, MockProgress progress, Workspace workspace, ComponentsLibrary library)
        {
            using (var waiter = new System.Threading.ManualResetEventSlim())
            {
                experiment.RunExperiment(progress, workspace, library);
                experiment.ExperimentStarted += (sender, args) =>
                {
                    experiment.StopRunningExperiment();
                };
                experiment.ExperimentCompleted += (sender, args) =>
                {
                    waiter.Set();
                };

                waiter.Wait();
            }
        }

    }

    #region MockProgress

    public class MockProgress : IProgress
    {
        private bool m_isIndeterminate;
        public bool IsIndeterminate
        {
            get
            {
                return m_isIndeterminate;
            }
            set
            {
                m_isIndeterminate = value;
            }
        }

        private double m_numSteps;
        public double NumSteps
        {
            get
            {
                return m_numSteps;
            }
            set
            {
                m_numSteps = value;
            }
        }

        private double m_currentStep;
        public double CurrentStep
        {
            get
            {
                return m_currentStep;
            }
            set
            {
                m_currentStep = value;
            }
        }

        private string m_currentStatus;
        public string CurrentStatus
        {
            get
            {
                return m_currentStatus;
            }
            set
            {
                m_currentStatus = value;
            }
        }

        public void Reset()
        {
            CurrentStep = 0;
            IsIndeterminate = false;
            CurrentStatus = string.Empty;
        }

        public void Increment()
        {
            CurrentStep +=1 ;
        }

        private bool m_hasError;
        public void SetError(bool hasError)
        {
            m_hasError = hasError;
        }

        public bool HasError
        {
            get
            {
                return m_hasError;
            }
        }
    }

    #endregion

}

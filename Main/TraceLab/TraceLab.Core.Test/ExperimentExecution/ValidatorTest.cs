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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Components;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Experiments;
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Decisions;

namespace TraceLab.Core.Test.ExperimentExecution
{
    [TestClass]
    public class ValidatorTest
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

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [ClassCleanup]
        public static void FixtureTeardown()
        {
        }

        private static readonly string EmptyDecisionGUID = Guid.NewGuid().ToString();
        private ComponentMetadataDefinition m_componentReaderMetaDefinition;
        private ComponentMetadataDefinition m_componentWriterMetaDefinition;
        private ComponentMetadataDefinition m_emptyComponentMetaDefinition;
        private ComponentMetadataDefinition m_incrementerComponentMetaDefinition;

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);

            MetadataDefinition definition;
            if (AppContext.Components.TryGetComponentDefinition(TraceLabTestApplication.TestEmptyComponentGUID, out definition))
            {
                m_emptyComponentMetaDefinition = definition as ComponentMetadataDefinition;
            }
            else
            {
                Assert.Fail("Failed to locate component metadatadefinition!");
            }

            if (AppContext.Components.TryGetComponentDefinition(TraceLabTestApplication.TestReaderComponentGUID, out definition))
            {
                m_componentReaderMetaDefinition = definition as ComponentMetadataDefinition;
            }
            else
            {
                Assert.Fail("Failed to locate component metadatadefinition!");
            }

            if (AppContext.Components.TryGetComponentDefinition(TraceLabTestApplication.TestWriterComponentGUID, out definition))
            {
                m_componentWriterMetaDefinition = definition as ComponentMetadataDefinition;
            }
            else
            {
                Assert.Fail("Failed to locate component metadatadefinition!");
            }

            if (AppContext.Components.TryGetComponentDefinition(TraceLabTestApplication.IncrementerComponentGUID, out definition))
            {
                m_incrementerComponentMetaDefinition = definition as ComponentMetadataDefinition;
            }
            else
            {
                Assert.Fail("Failed to locate component metadatadefinition!");
            }
        }

        [TestCleanup]
        public void TestTearDown()
        {

        }

        private DecisionMetadata CompileMockDecision(string decisionCode)
        {
            DecisionMetadata decisionMetadata = new DecisionMetadata("Decision");
            decisionMetadata.DecisionCode = decisionCode;

            DecisionCompilationRunner_Accessor.BuildSourceAndCompileDecisionModule(decisionMetadata, null, null,
                 AppContext.WorkspaceInstance.TypeDirectories, new LoggerNameRoot("Mock"));

            AppContext.DecisionsToClear.Add(decisionMetadata.UniqueDecisionID);

            return decisionMetadata;
        }

        /// <summary>
        /// Test checks if error occurs if there is node that has no path to the END node. 
        /// The most trivial example, that there is any path to End node.
        ///
        ///   Graph is following:
        /// 
        ///   Start -> v1 -> v2       end
        ///     
        /// Error should occur in node2, because it does not have path to End
        /// </summary>
        [TestMethod]
        [DeploymentItem("../Data/GenericTestData/", @"ValidatorTest/GraphStructureValidator_NoPathToEnd")]
        public void GraphStructureValidator_NoPathToEnd()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode node1 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node2 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);

            experiment.AddConnection(experiment.StartNode, node1);
            experiment.AddConnection(node1, node2);

            Assert.IsFalse(node2.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsTrue(template.IsEmpty);

            Assert.IsTrue(node2.HasError);
            Assert.AreEqual(node2.ErrorMessage, "Unable to detect path to the END node.");

        }

        /// <summary>
        /// Test checks if error occurs if there is node that has no path to the END node.
        /// In this case there is one node that does not have path to END, but other nodes have. 
        ///
        ///   Graph is following:
        /// 
        ///   Start -> v1 -> v2 -> end
        ///      |
        ///      --> v3
        ///
        /// Error should occur in node3, because it does not have path to End
        /// 
        /// </summary>
        [TestMethod]
        public void GraphStructureValidator_NoPathToEnd2()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode node1 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node2 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 15, 15);
            ExperimentNode node3 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 25, 25);

            experiment.AddConnection(experiment.StartNode, node1);
            experiment.AddConnection(experiment.StartNode, node3);
            experiment.AddConnection(node1, node2);
            experiment.AddConnection(node2, experiment.EndNode);

            Assert.IsFalse(node1.HasError);
            Assert.IsFalse(node2.HasError);
            Assert.IsFalse(node3.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsTrue(template.IsEmpty);

            Assert.IsTrue(node3.HasError);
            Assert.AreEqual(node3.ErrorMessage, "Unable to detect path to the END node.");

        }

        /// <summary>
        /// Test checks if error occurs if there is circular link in the graph.
        /// 
        ///   Graph is following:
        /// 
        ///   Start -> v1 -> v2 -> v3 -> end
        ///             ^__________|
        ///             
        /// Error should occur in node3
        /// </summary>
        [TestMethod]
        public void GraphStructureValidator_CircularLink()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode node1 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node2 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node3 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);

            experiment.AddConnection(experiment.StartNode, node1);
            experiment.AddConnection(node1, node2);
            experiment.AddConnection(node2, node3);
            experiment.AddConnection(node3, node1);

            Assert.IsFalse(node3.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsTrue(template.IsEmpty);

            Assert.IsTrue(node3.HasError);
            Assert.AreEqual(node3.ErrorMessage, "Circular link detected.");
        }

        /// <summary>
        /// Test checks if validation passes when there is circular link, but it originates in decision node.
        /// This special case for the circular link, that should be accepted. 
        /// Test also validates that all edges are correctly added to the template graph.
        /// 
        /// Graph is following:
        /// 
        ///   Start -> v1 -> v2 -> decision -> end
        ///             ^______________|
        /// 
        /// 
        /// </summary>
        [TestMethod]
        public void GraphStructureValidator_NoCircularLinkIfDecision()
        {
            Experiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode node1 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node2 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 15, 15);
            var decisionNode = AddDecisionToExperiment(experiment, 10, 10);
            decisionNode.Data.Metadata = CompileMockDecision("");

            experiment.AddConnection(experiment.StartNode, node1);
            experiment.AddConnection(node1, node2);
            experiment.AddConnection(node2, decisionNode);
            experiment.AddConnection(decisionNode, node1);
            experiment.AddConnection(decisionNode, experiment.EndNode);

            Assert.IsFalse(node1.HasError);
            Assert.IsFalse(node2.HasError);
            Assert.IsFalse(decisionNode.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);

            Assert.IsFalse(node1.HasError);
            Assert.IsFalse(node2.HasError);
            Assert.IsFalse(decisionNode.HasError);

            Assert.IsFalse(template.IsEmpty);
            Assert.AreEqual(5, template.Nodes.Count);

            //check all  edges
            foreach (RunnableNode node in template.Nodes)
            {
                if (node is RunnableDecisionNode)
                {
                    //in this graph decision node has only two outcoming edges
                    Assert.AreEqual(2, node.NextNodes.Count);
                }
                else if (node is RunnableEndNode)
                {
                    //end node has none outcoming edges
                    Assert.AreEqual(0, node.NextNodes.Count);
                }
                else
                {
                    //all other nodes in this graph has only one outcoming edge
                    Assert.AreEqual(1, node.NextNodes.Count);
                }
            }
        }

        /// <summary>
        /// Test checks if error occurs if there is any node that has input with no matchin output from predecessor nodes.
        /// Basic graph: 
        /// 
        ///   Start -> WriterNode -----> ReaderNode -----> end
        ///           (testoutput)       (testinput 
        ///                               => fakemapping)
        /// 
        /// Legend 
        /// => means mappedTo
        /// 
        /// Error should occur in readerNode because input named 'fakemapping' is not there
        /// </summary>
        [TestMethod]
        public void ValidateInputMapping_NoOutputFound()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode readerNode = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 5, 5);
            ExperimentNode writerNode = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 5, 5);

            experiment.AddConnection(experiment.StartNode, writerNode);
            experiment.AddConnection(writerNode, readerNode);
            experiment.AddConnection(readerNode, experiment.EndNode);

            (readerNode.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "fakemapping";

            Assert.IsFalse(readerNode.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsTrue(template.IsEmpty);

            Assert.IsTrue(readerNode.HasError);
            Assert.AreEqual(readerNode.ErrorMessage, "The component attempts to load 'fakemapping' from the Workspace. However, none of the previous components outputs 'fakemapping' to the Workspace.");
        }

        /// <summary>
        /// Test checks if error occurs if there is any node that has input with no matchin output from predecessor nodes.
        /// Graph failed, when the BFS algorithm was used. 
        /// 
        /// Graph:
        /// 
        ///   Start -> WriterNode -----> ReaderNode2 -----------> end
        ///     |      (testoutput        (testinput               ^
        ///     |       =>>fakemapping)     => fakemapping)        |
        ///     |                                                  |
        ///     --> ReaderNode1 -----------------------------------
        ///         (testinput
        ///          => fakemapping)
        ///         
        /// Legend
        /// => means mappedTo
        /// =>> means outputAs
        /// 
        /// Error should occur in readerNode1
        /// </summary>
        [TestMethod]
        public void ValidateInputMapping_NoOutputFound2()
        {
            Assert.Fail("Test temporarily broken. The input mapping validator temporarily validates across all components, " +
                        "not only predecessors nodes. Test ignored till the validator feature is going to be revisited.");

            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode readerNode1 = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 5, 5);
            ExperimentNode readerNode2 = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 15, 15);
            ExperimentNode writerNode = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 25, 25);

            experiment.AddConnection(experiment.StartNode, writerNode);
            experiment.AddConnection(experiment.StartNode, readerNode1);
            experiment.AddConnection(writerNode, readerNode2);
            experiment.AddConnection(readerNode1, experiment.EndNode);
            experiment.AddConnection(readerNode2, experiment.EndNode);

            (writerNode.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "fakemapping";
            (readerNode1.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "fakemapping";
            (readerNode2.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "fakemapping";

            Assert.IsFalse(readerNode1.HasError);
            Assert.IsFalse(readerNode2.HasError);
            Assert.IsFalse(writerNode.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsTrue(template.IsEmpty);

            Assert.IsFalse(readerNode2.HasError);
            Assert.IsFalse(writerNode.HasError);

            Assert.IsTrue(readerNode1.HasError);
            Assert.AreEqual(readerNode1.ErrorMessage, "The component attempts to load 'fakemapping' from the Workspace. However, none of the previous components outputs 'fakemapping' to the Workspace.");
        }

        /// <summary>
        /// 
        /// Start -> WriterNode -> ReaderNode -> End
        /// 
        /// EmptyNode 
        /// 
        /// 
        /// EmptyNode is skipped because it is not connected
        /// </summary>
        [TestMethod]
        public void ValidateInputMapping_CorrectFlow_OutputFound()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode readerNode = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 5, 5);
            ExperimentNode writerNode = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 15, 15);
            ExperimentNode emptyNode = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 25, 25);

            experiment.AddConnection(experiment.StartNode, writerNode);
            experiment.AddConnection(writerNode, readerNode);
            experiment.AddConnection(readerNode, experiment.EndNode);

            (writerNode.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "fakemapping";
            (readerNode.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "fakemapping";

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsFalse(template.IsEmpty);

            Assert.IsFalse(readerNode.HasError);
            Assert.IsFalse(writerNode.HasError);

            Assert.AreEqual(4, template.Nodes.Count);
        }

        /// <summary>
        /// 
        /// Start -> WriterNode ------------> WriterNode -----------> ReaderNode -> End
        ///          (testoutput             (testoutput              (testinput 
        ///           =>> test1)                =>> test2)             =>test1  
        /// 
        /// 
        /// </summary>
        [TestMethod]
        public void ValidateInputMapping_CorrectFlow_OutputFound2()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode readerNode = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 5, 5);
            ExperimentNode writerNode1 = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 15, 15);
            ExperimentNode writerNode2 = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 25, 25);

            experiment.AddConnection(experiment.StartNode, writerNode1);
            experiment.AddConnection(writerNode1, writerNode2);
            experiment.AddConnection(writerNode2, readerNode);
            experiment.AddConnection(readerNode, experiment.EndNode);

            (writerNode1.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "test1";
            (writerNode2.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "test2";
            (readerNode.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "test1";

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);
            Assert.IsFalse(template.IsEmpty);

            Assert.IsFalse(readerNode.HasError);
            Assert.IsFalse(writerNode1.HasError);
            Assert.IsFalse(writerNode2.HasError);

            Assert.AreEqual(5, template.Nodes.Count);

        }

        /// <summary>
        /// 
        /// Start ----> WriterNode1 -----------------------------------> ReaderNode2 -----> End
        ///   |         (testoutput                                        (testinput 
        ///   |          =>> test1)                                         =>test2  
        ///   |                                                                ^
        ///   |                                                                |
        ///    ------> WriterNode2 -------------> ReaderNode1 -----------------
        ///            (testoutput              (testinput
        ///             =>> test2                => test2
        /// 
        /// </summary>
        [TestMethod]
        public void ValidateInputMapping_CorrectFlow_OutputFound3()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode writerNode1 = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 15, 15);
            ExperimentNode writerNode2 = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 35, 35);

            ExperimentNode readerNode1 = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 5, 5);
            ExperimentNode readerNode2 = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 25, 25);


            experiment.AddConnection(experiment.StartNode, writerNode1);
            experiment.AddConnection(experiment.StartNode, writerNode2);

            experiment.AddConnection(writerNode1, readerNode2);

            experiment.AddConnection(writerNode2, readerNode1);
            experiment.AddConnection(readerNode1, readerNode2);

            experiment.AddConnection(readerNode2, experiment.EndNode);

            (writerNode1.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "test1";
            (writerNode2.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "test2";

            (readerNode1.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "test2";
            (readerNode2.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "test2";

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);

            Assert.IsFalse(readerNode1.HasError);
            Assert.IsFalse(readerNode2.HasError);
            Assert.IsFalse(writerNode1.HasError);
            Assert.IsFalse(writerNode2.HasError);

            Assert.IsFalse(template.IsEmpty);

            Assert.AreEqual(6, template.Nodes.Count);
        }

        /// <summary>
        /// This test tests if the graph has been accepted when all the input is correct.
        /// Test written since decisions nodes were introduced. 
        /// 
        /// Graph is following:
        ///                                                                       test>5
        ///   Start ----> WriterNode -------> ReaderNode -----------> decision ----------> end
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
        /// There shouldn't be any errors since the mapping is correct. 
        /// </summary>
        [TestMethod]
        public void ValidateInputMapping_Decision_CorrectMapping()
        {
            Experiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode writerNode = experiment.AddComponentFromDefinition(m_componentWriterMetaDefinition, 15, 15);
            (writerNode.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "test";

            ExperimentNode readerNode = experiment.AddComponentFromDefinition(m_componentReaderMetaDefinition, 25, 25);
            (readerNode.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "test";

            ExperimentNode incrementerNode = experiment.AddComponentFromDefinition(m_incrementerComponentMetaDefinition, 35, 35);
            (incrementerNode.Data.Metadata as ComponentMetadata).IOSpec.Input["testinput"].MappedTo = "test";
            (incrementerNode.Data.Metadata as ComponentMetadata).IOSpec.Output["testoutput"].MappedTo = "test";

            var decisionNode = AddDecisionToExperiment(experiment, 10, 10);
            //code doesn't really matter in this case, because we don't execute dispatcher 
            string decisionCode = "";

            decisionNode.Data.Metadata = CompileMockDecision(decisionCode);

            experiment.AddConnection(experiment.StartNode, writerNode);
            experiment.AddConnection(writerNode, readerNode);
            experiment.AddConnection(readerNode, decisionNode);
            experiment.AddConnection(decisionNode, incrementerNode);
            experiment.AddConnection(incrementerNode, readerNode);
            experiment.AddConnection(decisionNode, experiment.EndNode);

            Assert.IsFalse(writerNode.HasError);
            Assert.IsFalse(readerNode.HasError);
            Assert.IsFalse(incrementerNode.HasError);
            Assert.IsFalse(decisionNode.HasError);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(AppContext.WorkspaceInstance);
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);

            Assert.IsFalse(writerNode.HasError);
            Assert.IsFalse(readerNode.HasError);
            Assert.IsFalse(incrementerNode.HasError);
            Assert.IsFalse(decisionNode.HasError);

            Assert.IsFalse(template.IsEmpty);
            Assert.AreEqual(6, template.Nodes.Count);

            //check all  edges
            foreach (RunnableNode node in template.Nodes)
            {
                if (node is RunnableDecisionNode)
                {
                    //in this graph decision node has only two outcoming edges
                    Assert.AreEqual(2, node.NextNodes.Count);
                }
                else if (node is RunnableEndNode)
                {
                    //end node has none outcoming edges
                    Assert.AreEqual(0, node.NextNodes.Count);
                }
                else
                {
                    //all other nodes in this graph has only one outcoming edge
                    Assert.AreEqual(1, node.NextNodes.Count);
                }
            }
        }

        /// <summary>
        /// Testing if all component and decision nodes are disposed in case when initialization of any component node failed in the template graph creation.
        /// </summary>
        [TestMethod]
        public void TestTemplateGraphDisposal_ValidNodes()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            //construct some simple m_experiment
            ExperimentNode node1 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node2 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);

            experiment.AddConnection(experiment.StartNode, node1);
            experiment.AddConnection(node1, node2);
            experiment.AddConnection(node2, experiment.EndNode);

            //initiate mockNodesFactory
            MockNodesFactory mockNodesFactory = new MockNodesFactory();
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, mockNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);

            foreach (MockNode node in mockNodesFactory.CreatedNodes)
            {
                Assert.IsFalse(node.disposed);
            }

            //execute disptacher, because in case of correct graph dispatcher disposes nodes
            MockProgress progress = new MockProgress();
            using (var dispatcher = ExperimentRunnerFactory.CreateExperimentRunner(template))
            {
                dispatcher.ExecuteExperiment(progress);
                Assert.IsFalse(progress.HasError);
            }

            foreach (MockNode node in mockNodesFactory.CreatedNodes)
            {
                Assert.IsTrue(node.disposed);
            }
        }

        /// <summary>
        /// Testing if all component and decision nodes are disposed in case when initialization of any component node failed in the template graph creation.
        /// </summary>
        [TestMethod]
        public void TestTemplateGraphDisposal_InValidNodes()
        {
            IEditableExperiment experiment = ((IEditableExperiment)ExperimentManager.New());
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            //construct some simple m_experiment
            ExperimentNode node1 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node2 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);
            ExperimentNode node3 = experiment.AddComponentFromDefinition(m_emptyComponentMetaDefinition, 5, 5);

            node1.Data.Metadata.Label = "Broken Component";

            experiment.AddConnection(experiment.StartNode, node1);
            experiment.AddConnection(node1, node2);
            experiment.AddConnection(node2, node3);
            experiment.AddConnection(node3, experiment.EndNode);

            //initiate mockNodesFactory
            MockNodesFactory mockNodesFactory = new MockNodesFactory();
            RunnableExperimentBase template = GraphAdapter.Adapt(experiment, mockNodesFactory, AppContext.Components, AppContext.WorkspaceInstance.TypeDirectories);

            //adaptation should have failed and graph should be empty because broken node construction failed
            Assert.IsTrue(template.IsEmpty);

            //but also all of the previously created nodes before reaching the broken node during adaptation should have been disposed.
            foreach (MockNode node in mockNodesFactory.CreatedNodes)
            {
                Assert.IsTrue(node.disposed);
            }
        }


        /// <summary>
        /// Adds a new decision node at the specified coordinates
        /// </summary>
        public ExperimentDecisionNode AddDecisionToExperiment(Experiment experiment, double positionX, double positionY)
        {
            ExperimentDecisionNode newNode = null;

            SerializedVertexData data = new SerializedVertexData();
            data.X = positionX;
            data.Y = positionY;
            data.Metadata = new DecisionMetadata("Decision");

            newNode = new ExperimentDecisionNode(Guid.NewGuid().ToString(), data);
            experiment.AddVertex(newNode);

            return newNode;
        }
    }

    internal class MockNodesFactory : IRunnableNodeFactory
    {
        //keep reference to all created nodes
        public ICollection<MockNode> CreatedNodes = new List<MockNode>();

        public RunnableNode CreateNode(string id, Metadata metadata, LoggerNameRoot loggerNameRoot, TraceLab.Core.Components.ComponentsLibrary library, AppDomain componentsAppDomain,
            System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            return CreateNode(id, metadata, terminateExperimentExecutionResetEvent);
        }

        public RunnableNode CreateNode(string id, Metadata metadata, System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            StartNodeMetadata startNodeMetadata = metadata as StartNodeMetadata;
            EndNodeMetadata endNodeMetadata = metadata as EndNodeMetadata;
            ComponentMetadata componentMetadata = metadata as ComponentMetadata;

            if (startNodeMetadata != null)
            {
                return new RunnableStartNode(id);
            }
            else if (endNodeMetadata != null)
            {
                return new RunnableEndNode(id, endNodeMetadata.WaitsForAllPredecessors);
            }
            else if (componentMetadata != null)
            {
                //mock failure
                if (componentMetadata.Label.Equals("Broken Component"))
                {
                    throw new ArgumentException("Failed");
                }

                //otherwise create a mock node and keep reference
                MockNode mockNode = new MockNode(id);
                CreatedNodes.Add(mockNode);

                return mockNode;
            }
            else
            {
                throw new ArgumentException("Could not identify node type.");
            }
        }
    }

    internal class MockNode : RunnableNode
    {
        private string ID;

        public MockNode(string id)
            : base(id, id, new RunnableNodeCollection(), new RunnableNodeCollection(), null, true)
        {
            ID = id;
            System.Diagnostics.Trace.WriteLine(String.Format("Initializing mock node {0}.", ID));
        }

        public override void RunInternal()
        {

        }

        // Track whether Dispose has been called.
        public bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            // Note disposing has been done.
            System.Diagnostics.Trace.WriteLine(String.Format("Disposing mock node {0}.", ID));
            base.Dispose(disposing);
            disposed = true;
        }
    }
}
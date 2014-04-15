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
using TraceLab.Core.Experiments;
using TraceLab.Core.Test.Setup;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace TraceLab.ViewModels.Test
{
    [TestClass]
    public class ExperimentTest 
    {
        private static string ExperimentFile
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

        public ExperimentTest()
        {
        }

        [ClassInitialize]
        public static void ViewModelSetup(TestContext context)
        {
        }

        [ClassCleanup]
        public static void ViewModelTeardown()
        {
        }

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);

            AppContext.Components.Clear(); //clear library before loading
            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(AppContext.Components));
            libraryAccessor.LoadComponentsDefinitions(AppContext.WorkspaceInstance.TypeDirectories);

            string filename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "tracingGraphNew.teml");
            ExperimentFile = filename;
            System.IO.FileInfo fInfo = new System.IO.FileInfo(ExperimentFile);
            fInfo.IsReadOnly = false;
        }

        [TestCleanup]
        public void TestTearDown()
        {
            AppContext.Components.Clear();

            AppContext = null;
        }

        [TestMethod]
        [ExpectedException(typeof(TraceLab.Core.Exceptions.ExperimentLoadException))]
        public void ExperimentLoadNull()
        {
            IExperiment loadedExperiment = ExperimentManager.Load(null, AppContext.Components);
        }

        [TestMethod]
        [ExpectedException(typeof(TraceLab.Core.Exceptions.ExperimentLoadException))]
        public void ExperimentLoadEmpty()
        {
            IExperiment loadedExperiment = ExperimentManager.Load(string.Empty, AppContext.Components);
        }

        [TestMethod]
        public void ExperimentLoad()
        {
            IExperiment loadedExperiment = ExperimentManager.Load(ExperimentFile, AppContext.Components);
            Assert.IsNotNull(loadedExperiment);
            ValidateData(loadedExperiment);
            Assert.IsFalse(loadedExperiment.IsModified);
        }

        /// <summary>
        /// Loads the corrupted experiment file. The file is corrupted, but xml structure is not. 
        /// In particular one node was manually removed, but connections to that node still exists.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TraceLab.Core.Exceptions.ExperimentLoadException))]
        public void ExperimentLoadContentCorruptedFile()
        {
            string filename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "tracingGraphContentCorrupted.teml");
            IExperiment loadedExperiment = ExperimentManager.Load(filename, AppContext.Components);
        }

        /// <summary>
        /// Loads the corrupted experiment file. The file is corrupted in such a way that xml structure is broken. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TraceLab.Core.Exceptions.ExperimentLoadException))]
        public void ExperimentLoadStructureCorruptedFile()
        {
            string filename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "tracingGraphStructureCorrupted.teml");
            IExperiment loadedExperiment = ExperimentManager.Load(filename, AppContext.Components);
        }

        [TestMethod]
        public void ExperimentClone()
        {
            IExperiment loadedExperiment = ExperimentManager.Load(ExperimentFile, AppContext.Components);
            ValidateData(loadedExperiment);

            Experiment clone = (Experiment)((BaseExperiment)loadedExperiment).Clone();
            ValidateData(clone);
            Assert.IsFalse(clone.IsModified);

            Dictionary<string, ExperimentNode> clonedNodes = new Dictionary<string, ExperimentNode>();
            foreach (ExperimentNode clonedNode in clone.Vertices)
            {
                clonedNodes[clonedNode.ID] = clonedNode;
            }

            // Ensure that none of the vertices from the source experiment are in the clone experiment.
            // This checks references, since obviously the clone is supposed to have nodes with the same ID.
            foreach (ExperimentNode node in loadedExperiment.Vertices)
            {
                ExperimentNode clonedNode = null;
                Assert.IsTrue(clonedNodes.TryGetValue(node.ID, out clonedNode));
                Assert.IsFalse(object.ReferenceEquals(node, clonedNode));
                Assert.IsFalse(object.ReferenceEquals(node.Data, clonedNode.Data));
                Assert.IsFalse(object.ReferenceEquals(node.Data.Metadata, clonedNode.Data.Metadata));
            }
        }

        [TestMethod]
        public void ExperimentNew()
        {
            IExperiment experiment = ExperimentManager.New();
            Assert.IsNotNull(experiment);
            Assert.IsFalse(experiment.IsModified);
            Assert.AreEqual(2, experiment.VertexCount);
            Assert.IsTrue(string.IsNullOrEmpty(experiment.ExperimentInfo.FilePath));
        }

        private void ValidateData(IExperiment experiment)
        {
            Assert.AreEqual(ExperimentFile, experiment.ExperimentInfo.FilePath);
            Assert.AreEqual("<Experiment name>", experiment.ExperimentInfo.Name);
            Assert.AreEqual(15, experiment.VertexCount);
            Assert.AreEqual(18, experiment.EdgeCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewComponentNullDefinition()
        {
            IExperiment experiment = ExperimentManager.New();

            Assert.AreEqual(2, experiment.VertexCount);
            ExperimentNode node = ((IEditableExperiment)experiment).AddComponentFromDefinition(null, -5, 5);
        }

        [TestMethod]
        public void GetComponent()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ExperimentNode startNode = experiment.GetNode("Start");
            Assert.IsNotNull(startNode);
            Assert.AreEqual("Start", startNode.ID);
            Assert.IsTrue(startNode is TraceLab.Core.Experiments.ExperimentStartNode);
            ExperimentNode endNode = experiment.GetNode("End");
            Assert.IsNotNull(endNode);
            Assert.AreEqual("End", endNode.ID);
            Assert.IsTrue(endNode is TraceLab.Core.Experiments.ExperimentEndNode);

            ExperimentNode nodeDoesntExist = experiment.GetNode(Guid.NewGuid().ToString());
            Assert.IsNull(nodeDoesntExist);
        }

        [TestMethod]
        public void AddNewComponent()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            Assert.AreEqual(2, experiment.VertexCount);
            ExperimentNode node = ((IEditableExperiment)experiment).AddComponentFromDefinition(def, -5, 5);
            Assert.IsNotNull(node);
            Assert.AreEqual(3, experiment.VertexCount);
            Assert.IsTrue(experiment.IsModified);
        }

        [TestMethod]
        public void GetNewComponent()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node = ((IEditableExperiment)experiment).AddComponentFromDefinition(def, -5, 5);

            ExperimentNode foundNode = experiment.GetNode(node.ID);
            Assert.AreEqual(node, foundNode);
        }

        [TestMethod]
        public void RemoveComponent()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode added = ((IEditableExperiment)experiment).AddComponentFromDefinition(def, -5, 5);

            ((IEditableExperiment)experiment).RemoveVertex(added);
            Assert.AreEqual(2, experiment.VertexCount);
            foreach (ExperimentNode node in experiment.Vertices)
            {
                Assert.AreNotEqual(added.ID, node.ID);
            }
        }

        [TestMethod]
        public void GetRemovedComponent()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node = ((IEditableExperiment)experiment).AddComponentFromDefinition(def, -5, 5);
            ((IEditableExperiment)experiment).RemoveVertex(node);
            ExperimentNode foundNode = experiment.GetNode(node.ID);
            Assert.IsNull(foundNode);
        }

        [TestMethod]
        public void AddNewConnection()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def1 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node1 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def1, -5, 5);

            ComponentMetadataDefinition def2 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test2.dll"), "IDontExistEither");
            ExperimentNode node2 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def2, -5, 5);
            
            // We know already that experiments are modified after adding components, but we 
            // want to make sure that they're modified after adding a connection too, so lets clear the modification flag first.
            experiment.ResetModifiedFlag();
            Assert.IsFalse(experiment.IsModified);
            
            Assert.AreEqual(0, experiment.EdgeCount);
            ExperimentNodeConnection newEdge = ((IEditableExperiment)experiment).AddConnection(node1, node2);
            Assert.IsNotNull(newEdge);
            Assert.AreEqual(1, experiment.EdgeCount);
            Assert.IsTrue(experiment.IsModified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewConnectionNullNode1()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def1 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node1 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def1, -5, 5);

            experiment.ResetModifiedFlag();

            Assert.AreEqual(0, experiment.EdgeCount);
            ExperimentNodeConnection newEdge = ((IEditableExperiment)experiment).AddConnection(node1, null);
            Assert.IsNotNull(newEdge);
            Assert.AreEqual(1, experiment.EdgeCount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void AddNewConnectionCannotTargetStartNode()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def2 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node2 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def2, -5, 5);

            ExperimentNodeConnection newEdge = ((IEditableExperiment)experiment).AddConnection(node2, experiment.StartNode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewConnectionNullNode2()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def1 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node2 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def1, -5, 5);

            Assert.AreEqual(0, experiment.EdgeCount);
            ExperimentNodeConnection newEdge = ((IEditableExperiment)experiment).AddConnection(null, node2);
            Assert.IsNotNull(newEdge);
            Assert.AreEqual(1, experiment.EdgeCount);
        }

        [TestMethod]
        public void AddNewConnectionTwice()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def1 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node1 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def1, -5, 5);

            ComponentMetadataDefinition def2 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test2.dll"), "IDontExistEither");
            ExperimentNode node2 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def2, -5, 5);

            Assert.AreEqual(0, experiment.EdgeCount);
            ExperimentNodeConnection newEdge = ((IEditableExperiment)experiment).AddConnection(node1, node2);
            Assert.IsNotNull(newEdge);
            Assert.AreEqual(1, experiment.EdgeCount);
            
            //add the same connection once again
            ExperimentNodeConnection newEdge2 = ((IEditableExperiment)experiment).AddConnection(node1, node2);
            Assert.IsNotNull(newEdge2);
            Assert.AreEqual(newEdge, newEdge2);
            // the amount of edges should still be the same
            Assert.AreEqual(1, experiment.EdgeCount);
        }

        [TestMethod]
        public void RemoveConnection()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            ComponentMetadataDefinition def1 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test.dll"), "IDontExist");
            ExperimentNode node1 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def1, -5, 5);

            ComponentMetadataDefinition def2 = new ComponentMetadataDefinition(Guid.NewGuid().ToString(), System.IO.Path.Combine(AppContext.BaseTestDirectory, "Test2.dll"), "IDontExistEither");
            ExperimentNode node2 = ((IEditableExperiment)experiment).AddComponentFromDefinition(def2, -5, 5);

            Assert.AreEqual(0, experiment.EdgeCount);
            ExperimentNodeConnection newEdge = ((IEditableExperiment)experiment).AddConnection(node1, node2);
            Assert.IsNotNull(newEdge);
            Assert.AreEqual(1, experiment.EdgeCount);

            experiment.ResetModifiedFlag();

            Assert.AreEqual(4, experiment.VertexCount);
            ((IEditableExperiment)experiment).RemoveConnection(newEdge);

            // Verify that the edge was existed and the number of vertices was unaffected.
            Assert.AreEqual(0, experiment.EdgeCount);
            Assert.AreEqual(4, experiment.VertexCount);

            Assert.IsTrue(experiment.IsModified);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.IOException))]
        public void SaveOverReadOnlyFile()
        {
            IExperiment experiment = ExperimentManager.New();
            experiment.ExperimentInfo.FilePath = "C:\\somefakelocation\\mockExperiment.teml";
            // set it to read only
            System.IO.FileInfo finfo = new System.IO.FileInfo(ExperimentFile);
            finfo.IsReadOnly = true;

            // attempt to save expecting an exception regarding it being read only
            ExperimentManager.Save(experiment, ExperimentFile);
        }

        /// <summary>
        /// Checks if FilePath of experiment info is updated on save experiment.
        /// </summary>
        [TestMethod]
        public void SaveExperimentTest()
        {
            IExperiment experiment = ExperimentManager.New();
            Assert.IsNotNull(experiment);
            Assert.AreEqual(2, experiment.VertexCount);
            Assert.IsTrue(string.IsNullOrEmpty(experiment.ExperimentInfo.FilePath));

            string filename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "testSave.gml");
            try
            {
                bool success = ExperimentManager.Save(experiment, filename);

                Assert.IsTrue(success);
                Assert.IsTrue(File.Exists(filename));

                IExperiment loadedExperiment = ExperimentManager.Load(filename, AppContext.Components);
                
                Assert.AreEqual(experiment.ExperimentInfo, loadedExperiment.ExperimentInfo);

            }
            finally
            {
                if (File.Exists(filename))
                {
                    //cleanup
                    File.Delete(filename);
                }
            }
        }

        /// <summary>
        /// Checks if FilePath of experiment info is updated on save experiment.
        /// </summary>
        [TestMethod]
        public void PathUpdatedOnSaveExperiment()
        {
            IExperiment experiment = ExperimentManager.New();
            Assert.IsNotNull(experiment);
            Assert.AreEqual(2, experiment.VertexCount);
            Assert.IsTrue(string.IsNullOrEmpty(experiment.ExperimentInfo.FilePath));

            string filename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "testSave.gml");
            try
            {
                bool success = ExperimentManager.Save(experiment, filename);

                Assert.IsTrue(success);
                Assert.IsTrue(File.Exists(filename));

                //did experiment info filepath got update on save
                Assert.AreEqual(experiment.ExperimentInfo.FilePath, filename);
            }
            finally
            {
                if (File.Exists(filename))
                {
                    //cleanup
                    File.Delete(filename);
                }
            }
        }

        /// <summary>
        /// Opens the experiment with non existing composite component test.
        /// </summary>
        [TestMethod]
        public void OpenExperimentWithNonExistingCompositeComponentTest()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_with_non_existing_composite_component_bug_75.gml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings;

            Assert.IsNotNull(experiment);
            Assert.AreEqual(3, experiment.VertexCount);

            //find the composite component node
            CompositeComponentNode compositeComponentNode = null;
            foreach (ExperimentNode node in experiment.Vertices)
            {
                compositeComponentNode = node as CompositeComponentNode;
                if (compositeComponentNode != null)
                    break;
            }

            Assert.IsNotNull(compositeComponentNode); //if fails composite component node has not been found
            //once found check if it has error
            Assert.IsTrue(compositeComponentNode.HasError);

            //Also try to create ExperimentViewModel for the experiment. Assure that it does not crash
            var experimentViewModel = new TraceLab.UI.WPF.ViewModels.ExperimentViewModel_Accessor(experiment);
        }


        [TestMethod]
        public void AddCompositeComponent_Bug77()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_to_test_bug77.gml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings;

            //create ExperimentViewModel for the experiment - the crash happen in the experiment view model, so it has to be initialized
            var experimentViewModel = new TraceLab.UI.WPF.ViewModels.ExperimentViewModel_Accessor(experiment);

            CompositeComponentMetadataDefinition compositeComponentDefinition = null;
            //find the composite component to test bug 77
            foreach (MetadataDefinition definition in AppContext.Components.Components)
            {
                if (definition.Classname.Equals("Component to test bug 77"))
                {
                    compositeComponentDefinition = definition as CompositeComponentMetadataDefinition;
                    break;
                }
            }

            //check if definition has been found in library
            Assert.IsNotNull(compositeComponentDefinition);

            Assert.AreEqual(3, experiment.VertexCount);
            ExperimentNode node = ((IEditableExperiment)experiment).AddComponentFromDefinition(compositeComponentDefinition, -5, 5);
            Assert.IsNotNull(node);
            Assert.AreEqual(4, experiment.VertexCount);
            Assert.IsTrue(experiment.IsModified);

        }
    }
}

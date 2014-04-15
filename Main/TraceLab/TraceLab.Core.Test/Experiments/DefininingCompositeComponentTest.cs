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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Components;
using TraceLab.Core.Test.Setup;
using System.IO;
using TraceLab.Core.Experiments;
using System.Xml.Serialization;
using System.Xml;

namespace TraceLab.Core.Test.Experiments
{
    [TestClass]
    public class DefininingCompositeComponentTest
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

        public DefininingCompositeComponentTest()
        { }

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
            AppContext.Components.Clear(); //clear library before loading
            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(AppContext.Components));
            libraryAccessor.LoadComponentsDefinitions(AppContext.WorkspaceInstance.TypeDirectories);

            string componentsDir = (string)AppContext.Settings.ComponentPaths[0];
            exportFile = System.IO.Path.Combine(componentsDir, "testCompositeComponent.tcml");
        }

        private string exportFile;

        [TestCleanup]
        public void TestTearDown()
        {
            if (File.Exists(exportFile))
            {
                File.Delete(exportFile);
            }

            AppContext.Components.Clear();

            AppContext = null;
        }

        //nodes ids in the experiment_define_composite_component_basic_test.teml experiment
        private string TestWriterID = "ae8e7bce-eb87-40ab-a9be-a42427b73ba6";
        private string TestImporterID = "16cee8c9-c39e-471e-af0b-dcc2db50b3bb";

        /// <summary>
        /// Tests defining simple composite component based on the experiment.
        /// In this test entire experiment is selected. 
        /// 
        /// The test depends on that graph file.
        /// </summary>
        [TestMethod]
        public void DefineCompositeComponentBasicTestSelectAllNodes()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_define_composite_component_basic_test.teml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings; //assure that we apply global settings

            //in first basic test select all nodes
            foreach (ExperimentNode node in experiment.Vertices)
            {
                node.IsSelected = true;
            }

            DefiningCompositeComponentSetup setup = new DefiningCompositeComponentSetup(experiment);

            Assert.IsNotNull(setup);

            Assert.AreEqual(1, setup.InputSettings.Count);
            Assert.AreEqual(2, setup.OutputSettings.Count);
            Assert.AreEqual(1, setup.ConfigSettings.Count);

            Assert.IsTrue(setup.InputSettings["test1"].Include);
            Assert.IsTrue(setup.OutputSettings["test1"].Include);
            Assert.IsTrue(setup.OutputSettings["test2"].Include);

            //note, if there are two components with the same label and same config name, the expected behaviour is to add
            Assert.IsTrue(setup.ConfigSettings["Test writer Value"].Include);

            //get real names of the config values 
            string configName1 = setup.ConfigSettings["Test writer Value"].PropertyObject.Name;

            //change settings to sth different values
            setup.Name = "Test Composite Component";
            setup.Author = "Author XYZ";
            setup.Description = "Composite Component to test export";

            //don't include following in the export
            setup.InputSettings["test1"].Include = false;
            setup.OutputSettings["test1"].Include = false;

            //rescan library to be sure it is fresh scan
            int numberOfComponentsInLibrary = GetCountOfComponentsInLibrary();

            //set file path to composite component
            setup.CompositeComponentLocationFilePath = exportFile;
            Assert.IsFalse(File.Exists(setup.CompositeComponentLocationFilePath));

            setup.DefineComponent();

            Assert.IsTrue(File.Exists(exportFile));

            //rescan the library
            int updatedNumberOfComponents = GetCountOfComponentsInLibrary();

            //after rescan there should be one more component in the library
            Assert.AreEqual(numberOfComponentsInLibrary + 1, updatedNumberOfComponents);

            //iterate through all components until find new composite component
            CompositeComponentMetadataDefinition compositeComponentDefinition = FindCompositeComponent(exportFile);

            //assure that composite component definition has been found
            Assert.IsNotNull(compositeComponentDefinition); //if fails, composite component has not been found

            Assert.AreEqual(setup.Name, compositeComponentDefinition.Label);
            Assert.AreEqual(setup.Description, compositeComponentDefinition.Description);
            Assert.AreEqual(setup.Author, compositeComponentDefinition.Author);
            Assert.IsNotNull(compositeComponentDefinition.ComponentGraph);

            //the number of configuration properties should be the same
            Assert.AreEqual(setup.ConfigSettings.Count, compositeComponentDefinition.ConfigurationWrapperDefinition.Properties.Count);

            ConfigPropertyObject config1 = compositeComponentDefinition.ConfigurationWrapperDefinition.Properties[configName1];

            Assert.AreEqual("Test writer Value", config1.DisplayName);

            Assert.IsTrue(config1.Visible); //config1 should  be visible, because Include was set to false

            Assert.AreEqual(0, compositeComponentDefinition.IOSpecDefinition.Input.Count); //no input was included
            Assert.AreEqual(1, compositeComponentDefinition.IOSpecDefinition.Output.Count); //only one output was included
        }

        /// <summary>
        /// Tests defining simple composite component based on the experiment.
        /// In this test selects only one node and end node.
        /// 
        /// The test depends on that graph file.
        /// </summary>
        [TestMethod]
        public void DefineCompositeComponentBasicTestSelectSubraphWithNoEndAndStart()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_define_composite_component_basic_test.teml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings; //assure that we apply global settings

            //select just one node
            foreach (ExperimentNode node in experiment.Vertices)
            {
                if (node.ID == TestImporterID)
                {
                    node.IsSelected = true;
                }
            }

            DefiningCompositeComponentSetup setup = new DefiningCompositeComponentSetup(experiment);

            Assert.IsNotNull(setup);

            Assert.AreEqual(1, setup.InputSettings.Count);
            Assert.AreEqual(1, setup.OutputSettings.Count);
            Assert.AreEqual(0, setup.ConfigSettings.Count);
            Assert.IsTrue(setup.InputSettings["test1"].Include);
            Assert.IsTrue(setup.OutputSettings["test2"].Include);

            //rescan library to be sure it is fresh scan
            int numberOfComponentsInLibrary = GetCountOfComponentsInLibrary();

            //let's export the experiment as composite component
            setup.CompositeComponentLocationFilePath = exportFile;

            Assert.IsFalse(File.Exists(exportFile));
            setup.DefineComponent();

            Assert.IsTrue(File.Exists(exportFile));

            int updatedNumberOfComponents = GetCountOfComponentsInLibrary();

            //after rescan there should be one more component in the library
            Assert.AreEqual(numberOfComponentsInLibrary + 1, updatedNumberOfComponents);

            //iterate through all components until find new composite component
            CompositeComponentMetadataDefinition compositeComponentDefinition = FindCompositeComponent(exportFile);

            //assure that composite component definition has been found
            Assert.IsNotNull(compositeComponentDefinition); //if fails, composite component has not been found

            Assert.AreEqual(setup.Name, compositeComponentDefinition.Label);
            Assert.AreEqual(setup.Description, compositeComponentDefinition.Description);
            Assert.AreEqual(setup.Author, compositeComponentDefinition.Author);
            Assert.IsNotNull(compositeComponentDefinition.ComponentGraph);

            //the number of configuration properties should be the same
            Assert.AreEqual(setup.ConfigSettings.Count, compositeComponentDefinition.ConfigurationWrapperDefinition.Properties.Count);

            Assert.AreEqual(1, compositeComponentDefinition.IOSpecDefinition.Input.Count); //one input was included
            Assert.AreEqual(1, compositeComponentDefinition.IOSpecDefinition.Output.Count); //one output was included
        }


        /// <summary>
        /// Tests defining composite component based on the experiment.
        /// </summary>
        [TestMethod]
        public void DefineCompositeComponentTest()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_define_composite_component.teml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings; //assure that we apply global settings

            //select all nodes
            foreach (ExperimentNode node in experiment.Vertices)
            {
                node.IsSelected = true;
            }

            DefiningCompositeComponentSetup setup = new DefiningCompositeComponentSetup(experiment);

            Assert.IsNotNull(experiment);
            Assert.AreEqual(8, experiment.VertexCount);
            Assert.IsNotNull(setup);

            Assert.AreEqual(3, setup.InputSettings.Count);
            Assert.AreEqual(4, setup.OutputSettings.Count);
            Assert.AreEqual(2, setup.ConfigSettings.Count);

            //check if default values are to include everything
            Assert.IsTrue(setup.InputSettings["test1"].Include);
            Assert.IsTrue(setup.InputSettings["test2"].Include);
            Assert.IsTrue(setup.InputSettings["test4"].Include);
            Assert.IsTrue(setup.OutputSettings["test1"].Include);
            Assert.IsTrue(setup.OutputSettings["test2"].Include);
            Assert.IsTrue(setup.OutputSettings["test3"].Include);
            Assert.IsTrue(setup.OutputSettings["test4"].Include);

            //note, if there are two components with the same label and same config name, the expected behaviour is to add
            //the number value. In this case there are two Test importer components, so 2nd config has key 'Test importer Value 2'
            Assert.IsTrue(setup.ConfigSettings["Test writer Value"].Include);
            Assert.IsTrue(setup.ConfigSettings["Test writer Value 2"].Include);

            //get real names of the config values 
            string configName1 = setup.ConfigSettings["Test writer Value"].PropertyObject.Name;
            string configName2 = setup.ConfigSettings["Test writer Value 2"].PropertyObject.Name;

            //change settings to sth different values
            setup.Name = "Test Composite Component";
            setup.Author = "Author XYZ";
            setup.Description = "Composite Component to test export";

            //don't include following in the export
            setup.InputSettings["test1"].Include = false;
            setup.InputSettings["test2"].Include = false;
            setup.InputSettings["test4"].Include = false;
            setup.OutputSettings["test3"].Include = false;
            setup.ConfigSettings["Test writer Value"].Include = false;

            //change alias
            string configAlias2 = "Writer value";
            setup.ConfigSettings["Test writer Value 2"].Alias = configAlias2;

            //rescan library to be sure it is fresh scan
            int numberOfComponentsInLibrary = GetCountOfComponentsInLibrary();
            
            Assert.IsFalse(File.Exists(exportFile));

            //let's export the experiment as composite component
            setup.CompositeComponentLocationFilePath = exportFile;
            setup.DefineComponent();

            Assert.IsTrue(File.Exists(exportFile));

            //rescan the library
            int updatedNumberOfComponents = GetCountOfComponentsInLibrary();

            //after rescan there should be one more component in the library
            Assert.AreEqual(numberOfComponentsInLibrary + 1, updatedNumberOfComponents);

            //iterate through all components until find new composite component
            CompositeComponentMetadataDefinition compositeComponentDefinition = FindCompositeComponent(exportFile);

            //assure that composite component definition has been found
            Assert.IsNotNull(compositeComponentDefinition); //if fails, composite component has not been found

            Assert.AreEqual(setup.Name, compositeComponentDefinition.Label);
            Assert.AreEqual(setup.Description, compositeComponentDefinition.Description);
            Assert.AreEqual(setup.Author, compositeComponentDefinition.Author);
            Assert.IsNotNull(compositeComponentDefinition.ComponentGraph);

            //the number of configuration properties should be the same
            Assert.AreEqual(setup.ConfigSettings.Count, compositeComponentDefinition.ConfigurationWrapperDefinition.Properties.Count);

            ConfigPropertyObject config1 = compositeComponentDefinition.ConfigurationWrapperDefinition.Properties[configName1];
            ConfigPropertyObject config2 = compositeComponentDefinition.ConfigurationWrapperDefinition.Properties[configName2];

            Assert.AreEqual("Test writer Value", config1.DisplayName);
            Assert.AreEqual(configAlias2, config2.DisplayName); // the second should have display name as alias

            Assert.IsFalse(config1.Visible); //config1 should not be visible, because Include was set to false
            Assert.IsTrue(config2.Visible);

            Assert.AreEqual(0, compositeComponentDefinition.IOSpecDefinition.Input.Count); //none inputs value were included
            Assert.AreEqual(3, compositeComponentDefinition.IOSpecDefinition.Output.Count); //three output was included
        }

        /// <summary>
        /// Test exporting the experiment. 
        /// Refer to the experiment_with_composite_component.gml  
        /// The test depends on that graph file and the composite component it uses, which can be found at
        /// GenericTestData/TestComponents/composite_component.tcml
        /// Refers to bug #74
        /// </summary>
        [TestMethod]
        public void DefineCompositeComponentFromExperimentWithAnotherCompositeComponentTest()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_with_composite_component.teml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings; //applying these settings was causing the bug #74

            //select all nodes
            foreach (ExperimentNode node in experiment.Vertices)
            {
                node.IsSelected = true;
            }

            DefiningCompositeComponentSetup setup = new DefiningCompositeComponentSetup(experiment);

            Assert.IsNotNull(experiment);
            Assert.AreEqual(3, experiment.VertexCount);
            Assert.IsNotNull(setup);

            Assert.AreEqual(0, setup.InputSettings.Count);
            Assert.AreEqual(2, setup.OutputSettings.Count);
            Assert.AreEqual(2, setup.ConfigSettings.Count);

            //check if default name is the same as experiment info
            Assert.IsTrue(setup.OutputSettings["test_x"].Include);
            Assert.IsTrue(setup.OutputSettings["test_y"].Include);
            Assert.IsTrue(setup.ConfigSettings["Composite Component Test writer Value"].Include);
            Assert.IsTrue(setup.ConfigSettings["Composite Component Test writer Value 2"].Include);

            //get real names of the config values 
            string configName1 = setup.ConfigSettings["Composite Component Test writer Value"].PropertyObject.Name;
            string configName2 = setup.ConfigSettings["Composite Component Test writer Value 2"].PropertyObject.Name;

            //change settings to sth different values
            setup.Name = "Complex Composite Component";
            setup.Author = "Author XYZ";
            setup.Description = "Complex Composite Component to test export";

            //don't include following in the export
            setup.OutputSettings["test_y"].Include = false;

            //change alias
            setup.ConfigSettings["Composite Component Test writer Value"].Alias = "Config 1";
            setup.ConfigSettings["Composite Component Test writer Value 2"].Alias = "Config 2";
            setup.ConfigSettings["Composite Component Test writer Value 2"].Include = false;

            //rescan library to be sure it is fresh scan
            int numberOfComponentsInLibrary = GetCountOfComponentsInLibrary();

            //let's export the experiment as composite component
            Assert.IsFalse(File.Exists(exportFile));
            setup.CompositeComponentLocationFilePath = exportFile;

            setup.DefineComponent();

            Assert.IsTrue(File.Exists(exportFile));

            //rescan the library
            int updatedNumberOfComponents = GetCountOfComponentsInLibrary();

            //after rescan there should be one more component in the library
            Assert.AreEqual(numberOfComponentsInLibrary + 1, updatedNumberOfComponents);

            //iterate through all components until find new composite component
            CompositeComponentMetadataDefinition compositeComponentDefinition = FindCompositeComponent(exportFile);

            //assure that composite component definition has been found
            Assert.IsNotNull(compositeComponentDefinition); //if fails, composite component has not been found

            Assert.AreEqual(setup.Name, compositeComponentDefinition.Label);
            Assert.AreEqual(setup.Description, compositeComponentDefinition.Description);
            Assert.AreEqual(setup.Author, compositeComponentDefinition.Author);
            Assert.IsNotNull(compositeComponentDefinition.ComponentGraph);

            //the number of configuration properties should be the same
            Assert.AreEqual(setup.ConfigSettings.Count, compositeComponentDefinition.ConfigurationWrapperDefinition.Properties.Count);

            ConfigPropertyObject config1 = compositeComponentDefinition.ConfigurationWrapperDefinition.Properties[configName1];
            ConfigPropertyObject config2 = compositeComponentDefinition.ConfigurationWrapperDefinition.Properties[configName2];

            Assert.AreEqual("Config 1", config1.DisplayName);
            Assert.AreEqual("Config 2", config2.DisplayName);

            Assert.IsTrue(config1.Visible);
            Assert.IsFalse(config2.Visible); //config1 should not be visible, because Include was set to false

            Assert.AreEqual(0, compositeComponentDefinition.IOSpecDefinition.Input.Count);//none input value were included
            Assert.AreEqual(1, compositeComponentDefinition.IOSpecDefinition.Output.Count); //one output value was included
        }

        [TestMethod]
        public void DefineCompositeComponentSerializeDeserializeTest()
        {
            // load the experiment to be exported
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Preprocessor.gml");
            Experiment experiment = ExperimentManager.Load(experimentFilename, AppContext.Components);
            experiment.Settings = AppContext.Settings; //applying these settings was causing the bug #74

            //select all nodes
            foreach (ExperimentNode node in experiment.Vertices)
            {
                node.IsSelected = true;
            }

            DefiningCompositeComponentSetup setup = new DefiningCompositeComponentSetup(experiment);
            setup.CompositeComponentLocationFilePath = exportFile;

            var tmpCompositeComponentDefinition = setup.GenerateCompositeComponentDefinition();

            XmlSerializer serializer = new XmlSerializer(typeof(TraceLab.Core.Components.CompositeComponentMetadataDefinition));

            Assert.IsFalse(File.Exists(exportFile));

            using (TextWriter writer = new StreamWriter(exportFile))
            {
                serializer.Serialize(writer, tmpCompositeComponentDefinition);
                writer.Close();
            }

            //check if file has been created
            Assert.IsTrue(System.IO.File.Exists(exportFile));

            //deserialize the file to CompositeComponentMetadataDefinition
            CompositeComponentMetadataDefinition compositeComponentDefinition;
            using (XmlReader reader = XmlReader.Create(exportFile))
            {
                compositeComponentDefinition = (CompositeComponentMetadataDefinition)serializer.Deserialize(reader);
                compositeComponentDefinition.PostProcessReadXml(AppContext.Components, AppContext.BaseTestDirectory);
            }
            Assert.IsNotNull(compositeComponentDefinition);
            Assert.AreEqual(compositeComponentDefinition.ID, tmpCompositeComponentDefinition.ID);
            Assert.AreEqual(compositeComponentDefinition.Version, tmpCompositeComponentDefinition.Version);
            Assert.AreEqual(compositeComponentDefinition.Classname, tmpCompositeComponentDefinition.Classname); //NAME
            Assert.AreEqual(compositeComponentDefinition.Author, tmpCompositeComponentDefinition.Author);
            Assert.AreEqual(compositeComponentDefinition.Label, tmpCompositeComponentDefinition.Label);
            Assert.AreEqual(compositeComponentDefinition.Description, tmpCompositeComponentDefinition.Description);
            Assert.AreEqual(compositeComponentDefinition.IOSpecDefinition, tmpCompositeComponentDefinition.IOSpecDefinition);
            Assert.AreEqual(compositeComponentDefinition.ConfigurationWrapperDefinition.Properties.Count, tmpCompositeComponentDefinition.ConfigurationWrapperDefinition.Properties.Count);
            Assert.AreEqual(compositeComponentDefinition.ComponentGraph.VertexCount, tmpCompositeComponentDefinition.ComponentGraph.VertexCount);
            Assert.AreEqual(compositeComponentDefinition.ComponentGraph.EdgeCount, tmpCompositeComponentDefinition.ComponentGraph.EdgeCount);

        }

        #region Private Helper Methods

        private CompositeComponentMetadataDefinition FindCompositeComponent(string exportFile)
        {
            CompositeComponentMetadataDefinition compositeComponentDefinition = null;
            foreach (MetadataDefinition definition in AppContext.Components.Components)
            {
                compositeComponentDefinition = definition as CompositeComponentMetadataDefinition;
                //find the composite component that was just exported
                if (compositeComponentDefinition != null && compositeComponentDefinition.Assembly.Equals(exportFile))
                {
                    break;
                }
            }
            return compositeComponentDefinition;
        }

        private int GetCountOfComponentsInLibrary()
        {
            AppContext.Components.Rescan(AppContext.PackageManager, AppContext.WorkspaceInstance.TypeDirectories, true);
            while (AppContext.Components.IsRescanning)
            {
                System.Threading.Thread.Sleep(0);
            }
            int numberOfComponentsInLibrary = AppContext.Components.Components.Count(); // count available is an extension method accessibne by using System.Linq
            return numberOfComponentsInLibrary;
        }

        #endregion
    }
}

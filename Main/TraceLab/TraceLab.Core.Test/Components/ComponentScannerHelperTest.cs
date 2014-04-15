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
using TraceLabSDK.Component.Config;

namespace TraceLab.Core.Test.Components
{
    [TestClass]
    public class ComponentScannerHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exceptions.ComponentsLibraryException))]
        public void CreateComponentIdTestNullFailure()
        {
            string componentName = null;
            IOSpecDefinition componentIOSpec = null;
            string version = null;
            ConfigWrapperDefinition componentConfiguration = null;

            //should throw exception, as there has to be at least name provided that is not null
            string id = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);
        }

        [TestMethod]
        public void CreateComponentIdTestName()
        {
            string componentName = "Mock component";
            IOSpecDefinition componentIOSpec = null;
            string version = null;
            ConfigWrapperDefinition componentConfiguration = null;

            string id = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            //repeat with the same data - ids should be the same
            string id2 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreEqual(id, id2);

            //change name
            componentName = "Some other component name";
            string id3 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreNotEqual(id, id3);
        }

        [TestMethod]
        public void CreateComponentIdTestNameAndVersion()
        {
            string componentName = "Mock component";
            IOSpecDefinition componentIOSpec = null;
            string version = "1.0";
            ConfigWrapperDefinition componentConfiguration = null;

            string id = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            //repeat with the same data - ids should be the same
            string id2 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreEqual(id, id2);

            //change version
            version = "2.0";
            string id3 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreNotEqual(id, id3);
        }

        [TestMethod]
        public void CreateComponentIdTestIOSpec()
        {
            string componentName = "Mock component";
            IOSpecDefinition componentIOSpec = new IOSpecDefinition();
            componentIOSpec.Input.Add("mockinput", new IOItemDefinition("mockinput", "mocktype", "mockdescription", TraceLabSDK.IOSpecType.Input));
            componentIOSpec.Output.Add("mockoutput", new IOItemDefinition("mockoutput", "mocktype", "mockdescription", TraceLabSDK.IOSpecType.Output));

            string version = "1.0";
            ConfigWrapperDefinition componentConfiguration = null;

            string id = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            //repeat with the same data - ids should be the same
            string id2 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreEqual(id, id2);

            //copy original iospec and add input to it
            IOSpecDefinition componentIOSpec2 = new IOSpecDefinition(componentIOSpec);
            componentIOSpec2.Input.Add("mockinput2", new IOItemDefinition("mockinput2", "mock.type", "mockdescription", TraceLabSDK.IOSpecType.Input));
            string id3 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec2, version, componentConfiguration);
            Assert.AreNotEqual(id, id3);

            //copy original iospec and add output to it
            IOSpecDefinition componentIOSpec3 = new IOSpecDefinition(componentIOSpec);
            componentIOSpec3.Output.Add("mockoutput2", new IOItemDefinition("mockoutput2", "mock.type", "mockdescription", TraceLabSDK.IOSpecType.Output));
            string id4 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec3, version, componentConfiguration);
            Assert.AreNotEqual(id, id4);

            //finally compare with empty iospec
            IOSpecDefinition componentIOSpec5 = new IOSpecDefinition();
            string id6 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec5, version, componentConfiguration);
            Assert.AreNotEqual(id, id6);
        }

        [TestMethod]
        public void CreateComponentIdTestConfig()
        {
            string componentName = "Mock component";
            IOSpecDefinition componentIOSpec = null;
            string version = "1.0";

            //create config definition using createwrapper method of the ComponentMetadataDefinition class
            ConfigWrapperDefinition componentConfiguration = ComponentScannerHelper_Accessor.CreateConfigWrapperDefinition(typeof(MockConfig));
            
            string id = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            //repeat with the same data - ids should be the same
            string id2 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreEqual(id, id2);

            //change configuration
            componentConfiguration = ComponentScannerHelper_Accessor.CreateConfigWrapperDefinition(typeof(MockConfigWithAdditionalProperty));
            string id3 = ComponentScannerHelper_Accessor.CreateComponentId(componentName, componentIOSpec, version, componentConfiguration);

            Assert.AreNotEqual(id, id3);
        }

        class MockConfig
        {
            public FilePath MockFile
            {
                get;
                set;
            }

            public DirectoryPath MockDirectory
            {
                get;
                set;
            }
        }

        class MockConfigWithAdditionalProperty
        {
            public FilePath MockFile
            {
                get;
                set;
            }

            public DirectoryPath MockDirectory
            {
                get;
                set;
            }

            public int MockIntValue
            {
                get;
                set;
            }
        }
    }
}

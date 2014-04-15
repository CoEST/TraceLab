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
using TraceLab.Core.Test.Setup;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Decisions;

namespace TraceLab.Core.Test.ExperimentExecution
{
    [TestClass]
    public class DecisionCompilationTest
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

        ///// <summary>
        /////Gets or sets the test context which provides
        /////information about and functionality for the current test run.
        /////</summary>
        //public TestContext TestContext
        //{
        //    get;
        //    set;
        //}

        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext, false);
        }

        [TestCleanup]
        public void TestTearDown() 
        {
            AppContext.Dispose();
            AppContext = null;
        }

        #region Copmile Standard Decision

        private void CompileDecision(string code)
        {
            DecisionMetadata decisionMetadata = new DecisionMetadata("Decision");
        
            Dictionary<string, string> mockSuccessorNodeLabelIdLookup = new Dictionary<string,string>();
            mockSuccessorNodeLabelIdLookup.Add("node 1 label", "Fake node 1 id");
            mockSuccessorNodeLabelIdLookup.Add("node 2 label", "Fake node 2 id");

            Dictionary<string, string> mockPredeccessorsOutputsNameTypeLookup = new Dictionary<string, string>();
            mockPredeccessorsOutputsNameTypeLookup.Add("test", "int");
            mockPredeccessorsOutputsNameTypeLookup.Add("targetArtifacts", "TraceLabSDK.Types.TLArtifactsCollection");
            mockPredeccessorsOutputsNameTypeLookup.Add("sourceArtifacts", "TraceLabSDK.Types.TLArtifactsCollection");

            decisionMetadata.DecisionCode = code;

            DecisionCompilationRunner_Accessor.BuildSourceAndCompileDecisionModule(decisionMetadata, mockSuccessorNodeLabelIdLookup, mockPredeccessorsOutputsNameTypeLookup, 
                                               AppContext.WorkspaceInstance.TypeDirectories, new LoggerNameRoot("Mock"));
        }

        [TestMethod]
        public void DecisionCodeCompilationTest1()
        {
            string sampleCode = @"if(Load(""test"") < 5) { Select(""node 1 label""); } else { Select(""node 2 label""); } ";
            CompileDecision(sampleCode);
        }

        [TestMethod]
        public void DecisionCodeCompilationTest2()
        {
            string sampleCode = @"  positionX = Load(""test"");
                                    if (positionX < 5)
                                    {
                                        Select(""node 1 label"");
                                    }
                                    else
                                    {
                                        Select(""node 2 label"");
                                    }";
            CompileDecision(sampleCode);
        }

        [TestMethod]
        public void DecisionCodeCompilationTest3()
        {
            string sampleCode = @"  int positionX = 5 + 1;
                                    positionX += Load(""test"");
                                    if (positionX < 5)
                                    {
                                        Select(""node 1 label"");
                                    }
                                    else
                                    {
                                        Select(""node 2 label"");
                                    }";
            CompileDecision(sampleCode);
        }

        /// <summary>
        /// nested ifs
        /// </summary>
        [TestMethod]
        public void DecisionCodeCompilationTest4()
        {
            string sampleCode = @"  int positionX = 5 + 1;
                                    positionX += Load(""test"");
                                    if (positionX > 5)
                                    {
                                        if (positionX < 10)
                                        {
                                            Select(""node 1 label"");
                                        } else {
                                            Select(""node 2 label"");
                                        }
                                    }
                                    else
                                    {
                                        Select(""node 2 label"");
                                    }";
            CompileDecision(sampleCode);
        }

        /// <summary>
        /// using not allowed statement
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DecisionCodeParserException))]
        public void DecisionCodeCompilationTest5()
        {
            string sampleCode = @"  positionX = Load(""test"");
                                    System.Threading.Thread.Sleep(2000);
                                    if (positionX < 5)
                                    {
                                        Select(""node 1 label"");
                                    }
                                    else
                                    {
                                        Select(""node 2 label"");
                                    }";
            CompileDecision(sampleCode);
        }

        /// <summary>
        /// using not allowed statement
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DecisionCodeParserException))]
        public void DecisionCodeCompilationTest6()
        {
            string sampleCode = @"  positionX = Load(""test"");
                                    TraceLab.Core.Components.AppContext.Components.ComponentsDirectoryPath = ""hack"";
                                    if (positionX < 5)
                                    {
                                        Select(""node 1 label"");
                                    }
                                    else
                                    {
                                        Select(""node 2 label"");
                                    }";
            CompileDecision(sampleCode);
        }

        [TestMethod]
        public void DecisionCodeCompilationTest7()
        {
            string sampleCode = @"  positionX = Load(""targetArtifacts"");
                                    positionY = Load(""sourceArtifacts"");
                                    if (positionX.Count > 1 && positionY.Count >1)
                                    {
                                        Select(""node 1 label"");
                                    }
                                    else
                                    {
                                        Select(""node 2 label"");
                                    }";
            CompileDecision(sampleCode);
        }

        [TestMethod]
        [ExpectedException(typeof(DecisionCodeParserException))]
        public void DecisionNodeConstructorIncorrectCodeSyntax()
        {
            CompileDecision("incorrect statement");
        }

        #endregion

        #region Loop Conditions Code compilation

        private void CompileLoopDecision(string code)
        {
            //constuct mock loop metadata - subgraph does not matter in code compilation
            LoopScopeMetadata mockLoopMetadata = new LoopScopeMetadata(null, "Mock loop", String.Empty);

            Dictionary<string, string> mockSuccessorNodeLabelIdLookup = new Dictionary<string, string>();

            Dictionary<string, string> mockPredeccessorsOutputsNameTypeLookup = new Dictionary<string, string>();
            mockPredeccessorsOutputsNameTypeLookup.Add("test", "int");

            mockLoopMetadata.DecisionCode = code;

            DecisionCompilationRunner_Accessor.BuildSourceAndCompileDecisionModule(mockLoopMetadata, mockSuccessorNodeLabelIdLookup, mockPredeccessorsOutputsNameTypeLookup,
                                               AppContext.WorkspaceInstance.TypeDirectories, new LoggerNameRoot("Mock"));
        }

        [TestMethod]
        public void LoopConditionCodeCompilationTest1()
        {
            string sampleCode = @"(int)Load(""test"") < 5";
            CompileLoopDecision(sampleCode);
        }

        #endregion

        /// <summary>
        /// Tests the method that fixes the namespaces by adding @ in front of the parts of namespace that equal any of the .Net keyword
        /// The problem is that java allows creating namespaces, that in .Net would not be valid. Thus IKVMC compiled stubs can have 
        /// namespaced including net keyword. These has to be processed.
        /// For example, 
        /// using my.space.fixed;
        /// would not compile, as fixed is a keyword.
        /// It can be fixed in following way
        /// using my.space.@fixed;
        /// </summary>
        [TestMethod]
        public void DecisionModuleCompilatorNamespaceFixTest()
        {
            string @namespace = "my.valid.space";
            @namespace = DecisionCodeBuilder_Accessor.NamespaceFix(@namespace);
            Assert.AreEqual("my.valid.space", @namespace);

            @namespace = "my.namespace";
            @namespace = DecisionCodeBuilder_Accessor.NamespaceFix(@namespace);
            Assert.AreEqual("my.@namespace", @namespace);

            @namespace = "my.event.space";
            @namespace = DecisionCodeBuilder_Accessor.NamespaceFix(@namespace);
            Assert.AreEqual("my.@event.space", @namespace);

            @namespace = "my.event.space.fixed";
            @namespace = DecisionCodeBuilder_Accessor.NamespaceFix(@namespace);
            Assert.AreEqual("my.@event.space.@fixed", @namespace);
        }


    }
}

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
using TraceLab.Core.Benchmarks;
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Experiments;
using TraceLab.Core.Components;
using TraceLab.Core.Test.ExperimentExecution;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Workspaces;
using TraceLab.Core.Test.WebserviceAccess;

namespace TraceLab.Core.Test.Benchmarks
{
    [TestClass]
    public class BenchmarksTest
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

            BenchmarkDirectory = System.IO.Path.Combine(AppContext.BaseTestDirectory, "Benchmarks");
        }

        [TestCleanup]
        public void TestTearDown()
        {
            AppContext.Dispose();
        }

        private string BenchmarkDirectory
        {
            get;
            set;
        }

        [TestMethod]
        public void LoadBenchmarkInfoTest()
        {
            List<Benchmark> benchmarks = BenchmarkLoader.LoadBenchmarksInfo(BenchmarkDirectory);
            Assert.IsNotNull(benchmarks);
            Assert.AreEqual(1, benchmarks.Count);

            Benchmark testBenchmark = benchmarks[0];
            Assert.IsNotNull(testBenchmark);
            Assert.AreEqual("Test Benchmark", testBenchmark.BenchmarkInfo.Name);
            Assert.AreEqual("Test Benchmark Description", testBenchmark.BenchmarkInfo.Description);
            Assert.IsNotNull(testBenchmark.ComponentTemplate);

            //at this moment benchmark experiment should be still null, it has not yet been loaded
            Assert.IsNull(testBenchmark.BenchmarkExperiment);

            Assert.IsNotNull(testBenchmark.ComponentTemplate.IOSpec);
            Assert.IsNotNull("Default Solution", testBenchmark.ComponentTemplate.Label);
            string benchmarkPath = System.IO.Path.Combine(BenchmarkDirectory, "TestBenchmark.tbml");
            Assert.AreEqual(benchmarkPath, testBenchmark.BenchmarkInfo.FilePath);
        }

        [TestMethod]
        public void PrepareAndRunBenchmarkExperiment()
        {
            Assert.Fail("Test temporarily broken. Ignored till contest feature is going to be revisited.");

            List<Benchmark> benchmarks = BenchmarkLoader.LoadBenchmarksInfo(BenchmarkDirectory);
            Benchmark testBenchmark = benchmarks[0];

            // load the experiment to be run against benchmark
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, "experiment_to_be_benchmarked.gml");
            Experiment experimentToBeBenchmarked = ExperimentManager.Load(experimentFilename, AppContext.Components);

            //prepare matching io
            testBenchmark.PrepareMatchingIOByType(experimentToBeBenchmarked);
            Assert.AreEqual(2, testBenchmark.BenchmarkInputSetting.Count);
            Assert.AreEqual(1, testBenchmark.BenchmarkOutputsSetting.Count);
            
            //match benchmarkSourceArtifact with original source artifacts
            foreach (BenchmarkItemSetting<IOItem> pair in testBenchmark.BenchmarkInputSetting) 
            {
                IOItem item = pair.Item;
                ItemSettingCollection candidates = pair.CandidateSettings;
                if (item.MappedTo.Equals("benchmarkSourceArtifacts"))
                {
                    //we found the item we want to remap
                    pair.SelectedSetting = candidates["originalSourceArtifacts"];
                }
            }

            //finally prepare benchmark experiment
            testBenchmark.PrepareBenchmarkExperiment(experimentToBeBenchmarked, AppContext.Components);

            //assert that only two inputs are included in the export settings and one output
            int includedInputs = 0;
            foreach (KeyValuePair<string, ItemSetting> pair in testBenchmark.Setup.InputSettings)
            {
                if (pair.Value.Include == true) includedInputs++;
            }
            Assert.AreEqual(2, includedInputs);

            int includedOutputs = 0;
            foreach (KeyValuePair<string, ItemSetting> pair in testBenchmark.Setup.OutputSettings)
            {
                if (pair.Value.Include == true) includedOutputs++;
            }
            Assert.AreEqual(1, includedOutputs);
            
            Assert.IsNotNull(testBenchmark.BenchmarkExperiment);

            //for debug output file
            // string path = System.IO.Path.Combine(AppContext.BaseTestDirectory, "benchmarkTest1.gml");
            // AppContext.ExperimentManager.Save(testBenchmark.BenchmarkExperiment, path);

            MockProgress progress = new MockProgress();

            using (var dispatcher = ExperimentRunnerHelper.CreateExperimentRunner(testBenchmark.BenchmarkExperiment, AppContext.WorkspaceInstance, AppContext.Components))
            {
                dispatcher.ExecuteExperiment(progress);
                Assert.AreEqual(7, progress.NumSteps);
                Assert.IsFalse(progress.HasError);
            }
        }

        /// <summary>
        /// The method tests defining benchmark.
        /// Once benchmark is defined it runs the testing solution against newly defined benchmark.
        /// The benchmark is defined based on  DefiningBenchmarkTestExperiment.teml
        /// The testing solution is DefiningBenchmarkTestingSolution.teml
        /// The benchmark uses DefiningBenchmarkTestData.xml
        /// 
        /// The test also checks if Decision Node are processed in proper way, when running benchmark.
        /// The decision code has to match new label of the template component, if the template component is successor of decision node.
        /// Thus all Select("old label") statements in the decision code has to be updated to Select("new label");
        /// If not, the test would fail, because decision code would not compile.
        /// </summary>
        [TestMethod]
        public void DefiningBenchmarkTest()
        {
            Assert.Fail("Test temporarily broken. Ignored till contest feature is going to be revisited.");

            string baseExperimentFilename = "DefiningBenchmarkTestExperiment.teml";
            string testingSolutionFilename = "DefiningBenchmarkTestingSolution.teml";

            //create temporary directory for defining benchmark
            string benchmarkTemporaryDirectory = System.IO.Path.Combine(AppContext.BaseTestDirectory, "DefiningBenchmarkTest");
            System.IO.Directory.CreateDirectory(benchmarkTemporaryDirectory);

            string newBenchmarkFilePath = System.IO.Path.Combine(benchmarkTemporaryDirectory, "newDefinedBenchmark.tbml");
            
            //copy the test data into temporary benchmark directory
            string testData = System.IO.Path.Combine(AppContext.BaseTestDirectory, "DefiningBenchmarkTestData.xml");
            System.IO.File.Copy(testData, System.IO.Path.Combine(benchmarkTemporaryDirectory, "DefiningBenchmarkTestData.xml"));
            
            // load the experiment from which the benchmark is going to be defined from
            string baseExperimentFilePath = System.IO.Path.Combine(AppContext.BaseTestDirectory, baseExperimentFilename);
            Experiment baseExperimentForDefiningBenchmark = ExperimentManager.Load(baseExperimentFilePath, AppContext.Components);

            var benchmarkDefiner = new DefiningBenchmark(baseExperimentForDefiningBenchmark, AppContext.Components, AppContext.WorkspaceInstance, AppContext.PackageManager, AppContext.WorkspaceInstance.TypeDirectories, null);

            Assert.AreEqual(1, benchmarkDefiner.TemplatizableComponents.Count);
            Assert.AreEqual("Preprocessor", benchmarkDefiner.TemplatizableComponents[0].Data.Metadata.Label);

            //select preprocessor template as Component Template for benchmarking
            benchmarkDefiner.SelectedTemplateNode = benchmarkDefiner.TemplatizableComponents[0];

            //select new benchmark path
            benchmarkDefiner.BenchmarkInfo.FilePath = newBenchmarkFilePath;

            //set some values for benchmark info
            string benchmarkName = "Testing defining new benchmark";
            string author = "Re test author";
            string contributors = "Re test contributors";
            string description = "Re test description";
            string shortDescription = "Re test short description";
            DateTime deadline = DateTime.Now;
            string fakeExperimentResultsUnitname = "fakeunitname";
            string webpageLink = "test://test.webpage.link";

            benchmarkDefiner.BenchmarkInfo.Name = benchmarkName;
            benchmarkDefiner.BenchmarkInfo.Author = author;
            benchmarkDefiner.BenchmarkInfo.Contributors = contributors;
            benchmarkDefiner.BenchmarkInfo.Description = description;
            benchmarkDefiner.BenchmarkInfo.ShortDescription = shortDescription;
            benchmarkDefiner.BenchmarkInfo.Deadline = deadline;
            benchmarkDefiner.BenchmarkInfo.ExperimentResultsUnitname = fakeExperimentResultsUnitname;
            benchmarkDefiner.BenchmarkInfo.WebPageLink = new Uri(webpageLink);

            //assure file does not exists prior defining
            Assert.IsFalse(System.IO.File.Exists(benchmarkDefiner.BenchmarkInfo.FilePath));

            //set some mock experiment results as baseline
            TraceLabSDK.Types.Contests.TLExperimentResults fakeBaseline = CreateDummyExperimentResults("FAKE-BASELINE");

            benchmarkDefiner.SelectedExperimentResults = fakeBaseline;

            //call define benchmark
            benchmarkDefiner.Define();

            //check if new benchmark has been created
            Assert.IsTrue(System.IO.File.Exists(benchmarkDefiner.BenchmarkInfo.FilePath));

            //load newly defined benchmark
            List<Benchmark> benchmarks = BenchmarkLoader.LoadBenchmarksInfo(benchmarkTemporaryDirectory);
            Benchmark testBenchmark = benchmarks[0]; //there should be only 1, since the directory has been just created

            //check if new test benchmark has previously defined properties
            Assert.AreEqual(benchmarkName, testBenchmark.BenchmarkInfo.Name);
            Assert.AreEqual(author, testBenchmark.BenchmarkInfo.Author);
            Assert.AreEqual(contributors, testBenchmark.BenchmarkInfo.Contributors);
            Assert.AreEqual(description, testBenchmark.BenchmarkInfo.Description);
            Assert.AreEqual(shortDescription, testBenchmark.BenchmarkInfo.ShortDescription);
            Assert.AreEqual(deadline.ToString(), testBenchmark.BenchmarkInfo.Deadline.ToString());
            Assert.AreEqual(fakeExperimentResultsUnitname, testBenchmark.BenchmarkInfo.ExperimentResultsUnitname);

            //check if baseline results has been saved properly, by loading it from xml
            TraceLabSDK.Types.Contests.TLExperimentResults baseline = BenchmarkLoader.ReadBaseline(benchmarkDefiner.BenchmarkInfo.FilePath);
            Assert.AreEqual(fakeBaseline.TechniqueName, baseline.TechniqueName);
            Assert.AreEqual(fakeBaseline.Score, baseline.Score);
            Assert.AreEqual(fakeBaseline.AcrossAllDatasetsResults, baseline.AcrossAllDatasetsResults);
            Assert.IsTrue(fakeBaseline.DatasetsResults.SequenceEqual(baseline.DatasetsResults));
           
            // load the experiment to be run against new defined benchmark
            string experimentFilename = System.IO.Path.Combine(AppContext.BaseTestDirectory, testingSolutionFilename);
            Experiment testingSolutionExperiment = ExperimentManager.Load(experimentFilename, AppContext.Components);

            //finally prepare benchmark experiment 
            testBenchmark.PrepareBenchmarkExperiment(testingSolutionExperiment, AppContext.Components);

            //run benchmark
            MockProgress progress = new MockProgress();

            using (var dispatcher = CreateExperiment(testBenchmark.BenchmarkExperiment, AppContext.WorkspaceInstance, AppContext.Components))
            {
                dispatcher.ExecuteExperiment(progress);
                Assert.AreEqual(5, progress.NumSteps);
                Assert.IsFalse(progress.HasError);
            }
        }


        private static IExperimentRunner CreateExperiment(Experiment experiment, Workspace workspace, ComponentsLibrary library)
        {
            RunnableExperimentBase graph = null;

            ExperimentWorkspaceWrapper experimentWorkspaceWrapper = WorkspaceWrapperFactory.CreateExperimentWorkspaceWrapper(workspace, experiment.ExperimentInfo.Id);
            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(experimentWorkspaceWrapper);
            graph = GraphAdapter.Adapt(experiment, templateGraphNodesFactory, library, workspace.TypeDirectories);

            //clear Workspace
            workspace.DeleteUnits(experiment.ExperimentInfo.Id);

            IExperimentRunner dispatcher = ExperimentRunnerFactory.CreateExperimentRunner(graph);

            return dispatcher;
        }

        /// <summary>
        /// Creates some dummy experiment results, to test its serialization
        /// </summary>
        /// <param name="techniqueName">Name of the technique.</param>
        /// <returns></returns>
        internal static TraceLabSDK.Types.Contests.TLExperimentResults CreateDummyExperimentResults(string techniqueName)
        {
            var dummyResults = new TraceLabSDK.Types.Contests.TLExperimentResults(techniqueName);

            dummyResults.BaseData = MockContestResultsHelper.CreateDummyBaseData();

            //create some metrics
            var datasetResults1 = MockContestResultsHelper.CreateDummyDatasetResults("Dataset 1",
                MockContestResultsHelper.CreateDummySeriesMetricResults("Mock series metric", "Mock series metric description"),
                MockContestResultsHelper.CreateDummyBoxSummaryMetricResults("Mock box summary metric", "Mock box summary metric description"));
            var datasetResults2 = MockContestResultsHelper.CreateDummyDatasetResults("Dataset 2",
                MockContestResultsHelper.CreateDummySeriesMetricResults("Mock series metric", "Mock series metric description"),
                MockContestResultsHelper.CreateDummyBoxSummaryMetricResults("Mock box summary metric", "Mock box summary metric description"));
            var datasetResults3 = MockContestResultsHelper.CreateDummyDatasetResults("Across all datasets",
                MockContestResultsHelper.CreateDummySeriesMetricResults("Mock series metric", "Mock series metric description"),
                MockContestResultsHelper.CreateDummyBoxSummaryMetricResults("Mock box summary metric", "Mock box summary metric description"));

            dummyResults.AddDatasetResult(datasetResults1);
            dummyResults.AddDatasetResult(datasetResults2);
            dummyResults.AcrossAllDatasetsResults = datasetResults3;

            return dummyResults;
        }
    }
}

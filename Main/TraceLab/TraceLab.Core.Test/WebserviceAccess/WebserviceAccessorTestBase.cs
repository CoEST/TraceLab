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
using TraceLab.Core.WebserviceAccess;
using System.IO;
using TraceLab.Core.WebserviceAccess.Metrics;
using System.Xml.Serialization;

namespace TraceLab.Core.Test.WebserviceAccess
{
    [TestClass]
    public abstract class WebserviceAccessorTestBase
    {
        protected virtual WebserviceAccessor WebService
        {
            get;
            set;
        }
        protected virtual Credentials ValidCredentials
        {
            get;
            set;
        }

        protected virtual IXmlSerializable FakeBaseData
        {
            get;
            set;
        }

        [TestInitialize]
        public void Setup()
        {
            //create mock box line series metric
            LineSeriesMetric = MockContestResultsHelper.CreateDummySeriesMetricResults(m_mockSeriesMetricName, "Mock series metric description");

            //create mock box summary metric
            BoxSummaryMetric = MockContestResultsHelper.CreateDummyBoxSummaryMetricResults(m_mockBoxPlotMetricName, "Mock box plot metric description");

            DummyCorrectResults = GenerateDummyResults(LineSeriesMetric, BoxSummaryMetric);

            FakeBaseData = MockContestResultsHelper.CreateDummyBaseData();
        }

        [TestMethod]
        public virtual void AuthenticationValidCredentialsTest()
        {
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);
            Assert.IsNotNull(authenticationResponse.Ticket);
            Assert.IsNull(authenticationResponse.ErrorMessage);
        }

        [TestMethod]
        public virtual void AuthenticationEmptyPasswordTest()
        {
            var invalidCredentials = new Credentials() { Username = "tracelabtest", Password = "" };

            AuthenticationResponse authenticationResponse = AuthenticateSync(invalidCredentials);

            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, authenticationResponse.Status);
            Assert.IsNull(authenticationResponse.Ticket);
            //the error message comes straight from joomla authentication module
            Assert.IsNotNull(authenticationResponse.ErrorMessage);
            //The default message is 'Empty password not allowed'
            Assert.AreEqual("Empty password not allowed", authenticationResponse.ErrorMessage);
            System.Diagnostics.Debug.Write(authenticationResponse.ErrorMessage);
        }

        [TestMethod]
        public virtual void AuthenticationNotExistingUserTest()
        {
            var invalidCredentials = new Credentials() { Username = "notexistinguser", Password = "somepassword" };

            AuthenticationResponse authenticationResponse = AuthenticateSync(invalidCredentials);

            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, authenticationResponse.Status);
            Assert.IsNull(authenticationResponse.Ticket);
            //the error message comes straight from joomla authentication module
            Assert.IsNotNull(authenticationResponse.ErrorMessage);
            //The default message is 'User does not exist'
            Assert.AreEqual("User does not exist", authenticationResponse.ErrorMessage);
            System.Diagnostics.Debug.Write(authenticationResponse.ErrorMessage);
        }

        /// <summary>
        /// Authenticates and publishes the contest
        /// </summary>
        [TestMethod]
        public virtual void PublishContestTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishContestTest");

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);
            Assert.IsNull(response.ErrorMessage);
            Assert.IsNotNull(response.ContestWebsiteLink);
        }


        /// <summary>
        /// Authenticates and publishes the contest
        /// </summary>
        [TestMethod]
        public virtual void PublishContestWithBaselineResultsTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishContestWithBaselineResultsTest");

            //create dummy contest results (they have to match contest metric definitions
            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(dummyContest.ContestGUID, "Baseline", "Baseline technique description", DummyCorrectResults, score, FakeBaseData);

            dummyContest.BaselineResults = contestResults;

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);
            Assert.IsNull(response.ErrorMessage);
            Assert.IsNotNull(response.ContestWebsiteLink);
        }

        /// <summary>
        /// Authenticates and publishes the contest
        /// </summary>
        [TestMethod]
        public virtual void PublishContestWithIncorrectBaselineResultsTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishContestWithIncorrectBaselineResultsTest");

            //create dummy contest results that are incorrect (they do not match contest metric definitions)
            var seriesMetric = MockContestResultsHelper.CreateDummySeriesMetricResults("Not matching series metric name", "Some description");
            var boxMetric = MockContestResultsHelper.CreateDummyBoxSummaryMetricResults("Not matchin box metric name", "Some description");
            var resultDataset1 = CreateDummyResultsDTO("Dataset 1", seriesMetric, boxMetric); //the name of metric won't match with metric definitions in the contest
            var allResults = new List<DatasetResultsDTO>() { resultDataset1 };

            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(dummyContest.ContestGUID, "Baseline", "Baseline technique description", allResults, score, FakeBaseData);

            dummyContest.BaselineResults = contestResults;

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, response.Status);
            Assert.IsNotNull(response.ErrorMessage);
            Assert.AreEqual("Baseline results failed to be saved. Metric results does not match metrics defined in the contest.", response.ErrorMessage);
        }
                
        /// <summary>
        /// Authenticates and attemps to publish the contest with a wrong ticket
        /// </summary>
        [TestMethod]
        public virtual void PublishContestWrongTicketTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishContestWrongTicketTest");

            //don't use authenticationResponse.Ticket, instead pass different fake ticket
            Response response = PublishContestSync("fake ticket", ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, response.Status);
            Assert.IsNotNull(response.ErrorMessage);
            Assert.AreEqual("Given ticket is invalid or has expired. ", response.ErrorMessage);
        }

        [TestMethod]
        public virtual void RetrieveListOfContestsTest()
        {
            ListOfContestsResponse listOfContestsResponse = RetriveListOfContestsSync();
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, listOfContestsResponse.Status);
            Assert.IsNull(listOfContestsResponse.ErrorMessage);
            var listOfContests = listOfContestsResponse.ListOfContests;

            //in case there is not contest yet published attempt to publish one contest
            if (listOfContests.Count == 0)
            {
                //authenticate
                AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

                Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);
                
                //create dummy contest
                var dummyContest = CreateDummyContest("RetrieveListOfContestsTest");

                //publish contest
                Response response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

                Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);
            }

            //try retrieving the list of contests again and check if there is at least one contest in the list
            listOfContestsResponse = RetriveListOfContestsSync();
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, listOfContestsResponse.Status);
            Assert.IsNull(listOfContestsResponse.ErrorMessage);
            listOfContests = listOfContestsResponse.ListOfContests;

            Assert.IsTrue(listOfContests.Count > 0);

            //print list of contests
            foreach (Contest contest in listOfContests)
            {
                Assert.IsNotNull(contest); //check if contest is not null
                Assert.IsNull(contest.PackageFileContent);  //package file content should be null - retrieving the list of contest should not load the package files

                System.Diagnostics.Debug.WriteLine(String.Format("Contest name: {0}, Author: {1}, Short Description: {2}, Contest Guid: {3}", contest.Name, contest.Author, contest.ShortDescription, contest.ContestGUID));
            }
        }

        [TestMethod]
        public virtual void DownloadContestPackageTest()
        {
            //publish new dummy contest
            var authenticationResponse = AuthenticateSync(ValidCredentials);
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("DownloadContestPackageTest");

            var response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //now try to download the package of the dummy contest from webservice
            string contestPackage = DownloadContestPackageSync(dummyContest.ContestGUID);

            Assert.AreEqual(dummyContest.PackageFileContent, contestPackage);
        }

        [TestMethod]
        public virtual void PublishContestResultTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishContestResultTest");
            
            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);
            
            //once the contest is published test publishing contest results

            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(dummyContest.ContestGUID, 
                                                                "Mock technique", "Mock technique description", DummyCorrectResults, score, FakeBaseData);
            
            //reuse the same ticket and valid credentials... 
            //typically it would be another user, but it doesn't matter for test purposes (it may be so the contest creator also publishes results)
            ContestResultsPublishedResponse resultPublishedResponse = PublishContestResultSync(authenticationResponse.Ticket, ValidCredentials.Username, contestResults);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, resultPublishedResponse.Status);
            Assert.IsNull(resultPublishedResponse.ErrorMessage);
            
            //create other set of results for the same metrics
            var lineSeriesMetric = MockContestResultsHelper.CreateDummySeriesMetricResults(m_mockSeriesMetricName, "Mock series metric description");
            var boxSummaryMetric = MockContestResultsHelper.CreateDummyBoxSummaryMetricResults(m_mockBoxPlotMetricName, "Mock box plot metric description");
            var otherCorrectResults = GenerateDummyResults(lineSeriesMetric, boxSummaryMetric);

            score = random.NextDouble();
            contestResults = new ContestResults(dummyContest.ContestGUID, "Another technique", "Another technique description", otherCorrectResults, score, FakeBaseData);
            resultPublishedResponse = PublishContestResultSync(authenticationResponse.Ticket, ValidCredentials.Username, contestResults);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, resultPublishedResponse.Status);
            Assert.IsNull(resultPublishedResponse.ErrorMessage);
            Assert.IsNotNull(resultPublishedResponse.ContestWebsiteLink);
        }

        [TestMethod]
        public virtual void PublishEmptyContestResultTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishEmptyContestResultTest");

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //once the contest is published test publishing contest results

            //create dummy contest results (they have to match contest metric definitions
            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(dummyContest.ContestGUID, "Mock technique", "Mock technique description", new List<DatasetResultsDTO>(), score, FakeBaseData);

            //reuse the same ticket and valid credentials... 
            //typically it would be another user, but it doesn't matter for test purposes (it may be so the contest creator also publishes results)
            ContestResultsPublishedResponse resultPublishedResponse = PublishContestResultSync(authenticationResponse.Ticket, ValidCredentials.Username, contestResults);

            //cannot publish empty results
            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, resultPublishedResponse.Status);
            Assert.IsNotNull(resultPublishedResponse.ErrorMessage);
            Assert.AreEqual("Metric results does not match metrics defined in the contest.", resultPublishedResponse.ErrorMessage);
        }

        [TestMethod]
        public virtual void PublishContestResultToNotExistingContestTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            
            var dummyContest = CreateDummyContest("PublishContestResultToNotExistingContestTest");

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //once the contest is published test publishing contest results

            //create dummy contest results, they have to match contest metric definitions
            var results = GenerateDummyResults(LineSeriesMetric, BoxSummaryMetric);

            //generate new valid guid, but to contest that does not exist
            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(Guid.NewGuid().ToString(), "Mock technique", "Mock technique description", results, score, FakeBaseData);

            //reuse the same ticket and valid credentials... 
            //typically it would be another user, but it doesn't matter for test purposes (it may be so the contest creator also publishes results)
            ContestResultsPublishedResponse resultPublishedResponse = PublishContestResultSync(authenticationResponse.Ticket, ValidCredentials.Username, contestResults);

            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, resultPublishedResponse.Status);
            Assert.IsNotNull(resultPublishedResponse.ErrorMessage);
            Assert.AreEqual("The contest of the given id does not exists.", resultPublishedResponse.ErrorMessage);
        }

        [TestMethod]
        public virtual void PublishContestResultAfterDeadlineTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishContestResultAfterDeadlineTest");

            //set some deadline, that has already passed
            dummyContest.SetDeadline(new DateTime(1999, 1, 12));

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //once the contest is published test publishing contest results

            //create dummy contest results
            var results = GenerateDummyResults(LineSeriesMetric, BoxSummaryMetric);

            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(dummyContest.ContestGUID, "Mock technique", "Mock technique description", results, score, FakeBaseData);

            //reuse the same ticket and valid credentials... 
            //typically it would be another user, but it doesn't matter for test purposes (it may be so the contest creator also publishes results)
            ContestResultsPublishedResponse resultPublishedResponse = PublishContestResultSync(authenticationResponse.Ticket, ValidCredentials.Username, contestResults);

            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, resultPublishedResponse.Status);
            Assert.IsNotNull(resultPublishedResponse.ErrorMessage);
            Assert.AreEqual("The contest results could not been published, because it is passed the deadline.", resultPublishedResponse.ErrorMessage);
        }

        [TestMethod]
        public virtual void PublishNotMatchingContestResultTest()
        {
            //authenticate with valid credentials
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);

            //check if authentication succeed
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = CreateDummyContest("PublishNotMatchingContestResultTest");

            //the authentication provides the ticket necessary to publish the contest
            ContestPublishedResponse response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //once the contest is published test publishing contest results

            //create dummy contest results, that have metric that does not match contest metric definitions
            var seriesMetric = MockContestResultsHelper.CreateDummySeriesMetricResults("Not matching series metric name", "Some description");
            var boxMetric = MockContestResultsHelper.CreateDummyBoxSummaryMetricResults("Not matchin box metric name", "Some description");
            var resultDataset1 = CreateDummyResultsDTO("Dataset 1", seriesMetric, boxMetric); //the name of metric won't match with metric definitions in the contest
            var allResults = new List<DatasetResultsDTO>() { resultDataset1 };

            Random random = new Random();
            double score = random.NextDouble();
            ContestResults contestResults = new ContestResults(dummyContest.ContestGUID, "Mock technique", "Mock technique description", allResults, score, FakeBaseData);

            //reuse the same ticket and valid credentials... 
            //typically it would be another user, but it doesn't matter for test purposes (it may be so the contest creator also publishes results)
            ContestResultsPublishedResponse resultPublishedResponse = PublishContestResultSync(authenticationResponse.Ticket, ValidCredentials.Username, contestResults);

            //cannot publish empty results
            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, resultPublishedResponse.Status);
            Assert.IsNotNull(resultPublishedResponse.ErrorMessage);
            Assert.AreEqual("Metric results does not match metrics defined in the contest.", resultPublishedResponse.ErrorMessage);
        }

        /// <summary>
        /// Does the async call as synchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="act">The act.</param>
        /// <returns></returns>
        protected T DoAsyncCallAsSynchronous<T>(Action<Callback<T>> act) where T : Response, new()
        {
            T response = null;
            using (var waiter = new System.Threading.ManualResetEventSlim())
            {
                var callback = new Callback<T>();
                callback.CallCompleted += (sender, args) =>
                {
                    response = args.Response;
                    waiter.Set();
                };

                act(callback);

                waiter.Wait(); //wait until call service is done (when waiter is set)
            }

            return response;
        }

        protected ContestPublishedResponse PublishContestSync(string ticket, string username, Contest dummyContest)
        {
            ContestPublishedResponse response = DoAsyncCallAsSynchronous<ContestPublishedResponse>(
                (Callback<ContestPublishedResponse> callback) =>
                {
                    WebService.PublishContest(ticket, username, dummyContest, callback);
                });
            return response;
        }

        protected AuthenticationResponse AuthenticateSync(Credentials credentials)
        {
            AuthenticationResponse authenticationResponse = DoAsyncCallAsSynchronous<AuthenticationResponse>(
                (Callback<AuthenticationResponse> callback) =>
                {
                    WebService.Authenticate(credentials, callback);
                });
            return authenticationResponse;
        }

        protected ListOfContestsResponse RetriveListOfContestsSync()
        {
            ListOfContestsResponse listOfContests = DoAsyncCallAsSynchronous<ListOfContestsResponse>(
                (Callback<ListOfContestsResponse> callback) =>
                {
                    WebService.RetrieveListOfContests(callback);
                });
            return listOfContests;
        }

        protected string DownloadContestPackageSync(string contestGuid)
        {
            ContestPackageResponse contestPackage = DoAsyncCallAsSynchronous<ContestPackageResponse>(
                (Callback<ContestPackageResponse> callback) =>
                {
                    WebService.DownloadContestPackage(contestGuid, callback);
                });
            return contestPackage.ContestPackage;
        }

        protected ContestResultsPublishedResponse PublishContestResultSync(string ticket, string username, ContestResults contestResults)
        {
            ContestResultsPublishedResponse response = DoAsyncCallAsSynchronous<ContestResultsPublishedResponse>(
                (Callback<ContestResultsPublishedResponse> callback) =>
                {
                    WebService.PublishContestResults(ticket, username, contestResults, callback);
                });
            return response;
        }

        protected static string m_mockSeriesMetricName = "Mock series metric";
        protected static string m_mockBoxPlotMetricName = "Mock box plot metric";

        /// <summary>
        /// Creates the dummy contest.
        /// </summary>
        /// <param name="callingMethod">The calling method, just to extract name of calling test so that contest can be identified when published.</param>
        /// <returns></returns>
        protected virtual Contest CreateDummyContest(string callingMethod, byte[] bytes, string filename, string filetype)
        {            
            //prepare list of metric definitions
            var metrics = new List<MetricDefinition>();
            metrics.Add(new MetricDefinition(LineSeriesMetric));
            metrics.Add(new MetricDefinition(BoxSummaryMetric));

            var contestName = "TEST Contest : " + callingMethod;

            //prepare contest to publish
            Contest newContest = new Contest(Guid.NewGuid().ToString(),
                                            contestName,
                                            "TraceLab Test User",
                                            "Dummy contributor",
                                            "This is dummy contest - short description",
                                            "This is dummy contest - long description",
                                            new DateTime(2015, 1, 12),
                                            metrics,
                                            Datasets,
                                            bytes,
                                            filename,
                                            filetype
                                            );

            return newContest;
        }

        /// <summary>
        /// Creates the dummy contest.
        /// </summary>
        /// <param name="callingMethod">The calling method, just to extract name of calling test so that contest can be identified when published.</param>
        /// <returns></returns>
        protected virtual Contest CreateDummyContest(string callingMethod)
        {
            //read some file
            byte[] bytes = ReadDummyContestBytes("TraceLab.Core.Test.TestResources.DummyContestFile.pdf");

            return CreateDummyContest(callingMethod, bytes, "DummyContestFile.pdf", "application/pdf");
        }

        private static List<string> Datasets = new List<string>() { "Dataset 1", "Dataset 2" };
        private List<DatasetResultsDTO> DummyCorrectResults;
        private TraceLabSDK.Types.Contests.LineSeries LineSeriesMetric;
        private TraceLabSDK.Types.Contests.BoxSummaryData BoxSummaryMetric;

        private List<DatasetResultsDTO> GenerateDummyResults(TraceLabSDK.Types.Contests.LineSeries lineSeriesMetric, TraceLabSDK.Types.Contests.BoxSummaryData boxSummaryMetric)
        {
            var allResults = new List<DatasetResultsDTO>();
            foreach (string datasetName in Datasets)
            {
                allResults.Add(CreateDummyResultsDTO(datasetName, lineSeriesMetric, boxSummaryMetric));
            }
            return allResults;
        }

        
        private static DatasetResultsDTO CreateDummyResultsDTO(string datasetName, 
                                                         TraceLabSDK.Types.Contests.LineSeries lineSeriesMetric, 
                                                         TraceLabSDK.Types.Contests.BoxSummaryData boxSummaryMetric)
        {
            return new DatasetResultsDTO(MockContestResultsHelper.CreateDummyDatasetResults(datasetName, lineSeriesMetric, boxSummaryMetric));
        }
        
        protected static byte[] ReadDummyContestBytes(string manifestFileName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            var resourceStream = assembly.GetManifestResourceStream(manifestFileName);
         
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(resourceStream))
            {
                bytes = br.ReadBytes((int)resourceStream.Length);
            }
            return bytes;
        }
    }
}


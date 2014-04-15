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
using TraceLab.Core.WebserviceAccess;

namespace TraceLab.Core.Test.WebserviceAccess
{
    /// <summary>
    /// This class tests the client to response to the mock webservice. 
    /// The test does not require anything external to be installed, as the webrequest are mocked
    /// by MockWebservice.
    /// </summary>
    [TestClass]
    public class WebserviceAccessorClientTest : WebserviceAccessorTestBase
    {
        private static MockWebservice m_mockWebservice = new MockWebservice();

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            bool isRegistered = System.Net.WebRequest.RegisterPrefix("test", m_mockWebservice);
            Assert.IsTrue(isRegistered);
        }

        [TestInitialize]
        public void TestSetup()
        {
            WebService = new WebserviceAccessor("test://webservice.php", true);
            ValidCredentials = new Credentials() { Username = "tracelabtest", Password = "tracelabtest" };
        }

        [TestCleanup]
        public void TestTearDown()
        {
            m_mockWebservice.Reset();
        }

        #region Constructor tests

        [TestMethod]
        [ExpectedException(typeof(System.UriFormatException))]
        public void InvalidAddressTest()
        {
            WebService = new WebserviceAccessor("invalid url address");
        }

        /// <summary>
        /// Checks 2nd constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.UriFormatException))]
        public void InvalidAddressTest2()
        {
            WebService = new WebserviceAccessor("invalid url address", true);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NullWebserviceAddressTest()
        {
            WebService = new WebserviceAccessor(null);
        }

        /// <summary>
        /// Checks 2nd contructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NullWebserviceAddressTest2()
        {
            WebService = new WebserviceAccessor(null, true);
        }

        #endregion

        private string ValidAuthenticationResponse = @"{""Ticket"":""60338FDB-8342-4B05-A16A-9EE7FF3604EB"",""Status"":1,""ErrorMessage"":null}";
        private string PublishContestSuccessfulResponse = @"{""ContestWebsiteLink"":""test://fake.website.link"",""Status"":1,""ErrorMessage"":null}";
        private string PublishContestResultsSuccessfulResponse = @"{""ContestWebsiteLink"":""test:\/\/fakecontest.website.link"",""Status"":1,""ErrorMessage"":null}";

        [TestMethod]
        public override void AuthenticationValidCredentialsTest()
        {
            //add expected message to the mock webservice
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);

            base.AuthenticationValidCredentialsTest();
        }

        [TestMethod]
        public override void AuthenticationEmptyPasswordTest()
        {
            //add expected message to the mock webservice
            string emptyPasswordNotAllowed = @"{""Ticket"":null,""Status"":4,""ErrorMessage"":""Empty password not allowed""}";
            m_mockWebservice.EnqueueResponse(emptyPasswordNotAllowed);

            base.AuthenticationEmptyPasswordTest();
        }

        [TestMethod]
        public override void AuthenticationNotExistingUserTest()
        {
            //add expected message to the mock webservice
            string userDoesNotExistResponse = @"{""Ticket"":null,""Status"":4,""ErrorMessage"":""User does not exist""}";
            m_mockWebservice.EnqueueResponse(userDoesNotExistResponse);

            base.AuthenticationNotExistingUserTest();
        }

        [TestMethod]
        public override void PublishContestTest()
        {
            //add expected messages to the mock webservice
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);

            base.PublishContestTest();
        }

        [TestMethod]
        public override void PublishContestWrongTicketTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            string incorrectTicketResponse = @"{""Status"":4,""ErrorMessage"":""Given ticket is invalid or has expired. ""}";
            m_mockWebservice.EnqueueResponse(incorrectTicketResponse);

            base.PublishContestWrongTicketTest();
        }

        [TestMethod]
        public override void RetrieveListOfContestsTest()
        {
            //1. empty list response
            m_mockWebservice.EnqueueResponse(@"{""ListOfContests"":[],""Status"":1,""ErrorMessage"":null}");

            //2. if there is empty list the test attempts to publish contest
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);

            //3. return the list including one contest
            m_mockWebservice.EnqueueResponse(@"{""ListOfContests"":[{""ContestIndex"":""1"",
                                                    ""ContestGUID"":""c4259527-7fe8-4172-9016-90a9ea06e90f"",
                                                    ""Name"":""Dummy contest"",
                                                    ""Author"":""TraceLab Test User"",
                                                    ""Contributors"":""Dummy contributor"",
                                                    ""ShortDescription"":""This is dummy contest - short description"",
                                                    ""Description"":""This is dummy contest - long description"",
                                                    ""Deadline"":""2012-12-12 00:00:00""}],
                                                    ""Status"":1,""ErrorMessage"":null}");
                base.RetrieveListOfContestsTest();
        }

        [TestMethod]
        public override void DownloadContestPackageTest()
        {
            //response to successful authentication
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);

            //response to successful publish contest
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);

            //response that contain the content package
            byte[] contestBytes = ReadDummyContestBytes("TraceLab.Core.Test.TestResources.DummyContestFile.pdf");
            string contestPackageResponse = @"{""ContestPackage"":"""+Convert.ToBase64String(contestBytes)+@""",""Status"":1,""ErrorMessage"":null}";
            m_mockWebservice.EnqueueResponse(contestPackageResponse);

            base.DownloadContestPackageTest();
        }

        [TestMethod]
        public override void PublishContestResultTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);
            //try publish results first time
            m_mockWebservice.EnqueueResponse(PublishContestResultsSuccessfulResponse);
            //try publish results second time
            m_mockWebservice.EnqueueResponse(PublishContestResultsSuccessfulResponse);

            base.PublishContestResultTest();
        }

        [TestMethod]
        public override void PublishContestResultAfterDeadlineTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);
            m_mockWebservice.EnqueueResponse(@"{""Status"":4,""ErrorMessage"":""The contest results could not been published, because it is passed the deadline.""}");

            base.PublishContestResultAfterDeadlineTest();
        }

        [TestMethod]
        public override void PublishContestResultToNotExistingContestTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);
            m_mockWebservice.EnqueueResponse(@"{""Status"":4,""ErrorMessage"":""The contest of the given id does not exists.""}");
            base.PublishContestResultToNotExistingContestTest();
        }

        [TestMethod]
        public override void PublishEmptyContestResultTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);
            m_mockWebservice.EnqueueResponse(@"{""Status"":4,""ErrorMessage"":""Metric results does not match metrics defined in the contest.""}");
            base.PublishEmptyContestResultTest();
        }

        [TestMethod]
        public override void PublishNotMatchingContestResultTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);
            m_mockWebservice.EnqueueResponse(@"{""Status"":4,""ErrorMessage"":""Metric results does not match metrics defined in the contest.""}");
            base.PublishNotMatchingContestResultTest();
        }

        [TestMethod]
        public override void PublishContestWithBaselineResultsTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(PublishContestSuccessfulResponse);
            base.PublishContestWithBaselineResultsTest();
        }

        [TestMethod]
        public override void PublishContestWithIncorrectBaselineResultsTest()
        {
            m_mockWebservice.EnqueueResponse(ValidAuthenticationResponse);
            m_mockWebservice.EnqueueResponse(@"{""ContestWebsiteLink"":null,""Status"":4,""ErrorMessage"":""Baseline results failed to be saved. Metric results does not match metrics defined in the contest.""}");
            base.PublishContestWithIncorrectBaselineResultsTest();
        }


    }
}

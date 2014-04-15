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
using System.Runtime.Serialization;
using System.Net;
using System.Xml.XPath;
using TraceLabSDK.Types.Contests;
using TraceLab.Core.WebserviceAccess.Metrics;

namespace TraceLab.Core.Test.WebserviceAccess
{
    /// <summary>
    /// This class tests connection to the json webservice in joomla
    /// 
    /// To have successfully running tests, install joomla on your localhost apache server optionally with OpenSSL.
    /// Create user in joomla with credentials:
    /// Username: tracelabtest
    /// Password: tracelabtest
    /// 
    /// Install the jtracelab component in joomla
    /// </summary>
    [TestClass]
    public class WebserviceAccessorTest : WebserviceAccessorTestBase
    {
        [TestInitialize]
        public void TestSetup()
        {
            //WebService = new WebserviceAccessor("https://localhost/administrator/components/com_jtracelab/webservice/webservice.php", true);
            //WebService = new WebserviceAccessor("http://coest.org/testcontests/administrator/components/com_jtracelab/webservice/webservice.php", true);
        
            WebService = new WebserviceAccessor("http://coest.org/testjoomla/administrator/components/com_jtracelab/webservice/WebService.php", true);
            
            ValidCredentials = new Credentials() { Username = "tracelabtest", Password = "tracelabtest" };
        }

        [TestMethod]
        public void AddressNotFoundTest()
        {
            WebService = new WebserviceAccessor("https://localhost/notexisitingfakeadress", true);
            AuthenticationResponse authenticationResponse = AuthenticateSync(ValidCredentials);
            Assert.AreEqual(ResponseStatus.STATUS_FAILURE, authenticationResponse.Status);
            Assert.IsNull(authenticationResponse.Ticket);
            Assert.IsNotNull(authenticationResponse.ErrorMessage);
            System.Diagnostics.Debug.WriteLine(authenticationResponse.ErrorMessage); //404 error
        }


        /// <summary>
        /// Publishes the contest with large contest file.
        /// This allows to test if php can handle large contest file. 
        /// If server settings are not correct json_decode method cannot handle it and server response is just '\n' whitespace character.
        /// </summary>
        [TestMethod]
        public void PublishLargeContest()
        {
            var authenticationResponse = AuthenticateSync(ValidCredentials);
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            byte[] bytes = ReadDummyContestBytes("TraceLab.Core.Test.TestResources.TestLargeContestFile.tbml");
            var dummyContest = CreateDummyContest("DownloadContestPackageTest", bytes, "TestLargeContestFile.tbml", "application/xml");

            var response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //now try to download the package of the dummy contest from webservice
            string contestPackage = DownloadContestPackageSync(dummyContest.ContestGUID);

            Assert.AreEqual(dummyContest.PackageFileContent, contestPackage);
        }

        /// <summary>
        /// Publishes the contest with large baseline.
        /// The contest file itself is not as big, it is less than 8Mb, so json_decode should be handled fine,
        /// but adding the large baseline, the entire Contest message to be processed by server is large.
        /// Server needs to publish contest, and publish the baseline results.
        /// 
        /// mySql might have a problem with processing large inputs as it is in case of publishing the baseline.
        /// The Baseline consists of 
        /// -> BaseData that has 4 large similarity matrices
        /// -> 4 Dataset Results each of data for 5 Metrics
        /// 
        /// Unfortunately, the response for large contest file and contest with large baseline in both cases is '\n',
        /// which basically means, that php script failed totally...
        /// Server needs to have proper settings to handle large data.
        /// </summary>
        [TestMethod]
        public void PublishContestWithLargeBaseline()
        {
            var authenticationResponse = AuthenticateSync(ValidCredentials);
            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, authenticationResponse.Status);

            //create dummy contest
            var dummyContest = BuildContestWithLargeBaseline();
          
            var response = PublishContestSync(authenticationResponse.Ticket, ValidCredentials.Username, dummyContest);

            Assert.AreEqual(ResponseStatus.STATUS_SUCCESS, response.Status);

            //now try to download the package of the dummy contest from webservice
            string contestPackage = DownloadContestPackageSync(dummyContest.ContestGUID);

            Assert.AreEqual(dummyContest.PackageFileContent, contestPackage);
        }

        private Contest BuildContestWithLargeBaseline()
        {
            byte[] bytes = ReadDummyContestBytes("TraceLab.Core.Test.TestResources.ContestWithLargeBaseline.tbml");
            var dummyContest = CreateDummyContest("PublishContestWithLargeBaseline", bytes, "TestLargeContestFile.tbml", "application/xml");

            //read baseline results from file
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream("TraceLab.Core.Test.TestResources.ContestWithLargeBaseline.tbml");

            using (var reader = System.Xml.XmlReader.Create(resourceStream))
            {
                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();
                XPathNavigator iter = nav.SelectSingleNode("//TLExperimentResults");
                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TLExperimentResults), null);
                TLExperimentResults baseline = (TLExperimentResults)serializer.Deserialize(iter.ReadSubtree());

                dummyContest.BaselineResults = TraceLab.Core.Benchmarks.BenchmarkResultsHelper.PrepareBaselineContestRestults(dummyContest.ContestGUID, baseline, "BASELINE", "MOCK BASELINE");

                //extract metrics definitions out of selected Experiment Results
                List<MetricDefinition> metrics;
                List<string> datasets;
                TraceLab.Core.Benchmarks.BenchmarkResultsHelper.ExtractDatasetsAndMetricsDefinitions(baseline, out metrics, out datasets);

                dummyContest.Metrics = metrics;
                dummyContest.Datasets = datasets;
            }
            return dummyContest;
        }
    }
}

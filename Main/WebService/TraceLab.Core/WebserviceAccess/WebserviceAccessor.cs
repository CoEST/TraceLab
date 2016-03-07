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
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using TraceLabSDK;
using TraceLab.Core.Exceptions;
using TraceLab.Core.WebserviceAccess.Metrics;

namespace TraceLab.Core.WebserviceAccess
{
    public class WebserviceAccessor
    {
        private Uri m_webserviceUrl;
        private bool m_acceptAllSSLCertifications = false;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WebserviceAccessor"/> class.
        /// </summary>
        /// <param name="webserviceUrl">The webservice URL.</param>
        /// <exception cref="System.UriFormatException">throws when webservice url is malformed</exception>
        /// <exception cref="System.ArgumentNullException">throws when webservice url is null</exception>
        public WebserviceAccessor(string webserviceUrl)
        {
            Uri uri = new Uri(webserviceUrl, UriKind.Absolute);
            m_webserviceUrl = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebserviceAccessor"/> class.
        /// </summary>
        /// <param name="webserviceUrl">The webservice URL.</param>
        /// <param name="acceptAllSSLCertifications">if set to <c>true</c> [accept all SSL certifications].</param>
        /// <exception cref="System.UriFormatException">throws when webservice url is malformed</exception>
        /// <exception cref="System.ArgumentNullException">throws when webservice url is null</exception>
        public WebserviceAccessor(string webserviceUrl, bool acceptAllSSLCertifications) : this(webserviceUrl)
        {
            m_acceptAllSSLCertifications = acceptAllSSLCertifications;
        }

        #endregion

        #region Properties

        public Uri WebserviceUrl
        {
            get { return m_webserviceUrl; }
        }

        #endregion

        #region Public WebService Method Calls

        /// <summary>
        /// The method authenticates the specified credentials with webservice.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <exception cref="System.Net.WebException">throws exception if connection could not be established, or ssl certificate was not valid</exception>
        /// <returns>The authentication response. If authentication is successful the reponse includes the ticket that is required for some other calls to service</returns>
        public void Authenticate(Credentials credentials, Callback<AuthenticationResponse> callback)
        {
            //serialize credentials object to json format
            string jsonCredentials = JsonSerializer.Serialize<Credentials>(credentials);

            //call service and retrieve response json
            CallServiceAsync(TraceLabServiceCommands.Authenticate, jsonCredentials, callback);
        }

        /// <summary>
        /// Publishes the contest. 
        /// </summary>
        /// <param name="ticket">The ticket acquired during authentication</param>
        /// <param name="username">The username - whose ticket is provided</param>
        /// <param name="contest">The contest - the actual data to published</param>
        /// <exception cref="System.Net.WebException">throws exception if connection could not be established, or ssl certificate was not valid</exception>
        /// <returns>the response whether publishing was successful</returns>
        public void PublishContest(string ticket, string username, Contest contest, Callback<ContestPublishedResponse> callback)
        {
            //set authentication ticket
            Envelope envelope = new Envelope(ticket, username, contest);
            
            //serialize contest object to json format; requires adding the Contest, ContestResults, and all metric types as known types for the serialize method
            List<Type> knownTypes = new List<Type>(MetricTypesManager.GetMetricTypes);
            knownTypes.Add(typeof(Contest));
            knownTypes.Add(typeof(ContestResults));

            string jsonEnvelope = JsonSerializer.Serialize<Envelope>(envelope, knownTypes);

            //call service and retrieve response json
            CallServiceAsync(TraceLabServiceCommands.PublishContest, jsonEnvelope, callback);
        }

        /// <summary>
        /// Retrieves the list of contests from the webservice.
        /// </summary>
        /// <returns></returns>
        public void RetrieveListOfContests(Callback<ListOfContestsResponse> callback)
        {
            CallServiceAsync(TraceLabServiceCommands.RetrieveListOfContests, null, callback);
        }

        /// <summary>
        /// Downloads the contest package
        /// </summary>
        /// <param name="contestGuid">The contest GUID.</param>
        /// <returns>contest package encoded in base64</returns>
        public void DownloadContestPackage(string contestGuid, Callback<ContestPackageResponse> callback)
        {
            //serialize contest guid to json format;
            string jsonContestGuid = JsonSerializer.Serialize<string>(contestGuid);

            CallServiceAsync(TraceLabServiceCommands.DownloadContestPackage, jsonContestGuid, callback);
        }

        /// <summary>
        /// Publishes the contest results.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="username">The username.</param>
        /// <param name="contestResult">The contest result.</param>
        /// <param name="response">The response.</param>
        public void PublishContestResults(string ticket, string username, ContestResults contestResult, Callback<ContestResultsPublishedResponse> callback)
        {
            //set authentication ticket
            Envelope envelope = new Envelope(ticket, username, contestResult);

            //serialize contest result object to json format; requires adding the Contest result and all available metric types, as known type for the serialize method
            List<Type> knownTypes = new List<Type>(MetricTypesManager.GetMetricTypes);
            knownTypes.Add(typeof(ContestResults));

            string jsonEnvelope = JsonSerializer.Serialize<Envelope>(envelope, knownTypes);

            //call service and retrieve response json
            CallServiceAsync(TraceLabServiceCommands.PublishContestResults, jsonEnvelope, callback);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calls the webservice asynchronously
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="jsonArgs">The json object to be passed to the request stream</param>
        /// <exception cref="System.Net.WebException">throws exception if connection could not be established, or ssl certificate was not valid</exception>
        /// <returns>the response stream from webservice</returns>
        private void CallServiceAsync<T>(TraceLabServiceCommands command, string jsonArgs, Callback<T> callback) where T : Response, new()
        {
            if (m_acceptAllSSLCertifications)
            {
                //accept all ssl certificates, including invalid ssl certs
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            }

            //create web request
            WebClient webClient = new WebClient();
            webClient.UploadProgressChanged += UploadProgressChangedHandler<T>;
            webClient.UploadStringCompleted += UploadCompleted<T>;
            
            Uri requestUri = new Uri(m_webserviceUrl, "?command=" + command.ToString());

            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            if (jsonArgs == null) jsonArgs = String.Empty; //if args are null set them to empty string

            //if progress in the callback is not null set its maximum step to number of bytes that are uploaded
            if (callback.Progress != null)
            {
                callback.Progress.NumSteps = jsonArgs.Length;
                if (command.Equals(TraceLabServiceCommands.DownloadContestPackage) || command.Equals(TraceLabServiceCommands.RetrieveListOfContests))
                {
                    callback.Progress.CurrentStatus = "Downloading...";
                }
                else
                {
                    callback.Progress.CurrentStatus = "Uploading...";
                }
            }

            //start upload
            System.Diagnostics.Debug.WriteLine("Start...");
            webClient.UploadStringAsync(requestUri, "POST", jsonArgs, callback);
        }

        /// <summary>
        /// Handles the upload progress changed event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Net.UploadProgressChangedEventArgs"/> instance containing the event data.</param>
        private void UploadProgressChangedHandler<T>(object sender, UploadProgressChangedEventArgs e) where T : Response, new()
        {
            System.Diagnostics.Debug.WriteLine("Uploaded {0} of {1} bytes. {2} % complete...", e.BytesSent, e.TotalBytesToSend, e.ProgressPercentage);
            Callback<T> callback = (Callback<T>)e.UserState;
            if (callback.Progress != null)
            {
                if (callback.Progress.CurrentStep != e.BytesSent)
                {
                    callback.Progress.CurrentStep = e.BytesSent;
                }
            }
        }

        /// <summary>
        /// Event handler executed when uploads has been completed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Net.UploadStringCompletedEventArgs"/> instance containing the event data.</param>
        private void UploadCompleted<T>(object sender, UploadStringCompletedEventArgs e) where T : Response, new()
        {
            Callback<T> callback = (Callback<T>)e.UserState;
            T response;

            if (e.Cancelled == true)
            {
                response = UploadCancelled<T>(callback);
            }
            else if (e.Error != null)
            {
                response = UploadError<T>(e, callback);
            }
            else
            {
                response = UploadSuccessful<T>(e, callback);
            }

            callback.FireCallCompleted(new CallCompletedEventArgs<T>(response));
        }

        /// <summary>
        /// Executed if upload was successful
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The <see cref="System.Net.UploadStringCompletedEventArgs"/> instance containing the event data.</param>
        /// <param name="callback">The callback.</param>
        private T UploadSuccessful<T>(UploadStringCompletedEventArgs e, Callback<T> callback) where T : Response, new()
        {
            T response;

            string responseText = e.Result;

            //clear potential non-utf characters from the string
            responseText = System.Text.RegularExpressions.Regex.Replace(responseText, @"[^\u0000-\u007F]", "");

            //deserialize response json...
            try
            {
                response = JsonSerializer.Deserialize<T>(responseText);

                if (callback.Progress != null)
                {
                    if (response.Status.Equals(ResponseStatus.STATUS_SUCCESS))
                    {
                        callback.Progress.CurrentStatus = "Completed!";
                    }
                    else
                    {
                        callback.Progress.Reset();
                        callback.Progress.CurrentStatus = "Error!";
                        callback.Progress.SetError(true);
                    }
                }

            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                //upload was not successful
                if (callback.Progress != null)
                {
                    callback.Progress.Reset();
                    callback.Progress.CurrentStatus = "Error!";
                    callback.Progress.SetError(true);
                }
                response = new T();
                response.ErrorMessage = ex.Message;
                response.Status = ResponseStatus.STATUS_FAILURE;
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                //upload was not successful
                if (callback.Progress != null)
                {
                    callback.Progress.Reset();
                    callback.Progress.CurrentStatus = "Error!";
                    callback.Progress.SetError(true);
                }
                response = new T();
                response.ErrorMessage = ex.Message;
                response.Status = ResponseStatus.STATUS_FAILURE;
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return response;
        }

        /// <summary>
        /// Executed if upload had error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The <see cref="System.Net.UploadStringCompletedEventArgs"/> instance containing the event data.</param>
        /// <param name="callback">The callback.</param>
        private static T UploadError<T>(UploadStringCompletedEventArgs e, Callback<T> callback) where T : Response, new()
        {
            System.Diagnostics.Debug.WriteLine("Error! ");
            System.Diagnostics.Debug.WriteLine(e.Error.StackTrace);
            //upload was not successful
            if (callback.Progress != null)
            {
                callback.Progress.Reset();
                callback.Progress.CurrentStatus = "Error!";
                callback.Progress.SetError(true);
            }
            T response = new T();
            response.ErrorMessage = e.Error.Message;
            response.Status = ResponseStatus.STATUS_FAILURE;

            return response;
        }

        /// <summary>
        /// Executed if upload was cancelled
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback">The callback.</param>
        private static T UploadCancelled<T>(Callback<T> callback) where T : Response, new()
        {
            //upload has been cancelled
            System.Diagnostics.Debug.WriteLine("Call has been cancelled! ");
            //upload was not successful
            if (callback.Progress != null)
            {
                callback.Progress.Reset();
                callback.Progress.CurrentStatus = "Cancelled!";
                callback.Progress.SetError(true);
            }
            T response = new T();
            response.ErrorMessage = "Call has been cancelled!";
            response.Status = ResponseStatus.STATUS_FAILURE;

            return response;
        }

        /// <summary>
        /// This method accepts all ssl certificates, including invalid ones.
        /// Normally the method would verify the remote Secure Sockets Layer (SSL) certificate used for authentication. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certification">The certification.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns></returns>
        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #region Helper Methods

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        private string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        #endregion

        #endregion
    }
}

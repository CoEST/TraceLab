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
using TraceLab.Core;
using TraceLab.Core.WebserviceAccess;
using System.Windows.Input;
using TraceLab.UI.WPF.Commands;
using System.ComponentModel;
using TraceLabSDK;

namespace TraceLab.UI.WPF.ViewModels
{
    /// <summary>
    /// Serves as view model for authentication and upload control
    /// </summary>
    /// <typeparam name="T">The parameter of response used in upload handler - to serve both Contest and ContestResults upload</typeparam>
    class AuthenticationAndUploadViewModel<T> : INotifyPropertyChanged where T : Response, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationAndUploadViewModel&lt;T&gt;"/> class.
        /// THe authentication view model is used for the AuthenticationAndUpload Form.
        /// </summary>
        /// <param name="webService">The webservice accessor object that is used to access the webservice</param>
        /// <param name="uploadHandler">The upload handler, the method to be executed when authentication is completed</param>
        /// <param name="uploadingMessage">The uploading message - the info message that is shown during the uploading process</param>
        /// <param name="uploadCompletedMessage">The upload completed message - the info message that is shown when upload is completed successfully</param>
        public AuthenticationAndUploadViewModel(WebserviceAccessor webService, UploadHandler<T> uploadHandler, string uploadingMessage, string uploadCompletedMessage)
        {
            if (uploadHandler == null)
                throw new ArgumentNullException("uploadHandler");

            m_currentState = AuthenticationState.Authentication;
            Authenticate = new DelegateCommand(ExecuteAuthenticate, CanExecuteAuthenticate);
            m_webService = webService;

            if (m_webService != null)
            {
                //extract joomla root address from webserviceAddress, assuming it is TraceLab jooomla component
                if (m_webService.WebserviceUrl.AbsoluteUri.Contains("administrator"))
                {
                    /// Gets the webservice joomla base address. Needed for hyperlinks to create user accounts, forgot password, and username
                    string webserviceJoomlaBaseAddress = m_webService.WebserviceUrl.AbsoluteUri.Substring(0, m_webService.WebserviceUrl.AbsoluteUri.IndexOf("administrator"));
                    CreateAccountUrl = new Uri(webserviceJoomlaBaseAddress + JoomlaLinks.CreateAccountUrl);
                    RemindUserUrl = new Uri(webserviceJoomlaBaseAddress + JoomlaLinks.RemindUserUrl);
                    ResetPasswordUrl = new Uri(webserviceJoomlaBaseAddress + JoomlaLinks.ResetPasswordUrl);
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn("The webaddress is not a standard joomla com_jtracelab webservice address, thus the links to website for creating account, retrieving password won't work.");
                }
            }

            m_uploadHandler = uploadHandler;

            m_uploadingMessage = uploadingMessage;
            m_uploadCompletedMessage = uploadCompletedMessage;
        }

        private UploadHandler<T> m_uploadHandler;

        private string m_uploadingMessage;

        private string m_uploadCompletedMessage;

        private WebserviceAccessor m_webService;
        private WebserviceAccessor WebService
        {
            get
            {
                return m_webService;
            }
        }

        #region Authenticate and upload contest

        public ICommand Authenticate
        {
            get;
            private set;
        }

        /// <summary>
        /// Attempts to authenticate user based on provided credentials. 
        /// If authentication is successful the method proceed to uploading the contest.
        /// Otherwise it sets the error message based on response from the webservice.
        /// </summary>
        /// <param name="param">not used</param>
        private void ExecuteAuthenticate(object param)
        {
            //clear error message if there is any
            if (String.IsNullOrEmpty(ErrorMessage) == false) ErrorMessage = String.Empty;

            var progress = param as TraceLabSDK.IProgress;
            if (progress != null) m_progressBar = progress;

            try
            {
                //Authenticate 
                var authenticationCallback = new Callback<AuthenticationResponse>();
                authenticationCallback.CallCompleted += OnAuthenticationCompleted;
                WebService.Authenticate(m_credentials, authenticationCallback);
            }
            catch (ArgumentNullException)
            {
                //although UI should prevent user trying to authenticate if address is malformed, catch these exception just in case
                ErrorMessage = Messages.WebserviceUrlEmptyError;
            }
            catch (UriFormatException)
            {
                //although UI should prevent user trying to authenticate if address is malformed, catch these exception just in case
                ErrorMessage = Messages.WebserviceUrlMalformedError;
            }
            catch (Exception ex)
            {
                ErrorMessage = String.Format(Messages.WebserviceAccessError, ex.Message);
            }
        }

        private TraceLabSDK.IProgress m_progressBar;

        private void OnAuthenticationCompleted(object sender, CallCompletedEventArgs<AuthenticationResponse> responseArgs)
        {
            if (responseArgs.Response.IsAuthenticated == true)
            {
                CurrentState = AuthenticationState.Upload;
             
                InfoMessage = m_uploadingMessage;

                var publishingContest = new Callback<T>();
                publishingContest.CallCompleted += PublishCompleted;
                publishingContest.Progress = m_progressBar;
                
                //call the upload handler
                try
                {
                    m_uploadHandler(responseArgs.Response.Ticket, Username, publishingContest);
                }
                catch (InvalidOperationException ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                //show error
                ErrorMessage = responseArgs.Response.ErrorMessage;
            }
        }

        /// <summary>
        /// Executed when contest publishing has been completed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PublishCompleted(object sender, CallCompletedEventArgs<T> responseArgs)
        {
            if (responseArgs.Response.Status.Equals(ResponseStatus.STATUS_SUCCESS))
            {
                //show some message to user
                InfoMessage = m_uploadCompletedMessage;

                IResponseWithContestLink contestPublishedResponse = responseArgs.Response as IResponseWithContestLink;
                if (contestPublishedResponse != null)
                {
                    if (contestPublishedResponse.ContestWebsiteLink != null)
                        ContestWebsite = new Uri(contestPublishedResponse.ContestWebsiteLink);
                }
            }
            else
            {
                //show error message
                InfoMessage = Messages.PublishFailed;
                ErrorMessage = responseArgs.Response.ErrorMessage;
            }
        }

        /// <summary>
        /// Determines whether authentication can be executed
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if username and password are not empty, otherwise <c>false</c>.
        /// </returns>
        private bool CanExecuteAuthenticate(object param)
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
            {
                return false;
            }

            return true;
        }

        #endregion

        private Credentials m_credentials = new Credentials();
        /// <summary>
        /// Gets or sets the username required for authentication to the webservice
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get
            {
                return m_credentials.Username;
            }
            set
            {
                if (m_credentials.Username != value)
                {
                    m_credentials.Username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }

        /// <summary>
        /// Gets or sets the password required for authentication to the webservice
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                return m_credentials.Password;
            }
            set
            {
                if (m_credentials.Password != value)
                {
                    m_credentials.Password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        private string m_errorMessage;

        /// <summary>
        /// Gets or sets the error message, that can be displayed by several screens - for example, when authentication failed
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage
        {
            get
            {
                return m_errorMessage;
            }
            set
            {
                if (m_errorMessage != value)
                {
                    m_errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        private string m_infoMessage;

        /// <summary>
        /// Gets or sets the info message, that can be displayed by several screens - for example, in uploading window
        /// </summary>
        /// <value>
        /// The info message.
        /// </value>
        public string InfoMessage
        {
            get
            {
                return m_infoMessage;
            }
            set
            {
                if (m_infoMessage != value)
                {
                    m_infoMessage = value;
                    NotifyPropertyChanged("InfoMessage");
                }
            }
        }

        private Uri m_contestWebsite;

        /// <summary>
        /// Gets or sets the contest website url, so that user can go to contest website directly from within TraceLab
        /// </summary>
        /// <value>
        /// The info message.
        /// </value>
        public Uri ContestWebsite
        {
            get
            {
                return m_contestWebsite;
            }
            set
            {
                if (m_contestWebsite != value)
                {
                    m_contestWebsite = value;
                    NotifyPropertyChanged("ContestWebsite");
                }
            }
        }

        private AuthenticationState m_currentState;
        /// <summary>
        /// Gets the state of the current of the form. 
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public AuthenticationState CurrentState
        {
            get
            {
                return m_currentState;
            }
            private set
            {
                if (m_currentState != value)
                {
                    m_currentState = value;
                    NotifyPropertyChanged("CurrentState");
                }
            }
        }

        /// <summary>
        /// Gets the create account URL.
        /// </summary>
        public Uri CreateAccountUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the reset password URL.
        /// </summary>
        public Uri ResetPasswordUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the remind user URL.
        /// </summary>
        public Uri RemindUserUrl
        {
            get;
            private set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }

    public enum AuthenticationState {
        Authentication,
        Upload
    }

    /// <summary>
    /// The upload handler allows specyfying what method should be executed once authentication is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ticket">The ticket - provided from successful authentication.</param>
    /// <param name="username">The username - provided in successful authentication.</param>
    /// <param name="callback">The callback - the webservice callback object with results of the publishing</param>
    public delegate void UploadHandler<T>(string ticket, string username, Callback<T> callback) where T : Response, new();
}

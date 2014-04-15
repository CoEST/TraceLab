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
using System.Net;
using System.IO;
using System.Threading;

namespace TraceLab.Core.Test.WebserviceAccess
{
    class MockWebRequest : WebRequest
    {
        private MemoryStream m_requestStream;
        private MockWebResponse m_mockResponse;

        /// <summary>Initializes a new instance of <see cref="MockWebRequest"/>
        /// with the response to return.</summary>
        public MockWebRequest(Uri uri, string response)
        {
            m_requestUri = uri;
            m_requestStream = new MemoryStream();
            Stream m_responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response));
            m_mockResponse = new MockWebResponse(m_responseStream);
        }

        #region Required overrides

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            var mockResult = new MockAsyncResult(callback, state);
            mockResult.SetCompleted(false, m_requestStream);
            return mockResult;
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return m_requestStream;
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            var mockResult = new MockAsyncResult(callback, state);
            mockResult.SetCompleted(false, m_mockResponse);
            return mockResult;
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return m_mockResponse;
        }

        public override string Method { get; set; }

        private Uri m_requestUri;
        public override Uri RequestUri {
            get
            {
                return m_requestUri;
            }
         }

        public override WebHeaderCollection Headers { get; set; }

        public override long ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override ICredentials Credentials { get; set; }

        public override bool PreAuthenticate { get; set; }

        public override Stream GetRequestStream()
        {
            return m_requestStream;
        }

        public override WebResponse GetResponse()
        {
            return m_mockResponse;
        }

        #endregion
    }
}

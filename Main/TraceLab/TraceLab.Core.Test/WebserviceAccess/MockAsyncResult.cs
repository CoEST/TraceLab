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
using System.Threading;
using System.IO;

namespace TraceLab.Core.Test.WebserviceAccess
{
    internal class MockAsyncResult : IAsyncResult, IDisposable
    {
        private readonly AsyncCallback m_callback;
        private bool m_completed;
        private bool m_completedSynchronously;
        private object m_asyncState;
        private readonly ManualResetEvent m_waitHandle;
        private MockWebResponse m_result;
        private readonly object m_locker;
        private Stream m_writeStream;

        internal MockAsyncResult(AsyncCallback cb, object state)
            : this(cb, state, false)
        {
        }

        internal MockAsyncResult(AsyncCallback cb, object state,
            bool completed)
        {
            m_callback = cb;
            m_asyncState = state;
            m_completed = completed;
            m_completedSynchronously = completed;

            m_waitHandle = new ManualResetEvent(false);
            m_locker = new object();
        }

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return m_asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return m_waitHandle; }
        }

        public bool CompletedSynchronously
        {
            get
            {
                lock (m_locker)
                {
                    return m_completedSynchronously;
                }
            }
        }

        public bool IsCompleted
        {
            get
            {
                lock (m_locker)
                {
                    return m_completed;
                }
            }
        }

        #endregion

        
        internal void SetCompleted(bool completedSynchronously, MockWebResponse result)
        {
            lock (m_locker)
            {
                m_completed = true;
                m_completedSynchronously = completedSynchronously;
                m_result = result;
            }

            SignalCompletion();
        }

        internal void SetCompleted(bool completedSynchronously, Stream writeStream)
        {
            lock (m_locker)
            {
                m_completed = true;
                m_completedSynchronously = completedSynchronously;
                m_writeStream = writeStream;
            }

            SignalCompletion();
        }
        
        private void SignalCompletion()
        {
            m_waitHandle.Set();

            ThreadPool.QueueUserWorkItem(new WaitCallback(InvokeCallback));
        }

        /// <summary>
        /// Invokes the callback.
        /// </summary>
        /// <param name="state">The state.</param>
        private void InvokeCallback(object state)
        {
            if (m_callback != null)
            {
                m_callback(this);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (m_locker)
                {
                    if (m_waitHandle != null)
                    {
                        ((IDisposable)m_waitHandle).Dispose();
                    }
                }
            }
        }

        #endregion
    }
}

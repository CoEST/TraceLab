using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLabSDK;

namespace TraceLabWeb
{
    public class ExperimentProgress : IProgress
    {
        private bool m_isIndeterminate;
        public bool IsIndeterminate
        {
            get
            {
                return m_isIndeterminate;
            }
            set
            {
                m_isIndeterminate = value;
            }
        }
        private string log;

        public string GetLog
        {
            get
            {
                return log;
            }
            set
            {
                log = value;
            }
        }        

        private double m_numSteps;
        public double NumSteps
        {
            get
            {
                return m_numSteps;
            }
            set
            {
                m_numSteps = value;
            }
        }

        private double m_currentStep;
        public double CurrentStep
        {
            get
            {
                return m_currentStep;
            }
            set
            {
                m_currentStep = value;
            }
        }

        private string m_currentStatus;
        public string CurrentStatus
        {
            get
            {
                return m_currentStatus;
            }
            set
            {
                if (m_currentStatus != value)
                {
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        // NLog.LogManager.GetCurrentClassLogger().Info();
                        log += String.Format("Current Status: {0}", value);
                    }
                    m_currentStatus = value;
                }
            }
        }

        public void Reset()
        {
            CurrentStep = 0;
            IsIndeterminate = false;
            CurrentStatus = string.Empty;
            GetLog = "";
        }

        public void Increment()
        {
            CurrentStep += 1;
        }

        private bool m_hasError;
        public void SetError(bool hasError)
        {
            m_hasError = hasError;
        }

        public bool HasError
        {
            get
            {
                return m_hasError;
            }
        }
    }
}

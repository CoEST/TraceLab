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
using TraceLabSDK;

namespace TraceLabConsole
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
                        NLog.LogManager.GetCurrentClassLogger().Info(String.Format("Current Status: {0}", value));
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
        }

        public void Increment()
        {
            CurrentStep +=1 ;
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

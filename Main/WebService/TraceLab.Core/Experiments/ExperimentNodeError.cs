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


namespace TraceLab.Core.Experiments
{
    public class ExperimentNodeError
    {
        public ExperimentNodeError(string errorMessage)
        {
            m_errorMessage = errorMessage;
            m_errorType = ExperimentNodeErrorType.DEFAULT;
        }

        public ExperimentNodeError(string errorMessage, ExperimentNodeErrorType errorType)
        {
            m_errorMessage = errorMessage;
            m_errorType = errorType;
        }

        private readonly string m_errorMessage = null;
        public string ErrorMessage
        {
            get
            {
                return m_errorMessage;
            }
        }

        private readonly ExperimentNodeErrorType m_errorType;
        public ExperimentNodeErrorType ErrorType
        {
            get
            {
                return m_errorType;
            }
        }

        public override bool Equals(object obj)
        {
            ExperimentNodeError error = obj as ExperimentNodeError;
            if (error != null)
            {
                if (ErrorType.Equals(error.ErrorType))
                {
                    return string.Equals(ErrorMessage, error.ErrorMessage);
                }

                return false;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int errorHash = ErrorType.GetHashCode();
            if (ErrorMessage != null)
            {
                errorHash ^= ErrorMessage.GetHashCode();
            }

            return errorHash;
        }
    }
}

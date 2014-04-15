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
using System.Runtime.Serialization;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// The exception thrown by DecisionCodeParser if code has incorrect format
    /// </summary>
    [Serializable]
    public class DecisionCodeParserException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionCodeParserException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected DecisionCodeParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DecisionCodeParserException"/> class from being created.
        /// </summary>
        private DecisionCodeParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionCodeParserException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DecisionCodeParserException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionCodeParserException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DecisionCodeParserException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

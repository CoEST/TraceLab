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

namespace TraceLabSDK
{
    /// <summary>
    /// Represents the interface to a user control depicting the progress of an operation.
    /// </summary>
    public interface IProgress
    {
        /// <summary>
        /// Gets or sets a value indicating whether the progress of whatever is processing is indeterminate.
        /// 
        /// Eg. The number of steps cannot be determined, or the current step cannot be determined.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the process' progress is indeterminate; otherwise, <c>false</c>.
        /// </value>
        bool IsIndeterminate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of steps that must be completed.
        /// </summary>
        /// <value>
        /// The num steps.
        /// </value>
        double NumSteps
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current step.
        /// </summary>
        /// <value>
        /// The current step.
        /// </value>
        double CurrentStep
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        /// <remarks>
        /// This is usually a description of what the current step is doing.
        /// </remarks>
        string CurrentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Resets the current progress.
        /// </summary>
        void Reset();

        /// <summary>
        /// Increments the current progress to the next step.
        /// </summary>
        void Increment();

        /// <summary>
        /// Whether the Progress implementation should change it's display to display an error
        /// </summary>
        /// <param name="hasError">if set to <c>true</c> then an error has occurred in whatever is/was processing.</param>
        void SetError(bool hasError);
    }
}

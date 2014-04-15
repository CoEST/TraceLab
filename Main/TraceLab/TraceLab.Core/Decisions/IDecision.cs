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

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents metadata that is compilable with user provided code
    /// </summary>
    public interface IDecision : ILoggable
    {
        /// <summary>
        /// Gets or sets the decision code - the code provided by the user and the code that is saved in experiment file
        /// </summary>
        /// <value>
        /// The decision code.
        /// </value>
        string DecisionCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether code is dirty, ie. was modified
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is code dirty; otherwise, <c>false</c>.
        /// </value>
        bool IsCodeDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the compilation status. 
        /// </summary>
        /// <value>
        /// The compilation status.
        /// </value>
        CompilationStatus CompilationStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when request latest code event has been fired
        /// </summary>
        event EventHandler RequestLatestCode;
        
        /// <summary>
        /// Fires the request latest code event
        /// </summary>
        void FireRequestLatestCode();

        /// <summary>
        /// Gets the unique decision ID.
        /// </summary>
        string UniqueDecisionID
        {
            get;
        }

        /// <summary>
        /// Gets or sets the assembly.
        /// It is temporary dynamic assembly into which decision module is compiled to.
        /// It is used by both DecisionModuleCompilationRunner and DecisionLoader
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        new string SourceAssembly
        {
            get;
        }

        /// <summary>
        /// Gets or sets the classname.
        /// It is used by both DecisionModuleCompilationRunner and DecisionLoader
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        new string Classname
        {
            get;
        }
    }
}

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
using TraceLab.Core.Settings;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// ILoggable interface defines metadata that is loggable by nlog using logger factory.
    /// Classname, source assembly and label allows logging information with location of the error. 
    /// It is user friendly way of displaying the information. It is useful in particular for displaying exceptions.
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Gets the classname to display in the log message. The classname can be displayed in the log information, especially when exception occurs.
        /// </summary>
        string Classname
        {
            get;
        }

        /// <summary>
        /// Gets the source assembly to display in the log message. The classname can be displayed in the log information, especially when exception occurs.
        /// </summary>
        string SourceAssembly
        {
            get;
        }

        /// <summary>
        /// Gets the label to display in the log message. The classname can be displayed in the log information, especially when exception occurs.
        /// </summary>
        string Label
        {
            get;
        }

        /// <summary>
        /// Gets the log levels.
        /// </summary>
        IEnumerable<LogLevelItem> LogLevels
        {
            get;
        }
    }
}

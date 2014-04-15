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
using System.Collections.ObjectModel;

namespace TraceLabSDK
{
    /// <summary>
    /// An interface for communication with the Workspace.
    /// </summary>
    public interface IWorkspace
    {
        /// <summary>
        /// Tests whether a unit currently exists in the Workspace.
        /// </summary>
        /// <param name="unitname">The name of the unit</param>
        /// <returns>True if the unit exists in the Workspace, otherwise false.</returns>
        bool Exists(string unitname);

        /// <summary>
        /// Loads a copy of a unit from the Workspace
        /// </summary>
        /// <param name="unitname">The name of the unit</param>
        /// <returns>The unit if it exists, otherwise null</returns>
        /// <remarks>
        /// It is important to realize that because this is loading a copy, in order to save any
        /// changes, you will have to make your changes and then call Store()
        /// </remarks>
        object Load(string unitname);

        /// <summary>
        /// Stores a unit into the Workspace
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unit">The object to store as the unit.</param>
        void Store(string unitname, object unit);

        /// <summary>
        /// Gets the time stamp of a given unit.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns></returns>
        DateTime GetDateTimeStamp(string unitname);
    }
}

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
using TraceLabSDK;
using System.Collections.Generic;

namespace TraceLab.Core.Workspaces
{
    internal interface IWorkspaceInternal : IWorkspace, IDisposable
    {
        /// <summary>
        /// Deletes the specified unit name.
        /// </summary>
        /// <param name="unitName">Name of the unit.</param>
        void Delete(string unitName);

        /// <summary>
        /// Gets or sets the workspace namespace id.
        /// </summary>
        /// <value>
        /// The namespace id.
        /// </value>
        string NamespaceId { get; }

        /// <summary>
        /// Gets the directory where the workspace type assemblies are located.
        /// </summary>
        List<string> TypeDirectories { get; }

        /// <summary>
        /// Loads the byte representation of the workspace unit data.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns>The full byte representation of the workspace unit data</returns>
        byte[] LoadBytes(string unitname);

        /// <summary>
        /// Stores a unit bytes into the Workspace
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unitType">Type of the unit, that is stored. It later allows accessing workspace unit type without deserilizing data each time.</param>
        /// <param name="unitBytes">The unit bytes to be stored.</param>
        void StoreBytes(string unitname, Type unitType, byte[] unitBytes);

        /// <summary>
        /// Renames the unit.
        /// </summary>
        /// <param name="oldUnitname">The old unitname.</param>
        /// <param name="newUnitname">The new unitname.</param>
        void RenameUnit(string oldUnitname, string newUnitname);

        /// <summary>
        /// Copies the unit.
        /// </summary>
        /// <param name="fromUnitname">From unitname.</param>
        /// <param name="toUnitname">To unitname.</param>
        void CopyUnit(string fromUnitname, string toUnitname);

        /// <summary>
        /// Deletes the units that has name starting with given namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">namespace of units to be deleted.</param>
        void DeleteUnits(string namespaceId);

        /// <summary>
        /// Gets the list of units short names, which name starts with the given namespace prefixed with local namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">The namespace id.</param>
        /// <returns>
        /// Enumarable of units which real name starts with the given namespace
        /// </returns>
        IEnumerable<string> GetUnits(string namespaceId);
    }
}

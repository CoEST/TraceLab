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
using TraceLab.Core.Components;

namespace TraceLab.Core.Workspaces
{
    /// <summary>
    /// The class is a workspace wrapper for the scope. It differs from standard nested workspace wrapper 
    /// in terms of setup and teardown methods. In the scope all units from the workspace of outer graph or scope should be copied to inner scope,
    /// unlike in composite component nested workspace, where only units specified in IOSpec should be copied into inner graph workspace.
    /// </summary>
    [Serializable]
    internal class ScopeNestedWorkspaceWrapper : NestedWorkspaceWrapper
    {
        public ScopeNestedWorkspaceWrapper(IWorkspaceInternal parentWorkspace, string workspaceNamespaceId)
            : base(parentWorkspace, workspaceNamespaceId)
        {
        }

        /// <summary>
        /// Collection keeps unitname of units that has been stored by this workspace.
        /// Only units that has been stored within the workspace are moved to upper parent workspace in teardown phase.
        /// </summary>
        private HashSet<string> storedUnitNames = new HashSet<string>();

        public override void Store(string unitname, object unit)
        {
            base.Store(unitname, unit);

            //track outputted/or modified units through this workspace wrapper
            TrackStoredUnitname(unitname);
        }

        /// <summary>
        /// Stores a unit bytes into the Workspace
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unitType">Type of the unit, that is stored. It later allows accessing workspace unit type without deserilizing data each time.</param>
        /// <param name="unitBytes">The unit bytes to be stored.</param>
        public override void StoreBytes(string unitname, Type unitType, byte[] unitBytes)
        {
            base.StoreBytes(unitname, unitType, unitBytes);

            //track outputted/or modified units through this workspace wrapper
            TrackStoredUnitname(unitname);
        }

        /// <summary>
        /// Tracks unitnames that are stored within this workspace.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        public void TrackStoredUnitname(string unitname)
        {
            //only track unitnames stored by basic component within this workspace. Composite components residing
            //in the scope, store units in their workspace, but storing always go via parent scope. 
            //in essence it doesn't track unitnames that are stored by lower level nested workspace of composite component in the scopes.
            if (unitname.Contains(Workspace.NAMESPACE_DELIMITER) == false)
            {
                storedUnitNames.Add(unitname);
            }
        }
        
        /// <summary>
        /// Renames the unit.
        /// </summary>
        /// <param name="oldUnitname">The old unitname.</param>
        /// <param name="newUnitname">The new unitname.</param>
        public override void RenameUnit(string oldUnitname, string newUnitname)
        {
            base.RenameUnit(oldUnitname, newUnitname);

            //track renamed units through this workspace wrapper. In essence, it assures that units outputted by composite components in the scope
            //are also tracked; 
            //because NestedWorkspaceWrapper of composite components moves-renames all units specified in its IOSpec from its workspace wrapper to upper lever parent workspace wrapper.
            TrackStoredUnitname(newUnitname);
        }

        /// <summary>
        /// Setups the nested workspace. In this case, all parent workspace units are copied into this worskspace with local namespace.
        /// </summary>
        internal override void Setup()
        {
            foreach (string unitName in GetParentUnits())
            {
                //note that we call the parent workspace CopyUnit and not the local Rename unit that adds namespace to both old and new name
                m_parentWorkspace.CopyUnit(unitName, GetFullNamespacePath(unitName));
            }
        }

        /// <summary>
        /// Tears down - local units are being moved to parent workspace.
        /// Only units that has been stored withing this workspace are moved, ie. it includes units that has been overwritten.
        /// Other units are discarded.
        /// </summary>
        internal override void TearDown()
        {
            //RENAME OUTPUTS
            foreach (string unitName in GetLocalUnits())
            {
                //if unit has been stored from within the current scope, or previously incoming unit was overwritten within this scope
                //move the unit to parent workspace, otherwise just remove it

                if (storedUnitNames.Contains(unitName))
                {
                    //note that we call the parent workspace RenameUnit and not the local RenameUnit which adds namespace to both old and new name
                    m_parentWorkspace.RenameUnit(GetFullNamespacePath(unitName), unitName);
                }
                else
                {
                    Delete(unitName);
                }
            }
        }

        /// <summary>
        /// Gets the parent units.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetParentUnits()
        {
            //not needed to add local namespace - the parent has its namespace, and constructs full path namespace
            return m_parentWorkspace.GetUnits(String.Empty);
        }

        /// <summary>
        /// Gets local units.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetLocalUnits()
        {
            //not needed to add namespace - GetUnits method already adds local namespace, and constructs full path namespace
            return GetUnits(String.Empty);
        }

        #region Dispose

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="NestedWorkspaceWrapper"/> is reclaimed by garbage collection.
        /// </summary>
        ~ScopeNestedWorkspaceWrapper()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}

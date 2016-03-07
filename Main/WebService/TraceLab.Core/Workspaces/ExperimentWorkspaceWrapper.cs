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

namespace TraceLab.Core.Workspaces
{
    /// <summary>
    /// Experiment Wrapper is the wrapper for the given experiment. It is the wrapper with direct access to the Workspace.
    /// The wrapper takes of namespacing for all components that belong to the current experiment, so that they access only
    /// data withing this experiment.
    /// </summary>
    public class ExperimentWorkspaceWrapper : MarshalByRefObject, IWorkspace, IWorkspaceInternal
    {
        private Workspace m_workspaceInstance;
        private string m_workspaceNamespaceId;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentWorkspaceWrapper"/> class.
        /// </summary>
        /// <param name="workspaceInstance">The workspace instance.</param>
        /// <param name="workspaceNamespaceId">The workspace namespace id.</param>
        internal ExperimentWorkspaceWrapper(Workspace workspaceInstance, string workspaceNamespaceId)
        {
            m_workspaceInstance = workspaceInstance;
            m_workspaceNamespaceId = workspaceNamespaceId;
        }

        ~ExperimentWorkspaceWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Gets or sets the workspace namespace id.
        /// </summary>
        /// <value>
        /// The namespace id.
        /// </value>
        public string NamespaceId
        {
            get
            {
                return m_workspaceNamespaceId;
            }
            private set
            {
                m_workspaceNamespaceId = value;
            }
        }

        /// <summary>
        /// Tests whether a unit with added current namespace currently exists in the Workspace.
        /// </summary>
        /// <param name="unitname">The name of the unit</param>
        /// <returns>
        /// True if the unit exists in the Workspace, otherwise false.
        /// </returns>
        public bool Exists(string unitname)
        {
            // Testing the existance of a unit is fine at any time - ))
            return m_workspaceInstance.Exists(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Composite Workspace Wrapper loads the unitname with the full name with current namespace.
        /// </summary>
        /// <param name="unitname">The name of the variable to be loaded.</param>
        /// <returns>loaded workspace object</returns>
        public object Load(string unitname)
        {
            return m_workspaceInstance.Load(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Composite Workspace Wrapper stores unitname with its fullname that includes namespace 
        /// </summary>
        /// <param name="unitname">The name of the variable to be stored. </param>
        /// <param name="unit">object to be stored in the workspace.</param>
        public void Store(string unitname, object unit)
        {
            m_workspaceInstance.Store(GetFullNamespacePath(unitname), unit);
        }

        /// <summary>
        /// Gets the time stamp of a given unit with added current namespace
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns></returns>
        public DateTime GetDateTimeStamp(string unitname)
        {
            return m_workspaceInstance.GetDateTimeStamp(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Deletes the specified unitname with added current namespace.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        public void Delete(string unitname)
        {
            m_workspaceInstance.Delete(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Adds namespace prefix to the current value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetFullNamespacePath(string value)
        {
            string extendedUnitname = NamespaceId + Workspace.NAMESPACE_DELIMITER + value;
            return extendedUnitname;
        }

        public List<string> TypeDirectories
        {
            get { return m_workspaceInstance.TypeDirectories; }
        }

        /// <summary>
        /// Loads the byte representation of the workspace unit data.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns>
        /// The full byte representation of the workspace unit data
        /// </returns>
        public byte[] LoadBytes(string unitname)
        {
            return m_workspaceInstance.LoadBytes(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Stores a unit bytes into the Workspace
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unitType">Type of the unit, that is stored. It later allows accessing workspace unit type without deserilizing data each time.</param>
        /// <param name="unitBytes">The unit bytes to be stored.</param>
        public void StoreBytes(string unitname, Type unitType, byte[] unitBytes)
        {
            m_workspaceInstance.StoreBytes(GetFullNamespacePath(unitname), unitType, unitBytes);
        }

        /// <summary>
        /// Renames the unit.
        /// </summary>
        /// <param name="oldUnitname">The old unitname.</param>
        /// <param name="newUnitname">The new unitname.</param>
        public void RenameUnit(string oldUnitname, string newUnitname)
        {
            m_workspaceInstance.RenameUnit(GetFullNamespacePath(oldUnitname), GetFullNamespacePath(newUnitname));
        }

        /// <summary>
        /// Deletes the experiment units.
        /// </summary>
        public void DeleteExperimentUnits()
        {
            m_workspaceInstance.DeleteUnits(NamespaceId);
        }

        /// <summary>
        /// Deletes the units that has name starting with given namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">namespace of units to be deleted.</param>
        public void DeleteUnits(string namespaceId)
        {
            m_workspaceInstance.DeleteUnits(GetFullNamespacePath(namespaceId));
        }

        /// <summary>
        /// Copies the unit.
        /// </summary>
        /// <param name="fromUnitname">From unitname.</param>
        /// <param name="toUnitname">To unitname.</param>
        public void CopyUnit(string fromUnitname, string toUnitname)
        {
            m_workspaceInstance.CopyUnit(GetFullNamespacePath(fromUnitname), GetFullNamespacePath(toUnitname));
        }

        /// <summary>
        /// Gets the list of units short names, which name starts with the given namespace prefixed with local namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">The namespace id.</param>
        /// <returns>
        /// Enumarable of units which real name starts with the given namespace
        /// </returns>
        public IEnumerable<string> GetUnits(string namespaceId)
        {
            return m_workspaceInstance.GetUnits(GetFullNamespacePath(namespaceId));
        }
    }
}

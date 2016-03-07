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
using TraceLab.Core.Components;
using TraceLabSDK;
using System.Security.Permissions;

namespace TraceLab.Core.Workspaces
{
    /// <summary>
    /// NestedWorkspaceWrapper is the wrapper for all components workpace accessors in the specific composite graph.
    /// All items loaded and stored are given a guid namespac. 
    /// 
    /// For the component inside composite component the call path to Workspace is following
    /// WorkspaceWrapper -> NestedWorkspaceWrapper -> NestedWorkspaceWrapper -> ... -> Workspace
    /// 
    /// The number of nested wrappers depends on level of nested composite components. 
    /// 
    /// In addition to the ExperimentWorkspaceWrapper the NestedWrapper has additional teardown and setup phase where
    /// data is transfered from and to the composite component namespace. It is simply rename of the stream caches to the 
    /// name defined by parent workspace.
    /// </summary>
    [Serializable]
    internal class NestedWorkspaceWrapper : MarshalByRefObject, IWorkspace, IWorkspaceInternal
    {
        private readonly IOSpec IOSpec;
        protected readonly IWorkspaceInternal m_parentWorkspace;
        private string m_workspaceNamespaceId;

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedWorkspaceWrapper"/> class.
        /// </summary>
        /// <param name="iospec">The iospec.</param>
        /// <param name="parentWorkspace">The parent workspace instance or parent workspace wrapper.</param>
        /// <param name="workspaceNamespaceId">The workspace namespace id.</param>
        public NestedWorkspaceWrapper(IOSpec iospec, IWorkspaceInternal parentWorkspace, string workspaceNamespaceId)
            : this(parentWorkspace, workspaceNamespaceId)
        {
            IOSpec = iospec;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedWorkspaceWrapper"/> class.
        /// </summary>
        /// <param name="parentWorkspace">The parent workspace.</param>
        /// <param name="workspaceNamespaceId">The workspace namespace id.</param>
        protected NestedWorkspaceWrapper(IWorkspaceInternal parentWorkspace, string workspaceNamespaceId)
        {
            m_parentWorkspace = parentWorkspace;
            m_workspaceNamespaceId = workspaceNamespaceId;
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
            return m_parentWorkspace.Exists(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Composite Workspace Wrapper loads the unitname with the full name with current namespace.
        /// </summary>
        /// <param name="unitname">The name of the variable to be loaded.</param>
        /// <returns>loaded workspace object</returns>
        public virtual object Load(string unitname)
        {
            return m_parentWorkspace.Load(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Composite Workspace Wrapper stores unitname with its fullname that includes namespace 
        /// </summary>
        /// <param name="unitname">The name of the variable to be stored. </param>
        /// <param name="unit">object to be stored in the workspace.</param>
        public virtual void Store(string unitname, object unit)
        {
            m_parentWorkspace.Store(GetFullNamespacePath(unitname), unit);
        }

        /// <summary>
        /// Gets the time stamp of a given unit with added current namespace
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns></returns>
        public DateTime GetDateTimeStamp(string unitname)
        {
            return m_parentWorkspace.GetDateTimeStamp(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Deletes the specified unitname with added current namespace.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        public void Delete(string unitname)
        {
            m_parentWorkspace.Delete(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Adds namespace prefix to the current value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string GetFullNamespacePath(string value)
        {
            string extendedUnitname = NamespaceId + Workspace.NAMESPACE_DELIMITER + value;
            return extendedUnitname;
        }

        #region Setup

        internal virtual void Setup()
        {
            foreach (IOItem item in IOSpec.Input.Values)
            {
                if (m_parentWorkspace.Exists(item.MappedTo))
                {
                    //note that we call the parent workspace Rename Unit and not the local Rename unit that adds namespace to both old and new name
                    m_parentWorkspace.CopyUnit(item.MappedTo, GetFullNamespacePath(item.IOItemDefinition.Name));
                }
            }
        }

        #endregion

        #region TearDown

        internal virtual void TearDown()
        {
            renamedWorkspaceUnits.Clear();

            //RENAME OUTPUTS
            RenameIOItems(IOSpec.Output);

            DeleteRemainingUnits();
        }

        private void DeleteRemainingUnits()
        {
            //note that we call the parent workspace with current namespace
            m_parentWorkspace.DeleteUnits(NamespaceId);
        }

        private void RenameIOItems(IDictionary<string, IOItem> ioDictionary)
        {
            foreach (IOItem item in ioDictionary.Values)
            {
                string fromUnitname = item.IOItemDefinition.Name;
                string toUnitname = item.MappedTo;

                if (renamedWorkspaceUnits.Contains(fromUnitname + toUnitname) == false)
                {
                    //note that we call the parent workspace Rename Unit and not the local Rename unit that adds namespace to both old and new name
                    m_parentWorkspace.RenameUnit(GetFullNamespacePath(fromUnitname), toUnitname);
                    renamedWorkspaceUnits.Add(fromUnitname + toUnitname);
                }
            }
        }

        /// <summary>
        /// Keeps the list of already renamed items. Prevents double renaming - in case there are outputs mapped to same name
        /// </summary>
        private HashSet<string> renamedWorkspaceUnits = new HashSet<string>();
        
        #endregion
        
        public List<string> TypeDirectories
        {
            get { return m_parentWorkspace.TypeDirectories; }
        }

        /// <summary>
        /// Loads the byte representation of the workspace unit data.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns>
        /// The full byte representation of the workspace unit data
        /// </returns>
        public virtual byte[] LoadBytes(string unitname)
        {
            return m_parentWorkspace.LoadBytes(GetFullNamespacePath(unitname));
        }

        /// <summary>
        /// Stores a unit bytes into the Workspace
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unitType">Type of the unit, that is stored. It later allows accessing workspace unit type without deserilizing data each time.</param>
        /// <param name="unitBytes">The unit bytes to be stored.</param>
        public virtual void StoreBytes(string unitname, Type unitType, byte[] unitBytes)
        {
            m_parentWorkspace.StoreBytes(GetFullNamespacePath(unitname), unitType, unitBytes);
        }

        /// <summary>
        /// Renames the unit.
        /// </summary>
        /// <param name="oldUnitname">The old unitname.</param>
        /// <param name="newUnitname">The new unitname.</param>
        public virtual void RenameUnit(string oldUnitname, string newUnitname)
        {
            m_parentWorkspace.RenameUnit(GetFullNamespacePath(oldUnitname), GetFullNamespacePath(newUnitname));
        }

        /// <summary>
        /// Copies the unit.
        /// </summary>
        /// <param name="fromUnitname">From unitname.</param>
        /// <param name="toUnitname">To unitname.</param>
        public void CopyUnit(string fromUnitname, string toUnitname)
        {
            m_parentWorkspace.CopyUnit(GetFullNamespacePath(fromUnitname), GetFullNamespacePath(toUnitname));
        }

        /// <summary>
        /// Deletes the units that has name starting with given namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">namespace of units to be deleted.</param>
        public void DeleteUnits(string namespaceId)
        {
            m_parentWorkspace.DeleteUnits(GetFullNamespacePath(namespaceId));
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
            return m_parentWorkspace.GetUnits(GetFullNamespacePath(namespaceId));
        }

        #region Dispose

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="NestedWorkspaceWrapper"/> is reclaimed by garbage collection.
        /// </summary>
        ~NestedWorkspaceWrapper()
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

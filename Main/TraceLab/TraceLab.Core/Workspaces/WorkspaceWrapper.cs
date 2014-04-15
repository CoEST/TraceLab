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
using System.Globalization;
using System.IO;
using TraceLab.Core.Components;
using TraceLab.Core.Exceptions;
using TraceLabSDK;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace TraceLab.Core.Workspaces
{
    /// <summary>
    /// Workspace Wrapper is an workspace accessor/proxy for the component to access Workspace.
    /// 
    /// There are several things to understand about Workspace Wrapper
    /// 
    /// 1. Workspace Wrapper lives in the same app domain as component, while actual workspace lives in the main app domain.
    /// Calling methods such as Load and Store on Workspace causes crossing app domains, and serialization/deserialization any objects
    /// that are passing between app domains. 
    /// 
    /// Note, that methods of Workspace Wrapper has been implemented so the deserialization and serialization from byte array 
    /// happen locally in the wrapper and not in the Workspace itself. 
    /// Workspace method LoadBytes returns directly byte array of workspace unit (without deserializing it in Workspace)
    /// and StoreBytes takes object bytes and directly copies it into workspace unit cache stream. 
    /// 
    /// Thanks to this implementation deserialization/serialization happens only once per each Load and Store. 
    /// Previously during Store object was serialized and deserialized when crossing app domain border, and then serialized into the cache stream in Workspace.
    /// Similarly during Load, object was deserialized in Workspace from cache stream into object, then the object was serialized and deserialized while crossing app domain border.
    /// 
    /// 2. Workspace Wrapper does not have direct access to Workspace. 
    /// For the component in top level experiment the call path to Workspace is following
    /// WorkspaceWrapper -> ExperimentWorkspaceWrapper -> Workspace
    /// 
    /// For the component inside composite component the call path to Workspace is following
    /// WorkspaceWrapper -> NestedWorkspaceWrapper -> NestedWorkspaceWrapper -> ... -> ExperimentWorkspaceWrapper -> Workspace
    /// 
    /// The number of nested wrappers depends on level of nested composite components. 
    /// ExperimentWorkspaceWrapper and NestedWorkspaceWrapper are responsible for loading proper unit from proper experiment and composite graph namespace.
    /// </summary>
    [Serializable]
    internal class WorkspaceWrapper : MarshalByRefObject, IWorkspace, IWorkspaceInternal
    {
        private readonly IOSpec IOSpec;
        private IWorkspaceInternal m_workspace;

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
        /// Initializes a new instance of the <see cref="WorkspaceWrapper"/> class.
        /// </summary>
        /// <param name="iospec">The iospec.</param>
        /// <param name="workspaceInstance">The workspace instance.</param>
        public WorkspaceWrapper(IOSpec iospec, IWorkspaceInternal workspaceInstance)
        {
            IOSpec = iospec;
            m_workspace = workspaceInstance;
        }

        ~WorkspaceWrapper()
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
        /// This is the node workspace wrapper. It does not have namespace. Otherwise TraceLab is going to be broken.
        /// </summary>
        /// <value>
        /// The namespace id.
        /// </value>
        public string NamespaceId
        {
            get
            {
                throw new InvalidOperationException("This is the node workspace wrapper. It does not have namespace.");
            }
        }

        /// <summary>
        /// Tests whether a unit currently exists in the Workspace.
        /// </summary>
        /// <param name="unitname">The name of the unit</param>
        /// <returns>
        /// True if the unit exists in the Workspace, otherwise false.
        /// </returns>
        public bool Exists(string unitname)
        {
            // Testing the existance of a unit is fine at any time - ))
            return m_workspace.Exists(IOSpec.Input[unitname].MappedTo);
        }

        /// <summary>
        /// WorkspaceWrapper load ensures using mapping.
        /// It also checks if component had declared Input of the given unitname, and the same type. If this is not the case, method throws exception.
        /// 
        /// Load is implemented so the deserialization from byte array to the workspace unit data is done locally in the wrapper and not in the Workspace itself. 
        /// 
        /// Thanks to this implementation deserialization happens only once per each Load.
        /// Previously during Load, object was deserialized in Workspace from cache stream into object, then the object was serialized and deserialized while crossing app domain border.
        /// </summary>
        /// <param name="unitname">The name of the variable to be loaded. Notice this is not a mapping value, mapping comes from IOSpec. Unitname had to be declared by the component in definition.</param>
        /// <returns></returns>
        public Object Load(string unitname)
        {
            string mapping;
            if (IOSpec.TryGetInputMapping(unitname, out mapping) == false)
            {
                throw new WorkspaceException(String.Format(CultureInfo.CurrentCulture, "Component has not declared any input of the given name '{0}'", unitname));
            }
            if (String.IsNullOrEmpty(mapping))
            {
                throw new WorkspaceException(String.Format(CultureInfo.CurrentCulture, "'Mapped to' cannot be empty. Please provide the value for the mapping."));
            }

            //return m_workspace.Load(mapping); // not used due to low performance when returning object was crossing app domain boundary

            var bytes = LoadBytes(mapping);

            Object data;
            if (bytes == null)
            {
                //it means that workspace didn't have specific object data, so there is nothing to deserialize
                //simply return null
                data = null;
            }
            else
            {
                WorkspaceUnitData workspaceUnitData = null;

                //otherwise deserialize given bytes
                using (var dataStream = new MemoryStream(bytes))
                {
                    var reader = new BinaryReader(dataStream);

                    try
                    {
                        workspaceUnitData = (WorkspaceUnitData)Activator.CreateInstance(typeof(WorkspaceUnitData), true);
                        workspaceUnitData.ReadData(reader);
                    }
                    catch
                    {
                        // If deserialization failed, the cache is
                        // corrupt or invalid - attempt to delete and clear the cache
                        Delete(mapping);
                        workspaceUnitData = null;
                    }
                }

                if (workspaceUnitData == null)
                {
                    throw new WorkspaceException(String.Format("Loading {0} data failed.", mapping));
                }

                data = workspaceUnitData.Data;
            }

            return data;
        }

        /// <summary>
        /// WorkspaceWrapper store method ensures using the Outpus As parameter, instead of output name directly. 
        /// It also checks if component had declared Output of the given unitname, and checks if the provided object type matches the type declared in IOSpec.
        /// 
        /// Store id implemented so that the serialization of the object to stream and then array
        /// happen locally in the wrapper and not in the Workspace itself. 
        /// 
        /// Thanks to this implementation serialization happens only once per each Store. 
        /// Previously during Store object was serialized and deserialized when crossing app domain border, and then serialized into the cache stream in Workspace.
        /// </summary>
        /// <param name="unitname">The name of the variable to be stored. Notice this is going to be stored with a name specified in IOSpec by 'Output As' parameter. Unitname had to be declared by the component in definition.</param>
        /// <param name="unit">Type of the variable. It has to match the type declared in the definition.</param>
        public void Store(string unitname, object unit)
        {
            if (unit == null)
            {
                throw new WorkspaceException(String.Format(CultureInfo.CurrentCulture, "Given object is null. It is not allowed to store null objects in the workspace.", unitname));
            }

            string mapping;
            if (IOSpec.TryGetOutputMapping(unitname, out mapping) == false)
            {
                throw new WorkspaceException(String.Format(CultureInfo.CurrentCulture, "Component has not declared any output of the given name '{0}'", unitname));
            }
            if ((mapping != null && mapping.Length == 0))
            {
                throw new WorkspaceException(String.Format(CultureInfo.CurrentCulture, "'Output as' or 'Input as' cannot be empty. Please provide the name value for the parameter."));
            }
            if (mapping.Contains(Workspace.NAMESPACE_DELIMITER))
            {
                throw new WorkspaceException(String.Format("Output mapping '{0}' cannot contain character '{1}'", mapping, Workspace.NAMESPACE_DELIMITER));
            }

            //m_workspace.Store(mapping, unit); // not used anymore because of low performance when stored object was crossing app domain boundary
            
            try
            {
                // wrap unit into workspace data and serializae it to the stream
                WorkspaceUnitData workspaceData = new WorkspaceUnitData(unit);

                MemoryStream stream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(stream);
                //BinaryFormatter bf = new BinaryFormatter();
                //bf.Serialize(stream, workspaceData);

                workspaceData.WriteData(writer);

                byte[] bytes = stream.ToArray();

                //store byte arrays into the Workspace
                StoreBytes(mapping, unit.GetType(), bytes);
            }
            catch (SerializationException ex)
            {
                throw new WorkspaceException(
                    String.Format(
                        CultureInfo.CurrentCulture, "Object {0} for the given unit name '{1}' is not serializable and cannot be stored in the workspace.",
                        unit.GetType().FullName, mapping), ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new WorkspaceException(
                    String.Format(
                        CultureInfo.CurrentCulture, "Object {0} for the given unit name '{1}' cannot be stored in the workspace, because it is not serializable.",
                        unit.GetType().FullName, mapping), ex);
            }
        }

        /// <summary>
        /// Gets the time stamp of a given unit.
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns></returns>
        public DateTime GetDateTimeStamp(string unitname)
        {
            return m_workspace.GetDateTimeStamp(unitname);
        }

        /// <summary>
        /// Deletes the specified unit name.
        /// </summary>
        /// <param name="unitName">Name of the unit.</param>
        public void Delete(string unitName)
        {
            m_workspace.Delete(unitName);
        }

        public List<string> TypeDirectories
        {
            get { return m_workspace.TypeDirectories; }
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
            return m_workspace.LoadBytes(unitname);
        }

        /// <summary>
        /// Stores a unit bytes into the Workspace
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unitType">Type of the unit, that is stored. It later allows accessing workspace unit type without deserilizing data each time.</param>
        /// <param name="unitBytes">The unit bytes to be stored.</param>
        public void StoreBytes(string unitname, Type unitType, byte[] unitBytes)
        {
            m_workspace.StoreBytes(unitname, unitType, unitBytes);
        }

        /// <summary>
        /// Renames the unit.
        /// </summary>
        /// <param name="oldUnitname">The old unitname.</param>
        /// <param name="newUnitname">The new unitname.</param>
        public void RenameUnit(string oldUnitname, string newUnitname)
        {
            m_workspace.RenameUnit(oldUnitname, newUnitname);
        }

        /// <summary>
        /// Copies the unit.
        /// </summary>
        /// <param name="fromUnitname">From unitname.</param>
        /// <param name="toUnitname">To unitname.</param>
        public void CopyUnit(string fromUnitname, string toUnitname)
        {
            m_workspace.CopyUnit(fromUnitname, toUnitname);
        }

        /// <summary>
        /// Deletes the units that has name starting with given prefix (namespace).
        /// </summary>
        /// <param name="prefix">prefix of units to be deleted.</param>
        public void DeleteUnits(string prefix)
        {
            m_workspace.DeleteUnits(prefix);
        }

        /// <summary>
        /// Gets the list of units friendly names, which name starts with the given namespace prefixed with local namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">The namespace id.</param>
        /// <returns>
        /// Enumarable of units which real name starts with the given namespace
        /// </returns>
        public IEnumerable<string> GetUnits(string namespaceId)
        {
            return m_workspace.GetUnits(namespaceId);
        }
    }
}

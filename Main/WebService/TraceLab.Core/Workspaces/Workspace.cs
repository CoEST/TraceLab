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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using TraceLab.Core.Exceptions;
using TraceLab.Core.Workspaces.Serialization;
using TraceLabSDK;
using System.Security.Permissions;

namespace TraceLab.Core.Workspaces
{
    [Serializable]
    public class Workspace : MarshalByRefObject, IWorkspace, IWorkspaceInternal, IDisposable, System.ComponentModel.INotifyPropertyChanged
    {
        private readonly bool m_writeXml;
        private const string DataExt = ".xml";
        private const string CacheExt = ".cache";
        private readonly ReadOnlyObservableCollection<WorkspaceUnit> m_readonlyUnitsViewCollection;
        private readonly StreamManager m_streamManager;
        private readonly Dictionary<string, Assembly> m_typeAssemblyNames = new Dictionary<string,Assembly>();

        private readonly Dictionary<string, Type> m_supportedTypes = new Dictionary<string, Type>();
        /// <summary>
        /// Dictionary of supported types in the workspace (Key is type name).
        /// </summary>
        public Dictionary<string, Type> SupportedTypes
        {
            get { return this.m_supportedTypes; }
        }

        private bool m_writeUnitsToDisc = false;

        /// <summary>
        /// Represents workspace namespaces delimiter. Workspace may have several workspace wrappers each with their namespace delimited 
        /// with this NAMESPACE_DELIMITER
        /// </summary>
        public const string NAMESPACE_DELIMITER = ".";

        /// <summary>
        /// Gets or sets a value indicating whether workspace should write units to disc.
        /// </summary>
        /// <value>
        ///   <c>true</c> if workspace writes units to disc; otherwise, <c>false</c>.
        /// </value>
        public bool WriteUnitsToDisc
        {
            get 
            {
                if (m_objectDisposed)
                    throw new ObjectDisposedException("m_workspace");
                return m_writeUnitsToDisc; 
            }
            set 
            {
                if (m_objectDisposed)
                    throw new ObjectDisposedException("m_workspace");
                m_writeUnitsToDisc = value; 
            }
        }

        /// <summary>
        /// Keeps data in memory
        /// </summary>
        /// <remarks>A Unicomplex is a location in the fictional Star Trek universe that is used as a Workspace central base. </remarks>
        private readonly Dictionary<string, WorkspaceUnit> m_dataUnits;

        [NonSerialized]
        private readonly ReaderWriterLockSlim m_dataUnitsLock = new ReaderWriterLockSlim();
        [NonSerialized]
        bool m_objectDisposed;
        
        /// <summary>
        /// collection to show the data in the view.
        /// Notice that ReadOnlyObservableCollection is just a wrapper about actual collection
        /// </summary>
        private readonly ObservableCollection<WorkspaceUnit> m_unitsViewCollection;

        private string m_cacheTempDir;
        private string m_workspaceDir;

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
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");

            return null;
        }

        /// <summary>
        /// Creates a new s_instance of the Workspace with the given workspace, cache and type directories
        /// </summary>
        /// <param name="workspaceDir">Where XML workspace data should be stored</param>
        /// <param name="cacheDir">Where binary workspace data should be stored</param>
        /// <param name="typeDir">Where the supported types for the workspace are defined</param>
        public Workspace(string workspaceDir, string cacheDir, List<string> typeDirectories, StreamManager manager)
            : this(workspaceDir, cacheDir, manager)
        {
            foreach (string typeDir in typeDirectories)
            {
                if (string.IsNullOrWhiteSpace(typeDir))
                    throw new ArgumentException("Type Directory must exist", "typeDir");
                if (!Path.IsPathRooted(typeDir))
                    throw new ArgumentException("Absolute path is required for Type Directory: " + typeDir, "typeDir");
            }

            //set the paths
            TypeDirectories = typeDirectories;
            RegisterTypes();
        }

        /// <summary>
        /// Creates a new s_instance of the Workspace with the given workspace and cache directories, without pre-loading supported types.
        /// </summary>
        /// <param name="workspaceDir">Where XML workspace data should be stored</param>
        /// <param name="cacheDir">Where binary workspace data should be stored</param>
        public Workspace(string workspaceDir, string cacheDir, StreamManager manager)
            : this(manager)
        {
            WorkspaceDir = workspaceDir;
            CacheTempDir = cacheDir;

            InitCache();
        }

        private Workspace(StreamManager manager)
        {
            m_writeXml = false;
            m_dataUnits = new Dictionary<string, WorkspaceUnit>();
            //init stream manager
            m_streamManager = manager;

            //init observable collections
            m_unitsViewCollection = new ObservableCollection<WorkspaceUnit>();
            m_readonlyUnitsViewCollection = new ReadOnlyObservableCollection<WorkspaceUnit>(m_unitsViewCollection);
        }

        ~Workspace()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (!m_objectDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_objectDisposed)
            {
                if (disposing)
                {
                    m_dataUnitsLock.Dispose();
                }

                m_objectDisposed = true;
            }
        }

        public void Reset()
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");

            m_dataUnitsLock.EnterWriteLock();
            m_dataUnits.Clear();
            m_typeAssemblyNames.Clear();
            m_unitsViewCollection.Clear();
            m_dataUnitsLock.ExitWriteLock();

            InitCache();
        }

        public ReadOnlyObservableCollection<WorkspaceUnit> Units
        {
            get 
            {
                if (m_objectDisposed)
                    throw new ObjectDisposedException("m_workspace");

                return m_readonlyUnitsViewCollection; 
            }
        }

        /// <summary>
        /// Registers types contained inside default type directories (settings).
        /// </summary>
        private void RegisterTypes()
        {
            RegisterType(typeof(WorkspaceUnitData));

            //AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_TypeResolve;
            AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);

            this.m_typeDefinitionErrors = new List<string>();

            foreach (string dir in this.m_TypeDirectories)
            {
                RegisterTypesInDirectory(dir);
            }
        }

        /// <summary>
        /// Registers types contained inside packages.
        /// </summary>
        /// <param name="pPkgTypeDirectories">Directories inside packages where types are contained</param>
        public void RegisterPackageTypes(IEnumerable<string> pPkgTypeDirectories)
        {
            if (this.m_typeDefinitionErrors == null)
            {
                this.m_typeDefinitionErrors = new List<string>();
            }

            foreach (string dir in pPkgTypeDirectories)
            {
                if (this.TypeDirectories.Contains(dir) == false)
                {
                    RegisterTypesInDirectory(dir);
                    this.m_TypeDirectories.Add(dir);
                }
            }
        }

        /// <summary>
        /// Registers the types contained in the given directory.
        /// </summary>
        /// <param name="pDirectory">The directory where types are located</param>
        /// <returns>Errors ocurred while registering the types</returns>
        private void RegisterTypesInDirectory(string pDirectory)
        {
            if (Directory.Exists(pDirectory))
            {
                var typeDirectory = new DirectoryInfo(pDirectory);
                var files = typeDirectory.GetFiles("*.dll");
                foreach (FileInfo typeAssembly in files)
                {
                    try
                    {
                        AssemblyName name = AssemblyName.GetAssemblyName(typeAssembly.FullName);
                        if (name.Name.StartsWith("TraceLab"))
                        {
                            name.Version = null;
                        }

                        Assembly typeAssm = null;
                        typeAssm = Assembly.Load(name);
                        if (typeAssm == null)
                        {
                            Assembly.LoadFrom(typeAssembly.FullName);
                        }

                        bool found = false;
                        if (typeAssm != null)
                        {
                            var typesInAssembly = typeAssm.GetTypes();
                            foreach (Type type in typesInAssembly)
                            {
                                if (type.IsInterface == false && type.IsAbstract == false)
                                {
                                    RegisterType(type);
                                    found = true;
                                }
                                //object[] attribs = type.GetCustomAttributes(typeof(WorkspaceTypeAttribute), false);
                                //if (attribs.Length == 1)
                                //{
                                    //if (type.GetInterface("TraceLabSDK.Types.IWorkspaceType") != null)
                                    //if (!type.IsInterface && !type.IsAbstract)
                                    //{
                                    //    RegisterType(type);
                                    //    found = true;
                                    //}
                                //}
                            }
                        }

                        if (found && !m_typeAssemblyNames.ContainsKey(typeAssm.FullName))
                        {
                            m_typeAssemblyNames.Add(typeAssm.FullName, typeAssm);
                        }
                    }
                    catch (Exception e)
                    {
                        this.m_typeDefinitionErrors.Add(e.Message);
                    }
                }
            }
        }

        Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            Assembly found = null;
            m_typeAssemblyNames.TryGetValue(args.Name, out found);
            return found;
        }

        private void InitCache()
        {
            m_dataUnitsLock.EnterWriteLock();

            DirectoryInfo workDir = m_writeXml ? new DirectoryInfo(WorkspaceDir) : new DirectoryInfo(CacheTempDir);
            var files = workDir.GetFiles();

            foreach (var file in files)
            {
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                var realUnitName = filename;
                var unit = CreateWorkspaceUnit(realUnitName);
                AddDataUnit(unit);
            }

            m_dataUnitsLock.ExitWriteLock();
        }

        private string WorkspaceDir
        {
            get { return m_workspaceDir; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "m_workspace directory cannot be null");
                if (!Path.IsPathRooted(value))
                    throw new ArgumentException("Absolute worspace path is required.", "value");

                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }

                m_workspaceDir = value;
            }
        }

        private string CacheTempDir
        {
            get { return m_cacheTempDir; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "m_workspace directory cannot be null");
                if (!Path.IsPathRooted(value))
                    throw new ArgumentException("Absolute cache temp path is required.", "value");
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);

                m_cacheTempDir = value;
            }
        }

        private List<string> m_TypeDirectories;
        public List<string> TypeDirectories
        { 
            get
            {
                if (m_objectDisposed)
                    throw new ObjectDisposedException("m_workspace");

                return m_TypeDirectories;
            }
            private set { m_TypeDirectories = value; }
        }

        private IList<string> m_typeDefinitionErrors;
        public IList<string> TypeDefinitionErrors
        {
            get 
            {
                if (m_objectDisposed)
                    throw new ObjectDisposedException("m_workspace");

                return m_typeDefinitionErrors;
            }
            private set
            {
                m_typeDefinitionErrors = value;
                NotifyPropertyChanged("TypeDefinitionErrors");
            }
        }

        #region IWorkspace Members

        /// <summary>
        /// Tests whether a unit currently exists in the Workspace.
        /// </summary>
        /// <param name="unitname">The name of the unit</param>
        /// <returns>True if the unit exists in the Workspace, otherwise false.</returns>
        public bool Exists(string unitname)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");

            bool exists = false;

            m_dataUnitsLock.EnterReadLock();
            exists = m_dataUnits.ContainsKey(unitname);
            m_dataUnitsLock.ExitReadLock();
            
            return exists;
        }

        /// <summary>
        /// Loads data from the workspace
        /// </summary>
        /// <param name="unitname">Alias name for the data object, that is going to be loaded. </param>
        /// <returns>Data object, or null, if object has not been found for the given unitname</returns>
        public Object Load(string unitname)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (unitname == null)
                throw new ArgumentNullException("unitname", "Unitname cannot be null");
            if ((unitname.Length == 0))
                throw new ArgumentException("Unitname cannot be empty!", "unitname");

            m_dataUnitsLock.EnterUpgradeableReadLock();

            //returns null, when unit of given unit name does not exist
            Object retVal = null;
            
            try
            {
                WorkspaceUnit dataUnit;

                // It is now possible for a unit to exist in the dataunits while the dataUnit is null
                // - this should only occur when the file is in the cache
                bool exists = m_dataUnits.TryGetValue(unitname, out dataUnit);

                // If we don't already have this data unit, but the file exists in the data store
                if (exists == true && dataUnit == null && File.Exists(GetDataPath(unitname)))
                {
                    Trace.WriteLine("Load data using caching serializer.");

                    // Now we need to write, so make sure we get a write lock.
                    m_dataUnitsLock.EnterWriteLock();

                    // Note that CreateWorkspaceUnit takes the external unit name - not the 'real' one..
                    dataUnit = CreateWorkspaceUnit(unitname);

                    try
                    {
                        //add to dataunits
                        AddDataUnit(dataUnit);
                    }
                    finally
                    {
                        m_dataUnitsLock.ExitWriteLock();
                    }
                }

                if (dataUnit != null && string.Equals(unitname, dataUnit.RealUnitName, StringComparison.CurrentCulture))
                {
                    // Actually deserialize.
                    // -
                    // This can be outside the write lock as we've already added to the dictionary, 
                    // but must be inside the readlock in case anything decides to write a *different* data item
                    // to this workspace unit.
                    retVal = dataUnit.Data;

                    // If there was an error loading this data, then we need some sort of notification.
                    if (retVal == null)
                    {
                    }
                }
            }
            finally
            {
                m_dataUnitsLock.ExitUpgradeableReadLock();
            }

            return retVal;
        }

        /// <summary>
        /// Loads the byte representation of the workspace unit data.
        /// Should be used if caller is in another AppDomain. Prevents deserializing, and serializing again while
        /// crossing the boundary between app domains. 
        /// </summary>
        /// <param name="unitname">The unitname.</param>
        /// <returns>The full byte representation of the workspace unit data</returns>
        public byte[] LoadBytes(string unitname)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (unitname == null)
                throw new ArgumentNullException("unitname", "Unitname cannot be null");
            if ((unitname.Length == 0))
                throw new ArgumentException("Unitname cannot be empty!", "unitname");

            m_dataUnitsLock.EnterUpgradeableReadLock();

            //returns null, when unit of given unit name does not exist
            byte[] retVal = null;

            try
            {
                WorkspaceUnit dataUnit;

                // It is now possible for a unit to exist in the dataunits while the dataUnit is null
                // - this should only occur when the file is in the cache
                bool exists = m_dataUnits.TryGetValue(unitname, out dataUnit);

                // If we don't already have this data unit, but the file exists in the data store
                if (exists == true && dataUnit == null && File.Exists(GetDataPath(unitname)))
                {
                    Trace.WriteLine("Load data using caching serializer.");

                    // Now we need to write, so make sure we get a write lock.
                    m_dataUnitsLock.EnterWriteLock();

                    // Note that CreateWorkspaceUnit takes the external unit name - not the 'real' one..
                    dataUnit = CreateWorkspaceUnit(unitname);

                    try
                    {
                        //add to dataunits
                        AddDataUnit(dataUnit);
                    }
                    finally
                    {
                        m_dataUnitsLock.ExitWriteLock();
                    }
                }

                if (dataUnit != null && string.Equals(unitname, dataUnit.RealUnitName, StringComparison.CurrentCulture))
                {
                    // Get byte array of the data
                    // -
                    // This can be outside the write lock as we've already added to the dictionary, 
                    // but must be inside the readlock in case anything decides to write a *different* data item
                    // to this workspace unit.
                    
                    retVal = dataUnit.GetDataBytes();
                    
                    // If there was an error loading this data, then we need some sort of notification.
                    if (retVal == null)
                    {
                    }
                }
            }
            finally
            {
                m_dataUnitsLock.ExitUpgradeableReadLock();
            }

            return retVal;
        }

        /// <summary>
        /// Stores data in the workspace. 
        /// It uses caching serializer to store the object.
        /// </summary>
        /// <param name="unitname">Alias name for the object</param>
        /// <param name="unit">Actual data object</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Store(string unitname, object unit)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (unitname == null)
                throw new ArgumentNullException("unitname", "Unitname cannot be null");
            if ((unitname.Length == 0))
                throw new ArgumentException("Unitname cannot be empty!", "unitname");
            if (unit == null)
                throw new ArgumentException(String.Format("Object of the given unitname {0} is null and cannot be stored in the workspace.", unitname));

            m_dataUnitsLock.EnterWriteLock();

            try
            {
                WorkspaceUnit workspaceUnit;
                if (!m_dataUnits.TryGetValue(unitname, out workspaceUnit))
                {
                    workspaceUnit = CreateWorkspaceUnit(unitname);

                    lock(this)
                    {
                        workspaceUnit.Data = unit;
                    }

                    AddDataUnit(workspaceUnit);
                }
                else
                {
                    lock(this)
                    {
                        workspaceUnit.Data = unit;
                    }
                }

                //set timestamp
                string dataPath = GetDataPath(unitname);
                workspaceUnit.Timestamp = new FileInfo(dataPath).LastWriteTimeUtc;
            }
            catch (SerializationException ex)
            {
                throw new WorkspaceException(
                    String.Format(
                        CultureInfo.CurrentCulture, "Object {0} for the given unit name '{1}' is not serializable and cannot be stored in the workspace.",
                        unit.GetType().FullName, unitname), ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new WorkspaceException(
                    String.Format(
                        CultureInfo.CurrentCulture, "Object {0} for the given unit name '{1}' cannot be stored in the workspace, because it is not serializable.",
                        unit.GetType().FullName, unitname), ex);
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Stores a unit bytes into the Workspace
        /// Should be used if caller is in another AppDomain. Prevents deserializing, and serializing again while
        /// crossing the boundary between app domains.
        /// </summary>
        /// <param name="unitname">The name to store the unit as.  If the name already exists, the current value will be overwritten.</param>
        /// <param name="unitType">Type of the unit, that is stored. It later allows accessing workspace unit type without deserilizing data each time.</param>
        /// <param name="unitBytes">The unit bytes to be stored.</param>
        public void StoreBytes(string unitname, Type unitType, byte[] unitBytes)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (unitname == null)
                throw new ArgumentNullException("unitname", "Unitname cannot be null");
            if (unitBytes == null)
                throw new ArgumentNullException("unitBytes", "Unit bytes cannot be null");
            if ((unitname.Length == 0))
                throw new ArgumentException("Unitname cannot be empty!", "unitname");

            m_dataUnitsLock.EnterWriteLock();

            try
            {
                WorkspaceUnit workspaceUnit;
                if (!m_dataUnits.TryGetValue(unitname, out workspaceUnit))
                {
                    workspaceUnit = CreateWorkspaceUnit(unitname);

                    lock (this)
                    {
                        workspaceUnit.SetDataBytes(unitBytes, unitType);
                    }

                    AddDataUnit(workspaceUnit);
                }
                else
                {
                    lock (this)
                    {
                        workspaceUnit.SetDataBytes(unitBytes, unitType);
                    }
                }

                //set timestamp
                string dataPath = GetDataPath(unitname);
                workspaceUnit.Timestamp = new FileInfo(dataPath).LastWriteTimeUtc;
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        public DateTime GetDateTimeStamp(string unitname)
        {
            if (m_objectDisposed)
            {
                throw new ObjectDisposedException("m_workspace");
            }
            if (unitname == null)
            {
                throw new ArgumentNullException("unitname", "FriendlyUnitName cannot be null");
            }
            if (unitname.Length == 0)
            {
                throw new ArgumentException("FriendlyUnitName cannot be empty!", "unitname");
            }

            if (!m_dataUnits.ContainsKey(unitname))
            {
                throw new ArgumentException("Workspace does not contain given unitname", "unitname");
            }

            return m_dataUnits[unitname].Timestamp;
        }

        #endregion

        /// <summary>
        /// Creates the workspace unit.
        /// </summary>
        /// <param name="unitName">Name of the unit.</param>
        /// <returns></returns>
        internal WorkspaceUnit CreateWorkspaceUnit(string unitName)
        {
            string dataPath = GetDataPath(unitName);
            string cachePath = GetCachePath(unitName);

            // Create the WorkspaceUnit that we'll use for deserialization
            var serializer = new CachingSerializer(m_streamManager, dataPath, cachePath, m_supportedTypes.Values.ToArray(), m_writeUnitsToDisc, false);
            var dataUnit = new WorkspaceUnit(unitName, serializer);
            
            return dataUnit;
        }

        /// <summary>
        /// Removes a unit from memory - does NOT remove it from the workspace.
        /// </summary>
        /// <param name="unitName"></param>
        internal void DropUnitFromMemory(string unitName)
        {
            m_dataUnitsLock.EnterWriteLock();
            try
            {
                DropUnitFromMemoryInternal(unitName);
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        public void Delete(string unitName)
        {
            m_dataUnitsLock.EnterWriteLock();
            try
            {
                RemoveUnitFromWorkspaceInternal(unitName);
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deletes the units that has name starting with given namespaceId.
        /// </summary>
        /// <param name="namespaceId">The namespace id - prefix of units to be deleted</param>
        public void DeleteUnits(string prefix)
        {
            m_dataUnitsLock.EnterWriteLock();

            try
            {
                HashSet<string> experimentUnits = new HashSet<string>();

                foreach (WorkspaceUnit unit in Units)
                {
                    if (unit.RealUnitName.StartsWith(prefix))
                    {
                        experimentUnits.Add(unit.RealUnitName);
                    }
                }

                if (experimentUnits.Count > 0)
                {
                    foreach (string unitname in experimentUnits)
                    {
                        RemoveUnitFromWorkspaceInternal(unitname);
                    }
                }
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Performs the logic to removes a unit from memory - does NOT remove it from the workspace.
        /// 
        /// This modifies the dataunits and should only be called from within code that locks the dataunits.
        /// </summary>
        private void DropUnitFromMemoryInternal(string realUnitName)
        {
            m_dataUnits[realUnitName] = null;
        }

        /// <summary>
        /// Performs the logic to removes a unit from the workspace.
        /// 
        /// This modifies the dataunits and should only be called from within code that locks the dataunits.
        /// </summary>
        private void RemoveUnitFromWorkspaceInternal(string realUnitName)
        {
            string dataPath = GetDataPath(realUnitName);
            string cachePath = GetCachePath(realUnitName);
            
            m_unitsViewCollection.Remove(m_dataUnits[realUnitName]);
            m_dataUnits.Remove(realUnitName);

            //close the streams - note, it removes old streams from manager. It does not delete files
            m_streamManager.CloseStream(dataPath);
            m_streamManager.CloseStream(cachePath);
            
            File.Delete(dataPath);
            File.Delete(cachePath);
        }

        private static string GetFileName(string path, string externalUnitName, string extension)
        {
            string filename = System.IO.Path.Combine(path, externalUnitName + extension);
            return filename;
        }
         
        private string GetDataPath(string unitName)
        {
            return GetFileName(WorkspaceDir, unitName, DataExt);
        }

        private string GetCachePath(string unitName)
        {
            return GetFileName(CacheTempDir, unitName, CacheExt);
        }


        /// <summary>
        /// Clears the workspace.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the workspace is in an invalid state</exception>
        public void Clear()
        {
            if (m_dataUnitsLock == null)
                throw new InvalidOperationException();
            if (m_dataUnits == null)
                throw new InvalidOperationException();
            if (m_unitsViewCollection == null)
                throw new InvalidOperationException();

            m_dataUnitsLock.EnterWriteLock();
            try
            {
                ClearInternal();
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes everything from the workspace, including the cache.
        /// </summary>
        private void ClearInternal()
        {
            var dataunitsKeys = m_dataUnits.Keys.ToArray();
            foreach (var unitKey in dataunitsKeys)
            {
                RemoveUnitFromWorkspaceInternal(unitKey);
            }

            m_dataUnits.Clear();
            m_unitsViewCollection.Clear();
        }

        private void AddDataUnit(WorkspaceUnit workspaceUnit)
        {
            m_dataUnits.Add(workspaceUnit.RealUnitName, workspaceUnit);
            m_unitsViewCollection.Add(workspaceUnit);
        }

        public void RegisterTypes(Type[] types)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (types == null)
                throw new ArgumentNullException("s_metricTypes");

            foreach (Type type in types)
            {
                RegisterType(type);
            }
        }

        public void RegisterType(Type type)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (type == null || type.FullName == null)
                throw new InvalidOperationException();

            Type existingType;
            if (m_supportedTypes.TryGetValue(type.FullName, out existingType) == false)
            {
                ValidateType(type);
                m_supportedTypes.Add(type.FullName, type);
            }
            //else if (!existingType.Equals(type))
            //{
            //    throw new InvalidOperationException("Attempting to register ambiguous s_metricTypes");
            //}
        }

        /// <summary>
        /// Validates the type.
        /// </summary>
        /// <param name="type">The type to validate.</param>
        private static void ValidateType(Type type)
        {
            if (!type.IsPrimitive && !type.IsGenericParameter && !type.IsAbstract && !type.IsInterface)
            {
                if (type.IsSerializable)
                {
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (PropertyInfo prop in properties)
                    {
                        if (prop.PropertyType != type && prop.PropertyType.IsPrimitive == false &&
                            prop.PropertyType.IsAbstract == false)
                        {
                            ValidateType(prop.PropertyType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers the type of the object.
        /// </summary>
        /// <param name="objectOfTypeToRegister">The object of type to register.</param>
        public void RegisterObjectType(object objectOfTypeToRegister)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (objectOfTypeToRegister == null)
                throw new ArgumentNullException("objectOfTypeToRegister");

            Type objectType = objectOfTypeToRegister.GetType();
            if (objectType.FullName == null)
                throw new InvalidOperationException();

            Type existingType;
            m_supportedTypes.TryGetValue(objectType.FullName, out existingType);
            if (existingType != null && !existingType.Equals(objectType))
            {
                throw new InvalidOperationException("Attempting to register ambiguous s_metricTypes");
            }

            m_supportedTypes.Add(objectType.FullName, objectType);
        }

        /// <summary>
        /// Gets the workspace namespace id. Defaul main workspace namespace is empty.
        /// </summary>
        /// <value>
        /// The namespace id.
        /// </value>
        public string NamespaceId
        {
            get 
            {
                if (m_objectDisposed)
                    throw new ObjectDisposedException("m_workspace");

                return String.Empty; 
            }
        }

        /// <summary>
        /// Renames the unit.
        /// </summary>
        /// <param name="oldUnitname">The old unitname.</param>
        /// <param name="newUnitname">The new unitname.</param>
        public void RenameUnit(string oldUnitname, string newUnitname)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (String.IsNullOrWhiteSpace(oldUnitname))
                throw new ArgumentException("Old unitname cannot be null or empty or whitespace!", "oldUnitname");
            if (String.IsNullOrWhiteSpace(newUnitname))
                throw new ArgumentException("New unitname cannot be null or empty or whitespace!", "newUnitname");

            m_dataUnitsLock.EnterWriteLock();

            try
            {
                if (m_dataUnits.ContainsKey(oldUnitname))
                {
                    string newDataPath = GetDataPath(newUnitname);
                    string newCachePath = GetCachePath(newUnitname);

                    //get data unit to be renamed
                    var dataUnit = m_dataUnits[oldUnitname];

                    string oldDataPath = GetDataPath(oldUnitname);
                    string oldCachePath = GetCachePath(oldUnitname);

                    //check if there is already stream with the same name as newUnitname
                    if (m_dataUnits.ContainsKey(newUnitname) == true)
                    {
                        RemoveUnitFromWorkspaceInternal(newUnitname);
                    }

                    //move stream within stream manager from old name to new name
                    m_streamManager.MoveStream(oldCachePath, newCachePath);
                    m_streamManager.MoveStream(oldDataPath, newDataPath);

                    // rename data unit providing new data and cache path for caching serializer
                    dataUnit.Rename(newUnitname, newDataPath, newCachePath);

                    //remap data unit in dataUnits. Note unitsViewCollection does not have to be modified - since the data unit object didn't change
                    m_dataUnits.Remove(oldUnitname);

                    m_dataUnits.Add(dataUnit.RealUnitName, dataUnit);
                }
            }
            finally
            {
                m_dataUnitsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Copies the unit.
        /// </summary>
        /// <param name="fromUnitname">From unitname.</param>
        /// <param name="toUnitname">To unitname.</param>
        public void CopyUnit(string fromUnitname, string toUnitname)
        {
            if (m_objectDisposed)
                throw new ObjectDisposedException("m_workspace");
            if (String.IsNullOrWhiteSpace(fromUnitname))
                throw new ArgumentException("fromUnitname cannot be null or empty or whitespace!", "fromUnitname");
            if (String.IsNullOrWhiteSpace(toUnitname))
                throw new ArgumentException("toUnitname cannot be null or empty or whitespace!!", "toUnitname");

            if (m_dataUnits.ContainsKey(fromUnitname))
            {
                object obj = Load(fromUnitname);
                Store(toUnitname, obj);
            }
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }

        #endregion

        internal void ClearLoadErrors()
        {
            this.m_typeDefinitionErrors = new List<string>();
        }

        /// <summary>
        /// Gets the list of units short names, which name starts with the given namespace prefixed with local namespace (prefix to the real unit name)
        /// </summary>
        /// <param name="namespaceId">The namespace id.</param>
        /// <returns>
        /// Enumarable of units which real name starts with the given namespace
        /// </returns>
        public IEnumerable<string> GetUnits(string prefix)
        {
            List<string> units = new List<string>();

            m_dataUnitsLock.EnterReadLock();

            try
            {
                foreach (WorkspaceUnit unit in Units)
                {
                    if (unit.RealUnitName.StartsWith(prefix))
                    {
                        units.Add(unit.ShortUnitName);
                    }
                }
            }
            finally
            {
                m_dataUnitsLock.ExitReadLock();
            }

            return units;
        }
    }
}

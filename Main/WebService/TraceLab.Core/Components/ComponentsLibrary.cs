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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using TraceLab.Core.Exceptions;
using TraceLab.Core.Utilities;
using TraceLab.Core.Workspaces;
using TraceLabSDK;
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Components library is the central class that manages component definitions, and loads components assemblies.
    /// 
    /// The Component Library is a quite large class that was growing overtime. 
    /// It has quite a bit of code and functionality, but its core functionality is to control the scan via Component Scanner, 
    /// then keeping the list of all components metadata definitions discovered during that scan, 
    /// and finally loading/constructing component classes when the experiment starts via Component Loader. 
    /// 
    /// It also manages components in the packages.
    /// </summary>
    public class ComponentsLibrary : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged, IComponentsLibrary
    {
        private static ComponentsLibrary instance;

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

        private IEnumerable<TraceLab.Core.Settings.SettingsPath> m_componentsDirectoryPaths;

        /// <summary>
        /// Reads the tag overrides.
        /// </summary>
        private void ReadTagOverrides()
        {
            foreach (string dir in m_componentsDirectoryPaths)
            {
                string componentTagsFile = Path.Combine(dir, "components.tags");
                ReadTags(componentTagsFile, (a) => { return GetComponentDefinition(a, null); });
            }

            // Finally, Check for user-specific overrides.
            // TODO: How do I determine where the user file is?
        }


        /// <summary>
        /// Gets a library that is capable of using the referenced packages.
        /// </summary>
        /// <param name="references">The references to use.</param>
        /// <returns>
        /// Returns a version of the library that will search through the given references.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="references"/> is null.</exception>
        public IComponentsLibrary GetPackageAwareLibrary(IEnumerable<IPackageReference> references)
        {
            if (references == null)
                throw new ArgumentNullException();

            return new PackageAwareComponentLibrary(this, references);
        }

        /// <summary>
        /// Reads the tags.
        /// </summary>
        /// <param name="componentTagsFile">The component tags file.</param>
        /// <param name="getComponentDefinition">The get component definition.</param>
        public static void ReadTags(string componentTagsFile, Func<string, MetadataDefinition> getComponentDefinition)
        {
            if (File.Exists(componentTagsFile))
            {
                ComponentTagCollection tags = null;
                try
                {
                    var settings = new System.Xml.XmlReaderSettings();
                    settings.IgnoreComments = true;
                    settings.IgnoreWhitespace = true;
                    using (var reader = System.Xml.XmlReader.Create(componentTagsFile))
                    {
                        var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(ComponentTagCollection), null);
                        tags = (ComponentTagCollection)serial.Deserialize(reader);
                    }
                }
                catch (Exception)
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn("Error reading component tags: ");
                }

                if (tags != null)
                {
                    foreach (ComponentTags componentTags in tags)
                    {
                        try
                        {
                            var definition = getComponentDefinition(componentTags.ComponentDefinitionId);
                            if (definition.Tags == null)
                            {
                                definition.Tags = componentTags;
                            }
                            else
                            {
                                definition.Tags.ApplyOverrides(componentTags);
                            }
                        }
                        catch (ComponentsLibraryException)
                        {
                        }
                    }
                }
            }
        }

        private string m_dataRoot;
        /// <summary>
        /// Gets or sets the data root.
        /// </summary>
        /// <value>
        /// The data root.
        /// </value>
        public string DataRoot
        {
            get { return m_dataRoot; }
            set
            {
                if (m_dataRoot != value)
                {
                    m_dataRoot = value;
                    NotifyPropertyChanged("DataRoot");
                }
            }
        }

        /// <summary>
        /// Checks if directories in the given list exists.
        /// If directory does not exist it logs information to the user
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="existingDirectories">Method returns the list of directories, that exist and are correct, subset of the given original directories list</param>
        /// <returns>
        /// true if at least one directory in the list exists, false otherwise
        /// </returns>
        private bool CheckDirectoriesExist(IEnumerable<TraceLab.Core.Settings.SettingsPath> directories, 
            out IEnumerable<string> existingDirectories)
        {
            List<string> correctDirectories = new List<string>();

            bool exists = false;

            if (directories == null)
                throw new ArgumentNullException("directories", "Components directory path cannot be null");

            foreach (string dirPath in directories)
            {
                if (!System.IO.Path.IsPathRooted(dirPath))
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn(String.Format("Component directory '{0}' must be absolute path.", dirPath));
                }

                if (Directory.Exists(dirPath) == false)
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn(String.Format("Component directory does not exist: {0}", dirPath));
                }
                else
                {
                    //at least one directory must exists
                    correctDirectories.Add(dirPath);
                    exists = true;
                }
            }

            existingDirectories = correctDirectories;

            return exists;
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ComponentsLibrary() { }

        /// <summary>
        /// Prevents a default instance of the <see cref="ComponentsLibrary"/> class from being created.
        /// The constructor initializes the components definition collection, and components cache of all definitions.
        /// </summary>
        /// <param name="componentsDirectories">The components directories.</param>
        private ComponentsLibrary(IEnumerable<TraceLab.Core.Settings.SettingsPath> componentsDirectories)
        {
            m_componentsDirectoryPaths = componentsDirectories;
            m_readonlyComponentDefinitions = new ReadonlyObservableComponentDefinitionCollection(m_componentDefinitions);
            m_componentFilesCache = new ComponentsLibraryCache(this.m_dataRoot);
        }

        /// <summary>
        /// Access to singleton instance of components library
        /// Components must be initialized before usage, otherwise exception is being thrown
        /// </summary>
        public static ComponentsLibrary Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException("ComponentsLibrary has not been initialized.");

                return instance;
            }
        }

        /// <summary>
        /// Inits the components library. Must be executed
        /// </summary>
        /// <param name="componentsDirectories">The components directories.</param>
        public static void Init(IEnumerable<TraceLab.Core.Settings.SettingsPath> componentsDirectories)
        {
            if (instance != null)
                throw new InvalidOperationException("ComponentsLibrary has already been initalized");

            instance = InitInternal(componentsDirectories);
        }

        /// <summary>
        /// Inits the components library. Does not check if it has been initialized. 
        /// Note, it is seperated to another method for the Testing purposes, so that it TraceLabApplicationContext can create new testing libraries. 
        /// See TraceLabTestContext.
        /// </summary>
        /// <param name="componentsDirectories">The components directories.</param>
        /// <returns></returns>
        private static ComponentsLibrary InitInternal(IEnumerable<TraceLab.Core.Settings.SettingsPath> componentsDirectories)
        {
            return new ComponentsLibrary(componentsDirectories);
        }

        /// <summary>
        /// m_componentDefinitions field is a collection of components definition that are available to the user
        /// to add to its experiment.
        /// </summary>
        private ObservableComponentDefinitionCollection m_componentDefinitions = new ObservableComponentDefinitionCollection();
        
        /// <summary>
        /// m_readonlyComponentDefinitions is a readonly wrapper m_componentDefinitions
        /// </summary>
        private ReadonlyObservableComponentDefinitionCollection m_readonlyComponentDefinitions;

        /// <summary>
        /// Gets the collection of Components Definitions that are currently available in the library
        /// </summary>
        public IEnumerable<MetadataDefinition> Components
        {
            get
            {
                return m_readonlyComponentDefinitions;
            }
        }

        /// <summary>
        /// m_componentFilesCache keeps the MetadataDefinitions contained in all assembly and composite components files 
        /// found by ComponentScanner. Once a file is read once it is not read again until it is updated. The whole cache
        /// is stored in a binary file that is read during startup time and it is re-built when the user presses the 
        /// re-scan button on the Components Library UI. New files or modified files found during TraceLab startup are
        /// added or updated in the collection.
        /// </summary>
        private ComponentsLibraryCache m_componentFilesCache;

        /// <summary>
        /// Collection of directory paths were type files are located.
        /// </summary>
        private HashSet<string> m_packageTypeDirectories;
        public HashSet<string> PackageTypeDirectories
        {
            get { return this.m_packageTypeDirectories; }
        }

        /// <summary>
        /// Gets or sets the map from old component GUID to new their new GUID.
        /// If component still had old guid set, the map will keep this information, so that it can find the
        /// new autogenerated guid. 
        /// </summary>
        /// <value>
        /// The old GUID to new GUID map.
        /// </value>
        [Obsolete("Needed for backwards compability with old experiments that may refer to components by their old guids (not autogenerated guids).")]
        public IDictionary<string, string> m_oldGuidToNewGuidComponentsMap = new Dictionary<string, string>();

        public void Add(MetadataDefinition componentMetadefinition)
        {
            //check if file exists
            if (!File.Exists(componentMetadefinition.Assembly))
            {
                throw new ComponentsLibraryException("Specified assemblyFile file '" + componentMetadefinition.Assembly + "' does not exists.");
            }   

            string compID = componentMetadefinition.ID.ToLower(CultureInfo.CurrentCulture);
            if (m_componentDefinitions.Contains(compID))
            {
                var existingDefinition = m_componentDefinitions[compID];
                throw new ComponentsLibraryException(String.Format(Messages.SameComponents, existingDefinition.Classname, existingDefinition.Assembly, 
                                                                    componentMetadefinition.Classname, componentMetadefinition.Assembly));
            }

            m_componentDefinitions.Add(componentMetadefinition);

            //collect also all io spec types, so that components library view can be filter by it
            CollectFilterTypes(componentMetadefinition);
        }

        /// <summary>
        /// Removes the specified component metadefinition.
        /// </summary>
        /// <param name="componentMetadefinition">The component metadefinition.</param>
        /// <returns></returns>
        public bool Remove(MetadataDefinition componentMetadefinition)
        {
            return m_componentDefinitions.Remove(componentMetadefinition);   
        }

        /// <summary>
        /// Tries to find composite component metadata definition with given source filename. 
        /// </summary>
        /// <param name="sourceFilename">The source filename .tcml</param>
        /// <param name="definition">The definition is null if not found.</param>
        /// <returns>true if found, false otherwise</returns>
        public bool TryFindCompositeComponentMetadataDefinition(string sourceFilename, out CompositeComponentMetadataDefinition definition)
        {
            bool found = false;
            definition = null;

            foreach (MetadataDefinition def in Components)
            {
                var compositeCompDef = def as CompositeComponentMetadataDefinition;
                if (compositeCompDef != null)
                {
                    if (compositeCompDef.Assembly == sourceFilename)
                    {
                        found = true;
                        definition = compositeCompDef;
                        break;
                    }
                }
            }

            return found;
        }

        #region Collect Types for filtering

        private HashSet<KeyValuePair<string, string>> m_availableFilterTypes = new HashSet<KeyValuePair<string, string>>();
        /// <summary>
        /// Gets the available types which components library can be filter by.
        /// The key is user friendly type name, and its value is a full type name.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> AvailableFilterTypes
        {
            get
            {
                return m_availableFilterTypes;
            }
        }

        /// <summary>
        /// Collects the types of IOItems from all given components metadatadefinition which library can be filtered by.
        /// </summary>
        /// <param name="components">The components.</param>
        private void CollectFilterTypes(IEnumerable<MetadataDefinition> components)
        {
            foreach (MetadataDefinition def in components)
            {
                CollectFilterTypes(def);
            }
        }

        /// <summary>
        /// Collects the types IOItems from metadata definition which library can be filtered by. 
        /// </summary>
        /// <param name="metadefinition">The metadefinition.</param>
        private void CollectFilterTypes(MetadataDefinition metadefinition)
        {
            ComponentMetadataDefinition componentMetadefinition = metadefinition as ComponentMetadataDefinition;
            if (componentMetadefinition != null)
            {
                foreach (KeyValuePair<string, IOItemDefinition> pair in componentMetadefinition.IOSpecDefinition.Input)
                {
                    m_availableFilterTypes.Add(new KeyValuePair<string, string>(pair.Value.FriendlyType, pair.Value.Type));
                }
                foreach (KeyValuePair<string, IOItemDefinition> pair in componentMetadefinition.IOSpecDefinition.Output)
                {
                    m_availableFilterTypes.Add(new KeyValuePair<string, string>(pair.Value.FriendlyType, pair.Value.Type));
                }
            }
        }

        #endregion

        /// <summary>
        /// Occurs when the library finsished rescanning
        /// </summary>
        public event EventHandler Rescanned;

        /// <summary>
        /// Invokes the Rescanned event; called whenever the library has been rescanned
        /// </summary>
        /// <param name="e"></param>
        private void OnRescanned()
        {
            IsRescanning = false;
            if (Rescanned != null)
                Rescanned(this, EventArgs.Empty);


        }

        /// <summary>
        /// Occurs when library started rescanning
        /// </summary>
        public event EventHandler Rescanning;

        /// <summary>
        /// Invokes the Rescanning event; called whenever the library starts rescanning
        /// </summary>
        /// <param name="e"></param>
        private void OnRescanning()
        {
            IsRescanning = true;
            if (Rescanning != null)
                Rescanning(this, EventArgs.Empty);
        }

        private bool m_isRescanning;
        /// <summary>
        /// Gets a value indicating whether the library is beind rescanned.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the library is being rescanned; otherwise, <c>false</c>.
        /// </value>
        public bool IsRescanning
        {
            get
            {
                return m_isRescanning;
            }
            private set
            {
                if (m_isRescanning != value)
                {
                    m_isRescanning = value;
                    NotifyPropertyChanged("IsRescanning");
                }
            }
        }

        /// <summary>
        /// Loads the component based on the given metadata.
        /// 
        /// Note, that the component loader is constructed in the secondary App Domain via CreateInstanceAndUnwrap.
        /// It is essential that this is done in another App Domain, so that the assembly can be unloaded after
        /// experiment execution is completed. 
        /// </summary>
        /// <param name="componentMetadata">The component metadata.</param>
        /// <param name="workspace">The workspace.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which component assembly is going to be loaded into.</param>
        /// <returns>Loaded component</returns>
        internal IComponent LoadComponent(ComponentMetadata componentMetadata, IWorkspaceInternal workspace, ComponentLogger logger, AppDomain componentsAppDomain)
        {
            ComponentMetadataDefinition componentMetaDef = componentMetadata.ComponentMetadataDefinition;

            // ComponentLoader must be MarshalByRef, otherwise the properties don't get filled out the 
            // way that we want them to.
            ComponentLoader loader = (ComponentLoader)componentsAppDomain.CreateInstanceAndUnwrap(
                Assembly.GetExecutingAssembly().FullName, typeof(ComponentLoader).FullName, false,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance, null,
                new object[] { componentMetadata, workspace },
                System.Globalization.CultureInfo.CurrentCulture, new object[] { });

            // Perform the actual load by passing a reference to the Loader's Load function to the new 
            // AppDomain to execute
            loader.Load(logger);
            return loader.LoadedComponent;
        }

        /// <summary>
        /// Gets the component definition with a given ID
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <returns>
        /// The definition with the given ID.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        ///   
        /// <exception cref="TraceLab.Core.Exceptions.ComponentsLibraryException">Thrown if the library doesn't contain any definitions by the given ID.</exception>
        public MetadataDefinition GetComponentDefinition(string componentID)
        {
            return GetComponentDefinition(componentID, null);
        }

        /// <summary>
        /// Gets the component definition with a given ID
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <param name="references">Packages that should be checked for this component. Can be null or empty.</param>
        /// <returns>The definition with the given ID.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        /// <exception cref="TraceLab.Core.Exceptions.ComponentsLibraryException">Thrown if the library doesn't contain any definitions by the given ID.</exception>
        /// <remarks>
        /// It's important to note that the references will only be checked if the component is not found in the main library.
        /// </remarks>
        public MetadataDefinition GetComponentDefinition(String componentID, IEnumerable<IPackageReference> references)
        {
            if (componentID == null)
                throw new ArgumentNullException("componentID", "Component ID cannot be null");

            componentID = componentID.Trim().ToLower(CultureInfo.CurrentCulture);

            MetadataDefinition compMetaDef = null;
            if (m_componentDefinitions.Contains(componentID))
            {
                compMetaDef = m_componentDefinitions[componentID];
            }
            
            if (compMetaDef == null && references != null)
            {
                foreach (IPackageReference reference in references)
                {
                    ObservableComponentDefinitionCollection package;
                    if (m_packageLibrary.TryGetValue(reference.ID, out package))
                    {
                        if (package.Contains(componentID))
                        {
                            compMetaDef = package[componentID];
                        }
                    }

                    if (compMetaDef != null)
                    {
                        break;
                    }
                }
            }

            if (compMetaDef == null)
            {
                throw new ComponentsLibraryException( "Component library does not contain any Component of the given ID " + componentID);
            }

            return compMetaDef;
        }

        /// <summary>
        /// Tries to get the component definition with a given ID.
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <param name="metadataDefinition">The metadata definition.</param>
        /// <returns>
        /// True if the definition was found in the library, otherwise false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        public bool TryGetComponentDefinition(string componentID, out MetadataDefinition metadataDefinition)
        {
            return TryGetComponentDefinition(componentID, null, out metadataDefinition);
        }

        /// <summary>
        /// Tries to get the component definition with a given ID.
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <param name="references">Packages that should be checked for this component. Can be null or empty.</param>
        /// <param name="metadataDefinition">The metadata definition.</param>
        /// <returns>
        /// True if the definition was found in the library, otherwise false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        /// <remarks>
        /// It's important to note that the references will only be checked if the component is not found in the main library.
        /// </remarks>
        public bool TryGetComponentDefinition(String componentID, IEnumerable<IPackageReference> references, out MetadataDefinition metadataDefinition)
        {
            if (componentID == null)
                throw new ArgumentNullException("componentID", "Component ID cannot be null");

            componentID = componentID.Trim().ToLower(CultureInfo.CurrentCulture);

            if (m_oldGuidToNewGuidComponentsMap.ContainsKey(componentID))
            {
                componentID = m_oldGuidToNewGuidComponentsMap[componentID];
                componentID = componentID.Trim().ToLower(CultureInfo.CurrentCulture);
            }

            bool retVal = false;
            metadataDefinition = null;
            if (m_componentDefinitions.Contains(componentID))
            {
                retVal = true;
                metadataDefinition = m_componentDefinitions[componentID];
            }

            if (!retVal && references != null)
            {
                foreach (IPackageReference reference in references)
                {
                    ObservableComponentDefinitionCollection package;
                    if (m_packageLibrary.TryGetValue(reference.ID, out package))
                    {
                        if (package.Contains(componentID))
                        {
                            retVal = true;
                            metadataDefinition = package[componentID];
                        }
                    }

                    if (retVal)
                    {
                        break;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the components definition from references.
        /// </summary>
        /// <param name="references">The references.</param>
        /// <returns></returns>
        public HashSet<MetadataDefinition> GetComponentsDefinitionFromReferences(IEnumerable<IPackageReference> references)
        {
            HashSet<MetadataDefinition> collection = new HashSet<MetadataDefinition>();

            foreach (IPackageReference reference in references)
            {
                ObservableComponentDefinitionCollection package;
                if (m_packageLibrary.TryGetValue(reference.ID, out package))
                {
                    collection.UnionWith(package);
                }
            }

            return collection;
        }

        /// <summary>
        /// Loads the components definitions.
        /// </summary>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        private void LoadComponentsDefinitions(IEnumerable<string> workspaceTypeDirectories)
        {
            if (DataRoot == null)
                throw new InvalidOperationException("Data root directory cannot be null. Please set the data root first.");
            if (Directory.Exists(DataRoot) == false)
                throw new InvalidOperationException("Data root directory do not exist. Please set the data root first.");
            
            //if there is at least one directory existing continue and scan the directories
            //method creates new list of correct paths to be passed to he scanner
            IEnumerable<string> directoriesToScan;
            if (CheckDirectoriesExist(m_componentsDirectoryPaths, out directoriesToScan) == true)
            {
                List<string> errors = new List<string>();

                var scanResults = ScanDirectoriesForComponents(this, directoriesToScan, DataRoot, workspaceTypeDirectories);
                foreach (MetadataDefinition def in scanResults.Components)
                {
                    try
                    {
                        Add(def);
                    }
                    catch (ComponentsLibraryException e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().Warn(e.Message);
                        errors.Add(e.Message);
                    }
                }

                //keep the map from old guids to new components guids
                m_oldGuidToNewGuidComponentsMap.AddRange(scanResults.OldGuidToNewGuidMap, true);

                var howToAdd = new Action<MetadataDefinition>((MetadataDefinition def) =>
                {
                    m_componentDefinitions.Add(def);
                    CollectFilterTypes(def);
                });

                PostProcessCompositeComponents(this, scanResults.CompositeComponents, DataRoot, howToAdd);

                ReadTagOverrides();

                if (errors.Count > 0 || scanResults.Errors.Count > 0)
                {
                    if (DefinitionErrors == null)
                    {
                        DefinitionErrors = new List<string>();
                    }
                    ((List<string>)DefinitionErrors).AddRange(scanResults.Errors);
                    ((List<string>)DefinitionErrors).AddRange(errors);
                }
            }
        }

        /// <summary>
        /// Scans the provided directories for components.
        /// </summary>
        /// <param name="library">Instance of the component library.</param>
        /// <param name="directoriesToScan">Directories to scan.</param>
        /// <param name="dataRoot">Data root.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <returns>Results of the scanning (components metadata, errors and new files loaded.</returns>
        private static ComponentScanResults ScanDirectoriesForComponents(ComponentsLibrary library,
            IEnumerable<string> directoriesToScan, string dataRoot, IEnumerable<string> workspaceTypeDirectories)
        {   
            var results = new ComponentScanResults();

            ISet<string> filesInCache = library.m_componentFilesCache.GetUpToDateFiles();

            if (ComponentScanner.Scan(directoriesToScan, workspaceTypeDirectories, filesInCache, results))
            {
                library.m_componentFilesCache.AddComponentFiles(results.NewFilesLoaded);

                library.m_componentFilesCache.ProcessComponentScanResults(results);
            }

            foreach (string error in results.Errors)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(error);
            }

            return results;
        }

        /// <summary>
        /// Performs the post-scan processing of composite components, returning a collection of components that have been processed.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="compositeComponents">The composite components.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <param name="dataRoot">The data root.</param>
        /// <returns>A collection of CompositeComponentDefinitions that have been processed and are therefore valid to add to the library.</returns>
        /// <remarks>Potentially, not all CompositeComponentDefinitions will be processed.  This will happen if a dependency is missing.</remarks>
        private static void PostProcessCompositeComponents(ComponentsLibrary library, ObservableComponentDefinitionCollection compositeComponents, string dataRoot, Action<MetadataDefinition> howToAdd)
        {
            //get components definition sorted by their dependency
            Queue<CompositeComponentMetadataDefinition> sortedComponentsDefinitions = TopologicalSort(compositeComponents);

            //once sorted run post process xml in order of their dependency
            //foreach composite components definition run PostProcessReadXml to load their subgraphs
            //note that all other primitive componets are already in the library
            while (sortedComponentsDefinitions.Count > 0)
            {
                CompositeComponentMetadataDefinition compositeCompDef = sortedComponentsDefinitions.Dequeue();

                //loads subexperiment by calling post processs Xml
                compositeCompDef.PostProcessReadXml(library, dataRoot);
                howToAdd(compositeCompDef);
            }
        }

        /// <summary>
        /// Scans the package components.
        /// </summary>
        /// <param name="pkgManager">The PKG manager.</param>
        /// <param name="workspaceTypes">The workspace types.</param>
        public void ScanPackageComponents(PackageSystem.PackageManager pkgManager, IEnumerable<string> workspaceTypes)
        {
            HashSet<TraceLabSDK.PackageSystem.IPackage> processed = new HashSet<TraceLabSDK.PackageSystem.IPackage>();

            Dictionary<string, ObservableComponentDefinitionCollection> compositeComponents = new Dictionary<string, ObservableComponentDefinitionCollection>();
            List<string> errors = new List<string>();
            foreach (TraceLabSDK.PackageSystem.IPackage package in pkgManager)
            {
                // Make sure to process the references first.
                IEnumerable<IPackageReference> missingReferences;
                var references = pkgManager.GetReferencedPackages(package, out missingReferences);

                foreach (TraceLabSDK.PackageSystem.IPackage reference in references)
                {
                    ScanPackage(pkgManager, processed, compositeComponents, workspaceTypes, errors, reference);
                }

                ScanPackage(pkgManager, processed, compositeComponents, workspaceTypes, errors, package);
            }

            foreach (KeyValuePair<string, ObservableComponentDefinitionCollection> pair in compositeComponents)
            {
                var howToAdd = new Action<MetadataDefinition>((MetadataDefinition def) => 
                {
                    if (m_packageLibrary.ContainsKey(pair.Key) == false)
                    {
                        m_packageLibrary[pair.Key] = new ObservableComponentDefinitionCollection();
                    }
                    else
                    {
                        m_packageLibrary[pair.Key].Add(def);
                        CollectFilterTypes(def);
                    }
                });

                PostProcessCompositeComponents(this, pair.Value, DataRoot, howToAdd);
            }

            if (errors.Count > 0)
            {
                if (DefinitionErrors == null)
                {
                    DefinitionErrors = new List<string>();
                }
                ((List<string>)DefinitionErrors).AddRange(errors);
            }
        }

        /// <summary>
        /// Scans the package.
        /// </summary>
        /// <param name="pkgManager">The PKG manager.</param>
        /// <param name="processed">The processed.</param>
        /// <param name="compositeComponents">The composite components.</param>
        /// <param name="workspaceTypes">The workspace types.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="package">The package.</param>
        private void ScanPackage(PackageSystem.PackageManager pkgManager, HashSet<TraceLabSDK.PackageSystem.IPackage> processed, 
            Dictionary<string, ObservableComponentDefinitionCollection> compositeComponents, IEnumerable<string> workspaceTypes,
            List<string> errors, TraceLabSDK.PackageSystem.IPackage package)
        {
            if (!processed.Contains(package))
            {
                List<string> typeLocations = new List<string>(workspaceTypes);
                typeLocations.AddRange(pkgManager.GetDependantTypeLocations(package));

                List<string> componentLocations = new List<string>();
                foreach (string location in package.ComponentLocations)
                {
                    componentLocations.Add(Path.Combine(package.Location, location));
                }

                var scanResults = ScanDirectoriesForComponents(this, componentLocations, DataRoot, typeLocations);
                errors.AddRange(scanResults.Errors);

                compositeComponents[package.ID] = scanResults.CompositeComponents;
                CollectFilterTypes(scanResults.Components);

                m_packageLibrary.Add(package.ID, scanResults.Components);
                processed.Add(package);

                //keep the map from old guids to new components guids
                m_oldGuidToNewGuidComponentsMap.AddRange(scanResults.OldGuidToNewGuidMap, true);

                //ComponentDefinitionsExporter.Export(scanResults.Components, @"c:\tmp\" + package.Name + ".txt");
            }
        }

        private Dictionary<string, ObservableComponentDefinitionCollection> m_packageLibrary = new Dictionary<string,ObservableComponentDefinitionCollection>();

        private IEnumerable<string> m_definitionErrors;
        /// <summary>
        /// Gets the definition errors.
        /// </summary>
        public IEnumerable<string> DefinitionErrors
        {
            get { return m_definitionErrors; }
            private set
            {
                m_definitionErrors = value;
                NotifyPropertyChanged("DefinitionErrors");
            }
        }

        /// <summary>
        /// Clears library
        /// </summary>
        public void Clear()
        {
            m_packageLibrary.Clear();
            m_componentDefinitions.Clear();
            m_availableFilterTypes.Clear();
            DefinitionErrors = null;
        }

        /// <summary>
        /// Clears the load errors.
        /// </summary>
        public void ClearLoadErrors()
        {
            DefinitionErrors = null;
        }

        private void RescanInternal(object context)
        {
            object[] param = (object[])context;
            IEnumerable<string> workspaceTypeDirectories = (List<string>)param[1];
            PackageSystem.PackageManager pkgManager = (PackageSystem.PackageManager)param[0];
            bool rebuildCache = (bool)param[2];

            if (this.m_packageTypeDirectories == null)
            {
                // Packages can include types, so their directories should be checked
                this.m_packageTypeDirectories = new HashSet<string>(workspaceTypeDirectories);
                // Additional types - for packaged components only
                foreach (string location in pkgManager.PackageLocations)
                {
                    var packages = pkgManager[location];
                    foreach (TraceLabSDK.PackageSystem.IPackage pkg in packages)
                    {
                        foreach (string typesLocation in pkgManager.GetDependantTypeLocations(pkg))
                        {
                            string path = Path.GetFullPath(typesLocation);
                            this.m_packageTypeDirectories.Add(path);
                        }
                    }
                }
            }

            Clear();
            try
            {
                // Loads cache from file if it has been created and at program startup (not rescanning)
                if (rebuildCache == false)
                {
                    ComponentsLibraryCache cache = ComponentsLibraryCacheHelper.LoadCacheFile();
                    if (cache != null)
                    {
                        this.m_componentFilesCache = cache;
                        this.m_componentFilesCache.CheckAppInformation(this.m_dataRoot);
                    }
                    else
                    {
                        this.m_componentFilesCache = new ComponentsLibraryCache(this.m_dataRoot);
                    }
                }
                else
                {
                    this.m_componentFilesCache.Clear();
                }

                // Read packages first
                this.m_componentFilesCache.CheckTypeDirectories(this.m_packageTypeDirectories);
                ScanPackageComponents(pkgManager, this.m_packageTypeDirectories);
                LoadComponentsDefinitions(workspaceTypeDirectories);
                MetadataDefinitionFactory.LoadConstantDefinitionsIntoLibrary(m_componentDefinitions);

                // Only update cache file if it was modified
                if (this.m_componentFilesCache.WasModified || !ComponentsLibraryCacheHelper.CacheFileExists())
                {
                    ComponentsLibraryCacheHelper.StoreCacheFile(this.m_componentFilesCache);
                }
            }
            finally
            {
                OnRescanned();
            }
        }

        private void RescanInternalNOThread(PackageSystem.PackageManager emaPkgManager,
                                         IEnumerable<string> emaWorkspaceTypeDirectories,
                                         bool emaRebuildCache)
        {
            IEnumerable<string> workspaceTypeDirectories = emaWorkspaceTypeDirectories;
            PackageSystem.PackageManager pkgManager = emaPkgManager;
            bool rebuildCache = emaRebuildCache;

            if (this.m_packageTypeDirectories == null)
            {
                // Packages can include types, so their directories should be checked
                this.m_packageTypeDirectories = new HashSet<string>(workspaceTypeDirectories);
                // Additional types - for packaged components only
                foreach (string location in pkgManager.PackageLocations)
                {
                    var packages = pkgManager[location];
                    foreach (TraceLabSDK.PackageSystem.IPackage pkg in packages)
                    {
                        foreach (string typesLocation in pkgManager.GetDependantTypeLocations(pkg))
                        {
                            string path = Path.GetFullPath(typesLocation);
                            this.m_packageTypeDirectories.Add(path);
                        }
                    }
                }
            }

            Clear();
            try
            {
                // Loads cache from file if it has been created and at program startup (not rescanning)
                if (rebuildCache == false)
                {
                    ComponentsLibraryCache cache = ComponentsLibraryCacheHelper.LoadCacheFile();
                    if (cache != null)
                    {
                        this.m_componentFilesCache = cache;
                        this.m_componentFilesCache.CheckAppInformation(this.m_dataRoot);
                    }
                    else
                    {
                        this.m_componentFilesCache = new ComponentsLibraryCache(this.m_dataRoot);
                    }
                }
                else
                {
                    this.m_componentFilesCache.Clear();
                }

                // Read packages first
                this.m_componentFilesCache.CheckTypeDirectories(this.m_packageTypeDirectories);
                ScanPackageComponents(pkgManager, this.m_packageTypeDirectories);
                LoadComponentsDefinitions(workspaceTypeDirectories);
                MetadataDefinitionFactory.LoadConstantDefinitionsIntoLibrary(m_componentDefinitions);

                // Only update cache file if it was modified
                if (this.m_componentFilesCache.WasModified || !ComponentsLibraryCacheHelper.CacheFileExists())
                {
                    ComponentsLibraryCacheHelper.StoreCacheFile(this.m_componentFilesCache);
                }
            }
            finally
            {
                OnRescanned();
            }
        }


        private System.Threading.Thread m_rescanThread;
        public void Rescan(PackageSystem.PackageManager pkgManager, IEnumerable<string> workspaceTypeDirectories, bool rebuildCache)
        {
            if (m_rescanThread != null)
            {
                m_rescanThread.Abort();
                m_rescanThread.Join();
            }

            // If we're not currently rescanning, then notify that we're starting a rescan, otherwise the old one will just be aborted and this one will
            // finish in its due course.
            if (IsRescanning == false)
            {
                OnRescanning();
            }

            RescanInternalNOThread (pkgManager, workspaceTypeDirectories, rebuildCache);


            Thread thread = ThreadFactory.CreateThread(new System.Threading.ParameterizedThreadStart(RescanInternal));
            thread.IsBackground = true;
            thread.Priority = System.Threading.ThreadPriority.Highest;// BelowNormal;
            thread.Name = "Component Library Rescan Thread";
       //     thread.Start(new Object[] { pkgManager, workspaceTypeDirectories, rebuildCache });

      //      m_rescanThread = thread;
        }

        #region LockRescanning

        /// <summary>
        /// Corresponds to number of experiments that are currently running.
        /// </summary>
        private int m_rescanLockRefcount;
        private object rescanSyncLock = new object();

        internal void LockRescanning()
        {
            lock (rescanSyncLock)
            {
                m_rescanLockRefcount++;
                CanRescan = false;
            }
        }

        internal void UnLockRescanning()
        {
            lock (rescanSyncLock)
            {
                m_rescanLockRefcount--;
                if (m_rescanLockRefcount == 0)
                {
                    CanRescan = true; //once there is no more running experiments, set rescan back to true
                }
            }
        }

        private bool m_canRescan = true;

        /// <summary>
        /// Gets or sets a value indicating whether this component library can be rescanned.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can rescan; otherwise, <c>false</c>.
        /// </value>
        public bool CanRescan
        {
            get { return m_canRescan; }
            set
            {
                if (m_canRescan != value)
                {
                    m_canRescan = value;
                    NotifyPropertyChanged("CanRescan");
                }
            }
        }

        #endregion

        /// <summary>
        /// Sorts the components list using topological sorting algorithm. In returns the queue of composite components definition, with the order
        /// in each the components should load their components graphs. 
        /// The composite components graphs that have unresolved dependency (ie. they are depended on the composite components that are not existing in the library)
        /// are not included in the queue. Thus they won't be added to the library.
        /// </summary>
        /// <param name="unsortedComponentsList">The unsorted components list.</param>
        /// <returns></returns>
        private static Queue<CompositeComponentMetadataDefinition> TopologicalSort(ObservableComponentDefinitionCollection componentsDefCollection)
        {
            Queue<CompositeComponentMetadataDefinition> componentsDefinitionsWithNoDependency;
            ComponentDependencyHierarchy dependencyHierarchy = BuildDependencyHierarchy(componentsDefCollection, out componentsDefinitionsWithNoDependency);

            //once dependency of each component is ready, the components can be sorted

            //init the list that will contain components in order first in first out
            Queue<CompositeComponentMetadataDefinition> sortedComponents = new Queue<CompositeComponentMetadataDefinition>();

            //until list of components with no dependency is not empty continue sorting
            while (componentsDefinitionsWithNoDependency.Count > 0)
            {
                CompositeComponentMetadataDefinition currentComponentDefinition = componentsDefinitionsWithNoDependency.Dequeue();

                //add current component to the queue
                sortedComponents.Enqueue(currentComponentDefinition);

                //get components that depends on the currentComponentDefinition
                IEnumerable<DependencyLink> dependencyLinks;
                if (dependencyHierarchy.TryGetInEdges(currentComponentDefinition, out dependencyLinks))
                {
                    //create a shallow copy of the dependencyLinks, so that we can modify the dependencyHierarchy (otherwise, if iterated directly,it would throw exception with error that collection was modified
                    IEnumerable<DependencyLink> copyOfDependencyLinks = new List<DependencyLink>(dependencyLinks);
                    //if there are any dependants, remove that links from dependency graph
                    foreach (DependencyLink link in copyOfDependencyLinks)
                    {
                        dependencyHierarchy.RemoveEdge(link);

                        //check if the source component (the component that depended on the currentComponentDefinition) has other dependencies
                        //if not enqueue it to the list of componentsDefinitionsWithNoDependency
                        if (dependencyHierarchy.IsOutEdgesEmpty(link.Source))
                        {
                            componentsDefinitionsWithNoDependency.Enqueue(link.Source);
                        }
                    }
                }
            }

            return sortedComponents;
        }

        /// <summary>
        /// Builds the dependency hierarchy from collection of composite components.
        /// It also discovers components that they don't have dependency and adds them to the queue.
        /// </summary>
        /// <param name="componentsDefCollection">The components def collection.</param>
        /// <param name="componentsDefinitionsWithNoDependency">Outputs the queue of components definitions with no dependency.</param>
        /// <returns>Returns the graph of dependencies between components. </returns>
        private static ComponentDependencyHierarchy BuildDependencyHierarchy(
                                                        ObservableComponentDefinitionCollection componentsDefCollection,
                                                        out Queue<CompositeComponentMetadataDefinition> componentsDefinitionsWithNoDependency)
        {
            componentsDefinitionsWithNoDependency = new Queue<CompositeComponentMetadataDefinition>();

            //prepare the collection of dependencies
            ComponentDependencyHierarchy dependencyHierarchy = new ComponentDependencyHierarchy();

            //build dependency hierarch
            foreach (CompositeComponentMetadataDefinition dependentComponent in componentsDefCollection)
            {
                //gets the list of components that this dependentComponent depends on (referred later as independentComponents)
                List<string> dependency = dependentComponent.DetermineDependency();

                //if the component is not depended on any other composite component add it to the queue of totally independent components
                if (dependency.Count == 0)
                {
                    componentsDefinitionsWithNoDependency.Enqueue(dependentComponent);
                    continue; //there is no dependency links coming from this component
                }

                // for each component in dependency add it to the hierarchy of dependency
                foreach (string compositeComponentId in dependency)
                {
                    //check if componentsDefCollection contains such component
                    if (componentsDefCollection.Contains(compositeComponentId))
                    {
                        CompositeComponentMetadataDefinition independentComponent = (CompositeComponentMetadataDefinition)componentsDefCollection[compositeComponentId];

                        if (dependencyHierarchy.ContainsEdge(dependentComponent, independentComponent) == false)
                        {
                            dependencyHierarchy.AddVerticesAndEdge(new DependencyLink(dependentComponent, independentComponent));
                        }
                    }
                    else
                    {
                        //mark that components cannot be satisfied, because the component is not in the list of loaded definitions
                        //thus its component graph would not be possible to load (it would crash application)
                        //and this composite component definition should not be added to library
                        NLog.LogManager.GetCurrentClassLogger().Warn(String.Format(Messages.CompositeComponentDependencyNotFound, dependentComponent.Label, dependentComponent.ID, compositeComponentId));
                    }
                }
            }
            return dependencyHierarchy;
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }

        #endregion
    }

    #region Helper structures

    internal class ComponentDependencyHierarchy : QuickGraph.BidirectionalGraph<CompositeComponentMetadataDefinition, DependencyLink>
    {

    }

    internal class DependencyLink : QuickGraph.IEdge<CompositeComponentMetadataDefinition>
    {
        public DependencyLink(CompositeComponentMetadataDefinition source, CompositeComponentMetadataDefinition target)
        {
            m_source = source;
            m_target = target;
        }

        #region IEdge<MockNode> Members

        private CompositeComponentMetadataDefinition m_source;
        public CompositeComponentMetadataDefinition Source
        {
            get { return m_source; }
        }

        private CompositeComponentMetadataDefinition m_target;
        public CompositeComponentMetadataDefinition Target
        {
            get { return m_target; }
        }

        #endregion
    }

    #endregion
}

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
using System.Reflection;
using TraceLabSDK;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Scans a set of directories in search for the components, both primitive components found in assemblies, and .teml composite components.
    /// 
    /// The Component Scanner is responsible for finding all components definitions. It scans all components directories defined in application 
    /// settings, and components directories in installed packages. In each directory it searches for assemblies (.dll files), loads each of them, 
    /// and finds classes with Component attribute. For each class it creates metadata definition instance with all the information about such component: 
    /// its assembly, classname, name of a component, author, descritpion, input/output specification, Configuration specification; 
    /// in case of composite components it loads the subgraph experiment.
    /// 
    /// Once such metadata definitions are stored, the component scanner is closed and all components assemblies are unloaded. 
    /// More specifically, scanning is done in secondary app domain, therefore all assemblies are loaded into it, 
    /// and once scanning is completed the app domain is destroyed. Therefore, it is possible to Rescan library, 
    /// and recompile components without a need to close TraceLab each time there is newly compiled assembly. 
    /// </summary>
    class ComponentScanner : MarshalByRefObject
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="ComponentScanner"/> class from being created.
        /// </summary>
        private ComponentScanner()
        {
            Errors = new List<string>();

            NewFilesLoaded = new List<FileDescriptor>();
            FilesToGetFromCache = new List<string>();
        }

        #region Private Properties

        /// <summary>
        /// Gets or sets the container for errors that occured during scan
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        private List<string> Errors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container for the new files found during scan
        /// </summary>
        /// <value>
        /// The new files.
        /// </value>
        private List<FileDescriptor> NewFilesLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container for the files needed to be retrieved from cache
        /// </summary>
        /// <value>
        /// The new files.
        /// </value>
        private List<string> FilesToGetFromCache
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Scans the specified components directory path.
        /// HashSet serialization in mono 2.10.6 on linux is not implemented (and it is required for crossappdomain calls)
        /// It had to be replaced by another collection.
        /// It has been implemented in mono 2.10.8, but it has not yet been released on opensuse!
        /// </summary>
        /// <param name="componentsDirectoryPath">The components directory path.</param>
        /// <param name="cachedFilesList">The cached files list.</param>
        private void Scan(IEnumerable<string> componentsDirectoryPath, List<string> cachedFilesList)
        {
            HashSet<string> cachedFiles = new HashSet<string>(cachedFilesList);
            Scan(componentsDirectoryPath, cachedFiles);
        }

        /// <summary>
        /// Scans the specified directories for composite and regular components.
        /// </summary>
        /// <param name="componentsDirectoryPath">Paths of all directories to be scanned.</param>
        /// <param name="cachedFiles">Paths of files already stored in the cache.</param>
        private void Scan(IEnumerable<string> componentsDirectoryPath, ISet<string> cachedFiles)
        {
            //scan first for all primitive components definitions
            ScanForPrimitiveComponents(componentsDirectoryPath, cachedFiles);

            //secondly scan for all composite components definitions
            ScanForCompositeComponents(componentsDirectoryPath, cachedFiles);
        }

        /// <summary>
        /// Scans the specified directories for primitive components.
        /// </summary>
        /// <param name="componentsDirectoryPath">Directories to be scanned.</param>
        /// <param name="cachedFiles">Paths of files already stored in the cache.</param>
        private void ScanForPrimitiveComponents(IEnumerable<string> componentsDirectoryPath, ISet<string> cachedFiles)
        {
            var pathSet = new HashSet<string>(componentsDirectoryPath);

            foreach (string dir in pathSet)
            {
                string[] filePaths = GetAssemblyFiles(dir);

                if (filePaths == null)
                {
                    continue;
                }

                foreach (string comp in filePaths)
                {
                    string fullPath = Path.GetFullPath(comp);

                    if (cachedFiles.Contains(fullPath))
                    {
                        FilesToGetFromCache.Add(fullPath);
                    }
                    else
                    {
                        AssemblyFileDescriptor assembly = ScanAssembly(fullPath);
                        if (assembly != null)
                        {
                            NewFilesLoaded.Add(assembly);
                            FilesToGetFromCache.Add(fullPath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Locates the assembly files in the given directory. In other words it searches the directory for all files with extension .dll
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">If directory was not found</exception>
        /// <returns>array of file paths to the assembly files</returns>
        private string[] GetAssemblyFiles(string dir)
        {
            return GetFiles(dir, "*.dll");
        }

        /// <summary>
        /// Scans the given assembly in search for component definitions.
        /// In case there are some exception the errors are being collected.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns>the assembly desriptor with collection of correct MetadataDefinition for all components found inside the assembly. </returns>
        private AssemblyFileDescriptor ScanAssembly(string assemblyPath)
        {
            AssemblyFileDescriptor newAssemblFile = null;

            try
            {
                AssemblyFileDescriptor assemblyFile = new AssemblyFileDescriptor(assemblyPath);
               /* string assemblyPath2 = System.Reflection.Assembly.GetExecutingAssembly().Location;
                String assemblyPath3 = @"C:\Users\cebernalc\Documents\GitHub\TraceLab\Main\WebService\Bin\SEMERU.Core.dll";
                String assemblyPath4 = @"C:\Users\cebernalc\Documents\GitHub\TraceLab\Main\WebService\Bin\SEMERU.Components.dll";
                String assemblyPath5 = @"C:\Users\cebernalc\Documents\GitHub\TraceLab\Main\WebService\Bin\SEMERU.Types.dll";
                String assemblyPath6 = @"C:\Users\cebernalc\Documents\GitHub\TraceLab\Main\WebService\Bin\TraceLabSDK.dll";
                String assemblyPath7 = @"C:\Users\cebernalc\Documents\GitHub\TraceLab\Main\WebService\Bin\TraceLabSDK.Types.dll";
                String assemblyPath8 = @"C:\Users\cebernalc\Documents\GitHub\TraceLab\Main\WebService\Bin\TraceLab.Core.dll";*/




                var assm = Assembly.LoadFrom(assemblyPath);
                /*var assm2 = Assembly.LoadFrom(assemblyPath2);
                var assm3 = Assembly.LoadFrom(assemblyPath3);
                var assm4 = Assembly.LoadFrom(assemblyPath4);
                var assm5 = Assembly.LoadFrom(assemblyPath5);
                var assm6 = Assembly.LoadFrom(assemblyPath6);
                var assm7 = Assembly.LoadFrom(assemblyPath7);
                var assm8 = Assembly.LoadFrom(assemblyPath8);

                _AssemblyName[] references3 = assm3.GetReferencedAssemblies();
                _AssemblyName [] references4 = assm4.GetReferencedAssemblies();



                 var types2 = assm2.GetExportedTypes();
                 var types3 = assm3.GetExportedTypes();
                 var types4 = assm4.GetExportedTypes();*/
                //var assm = Assembly.UnsafeLoadFrom(assemblyPath);
                //var assm = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                //var assm = Assembly.LoadFrom(assemblyPath);
                //get all types from the assembly
                var types = assm.GetExportedTypes();
                foreach (var exportedType in types)
                {
                    try
                    {
                        //check the type for the component attribute
                        CheckType(assemblyPath, exportedType, assemblyFile);
                    }
                    catch (Exception e)
                    {
                        var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                        message = string.Format("Unable to load component '{0}' in assemblyFile {1}: {2}", exportedType.FullName, assemblyPath, message);
                        Errors.Add(message);
                    }
                }
                //even though there could be some errors when loading some components, some other components may be correct,
                //thus ignore the flag of noErrors. Previously even if just one component failed to load from assembly it didn't show any components
                //from the given assembly
                newAssemblFile = assemblyFile;
                
            }
            catch (Exception e)
            {
                // Catch everything, just log
                // Basically, if an exception is thrown, then this is not a valid Component.
                newAssemblFile = null;
                string message = e.InnerException != null ? e.InnerException.Message : e.Message;
                message = string.Format("Unable to load assemblyFile {0}: {1}", assemblyPath, message);
                Errors.Add(message);
            }

            return newAssemblFile;
        }

        /// <summary>
        /// Checks the type for existence of Component Attribute. If type has Component Attribute it creates 
        /// component metadata definition and adds it to the assembly file descriptor.
        /// If Component Attribute was found, but was in incorrect format error is reported. 
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="exportedType">Type of the exported.</param>
        /// <param name="assemblyFile">The assembly file.</param>
        private void CheckType(string assemblyPath, Type exportedType, AssemblyFileDescriptor assemblyFile)
        {
            var attribs = exportedType.GetCustomAttributes(typeof(ComponentAttribute), false);
            if (attribs.Length == 1)
            {
                try
                {
                    var attrib = (ComponentAttribute)attribs[0];
                    var ioSpecDefinition = ComponentScannerHelper.ReadIOSpec(exportedType);
                    var configurationWrapperDefinition = ComponentScannerHelper.CreateConfigWrapperDefinition(attrib.ConfigurationType);
                    var componentId = ComponentScannerHelper.CreateComponentId(attrib.Name, ioSpecDefinition, attrib.Version, configurationWrapperDefinition);

                    ComponentTags tags = new ComponentTags(componentId);
                    var tagAttribs = exportedType.GetCustomAttributes(typeof(TagAttribute), false);
                    foreach (TagAttribute tag in tagAttribs)
                    {
                        tags.SetTag(tag.Tag, false);
                    }

                    //name participates in guid generation, and is assign as default label, unless other default label has been specified
                    string defaultLabel = (String.IsNullOrWhiteSpace(attrib.DefaultLabel)) ? attrib.Name : attrib.DefaultLabel;

                    //gather all documentation links
                    List<DocumentationLink> documentationLinks = new List<DocumentationLink>();
                    var linkAttribs = exportedType.GetCustomAttributes(typeof(LinkAttribute), false);
                    foreach (LinkAttribute link in linkAttribs)
                    {
                        Uri url;
                        if (Uri.TryCreate(link.Url, UriKind.Absolute, out url))
                        {
                            documentationLinks.Add(new DocumentationLink(url, link.Title, link.Description, link.Order));
                        }
                        else
                        {
                            NLog.LogManager.GetCurrentClassLogger().Warn(
                                String.Format("Documentation link url '{0}' for component '{1}' could not be processed correctly and has been ommitted", link.Url , defaultLabel));
                        }
                    }

                    var compDef = new ComponentMetadataDefinition(componentId,
                                                        assemblyPath,
                                                        exportedType.FullName,
                                                        ioSpecDefinition,
                                                        defaultLabel,
                                                        attrib.Version,
                                                        attrib.Description,
                                                        attrib.Author,
                                                        attrib.IsJava ? Language.JAVA : Language.NET,
                                                        configurationWrapperDefinition,
                                                        tags,
                                                        documentationLinks);

                    assemblyFile.MetadataCollection.Add(compDef);

                    //if old manual guid has been set, add it to the map from new autogenerated guid to old guid
                    if (attrib.GuidIDString != null && attrib.GuidID != null)
                    {
                        string oldGuidID = attrib.GuidID.ToString();
                        if (!assemblyFile.NewGuidToOldGuid.ContainsValue(oldGuidID))
                        {
                            assemblyFile.NewGuidToOldGuid.Add(componentId, oldGuidID);
                        }
                    }
                }
                catch (Exceptions.ComponentsLibraryException ex)
                {
                    Errors.Add(String.Format("Potential component type {0} located in assembly {1} has invalid ComponentAttribute: {2}", exportedType, assemblyPath, ex.Message));
                }
            }
            else if (1 < attribs.Length)
            {
                var errorMsg = "ERROR: Somehow there are more than one ComponentAttribute instances on type " + exportedType.FullName;
                Errors.Add(errorMsg);
            }
        }

        /// <summary>
        /// Scans the specified directories for composite components.
        /// </summary>
        /// <param name="componentsDirectoryPath">Directories to be scanned.</param>
        /// <param name="cachedFiles">Paths of files already stored in the cache.</param>
        private void ScanForCompositeComponents(IEnumerable<string> componentsDirectoryPath, ISet<string> cachedFiles)
        {
            var pathSet = new HashSet<string>(componentsDirectoryPath);
            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(CompositeComponentMetadataDefinition), null);

            foreach (var dir in pathSet)
            {
                string[] filePaths = GetCompositeComponentFiles(dir);

                if (filePaths == null)
                {
                    continue;
                }

                foreach (var filename in filePaths)
                {
                    string fullPath = Path.GetFullPath(filename);

                    if (cachedFiles.Contains(fullPath))
                    {
                        FilesToGetFromCache.Add(fullPath);
                    }
                    else
                    {
                        CompositeComponentFileDescriptor compFile = ReadCompositeComponentFile(serializer, filename);
                        if (compFile != null)
                        {
                            NewFilesLoaded.Add(compFile);
                            FilesToGetFromCache.Add(fullPath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads the composite component file. It deserializes the xml file of composite component. 
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private CompositeComponentFileDescriptor ReadCompositeComponentFile(XmlSerializer serializer, string filename)
        {
            CompositeComponentFileDescriptor file = null;

            try
            {
                var reader = XmlReader.Create(filename);
                var compositeComponentDefinition = (CompositeComponentMetadataDefinition)serializer.Deserialize(reader);
                compositeComponentDefinition.Assembly = filename;

                file = new CompositeComponentFileDescriptor(filename, compositeComponentDefinition);
            }
            catch (Exception e)
            {
                // Catch everything, just log
                // Basically, if an exception is thrown, then this is not a valid Component.
                file = null;
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                message = string.Format("Unable to load composite component {0}: {1}", filename, message);
                Errors.Add(message);
            }

            return file;
        }

        /// <summary>
        /// Locates the composite component files. In other words it searches directory for all files with extenstion .tcml
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        private string[] GetCompositeComponentFiles(string dir)
        {
            return GetFiles(dir, "*.tcml");
        }

        #endregion

        /// <summary>
        /// Locates the files in the given directory matching given pattern. 
        /// In case directory was not found the error is collected.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns>array of file paths to the assembly files</returns>
        private string[] GetFiles(string dir, string searchPattern)
        {
            string[] filePaths = null;
            try
            {
                filePaths = System.IO.Directory.GetFiles(dir, searchPattern);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                // Catch everything, just log
                // Basically, if an exception is thrown, then this is not a valid Component.
                string message = e.InnerException != null ? e.InnerException.Message : e.Message;
                message = string.Format("Directory does not exist: {0}", message);
                Errors.Add(message);
            }
            return filePaths;
        }

        /// <summary>
        /// Scans the specified directories for components not already present in cache.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <param name="cachedFiles">Components already loaded in the cache.</param>
        /// <param name="results">.</param>
        /// <returns>
        /// True or False depending on the success of the scanning operation.
        /// </returns>
        public static bool Scan(IEnumerable<string> directories, IEnumerable<string> workspaceTypeDirectories,
            ISet<string> cachedFiles, ComponentScanResults results)
        {
            bool success = true;

            AppDomain newDomain = null;

            try
            {
                var libraryHelper = new LibraryHelper(workspaceTypeDirectories);

                newDomain = libraryHelper.CreateDomain("ComponentScanner", false);
                newDomain.Load(Assembly.GetExecutingAssembly().GetName());

                // Preload the workspace types so that the component scanner won't barf when a component references a workspace type.
                libraryHelper.PreloadWorkspaceTypes(newDomain);

                ComponentScanner scanner = (ComponentScanner)newDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ComponentScanner).FullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null);

                if (!RuntimeInfo.IsRunInMono)
                {
                    scanner.Scan(directories, cachedFiles);
                }
                else
                {
                    scanner.Scan(directories, new List<string>(cachedFiles));
                }

                results.NewFilesLoaded = new List<FileDescriptor>(scanner.NewFilesLoaded);
                results.RequestedFiles = new List<string>(scanner.FilesToGetFromCache);

                results.Errors = new List<string>(scanner.Errors);
            }
            catch (Exception e)
            {
                success = false;

                results.NewFilesLoaded = new List<FileDescriptor>();
                results.RequestedFiles = new List<string>();
                results.OldGuidToNewGuidMap = new Dictionary<string, string>();

                results.Errors = results.Errors ?? new List<string>();
                results.Errors.Add(e.Message);
            }
            finally
            {
                if (newDomain != null)
                {
#if !MONO_DEV
                    //appdomain unload crashes mono.exe process when running directly from 
                    //Mono Develop, possibly because attached debugger. When run normally works fine.
                    //use MONO_DEV compiler symbol when developing
                    AppDomain.Unload(newDomain);
#endif
                }
            }

            return success;
        }
    }
}

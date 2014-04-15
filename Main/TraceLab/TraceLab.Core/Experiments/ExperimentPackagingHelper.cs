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
using System.IO;
using TraceLab.Core.Components;
using TraceLabSDK.PackageSystem;
using System.ComponentModel;
using System.Linq;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Configuration for packaging experiments (determines which files/directories are added to the package)
    /// </summary>
    public class ExperimentPackageConfig : INotifyPropertyChanged
    {
        public ExperimentPackageConfig()
        {
            this.IncludeOtherPackagesAssemblies = false;
            this.IncludeOtherPackagesFilesDirs = false;
            this.IncludeIndependentFilesDirs = true;
        }

        private bool m_includeOtherPackagesAssemblies;

        /// <summary>
        /// Gets or sets a value indicating whether package should include other packages assemblies.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [include other packages assemblies]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeOtherPackagesAssemblies
        {
            get { return m_includeOtherPackagesAssemblies; }
            set 
            {
                if (m_includeOtherPackagesAssemblies != value)
                {
                    m_includeOtherPackagesAssemblies = value;
                    NotifyPropertyChanged("IncludeOtherPackagesAssemblies");
                }
            }
        }

        private bool m_includeOtherPackagesFilesDirs;

        /// <summary>
        /// Gets or sets a value indicating whether package should include other packages files and directories.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [include other packages files dirs]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeOtherPackagesFilesDirs
        {
            get { return m_includeOtherPackagesFilesDirs; }
            set 
            {
                if (m_includeOtherPackagesFilesDirs != value)
                {
                    m_includeOtherPackagesFilesDirs = value;
                    NotifyPropertyChanged("IncludeOtherPackagesFilesDirs");
                }
            }
        }

        private bool m_includeIndependentFilesDirs;

        /// <summary>
        /// Gets or sets a value indicating whether package should include independent files and directories
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [include independent files dirs]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeIndependentFilesDirs
        {
            get { return m_includeIndependentFilesDirs; }
            set 
            { 
                if (m_includeIndependentFilesDirs != value)
                {
                    m_includeIndependentFilesDirs = value;
                    NotifyPropertyChanged("IncludeIndependentFilesDirs");
                }
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }

    /// <summary>
    /// Structure that holds the results of the experiment packaging
    /// </summary>
    public class ExperimentPackagingResults
    {
        public IExperiment Experiment;
        public List<string> TypeAssemblies;
        public List<string> ComponentAssemblies;
        public List<PackageFileInfo> Files;
        public List<PackageFileInfo> Directories;

        public ExperimentPackagingResults(IExperiment pExperiment)
        {
            this.Experiment = pExperiment;
            this.TypeAssemblies = new List<string>();
            this.ComponentAssemblies = new List<string>();
            this.Files = new List<PackageFileInfo>();
            this.Directories = new List<PackageFileInfo>();
        }
    }

    public class PackageFileInfo
    {
        public PackageFileInfo(TraceLabSDK.Component.Config.BasePath filePath)
        {
            //1. First record current absolute path to real file -> this file is going to be added to package
            m_absoluteLocation = Path.GetFullPath(filePath.Absolute);
            
            //2. Determine new relative directory by eliminating '../../' -> it has to be subdirectory, so we can add it to the package.
            m_relativeLocation = DetermineRelativePathToSubDir(filePath.Relative, out m_foldersPath);
        }

        /// <summary>
        /// Determines the relative path to sub dir. It eliminates relation to upper folders by eliminating '../../' 
        /// It also collects sub Folders into linked list - so that we can create them in the package builder
        /// 
        /// In other words, if current relative path is:
        /// Input:         Output:
        /// ../../A.xml     -> A.xml
        /// ../Input/B.xml  -> Input/B.xml
        /// MyInput/C.xml   -> MyInput/C.xml
        /// </summary>
        /// <param name="relative">The relative path.</param>
        /// <returns>New relative path to a subfolder</returns>
        private static string DetermineRelativePathToSubDir(string relative, out LinkedList<string> foldersPath)
        {
            foldersPath = new LinkedList<string>();

            char dirSeparatorChar = System.IO.Path.DirectorySeparatorChar;

            string[] directories = relative.Split(dirSeparatorChar);

            System.Text.StringBuilder pathBuilder = new System.Text.StringBuilder();

            int count = 0;
            foreach(string dir in directories)
            {
                if (dir != "..")
                {
                    //skip last, as it is a filename, or final directory to be added in the view
                    if (count < directories.Length - 1)
                    {
                        foldersPath.AddLast(dir);
                    }
                    pathBuilder.Append(dir + dirSeparatorChar);
                }
                count++;
            }

            pathBuilder.Remove(pathBuilder.Length - 1, 1);

            return pathBuilder.ToString();
        }

        private string m_relativeLocation;

        /// <summary>
        /// Gets the relative location
        /// </summary>
        public string RelativeLocation
        {
            get { return m_relativeLocation; }
        }

        private string m_absoluteLocation;
        /// <summary>
        /// Gets the absolute location to the actual file
        /// </summary>
        public string AbsoluteLocation
        {
            get { return m_absoluteLocation; }
        }

        private LinkedList<string> m_foldersPath;

        /// <summary>
        /// Gets the folders path; each element contains the name of the folder in the path to the file
        /// </summary>
        public LinkedList<string> FoldersPath
        {
            get { return m_foldersPath; }
        }
    }

    /// <summary>
    /// Helper class for extracting data (types/components assemblies, files & directories)
    /// from an experiment.
    /// </summary>
    public class ExperimentPackagingHelper
    {
        #region Properties

        /// <summary>
        /// Dictionary of supported types in the workspace.
        /// </summary>
        private Dictionary<string, Type> m_supportedTypes;

        /// <summary>
        /// Set of assembly files for types used by the experiment.
        /// </summary>
        private HashSet<string> m_typeAssemblies;

        /// <summary>
        /// Set of assembly files for components used by the experiment.
        /// </summary>
        private HashSet<string> m_componentAssemblies;

        /// <summary>
        /// Set of files used by the experiment components.
        /// </summary>
        private HashSet<PackageFileInfo> m_files;

        /// <summary>
        /// Set of directories used by the experiment components.
        /// </summary>
        private HashSet<PackageFileInfo> m_directories;

        /// <summary>
        /// Configuration of experiment packaging
        /// </summary>
        private ExperimentPackageConfig m_config;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentPackagingHelper"/> class.
        /// </summary>
        /// <param name="pSupportedTypes">The p supported types.</param>
        public ExperimentPackagingHelper(Dictionary<string, Type> pSupportedTypes)
        {
            this.m_typeAssemblies = new HashSet<string>();
            this.m_componentAssemblies = new HashSet<string>();
            this.m_files = new HashSet<PackageFileInfo>();
            this.m_directories = new HashSet<PackageFileInfo>();

            this.m_supportedTypes = pSupportedTypes;
        }

        public ExperimentPackagingHelper(ExperimentPackageConfig pConfig, Dictionary<string, Type> pSupportedTypes)
        {
            this.m_typeAssemblies = new HashSet<string>();
            this.m_componentAssemblies = new HashSet<string>();
            this.m_files = new HashSet<PackageFileInfo>();
            this.m_directories = new HashSet<PackageFileInfo>();

            this.m_supportedTypes = pSupportedTypes;
            this.m_config = pConfig;
        }

        /// <summary>
        /// Goes through all the vertices in the experiment graph and extracts the components/types assemblies
        /// and other files/directories used.
        /// </summary>
        /// <param name="pVertices">Collection of vertices in the experiment graph.</param>
        public ExperimentPackagingResults PackExperiment(Experiment experiment)
        {
            foreach (ExperimentNode vertex in experiment.Vertices)
            {
                ExtractFilesFromNode(vertex);
            }

            ExperimentPackagingResults results = new ExperimentPackagingResults(experiment);
            results.TypeAssemblies = new List<string>(this.m_typeAssemblies);
            results.ComponentAssemblies = new List<string>(this.m_componentAssemblies);
            results.Files = new List<PackageFileInfo>(this.m_files);
            results.Directories = new List<PackageFileInfo>(this.m_directories);

            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(experiment.ExperimentInfo.FilePath));
            ExperimentManager.SaveAs(experiment, tempPath, ReferencedFiles.IGNORE);

            return results;
        }

        /// <summary>
        /// Extracts the components/types assemblies from the given vertex.
        /// </summary>
        /// <param name="pVertex">Vertex from the experiment graph.</param>
        private void ExtractFilesFromNode(ExperimentNode pVertex)
        {
            if (pVertex is ComponentNode) // Regular component
            {
                ComponentMetadata metaData = (ComponentMetadata)pVertex.Data.Metadata;
                if (metaData != null)
                {
                    ComponentMetadataDefinition metaDataDef = metaData.ComponentMetadataDefinition;
                    if (metaDataDef != null)
                    {
                        bool include;
                        if (m_config.IncludeOtherPackagesAssemblies)
                        {
                            include = true;
                        }
                        else
                        {
                            //determine if component is independent or is coming from another package
                            include = !IsAssemblyInAnotherPackage(metaDataDef.Assembly);
                        }

                        if (include)
                        {
                            // Component assembly
                            this.m_componentAssemblies.Add(Path.GetFullPath(metaDataDef.Assembly));

                            // Extracting types from IOSpec
                            ExtractTypesFromIOSpec(metaDataDef.IOSpecDefinition);
                        }
                    }

                    ConfigWrapper config = metaData.ConfigWrapper;
                    if (config != null)
                    {
                        // Extracting paths for files/directories from components' configuration
                        foreach (ConfigPropertyObject configValue in config.ConfigValues.Values)
                        {
                            // Files
                            if (configValue.Type == "TraceLabSDK.Component.Config.FilePath" && configValue.Value != null)
                            {
                                // Independent files
                                if (configValue.Value is TraceLab.Core.Components.TraceLabFilePath == false)
                                {
                                    if (this.m_config.IncludeIndependentFilesDirs)
                                    {
                                        var filePath = (TraceLabSDK.Component.Config.FilePath)configValue.Value;
                                        if (File.Exists(filePath.Absolute))
                                        {
                                            PackageFileInfo packageFileInfo = new PackageFileInfo(filePath);
                                            m_files.Add(packageFileInfo);

                                            //Change config value relative path -> this path is saved within experiment (note, we operate on the experiment clone)
                                            //so that relative path is in the subfolder of Experiment folder
                                            filePath.Relative = packageFileInfo.RelativeLocation;
                                        }
                                    }
                                }
                                // Files contained in a package
                                else
                                {
                                    if (this.m_config.IncludeOtherPackagesFilesDirs)
                                    {
                                        //TraceLabFilePath represents the file reference located in the package
                                        var packageFilePath = (TraceLabFilePath)configValue.Value;
                                        if (File.Exists(packageFilePath.Absolute))
                                        {
                                            PackageFileInfo packageFileInfo = new PackageFileInfo(packageFilePath);
                                            m_files.Add(packageFileInfo);

                                            //change the configValue to basic FilePath, (not TraceLabFilePath in the package), with updated Relative Path
                                            //so that relative path is in the subfolder of Experiment folder
                                            TraceLabSDK.Component.Config.FilePath basicFilePath = new TraceLabSDK.Component.Config.FilePath();
                                            basicFilePath.Init(packageFilePath.Absolute, packageFilePath.DataRoot);
                                            basicFilePath.Relative = packageFileInfo.RelativeLocation;
                                            configValue.Value = basicFilePath;
                                        }
                                    }
                                }
                            }
                            // Directories
                            else if (configValue.Type == "TraceLabSDK.Component.Config.DirectoryPath" && configValue.Value != null)
                            {
                                // Independent directories
                                if (configValue.Value is TraceLab.Core.Components.TraceLabDirectoryPath == false)
                                {
                                    if (this.m_config.IncludeIndependentFilesDirs)
                                    {
                                        var dirPath = (TraceLabSDK.Component.Config.DirectoryPath)configValue.Value;
                                        if (Directory.Exists(dirPath.Absolute))
                                        {
                                            PackageFileInfo packageDirectoryInfo = new PackageFileInfo(dirPath);
                                            m_directories.Add(packageDirectoryInfo);

                                            //Change config value relative path -> this path is saved within experiment (note, we operate on the experiment clone)
                                            //so that relative path is in the subfolder of Experiment folder
                                            dirPath.Relative = packageDirectoryInfo.RelativeLocation;
                                        }
                                    }
                                }
                                // Directories contained in a package
                                else
                                {
                                    if (this.m_config.IncludeOtherPackagesFilesDirs)
                                    {
                                        var packageDirPath = (TraceLabDirectoryPath)configValue.Value;
                                        if (Directory.Exists(packageDirPath.Absolute))
                                        {
                                            PackageFileInfo packageDirInfo = new PackageFileInfo(packageDirPath);
                                            m_directories.Add(packageDirInfo);

                                            //change the configValue to basic FilePath, (not TraceLabFilePath in the package), with updated Relative Path
                                            //so that relative path is in the subfolder of Experiment folder
                                            TraceLabSDK.Component.Config.DirectoryPath basicFilePath = new TraceLabSDK.Component.Config.DirectoryPath();
                                            basicFilePath.Init(packageDirPath.Absolute, packageDirPath.DataRoot);
                                            basicFilePath.Relative = packageDirInfo.RelativeLocation;
                                            configValue.Value = basicFilePath;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (pVertex is CompositeComponentNode) // Composite Components, Loops & Scopes
            {
                CompositeComponentBaseMetadata metaData = (CompositeComponentBaseMetadata)pVertex.Data.Metadata;
                if (metaData != null)
                {
                    foreach (var vertex in metaData.ComponentGraph.Vertices)
                    {
                        ExtractFilesFromNode(vertex);
                    }

                    // Only composite components have MetadataDefinition
                    if (pVertex.Data.Metadata is CompositeComponentMetadata)
                    {
                        CompositeComponentMetadataDefinition metaDataDefinition = ((CompositeComponentMetadata)pVertex.Data.Metadata).ComponentMetadataDefinition;

                        this.m_componentAssemblies.Add(Path.GetFullPath(metaDataDefinition.Assembly));

                        // Extracting types from IOSpec
                        ExtractTypesFromIOSpec(metaDataDefinition.IOSpecDefinition);
                    }
                }
            }
        }

        /// <summary>
        /// Extracts types assemblies from the given IOSpecDefinition.
        /// </summary>
        /// <param name="pIOSpecDefinition">IO Spec Definition of a component.</param>
        private void ExtractTypesFromIOSpec(IOSpecDefinition pIOSpecDefinition)
        {
            CollectTypeAssemblies(pIOSpecDefinition.Input);
            CollectTypeAssemblies(pIOSpecDefinition.Output);
        }

        private static string[] s_listOfTraceLabSDKTypesAssemblies = new string[] { "TraceLabSDK.Types.dll", "TraceLabSDK.Types.UI.dll", "TraceLabSDK.Types.UI.WPF.dll" };

        private void CollectTypeAssemblies(Dictionary<string, IOItemDefinition> ioItems)
        {
            Type type;
            foreach (var IOItem in ioItems)
            {
                if (this.m_supportedTypes.TryGetValue(IOItem.Value.Type, out type) && type != null)
                {
                    if (!this.m_typeAssemblies.Contains(type.Assembly.Location))
                    {
                        bool include;
                        //first check if it is not any of standard TraceLabSDK.Types assemblies - these should not be automatically included
                        if (s_listOfTraceLabSDKTypesAssemblies.Any(a => type.Assembly.Location.EndsWith(a)))
                        {
                            //it is in the main TraceLab installation
                            include = false;
                        } 
                        else if (m_config.IncludeOtherPackagesAssemblies)
                        {
                            include = true;
                        }
                        else
                        {
                            //determine if component is independent or is coming from another package
                            include = !IsAssemblyInAnotherPackage(type.Assembly.Location);
                        }

                        if (include)
                        {
                            this.m_typeAssemblies.Add(type.Assembly.Location);

                            //check also for corresponding UI assembly
                            AddUITypeAssemblies(type.Assembly.Location);
                        }
                    }
                }
            }
        }

        private void AddUITypeAssemblies(string typesAssemblyLocation)
        {
            string loc = typesAssemblyLocation;
            loc = loc.Remove(loc.LastIndexOf('.'));
            foreach (string ext in TraceLab.Core.Workspaces.WorkspaceUIAssemblyExtensions.Extensions)
            {
                string uiAssemblyLoc = loc + ext;
                if (File.Exists(uiAssemblyLoc))
                {
                    this.m_typeAssemblies.Add(uiAssemblyLoc);
                }
            }
        }

        private bool IsAssemblyInAnotherPackage(string assemblyLocation)
        {
            bool isInAnotherPackage = false;

            var packages = TraceLab.Core.PackageSystem.PackageManager.Instance.Packages;

            foreach (KeyValuePair<string, IPackage> pair in packages)
            {
                IPackage package = pair.Value;
                foreach (IPackageFile packageFile in package.Files)
                {
                    string fullPath = Path.GetFullPath(Path.Combine(package.Location, packageFile.Path));
                    if (Path.Equals(assemblyLocation, fullPath))
                    {
                        isInAnotherPackage = true;
                        break;
                    }
                }
            }

            return isInAnotherPackage;
        }
    }
}

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
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Collection of files descriptors (either Assemblies or Composite Component Files).
    /// </summary>
    [Serializable]
    class FilesCollection : KeyedCollection<string, FileDescriptor>
    {
        public FilesCollection() : base() { }

        protected override string GetKeyForItem(FileDescriptor item)
        {
            return item.AbsolutePath;
        }

        public int Size()
        {
            return this.Count;
        }
    }

    /// <summary>
    /// Data structure for holding information about component files. It is used to speed up TraceLab startup time
    /// and reduce amount of disc I/O operations (specially the ones involving reflection).
    /// </summary>
    [Serializable]
    class ComponentsLibraryCache
    {
        #region Members

        /// Application executable path
        /// </summary>
        private string m_appExecutable;
        public string AppExecutable
        {
            get { return this.m_appExecutable; }
            set { this.m_appExecutable = value; }
        }

        /// Application base directory path
        /// </summary>
        private string m_baseDirectory;
        public string BaseDirectory
        {
            get { return this.m_baseDirectory; }
            set { this.m_baseDirectory = value; }
        }

        /// <summary>
        /// Collection of components files.
        /// </summary>
        private FilesCollection m_typeFiles;
        public FilesCollection TypeFiles
        {
            get { return this.m_typeFiles; }
        }

        /// <summary>
        /// Collection of components files.
        /// </summary>
        private FilesCollection m_componentFiles;
        public FilesCollection ComponentFiles
        {
            get { return this.m_componentFiles; }
        }

        /// <summary>
        /// Cache modification flag (if TRUE cache is re-written to disc).
        /// </summary>
        private bool m_wasModified;
        public bool WasModified
        {
            get { return this.m_wasModified; }
            set { this.m_wasModified = value; }
        }

        /// <summary>
        #endregion Members

        #region Methods

        public ComponentsLibraryCache(string baseDirectory)
        {
            this.m_appExecutable = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            this.m_baseDirectory = baseDirectory;
            this.m_typeFiles = new FilesCollection();
            this.m_componentFiles = new FilesCollection();
            this.m_wasModified = true;
        }

        /// <summary>
        /// Checks the directories for possible changes in the type files.
        /// </summary>
        /// <param name="typeDirectories">The directories where type files are located</param>
        public void CheckTypeDirectories(ISet<string> typeDirectories)
        {
            try
            {
                List<FileDescriptor> typeFiles = new List<FileDescriptor>();
                bool typesChanged = false;
                foreach (string directory in typeDirectories)
                {
                    if (Directory.Exists(directory))
                    {
                        string[] filePaths = System.IO.Directory.GetFiles(directory, "*.dll");
                        foreach (string file in filePaths)
                        {
                            string fullPath = Path.GetFullPath(file);

                            if (this.m_typeFiles.Contains(fullPath))
                            {
                                FileDescriptor fd = this.m_typeFiles[fullPath];
                                if (fd.isUpToDate() == false)
                                {
                                    fd.update();
                                    typesChanged = true;
                                }
                                typeFiles.Add(fd);
                            }
                            else
                            {
                                try
                                {
                                    FileDescriptor fd = new FileDescriptor(fullPath);
                                    typeFiles.Add(fd);
                                    typesChanged = true;
                                }
                                catch (FileNotFoundException ex)
                                {
                                    NLog.LogManager.GetCurrentClassLogger().Warn(ex.Message);
                                }
                            }
                        }
                    }
                }

                this.m_typeFiles.Clear();
                foreach (FileDescriptor fd in typeFiles)
                {
                    this.m_typeFiles.Add(fd);
                }

                if (typesChanged || this.m_typeFiles.Count == 0)
                {
                    this.m_componentFiles.Clear();
                    this.m_wasModified = true;
                }
            }
            catch (System.IO.IOException)
            {
                this.Clear();
            }
        }

        /// <summary>
        /// Adds collection of new files found by the ComponentScanner.
        /// </summary>
        /// <param name="files">Collection of new files read during scanning.</param>
        public void AddComponentFiles(IList<FileDescriptor> files)
        {
            if (files != null && files.Count > 0)
            {
                this.m_wasModified = true;
                foreach (FileDescriptor file in files)
                {
                    this.m_componentFiles.Add(file);
                }
            }
        }

        /// <summary>
        /// Creates a set containing the filepaths of files that are up-to-date in the collection. FilesDescriptors
        /// that are not up-to-date anymore are deleted from the cache.
        /// </summary>
        /// <returns>Set of paths for all up-to-date files in cache.</returns>
        public ISet<string> GetUpToDateFiles()
        {
            HashSet<string> files = new HashSet<string>();
            LinkedList<string> filesToRemove = new LinkedList<string>();

            foreach (FileDescriptor compFile in this.m_componentFiles)
            {
                if (compFile.isUpToDate())
                {
                    files.Add(compFile.AbsolutePath);
                }
                else
                {
                    filesToRemove.AddLast(compFile.AbsolutePath);
                }
            }

            // Remove assemblies from cache that are not up-to-date anymore
            foreach (string filepath in filesToRemove)
            {
                this.m_componentFiles.Remove(filepath);
            }

            return files;
        }

        /// <summary>
        /// Processes the ComponentScanResults by extracting the new files read during scanning and
        /// by adding the requested MetadataDefinition to the results.
        /// </summary>
        /// <param name="results">The results from scanning.</param>
        public void ProcessComponentScanResults(ComponentScanResults results)
        {
            foreach (string filePath in results.RequestedFiles)
            {
                try
                {
                    FileDescriptor file = this.m_componentFiles[filePath];
                    if (file is AssemblyFileDescriptor)
                    {
                        AssemblyFileDescriptor assembly = (AssemblyFileDescriptor)file;
                        foreach (ComponentMetadataDefinition metadata in assembly.MetadataCollection)
                        {
                            if (!results.Components.Contains(metadata.ID))
                            {
                                results.Components.Add(metadata);
                                string GuidID;
                                if (assembly.NewGuidToOldGuid.TryGetValue(metadata.ID, out GuidID))
                                {
                                    results.OldGuidToNewGuidMap.Add(GuidID, metadata.ID);
                                }
                            }
                            else
                            {
                                MetadataDefinition oldMetadata = results.Components[metadata.ID];
                                string msg = String.Format(Messages.SameComponents, oldMetadata.Classname, oldMetadata.Assembly,
                                                            metadata.Classname, metadata.Assembly);
                                NLog.LogManager.GetCurrentClassLogger().Warn(msg);
                                results.Errors.Add(msg);
                            }
                        }
                    }
                    else if (file is CompositeComponentFileDescriptor)
                    {
                        CompositeComponentFileDescriptor component = (CompositeComponentFileDescriptor)file;
                        if (!results.CompositeComponents.Contains(component.MetadataDefinition.ID))
                        {
                            results.CompositeComponents.Add(component.MetadataDefinition);
                        }
                        else
                        {
                            MetadataDefinition oldMetadata = results.CompositeComponents[component.MetadataDefinition.ID];
                            string msg = String.Format(Messages.SameComponents, oldMetadata.Classname, oldMetadata.Assembly,
                                                        component.MetadataDefinition.Classname, component.MetadataDefinition.Assembly);
                            NLog.LogManager.GetCurrentClassLogger().Warn(msg);
                            results.Errors.Add(msg);
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                }
            }
        }

        /// <summary>
        /// Checks the cache integrity.
        /// </summary>
        /// <param name="baseDirectory">The base application directory.</param>
        public void CheckAppInformation(string baseDirectory)
        {
            string executable = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            if (this.m_appExecutable == null || this.m_appExecutable != executable ||
                this.m_baseDirectory == null || this.m_baseDirectory != baseDirectory)
            {
                ComponentsLibraryCacheHelper.DeleteCacheFile();
                this.Clear();
                this.m_appExecutable = executable;
                this.m_baseDirectory = baseDirectory;
            }
        }

        /// <summary>
        /// Clears the contents of the cache.
        /// </summary>
        public void Clear()
        {
            ComponentsLibraryCacheHelper.DeleteCacheFile();
            this.m_typeFiles.Clear();
            this.m_componentFiles.Clear();
            this.WasModified = true;
        }

        #endregion Methods
    }
}

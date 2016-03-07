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
using TraceLabSDK.PackageSystem;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TraceLabSDK;

namespace TraceLab.Core.PackageSystem
{
    public sealed class PackageManager : MarshalByRefObject, IPackageManager, INotifyPropertyChanged, IEnumerable<IPackage>
    {
        private MultiDictionary<string, IPackage> m_packageLookupByRoot = new MultiDictionary<string, IPackage>();
        private Dictionary<string, IPackage> m_packages = new Dictionary<string, IPackage>();
        private ObservableCollection<string> m_packageLocations = new ObservableCollection<string>();
        private ReadOnlyObservableCollection<string> m_readonlyPackageLocations;

        private ObservableCollection<string> m_packageLoadErrors = new ObservableCollection<string>();
        private ReadOnlyObservableCollection<string> m_readonlyPackageLoadErrors;

        public PackageManager()
        {
            m_readonlyPackageLocations = new ReadOnlyObservableCollection<string>(m_packageLocations);
            m_readonlyPackageLoadErrors = new ReadOnlyObservableCollection<string>(m_packageLoadErrors);
        }

        private static PackageManager s_manager;

        #region MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion

        public static PackageManager Instance
        {
            get
            {
                if (s_manager == null)
                    s_manager = new PackageManager();

                return s_manager;
            }
        }

        internal static void InitializeManager(PackageManager manager)
        {
            if (s_manager == null)
                s_manager = manager;
        }

        public ReadOnlyObservableCollection<string> PackageLoadErrors
        {
            get
            {
                return m_readonlyPackageLoadErrors;
            }
        }

        /// <summary>
        /// Gets the current list of package locations
        /// </summary>
        public ReadOnlyCollection<string> PackageLocations
        {
            get { return m_readonlyPackageLocations; }
        }

        /// <summary>
        /// Gets the packages collection
        /// </summary>
        public Dictionary<string, IPackage> Packages
        {
            get { return m_packages; }
        }

        /// <summary>
        /// Gets the Packages within the specified root location.
        /// </summary>
        public IEnumerable<IPackage> this[string location]
        {
            get
            {
                IEnumerable<IPackage> retVal = new List<IPackage>();
                if (!string.IsNullOrWhiteSpace(location))
                {
                    if (m_packageLookupByRoot.ContainsKey(location))
                    {
                        retVal = m_packageLookupByRoot[location];
                    }
                }

                return retVal;
            }
        }

        public IEnumerable<Settings.SettingsPath> ComponentDirectories
        {
            get
            {
                foreach (KeyValuePair<string, IPackage> pkg in m_packages)
                {
                    foreach (string loc in pkg.Value.ComponentLocations)
                    {
                        var path = new Settings.SettingsPath(true, System.IO.Path.Combine(pkg.Value.Location, loc));
                        yield return path;
                    }
                }

                yield break;
            }
        }

        public IEnumerable<Settings.SettingsPath> TypeDirectories
        {
            get
            {
                foreach (KeyValuePair<string, IPackage> pkg in m_packages)
                {
                    foreach (string loc in pkg.Value.TypeLocations)
                    {
                        var path = new Settings.SettingsPath(true, System.IO.Path.Combine(pkg.Value.Location, loc));
                        yield return path;
                    }
                }

                yield break;
            }
        }

        public void Clear()
        {
            m_packages.Clear();
            m_packageLoadErrors.Clear();
            m_packageLookupByRoot.Clear();
        }

        /// <summary>
        /// Determines whether the manager contains the package that is referenced by <paramref name="package"/>.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>
        ///   <c>true</c> if the manager contains the package that is referenced by <paramref name="package"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="package"/> is null.</exception>
        public bool Contains(IPackageReference package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            return m_packages.ContainsKey(package.ID);
        }

        public IPackage GetPackage(IPackageReference package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            if (m_packages.ContainsKey(package.ID))
            {
                return m_packages[package.ID];
            }

            return null;
        }

        public void AddPackageLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException(String.Format("The given location must be non-null and a valid path: '{0}'", location), "location");

            if(!System.IO.Directory.Exists(location)) 
            {
                //attempt to create it 
                try 
                {
                    System.IO.Directory.CreateDirectory(location);
                } 
                catch(Exception ex) 
                {
                    throw new ArgumentException(String.Format("The given package location does not exist and failed to be created: '{0}', because '{1}'", location, ex.Message));
                }
            }

            m_packageLocations.Add(location);
        }

        public void AddPackageLocations(IEnumerable<string> locations)
        {
            foreach (string loc in locations)
            {
                AddPackageLocation(loc);
            }
        }

        public void AddPackageLocations(IEnumerable<Settings.SettingsPath> locations)
        {
            foreach (Settings.SettingsPath path in locations)
            {
                AddPackageLocation(path.Path);
            }
        }

        public void RemovePackageLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("The given location must be non-null and not empty.", "location");

            var pkgs = m_packages.ToArray();
            foreach (KeyValuePair<string, IPackage> pkg in pkgs)
            {
                if (pkg.Value.Location == location)
                {
                    m_packages.Remove(pkg.Key);
                }
            }

            m_packageLocations.Remove(location);
        }

        public void ScanForPackages()
        {
            Clear();

            foreach (string location in PackageLocations)
            {
                try
                {
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(location);
                    if (dirInfo.Exists)
                    {
                        var subDirectories = dirInfo.GetDirectories();
                        foreach (System.IO.DirectoryInfo subDir in subDirectories)
                        {
                            try
                            {
                                Package newPackage = new Package(subDir.FullName, true);
                                m_packages.Add(newPackage.ID, newPackage);
                                m_packageLookupByRoot.Add(dirInfo.FullName, newPackage);
                            }
                            catch (Exception e)
                            {
                                m_packageLoadErrors.Add(string.Format("Error loading package '{0}' due to: {1}", subDir.Name, e.Message));
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Gets the packages referenced by the given package.  This is a recursive call and will gather references that are indirectly referenced.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns></returns>
        public IEnumerable<IPackage> GetReferencedPackages(IPackage package, out IEnumerable<IPackageReference> missingReferences)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            List<IPackageReference> missing = new List<IPackageReference>();
            List<IPackage> references = new List<IPackage>();

            Queue<IPackage> processing = new Queue<IPackage>();
            processing.Enqueue(package);

            while (processing.Count > 0)
            {
                IPackage currentPackage = processing.Dequeue();
                foreach (IPackageReference key in currentPackage.References)
                {
                    IPackage reference;
                    m_packages.TryGetValue(key.ID, out reference);

                    // We use insert in order to allow the 'most important' ancestor references to appear first
                    if (reference != null)
                    {
                        references.Insert(0, reference);
                        processing.Enqueue(reference);
                    }
                    else
                    {
                        missing.Insert(0, key);
                    }
                }
            }

            // Finally, remove duplicates.
            references = RemoveDuplicates(references);
            missing = RemoveDuplicates(missing);

            missingReferences = missing;
            return references;
        }

        /// <summary>
        /// Removes the duplicates from the given source, preserving order.
        /// 
        /// Eg.  A B C D B F E => A B C D F E
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        List<T> RemoveDuplicates<T>(List<T> source)
        {
            List<T> newList = new List<T>();
            HashSet<T> set = new HashSet<T>();

            foreach (T item in source)
            {
                if (!set.Contains(item))
                {
                    set.Add(item);
                    newList.Add(item);
                }
            }

            return newList;
        }

        /// <summary>
        /// Determines whether all of the specified package's references exist in the Manager.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>
        ///   <c>true</c> if all references are accounted for; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAllReferences(IPackage package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            bool success = true;
            foreach (IPackageReference key in package.References)
            {
                if (!success)
                    break;

                IPackage reference;
                if (m_packages.TryGetValue(key.ID, out reference))
                {
                    success &= HasAllReferences(reference);
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Returns the directories containing type information used by the given package.  This is primarily a convenience function.
        /// Returns absolute paths to the types locations of given package and all types in referenced packages.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns></returns>
        /// <remarks>This is only valid if all the references exist.</remarks>
        public IEnumerable<string> GetDependantTypeLocations(IPackage package)
        {
            HashSet<string> paths = new HashSet<string>();
            //firstly collect types location in the current package
            foreach (string location in package.TypeLocations)
            {
                string absoluteLocation = System.IO.Path.Combine(package.Location, location);
                paths.Add(absoluteLocation);
            }

            //secondly collect types location in referenced packages
            IEnumerable<IPackageReference> missing;
            IEnumerable<IPackage> packages = GetReferencedPackages(package, out missing);

            if (missing.Count() > 0)
            {
                throw new InvalidOperationException("Cannot get dependant type information");
            }
                        
            foreach (IPackage pkg in packages)
            {
                foreach (string location in package.TypeLocations)
                {
                    string absoluteLocation = System.IO.Path.Combine(package.Location, location);
                    paths.Add(absoluteLocation);
                }
            }

            return paths;
        }

        /// <summary>
        /// Gets the absolute path for the given package file
        /// </summary>
        /// <param name="packageID">The ID of the package.</param>
        /// <param name="packageFileID">The ID of the package file.</param>
        /// <returns>
        /// The absolute path of the package file, or null if either the package or the package file doesn't exist.
        /// </returns>
        public string GetAbsolutePath(string packageID, string packageFileID)
        {
            string ret = null;

            IPackage package = null;
            m_packages.TryGetValue(packageID, out package);
            if (package != null)
            {
                ret = package.GetAbsolutePath(packageFileID);
            }

            return ret;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        public void SetPackageLocations(ObservableCollection<Settings.SettingsPath> observableCollection)
        {
            observableCollection.CollectionChanged += observableCollection_CollectionChanged;
            AddPackageLocations(observableCollection);
        }

        void observableCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Settings.SettingsPath> observableCollection = (ObservableCollection<Settings.SettingsPath>)sender;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    AddPackageLocation(((Settings.SettingsPath)e.NewItems[0]).Path);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    RemovePackageLocation(((Settings.SettingsPath)e.OldItems[0]).Path);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    RemovePackageLocation(((Settings.SettingsPath)e.OldItems[0]).Path);
                    AddPackageLocation(((Settings.SettingsPath)e.NewItems[0]).Path);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    Clear();

                    AddPackageLocations(observableCollection);
                    break;
                default:
                    break;
            }
        }

        #region IEnumerable<IPackage> Members

        public IEnumerator<IPackage> GetEnumerator()
        {
            return m_packages.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_packages.Values.GetEnumerator();
        }

        #endregion
    }
}

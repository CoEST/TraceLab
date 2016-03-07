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
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    internal class LibraryHelper
    {
        private static string SdkLocation
        {
            get
            {
                System.Reflection.Assembly sdk = typeof(IComponent).Assembly;
                return System.IO.Path.GetDirectoryName(sdk.Location);
            }
        }

        private static string CoreLocation
        {
            get
            {
                Assembly core = typeof(LibraryHelper).Assembly;
                return Path.GetDirectoryName(core.Location);
            }
        }

        private List<string> m_workspaceTypesDirectories;
        private List<string> WorkspaceTypeLocations
        {
            get
            {
                return m_workspaceTypesDirectories;
            }
        }

        private List<string> m_workspaceTypes;

        public LibraryHelper(IEnumerable<string> workspaceTypesDirectories)
        {
            if (workspaceTypesDirectories == null)
                throw new ArgumentNullException("workspaceTypesDirectories");
            if (workspaceTypesDirectories.Count() == 0)
                throw new ArgumentException("List of workspace s_metricTypes locations cannot be empty", "workspaceTypesDirectories");

            m_workspaceTypesDirectories = new List<string>(workspaceTypesDirectories);
        }

        public AppDomain CreateDomain(string nameOfDomain)
        {
            bool defaultShadowCopy;
            if (!RuntimeInfo.IsRunInMono)
            {
                defaultShadowCopy = true;
            }
            else
            {
                //in case there are additinal types to be loaded, there is error on mono "Failed to create shadow copy"
                //when preloading workspace types to components app domain, and decision module compilation app domain
                //so in case of mono don't use shadow copy
                defaultShadowCopy = false;
            }

            return CreateDomain(nameOfDomain, defaultShadowCopy);
        }

        public AppDomain CreateDomain(string nameOfDomain, bool shadowCopy)
        {
            List<string> paths = new List<string>();
            paths.Add(SdkLocation);
            paths.Add(CoreLocation);

            var newDomain = CreateDomainForPaths(nameOfDomain, paths, shadowCopy);
            PreloadWorkspaceTypes(newDomain);
            SetupPackageManager(newDomain);
            return newDomain;
        }

        private static AppDomain CreateDomainForPaths(string nameOfDomain, IEnumerable<string> paths, bool shadowCopy)
        {
            AppDomainSetup ads = new AppDomainSetup();

            // The private binary paths must be relative to and subdirectories of the ApplicationBase directory
            // therefore we have to make sure that we set our ApplicationBase to the section of the path that is 
            // common to both the currently loaded SDK (eg. from the main TraceLab build) and the component directory
            foreach (string path in paths)
            {
                if (string.IsNullOrEmpty(ads.ApplicationBase))
                {
                    ads.ApplicationBase = path;
                }
                else
                {
                    ads.ApplicationBase = GetCommonBasePath(path, ads.ApplicationBase);
                }
            }

            foreach (string path in paths)
            {
                if (string.IsNullOrWhiteSpace(ads.PrivateBinPath))
                {
                    ads.PrivateBinPath = path;
                }
                else if (!string.IsNullOrWhiteSpace(path))
                {
                    if (Directory.Exists(path))
                    {
                        ads.PrivateBinPath = ads.PrivateBinPath + Path.PathSeparator + path;
                    }
                    else if (File.Exists(path))
                    {
                        ads.PrivateBinPath = ads.PrivateBinPath + Path.PathSeparator + Path.GetDirectoryName(path);
                    }
                }
            }

            // Shadow copy files when loading them so that the main files are never actually locked
            // (can be replaced at any time)
            ads.ShadowCopyFiles = shadowCopy.ToString(CultureInfo.CurrentCulture);

            // We don't explicitly need CodeDownloading, and since it's a security risk, we turn it off.
            ads.DisallowCodeDownload = true;
            ads.ApplicationName = nameOfDomain;

            if(!RuntimeInfo.IsRunInMono)
            {
                //Mono Migration Analyzer for Mono 2.8 report showed that
                //DisallowApplicationBaseProbing	This property exists but not considered.
                //thus excluded for mono
                ads.DisallowApplicationBaseProbing = false;
            }

            var domain = CreateDomain(ads);
            return domain;
        }

        private static void PreLoadSdk(AppDomain domain)
        {
            if (domain == null)
                throw new ArgumentNullException("domain");

            AssemblyName name = new AssemblyName(typeof(IComponent).Assembly.FullName);
            domain.Load(name);
        }

        private void SetupPackageManager(AppDomain domain)
        {
            var setup = new PackageManagerSetup();
            domain.DoCallBack(setup.DoSetup);
        }

        public void PreloadWorkspaceTypes(AppDomain domain)
        {
            if (m_workspaceTypes == null)
            {
                m_workspaceTypes = new List<string>();
                foreach (string location in WorkspaceTypeLocations)
                {
                    if (!string.IsNullOrEmpty(location) && Directory.Exists(location))
                    {
                        List<string> files = new List<string>(Directory.GetFiles(location, "*.dll", SearchOption.TopDirectoryOnly));
                        m_workspaceTypes.AddRange(files);
                    }
                }
            }

            DoPreloadWorkspaceTypes(m_workspaceTypes, domain);
        }

        private static void DoPreloadWorkspaceTypes(List<string> types, AppDomain domain)
        {
            foreach (string file in types)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(file);
                    if (name.Name.StartsWith("TraceLab"))
                    {
                        name.Version = null;
                    }

                    Assembly typeAssm = null;
                    typeAssm = domain.Load(name);
                }
                catch (BadImageFormatException) { }
                catch (FileNotFoundException) { }
                catch (FileLoadException) { }
                catch (System.Security.SecurityException) { }
                catch (ArgumentException) { }
                catch (PathTooLongException) { }
            }
        }

        [Serializable]
        private class PackageManagerSetup
        {
            private TraceLab.Core.PackageSystem.PackageManager m_manager;
            public PackageManagerSetup()
            {
                m_manager = TraceLab.Core.PackageSystem.PackageManager.Instance;
            }

            public void DoSetup()
            {
                TraceLab.Core.PackageSystem.PackageManager.InitializeManager(m_manager);
            }
        }

        private static AppDomain CreateDomain(AppDomainSetup ads)
        {
            // Create the new domain with whatever current security evidence we're running with.
            AppDomain newDomain = AppDomain.CreateDomain(ads.ApplicationName, AppDomain.CurrentDomain.Evidence, ads);

            PreLoadSdk(newDomain);

            // Register a handler for unhandled exceptions so that we can always be sure that _something_ happens to them.
            newDomain.UnhandledException += ComponentDomainExceptionHelper.AppDomainUnhandledException;
            newDomain.TypeResolve += ComponentDomainExceptionHelper.AppDomainTypeResolve;
            newDomain.AssemblyResolve += ComponentDomainExceptionHelper.AppDomainAssemblyResolve;

            return newDomain;
        }

        public static void DestroyDomain(AppDomain domain)
        {
            System.Diagnostics.Debug.WriteLine("Unloading domain: " + domain.FriendlyName);
            if (!RuntimeInfo.IsRunInMono)
            {
                domain.DoCallBack(System.Windows.Forms.Application.Exit);
            }

#if !MONO_DEV
            //appdomain unload crashes mono.exe process when running directly from 
            //Mono Develop, possibly because attached debugger. When run normally works fine.
            //use MONO_DEV compiler symbol when developing
            AppDomain.Unload(domain);
#endif
        }

        /// <summary>
        /// Retrieves the common base path between two paths.
        /// </summary>
        /// <param name="componentDefinition"></param>
        /// <returns>
        /// Returns the path that is common between the two; if there is nothing in common, String.Empty is returned.
        /// </returns>
        private static string GetCommonBasePath(string componentPath, string otherPath)
        {
            string commonBase = string.Empty;
            string compRoot = Path.GetPathRoot(componentPath);
            string otherRoot = Path.GetPathRoot(otherPath);

            // if the roots aren't the same we aren't going to do anything.
            if (compRoot == otherRoot)
            {
                commonBase = Path.GetFullPath(compRoot);
                string[] compDirectories = componentPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string[] otherDirectories = otherPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                // Index zero is the root, which we already have.
                for (int i = 1; i < compDirectories.Length && i < otherDirectories.Length; ++i)
                {
                    // Case-insensitive comparison
                    if (compDirectories[i].Equals(otherDirectories[i], StringComparison.CurrentCulture))
                    {
                        commonBase = Path.Combine(commonBase, compDirectories[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return commonBase;
        }

        /// <summary>
        /// Retrieves the common base path between a component and an assembly.
        /// </summary>
        /// <param name="componentDefinition"></param>
        /// <returns>
        /// Returns the path that is common between the two; if there is nothing in common, String.Empty is returned.
        /// </returns>
        //  COMMENTED BY CODEIT.RIGHT
        //        private static string GetCommonBasePath(ComponentMetadataDefinition componentDefinition, Assembly other)
        //        {
        //            string componentPath = Path.GetDirectoryName(componentDefinition.Assembly);
        //            string otherPath = Path.GetDirectoryName(other.Location);
        //
        //            return GetCommonBasePath(componentPath, otherPath);
        //        }

        /// <summary>
        /// Retrieves the common base path between a path and an assembly.
        /// </summary>
        /// <param name="componentDefinition"></param>
        /// <returns>
        /// Returns the path that is common between the two; if there is nothing in common, String.Empty is returned.
        /// </returns>
        private static string GetCommonBasePath(string path1, Assembly other)
        {
            string otherPath = Path.GetDirectoryName(other.Location);

            return GetCommonBasePath(path1, otherPath);
        }

        /// <summary>
        /// Catch unhandled exceptions in Component domains and rethrow them so that the main application hears about it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void newDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Rethrow whatever happened on that domain
            throw (Exception)e.ExceptionObject;
        }
    }

    [Serializable]
    internal class CrossDomainLoader
    {
        private string m_codebase;

        internal CrossDomainLoader(string codebase)
        {
            if (string.IsNullOrEmpty(codebase))
                throw new ArgumentNullException("codebase");

            m_codebase = codebase;
        }

        internal void Load()
        {
            try
            {
                Assembly.LoadFrom(m_codebase);
            }
            catch (BadImageFormatException) { }
            catch (FileNotFoundException) { }
            catch (FileLoadException) { }
            catch (System.Security.SecurityException) { }
            catch (ArgumentException) { }
            catch (PathTooLongException) { }
        }
    }
}

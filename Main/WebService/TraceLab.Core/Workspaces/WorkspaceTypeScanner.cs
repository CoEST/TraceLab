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
using System.Reflection;
using TraceLabSDK;

namespace TraceLab.Core.Workspaces
{
    /// <summary>
    /// Similar to the component scanner, this will scan a directory to see if it has any workspace types in it.
    /// </summary>
    /// <remarks>Since types can be auto-loaded by the framework as necessary, this just needs to discover which directories actually have types in them.</remarks>
    class WorkspaceTypeScanner : MarshalByRefObject
    {
        private List<string> m_typeDirectories;
        public IEnumerable<string> TypeDirectories
        {
            get { return m_typeDirectories; }
        }

        private List<string> m_errors;
        public IEnumerable<string> Errors
        {
            get { return m_errors; }
        }

        WorkspaceTypeScanner()
        {
            m_typeDirectories = new List<string>();
            m_errors = new List<string>();
        }

        public void Scan(IEnumerable<string> candidateDirectories)
        {
            var pathSet = new HashSet<string>(candidateDirectories);

            foreach (var dir in pathSet)
            {
                string[] filePaths = GetAssemblyFiles(dir);

                if (filePaths == null)
                    continue;

                bool foundType = false;
                foreach (var assembly in filePaths)
                {
                    // If we've already found a type, then break.
                    if (foundType)
                        break;

                    try
                    {
                        var assm = Assembly.LoadFrom(assembly);

                        var types = assm.GetExportedTypes();
                        foreach (var exportedType in types)
                        {
                            // If we've already found a type, then break.
                            if (foundType)
                                break;

                            foundType |= CheckType(dir, assembly, exportedType);
                        }
                    }
                    catch (Exception e)
                    {
                        // Catch everything, just log
                        // Basically, if an exception is thrown, then this is not a valid Component.
                        string message = e.InnerException != null ? e.InnerException.Message : e.Message;
                        message = string.Format("Unable to load assembly {0}: {1}", assembly, message);
                        m_errors.Add(message);
                    }
                }
            }
        }

        private string[] GetAssemblyFiles(string dir)
        {
            string[] filePaths = null;
            try
            {
                filePaths = System.IO.Directory.GetFiles(dir, "*.dll");
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                // Catch everything, just log
                // Basically, if an exception is thrown, then this is not a valid Component.
                string message = e.InnerException != null ? e.InnerException.Message : e.Message;
                message = string.Format("Directory does not exist: {0}", message);
                m_errors.Add(message);
            }
            return filePaths;
        }

        private bool CheckType(string dir, string assembly, Type exportedType)
        {
            bool foundType = false;
            try
            {
                var attribs = exportedType.GetCustomAttributes(typeof(WorkspaceTypeAttribute), false);
                if (attribs.Length == 1)
                {
                    var attrib = (WorkspaceTypeAttribute)attribs[0];
                    m_typeDirectories.Add(dir);
                    foundType = true;
                }
                else if (1 < attribs.Length)
                {
                    var errorMsg = "ERROR: Somehow there are more than one WorkspaceTypeAttribute instances on type " + exportedType.FullName;
                    m_errors.Add(errorMsg);
                }
            }
            catch (Exception e)
            {
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                message = string.Format("Unable to load component '{0}' in assembly {1}: {2}", exportedType.FullName, assembly, message);
                m_errors.Add(message);
            }

            return foundType;
        }

        public static bool Scan(IEnumerable<string> directories, IEnumerable<string> workspaceTypeDirectories, out List<string> directoriesWithTypes, out List<string> errors)
        {
            bool success = true;
            AppDomain newDomain = null;
            errors = null;
            try
            {
                var libraryHelper = new TraceLab.Core.Components.LibraryHelper(workspaceTypeDirectories);

                newDomain = libraryHelper.CreateDomain("WorkspaceTypeScanner", false);
                newDomain.Load(Assembly.GetExecutingAssembly().GetName());

                // Preload the workspace types so that the component scanner won't barf when a component references a workspace type.
                libraryHelper.PreloadWorkspaceTypes(newDomain);

                WorkspaceTypeScanner scanner = (WorkspaceTypeScanner)newDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(WorkspaceTypeScanner).FullName);

                scanner.Scan(directories);

                errors = new List<string>(scanner.Errors);
                foreach (string error in errors)
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn(error);
                }

                // Copy the list so we only get a single AppDomain thunk
                directoriesWithTypes = new List<string>(scanner.TypeDirectories);
            }
            catch (Exception e)
            {
                success = false;
                directoriesWithTypes = new List<string>();

                errors = errors ?? new List<string>();
                errors.Add(e.Message);
            }
            finally
            {
                if (newDomain != null)
                {
                    AppDomain.Unload(newDomain);
                }
            }

            return success;
        }

    }
}

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
using System.IO;
using TraceLab.Core.Exceptions;
using TraceLab.Core.Workspaces.Serialization;
using TraceLabSDK;
using System.Collections.Generic;

namespace TraceLab.Core.Workspaces
{
    /**
     * Manages all existing Workspaces and gives access to the Workspace
     * 
     * 
     */
    public sealed class WorkspaceManager
    {
        private static readonly object s_padlock = new object();
        private static IWorkspaceInternal s_workspaceInstance;

        private WorkspaceManager()
        {
        }

        public static void InitWorkspace(List<string> typeDirectories, string workspaceDirectory, string cacheDirectory)
        {
            if (typeDirectories == null)
                throw new ArgumentNullException("typeDirectories");
            if (typeDirectories.Count == 0)
                throw new ArgumentException("List of type directories cannot be empty", "typeDirectories");
            if (cacheDirectory == null)
                throw new ArgumentNullException("cacheDirectory");
            if (cacheDirectory.Length == 0)
                throw new ArgumentException("m_workspace cache directory must be a valid directory", "cacheDirectory");

            if (s_workspaceInstance != null)
                s_workspaceInstance.Dispose();

            StreamManager manager = StreamManager.CreateNewManager();
            s_workspaceInstance = InitWorkspaceInternal(typeDirectories, workspaceDirectory, cacheDirectory, manager);
        }

        private static IWorkspaceInternal InitWorkspaceInternal(List<string> typeDirectories, string workspaceDirectory, string cacheDirectory, StreamManager manager)
        {
            lock (s_padlock)
            {
                return new Workspace(workspaceDirectory, cacheDirectory, typeDirectories, manager);
            }
        }


        public static IWorkspace WorkspaceInstance
        {

            //currently in the prototype we have only one Workspace, so just return this one...
            //later each experiments need to have one Workspace
            //simple thread safety - to be reviewed - we don't want to lock each time, but who cares for now...
            get
            {
                lock(s_padlock)
                {
                    if (s_workspaceInstance == null)
                    {
                        throw new WorkspaceException("m_workspace has not been initialized");
                    }

                    return s_workspaceInstance;
                }
            }
        }

        internal static IWorkspaceInternal WorkspaceInternalInstance
        {

            //currently in the prototype we have only one Workspace, so just return this one...
            //later each experiments need to have one Workspace
            //simple thread safety - to be reviewed - we don't want to lock each time, but who cares for now...
            get
            {
                lock (s_padlock)
                {
                    if (s_workspaceInstance == null)
                    {
                        throw new TraceLab.Core.Exceptions.WorkspaceException("m_workspace has not been initialized");
                    }

                    return s_workspaceInstance;
                }
            }
        }
    }
}

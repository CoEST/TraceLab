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
using TraceLab.Core.Components;
using System.Reflection;

namespace TraceLab.Core.Workspaces
{
    [Serializable]
    internal class WorkspaceWrapperFactory
    {
        private WorkspaceWrapperFactory() { }

        /// <summary>
        /// Creates the workspace wrapper, which ensures that loading and storing of workspace items has been declared by the component.
        /// </summary>
        /// <param name="originalComponentMetadata">The component metadata.</param>
        /// <param name="workspaceInstance">The workspace instance.</param>
        /// <returns>created workspace wrapper</returns>
        public static WorkspaceWrapper CreateWorkspaceWrapper(ComponentMetadata componentMetadata, IWorkspaceInternal workspaceInstance)
        {
            return new WorkspaceWrapper(componentMetadata.IOSpec, workspaceInstance);
        }

        /// <summary>
        /// Creates the composite component workspace wrapper. 
        /// It is specifc workspace wrapper, that uses the composite node namespace to store and load items. It pass loading and storing to its
        /// workspace instance, but with added namespace. 
        /// It also has method to setup and teardown iospec and config before executing composite node.
        /// </summary>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <param name="workspaceInstance">The workspace instance.</param>
        /// <param name="workspaceNamespaceId">The workspace namespace id.</param>
        /// <returns></returns>
        public static NestedWorkspaceWrapper CreateCompositeComponentWorkspaceWrapper(CompositeComponentMetadata compositeComponentMetadata, IWorkspaceInternal workspaceInstance, string workspaceNamespaceId, AppDomain componentsAppDomain) 
        {
            var type = typeof(NestedWorkspaceWrapper);

            var nestedWorkspaceWrapper = (NestedWorkspaceWrapper)componentsAppDomain.CreateInstanceAndUnwrap(
                type.Assembly.FullName, type.FullName, false,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance, null,
                new object[] { compositeComponentMetadata.IOSpec, workspaceInstance, workspaceNamespaceId },
                System.Globalization.CultureInfo.CurrentCulture, new object[] { });

            return nestedWorkspaceWrapper; 
        }

        /// <summary>
        /// Creates the scope nested workspace wrapper. 
        /// It is specifc workspace wrapper, that uses the composite node namespace to store and load items. It pass loading and storing to its
        /// workspace instance, but with added namespace. 
        /// It also has method to setup and teardown workspace units from and to nested workspace.
        /// </summary>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <param name="workspaceInstance">The workspace instance.</param>
        /// <param name="workspaceNamespaceId">The workspace namespace id.</param>
        /// <returns></returns>
        public static ScopeNestedWorkspaceWrapper CreateCompositeComponentWorkspaceWrapper(ScopeBaseMetadata scopeMetadata, IWorkspaceInternal workspaceInstance, string workspaceNamespaceId, AppDomain componentsAppDomain)
        {
            var type = typeof(ScopeNestedWorkspaceWrapper);

            var scopeNestedWorkspaceWrapper = (ScopeNestedWorkspaceWrapper)componentsAppDomain.CreateInstanceAndUnwrap(
                type.Assembly.FullName, type.FullName, false,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance, null,
                new object[] { workspaceInstance, workspaceNamespaceId },
                System.Globalization.CultureInfo.CurrentCulture, new object[] { });

            return scopeNestedWorkspaceWrapper;
        }

        public static ExperimentWorkspaceWrapper CreateExperimentWorkspaceWrapper(Workspace workspaceInstance, string experimentId)
        {
            return new ExperimentWorkspaceWrapper(workspaceInstance, experimentId);
        }
    }

}

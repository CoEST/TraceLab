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
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Responsible for node creation for the sub template graph
    /// </summary>
    internal class NodesFactoryOfSubGraph : RunnableNodeFactory
    {
        /// <summary>
        /// Initializes a new s_instance of the <see cref="NodesFactoryOfSubGraph"/> class.
        /// </summary>
        /// <param name="compositeComponentMetadata">The composite component metadata, which is going to be used to override config values in subgraph.</param>
        /// <param name="workspaceWrapper">The workspace wrapper.</param>
        /// <param name="experimentReverseLookup">The experiment reverse lookup holds information of all predecessor experiments in the path - used to prevent recursion in case subexperiment refers back to any of the previous experiments.</param>
        public NodesFactoryOfSubGraph(CompositeComponentMetadata compositeComponentMetadata, IWorkspaceInternal workspaceWrapper) : base(workspaceWrapper) //compositeComponentMetadata.SubExperimentFile
        {
            m_compositeComponentMetadata = compositeComponentMetadata;
        }
        
        private CompositeComponentMetadata m_compositeComponentMetadata;

        /// <summary>
        /// Creates the Runnable node with a specific id based on the given metadata.
        /// Uses Composite Component Metadata config values to override Components config values in subgraph.
        /// </summary>
        /// <param name="id">The id of the node.</param>
        /// <param name="metadata">The component metadata.</param>
        /// <param name="loggerNameRoot">The logger name root - needed so that the logs are specific per experiment and experiment window.</param>
        /// <param name="library">The library of components.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which components assemblies are going to be loaded into.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The event that allows signalling termination of the experiment;
        /// Needed for the composite components sublevel experiments, so that they hold the referance to the same termination event as top level experiment</param>
        /// <returns>
        /// Created node
        /// </returns>
        public override RunnableNode CreateNode(String id, Metadata metadata, LoggerNameRoot loggerNameRoot, ComponentsLibrary library,
                                                AppDomain componentsAppDomain, System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            RunnableNode retNode;

            ComponentMetadata originalComponentMetadata = metadata as ComponentMetadata;
            CompositeComponentMetadata compositeComponentMetadata = metadata as CompositeComponentMetadata;

            if (originalComponentMetadata != null)
            {
                ComponentMetadata overrideComponentMetadata = (ComponentMetadata)originalComponentMetadata.Clone();
                OverrideConfigValues(id, overrideComponentMetadata);
                retNode = base.CreateNode(id, overrideComponentMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }
            else if (compositeComponentMetadata != null)
            {
                OverrideConfigValues(id, compositeComponentMetadata);
                retNode = base.CreateCompositeComponentNode(id, compositeComponentMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }
            else
            {
                retNode = base.CreateNode(id, metadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }

            return retNode;
        }

        /// <summary>
        /// Overrides the config values of the given metadata with values from the this CompositeComponent config values.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="metadata">The metadata.</param>
        private void OverrideConfigValues(String id, IConfigurableAndIOSpecifiable metadata)
        {
            foreach (string configParameter in metadata.ConfigWrapper.ConfigValues.Keys)
            {
                string extendedParameterName = String.Format("{0}:{1}", id, configParameter);
                ConfigPropertyObject overridePropertyObject = m_compositeComponentMetadata.ConfigWrapper.ConfigValues[extendedParameterName];
                metadata.ConfigWrapper.ConfigValues[configParameter] = overridePropertyObject;
            }
        }
    }
}

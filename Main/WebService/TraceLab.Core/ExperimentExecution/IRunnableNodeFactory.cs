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

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents the nodes factory, responsible for node creation for the template graph
    /// </summary>
    internal interface IRunnableNodeFactory
    {
        /// <summary>
        /// Creates the Runnable node with a specific id based on the given metadata.
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
        RunnableNode CreateNode(String id, TraceLab.Core.Components.Metadata metadata, 
                                TraceLab.Core.Components.LoggerNameRoot loggerNameRoot, 
                                TraceLab.Core.Components.ComponentsLibrary library,
                                AppDomain componentsAppDomain,
                                System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent);
    }
}

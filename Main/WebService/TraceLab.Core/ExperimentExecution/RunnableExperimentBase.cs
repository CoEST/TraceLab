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

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Defines template graph
    /// </summary>
    internal abstract class RunnableExperimentBase : IDisposable
    {
        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        public abstract void AddNode(String id, Metadata metadata, LoggerNameRoot loggerNameRoot);

        /// <summary>
        /// Adds the directed edge between two nodes.
        /// </summary>
        /// <param name="fromNodeId">Node id that edge is coming out from</param>
        /// <param name="toNodeId">Node id that edge is coming in to</param>
        public abstract void AddDirectedEdge(String fromNodeId, String toNodeId);

        /// <summary>
        /// Determines whether template graph contains the node of the specified id.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <returns>
        ///   <c>true</c> if template graph contains node of the specified id; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool Contains(String nodeId);

        /// <summary>
        /// Removes the node of the specified id.
        /// </summary>
        /// <param name="nodeId">The id of the node to be existed</param>
        /// <returns>
        ///     <c>true</c> if node has been existed successfully; otherwise, <c>false</c>.
        /// </returns>
        //public abstract bool Remove(String nodeId);

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        public abstract RunnableNodeCollection Nodes
        {
            get;
        }

        /// <summary>
        /// Gets the start node.
        /// </summary>
        public abstract RunnableNode StartNode
        {
            get;
        }

        /// <summary>
        /// Gets the end node.
        /// </summary>
        public abstract RunnableNode EndNode
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this template graph is empty, ie. does not contain any nodes
        /// </summary>
        /// <value>
        ///   <c>true</c> if template graph is empty; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsEmpty
        {
            get;
        }

        /// <summary>
        /// Clears the template graph from all nodes
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Gets the components app domain. The components app domain is the domain, in which all components
        /// assemblies get loaded to for the execution of the Runnable Experiment
        /// </summary>
        public abstract AppDomain ComponentsAppDomain
        {
            get;
        }

        /// <summary>
        /// The event that allows signalling termination of the experiment
        /// </summary>
        public abstract System.Threading.ManualResetEvent TerminateExperimentExecutionResetEvent
        {
            get;
        }


        #region Dispose

        ~RunnableExperimentBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) { }

        #endregion

    }
}

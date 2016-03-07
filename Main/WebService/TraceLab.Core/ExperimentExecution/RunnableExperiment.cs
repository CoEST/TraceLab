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
using System.Text;
using TraceLab.Core.Components;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents experimental graph for the experiment execution. In other words this graph is executed by the dispatcher. 
    /// Effectively it is a copy of the View m_experiment graph, that includes only necessary information for the experiment to run. 
    /// </summary>
    internal class RunnableExperiment : RunnableExperimentBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new s_instance of the <see cref="RunnableExperiment"/> class.
        /// </summary>
        /// <param name="nodesFactory">The nodes factory that is going to be used to create nodes when nodes are added to template graph.</param>
        /// <param name="library">The library.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which components assemblies are going to be loaded into.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The event that allows signalling termination of the experiment</param>
        public RunnableExperiment(IRunnableNodeFactory nodesFactory, TraceLab.Core.Components.ComponentsLibrary library,
                                  AppDomain componentsAppDomain, System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            if (componentsAppDomain == null)
                throw new ArgumentNullException("componentsAppDomain");
            if (terminateExperimentExecutionResetEvent == null)
                throw new ArgumentNullException("terminateExperimentExecutionResetEvent");

            m_componentsAppDomain = componentsAppDomain;
            m_terminateExperimentExecutionResetEvent = terminateExperimentExecutionResetEvent;
            m_nodes = new RunnableNodeCollection();
            m_nodesFactory = nodesFactory;
            Library = library;
        }

        #endregion

        #region Nodes Factory

        private IRunnableNodeFactory m_nodesFactory;

        #endregion

        #region Properties

        protected TraceLab.Core.Components.ComponentsLibrary Library
        {
            get;
            private set;
        }

        private RunnableNodeCollection m_nodes;
        public override RunnableNodeCollection Nodes
        {
            get
            {
                return m_nodes;
            }
        }

        private RunnableNode m_startNode;
        public override RunnableNode StartNode
        {
            get
            {
                if (m_startNode == null)
                {
                    throw new TraceLab.Core.Exceptions.InconsistentTemplateException("Template does not have a start node defined.");
                }
                return m_startNode;
            }
        }

        private RunnableNode m_endNode;
        public override RunnableNode EndNode
        {
            get
            {
                if (m_endNode == null)
                {
                    throw new TraceLab.Core.Exceptions.InconsistentTemplateException("Template does not have a end node defined.");
                }
                return m_endNode;
            }
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="metadata">The metadata.</param>
        public override void AddNode(String id, Metadata metadata, LoggerNameRoot loggerNameRoot)
        {
            // adds a node to the graph
            RunnableNode node = m_nodesFactory.CreateNode(id, metadata, loggerNameRoot, Library, ComponentsAppDomain, TerminateExperimentExecutionResetEvent);
            if (node is RunnableStartNode)
            {
                //allow set only once
                if (m_startNode != null)
                {
                    throw new TraceLab.Core.Exceptions.InconsistentTemplateException("Template cannot have two start nodes defined.");
                }
                m_startNode = node;
            }
            if (node is RunnableEndNode)
            {
                //allow set only once
                if (m_endNode != null)
                {
                    throw new TraceLab.Core.Exceptions.InconsistentTemplateException("Template cannot have two end nodes defined.");
                }
                m_endNode = node;
            }
            m_nodes.Add(node);
        }

        /// <summary>
        /// Adds the directed edge between two nodes.
        /// </summary>
        /// <param name="fromNodeId">Node id that edge is coming out from</param>
        /// <param name="toNodeId">Node id that edge is coming in to</param>
        public override void AddDirectedEdge(String fromNodeId, String toNodeId)
        {
            AddDirectedEdgeImpl(m_nodes[fromNodeId], m_nodes[toNodeId]);
        }

        private static void AddDirectedEdgeImpl(IRunnableNode from, IRunnableNode to)
        {
            from.AddNextNode(to);
            to.AddPreviousNode(from);
        }

        /// <summary>
        /// Determines whether template graph contains the node of the specified id.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <returns>
        ///   <c>true</c> if template graph contains node of the specified id; otherwise, <c>false</c>.
        /// </returns>
        public override bool Contains(String nodeId)
        {
            return m_nodes.Contains(nodeId);
        }

        /// <summary>
        /// Gets a value indicating whether this template graph is empty, ie. does not contain any nodes
        /// </summary>
        /// <value>
        ///   <c>true</c> if template graph is empty; otherwise, <c>false</c>.
        /// </value>
        public override bool IsEmpty
        {
            get {
                return (Nodes.Count == 0) ? true : false;
            }
        }

        /// <summary>
        /// Clears the template graph from all nodes
        /// </summary>
        public override void Clear()
        {
            foreach (RunnableNode node in m_nodes)
            {
                node.Dispose();
            }

            m_nodes.Clear();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents textual representation of the graph.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents textual representation of the graph.
        /// </returns>
        public override string ToString()
        {
            StringBuilder g = new StringBuilder();

            foreach (RunnableNode n in m_nodes)
            {
                g.Append("Node " + n.Id);
                foreach (RunnableNode neighbor in n.NextNodes)
                    g.Append("->" + neighbor.Id);
                g.AppendLine();
            }

            return g.ToString();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear();
            }
        }

        #endregion

        private AppDomain m_componentsAppDomain;
        public override AppDomain ComponentsAppDomain
        {
            get 
            {
                return m_componentsAppDomain;
            }
        }

        private System.Threading.ManualResetEvent m_terminateExperimentExecutionResetEvent;
        /// <summary>
        /// The event that allows signalling termination of the experiment
        /// </summary>
        public override System.Threading.ManualResetEvent TerminateExperimentExecutionResetEvent
        {
            get { return m_terminateExperimentExecutionResetEvent; }
        }
    }
}

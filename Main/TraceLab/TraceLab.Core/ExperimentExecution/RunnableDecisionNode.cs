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
using TraceLabSDK;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents decision node in the experiment. 
    /// </summary>
    [Serializable]
    internal class RunnableDecisionNode : RunnableNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunnableDecisionNode"/> class.
        /// </summary>
        /// <param name="id">The id of this decision node.</param>
        /// <param name="label">The label - useful for debugging.</param>
        /// <param name="decisionModule">The decision module that is going to be invoked to select nodes to be executed after decision</param>
        /// <param name="library">The reference to the components library.</param>
        public RunnableDecisionNode(String id, String label, TraceLab.Core.Decisions.IDecisionModule decisionModule, ComponentsLibrary library, bool waitForAllPredecessors)
            : base(id, label, new RunnableNodeCollection(), new RunnableNodeCollection(), library, waitForAllPredecessors)
        {
            if (decisionModule == null)
                throw new ArgumentNullException("decisionModule");
            m_candidateNodes = new RunnableNodeCollection();
            m_decisionModule = decisionModule;
        }

        /// <summary>
        /// The decision module that is going to be invoked to select nodes to be executed after decision
        /// </summary>
        private TraceLab.Core.Decisions.IDecisionModule m_decisionModule;

        /// <summary>
        /// The candidate nodes, representing first nodes of outgoing paths from this decision node.
        /// </summary>
        private RunnableNodeCollection m_candidateNodes = null;

        /// <summary>
        /// Adds the next successor node. It also stores the node in the candidate nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        public override void AddNextNode(IRunnableNode node)
        {
            base.AddNextNode(node);
            m_candidateNodes.Add(node);
        }

        /// <summary>
        /// The decision node first delegates decision by calling its decision module to select nodes that are going to be run, from among all candidates nodes.
        /// Based on the decision the graph is being slightly modified. The nodes, which are going to be executed as a result of this decision, are discovered
        /// and their execution status is set to NOTACTIVE. 
        /// Their Previous nodes (predecesoor nodes) are modified by removing nodes coming from paths that are not taken as a result of this decision.
        /// This successor nodes are being cleared, and only selected nodes are being added to the collection of the Next Nodes.
        /// Thanks to this solution dispatcher doesn't need to know anything about underlying graph. 
        /// </summary>
        public override void RunInternal()
        {
            RunnableNodeCollection selectedNodes = m_decisionModule.Decide(m_candidateNodes);

            if (selectedNodes == null || selectedNodes.Count == 0)
            {
                HasError = true;
                ErrorMessage = "A decision node must make a decision.";
            }
            else
            {
                // send signal only to selected nodes...
                NextNodes.Clear();
                NextNodes.AddRange(selectedNodes);
            }
        }

        
        #region Dispose

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_decisionModule != null)
                {
                    m_decisionModule = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

    }
}

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

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Represents the loop scope, the node with composite editable graph
    /// </summary>
    public class ScopeNode : ScopeNodeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        public ScopeNode(string id, SerializedVertexDataWithSize data) : base(id, data) {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ScopeNode"/> class from being created.
        /// </summary>
        private ScopeNode() : base() {
        }

        /// <summary>
        /// Gets or sets the decision node - entry node to this scope
        /// </summary>
        /// <value>
        /// The decision node.
        /// </value>
        public ExperimentDecisionNode DecisionNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exit decision node - exit node from the scope
        /// </summary>
        /// <value>
        /// The exit decision node.
        /// </value>
        public ExitDecisionNode ExitDecisionNode
        {
            get;
            set;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override ExperimentNode Clone()
        {
            var clone = new ScopeNode();
            clone.CopyFrom(this);
            return clone;
        }

        /// <summary>
        /// Copies data from another node to this
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(ExperimentNode other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            base.CopyFrom(other);
            InitializeComponentGraph();
            //references to decision node and exit node have to be set outside - as these nodes are also cloned
        }
    }

    class ScopeNodeHelper
    {
        /// <summary>
        /// Helper method, that sets the scope decision entry and exit nodes.
        /// If source vertex is scope and target vertex is exit decision node, it sets that source scope vertex ExitDecisionNode property to the given target exit node.
        /// Similarly, if target vertex is a scope node, and source vertex is decision, it sets that target scope vertex Decision property to the given source decision node.
        /// </summary>
        /// <param name="sourceVert">The source vert.</param>
        /// <param name="targetVert">The target vert.</param>
        public static void TryFixScopeDecisionEntryAndExitNodes(ExperimentNode sourceVert, ExperimentNode targetVert)
        {
            //case 1: sourceVert is ScopeNode, and target is ExitDecisionNode
            ScopeNode scopeNode = sourceVert as ScopeNode;
            if (scopeNode != null)
            {
                ExitDecisionNode exitDecisionNode = targetVert as ExitDecisionNode;
                if (exitDecisionNode != null)
                {
                    scopeNode.ExitDecisionNode = exitDecisionNode;
                }
            }
            else
            {
                //case 2: targetNode is ScopeNode, and source is DecisionNode
                scopeNode = targetVert as ScopeNode;
                if (scopeNode != null)
                {
                    ExperimentDecisionNode decisionNode = sourceVert as ExperimentDecisionNode;
                    if (decisionNode != null)
                    {
                        scopeNode.DecisionNode = decisionNode;
                    }
                }
            }
        }
    }
}

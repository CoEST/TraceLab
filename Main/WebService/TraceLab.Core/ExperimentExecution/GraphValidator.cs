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

using System.Collections.Generic;
using System.Linq;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;

namespace TraceLab.Core.ExperimentExecution
{
    internal static class GraphValidator
    {
        /// <summary>
        /// This validator validates the graph structure of the experiment. It analizes all paths weather they are connected properly to the end.
        /// Traverse experiment from start. Any nodes that are not connected are skipped.
        /// If back edge is detected then it is marked as error at the edge source node. 
        /// Any paths that are not completed till the END node are skipped - it could be run then without those branches, but currently they are marked with error.
        /// </summary>
        /// <param name="m_experiment">Graph m_experiment, that allow retrieves out edges for each node</param>
        /// <param name="vertices">returns list of vertices that are going to be included in the experiment run. It skips vertices that are not connected to main flow beginning at the START node</param>
        /// <param name="edges">returns list of edges that are going to be included in the experiment run. It skips edges that are on the path that does no go to END node.</param>
        /// <returns>returns true if there are no errors; false if any error has been detected</returns>
        public static bool Validate(IExperiment experiment, out List<ExperimentNode> vertices, out List<ExperimentNodeConnection> edges)
        {
            bool noErrors = true;

            vertices = new List<ExperimentNode>();
            edges = new List<ExperimentNodeConnection>();

            Dictionary<ExperimentNode, GraphNodeStatus> verticesStatuses = new Dictionary<ExperimentNode, GraphNodeStatus>();
  
            DFSVisit(experiment.StartNode, false, experiment, verticesStatuses, ref vertices, ref edges, ref noErrors);

            return noErrors;

        }

        private enum GraphNodeStatus 
        { 
            Visiting, 
            HasNoPathToEnd, 
            HasPathToEnd 
        }

        private static void DFSVisit(ExperimentNode node, bool pathFromDecision, IExperiment graph, Dictionary<ExperimentNode, GraphNodeStatus> verticesStatuses, ref List<ExperimentNode> vertices, ref List<ExperimentNodeConnection> edges, ref bool noErrors)
        {
            verticesStatuses.Add(node, GraphNodeStatus.Visiting);

            GraphNodeStatus status = GraphNodeStatus.HasNoPathToEnd;

            bool isDecision = (node.Data.Metadata is DecisionMetadata);
            bool comingFromDecision = isDecision || pathFromDecision;

            if (graph.OutEdges(node).Count<ExperimentNodeConnection>() > 0)
            {
                foreach (ExperimentNodeConnection edge in graph.OutEdges(node))
                {
                    if (verticesStatuses.ContainsKey(edge.Target) == false)
                    {
                        DFSVisit(edge.Target, comingFromDecision, graph, verticesStatuses, ref vertices, ref edges, ref noErrors);
                    }
                    if (verticesStatuses[edge.Target].Equals(GraphNodeStatus.Visiting) == true && comingFromDecision == false)
                    {
                        noErrors = false;
                        SetErrorOnNode(node, "Circular link detected.");
                    } 
                    else if (verticesStatuses[edge.Target].Equals(GraphNodeStatus.HasPathToEnd) == true ||
                            (verticesStatuses[edge.Target].Equals(GraphNodeStatus.Visiting) == true && comingFromDecision == true))
                    {
                        // Add EDGE and its SOURCE and TARGET vertices only if Path was completed to END.
                        // or if it is circular link that is coming from the decision

                        status = GraphNodeStatus.HasPathToEnd;
                        if (vertices.Contains(edge.Target) == false)
                            vertices.Add(edge.Target);
                        if (vertices.Contains(edge.Source) == false) //current node, ie. node == edge.Source
                            vertices.Add(edge.Source);
                        edges.Add(edge);
                    }
                }
            }
            else
            {
                if (node.Data.Metadata is EndNodeMetadata)
                {
                    status = GraphNodeStatus.HasPathToEnd;
                }
                else
                {
                    noErrors = false;
                    SetErrorOnNode(node, "Unable to detect path to the END node.");
                }
            }
            verticesStatuses[node] = status;
        }

        /// <summary>
        /// Sets error on node. In case if node already has been marked with error, just adds additional error message.
        /// </summary>
        /// <param name="node">Node to be marked with error</param>
        /// <param name="errorMessage">Error message that describes the error</param>
        public static void SetErrorOnNode(ExperimentNode node, string errorMessage)
        {
            if (node.HasError)
            {
                //add message to existing errorMessage
                node.SetError(node.ErrorMessage + System.Environment.NewLine + errorMessage);
            }
            else
            {
                node.SetError(errorMessage);
            }
        }

    }
}

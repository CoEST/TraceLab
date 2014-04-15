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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Composite Component Graph represents the subexperiment of the composite component.
    /// In practice it is a wrapper around the sub experiment.
    /// </summary>
    public class CompositeComponentGraph : BaseExperiment
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentGraph"/> class.
        /// </summary>
        protected CompositeComponentGraph() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentGraph"/> class
        /// without reference to any node. 
        /// </summary>
        /// <param name="componentGraph">The component graph.</param>
        public CompositeComponentGraph(BaseExperiment componentGraph) : base()
        {
            if (componentGraph == null)
            {
                throw new ArgumentException("Component graph cannot be null");
            }

            CopyFrom(componentGraph);
            OwnerNode = null; //does not belong to any node
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentGraph"/> class with the given settings.
        /// </summary>
        /// <param name="compositeComponentNode">The composite component node.</param>
        /// <param name="componentGraph">The component graph.</param>
        /// <param name="settings">The settings. All components are initialized with with the given settings.</param>
        public CompositeComponentGraph(CompositeComponentNode compositeComponentNode, CompositeComponentGraph componentGraph) : base()
        {
            if (componentGraph == null)
            {
                throw new ArgumentException("Component graph cannot be null");
            }

            CopyFrom(componentGraph);
            OwnerNode = compositeComponentNode;
        }
        
        public override BaseExperiment Clone()
        {
            var clone = new CompositeComponentGraph(this);
            clone.ResetModifiedFlag();
            return clone;
        }

        /// <summary>
        /// Gets the actual wrapped experiment. Needed for ExperimentWrapperToExperimentConverter
        /// </summary>
        /// <returns></returns>
        public IExperiment GetExperiment()
        {
            return this;
        }

        private CompositeComponentNode m_ownerNode;
        public virtual CompositeComponentNode OwnerNode
        {
            get
            {
                return m_ownerNode;
            }
            set
            {
                m_ownerNode = value;
            }
        }

        #endregion

        /// <summary>
        /// Calculates the modification.
        /// </summary>
        /// <returns></returns>
        protected override bool CalculateModification()
        {
            return base.CalculateModification();
        }
             
        private string m_fullGraphIdPath;

        /// <summary>
        /// Gets the graph id path. It is the path constructed with all owner nodes id along the path to the node id in the top level experiment.
        /// </summary>
        public string GraphIdPath
        {
            get
            {
                if (m_fullGraphIdPath == null)
                {
                    DiscoverFullGraphIDPath();
                }
                return m_fullGraphIdPath;
            }
        }

        /// <summary>
        /// Discovers the full graph ID path.
        /// </summary>
        private void DiscoverFullGraphIDPath()
        {
            //call the method that discovers it along with top owner node
            CompositeComponentGraph parentGraph = OwnerNode.Owner as CompositeComponentGraph;
            if (parentGraph != null)
            {
                //otherwise add to already existing path
                m_fullGraphIdPath = parentGraph.GraphIdPath + ":" + OwnerNode.ID;
            } 
            else 
            {
                //otherwise it must have reached the top level experiment
                m_fullGraphIdPath = OwnerNode.ID;
            }
        }
        
        private ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> m_references = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>();
        /// <summary>
        /// Gets or sets the collection of references to the packages
        /// </summary>
        /// <value>
        /// The references.
        /// </value>
        public override ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> References
        {
            get { return m_references; }
            set
            {
                if (m_references != value)
                {
                    m_references = value;
                    if (m_references == null)
                    {
                        m_references = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>();
                    }

                    NotifyPropertyChanged("References");
                }
            }
        }

        #region Create Composite Component

        /// <summary>
        /// Constructs the graph from selected nodes of the given experiment graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static CompositeComponentGraph ConstructGraphFromSelectedNodes(BaseExperiment originalExperiment)
        {
            var compositeComponentGraph = new CompositeComponentGraph();

            // Clone experiment info.
            compositeComponentGraph.ExperimentInfo = new ExperimentInfo();

            AssureCompleteDecisionSelection(originalExperiment);

            //keep lookup from original node to its clone for later edge reproduction
            Dictionary<ExperimentNode, ExperimentNode> clonedNodeLookup = new Dictionary<ExperimentNode, ExperimentNode>();

            // Clone vertices that are selected and add them to the composite component graph
            foreach (ExperimentNode node in originalExperiment.Vertices)
            {
                if (node.IsSelected)
                {
                    var clonedNode = node.Clone();
                    clonedNodeLookup[node] = clonedNode;
                    compositeComponentGraph.SetLogLevelSettings(clonedNode, compositeComponentGraph.Settings);
                    compositeComponentGraph.AddVertex(clonedNode);

                    if (clonedNode.ID == "Start" && compositeComponentGraph.StartNode == null)
                    {
                        compositeComponentGraph.StartNode = (ExperimentStartNode)clonedNode;
                    }
                    else if (clonedNode.ID == "End" && compositeComponentGraph.EndNode == null)
                    {
                        compositeComponentGraph.EndNode = (ExperimentEndNode)clonedNode;
                    }
                }
            }

            // Clone edges
            foreach (ExperimentNodeConnection connection in originalExperiment.Edges)
            {
                ExperimentNode cloneSourceNode, cloneTargetNode;

                //add edges only if both source and target nodes has been found in clones lookup
                bool foundSourceNode = clonedNodeLookup.TryGetValue(connection.Source, out cloneSourceNode);
                bool foundTargetNode = clonedNodeLookup.TryGetValue(connection.Target, out cloneTargetNode);
                if (foundSourceNode && foundTargetNode)
                {
                    ExperimentNodeConnection clonedConnection = new ExperimentNodeConnection(connection.ID, cloneSourceNode, cloneTargetNode, connection.IsFixed, connection.IsVisible);
                    //copy also all route points
                    clonedConnection.RoutePoints.CopyPointsFrom(connection.RoutePoints);

                    compositeComponentGraph.AddEdge(clonedConnection);

                    //perform fixes for scopes 
                    ScopeNodeHelper.TryFixScopeDecisionEntryAndExitNodes(cloneSourceNode, cloneTargetNode);
                }
            }

            ConnectNodesToStartAndEndNode(compositeComponentGraph);

            //move graph closer to the origin point
            TraceLab.Core.Utilities.ExperimentHelper.MoveGraphCloserToOriginPoint(compositeComponentGraph, 200, 200);
                        
            if (originalExperiment.References != null)
            {
                if (!TraceLabSDK.RuntimeInfo.IsRunInMono)
                {
                    compositeComponentGraph.References = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>(originalExperiment.References);
                }
                else
                {
                    // Mono currently has not implemented constructor of ObservableCollection(IEnumerable<T> collection)
                    // thus we have to add references manually
                    compositeComponentGraph.References = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>();
                    foreach (TraceLabSDK.PackageSystem.IPackageReference reference in originalExperiment.References)
                    {
                        compositeComponentGraph.References.Add(reference);
                    }
                }
            }

            compositeComponentGraph.OwnerNode = null;

            return compositeComponentGraph;
        }

        /// <summary>
        /// Assures that all decision are selected with their scopes.
        /// If decision has been partially selected, meaning only decision with not all scopes was selected,
        /// or only some scopes were selected without decision node, this method will select all nodes
        /// to assure completeness of selection.
        /// </summary>
        /// <param name="originalExperiment">The original experiment.</param>
        private static void AssureCompleteDecisionSelection(BaseExperiment originalExperiment)
        {
            foreach (ExperimentNode node in originalExperiment.Vertices)
            {
                if (node.IsSelected)
                {
                    //case 1. node is ExperimentDecisionNode
                    if (node is ExperimentDecisionNode)
                    {
                        SelectDecisionsScopes(originalExperiment, node);
                    }
                    else if (node is ExitDecisionNode)
                    {
                        SelectDecisionsScopesConnectedToExitNode(originalExperiment, node);
                    }
                    else
                    {
                        //case 2. node is a scope node
                        ScopeNode scopeNode = node as ScopeNode;
                        if (scopeNode != null)
                        {
                            if (!scopeNode.DecisionNode.IsSelected)
                            {
                                scopeNode.DecisionNode.IsSelected = true;
                                SelectDecisionsScopes(originalExperiment, scopeNode.DecisionNode);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Selects the scopes and ExitDecisionNode connected to given decision node
        /// </summary>
        /// <param name="originalExperiment">The original experiment.</param>
        /// <param name="decisionNode">The decision node.</param>
        private static void SelectDecisionsScopes(BaseExperiment originalExperiment, ExperimentNode decisionNode)
        {
            //iterate throught all outcoming edges and delete successor nodes
            IEnumerable<ExperimentNodeConnection> edges;
            if (originalExperiment.TryGetOutEdges(decisionNode, out edges))
            {
                //iterate over the copy of edges, as edges itself are going to be delete when nodes are deleted
                foreach (ExperimentNodeConnection edge in new List<ExperimentNodeConnection>(edges))
                {
                    //check if target is scope or exit, as there are might be old Decision nodes from old experiments
                    //that do not have fixed connections to scopes
                    if (edge.Target is ScopeNode || edge.Target is ExitDecisionNode)
                    {
                        if (!edge.Target.IsSelected)
                            edge.Target.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Selects the decision and its scopes connected to the given exit node
        /// </summary>
        /// <param name="originalExperiment">The original experiment.</param>
        /// <param name="node">The exit decision node.</param>
        private static void SelectDecisionsScopesConnectedToExitNode(BaseExperiment originalExperiment, ExperimentNode exitDecisionNode)
        {
            //iterate throught all outcoming edges and delete successor nodes
            IEnumerable<ExperimentNodeConnection> edges;
            if (originalExperiment.TryGetInEdges(exitDecisionNode, out edges))
            {
                //iterate over the copy of edges, as edges itself are going to be delete when nodes are deleted
                foreach (ExperimentNodeConnection edge in new List<ExperimentNodeConnection>(edges))
                {
                    //check if target is scope or exit, as there are might be old Decision nodes from old experiments
                    //that do not have fixed connections to scopes
                    if (edge.Source is ScopeNode || edge.Source is ExperimentDecisionNode)
                    {
                        if (!edge.Source.IsSelected)
                            edge.Source.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Connects the nodes to start and end node.
        /// In case user selected partial graph without start or/and end node, this methods adds 
        /// accordingly Start and End node to the given composite graph. 
        /// If some selected nodes didn't have any outgoing paths they are automatically connected to the end node.
        /// If some selected nodes didn't have any incoming paths they are automatically connected to the start node.
        /// </summary>
        /// <param name="compositeComponentGraph">The composite component graph.</param>
        private static void ConnectNodesToStartAndEndNode(CompositeComponentGraph compositeComponentGraph)
        {
            if (compositeComponentGraph.StartNode == null || compositeComponentGraph.EndNode == null)
            {
                double startX, startY, endX, endY;

                DetermineOptimalStartEndPositions(compositeComponentGraph, out startX, out startY, out endX, out endY);

                if (compositeComponentGraph.StartNode == null)
                {
                    //create start node and add it to graph, and connect it to the existing graph
                    ExperimentStartNode start = new ExperimentStartNode();
                    start.Data.X = startX;
                    start.Data.Y = startY;
                    compositeComponentGraph.AddVertex(start);
                    compositeComponentGraph.StartNode = start;
                }

                if (compositeComponentGraph.EndNode == null)
                {
                    //create start node and add it to graph, and connect it to the existing graph
                    ExperimentEndNode end = new ExperimentEndNode();
                    end.Data.X = endX;
                    end.Data.Y = endY;
                    compositeComponentGraph.AddVertex(end);
                    compositeComponentGraph.EndNode = end;
                }

                //connect subgraph to start and end
                foreach (ExperimentNode node in compositeComponentGraph.Vertices)
                {
                    if (node is ExperimentStartNode == false && node is ExperimentEndNode == false)
                    {
                        //if node has not any incoming connection from other Selected Nodes it has to be connected to the start
                        if (compositeComponentGraph.IsInEdgesEmpty(node))
                        {
                            compositeComponentGraph.AddEdge(new ExperimentNodeConnection(Guid.NewGuid().ToString(), compositeComponentGraph.StartNode, node));
                        }

                        //if node has not any outcoming connection from other Selected Nodes it has to be connected to the end
                        if (compositeComponentGraph.IsOutEdgesEmpty(node))
                        {
                            compositeComponentGraph.AddEdge(new ExperimentNodeConnection(Guid.NewGuid().ToString(), node, compositeComponentGraph.EndNode));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines the optimal start and end positions.
        /// </summary>
        /// <param name="compositeComponentGraph">The composite component graph.</param>
        /// <param name="startX">The start X.</param>
        /// <param name="startY">The start Y.</param>
        /// <param name="endX">The end X.</param>
        /// <param name="endY">The end Y.</param>
        private static void DetermineOptimalStartEndPositions(CompositeComponentGraph compositeComponentGraph, out double startX, out double startY, out double endX, out double endY)
        {
            double leftX, rightX, topY, bottomY;
            TraceLab.Core.Utilities.ExperimentHelper.DetermineGraphBoundaries(compositeComponentGraph, out leftX, out rightX, out topY, out bottomY);

            startX = 0;
            startY = 0;
            endX = 0;
            endY = 0;

            //calculate optimal start and end position
            if (leftX == rightX)
            {
                startX = leftX;
                endX = leftX;
            }
            else
            {
                double middleX = (leftX + rightX) / 2;
                startX = middleX;
                endX = middleX;
            }

            startY = topY - 50;
            endY = bottomY + 50;
        }

        #endregion
    }
}

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
using System.Linq;
using System.Text;
using TraceLab.Core.Experiments;
using TraceLab.Core.Components;
using System.Windows.Input;
using TraceLab.UI.WPF.Commands;
using System.Windows;

namespace TraceLab.UI.WPF.ViewModels
{
    class TopLevelEditableExperimentViewModel : TopLevelExperimentViewModel, IEditableExperiment
    {
        public TopLevelEditableExperimentViewModel(IEditableExperiment experiment)
            : base(experiment)
        {
            m_experiment = experiment;

            RemoveNode = new DelegateCommand(RemoveNodeFunc, CanRemoveNodeFunc);
            RemoveSelectedNodes = new DelegateCommand(RemoveSelectedNodesFunc, CanRemoveSelectedNodesFunc);
            DragOver = new DelegateCommand(DragOverFunc);
            Drop = new DelegateCommand(DropFunc, CanDropFunc);
            ToogleLogLevel = new DelegateCommand(ToogleLogLevelFunc, CanToogleLogLevelFunc);
        }

        private IEditableExperiment m_experiment;

        #region Remove Node

        /// <summary>
        /// Gets the remove node command 
        /// RemoveNode is a delegate command defined by two functions RemoveNodeFunc and CanRemoveNodeFunc.
        /// This command is bind to the GraphView RemoveNodeCommand dependency property.
        /// For example ScopeGraph and DockableGraph controls in files: GraphDocument, ScopeNodeControl and LoopScopeNode control.
        /// </summary>
        public ICommand RemoveNode
        {
            get;
            private set;
        }

        /// <summary>
        /// Removes the node.
        /// </summary>
        /// <param name="param">The param.</param>
        private void RemoveNodeFunc(object param)
        {
            ExperimentNode vert = param as ExperimentNode;
            if (vert != null && (vert is ComponentNode || vert is ExperimentDecisionNode || vert is CompositeComponentNode))
            {
                RemoveInfoForNode(vert);

                RemoveVertex(vert);
            }
        }

        /// <summary>
        /// Determines whether the specified node given as param can be removed
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> the node can be removed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRemoveNodeFunc(object param)
        {
            bool canRemove = false;
            if (IsExperimentRunning == false)
            {
                ExperimentNode vert = param as ExperimentNode;
                if (vert != null && (vert is ComponentNode || vert is ExperimentDecisionNode || vert is CompositeComponentNode))
                {
                    if (IsNotLastRemainingDecisionScope(vert))
                    {
                        canRemove = true;
                    }
                }
            }

            return canRemove;
        }
        
        /// <summary>
        /// Determines whether the given node is not the last remaining scope that belongs to its decision.
        /// Function returns true for all other types of nodes with one exception.
        /// If the given node is a last remaining scope that belongs to decision the method returns false.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   <c>true</c> if node is not the last remaining decision scope; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNotLastRemainingDecisionScope(ExperimentNode node)
        {
            //as default allow removing any node
            bool canRemove = true;

            //case 1. if node is a scope node -> delete it only if it is not the last scope of the decision
            ScopeNode scopeNode = node as ScopeNode;
            IEnumerable<ExperimentNodeConnection> edges;
            if (scopeNode != null && TryGetOutEdges(scopeNode.DecisionNode, out edges))
            {
                //there is one invisible connection from decision to exit node
                //thus the last remaining scope will be the second connection, so we compare count of edges with 2
                canRemove = (edges.Count() > 2);
            }
           
            return canRemove;
        }

        #endregion

        #region Remove Selected Nodes

        /// <summary>
        /// Gets the remove selected node command 
        /// RemoveSelectedNodes is a delegate command defined by two functions RemoveSelectedNodesFunc and CanRemoveSelectedNodesFunc.
        /// This command is bind to the GraphView RemoveNodeCommand dependency property.
        /// For example ScopeGraph and DockableGraph controls in files: GraphDocument, ScopeNodeControl and LoopScopeNode control.
        /// </summary>
        public ICommand RemoveSelectedNodes
        {
            get;
            private set;
        }

        /// <summary>
        /// Removes the selected nodes specified by param.
        /// </summary>
        /// <param name="param">The param.</param>
        private void RemoveSelectedNodesFunc(object param)
        {
            // Display message box
            MessageBoxResult result = MessageBox.Show(TraceLab.Core.Messages.ConfirmRemoveSelectedNodes,
                                                      TraceLab.Core.Messages.DoYouWantToContinue, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                RemoveSelectedVertices();
            }
        }

        /// <summary>
        /// Determines whether selected nodes specified by param can be removed
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if selected nodes can be removed; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRemoveSelectedNodesFunc(object param)
        {
            bool isAnyNodeSelected = false;
            var enumerator = Vertices.GetEnumerator();
            while (isAnyNodeSelected == false && enumerator.MoveNext())
            {
                var node = enumerator.Current;
                if (node is ExperimentStartNode == false && node is ExperimentEndNode == false)
                {
                    isAnyNodeSelected = node.IsSelected;
                }
            }
            return isAnyNodeSelected;
        }
        
        /// <summary>
        /// Removes the info panes for selected nodes.
        /// </summary>
        private void RemoveInfoForSelectedNodes()
        {
            foreach (ExperimentNode vert in Vertices)
            {
                if (vert.IsSelected == true && vert != null && (vert is ComponentNode || vert is ExperimentDecisionNode || vert is CompositeComponentNode))
                {
                    RemoveInfoForNode(vert);
                }
            }
        }

        #endregion

        #region ToogleLogLevel on selected nodes

        /// <summary>
        /// Gets the toogle log level.
        /// ToogleLogLevel is a delegate command defined by two functions ToogleLogLevelFunc and CanToogleLogLevelFunc.
        /// This command is bind to the GraphView ToogleLogLevelCommand dependency property.
        /// For example ScopeGraph and DockableGraph controls in files: GraphDocument, ScopeNodeControl and LoopScopeNode control.
        /// </summary>
        public ICommand ToogleLogLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// Toogles the log level
        /// </summary>
        /// <param name="param">The param.</param>
        private void ToogleLogLevelFunc(object param)
        {
            var item = param as TraceLab.Core.Settings.LogLevelItem;

            //if log item is not locked 
            if (item != null && item.IsLocked == false)
            {
                //set to opposite value
                item.IsEnabled = !item.IsEnabled;

                //also apply change to all selected nodes
                ToogleLogLevelOnSelectedNodes(item.IsEnabled, item.Level);
            }
        }

        /// <summary>
        /// Determines whether log level can be toogled
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if the log level can be toogled; otherwise, <c>false</c>.
        /// </returns>
        private bool CanToogleLogLevelFunc(object param)
        {
            bool canDo = false;
            var item = param as TraceLab.Core.Settings.LogLevelItem;

            //if log item is not locked 
            if (item != null && item.IsLocked == false)
            {
                //check if there are any nodes selected
                bool isAnyNodeSelected = false;
                var enumerator = Vertices.GetEnumerator();
                while (isAnyNodeSelected == false && enumerator.MoveNext())
                {
                    var node = enumerator.Current;
                    if (node is ExperimentStartNode == false && node is ExperimentEndNode == false)
                    {
                        isAnyNodeSelected = node.IsSelected;
                    }
                }
                canDo = isAnyNodeSelected;
            }

            return canDo;
        }
        
        /// <summary>
        /// Toogles the logging on selected nodes of the given level. (implements interface)
        /// </summary>
        /// <param name="enable">if set to <c>true</c> it enables the logging for the given level on all selected nodes.</param>
        /// <param name="level">The level.</param>
        public void ToogleLogLevelOnSelectedNodes(bool enable, NLog.LogLevel logLevel)
        {
            m_experiment.ToogleLogLevelOnSelectedNodes(enable, logLevel);
        }

        #endregion

        #region Drop

        /// <summary>
        /// Gets the drop command
        /// </summary>
        public ICommand Drop
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines whether the drop command can be executed
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can drop func] the specified param; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDropFunc(object param)
        {
            bool canDrop = false;

            System.Windows.DragEventArgs args = param as System.Windows.DragEventArgs;
            TraceLab.UI.WPF.EventArgs.DropLinkEventArgs linkArgs = param as TraceLab.UI.WPF.EventArgs.DropLinkEventArgs;

            if (IsExperimentRunning == false)
            {
                var sourceNode = linkArgs.Source as ExperimentNode;
                var targetNode = linkArgs.Target as ExperimentNode;
                canDrop |= (linkArgs != null &&
                            IsValidLinkSource(sourceNode) &&
                            (linkArgs.ExistingEdge != null || IsValidLinkTarget(targetNode)));

                if(sourceNode != null && targetNode != null) 
                {
                    canDrop &= sourceNode.Owner == targetNode.Owner;
                }

                canDrop |= (args != null && args.Data.GetDataPresent("ComponentDefinition"));
            }
            else
            {
                if (args != null)
                {
                    args.Effects = System.Windows.DragDropEffects.None;
                }
            }
            return canDrop;
        }

        /// <summary>
        /// Executes drop
        /// </summary>
        /// <param name="param">The param.</param>
        private void DropFunc(object param)
        {
            if (IsExperimentRunning == false)
            {
                TraceLab.UI.WPF.EventArgs.GraphDragEventArgs args = param as TraceLab.UI.WPF.EventArgs.GraphDragEventArgs;
                TraceLab.UI.WPF.EventArgs.DropLinkEventArgs linkArgs = param as TraceLab.UI.WPF.EventArgs.DropLinkEventArgs;
                if (args != null)
                {
                    MetadataDefinition metadataDefinition = args.DragArguments.Data.GetData("ComponentDefinition") as MetadataDefinition;

                    if (metadataDefinition != null)
                    {
                        System.Windows.Point pos = args.Position;
                        AddComponentFromDefinition(metadataDefinition, pos.X, pos.Y);
                    }
                }
                else if (linkArgs != null)
                {
                    ExperimentNodeConnection edge = (ExperimentNodeConnection)linkArgs.ExistingEdge;

                    // If we're NOT removing an edge, OR the new edge will be different than the old edge.
                    if (linkArgs.ExistingEdge == null || (edge.Source == linkArgs.Source && edge.Target != linkArgs.Target))
                    {
                        if (linkArgs.ExistingEdge != null)
                        {
                            RemoveConnection(edge);
                        }

                        if (linkArgs.Source != null && linkArgs.Target != null)
                        {
                            AddConnection((ExperimentNode)linkArgs.Source, (ExperimentNode)linkArgs.Target);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified node is a valid link source
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   <c>true</c> if the specified node is a valid link source; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidLinkSource(ExperimentNode node)
        {
            bool isValid = false;
            if (node != null && (node is ExperimentStartNode || node is ComponentNode || node is ExperimentDecisionNode || node is CompositeComponentNode || node is ExitDecisionNode))
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Determines whether the specified node is a valid link target
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   <c>true</c> if the specified node is a valid link target; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidLinkTarget(ExperimentNode node)
        {
            bool isValid = false;

            //note that a HACK_Node is an invisible node, that is used as a target node of the link that is being dragged
            if (node != null && node.ID != "HACK_Node" && node is ScopeNode == false &&
                (node is ExperimentEndNode || node is ComponentNode || node is ExperimentDecisionNode || node is CompositeComponentNode))
            {
                isValid = true;
            }
            return isValid;
        }
        
        #endregion

        #region Drag

        /// <summary>
        /// Gets the drag over command
        /// </summary>
        public ICommand DragOver
        {
            get;
            private set;
        }

        /// <summary>
        /// Executes drag over
        /// </summary>
        /// <param name="param">The param.</param>
        private void DragOverFunc(object param)
        {
            TraceLab.UI.WPF.EventArgs.GraphDragEventArgs args = param as TraceLab.UI.WPF.EventArgs.GraphDragEventArgs;
            MetadataDefinition metadataDefinition = args.DragArguments.Data.GetData("ComponentDefinition") as MetadataDefinition;

            args.DragArguments.Effects = System.Windows.DragDropEffects.None;
            if (metadataDefinition != null && IsExperimentRunning == false)
            {
                args.DragArguments.Effects = System.Windows.DragDropEffects.Copy;
            }
        }

        #endregion

        #region IEditableExperiment Members

        /// <summary>
        /// Adds a new component at the specified coordinates
        /// </summary>
        /// <param name="metadataDefinition"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the component definition is null</exception>
        public ExperimentNode AddComponentFromDefinition(MetadataDefinition metadataDefinition, double positionX, double positionY)
        {
            return m_experiment.AddComponentFromDefinition(metadataDefinition, positionX, positionY);
        }
        
        /// <summary>
        /// Adds a connection between two vertices
        /// </summary>
        /// <param name="fromNode">The source node</param>
        /// <param name="toNode">The target node</param>
        /// <returns>The newly created connection</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if either fromNode or toNode is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if creating the link would create an invalid graph, for s_instance if it creates an improper circular graph
        /// </exception>
        public ExperimentNodeConnection AddConnection(ExperimentNode fromNode, ExperimentNode toNode)
        {
            return m_experiment.AddConnection(fromNode, toNode);
        }

        /// <summary>
        /// Removes the given connection from the m_experiment.
        /// </summary>
        /// <param name="edge">The edge to remove</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the ViewModel is in a bad state that it cannot recover from</exception>
        public void RemoveConnection(ExperimentNodeConnection edge)
        {
            m_experiment.RemoveConnection(edge);
        }

        /// <summary>
        /// Removes the vertex.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public bool RemoveVertex(ExperimentNode v)
        {
            return m_experiment.RemoveVertex(v);
        }
        
        /// <summary>
        /// Removes the selected vertices. (interface implementation)
        /// </summary>
        public void RemoveSelectedVertices()
        {
            RemoveInfoForSelectedNodes();
            m_experiment.RemoveSelectedVertices();
        }

        /// <summary>
        /// Adds the fixed connection.
        /// </summary>
        /// <param name="fromNode">From node.</param>
        /// <param name="toNode">To node.</param>
        /// <param name="isVisible">if set to <c>true</c> edge is visible.</param>
        /// <returns>
        /// the added connection
        /// </returns>
        public ExperimentNodeConnection AddFixedConnection(ExperimentNode fromNode, ExperimentNode toNode, bool isVisible)
        {
            return m_experiment.AddFixedConnection(fromNode, toNode, isVisible);
        }

        /// <summary>
        /// Adds the node vertex to the experiment
        /// </summary>
        /// <param name="node">The node to be added</param>
        /// <returns>
        /// true if node was added
        /// </returns>
        public bool AddVertex(ExperimentNode node)
        {
            return m_experiment.AddVertex(node);
        }

        /// <summary>
        /// Sets the log level current experiment settings on the node.
        /// </summary>
        /// <param name="node">The node which log level setting are set.</param>
        public void SetLogLevelSettings(ExperimentNode node)
        {
            m_experiment.SetLogLevelSettings(node);
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public TraceLab.Core.Settings.Settings Settings
        {
            get { return m_experiment.Settings; }
        }

        #endregion
    }
}
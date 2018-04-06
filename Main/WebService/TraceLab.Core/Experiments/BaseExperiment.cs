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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using QuickGraph;
using TraceLab.Core.Components;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Utilities;
using TraceLab.Core.Workspaces;
using TraceLabSDK;

namespace TraceLab.Core.Experiments
{
    public abstract class BaseExperiment : BidirectionalGraph<ExperimentNode, ExperimentNodeConnection>, IExperiment, INotifyPropertyChanged
    {
        #region Constructor

        public BaseExperiment()
        {
            //initiate experiment info with new guid
            ExperimentInfo = new ExperimentInfo();
            
            EdgeAdded += NotifyEdgesCountChange;
            EdgeRemoved += NotifyEdgesCountChange;
        }

        private void NotifyEdgesCountChange(ExperimentNodeConnection e)
        {
            e.Target.FireIncomingEdgesCountPropertyChanged();
        }
        
        public BaseExperiment(string name, string filepath)
            : this()
        {
            ExperimentInfo.Name = name;
            ExperimentInfo.FilePath = filepath;
            ExperimentInfo.LayoutName = "EfficientSugiyama";
        }

        #endregion

        private TraceLab.Core.Settings.Settings m_settings;
        public TraceLab.Core.Settings.Settings Settings
        {
            get { return m_settings; }
            set
            {
                if (value != m_settings)
                {
                    m_settings = value;

                    //update all components and sublevel components to the new settings
                    foreach (ExperimentNode experimentNode in Vertices)
                    {
                        SetLogLevelSettings(experimentNode, value);

                        CompositeComponentNode compositeComponentNode = experimentNode as CompositeComponentNode;
                        if (compositeComponentNode != null && compositeComponentNode.CompositeComponentMetadata.ComponentGraph != null)
                        {
                            compositeComponentNode.CompositeComponentMetadata.ComponentGraph.Settings = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cache off a lookup of Node ID to Node - this will be used by the NodeEvent
        /// handlers when running the experiment
        /// </summary>
        private Dictionary<string, ExperimentNode> m_idMap = new Dictionary<string, ExperimentNode>();

        private ExperimentInfo m_experimentInfo;
        [XmlElement("ExperimentInfo")]
        public ExperimentInfo ExperimentInfo
        {
            get
            {
                return m_experimentInfo;
            }
            set
            {
                m_experimentInfo = value;
                m_experimentInfo.PropertyChanged += ExperimentInfoPropertyChanged;
            }
        }

        public string Title
        {
            get
            {
                return ExperimentInfo.Name;
            }
        }

        private bool m_isExperimentRunning = false;
        public bool IsExperimentRunning
        {
            get
            {
                return m_isExperimentRunning;
            }
            set
            {
                if (m_isExperimentRunning != value)
                {
                    m_isExperimentRunning = value;
                    NotifyPropertyChanged("IsExperimentRunning");
                }
            }
        }

        private void ExperimentInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        #region IExperiment Members

        public ExperimentStartNode StartNode
        {
            get;
            protected set;
        }

        public ExperimentEndNode EndNode
        {
            get;
            protected set;
        }

        /// <summary>
        /// This method searches through all existing verices and searches for start and end node to update the references.
        /// It is needed after loading the experiment from the xml file. 
        /// </summary>
        public void ReloadStartAndEndNode()
        {
            IEnumerator<ExperimentNode> iterator = Vertices.GetEnumerator();

            // TODO: Remove the ID mappings once we've converted all saved graphs to the new Start/End metadata
            while (iterator.MoveNext() && (StartNode == null || EndNode == null))
            {

                ExperimentNode node = iterator.Current;

                if (node.Data.Metadata is EndNodeMetadata || node.ID == "End")
                {
                    EndNode = (ExperimentEndNode)node;
                }
                else if (node.Data.Metadata is StartNodeMetadata || node.ID == "Start")
                {
                    StartNode = (ExperimentStartNode)node;
                }
            }

            if (StartNode == null || EndNode == null)
            {
                throw new ArgumentException("Experiment does not have Start or End node. Experiment file has been corrupted.");
            }
        }

        private ObservableDictionary<ExperimentNode, ExperimentNodeError> m_errors = new ObservableDictionary<ExperimentNode, ExperimentNodeError>();
        public ObservableDictionary<ExperimentNode, ExperimentNodeError> Errors
        {
            get
            {
                return m_errors;
            }
        }

        /// <summary>
        /// Sets the log level current experiment settings on the node.
        /// It has internal access to allow ComponentFactory to set settings of nodes that are contructed during the process
        /// </summary>
        /// <param name="node">The node which log level setting are set.</param>
        public void SetLogLevelSettings(ExperimentNode node)
        {
            SetLogLevelSettings(node, Settings);
        }

        /// <summary>
        /// Sets the log level settings on the node
        /// </summary>
        /// <param name="node">The node which log level setting are set.</param>
        /// <param name="settings">The settings to be used to set log level.</param>
        protected void SetLogLevelSettings(ExperimentNode node, TraceLab.Core.Settings.Settings settings)
        {
            if (settings != null && settings.ExperimentSettings != null)
            {
                // set components log level settings according to global log level settings.
                foreach (TraceLab.Core.Settings.GlobalLogLevelSetting setting in settings.ExperimentSettings.GlobalLogLevelsSettings)
                {
                    node.Data.Metadata.SetLogLevel(setting.Level, setting.IsEnabled, setting.IsLocked);
                }
            }

            node.Data.Metadata.ListenToGlobalLogLevelSettingChange(settings);
        }

        /// <summary>
        /// Adds the node vertex to the experiment
        /// </summary>
        /// <param name="node">The node to be added</param>
        /// <returns></returns>
        public override bool AddVertex(ExperimentNode node)
        {
            if (node.HasError == true)
            {
                Errors.Add(node, node.Error);
            }
            node.ErrorChanged += OnNodeErrorChanged;
            node.Owner = this;

            m_idMap.Add(node.ID, node);

            return base.AddVertex(node);
        }

        #region Remove Vertex

        /// <summary>
        /// Removes the vertex. It handles special case of decision node, that
        /// also must delete its corresponding nodes
        /// </summary>
        /// <param name="node">The node to be removed from experiment</param>
        /// <returns></returns>
        public override bool RemoveVertex(ExperimentNode node)
        {
            //case 1: node is decision -> delete all scopes belonging to this decision node
            if (node is ExperimentDecisionNode)
            {
                return DeleteDecisionNodeWithItsScopes(node);
            }
            else 
            {
                return ExecuteRemoveVertex(node);
            }
        }

        /// <summary>
        /// Deletes the decision node with its scopes and exit decision node.
        /// Assumption is that all scopes and exit decision node are connected to the Decision.
        /// Currently it is always the case.
        /// Adjust the code if that logic is no longer in place.
        /// </summary>
        /// <param name="decisionNode">The decision node.</param>
        /// <returns></returns>
        private bool DeleteDecisionNodeWithItsScopes(ExperimentNode decisionNode)
        {
            //iterate throught all outcoming edges and delete successor nodes
            IEnumerable<ExperimentNodeConnection> edges;
            if (TryGetOutEdges(decisionNode, out edges))
            {
                // HERZUM SPRINT 2.0 TLAB-148
                //iterate over the copy of edges, as edges itself are going to be delete when nodes are deleted
                bool found = false;
                foreach (ExperimentNodeConnection edge in new List<ExperimentNodeConnection>(edges))
                {
                    if (edge.Target is ExitDecisionNode)
                        found = true;
                }
                if (found)
                // END HERZUM SPRINT 2.0 TLAB-148

                //iterate over the copy of edges, as edges itself are going to be delete when nodes are deleted
                foreach (ExperimentNodeConnection edge in new List<ExperimentNodeConnection>(edges))
                {
                    //check if target is scope or exit, as there are might be old Decision nodes from old experiments
                    //that do not have fixed connections to scopes
                    if (edge.Target is ScopeNode || edge.Target is ExitDecisionNode)
                    {
                        ExecuteRemoveVertex(edge.Target);
                    }
                }
            }
            //finally delete decision node itself
            return ExecuteRemoveVertex(decisionNode);
        }

        /// <summary>
        /// Performs actual delete of the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private bool ExecuteRemoveVertex(ExperimentNode node)
        {
            if (node.HasError == true)
            {
                Errors.Remove(node);
            }
            node.ErrorChanged -= OnNodeErrorChanged;

            m_idMap.Remove(node.ID);

            return base.RemoveVertex(node);
        }

        // HERZUM SPRINT 2.3 TLAB-140
        /// <summary>
        /// Removes the selected vertices.
        /// </summary>
        /*
        public void RemoveSelectedVertices()
        {
            foreach (ExperimentNode node in new List<ExperimentNode>(Vertices))
            {
                //don't remove scope nodes and exit decisions nodes directly - if decision node was also selected that logic will
                //remove its correposding nodes
                if (node.IsSelected == true && node is ExperimentStartNode == false && node is ExperimentEndNode == false
                    && node is ScopeNode == false && node is ExitDecisionNode == false)
                {
                    RemoveVertex(node);
                }
            }
        }
        */
        public void RemoveSelectedVertices()
        {
            foreach (ExperimentNode node in new List<ExperimentNode>(Vertices))
            {
                //don't remove scope nodes and exit decisions nodes directly - if decision node was also selected that logic will
                //remove its correposding nodes
                if (node.IsSelected == true && node is ExperimentStartNode == false && node is ExperimentEndNode == false
                    && (node is ScopeNode == false ||(node is ScopeNode == true && (node as ScopeNode).DecisionNode == null)) && node is ExitDecisionNode == false)
                {
                    RemoveVertex(node);
                }
            }
        }
        // END HERZUM SPRINT 2.3 TLAB-140

        #endregion Remove Vertex

        /// <summary>
        /// Toogles the logging on selected nodes of the given level. 
        /// </summary>
        /// <param name="enable">if set to <c>true</c> it enables the logging for the given level on all selected nodes.</param>
        /// <param name="level">The level.</param>
        public void ToogleLogLevelOnSelectedNodes(bool enable, NLog.LogLevel level)
        {
            foreach (ExperimentNode node in Vertices)
            {
                if (node.IsSelected == true && node is ExperimentStartNode == false && node is ExperimentEndNode == false)
                {
                    foreach (TraceLab.Core.Settings.LogLevelItem logItem in node.Data.Metadata.LogLevels)
                    {
                        if (logItem.Level == level)
                        {
                            logItem.IsEnabled = enable;
                        }
                    }
                }
            }
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
            if (fromNode == null)
                throw new ArgumentNullException("fromNode");
            if (toNode == null)
                throw new ArgumentNullException("toNode");
            if (toNode is ExperimentStartNode || toNode.ID.Equals("START", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException("Cannot link to Start node.");

            ExperimentNodeConnection edge;
            if (TryGetEdge(fromNode, toNode, out edge) == false)
            {
                edge = new ExperimentNodeConnection(Guid.NewGuid().ToString(), fromNode, toNode);
                AddEdge(edge);
            }
            return edge;
        }

        /// <summary>
        /// Adds the fixed connection.
        /// </summary>
        /// <param name="fromNode">From node.</param>
        /// <param name="toNode">To node.</param>
        /// <param name="isVisible">if set to <c>true</c> edge is visible.</param>
        /// <returns>the added connection</returns>
        public ExperimentNodeConnection AddFixedConnection(ExperimentNode fromNode, ExperimentNode toNode, bool isVisible)
        {
            ExperimentNodeConnection edge;
            if (TryGetEdge(fromNode, toNode, out edge) == false)
            {
                edge = new ExperimentNodeConnection(Guid.NewGuid().ToString(), fromNode, toNode, true, isVisible);
                AddEdge(edge);
            }
            return edge;
        }

        /// <summary>
        /// Removes the given connection from the m_experiment.
        /// </summary>
        /// <param name="edge">The edge to remove</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the ViewModel is in a bad state that it cannot recover from</exception>
        public void RemoveConnection(ExperimentNodeConnection edge)
        {
            RemoveEdge(edge);
        }

        /// <summary>
        /// Gets the node of the specific node id. If Node was not found it returns null.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Node with given id. Null if not found.
        /// </returns>
        public ExperimentNode GetNode(string id)
        {
            ExperimentNode found = null;
            m_idMap.TryGetValue(id, out found);
            return found;
        }

        /// <summary>
        /// Clears all the errors in all vertices, including errors in all subexperiments and their vertices.
        /// </summary>
        public void ClearErrors()
        {
            foreach (ExperimentNode node in m_idMap.Values)
            {
                TraceLab.Core.Experiments.CompositeComponentNode compositeComponentNode = node as TraceLab.Core.Experiments.CompositeComponentNode;
                if (compositeComponentNode != null)
                {
                    compositeComponentNode.ClearError();
                }
                node.ClearError();
            }
        }

        /// <summary>
        /// Occurs when node is added
        /// </summary>
        public event VertexAction<ExperimentNode> NodeAdded
        {
            add
            {
                base.VertexAdded += value;
            }
            remove
            {
                base.VertexAdded -= value;
            }
        }

        /// <summary>
        /// Occurs when when node is removed
        /// </summary>
        public event VertexAction<ExperimentNode> NodeRemoved
        {
            add
            {
                base.VertexRemoved += value;
            }
            remove
            {
                base.VertexRemoved -= value;
            }
        }

        public event QuickGraph.EdgeAction<ExperimentNode, ExperimentNodeConnection> EdgeAdded
        {
            add
            {
                base.EdgeAdded += value;
            }
            remove
            {
                base.EdgeAdded -= value;
            }
        }
        
        public event QuickGraph.EdgeAction<ExperimentNode, ExperimentNodeConnection> EdgeRemoved
        {
            add
            {
                base.EdgeRemoved += value;
            }
            remove
            {
                base.EdgeRemoved -= value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of references to the packages
        /// </summary>
        /// <value>
        /// The references.
        /// </value>
        public abstract ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> References
        {
            get;
            set;
        }


        #endregion

        private object lockErrors = new object();

        /// <summary>
        /// Called when node error changed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="errorArgs">The <see cref="TraceLab.Core.Experiments.ExperimentNodeErrorEventArgs"/> instance containing the event data.</param>
        private void OnNodeErrorChanged(object sender, ExperimentNodeErrorEventArgs errorArgs)
        {
            ExperimentNode node = sender as ExperimentNode;
            if (node != null)
            {
                if (errorArgs.NodeError == null)
                {
                    lock (lockErrors)
                    {
                        m_errors.Remove(node);
                    }
                }
                else
                {
                    ExperimentNodeError nodeError;
                    if (m_errors.TryGetValue(node, out nodeError))
                    {
                        lock (lockErrors)
                        {
                            nodeError = errorArgs.NodeError;
                        }
                    }
                    else
                    {
                        lock (lockErrors)
                        {
                            m_errors.Add(node, errorArgs.NodeError);
                        }
                    }
                }
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        new public abstract BaseExperiment Clone();

        /// <summary>
        /// Copies data into this experiment from another given experiment
        /// </summary>
        /// <param name="other">The other experiment from which data is copied.</param>
        protected virtual void CopyFrom(BaseExperiment other)
        {
            // Clone experiment info.
            this.m_experimentInfo = other.m_experimentInfo.Clone();

            // Clone vertices
            Dictionary<ExperimentNode, ExperimentNode> clonedNodeLookup = new Dictionary<ExperimentNode, ExperimentNode>();
            foreach (ExperimentNode node in other.Vertices)
            {
                var clonedNode = node.Clone();
                clonedNodeLookup[node] = clonedNode;
                SetLogLevelSettings(clonedNode, Settings);
                AddVertex(clonedNode);
            }

            // Populate our Start and End node properties - this can only be done after the vertices are added.
            ReloadStartAndEndNode();

            // Clone edges
            foreach (ExperimentNodeConnection connection in other.Edges)
            {
                ExperimentNode cloneSource = clonedNodeLookup[connection.Source];
                ExperimentNode cloneTarget = clonedNodeLookup[connection.Target];
                ExperimentNodeConnection clonedConnection = new ExperimentNodeConnection(connection.ID, cloneSource, cloneTarget, connection.IsFixed, connection.IsVisible);
                //copy also all route points
                clonedConnection.RoutePoints.CopyPointsFrom(connection.RoutePoints);

                AddEdge(clonedConnection);

                //special case - fix scope node references
                ScopeNodeHelper.TryFixScopeDecisionEntryAndExitNodes(cloneSource, cloneTarget);
            }

            // Do deep copy of errors.
            this.m_errors = new ObservableDictionary<ExperimentNode, ExperimentNodeError>();
            foreach (KeyValuePair<ExperimentNode, ExperimentNodeError> pair in other.m_errors)
            {
                m_errors[clonedNodeLookup[pair.Key]] = new ExperimentNodeError(pair.Value.ErrorMessage, pair.Value.ErrorType);
            }

            if (other.References != null)
            {
                References = other.References.CopyCollection();
            }
        }


        // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
        public void CopyAndAdd(CompositeComponentGraph subExperiment, double x, double y)
        {
            if (subExperiment == null)
                return;

            CopyAndAdd (subExperiment, x, y, true);
        }

        public void CopyAndAdd(CompositeComponentGraph subExperiment, double x, double y, bool first)
        {
            if (subExperiment == null)
                return;

            if (subExperiment.IsVerticesEmpty)
                return;

            bool firstNode = true;
            double minX = 0;
            double minY = 0;
            double offsetX = 0;
            double offsetY = 0;

            if (first){

                foreach (ExperimentNode node in subExperiment.Vertices)   
                // HERZUM SPRINT 2.6 TLAB-175         
                // if (node.ID != "Start" && node.ID != "End"){
                if ((node.ID != "Start" && node.ID != "End") && !(first && (node is ExperimentStartNode || node is ExperimentEndNode))) {
                // END HERZUM SPRINT 2.6 TLAB-175
                    if (firstNode){
                        minX = node.Data.X;
                        minY = node.Data.Y;
                        firstNode = false;
                    }
                    else {
                        if (node.Data.X < minX) minX = node.Data.X;
                        if (node.Data.Y < minY) minY = node.Data.Y;
                    }
                }
            }

            offsetX = x - minX;
            offsetY = y - minY;

            //keep lookup from original node to its clone for later edge reproduction
            Dictionary<ExperimentNode, ExperimentNode> clonedNodeLookup = new Dictionary<ExperimentNode, ExperimentNode>();

            // Clone vertices and add them to the composite component graph
            foreach (ExperimentNode node in subExperiment.Vertices)
            {
                // HERZUM SPRINT 2.6 TLAB-175
                // if (node.ID != "Start" && node.ID != "End")
                if ((node.ID != "Start" && node.ID != "End") && !(first && (node is ExperimentStartNode || node is ExperimentEndNode)))
                // END HERZUM SPRINT 2.6 TLAB-175
                {
                    var clonedNode = node.Clone();
                    clonedNode.ID = Guid.NewGuid().ToString();
                    SetLogLevelSettings(clonedNode, Settings);
                    clonedNodeLookup[node] = clonedNode;

                    clonedNode.Owner = this;

                    if (first){
                        clonedNode.Data.X = clonedNode.Data.X + offsetX;
                        clonedNode.Data.Y = clonedNode.Data.Y + offsetY;
                    }

                    AddVertex(clonedNode);

                    ScopeBaseMetadata scopeBaseMetadata = node.Data.Metadata as ScopeBaseMetadata;
                    ScopeBaseMetadata clonedScopeBaseMetadata = clonedNode.Data.Metadata as ScopeBaseMetadata;
                    if ((scopeBaseMetadata != null && clonedScopeBaseMetadata!= null) && 
                        (scopeBaseMetadata.ComponentGraph!=null))
                    {
                        clonedScopeBaseMetadata.ComponentGraph.Clear();
                        clonedScopeBaseMetadata.ComponentGraph.GetExperiment().ExperimentInfo = clonedScopeBaseMetadata.ComponentGraph.GetExperiment().ExperimentInfo.CloneWithNewId();
                        clonedScopeBaseMetadata.ComponentGraph.CopyAndAdd(scopeBaseMetadata.ComponentGraph,x,y,false);

                    }
                }
            }

            // Clone edges
            foreach (ExperimentNodeConnection connection in subExperiment.Edges)
            {
                ExperimentNode cloneSourceNode, cloneTargetNode;

                //add edges only if both source and target nodes has been found in clones lookup
                bool foundSourceNode = clonedNodeLookup.TryGetValue(connection.Source, out cloneSourceNode);
                bool foundTargetNode = clonedNodeLookup.TryGetValue(connection.Target, out cloneTargetNode);
                if (foundSourceNode && foundTargetNode)
                {
                    ExperimentNodeConnection clonedConnection = new ExperimentNodeConnection(Guid.NewGuid().ToString(), cloneSourceNode, cloneTargetNode, connection.IsFixed, connection.IsVisible);
                    //copy also all route points
                    clonedConnection.RoutePoints.CopyPointsFrom(connection.RoutePoints);
                    AddEdge(clonedConnection);

                    //special case - fix scope node references
                    ScopeNodeHelper.TryFixScopeDecisionEntryAndExitNodes(cloneSourceNode, cloneTargetNode);
                }
            }

            // HERZUM SPRINT 2.4 TLAB-56 TLAB-57 TLAB-58 TLAB-59
            if (first){
                if (subExperiment.References != null)
                {
                    References = subExperiment.References.CopyCollection();
                }
            }
            // HERZUM SPRINT 2.4 TLAB-56 TLAB-57 TLAB-58 TLAB-59

            // Do deep copy of errors.
            this.m_errors = new ObservableDictionary<ExperimentNode, ExperimentNodeError>();
            foreach (KeyValuePair<ExperimentNode, ExperimentNodeError> pair in m_errors)
            {
                m_errors[clonedNodeLookup[pair.Key]] = new ExperimentNodeError(pair.Value.ErrorMessage, pair.Value.ErrorType);
            }
        }

        // HERZUM SPRINT 4: TLAB-215
        public bool ThereAreNodeErrors(out string message)
        {
            bool wrong = false;
            string localMessage = "";
            string subMessage = "";
            // HERZUM SPRINT 4.2: TLAB-215
            // if (this.Errors.Count > 0){
            // END HERZUM SPRINT 4.2: TLAB-215
                foreach (ExperimentNode node in Vertices){ 
                    if (node.Error!= null && node.Error.ErrorMessage != null)
                        localMessage = localMessage + "\r\n" + node.Data.Metadata.Label + " Component: " + node.Error.ErrorMessage + "\r\n";
                    ScopeBaseMetadata meta = node.Data.Metadata as ScopeBaseMetadata;
                    if (meta != null && meta.ComponentGraph !=null){ 
                        wrong = meta.ComponentGraph.ThereAreNodeErrors(out subMessage);
                        localMessage = localMessage + subMessage;
                    }
                }
            // HERZUM SPRINT 4.2: TLAB-215
            // }
            // END HERZUM SPRINT 4.2: TLAB-215
            message = localMessage;
            return (this.Errors.Count > 0) || wrong;
        }
        // END HERZUM SPRINT 4: TLAB-215

        // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

        #endregion

        #region INotifyPropertyChanged Members

        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
            }
            remove
            {
                m_propertyChanged -= value;
            }
        }

        protected void NotifyPropertyChanged(string property)
        {
            NotifyPropertyChanged(new PropertyChangedEventArgs(property));
        }

        private void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, e);
        }

        #endregion

        #region IModifiable Members

        private bool m_isModified;
        private bool m_deferModification;
        public virtual bool IsModified
        {
            get
            {
                if (m_deferModification)
                {
                    m_isModified = CalculateModification();
                    m_deferModification = false;
                }

                return m_isModified;
            }
            set
            {
                if (m_isModified != value && m_deferModification == false)
                {
                    m_deferModification = true;
                    NotifyPropertyChanged("IsModified");
                }
            }
        }

        public virtual void ResetModifiedFlag()
        {
            ExperimentInfo.ResetModifiedFlag();

            foreach (ExperimentNode node in Vertices)
            {
                node.ResetModifiedFlag();
            }

            foreach (ExperimentNodeConnection connection in Edges)
            {
                connection.ResetModifiedFlag();
            }

            IsModified = false;
        }

        protected virtual bool CalculateModification()
        {
            bool isModified = false;

            isModified |= ExperimentInfo.IsModified;

            foreach (ExperimentNode node in Vertices)
            {
                isModified |= node.IsModified;
            }

            foreach (ExperimentNodeConnection connection in Edges)
            {
                isModified |= connection.IsModified;
            }

            return isModified;
        }

        #endregion

        #region Node execution changes

        internal void dispatcher_NodeFinished(object sender, NodeEventArgs e)
        {
            ExperimentNode found = GetNode(e.NodeId);
            if (found != null)
            {
                found.IsExecuting = false;
                found.ClearError();
                OnNodeFinished(found);
            }
        }

        internal void dispatcher_NodeHasError(object sender, NodeEventArgs e)
        {
            ExperimentNode found = GetNode(e.NodeId);
            if (found != null)
            {
                found.IsExecuting = false;
                found.SetError(e.ErrorText);
                OnNodeHasError(found);
            }
        }

        internal void dispatcher_NodeExecuting(object sender, NodeEventArgs e)
        {
            ExperimentNode found = GetNode(e.NodeId);
            if (found != null)
            {
                found.IsExecuting = true;
                OnNodeStarted(found);
            }
        }

        public event EventHandler<ExperimentNodeEventArgs> NodeStarted;
        private void OnNodeStarted(ExperimentNode node)
        {
            if (NodeStarted != null)
            {
                NodeStarted(this, new ExperimentNodeEventArgs(node));
            }
        }

        public event EventHandler<ExperimentNodeEventArgs> NodeFinished;
        private void OnNodeFinished(ExperimentNode node)
        {
            if (NodeFinished != null)
            {
                NodeFinished(this, new ExperimentNodeEventArgs(node));
            }
        }

        public event EventHandler<ExperimentNodeEventArgs> NodeHasError;
        private void OnNodeHasError(ExperimentNode node)
        {
            if (NodeHasError != null)
            {
                NodeHasError(this, new ExperimentNodeEventArgs(node));
            }
        }

        #endregion

        #region Run/Destroy Experiment

        /// <summary>
        /// Creates the experiment.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <param name="baseline">The baseline - if baseline is different than null it is going to be written into workspace before executing the experiment
        /// with the Unitname BASELINE.</param>
        /// <returns></returns>
        private IExperimentRunner CreateExperimentRunner(Workspace workspace, ComponentsLibrary library, TraceLabSDK.Types.Contests.TLExperimentResults baseline)
        {
            RunnableExperimentBase graph = null;

            var experimentWorkspaceWrapper = WorkspaceWrapperFactory.CreateExperimentWorkspaceWrapper(workspace, ExperimentInfo.Id);

            RunnableNodeFactory templateGraphNodesFactory = new RunnableNodeFactory(experimentWorkspaceWrapper);
            graph = GraphAdapter.Adapt(this, templateGraphNodesFactory, library, experimentWorkspaceWrapper.TypeDirectories);

            //clear Workspace
            experimentWorkspaceWrapper.DeleteExperimentUnits();

            //if baseline has been provided write it into the workspace before returning the dispatcher
            if (baseline != null)
            {
                experimentWorkspaceWrapper.Store("BASELINE", baseline);
            }

            IExperimentRunner dispatcher = ExperimentRunnerFactory.CreateExperimentRunner(graph);
            dispatcher.NodeExecuting += dispatcher_NodeExecuting;
            dispatcher.NodeFinished += dispatcher_NodeFinished;
            dispatcher.NodeHasError += dispatcher_NodeHasError;
            dispatcher.ExperimentFinished += dispatcher_ExperimentFinished;
            dispatcher.ExperimentStarted += dispatcher_ExperimentStarted;

            m_dispatcher = dispatcher;

            return dispatcher;
        }

        private IExperimentRunner m_dispatcher;
        public void StopRunningExperiment()
        {
            if (m_dispatcher != null)
            {
                m_dispatcher.TerminateExperimentExecution();
            }
        }

        /// <summary>
        /// Runs the experiement.
        /// </summary>
        /// <param name="progress">The progress.</param>
        public void RunExperiment(IProgress progress, Workspace workspace, ComponentsLibrary library)
        {
            RunExperiment(progress, workspace, library, null);
        }

        /// <summary>
        /// Runs the experiement.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <param name="experiment">The experiment.</param>
        /// <param name="baseline">(optional) The baseline data that is going to be preloaded into workspace before executing the experiment.</param>
        public void RunExperiment(IProgress progress, Workspace workspace, ComponentsLibrary library, TraceLabSDK.Types.Contests.TLExperimentResults baseline)
        {
            progress.CurrentStatus = "Preparing experiment...<br/>";
            progress.IsIndeterminate = true;
            progress.SetError(false);
            ClearErrors();

            Action method = () =>
            {
                //prevent the component library from rescanning while running the experiment 
                using (var rescanLibraryGuard = new RescanLibraryGuard(library))
                {
                    using (var dispatcher = CreateExperimentRunner(workspace, library, baseline))
                    {
                        dispatcher.ExecuteExperiment(progress);
                    }
                }
            };

            Thread dispatchThread = ThreadFactory.CreateThread(new System.Threading.ThreadStart(method));
            dispatchThread.IsBackground = true;
            dispatchThread.Name = "ExperimentRunner";
            dispatchThread.SetApartmentState(System.Threading.ApartmentState.STA);
            dispatchThread.Start();
        }

        #region Event handlers

        void dispatcher_ExperimentStarted(object sender, EventArgs e)
        {
            IsExperimentRunning = true;
            OnExperimentStarted((IExperimentRunner)sender);
        }

        void dispatcher_ExperimentFinished(object sender, EventArgs e)
        {
            IExperimentRunner dispatcher = sender as IExperimentRunner;
            if (dispatcher != null)
            {
                dispatcher.NodeExecuting -= dispatcher_NodeExecuting;
                dispatcher.NodeFinished -= dispatcher_NodeFinished;
                dispatcher.ExperimentFinished -= dispatcher_ExperimentFinished;
            }

            //Reset terminate event at this moment, so that all running experiment runners (for all composite components sub level experiments can complete too)
            dispatcher.ResetExperimentExecutionTerminateEvent();

            IsExperimentRunning = false;
            OnExperimentCompleted(dispatcher);
        }

        public event EventHandler<ExperimentEventArgs> ExperimentCompleted;
        private void OnExperimentCompleted(IExperimentRunner dispatcher)
        {
            if (ExperimentCompleted != null)
            {
                ExperimentCompleted(this, new ExperimentEventArgs(dispatcher));
            }
        }

        public event EventHandler<ExperimentEventArgs> ExperimentStarted;
        private void OnExperimentStarted(IExperimentRunner dispatcher)
        {
            if (ExperimentStarted != null)
            {
                ExperimentStarted(this, new ExperimentEventArgs(dispatcher));
            }
        }

        #endregion

        #endregion
    }
}
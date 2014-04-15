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
using System.ComponentModel;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Commands;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using TraceLab.Core.Components;
using TraceLab.UI.WPF.ViewModels.Nodes;
using System.Windows;

namespace TraceLab.UI.WPF.ViewModels
{
    public abstract class BaseLevelExperimentViewModel : IExperiment, IGraphViewModel, IZoomableViewModel, INotifyPropertyChanged
    {
        #region Constructor

        public BaseLevelExperimentViewModel(IExperiment experiment, BaseLevelExperimentViewModel owner)
        {
            if (experiment == null)
                throw new ArgumentNullException("experiment", "Wrapped experiment cannot be null");

            m_experiment = experiment;
            m_experiment.PropertyChanged += m_experiment_PropertyChanged;
            Owner = owner;

            ToggleInfoPaneForNode = new DelegateCommand(ToggleInfoPaneForNodeFunc);
        }

        void m_experiment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion

        private IExperiment m_experiment;

        public IExperiment GetExperiment()
        {
            return m_experiment;
        }

        public BaseLevelExperimentViewModel Owner
        {
            get;
            private set;
        }

        #region Wrapped Properties

        public ExperimentStartNode StartNode
        {
            get
            {
                return m_experiment.StartNode;
            }
        }

        public ExperimentEndNode EndNode
        {
            get
            {
                return m_experiment.EndNode;
            }
        }

        private readonly ObservableCollection<ExperimentNodeInfo> m_nodeInfo = new ObservableCollection<ExperimentNodeInfo>();
        public ObservableCollection<ExperimentNodeInfo> NodeInfo
        {
            get
            {
                return m_nodeInfo;
            }
        }

        public ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> References
        {
            get { return m_experiment.References; }
        }

        [XmlElement]
        public ExperimentInfo ExperimentInfo
        {
            get
            {
                return m_experiment.ExperimentInfo;
            }
            set
            {
                m_experiment.ExperimentInfo = value;
            }
        }

        [XmlIgnore]
        public bool IsExperimentRunning
        {
            get
            {
                return m_experiment.IsExperimentRunning;
            }
        }

        public void StopRunningExperiment()
        {
            m_experiment.StopRunningExperiment();
        }

        public void RunExperiment(TraceLabSDK.IProgress progress, TraceLab.Core.Workspaces.Workspace workspace, ComponentsLibrary library)
        {
            m_experiment.RunExperiment(progress, workspace, library);
        }

        /// <summary>
        /// Gets the title for the document tab.
        /// </summary>
        public string Title
        {
            get { return m_experiment.Title; }
        }

        private double m_translateX;
        public double TranslateX
        {
            get { return m_translateX; }
            set 
            {
                m_translateX = value;
                OnPropertyChanged("TranslateX");
            }
        }

        private double m_translateY;
        public double TranslateY
        {
            get { return m_translateY; }
            set 
            {
                m_translateY = value;
                OnPropertyChanged("TranslateY");
            }
        }

        private double m_zoom = 1.0;
        public double Zoom
        {
            get { return m_zoom; }
            set 
            {
                m_zoom = value; 
                OnPropertyChanged("Zoom"); 
            }
        }

        #endregion

        #region IExperiment interface wrapper

        public event QuickGraph.EdgeAction<ExperimentNode, ExperimentNodeConnection> EdgeAdded;

        public event QuickGraph.EdgeAction<ExperimentNode, ExperimentNodeConnection> EdgeRemoved;

        public event QuickGraph.VertexAction<ExperimentNode> NodeAdded;

        public event QuickGraph.VertexAction<ExperimentNode> NodeRemoved;

        public IEnumerable<ExperimentNodeConnection> OutEdges(ExperimentNode v)
        {
            return m_experiment.OutEdges(v);
        }

        public IEnumerable<ExperimentNodeConnection> InEdges(ExperimentNode v)
        {
            return m_experiment.InEdges(v);
        }

        public IEnumerable<ExperimentNode> Vertices
        {
            get { return m_experiment.Vertices; }
        }

        public bool ContainsEdge(ExperimentNode source, ExperimentNode target)
        {
            return m_experiment.ContainsEdge(source, target);
        }

        public bool TryGetEdge(ExperimentNode source, ExperimentNode target, out ExperimentNodeConnection edge)
        {
            return m_experiment.TryGetEdge(source, target, out edge);
        }

        public bool TryGetEdges(ExperimentNode source, ExperimentNode target, out IEnumerable<ExperimentNodeConnection> edges)
        {
            return m_experiment.TryGetEdges(source, target, out edges);
        }

        public bool IsOutEdgesEmpty(ExperimentNode v)
        {
            return m_experiment.IsOutEdgesEmpty(v);
        }

        public int OutDegree(ExperimentNode v)
        {
            return m_experiment.OutDegree(v);
        }

        public ExperimentNodeConnection OutEdge(ExperimentNode v, int index)
        {
            return m_experiment.OutEdge(v, index);
        }

        public bool TryGetOutEdges(ExperimentNode v, out IEnumerable<ExperimentNodeConnection> edges)
        {
            return m_experiment.TryGetOutEdges(v, out edges);
        }

        public bool AllowParallelEdges
        {
            get { return m_experiment.AllowParallelEdges; }
        }

        public bool IsDirected
        {
            get { return m_experiment.IsDirected; }
        }

        public bool ContainsVertex(ExperimentNode node)
        {
            return m_experiment.ContainsVertex(node);
        }

        public bool IsVerticesEmpty
        {
            get { return m_experiment.IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return m_experiment.VertexCount; }
        }

        public bool ContainsEdge(ExperimentNodeConnection edge)
        {
            return m_experiment.ContainsEdge(edge);
        }

        public int EdgeCount
        {
            get { return m_experiment.EdgeCount; }
        }

        public IEnumerable<ExperimentNodeConnection> Edges
        {
            get { return m_experiment.Edges; }
        }

        public bool IsEdgesEmpty
        {
            get { return m_experiment.IsEdgesEmpty; }
        }

        public TraceLab.Core.Utilities.ObservableDictionary<ExperimentNode, ExperimentNodeError> Errors
        {
            get { return m_experiment.Errors; }
        }

        public void ClearErrors()
        {
            m_experiment.ClearErrors();
        }

        public ExperimentNode GetNode(string id)
        {
            return m_experiment.GetNode(id);
        }

        #endregion

        #region Equals Override

        public override bool Equals(object obj)
        {
            BaseLevelExperimentViewModel experimentWrapper = obj as BaseLevelExperimentViewModel;
            if (experimentWrapper != null)
            {
                return m_experiment.Equals(experimentWrapper.GetExperiment());
            }

            IExperiment experiment = obj as IExperiment; //at this moment it is not known it is not TopLevelExperimentViewModel, so it is actual m_experiment
            if (experiment != null)
            {
                return m_experiment.Equals(experiment);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int experimentHash = m_experiment.GetHashCode();

            return experimentHash;
        }

        /// <summary>
        /// Checks if wrapped m_experiment equals with specified m_experiment
        /// </summary>
        /// <param name="m_experiment"></param>
        /// <returns></returns>
        public bool Equals(IExperiment experiment)
        {
            return (m_experiment == experiment);
        }

        public bool Equals(BaseLevelExperimentViewModel otherWrapper)
        {
            return (this == otherWrapper);
        }

        #endregion

        #region Info Nodes

        private Dictionary<ExperimentNode, ExperimentNodeInfo> m_inactiveInfo = new Dictionary<ExperimentNode, ExperimentNodeInfo>();
        private Dictionary<ExperimentNode, ExperimentNodeInfo> m_activeInfoLookup = new Dictionary<ExperimentNode, ExperimentNodeInfo>();

        public ICommand ToggleInfoPaneForNode
        {
            get;
            private set;
        }

        private void ToggleInfoPaneForNodeFunc(object param)
        {
            var vertexControl = param as GraphSharp.Controls.VertexControl;

            if (vertexControl != null)
            {
                ExperimentNode node = vertexControl.Vertex as ExperimentNode;

                if (node != null)
                {
                    // First check if this is currectly visibleList
                    ExperimentNodeInfo info;
                    bool showInfo = false;
                    if (!m_activeInfoLookup.TryGetValue(node, out info))
                    {
                        // If it's not currently active, then check if we've shown it before.
                        if (!m_inactiveInfo.TryGetValue(node, out info))
                        {
                            // If we haven't shown this before, then create the info object.
                            info = ExperimentNodeInfo.CreateInfo(vertexControl);
                            Point suggestedLocation = FindOptimalLocation(vertexControl);
                            info.X = suggestedLocation.X;
                            info.Y = suggestedLocation.Y;
                        }

                        // It was either inactive or not previously shown - show it now.
                        showInfo = true;
                    }

                    // Show the info
                    if (showInfo)
                    {
                        ShowInfoForNode(node, info);
                    }
                    else
                    {
                        HideInfoForNode(node, info);
                    }
                }
            }
        }

        private static Point FindOptimalLocation(GraphSharp.Controls.VertexControl vertexControl)
        {
            Point suggestedLocation = new Point();

            var node = vertexControl.Vertex as ExperimentNode;
            if (node != null)
            {
                if (vertexControl != null)
                {
                    const int padding = 5;
                    suggestedLocation = new Point(vertexControl.TopLeftX + vertexControl.ActualWidth, vertexControl.TopLeftY + vertexControl.ActualHeight + padding);

                    //special case, if node belongs to the graph in the scope
                    var ownerGraph = node.Owner as CompositeComponentEditableGraph;
                    if (ownerGraph != null)
                    {
                        var scopeNode = ownerGraph.OwnerNode as ScopeNodeBase;
                        if (scopeNode != null)
                        {
                            var data = scopeNode.DataWithSize;
                            double labelBorderHeight = 23; // check scope node control
                            suggestedLocation = new Point(suggestedLocation.X + data.X - (data.Width / 2), suggestedLocation.Y + data.Y - (data.Height / 2) + labelBorderHeight);
                        }
                    }
                }
            }

            return suggestedLocation;
        }

        private void HideInfoForNode(ExperimentNode node, ExperimentNodeInfo info)
        {
            m_activeInfoLookup.Remove(node);
            m_inactiveInfo.Add(node, info);
            NodeInfo.Remove(info);
            node.IsInfoPaneExpanded = false;
        }

        private void ShowInfoForNode(ExperimentNode node, ExperimentNodeInfo info)
        {
            m_inactiveInfo.Remove(node);
            m_activeInfoLookup.Add(node, info);
            NodeInfo.Add(info);
            node.IsInfoPaneExpanded = true;
        }

        protected void RemoveInfoForNode(ExperimentNode vert)
        {
            ExperimentNodeInfo info;
            if (!m_activeInfoLookup.TryGetValue(vert, out info))
            {
                m_inactiveInfo.TryGetValue(vert, out info);
            }

            if (info != null)
            {
                NodeInfo.Remove(info);
            }
            m_activeInfoLookup.Remove(vert);
            m_inactiveInfo.Remove(vert);
        }

        #endregion

        public bool IsModified
        {
            get { return m_experiment.IsModified; }
            set
            {
                m_experiment.IsModified = value;
            }
        }

        public void ResetModifiedFlag()
        {
            m_experiment.ResetModifiedFlag();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}

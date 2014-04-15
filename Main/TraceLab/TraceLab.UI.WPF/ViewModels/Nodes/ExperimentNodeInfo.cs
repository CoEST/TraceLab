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
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.ViewModels.Nodes
{
    /// <summary>
    /// Represent the view model for the node info panel.
    /// Based on the view model type different panels are being rendered. 
    /// Thus DataTemplates should be provided in the resource dictionary for each subtype of ExperimentNodeInfo.
    /// For example, NodeResourcesForExperiment is an example of provided DataTemplate
    /// </summary>
    public class ExperimentNodeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentNodeInfo"/> class.
        /// </summary>
        /// <param name="originVertexControl">The origin vertex control.</param>
        protected ExperimentNodeInfo(GraphSharp.Controls.VertexControl originVertexControl)
        {
            OriginVertexControl = originVertexControl;
            Node = (ExperimentNode)originVertexControl.Vertex;
        }

        private GraphSharp.Controls.VertexControl m_originVertexControl;
        /// <summary>
        /// Gets or sets the origin vertex control.
        /// </summary>
        /// <value>
        /// The origin vertex control.
        /// </value>
        public GraphSharp.Controls.VertexControl OriginVertexControl
        {
            get { return m_originVertexControl; }
            set { m_originVertexControl = value; }
        }


        /// <summary>
        /// Shortcut to OriginVertexControl.Vertex
        /// </summary>
        public ExperimentNode Node
        {
            get;
            private set;
        }

        private double m_x;
        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        private double m_y;
        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// Creates the info panel view model for the given vertex control based on its type
        /// </summary>
        /// <param name="vertexControl">The vertex control for which the info is created.</param>
        /// <returns>node info view model</returns>
        public static ExperimentNodeInfo CreateInfo(GraphSharp.Controls.VertexControl vertexControl)
        {
            var node = vertexControl.Vertex as ExperimentNode;

            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (node.Data == null)
            {
                throw new InvalidOperationException("Cannot create info for node with invalid data.");
            }

            ExperimentNodeInfo info = null;

            //there are different types of node info view models depending on what vertex control type is
            //based on the node info type different view templates are used to render the info panel

            //most common is component, so check it first
            if (node.Data.Metadata is ComponentMetadata)
            {
                info = new ComponentNodeInfo(vertexControl);
            }
            else if (node.Data.Metadata is CompositeComponentMetadata)
            {
                info = new CompositeComponentNodeInfo(vertexControl);
            }
            else if (node.Data.Metadata is DecisionMetadata)
            {
                info = new DecisionNodeInfo(vertexControl);
            }
            else if (node.Data.Metadata is LoopScopeMetadata)
            {
                info = new LoopDecisionNodeInfo(vertexControl);
            }

            return info;
        }
    }
}

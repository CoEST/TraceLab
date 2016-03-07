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

using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    public interface IEditableExperiment : IExperiment
    {
        /// <summary>
        /// Adds a new component at the specified coordinates
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the component definition is null</exception>
        ExperimentNode AddComponentFromDefinition(MetadataDefinition metadataDefinition, double positionX, double positionY);

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
        ExperimentNodeConnection AddConnection(ExperimentNode fromNode, ExperimentNode toNode);

        /// <summary>
        /// Adds the fixed connection.
        /// </summary>
        /// <param name="fromNode">From node.</param>
        /// <param name="toNode">To node.</param>
        /// <param name="isVisible">if set to <c>true</c> edge is visible.</param>
        /// <returns>the added connection</returns>
        ExperimentNodeConnection AddFixedConnection(ExperimentNode fromNode, ExperimentNode toNode, bool isVisible);

        /// <summary>
        /// Removes the given connection from the m_experiment.
        /// </summary>
        /// <param name="edge">The edge to remove</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the ViewModel is in a bad state that it cannot recover from</exception>
        void RemoveConnection(ExperimentNodeConnection edge);

        /// <summary>
        /// Adds the node vertex to the experiment
        /// </summary>
        /// <param name="node">The node to be added</param>
        /// <returns>true if node was added</returns>
        bool AddVertex(ExperimentNode node);

        /// <summary>
        /// Removes the node.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>true if node was removed</returns>
        bool RemoveVertex(ExperimentNode node);

        /// <summary>
        /// Removes the selected vertices.
        /// </summary>
        void RemoveSelectedVertices();

        /// <summary>
        /// Toogles the logging on selected nodes of the given level. 
        /// </summary>
        /// <param name="enable">if set to <c>true</c> it enables the logging for the given level on all selected nodes.</param>
        /// <param name="level">The level.</param>
        void ToogleLogLevelOnSelectedNodes(bool enable, NLog.LogLevel level);

        /// <summary>
        /// Sets the log level current experiment settings on the node.
        /// </summary>
        /// <param name="node">The node which log level setting are set.</param>
        void SetLogLevelSettings(ExperimentNode node);

        /// <summary>
        /// Gets the settings.
        /// </summary>
        Settings.Settings Settings
        {
            get;
        }
    }
}
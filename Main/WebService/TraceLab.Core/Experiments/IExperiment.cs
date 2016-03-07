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
using System.Collections.ObjectModel;
using System.ComponentModel;
using QuickGraph;
using TraceLab.Core.Components;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Experiments
{
    public interface IExperiment : IVertexAndEdgeListGraph<ExperimentNode, ExperimentNodeConnection>, IModifiable, INotifyPropertyChanged
    {
        ExperimentStartNode StartNode
        {
            get;
        }

        ExperimentEndNode EndNode
        {
            get;
        }

        ExperimentInfo ExperimentInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the title for the document tab.
        /// </summary>
        string Title
        {
            get;
        }

        ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> References
        {
            get;
        }

        TraceLab.Core.Utilities.ObservableDictionary<ExperimentNode, ExperimentNodeError> Errors
        {
            get;
        }

        IEnumerable<ExperimentNodeConnection> InEdges(ExperimentNode v);

        event EdgeAction<ExperimentNode, ExperimentNodeConnection> EdgeAdded;
        event EdgeAction<ExperimentNode, ExperimentNodeConnection> EdgeRemoved;
        event VertexAction<ExperimentNode> NodeAdded;
        event VertexAction<ExperimentNode> NodeRemoved;

        /// <summary>
        /// Gets the node of the specific node id. If Node was not found it returns null.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Node with given id. Null if not found. </returns>
        ExperimentNode GetNode(string id);

        /// <summary>
        /// Clears all the errors in all vertices, including errors in all subexperiments and their vertices.
        /// </summary>
        void ClearErrors();
        
        bool IsExperimentRunning { get; }

        void StopRunningExperiment();

        void RunExperiment(TraceLabSDK.IProgress progress, TraceLab.Core.Workspaces.Workspace workspace, ComponentsLibrary library);
    }
}

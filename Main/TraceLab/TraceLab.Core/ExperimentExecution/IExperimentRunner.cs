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
using TraceLabSDK;

namespace TraceLab.Core.ExperimentExecution
{
    public class NodeEventArgs : EventArgs
    {
        public NodeEventArgs(string nodeId) : this(nodeId, null)
        {
        }

        public NodeEventArgs(string nodeId, string errorText)
        {
            NodeId = nodeId;
            ErrorText = errorText;
        }

        public string NodeId
        {
            get;
            private set;
        }

        public string ErrorText
        {
            get;
            private set;
        }
    }

    delegate void NodeEventHandler(string nodeId);

    public interface IExperimentRunner : IDisposable
    {
        void ExecuteExperiment(IProgress progress);

        /// <summary>
        /// Terminates the experiment execution by setting terminate event.
        /// It allows stopping the experiment.
        /// </summary>
        void TerminateExperimentExecution();

        /// <summary>
        /// Resets the experiment execution terminate event.
        /// </summary>
        void ResetExperimentExecutionTerminateEvent();

        /// <summary>
        /// Called when a Node begins it's <c>Run</c>.
        /// </summary>
        event EventHandler<NodeEventArgs> NodeExecuting;

        /// <summary>
        /// Will be called upon the successful completion of a Node's <c>Run</c>.
        /// </summary>
        event EventHandler<NodeEventArgs> NodeFinished;

        /// <summary>
        /// If a node encounters an error while running, this will be called INSTEAD of <c>NodeFinished</c>.
        /// </summary>
        event EventHandler<NodeEventArgs> NodeHasError;

        event EventHandler ExperimentStarted;
        event EventHandler ExperimentFinished;

        void OnNodeExecuting(IRunnableNode node);
        void OnNodeFinished(IRunnableNode node);
        void OnNodeHasError(IRunnableNode node, string errorMessage);
    }
}

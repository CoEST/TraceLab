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
using System.Threading;
namespace TraceLab.Core.ExperimentExecution
{
    public interface IRunnableNode
    {
        /// <summary>
        /// Adds the node to the collection of successor nodes.
        /// </summary>
        /// <param name="nextNode">The next node.</param>
        void AddNextNode(IRunnableNode nextNode);

        /// <summary>
        /// Adds the node to the collection of predecessor nodes.
        /// </summary>
        /// <param name="previousNode">The previous node.</param>
        void AddPreviousNode(IRunnableNode previousNode);
        
        /// <summary>
        /// Gets the error message.
        /// </summary>
        string ErrorMessage { get; }
        
        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        bool HasError { get; }

        /// <summary>
        /// Gets the node id.
        /// </summary>
        string Id { get; }
        object InitializeLifetimeService();

        /// <summary>
        /// Blocks the calling thread until a node thread terminates
        /// </summary>
        void JoinNodeThread();

        /// <summary>
        /// Gets the label.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the collection of successor nodes.
        /// </summary>
        RunnableNodeCollection NextNodes { get; }

        /// <summary>
        /// Gets the collection of predecessor nodes.
        /// </summary>
        RunnableNodeCollection PreviousNodes { get; }

        /// <summary>
        /// Runs creates the thread with specified specified arguments and starts this thread.
        /// </summary>
        /// <param name="args">The args.</param>
        AutoResetEvent Run(RunnableNodeThreadArgs args);

        /// <summary>
        /// The actual run execution of each node, that is executed in seperate thread
        /// </summary>
        void RunInternal();

        /// <summary>
        /// Gets a value indicating whether the node should wait for all predecessors to be completed.
        /// If false, node will start executing as soon as first predeccesor will complete its task.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [wait for all predecessors]; otherwise, <c>false</c>.
        /// </value>
        bool WaitsForAllPredecessors { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();
        bool Equals(object obj);
        int GetHashCode();
    }
}

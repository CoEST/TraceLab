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
using System.Threading;
using TraceLabSDK;
using System.Runtime.Serialization;
using TraceLab.Core.Exceptions;
using System.Collections.ObjectModel;
using TraceLab.Core.Utilities;
using System.Collections;

namespace TraceLab.Core.ExperimentExecution
{
    class ExperimentRunner : IExperimentRunner
    {
        private RunnableExperimentBase m_runnableExperiment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentRunner"/> class for the runnable experiment.
        /// </summary>
        /// <param name="runnableExperiment">The runnable experiment.</param>
        public ExperimentRunner(RunnableExperimentBase runnableExperiment) : this(runnableExperiment, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentRunner"/> class.
        /// </summary>
        /// <param name="runnableExperiment">The runnable experiment.</param>
        /// <param name="destroyComponentsAppDomain">if set to <c>true</c> the experiment runner will destroy components app domain of this runnable experiment, once experiment is finished.
        /// In case of sub level experiments, the domain should not yet be destroyed. (there is one AppDomain for entire experiment, including
        /// subexperiments). The app domain should be destroyed only when top level experiment has been finished.</param>
        /// <param name="terminateExperimentResetEvent">The terminate experiment reset event of the top level experiment runner
        /// It allows stopping the experiment. 
        /// Sublevel experiments need to also stop processing their nodes.
        /// </param>
        public ExperimentRunner(RunnableExperimentBase runnableExperiment, bool destroyComponentsAppDomain)
        {
            if (runnableExperiment == null)
                throw new ArgumentNullException("runnableExperiment");

            m_runnableExperiment = runnableExperiment;
            m_disposeRunnableExperiment = destroyComponentsAppDomain;
        }

        ~ExperimentRunner()
        {
        }

        /// <summary>
        /// Determines if components app domain of the runnable experiment is supposed to be destroyed after executing the experiment.
        /// In case of sub level experiments, the domain should not yet be destroyed. (there is one AppDomain for entire experiment, including
        /// subexperiments). The app domain should be destroyed only when top level experiment has been finished. 
        /// </summary>
        private bool m_disposeRunnableExperiment;

        /// <summary>
        /// Terminates the experiment execution
        /// </summary>
        public void TerminateExperimentExecution()
        {
            m_runnableExperiment.TerminateExperimentExecutionResetEvent.Set();
        }

        /// <summary>
        /// Resets the experiment execution terminate event.
        /// </summary>
        public void ResetExperimentExecutionTerminateEvent()
        {
            m_runnableExperiment.TerminateExperimentExecutionResetEvent.Reset();
        }

        /// <summary>
        /// Executes the experiment.
        /// </summary>
        /// <param name="progress">The progress.</param>
        public void ExecuteExperiment(IProgress progress)
        {
            try
            {
                bool successful = true;
                string endMessage = Messages.ExperimentRunnerSuccessMessage;

                OnExperimentStarted();

                if (m_runnableExperiment.IsEmpty == false)
                {
                    if (progress != null)
                    {
                        progress.Reset();
                        progress.NumSteps = m_runnableExperiment.Nodes.Count;
                        // Start at 1 so user can tell something is happening.
                        progress.Increment();
                        progress.CurrentStatus = Messages.ProgressExperimentProcessing + "<br/>";
                    }

                    // collection of nodes currently executing
                    ActiveNodesList currentActiveNodesList = new ActiveNodesList(m_runnableExperiment.TerminateExperimentExecutionResetEvent);

                    // collection of nodes pending to be started
                    Queue<RunnableNode> pendingNodesToBeRun = new Queue<RunnableNode>();
                    
                    // enqueue start node
                    pendingNodesToBeRun.Enqueue(m_runnableExperiment.StartNode);

                    bool end = false;
                    while (!end) //until end component is not completed
                    {
                        //activate all pending nodes
                        while (pendingNodesToBeRun.Count > 0)
                        {
                            RunnableNode node = pendingNodesToBeRun.Dequeue();

                            if (currentActiveNodesList.Contains(node) == false)
                            {
                                RunnableNodeThreadArgs args = new RunnableNodeThreadArgs { ExperimentRunner = this };
                                var resetEvent = node.Run(args, progress); // brian added progress parameter

                                currentActiveNodesList.Add(node, resetEvent);
                            }
                        }

                        if (currentActiveNodesList.Count > 0)
                        {
                            //wait for any node to be completed
                            int index = WaitHandle.WaitAny(currentActiveNodesList.NodeResetEvents);

                            //the index 0 is the termination signal... if the index is higher than zero then process the completed node
                            if (index > 0)
                            {
                                RunnableNode completedNode = currentActiveNodesList.TakeOutNode(index);

                                if (progress != null)
                                {
                                    progress.Increment();
                                }

                                // if experiment runner reaches end node, then prepare to exit the experiment runner
                                if (completedNode.Equals(m_runnableExperiment.EndNode))
                                {
                                    end = true;

                                    // if there are any other nodes still running other than end node
                                    if (currentActiveNodesList.Count > 1)
                                    {
                                        successful = false;
                                        endMessage = Messages.ExperimentRunnerErrorMessage;
                                        TerminateExperimentExecution(); //send signal terminate in case sth else in sub level experiment is running
                                        NLog.LogManager.GetCurrentClassLogger().Error(Messages.ExperimentRunnerEarlyTerminationErrorMessage);
                                    }
                                }
                                else
                                {
                                    // send one token to all successor nodes
                                    foreach (RunnableNode successorNode in completedNode.NextNodes)
                                    {
                                        //sends token; method will return true if successor node is ready to be run
                                        bool readyToRun = successorNode.SendToken();

                                        if (readyToRun)
                                        {
                                            //add node to the pending nodes to be activated
                                            pendingNodesToBeRun.Enqueue(successorNode);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //EXECUTED ON SIGNAL TERMINATED
                                // If any node signal TERMINATE for any reason, we shut down the experiment and stop processing new nodes.
                                end = true;
                                successful = false;
                                endMessage = Messages.ExperimentExecutionTerminated;
                                System.Diagnostics.Trace.WriteLine("TERMINATED!");
                            }
                        }
                        else
                        {
                            end = true;
                            successful = false;
                            endMessage = Messages.ExperimentRunnerInfiniteWaitDetected;
                            TerminateExperimentExecution(); //send signal terminate in case sth else in sub level experiment is running
                            System.Diagnostics.Trace.WriteLine("Infinite wait detected!");
                        }

                    } // while(!end)

                    //wait if there are any still running threads, only in case if experiment runner ended on termination
                    foreach (RunnableNode n in currentActiveNodesList.Nodes)
                    {
                        //Blocks this thread until all node threads terminate.
                        n.JoinNodeThread();
                    }
                }
                else // if (m_runnableExperiment.IsEmpty == false)
                {
#if DEBUG
                    NLog.LogManager.GetCurrentClassLogger().Debug("RunnableExperiment is empty");
#endif
                    successful = false;
                    endMessage = Messages.ExperimentRunnerErrorMessage;
                }

                if (progress != null)
                {
                    progress.Reset();
                    if (!successful)
                    {
                        progress.SetError(true);
                    }

                    progress.CurrentStatus = endMessage + "<br/>";
                }
            }
            finally
            {
                if (m_disposeRunnableExperiment == true)
                {
                    // destroy components app domain... note the app domain should not be destroyed by sub level experiments
                    TraceLab.Core.Components.LibraryHelper.DestroyDomain(m_runnableExperiment.ComponentsAppDomain);

                    //dispose entire experiment - it disposes all subexperiments as well.
                    m_runnableExperiment.Dispose();
                }

                OnExperimentFinished();
            }
        }

        #region Active Nodes Collection

        /// <summary>
        /// The helper collection that keeps the list of nodes that has been activated.
        /// </summary>
        [Serializable]
        private class ActiveNodesList
        {
            private List<RunnableNode> m_activeNodesList = new List<RunnableNode>();
            private List<EventWaitHandle> m_nodeResetEvents = new List<EventWaitHandle>();

            /// <summary>
            /// Initializes a new instance of the <see cref="ActiveNodesList"/> class.
            /// </summary>
            /// <param name="terminateExperimentExecutionResetEvent">The terminate experiment execution reset event, that should be at the beggining of the reset events</param>
            internal ActiveNodesList(EventWaitHandle terminateExperimentExecutionResetEvent) 
            {
                m_nodeResetEvents.Add(terminateExperimentExecutionResetEvent);
            }

            public IEnumerable<RunnableNode> Nodes
            {
                get { return m_activeNodesList; }
            }

            /// <summary>
            /// Adds the specified node with its corresponding auto reset event.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            public void Add(RunnableNode node, EventWaitHandle nodeResetEvent)
            {
                m_activeNodesList.Add(node);
                m_nodeResetEvents.Add(nodeResetEvent);
            }

            public bool Contains(RunnableNode node)
            {
                return m_activeNodesList.Contains(node);
            }

            public WaitHandle[] NodeResetEvents 
            {
                get
                {
                    return m_nodeResetEvents.ToArray();
                }
            }

            public int Count
            {
                get { return m_activeNodesList.Count; }
            }

            /// <summary>
            /// Takes the out node from the collection from specified index.
            /// In other words, it removes the node from the collection and returns it.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns></returns>
            public RunnableNode TakeOutNode(int index)
            {
                //the index of active nodes list is shifted by one, because node reset events have terminate events at the beginning
                RunnableNode node = m_activeNodesList[index-1];
                //remove node from both lists
                m_activeNodesList.RemoveAt(index-1); 
                m_nodeResetEvents.RemoveAt(index);
                
                //return the node
                return node;
            }
        }

        #endregion

        #region Events

        public event EventHandler<NodeEventArgs> NodeExecuting;
        public event EventHandler<NodeEventArgs> NodeFinished;
        public event EventHandler<NodeEventArgs> NodeHasError;
        public event EventHandler ExperimentStarted;
        public event EventHandler ExperimentFinished;

        public void OnNodeExecuting(IRunnableNode node)
        {
            if (NodeExecuting != null)
            {
                NodeExecuting(this, new NodeEventArgs(node.Id));
            }
        }

        /// <summary>
        /// Fire event if the given node has finished.
        /// </summary>
        /// <param name="node">The node.</param>
        public void OnNodeFinished(IRunnableNode node)
        {
            if (NodeFinished != null)
            {
                NodeFinished(this, new NodeEventArgs(node.Id));
            }
        }

        public void OnNodeHasError(IRunnableNode node, string errorMessage)
        {
            if (NodeHasError != null)
            {
                NodeHasError(this, new NodeEventArgs(node.Id, errorMessage));
            }
        }

        private void OnExperimentStarted()
        {
            if (ExperimentStarted != null)
            {
                ExperimentStarted(this, new EventArgs());
            }
        }

        private void OnExperimentFinished()
        {
            if (ExperimentFinished != null)
            {
                ExperimentFinished(this, new EventArgs());
            }
        }
        #endregion

        #region Dispose

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

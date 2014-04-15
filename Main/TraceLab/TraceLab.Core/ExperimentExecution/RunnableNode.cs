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
using System.Security.Permissions;
using System.Threading;
using System.Collections.Generic;
using TraceLab.Core.Utilities;
using System.Diagnostics;
using TraceLabSDK;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents base class for all template graph nodes. 
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Label = {Label}, PrevNodeCount = {PreviousNodes.Count}")]
    internal abstract class RunnableNode : MarshalByRefObject, IRunnableNode
    {
        #region Constructors

        protected RunnableNode()
        {
        }

        protected RunnableNode(String id, String label, TraceLab.Core.Components.ComponentsLibrary library, bool waitForAllPredecessors)
            : this(id, label, new RunnableNodeCollection(), new RunnableNodeCollection(), library, waitForAllPredecessors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunnableNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="label">The label.</param>
        /// <param name="nextNodes">The next nodes.</param>
        /// <param name="previousNodes">The previous nodes.</param>
        /// <param name="library">The library.</param>
        /// <param name="waitForAllPredecessor">if set to <c>true</c> the node will wait for all predecessor nodes to be completed.</param>
        protected RunnableNode(String id, String label, RunnableNodeCollection nextNodes, RunnableNodeCollection previousNodes, TraceLab.Core.Components.ComponentsLibrary library, bool waitForAllPredecessors)
        {
            Id = id;
            Label = label;
            NextNodes = nextNodes;
            PreviousNodes = previousNodes;
            Library = library;
            WaitsForAllPredecessors = waitForAllPredecessors;
        }

        #endregion

        /// <summary>
        /// The actual run execution of each node, that is executed in seperate thread
        /// </summary>
        abstract public void RunInternal();

        #region RunnableNode Members

        private RunnableNodeCollection m_nextNodes = null;
        /// <summary>
        /// Gets the collection of successor nodes.
        /// </summary>
        public RunnableNodeCollection NextNodes
        {
            get
            {
                return m_nextNodes;
            }
            private set
            {
                m_nextNodes = value;
            }
        }

        private RunnableNodeCollection m_previousNodes = null;
        /// <summary>
        /// Gets the collection of predecessor nodes.
        /// </summary>
        public RunnableNodeCollection PreviousNodes
        {
            get
            {
                return m_previousNodes;
            }
            private set
            {
                m_previousNodes = value;
            }
        }

        private string m_id;
        /// <summary>
        /// Gets the node id.
        /// </summary>
        public string Id
        {
            get
            {
                return m_id;
            }
            private set
            {
                m_id = value;
            }
        }

        private string m_label;
        /// <summary>
        /// Gets the label.
        /// </summary>
        public string Label
        {
            get
            {
                return m_label;
            }
            private set
            {
                m_label = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get;
            protected set;
        }

        private Thread m_nodeThread;
        private AutoResetEvent m_nodeResetEvent = new AutoResetEvent(false);
        
        /// <summary>
        /// Signals that the node completed its task,
        /// by setting the state of the event to signaled, allowing the experiment runner to proceed.
        /// </summary>
        internal void SignalCompletion()
        {
            m_nodeResetEvent.Set();
        }

        /// <summary>
        /// Runs creates the thread with specified specified arguments and starts this thread.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>The the AutoResetEvent that will signal, when the thread finishes.</returns>
        public AutoResetEvent Run(RunnableNodeThreadArgs args)
        {
            if (args == null || args.ExperimentRunner == null)
                throw new ArgumentNullException("args", "RunnableNodeThreadArgs and its dispatcher cannot be null to start a thread");

            args.Node = this;

            // Sets the state of the reset event to nonsignaled
            m_nodeResetEvent.Reset();

            // Create a task and supply a user delegate by using a lambda expression.
            var nodeThread = ThreadFactory.CreateThread(new ParameterizedThreadStart(ThreadRun));
            nodeThread.Name = String.Format("Worker: {0}, {1}", Id, Label);
            nodeThread.IsBackground = true;

            nodeThread.SetApartmentState(ApartmentState.STA);
            nodeThread.Start(args);

            m_nodeThread = nodeThread;

            return m_nodeResetEvent;
        }

        /// <summary>
        /// Blocks the calling thread until a node thread terminates
        /// </summary>
        public void JoinNodeThread()
        {
            if (m_nodeThread != null)
            {
                m_nodeThread.Join();
                m_nodeThread = null;
            }
        }

        /// <summary>
        /// Adds the node to the collection of successor nodes.
        /// </summary>
        /// <param name="nextNode">The next node.</param>
        public virtual void AddNextNode(IRunnableNode nextNode)
        {
            NextNodes.Add(nextNode);
        }

        /// <summary>
        /// Adds the node to the collection of predecessor nodes.
        /// </summary>
        /// <param name="previousNode">The previous node.</param>
        public void AddPreviousNode(IRunnableNode previousNode)
        {
            PreviousNodes.Add(previousNode);
        }

        private bool m_waitForAllPredecessors;

        /// <summary>
        /// Gets a value indicating whether the node should wait for all predecessors to be completed.
        /// If false, node will start executing as soon as first predeccesor will complete its task.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [wait for all predecessors]; otherwise, <c>false</c>.
        /// </value>
        public bool WaitsForAllPredecessors
        {
            get
            {
                return m_waitForAllPredecessors;
            }
            private set
            {
                m_waitForAllPredecessors = value;
            }
        }
        
        public static IEnumerable<IRunnableNode> GetPreviousNodes(IRunnableNode node)
        {
            return node.PreviousNodes;
        }

        #endregion
        
        #region Dispose

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="RunnableNode"/> is reclaimed by garbage collection.
        /// </summary>
        ~RunnableNode()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                JoinNodeThread();
            }
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            RunnableNode other = obj as RunnableNode;
            if (other != null)
            {
                isEqual = object.Equals(Id, other.Id);
            }
            else
            {
                isEqual = base.Equals(obj);
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected TraceLab.Core.Components.ComponentsLibrary Library
        {
            get;
            private set;
        }

        #region Thread Run

        private static void ThreadRun(object obj)
        {
            RunnableNodeThreadArgs args = (RunnableNodeThreadArgs)obj;
            RunnableNode activeNode = (RunnableNode)args.Node;
            //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            // Notify that this node is starting work
            args.ExperimentRunner.OnNodeExecuting(activeNode);

            Exception error = null;
            try
            {
                activeNode.RunInternal();
            }
            catch (Exception e)
            {
                error = e;
            }

            if (error != null)
            {
                activeNode.HasError = true;
                activeNode.ErrorMessage = error.Message;

                RunnableComponentNode componentNode = activeNode as RunnableComponentNode;
                if (componentNode != null)
                {
                    if (error is ComponentException)
                    {
                        componentNode.Logger.Error(error.Message);
                    }
                    else
                    {
                        componentNode.Logger.ErrorException(error.Message, error);
                    }
                }

                args.ExperimentRunner.OnNodeHasError(activeNode, activeNode.ErrorMessage);
                args.ExperimentRunner.TerminateExperimentExecution();
            }
            else
            {
                // Notify this node is done, then start preparing for any remaining nodes.
                args.ExperimentRunner.OnNodeFinished(activeNode);
                
                //SIGNAL COMPLETION, allowing the waiting Experiment Runner to proceed
                activeNode.SignalCompletion();
            }

            //sw.Stop();

            //System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.Name + ": " + sw.ElapsedMilliseconds);
            //System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.Name + ": " + sw.Elapsed.TotalMilliseconds);
            //System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.Name + ": " + (sw.ElapsedTicks));

        }
        
        #endregion

        private int m_numberOfTokens = 0;

        /// <summary>
        /// Sends the token to the runnable node. If node is set to wait for ALL incoming nodes, it will collect the token
        /// till it reaches number of predecessor nodes, then it is ready to be executed and method returns true.
        /// If node is set to wait for ANY the method always returns true.
        /// </summary>
        /// <returns>true if node is ready to be run, otherwise false</returns>
        internal bool SendToken()
        {
            bool readyToRun = false;

            //if node is waiting for all predeccessor to be completed
            if (WaitsForAllPredecessors)
            {
                //collect the token
                m_numberOfTokens++;

                //and if number of tokens is equal to number of previous nodes, the node is ready to be run
                if (m_numberOfTokens % PreviousNodes.Count == 0)
                {
                    //reset number of tokens
                    m_numberOfTokens = 0;
                    readyToRun = true;
                }
            }
            else
            {
                //otherwise, nodes waits for ANY predecessor to be completed... simply let it run
                readyToRun = true;
            }

            return readyToRun;

        }
    }
}

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
using TraceLab.Core.Workspaces;
using TraceLab.Core.Components;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents compositComponentNode in the RunnableExperimentBase
    /// </summary>
    internal class RunnableCompositeComponentNode : RunnableNode
    {
        #region Constructor

        /// <summary>
        /// Initializes a new s_instance of the <see cref="RunnableCompositeComponentNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <param name="templateGraph">The template graph.</param>
        /// <param name="workspaceWrapper">The composite component workspace wrapper.</param>
        public RunnableCompositeComponentNode(String id,
                    CompositeComponentBaseMetadata compositeComponentMetadata, 
                    RunnableExperimentBase templateGraph, 
                    NestedWorkspaceWrapper workspaceWrapper, 
                    TraceLab.Core.Components.ComponentsLibrary library, 
                    bool waitForAllPredecessors)
                    : base(id, compositeComponentMetadata.Label, new RunnableNodeCollection(), new RunnableNodeCollection(), library, waitForAllPredecessors)
        {
            m_compositeComponentMetadata = compositeComponentMetadata;
            m_subLevelExperiment = templateGraph;
            m_workspace = workspaceWrapper;
        }

        #endregion

        #region Private fields

        private RunnableExperimentBase m_subLevelExperiment;

        //simply keep composite component metadata
        private CompositeComponentBaseMetadata m_compositeComponentMetadata;

        protected NestedWorkspaceWrapper m_workspace;

        #endregion

        public override void RunInternal()
        {
            Setup();

            var subGraph = m_compositeComponentMetadata.ComponentGraph;

            Action method = () =>
            {
                using (IExperimentRunner dispatcher = ExperimentRunnerFactory.CreateExperimentRunnerForSubLevelExperiment(m_subLevelExperiment))
                {
                    dispatcher.NodeExecuting += subGraph.dispatcher_NodeExecuting;
                    dispatcher.NodeFinished += subGraph.dispatcher_NodeFinished;
                    dispatcher.NodeHasError += subGraph.dispatcher_NodeHasError;
                    dispatcher.ExperimentFinished += (s, a) =>
                    {
                        dispatcher.NodeExecuting -= subGraph.dispatcher_NodeExecuting;
                        dispatcher.NodeFinished -= subGraph.dispatcher_NodeFinished;
                        dispatcher.NodeHasError -= subGraph.dispatcher_NodeHasError;
                    };

                    dispatcher.ExecuteExperiment(null);
                }
            };

            Thread dispatchThread = ThreadFactory.CreateThread(new System.Threading.ThreadStart(method));
            dispatchThread.IsBackground = true;
            dispatchThread.Name = "SubExperimentRunner";
            dispatchThread.SetApartmentState(System.Threading.ApartmentState.STA);
            dispatchThread.Start();
            dispatchThread.Join();

            TearDown();
        }

        private void Setup()
        {
            m_workspace.Setup();
        }

        private void TearDown()
        {
            m_workspace.TearDown();
        }
    }
}

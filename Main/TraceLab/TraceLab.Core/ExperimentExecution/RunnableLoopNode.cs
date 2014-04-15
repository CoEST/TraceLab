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
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.ExperimentExecution
{
    internal class RunnableLoopNode : RunnableCompositeComponentNode
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RunnableLoopNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="loopDecisionModule">The loop decision module.</param>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <param name="templateGraph">The template graph.</param>
        /// <param name="workspaceWrapper">The composite component workspace wrapper.</param>
        /// <param name="library">The library.</param>
        /// <param name="waitForAllPredecessors">if set to <c>true</c> [wait for all predecessors].</param>
        public RunnableLoopNode(String id,
            TraceLab.Core.Decisions.ILoopDecisionModule loopDecisionModule,
            CompositeComponentBaseMetadata compositeComponentMetadata,
            RunnableExperimentBase templateGraph,
            NestedWorkspaceWrapper workspaceWrapper,
            TraceLab.Core.Components.ComponentsLibrary library,
            bool waitForAllPredecessors)
            : base(id, compositeComponentMetadata, templateGraph, workspaceWrapper, library, waitForAllPredecessors) 
        {
            m_loopDecisionModule = loopDecisionModule;
        }

        #endregion

        /// <summary>
        /// The decision module that is going to be invoked to determine whether the loop should be repeated.
        /// The loop scope is repeated as long as a specified module condition is true.
        /// </summary>
        private TraceLab.Core.Decisions.ILoopDecisionModule m_loopDecisionModule;

        public override void RunInternal()
        {
            /// The loop scope is repeated as long as a specified module condition is true.
            while(m_loopDecisionModule.Condition())
            {
                base.RunInternal();
            }
        }
    }
}

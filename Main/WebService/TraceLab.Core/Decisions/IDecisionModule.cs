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

using TraceLab.Core.ExperimentExecution;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Interface representing the module classes for Decision nodes.
    /// DecisionCompilationRunner compiles classes implementing this interaface for all DecisionMetadata nodes
    /// Once compiled, objects of those classes are being instantiated by the DecisionModuleFactory for all RunnableDecisionNodes
    /// </summary>
    public interface IDecisionModule
    {
        /// <summary>
        /// Decides which nodes are going to be executed next from within the candidate paths
        /// </summary>
        /// <param name="candidatePaths">The candidate paths.</param>
        /// <returns>the collection of nodes that are going to be executed next after the decision</returns>
        RunnableNodeCollection Decide(RunnableNodeCollection candidatePaths);
    }
}

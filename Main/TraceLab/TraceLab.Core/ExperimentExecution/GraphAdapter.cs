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
using System.Globalization;
using TraceLab.Core.Experiments;
using TraceLab.Core.Components;
using System.Reflection;

namespace TraceLab.Core.ExperimentExecution
{
    internal static class GraphAdapter
    {
        /// <summary>
        /// Adapts the specified experiment into runnable experiment.
        /// Adapt method validates given experiment and builds runnable experiment which is going to be executed by the experiment runner.
        /// During adapation the experiment is validated.
        /// In case of detected error in the experiment, this method returns empty RunnableExperimentBase with no nodes and no edges.
        /// There are several errors that can be detected.
        /// There are nodes connected to start, but not to graph end.
        /// Loops without decisions nodes.
        /// Input mappings are incorrect.
        /// Failed component load or instantiation, especially in case of incorrect configuration values.
        /// </summary>
        /// <param name="experiment">The experiment which is going to be adapted into RunnableExperiment..</param>
        /// <param name="nodesFactory">The nodes factory, by which all nodes in runnable experiment are created..</param>
        /// <param name="library">The library of components.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <returns>
        /// Runnable experiment that experiment is going to execute, pruned from nodes that are not connected to main flow beginning at Start node. 
        /// In case of detected error in the experiment, this method returns empty RunnableExperimentBase with no nodes and no edges.
        /// </returns>
        public static RunnableExperimentBase Adapt(IExperiment experiment, IRunnableNodeFactory nodesFactory, Components.ComponentsLibrary library, List<string> workspaceTypeDirectories)
        {
            LoggerNameRoot loggerNameRoot = new LoggerNameRoot(experiment.ExperimentInfo.Id);

            // Create the new domain for the runnable experiment with whatever current security evidence we're running with.
            // The components app domain is the app domain which components assemblies are going to be loaded into.

            var helper = new TraceLab.Core.Components.LibraryHelper(workspaceTypeDirectories);
            AppDomain componentsAppDomain = helper.CreateDomain(experiment.ExperimentInfo.Id);

            return Adapt(experiment, loggerNameRoot, nodesFactory, library, workspaceTypeDirectories, componentsAppDomain, new System.Threading.ManualResetEvent(false), true);
        }

        /// <summary>
        /// Adapts the specified experiment into runnable experiment.
        /// Adapt method validates given experiment and builds runnable experiment which is going to be executed by the experiment runner.
        /// During adapation the experiment is validated.
        /// In case of detected error in the experiment, this method returns empty RunnableExperimentBase with no nodes and no edges.
        /// There are several errors that can be detected.
        /// There are nodes connected to start, but not to graph end.
        /// Loops without decisions nodes.
        /// Input mappings are incorrect.
        /// Failed component load or instantiation, especially in case of incorrect configuration values.
        /// </summary>
        /// <param name="experiment">The experiment which is going to be adapted into RunnableExperiment..</param>
        /// <param name="loggerNameRoot">The logger name root - needed so that the logs are specific per experiment and experiment window.</param>
        /// <param name="nodesFactory">The nodes factory, by which all nodes in runnable experiment are created..</param>
        /// <param name="library">The library of components.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which components assemblies are going to be loaded into.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The event that allows signalling termination of the experiment</param>
        /// <returns>
        /// Runnable experiment that experiment is going to execute, pruned from nodes that are not connected to main flow beginning at Start node.
        /// In case of detected error in the experiment, this method returns empty RunnableExperimentBase with no nodes and no edges.
        /// </returns>
        public static RunnableExperimentBase Adapt(IExperiment experiment, LoggerNameRoot loggerNameRoot, IRunnableNodeFactory nodesFactory, 
                                                    Components.ComponentsLibrary library, List<string> workspaceTypeDirectories,
                                                    AppDomain componentsAppDomain, System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent, bool validateInputMapping)
        {
            RunnableExperimentBase runnableExperiment = new RunnableExperiment(nodesFactory, library, componentsAppDomain, terminateExperimentExecutionResetEvent);

            List<ExperimentNode> vertices;
            List<ExperimentNodeConnection> edges;
            bool noErrors = ExperimentValidator.ValidateExperiment(experiment, out vertices, out edges, workspaceTypeDirectories, validateInputMapping, loggerNameRoot);

            if (noErrors)
            {
                foreach (ExperimentNode node in vertices)
                {
                    try
                    {
                        runnableExperiment.AddNode(node.ID, node.Data.Metadata, loggerNameRoot);
                    }
                    catch (TraceLab.Core.Exceptions.IncorrectSubTemplateException ex)
                    {
                        runnableExperiment.Clear();
                        noErrors = false;
                        NLog.LogManager.GetCurrentClassLogger().Error(ex.Message);
                        node.SetError(ex.Message);
                        break;
                    }
                    catch (Exception ex)
                    {
                        runnableExperiment.Clear();
                        noErrors = false;
                        string msg = "Unable to initialize component: " + ex.Message;
                        NLog.LogManager.GetCurrentClassLogger().Error(msg, ex);
                        node.SetError(msg);
                        break;
                    }
                }
            }

            if (noErrors)
            {
                foreach (ExperimentNodeConnection edge in edges)
                {
                    runnableExperiment.AddDirectedEdge(edge.Source.ID, edge.Target.ID);
                }

            } 
            
            return runnableExperiment;
        }
    }
}

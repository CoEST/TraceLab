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
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;
using TraceLabSDK;
using TraceLab.Core.Decisions;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Responsible for node creation for the template graph
    /// </summary>
    internal class RunnableNodeFactory : IRunnableNodeFactory
    {
        protected IWorkspaceInternal Workspace { get; private set; }

        public RunnableNodeFactory(IWorkspaceInternal workspace)
        {
            Workspace = workspace;
        }

        /// <summary>
        /// Creates the Runnable node with a specific id based on the given metadata.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="metadata">The component metadata.</param>
        /// <param name="loggerNameRoot">The logger name root - needed so that the logs are specific per experiment and experiment window.</param>
        /// <param name="library">The library of components.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which components assemblies are going to be loaded into.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The event that allows signalling termination of the experiment; 
        /// Needed for the composite components sublevel experiments, so that they hold the referance to the same termination event as top level experiment.</param>
        /// <returns>
        /// Created node
        /// </returns>
        public virtual RunnableNode CreateNode(String nodeId, Metadata metadata, LoggerNameRoot loggerNameRoot, 
                                               ComponentsLibrary library, AppDomain componentsAppDomain, System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            RunnableNode retNode;

            ComponentMetadata componentMetadata = metadata as ComponentMetadata;
            DecisionMetadata decisionMetadata = metadata as DecisionMetadata;
            StartNodeMetadata startNodeMetadata = metadata as StartNodeMetadata;
            EndNodeMetadata endNodeMetadata = metadata as EndNodeMetadata;
            ScopeBaseMetadata scopeMetadata = metadata as ScopeBaseMetadata;
            LoopScopeMetadata loopMetadata = metadata as LoopScopeMetadata;
            CompositeComponentMetadata compositeComponentMetadata = metadata as CompositeComponentMetadata;
            ExitDecisionMetadata exitDecisionMetadata = metadata as ExitDecisionMetadata;
            // HERZUM SPRINT 2.0: TLAB-65
            ChallengeMetadata challengeMetadata = metadata as ChallengeMetadata;
            // END HERZUM SPRINT 2.0: TLAB-65
            if (componentMetadata != null)
            {
                TraceLabSDK.ComponentLogger logger = TraceLab.Core.Components.LoggerFactory.CreateLogger(loggerNameRoot, nodeId, componentMetadata);
                IComponent component = library.LoadComponent(componentMetadata, Workspace, logger, componentsAppDomain);
                retNode = new RunnableComponentNode(nodeId, componentMetadata.Label, component, logger, library, componentMetadata.WaitsForAllPredecessors);
            }
            else if (decisionMetadata != null)
            {
                IDecisionModule decisionModule = DecisionModuleFactory.LoadDecisionModule(decisionMetadata, Workspace, componentsAppDomain);
                retNode = new RunnableDecisionNode(nodeId, decisionMetadata.Label, decisionModule, library, decisionMetadata.WaitsForAllPredecessors);
            }
            else if (startNodeMetadata != null)
            {
                retNode = new RunnableStartNode(nodeId);
            }
            else if (endNodeMetadata != null)
            {
                retNode = new RunnableEndNode(nodeId, endNodeMetadata.WaitsForAllPredecessors);
            }
            else if (loopMetadata != null)
            {
                retNode = CreateLoopNode(nodeId, loopMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }
            else if (scopeMetadata != null)
            {
                retNode = CreateScopeCompositeComponentNode(nodeId, scopeMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }
            else if (compositeComponentMetadata != null)
            {
                retNode = CreateCompositeComponentNode(nodeId, compositeComponentMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }
            else if (exitDecisionMetadata != null)
            {
                retNode = new RunnablePrimitiveNode(nodeId, exitDecisionMetadata.WaitsForAllPredecessors);
            }
            // HERZUM SPRINT 2.0: TLAB-65
            else if (challengeMetadata != null)
            {
                retNode = CreateScopeCompositeComponentNode(nodeId, challengeMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent);
            }
            // END HERZUM SPRINT 2.0: TLAB-65
            else
            {
                throw new Exceptions.InconsistentTemplateException("Could not identify node type.");
            }

            return retNode;
        }

        #region Composite Components (Composites & Scopes & Loops)

        /// <summary>
        /// Creates the composite component node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <param name="loggerNameRoot">The logger name root - needed so that the logs are specific per experiment and experiment window.</param>
        /// <param name="library">The library of components.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which components assemblies are going to be loaded into.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The event that allows signalling termination of the experiment; The sublevel experiments hold the referance to the same termination event as top level experiment.</param>
        /// <returns>
        /// Created composite component node
        /// </returns>
        protected RunnableNode CreateCompositeComponentNode(string id, CompositeComponentMetadata compositeComponentMetadata, LoggerNameRoot loggerNameRoot,
                                                            ComponentsLibrary library, AppDomain componentsAppDomain, 
                                                            System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            NestedWorkspaceWrapper workspaceWrapper = WorkspaceWrapperFactory.CreateCompositeComponentWorkspaceWrapper(compositeComponentMetadata, Workspace, id, componentsAppDomain);
            NodesFactoryOfSubGraph nodesFactoryOfSubGraph = new NodesFactoryOfSubGraph(compositeComponentMetadata, workspaceWrapper);
            
            RunnableExperimentBase subExperiment = ConstructSubExperiment(compositeComponentMetadata, loggerNameRoot, library, 
                                                            componentsAppDomain, terminateExperimentExecutionResetEvent, nodesFactoryOfSubGraph);
            
            return new RunnableCompositeComponentNode(id, compositeComponentMetadata, subExperiment, workspaceWrapper, library, compositeComponentMetadata.WaitsForAllPredecessors);
        }

        /// <summary>
        /// Creates the scope composite component node. It actually returns the composite component, but with different nested workspace wrapper.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="scopeMetadata">The scope metadata.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <param name="library">The library.</param>
        /// <param name="componentsAppDomain">The components app domain.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The terminate experiment execution reset event.</param>
        /// <returns></returns>
        protected RunnableNode CreateScopeCompositeComponentNode(string id, ScopeBaseMetadata scopeMetadata, LoggerNameRoot loggerNameRoot,
                                                                 ComponentsLibrary library, AppDomain componentsAppDomain,
                                                                 System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            ScopeNestedWorkspaceWrapper workspaceWrapper = WorkspaceWrapperFactory.CreateCompositeComponentWorkspaceWrapper(scopeMetadata, Workspace, id, componentsAppDomain);

            //scope can standard runnable factory, unlike the composite component
            IRunnableNodeFactory nodesFactory = new RunnableNodeFactory(workspaceWrapper);

            RunnableExperimentBase subExperiment = ConstructSubExperiment(scopeMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent, nodesFactory);

            return new RunnableCompositeComponentNode(id, scopeMetadata, subExperiment, workspaceWrapper, library, scopeMetadata.WaitsForAllPredecessors);
        }

        /// <summary>
        /// Creates the scope composite component node. It actually returns the composite component, but with different nested workspace wrapper.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="loopScopeMetadata">The scope metadata.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <param name="library">The library.</param>
        /// <param name="componentsAppDomain">The components app domain.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The terminate experiment execution reset event.</param>
        /// <returns></returns>
        protected RunnableNode CreateLoopNode(string id, LoopScopeMetadata loopScopeMetadata, LoggerNameRoot loggerNameRoot,
                                                                 ComponentsLibrary library, AppDomain componentsAppDomain,
                                                                 System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent)
        {
            ScopeNestedWorkspaceWrapper workspaceWrapper = WorkspaceWrapperFactory.CreateCompositeComponentWorkspaceWrapper(loopScopeMetadata, Workspace, id, componentsAppDomain);

            //scope can standard runnable factory, unlike the composite component
            IRunnableNodeFactory nodesFactory = new RunnableNodeFactory(workspaceWrapper);

            RunnableExperimentBase subExperiment = ConstructSubExperiment(loopScopeMetadata, loggerNameRoot, library, componentsAppDomain, terminateExperimentExecutionResetEvent, nodesFactory);

            ILoopDecisionModule decisionModule = DecisionModuleFactory.LoadDecisionModule(loopScopeMetadata, Workspace, componentsAppDomain);
            return new RunnableLoopNode(id, decisionModule, loopScopeMetadata, subExperiment, workspaceWrapper, library, loopScopeMetadata.WaitsForAllPredecessors);
        }

        /// <summary>
        /// Constructs the sub experiment.
        /// </summary>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <param name="library">The library.</param>
        /// <param name="componentsAppDomain">The components app domain.</param>
        /// <param name="terminateExperimentExecutionResetEvent">The terminate experiment execution reset event - must be the same as top level experiment termination event.</param>
        /// <param name="nodesFactoryOfSubGraph">The nodes factory of sub graph.</param>
        /// <returns></returns>
        private RunnableExperimentBase ConstructSubExperiment(CompositeComponentBaseMetadata compositeComponentMetadata, LoggerNameRoot loggerNameRoot, 
                                                ComponentsLibrary library, AppDomain componentsAppDomain, 
                                                System.Threading.ManualResetEvent terminateExperimentExecutionResetEvent, IRunnableNodeFactory nodesFactoryOfSubGraph)
        {
            //add to experiment id the owner node id to make it unique
            LoggerNameRoot compositeComponentNodeLoggerNameRoot = loggerNameRoot.CreateLoggerNameRootForCompositeNode(compositeComponentMetadata);

            RunnableExperimentBase subExperiment = GraphAdapter.Adapt(compositeComponentMetadata.ComponentGraph, compositeComponentNodeLoggerNameRoot,
                                                                        nodesFactoryOfSubGraph, library, Workspace.TypeDirectories,
                                                                        componentsAppDomain, terminateExperimentExecutionResetEvent, false);

            if (subExperiment.IsEmpty)
            {
                throw new TraceLab.Core.Exceptions.IncorrectSubTemplateException("Unable to execute subexperiment due to errors.");
            }
            return subExperiment;
        }

        #endregion
    }
}
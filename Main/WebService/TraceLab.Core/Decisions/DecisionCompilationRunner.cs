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
using TraceLab.Core.Experiments;
using System.Reflection;
using TraceLab.Core.Utilities;
using TraceLab.Core.Components;
using System.Security.Permissions;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Decision Compilation Runner is a central point to execute the decision compilation. 
    /// It is responsible for preparation of the code, and calling decision compiler to compile code based on all prepared state.
    /// </summary>
    public static class DecisionCompilationRunner
    {
        #region Public

        /// <summary>
        /// Compiles the code of the single decision node or loop scope node. It handles DecisionNode and LoopScopeNode slightly differently.
        /// Method.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="experiment">The experiment.</param>
        /// <param name="workspaceTypesDirectories">The workspace types directories.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        public static void CompileDecision(ExperimentNode node, IExperiment experiment, List<string> workspaceTypesDirectories, LoggerNameRoot loggerNameRoot)
        {
            ExperimentDecisionNode decisionNode = node as ExperimentDecisionNode;
            if (decisionNode != null)
            {
                Dictionary<string, string> successorNodeLabelIdLookup = PrepareSuccessorNodesLabelIdLookup(node, experiment);
                CompileDecisionInternal(decisionNode, experiment, workspaceTypesDirectories, loggerNameRoot, successorNodeLabelIdLookup);
            }
            else
            {
                LoopScopeNode loopScopeNode = node as LoopScopeNode;
                if (loopScopeNode != null)
                {
                    //loop scope does not need successor nodes lookups, so pass in empty dictionary
                    Dictionary<string, string> successorNodeLabelIdLookup = new Dictionary<string, string>();
                    CompileDecisionInternal(loopScopeNode, experiment, workspaceTypesDirectories, loggerNameRoot, successorNodeLabelIdLookup);
                }
            }
        }

        /// <summary>
        /// Compiles all decision nodes code and loops code in the given experiment
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <param name="availableInputMappingsPerNode">The available input mappings per node.</param>
        /// <param name="workspaceTypesDirectories">The workspace types directories.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <returns>
        /// true if there were no errors, otherwise false
        /// </returns>
        public static bool CompileAllDecisionNodes(IExperiment experiment, InputMappings availableInputMappingsPerNode, 
                                                   List<string> workspaceTypesDirectories, LoggerNameRoot loggerNameRoot)
        {
            bool noErrors = true;

            foreach (ExperimentNode node in experiment.Vertices)
            {
                IDecision decisionMetadata = node.Data.Metadata as IDecision;
                if (decisionMetadata != null)
                {
                    try
                    {
                        //build successor nodes label id lookup
                        Dictionary<string, string> successorNodeLabelIdLookup = PrepareSuccessorNodesLabelIdLookup(node, experiment);
                        Dictionary<string, string> predeccessorsOutputsNameTypeLookup = PreparePredeccessorsOutputsNameTypeLookup(node, availableInputMappingsPerNode);

                        node.ClearError();

                        BuildSourceAndCompileDecisionModule(decisionMetadata, successorNodeLabelIdLookup,
                            predeccessorsOutputsNameTypeLookup, workspaceTypesDirectories, loggerNameRoot);
                    }
                    catch (ArgumentException ex)
                    {
                        noErrors = false;
                        node.SetError(ex.Message);
                    }
                }
            }

            return noErrors;
        }
        
        #endregion

        #region Private Compile Decisions Helper Methods

        /// <summary>
        /// Compiles the decision.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="experiment">The experiment.</param>
        /// <param name="workspaceTypesDirectories">The workspace types directories.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        private static void CompileDecisionInternal(ExperimentNode node, IExperiment experiment, List<string> workspaceTypesDirectories, 
                                                    LoggerNameRoot loggerNameRoot, Dictionary<string, string> successorNodeLabelIdLookup)
        {
            InputMappings availableInputMappingsPerNode = new InputMappings(experiment);

            Dictionary<string, string> predeccessorsOutputsNameTypeLookup = PreparePredeccessorsOutputsNameTypeLookup(node, availableInputMappingsPerNode);

            IDecision decisionMetadata = (IDecision)node.Data.Metadata;
            try
            {
                if (decisionMetadata != null)
                {
                    node.ClearError();

                    BuildSourceAndCompileDecisionModule(decisionMetadata, successorNodeLabelIdLookup, predeccessorsOutputsNameTypeLookup, workspaceTypesDirectories, loggerNameRoot);

                    decisionMetadata.CompilationStatus = TraceLab.Core.Components.CompilationStatus.Successful;
                }
            }
            catch (ArgumentException ex)
            {
                decisionMetadata.CompilationStatus = TraceLab.Core.Components.CompilationStatus.Failed;
                node.SetError(ex.Message);
            }
        }

        /// <summary>
        /// Builds the source of decision module and compile decision module into the assembly
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="successorNodeLabelIdLookup">The successor node label id lookup.</param>
        /// <param name="predeccessorsOutputsNameTypeLookup">The predeccessors outputs name type lookup.</param>
        /// <param name="workspaceTypesDirectories">The workspace types directories.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        private static void BuildSourceAndCompileDecisionModule(IDecision metadata, Dictionary<string, string> successorNodeLabelIdLookup,
                                            Dictionary<string, string> predeccessorsOutputsNameTypeLookup, 
                                            List<string> workspaceTypesDirectories, LoggerNameRoot loggerNameRoot)
        {
            metadata.FireRequestLatestCode();

            //create local componentlogger (ComponentLoggerImplementation implements MarshalByRefObject, thanks to which it can pass logs between appdomains
            TraceLabSDK.ComponentLogger logger = LoggerFactory.CreateLogger(loggerNameRoot, metadata.UniqueDecisionID, metadata);

            //construct the final code, and collect types assemblies locations to be referenced by the compilator
            HashSet<string> assembliesReferenceLocations;
            string finalDecisionModuleSourceCode = DecisionCodeBuilder.BuildCodeSource(metadata, workspaceTypesDirectories, 
                                successorNodeLabelIdLookup, predeccessorsOutputsNameTypeLookup, logger, out assembliesReferenceLocations);

            // Create the new domain with whatever current security evidence we're running with using the library helper
            LibraryHelper helper = new LibraryHelper(workspaceTypesDirectories);

            AppDomain newDomain = helper.CreateDomain("DecisionModuleCompilation");
            newDomain.Load(Assembly.GetExecutingAssembly().GetName());
            helper.PreloadWorkspaceTypes(newDomain);

            //// Load our output assembly into the other domain.
            DecisionModuleCompilator compiler =
                (DecisionModuleCompilator)newDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DecisionModuleCompilator).FullName,
                                                                            false,
                                                                            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance, null,
                                                                            new object[] { },
                                                                            System.Globalization.CultureInfo.CurrentCulture, new object[] { });
            
            compiler.CompileDecisionModule(finalDecisionModuleSourceCode, metadata.SourceAssembly, assembliesReferenceLocations);

#if !MONO_DEV
            //when developing on mono in MonoDevelop application crashes when unloading appdomain.
            //it only happens from within of MonoDevelop with attached debugger. Running normally, works fine.
            AppDomain.Unload(newDomain);
#endif
        }

        /// <summary>
        /// Prepares the lookup of the successor nodes label to their ids.
        /// </summary>
        /// <param name="decisionNode">The decision node for which the lookup is created</param>
        /// <param name="experiment">The experiment - needed to find outgoing edges</param>
        /// <returns>lookup of the labels of the successor nodes to their guids. the key is the node label, and the value is the node id</returns>
        public static Dictionary<string, string> PrepareSuccessorNodesLabelIdLookup(ExperimentNode decisionNode, IExperiment experiment)
        {
            Dictionary<string, string> successorNodeLabelIdLookup = new Dictionary<string, string>();
        //foreach (ExperimentNodeConnection outEdge in experiment.OutEdges(decisionNode)) TLAB-171
            foreach (ExperimentNodeConnection outEdge in decisionNode.Owner.OutEdges(decisionNode))
            {
                try
                {
                    successorNodeLabelIdLookup.Add(outEdge.Target.Data.Metadata.Label, outEdge.Target.ID);
                }
                catch (ArgumentException ex)
                {
                    throw new DecisionCodeParserException(String.Format(Messages.DecisionErrorAmbigousChoiceOfTwoNodes + "{0}", outEdge.Target.Data.Metadata.Label), ex);
                }
            }
            return successorNodeLabelIdLookup;
        }

        /// <summary>
        /// Prepares the predeccessors outputs lookup.
        /// </summary>
        /// <param name="vert">The vert.</param>
        /// <param name="availableInputMappingsPerNode">The available input mappings per node.</param>
        /// <returns>
        /// list of all the outputs from previous nodes. the key is output name, and the value is the type of that output
        /// </returns>
        public static Dictionary<string, string> PreparePredeccessorsOutputsNameTypeLookup(ExperimentNode vert, InputMappings availableInputMappingsPerNode)
        {
            Dictionary<string, string> predeccessorsOutputsNameTypeLookup;

            if (availableInputMappingsPerNode.TryGetValue(vert, out predeccessorsOutputsNameTypeLookup) == false)
            {
                predeccessorsOutputsNameTypeLookup = new Dictionary<string, string>(); //return empty - there is not path from start node to decision
            }

            return predeccessorsOutputsNameTypeLookup;
        }

        #endregion

        #region Static Properties

        private static string s_decisionDirectoryPath;
        /// <summary>
        /// Gets or sets the decision directory path.
        /// Directory path is the directory into which the compiled decision assemblies are being saved into.
        /// </summary>
        /// <value>
        /// The decision directory path.
        /// </value>
        public static string DecisionDirectoryPath
        {
            get
            {
                return s_decisionDirectoryPath;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Decision directory path cannot be null");
                if (!System.IO.Path.IsPathRooted(value))
                    throw new ArgumentException("Absolute decision directory path information is required.", "value");
                if (!System.IO.Directory.Exists(value))
                {
                    System.IO.Directory.CreateDirectory(value);
                }
                s_decisionDirectoryPath = value;
            }
        }

        #endregion
    }
}

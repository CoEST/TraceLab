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
using System.Reflection;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Responsible for loading decision modules and loop decision modules classes.
    /// Both classes are dynamically compiled classes previously by DecisionCompilationRunner. 
    /// DecisionModuleFactory instantiate objects of those classes to be used during execution of the experiment
    /// </summary>
    static class DecisionModuleFactory
    {
        /// <summary>
        /// Loads the decision module based on the decision metadata
        /// </summary>
        /// <param name="decisionMetadata">The decision metadata.</param>
        /// <param name="workspaceWrapper">The workspace wrapper.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which decision assembly is going to be loaded into.</param>
        /// <returns>Loaded decision</returns>
        internal static IDecisionModule LoadDecisionModule(DecisionMetadata decisionMetadata, IWorkspaceInternal workspaceWrapper,
                                                                                AppDomain componentsAppDomain)
        {
            DecisionLoader loader = ConstructDecisionModuleInComponentsAppDomain(decisionMetadata, workspaceWrapper, componentsAppDomain);

            return (IDecisionModule)loader.LoadedDecisionModule;
        }

        /// <summary>
        /// Loads the decision module based on the decision metadata
        /// </summary>
        /// <param name="decisionMetadata">The decision metadata.</param>
        /// <param name="workspaceWrapper">The workspace wrapper.</param>
        /// <param name="componentsAppDomain">The components app domain is the app domain which decision assembly is going to be loaded into.</param>
        /// <returns>Loaded decision</returns>
        internal static ILoopDecisionModule LoadDecisionModule(LoopScopeMetadata loopMetadata, IWorkspaceInternal workspaceWrapper,
                                                                                AppDomain componentsAppDomain)
        {
            DecisionLoader loader = ConstructDecisionModuleInComponentsAppDomain(loopMetadata, workspaceWrapper, componentsAppDomain);

            return (ILoopDecisionModule)loader.LoadedDecisionModule;
        }

        private static DecisionLoader ConstructDecisionModuleInComponentsAppDomain(IDecision decisionMetadata, IWorkspaceInternal workspaceWrapper, AppDomain componentsAppDomain)
        {
            // DecisionLoader must be MarshalByRef, otherwise the properties don't get filled out the 
            // way that we want them to.
            DecisionLoader loader = (DecisionLoader)componentsAppDomain.CreateInstanceAndUnwrap(
                Assembly.GetExecutingAssembly().FullName, typeof(DecisionLoader).FullName, false,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance, null,
                new object[] { decisionMetadata.Classname, decisionMetadata.SourceAssembly, workspaceWrapper },
                System.Globalization.CultureInfo.CurrentCulture, new object[] { });

            // Perform the actual load by passing a reference to the Loader's Load function to the new 
            // AppDomain to execute
            loader.Load();
            return loader;
        }
    }
}

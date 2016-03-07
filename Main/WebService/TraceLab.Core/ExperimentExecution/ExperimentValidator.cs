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
using TraceLab.Core.Components;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// ExperimentValidator is a top class validating entire experiments. It uses GraphValidator for validating graph structure, InputMapping validator to
    /// validate input mappings, and it also validates if all components are currently in the components library.
    /// </summary>
    public class ExperimentValidator
    {
        /// <summary>
        /// Validates the entire experiment. 
        /// It validates the structure of the experimental graph, 
        /// validates all inputs and outputs, 
        /// checks if all components are in the component library.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <returns>true if there is no errors, false, if errors has been detected</returns>
        public static bool ValidateExperiment(IExperiment experiment, List<string> workspaceTypesDirectories, LoggerNameRoot loggerNameRoot)
        {
            List<ExperimentNode> vertices;
            List<ExperimentNodeConnection> edges;
            return ValidateExperiment(experiment, out vertices, out edges, workspaceTypesDirectories, true, loggerNameRoot);
        }

        /// <summary>
        /// Validates the entire experiment.
        /// It validates the structure of the experimental graph,
        /// validates all inputs and outputs,
        /// checks if all components are in the component library.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <param name="vertices">The vertices, that has been parsed from the start node. Nodes that are not connected to the start are being skipped.</param>
        /// <param name="edges">The edges, that has been parsed from the start node.</param>
        /// <returns>
        /// true if there is no errors, false, if errors has been detected
        /// </returns>
        public static bool ValidateExperiment(IExperiment experiment, out List<ExperimentNode> vertices, out List<ExperimentNodeConnection> edges,
                                              List<string> workspaceTypesDirectories, bool validateInputMapping, LoggerNameRoot loggerNameRoot)
        {
            bool noErrors = GraphValidator.Validate(experiment, out vertices, out edges);

            if (noErrors)
            {
                var availableInputMappingsPerNode = new TraceLab.Core.Utilities.InputMappings(experiment);
                if (validateInputMapping)
                {
                    noErrors = InputMappingValidator.Validate(experiment, availableInputMappingsPerNode);
                }
                
                if (noErrors)
                {
                    //recompile all decisions
                    noErrors = TraceLab.Core.Decisions.DecisionCompilationRunner.CompileAllDecisionNodes(experiment, availableInputMappingsPerNode, 
                                                                                                            workspaceTypesDirectories, loggerNameRoot);
                }
            }

            if (noErrors)
            {
                noErrors = ValidComponents(vertices);
            }
            
            return noErrors;
        }
        
        /// <summary>
        /// Validates if all components exist in the library, and their mapping format.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns>true if all components are in the library, false otherwise</returns>
        private static bool ValidComponents(List<ExperimentNode> vertices)
        {
            bool noErrors = true;

            //validate if all components are in the library
            foreach (ExperimentNode node in vertices)
            {
                ComponentMetadata componentMetadata = node.Data.Metadata as ComponentMetadata;
                if (componentMetadata != null)
                {
                    if (componentMetadata.ComponentMetadataDefinition == null)
                    {
                        noErrors = false;
                        string errorMessage = String.Format(System.Globalization.CultureInfo.CurrentCulture, "Component library does not contain any Component of the given ID {0}", componentMetadata.ComponentMetadataDefinitionID);
                        NLog.LogManager.GetCurrentClassLogger().Error(errorMessage);
                        node.SetError(errorMessage);
                    }
                    else
                    {
                        //if component exist check also output mapping formats
                        string errorMessage;
                        if (CheckOutputMappingFormat(componentMetadata, out errorMessage) == false)
                        {
                            noErrors = false;
                            NLog.LogManager.GetCurrentClassLogger().Error(errorMessage);
                            node.SetError(errorMessage);
                        }
                    }
                }
            }

            return noErrors;
        }

        /// <summary>
        /// Checks the output mappings format for illegal characters.
        /// In particular it assures that mappings do not contain Workspace Namespace_Delimiter character.
        /// </summary>
        /// <param name="componentMetadata">The component metadata.</param>
        /// <param name="errorMessage">The error message, if method returns false</param>
        /// <returns>true, if there are no errors, otherwise false.</returns>
        private static bool CheckOutputMappingFormat(ComponentMetadata componentMetadata, out string errorMessage)
        {
            bool noErrors = true;

            StringBuilder error = new StringBuilder();

            //validate also if mappings do not contain illegal characters (in particular workspace namespace delimiter)
            foreach (IOItem item in componentMetadata.IOSpec.Output.Values)
            {
                if (item.MappedTo.Contains(TraceLab.Core.Workspaces.Workspace.NAMESPACE_DELIMITER))
                {
                    noErrors = false;
                    error.AppendLine(String.Format("Output mapping '{0}' cannot contain character '{1}'", item.MappedTo, TraceLab.Core.Workspaces.Workspace.NAMESPACE_DELIMITER));
                }
            }

            errorMessage = error.ToString();

            return noErrors;
        }
    }
}

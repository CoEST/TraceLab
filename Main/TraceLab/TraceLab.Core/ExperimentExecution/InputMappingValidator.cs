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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.ExperimentExecution
{
    internal class InputMappingValidator
    {
        /// <summary>
        /// Validates if the list of outputs from incoming vertices satisfies all the inputs for all vertices in the lookup.
        /// </summary>
        /// <returns>true, if all vertices have correct mapping, false if there is any node with input that is not satisfied by previous outputs.</returns>
        public static bool Validate(IExperiment experiment, InputMappings availableInputMappingsPerNode)
        {
            bool retVal = true;

            foreach (ExperimentNode currentNode in availableInputMappingsPerNode.Nodes)
            {
                bool validationResults = ValidateInputMapping(currentNode, availableInputMappingsPerNode[currentNode]);
                if (retVal != false)
                {
                    retVal = validationResults;
                }
            }
    
            return retVal;
        }
        
        /// <summary>
        /// Validates if the list of outputs from incoming vertices satisfies all the inputs for the current node
        /// </summary>
        /// <param name="node">Node to be validated</param>
        /// <param name="incomingOutputs">List of incoming outputs from the previous nodes</param>
        /// <param name="noErrors">assigns false if error has been detected, otherwise keep it as it was</param>
        private static bool ValidateInputMapping(ExperimentNode node, Dictionary<string, string> incomingOutputs)
        {
            bool noErrors = true;
            IConfigurableAndIOSpecifiable ioSpecMetadata = node.Data.Metadata as IConfigurableAndIOSpecifiable;
            if (ioSpecMetadata != null)
            {
                //check if the list of outputs from incoming vertices satisfies all the inputs for the current node
                foreach (IOItem inputItem in ioSpecMetadata.IOSpec.Input.Values)
                {
                    if (incomingOutputs.ContainsKey(inputItem.MappedTo) == false)
                    {
                        noErrors = false;
                        GraphValidator.SetErrorOnNode(node, String.Format(CultureInfo.CurrentCulture, "The component attempts to load '{0}' from the Workspace. However, none of the previous components outputs '{1}' to the Workspace.", inputItem.MappedTo, inputItem.MappedTo));
                    }
                }
            }
            return noErrors;
        }
    }
}

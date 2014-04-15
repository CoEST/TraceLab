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

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Helper class responsible for creating contant component definitions (hard coded)
    /// </summary>
    internal static class MetadataDefinitionFactory
    {
        /// <summary>
        /// Method adds the constant fixed definitions into given components defintion collection.
        /// Currently it adds two definitions, decision and loop.
        /// </summary>
        /// <param name="componentsDefinitionCollection">The collection of components definition into which fixed definition are going to be added</param>
        internal static void LoadConstantDefinitionsIntoLibrary(ObservableComponentDefinitionCollection componentsDefinitionCollection)
        {
            DecisionMetadataDefinition decisionMetadataDefinition = BuildDecisionDefinition();
            componentsDefinitionCollection.Add(decisionMetadataDefinition);

            LoopMetadataDefinition loopMetadataDefinition = BuildWhileLoopDefinition();
            componentsDefinitionCollection.Add(loopMetadataDefinition);

            DecisionMetadataDefinition gotoDecisionMetadataDefinition = BuildGoToDecisionDefinition();
            componentsDefinitionCollection.Add(gotoDecisionMetadataDefinition);
        }
                
        /// <summary>
        /// Builds the decision definition.
        /// </summary>
        /// <returns></returns>
        private static DecisionMetadataDefinition BuildDecisionDefinition()
        {
            DecisionMetadataDefinition decisionMetadataDefinition = new DecisionMetadataDefinition(DecisionMetadataDefinition.DecisionGuid);
            decisionMetadataDefinition.Label = "If Statement Decision (with scopes)";
            decisionMetadataDefinition.Tags.SetTag("Decision & Loops", false);
            decisionMetadataDefinition.Description = "Decisions provides way of specifying execution paths via corresponding scopes based on runtime data in the workspace.";
            return decisionMetadataDefinition;
        }
        
        private static DecisionMetadataDefinition BuildGoToDecisionDefinition()
        {
            DecisionMetadataDefinition decisionMetadataDefinition = new DecisionMetadataDefinition(DecisionMetadataDefinition.GotoDecisionGuid);
            decisionMetadataDefinition.Label = "Goto Decision";
            decisionMetadataDefinition.Tags.SetTag("Decision & Loops", false);
            decisionMetadataDefinition.Description = "Goto Decisions provides way of specifying execution paths based on runtime data in the workspace.";
            return decisionMetadataDefinition;
        }

        /// <summary>
        /// Builds the loop definition.
        /// </summary>
        /// <returns></returns>
        private static LoopMetadataDefinition BuildWhileLoopDefinition()
        {
            LoopMetadataDefinition loopMetadataDefinition = new LoopMetadataDefinition(LoopMetadataDefinition.WhileLoopGuid);
            loopMetadataDefinition.Label = "While Loop";
            loopMetadataDefinition.Tags.SetTag("Decision & Loops", false);
            loopMetadataDefinition.Description = "Loop provides a editable inner graph that can be repeated multiple types based on the given condition.";
            return loopMetadataDefinition;
        }
    }
}

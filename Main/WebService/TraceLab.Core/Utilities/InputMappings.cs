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

namespace TraceLab.Core.Utilities
{
    /// <summary>
    /// Datastructures represents available input mappings per each node in the given experiment. 
    /// </summary>
    public class InputMappings
    {
        /// <summary>
        /// The dictionary of experiment nodes and their corresponding available mappings (mapping name, and their types)
        /// </summary>
        private Dictionary<ExperimentNode, Dictionary<string, string>> m_availableInputMappingsPerNode;

        public InputMappings(IExperiment experiment)
        {
            m_availableInputMappingsPerNode = new Dictionary<ExperimentNode, Dictionary<string, string>>();

            ConstructInputMappingsPerNode(experiment);
        }

        #region Private

        /// <summary>
        /// It is TEMPORARY implementation. To do!
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        private void ConstructInputMappingsPerNode(IExperiment experiment)
        {
            IncludeMappingsFromExperiment(experiment);
        }

        private void IncludeMappingsFromExperiment(IExperiment experiment)
        {
            List<ExperimentNode> allNodes = new List<ExperimentNode>();
            CollectNodesInExperiment(experiment, ref allNodes);

            foreach (ExperimentNode currentNode in allNodes)
            {
                IConfigurableAndIOSpecifiable ioSpecMetadata = currentNode.Data.Metadata as IConfigurableAndIOSpecifiable;
                if (ioSpecMetadata != null)
                {
                    foreach (ExperimentNode otherNode in allNodes)
                    {
                        if (otherNode != currentNode)
                        {
                            foreach (IOItem outputItem in ioSpecMetadata.IOSpec.Output.Values)
                            {
                                AddMapping(otherNode, outputItem.MappedTo, outputItem.IOItemDefinition.Type);
                            }
                        }
                    }
                }
            }
        }

        private void CollectNodesInExperiment(IExperiment experiment, ref List<ExperimentNode> nodes)
        {
            foreach (ExperimentNode currentNode in experiment.Vertices)
            {
                ScopeNodeBase scopeNode = currentNode as ScopeNodeBase;
                if (scopeNode != null)
                {
                    CollectNodesInExperiment(scopeNode.CompositeComponentMetadata.ComponentGraph, ref nodes);
                }

                nodes.Add(currentNode);
            }
        }
                
        private void AddMapping(ExperimentNode node, string mapping, string mappingType)
        {
            Dictionary<string, string> mappingsForNode;
            if (m_availableInputMappingsPerNode.TryGetValue(node, out mappingsForNode) == false)
            {
                mappingsForNode = new Dictionary<string, string>();
                m_availableInputMappingsPerNode.Add(node, mappingsForNode);
            }

            if (mappingsForNode.ContainsKey(mapping) == false)
            {
                mappingsForNode.Add(mapping, mappingType);
            }
        }

        #endregion

        #region Public

        public IEnumerable<ExperimentNode> Nodes
        {
            get
            {
                return m_availableInputMappingsPerNode.Keys;
            }
        }

        public Dictionary<string, string> this[ExperimentNode node]
        {
            get
            {
                return m_availableInputMappingsPerNode[node];
            }
        }

        public bool TryGetValue(ExperimentNode node, out Dictionary<string, string> mappings)
        {
            return m_availableInputMappingsPerNode.TryGetValue(node, out mappings);
        }

        public bool ContainsMappingsForNode(ExperimentNode node)
        {
            return m_availableInputMappingsPerNode.ContainsKey(node);
        }

        #endregion
    }
}

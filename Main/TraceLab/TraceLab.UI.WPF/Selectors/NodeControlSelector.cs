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

using System.Windows;
using System.Windows.Controls;
using GraphSharp.Controls;
using System.Collections.Generic;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Selectors
{
    /// <summary>
    /// The class is responsible for selecting the template for the specific vertex control.
    /// Based on the node type selectors tries to locate the template within currently available resources.
    /// Thus each graph view (for example DockableGraph, ScopeGraph or ReadonlyGraph) need to provide
    /// all templates in their resource dictionary.
    /// </summary>
    public class NodeControlSelector : DataTemplateSelector
    {
        /// <summary>
        /// Selects the template based on the given vertex control type
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var control = container as FrameworkElement;
            var node = item as VertexControl;

            DataTemplate template = null;
            if (control != null)
            {
                if (node != null)
                {
                    if (node.Vertex is TraceLab.Core.Experiments.ExperimentStartNode)
                    {
                        template = (DataTemplate)control.TryFindResource("StartNodeControl");
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.ExperimentEndNode)
                    {
                        template = (DataTemplate)control.TryFindResource("EndNodeControl");
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.ComponentNode)
                    {
                        template = (DataTemplate)control.TryFindResource("ComponentNodeControl");
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.ExperimentDecisionNode)
                    {
                        ExperimentDecisionNode decisionNode = (ExperimentDecisionNode)node.Vertex;
                        if (IsGotoDecision(decisionNode))
                        {
                            template = (DataTemplate)control.TryFindResource("GotoDecisionNodeControl");
                        }
                        else
                        {
                            template = (DataTemplate)control.TryFindResource("DecisionNodeControl");
                        }
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.ScopeNode)
                    {
                        //scope node must be checked before CompositeComponentNode, as it extends CompositeComponentNode
                        template = (DataTemplate)control.TryFindResource("ScopeNodeControl");
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.LoopScopeNode)
                    {
                        template = (DataTemplate)control.TryFindResource("LoopScopeNodeControl");
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.CompositeComponentNode)
                    {
                        template = (DataTemplate)control.TryFindResource("CompositeComponentNodeControl");
                    }
                    else if (node.Vertex is TraceLab.Core.Experiments.ExitDecisionNode)
                    {
                        template = (DataTemplate)control.TryFindResource("ExitDecisionNodeControl");
                    }
                }

                if(template == null)
                    template = (DataTemplate)control.TryFindResource("DefaultNodeControl");
            }

            return template;
        }

        /// <summary>
        /// Determines whether the given decision is an old version decision.
        /// Previously decisions were independent nodes without scopes and corresponding exit node.
        /// Thus for old decisions we want to choose another template.
        /// </summary>
        /// <param name="decisionNode">The decision node.</param>
        /// <returns>
        ///   <c>true</c> if the specified decision node is old decision; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsGotoDecision(ExperimentDecisionNode decisionNode)
        {
            IEditableExperiment experiment = decisionNode.Owner as IEditableExperiment;

            IEnumerable<ExperimentNodeConnection> outEdges;
            if (experiment.TryGetOutEdges(decisionNode, out outEdges))
            {
                foreach (ExperimentNodeConnection connection in outEdges)
                {
                    if (connection.Target is ExitDecisionNode)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}

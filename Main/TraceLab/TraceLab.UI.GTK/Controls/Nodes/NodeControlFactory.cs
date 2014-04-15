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
//
using System;
using TraceLab.Core.Experiments;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    public class NodeControlFactory
    {
        public NodeControlFactory(ApplicationContext applicationContext)
        {
            m_applicationContext = applicationContext;
            m_mapNodesToControls = new Dictionary<ExperimentNode, BasicNodeControl>();
        }

        public BasicNodeControl CreateNodeControl(ExperimentNode node) 
        {
            BasicNodeControl control;

            if (node is ComponentNode) 
            {
                control = new ComponentControl(node, m_applicationContext);
            } 
            else if (node is ExperimentStartNode) 
            {
                control = new StartNodeControl(node, m_applicationContext);
            } 
            else if (node is ExperimentEndNode) 
            { 
                control = new EndNodeControl(node, m_applicationContext);
            }
            else if (node is ExperimentDecisionNode) 
            { 
                control = new DecisionNodeControl(node, m_applicationContext);
            }
            else if (node is CompositeComponentNode)
            {
                control = new CompositeComponentControl(node, m_applicationContext);
            }
            else 
            {
                control = new BasicNodeControl(node, m_applicationContext);
            }

            m_mapNodesToControls.Add(node, control);

            return control;
        }


        public bool TryGetNodeControl(ExperimentNode node, out BasicNodeControl nodeControl)
        {
            return m_mapNodesToControls.TryGetValue(node, out nodeControl);
        }
        
        public bool RemoveFromLookup(ExperimentNode node)
        {
            return m_mapNodesToControls.Remove(node);
        }

        private ApplicationContext m_applicationContext;
        private Dictionary<ExperimentNode, BasicNodeControl> m_mapNodesToControls;
    }
}


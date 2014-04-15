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

namespace TraceLab.UI.GTK
{
    /// <summary>
    /// A class responsible for drawing experiment onto experiment canvas
    /// </summary>
    internal class ExperimentDrawer
    {
        private NodeControlFactory m_nodeControlFactory;
        private NodeConnectionControlFactory m_nodeConnectionControlFactory;
        private ExperimentCanvasWidget m_experimentCanvasWidget;

        public ExperimentDrawer(ExperimentCanvasWidget experimentCanvasWidget,
                                NodeControlFactory nodeControlFactory, 
                                NodeConnectionControlFactory nodeConnectionControlFactory)
        {
            m_experimentCanvasWidget = experimentCanvasWidget;
            m_nodeControlFactory = nodeControlFactory;
            m_nodeConnectionControlFactory = nodeConnectionControlFactory;
        }

        #region Draw Experiment
        
        public void DrawExperiment(IExperiment experiment, bool editable) 
        {
            foreach(ExperimentNode node in experiment.Vertices) 
            {
                DrawComponent(node, editable);
            }
            
            foreach(ExperimentNodeConnection edge in experiment.Edges) 
            {
                DrawEdge(edge, editable);
            }
        }
        
        public void DrawComponent(ExperimentNode node, bool editable) 
        {
            BasicNodeControl componentControl = m_nodeControlFactory.CreateNodeControl(node);
            m_experimentCanvasWidget.ExperimentCanvas.View.Drawing.Add(componentControl);
            componentControl.MoveTo(node.Data.X, node.Data.Y);
            m_experimentCanvasWidget.ExperimentCanvas.View.ClearSelection();
            componentControl.IsEditable = editable;
        }
        
        public void DrawEdge(ExperimentNodeConnection edge, bool editable) 
        {
            ExperimentNode source = edge.Source;
            ExperimentNode target = edge.Target;
            
            BasicNodeControl sourceControl, targetControl;
            if(m_nodeControlFactory.TryGetNodeControl(source, out sourceControl) &&
               m_nodeControlFactory.TryGetNodeControl(target, out targetControl))
            {   
                NodeConnectionControl connection = m_nodeConnectionControlFactory.CreateNewNodeConnectionControl(sourceControl, targetControl);
                connection.ConnectStart(sourceControl.ConnectorAt(source.Data.X, source.Data.Y));
                connection.ConnectEnd(targetControl.ConnectorAt(target.Data.X, target.Data.Y));
                connection.UpdateConnection();
                connection.IsEditable = editable;
                
                m_experimentCanvasWidget.ExperimentCanvas.View.Drawing.Add(connection);
                m_experimentCanvasWidget.ExperimentCanvas.View.ClearSelection();
            }
        }
        
        #endregion Draw Experiment
    }
}


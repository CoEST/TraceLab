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
    /// <summary>
    /// Node connection control factory exposes methods that create 
    /// </summary>
    public class NodeConnectionControlFactory
    {
        private ApplicationContext m_applicationContext;

        public NodeConnectionControlFactory(ApplicationContext applicationContext)
        {
            m_applicationContext = applicationContext;
            m_mapEdgesToControls = new Dictionary<ExperimentNodeConnection, NodeConnectionControl>();
        }

        public NodeConnectionControl CreateNewNodeConnectionControl(BasicNodeControl sourceComponent, BasicNodeControl targetComponent) 
        {
            Experiment experimentOwner = m_applicationContext.Application.Experiment;
            NodeConnectionControl connectionControl = new NodeConnectionControl(experimentOwner, sourceComponent, targetComponent);
            connectionControl.ConnectionCompleted += HandleConnectionCompleted;
            return connectionControl; 
        }

        public NodeConnectionControl CreateNewNodeConnectionControl() 
        {
            Experiment experimentOwner = m_applicationContext.Application.Experiment;
            NodeConnectionControl connectionControl = new NodeConnectionControl(experimentOwner);
            connectionControl.ConnectionCompleted += HandleConnectionCompleted;
            return connectionControl; 
        }

        /// <summary>
        /// Handles the connection completed event.
        /// When node connection control is connected to the end and completed,
        /// this method stores the mapping from Experiment Node Connection (model)
        /// to Node Connection Control (view).
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// E.
        /// </param>
        private void HandleConnectionCompleted(object sender, ConnectionCompletedEventArgs e) 
        {
            NodeConnectionControl connectionControl = sender as NodeConnectionControl;
            m_mapEdgesToControls.Add(e.ExperimentNodeConnection, connectionControl);

            //listener now can be removed
            connectionControl.ConnectionCompleted -= HandleConnectionCompleted;
        }

        public bool TryGetConnectionControl(ExperimentNodeConnection nodeConnection, out NodeConnectionControl connectionControl)
        {
            return m_mapEdgesToControls.TryGetValue(nodeConnection, out connectionControl);
        }

        public bool RemoveFromLookup(ExperimentNodeConnection nodeConnection)
        {
            return m_mapEdgesToControls.Remove(nodeConnection);
        }

        public void ClearLookup ()
        {
            m_mapEdgesToControls.Clear();
        }

        private Dictionary<ExperimentNodeConnection, NodeConnectionControl> m_mapEdgesToControls;

    }
}


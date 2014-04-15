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
using MonoHotDraw.Figures;
using MonoHotDraw.Util;
using TraceLab.Core.Experiments;
using MonoHotDraw.Connectors;
using System.Collections.Generic;
using MonoHotDraw.Locators;
using MonoHotDraw.Handles;
using MonoHotDraw.Tools;
using MonoHotDraw;

namespace TraceLab.UI.GTK
{
    public class ComponentControl: BasicNodeControl
    {
        public ComponentControl(ExperimentNode node, ApplicationContext applicationContext) : base(node, applicationContext)
        {
            m_controlButtons = new NodeControlButtons (this, applicationContext);
            m_controlButtons.InfoButton.Toggled += OnInfoToggled;
        }

        /// <summary>
        /// When info handle is toggled it opens the info pad.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void OnInfoToggled(object sender, ToggleEventArgs args) 
        {
            m_applicationContext.MainWindow.ShowComponentInfoPad(this);
        }
        
        /// <summary>
        /// Gets the handles enumerator of all the icons:
        /// new connection, remove, and info
        /// </summary>
        /// <value>The handles enumerator.</value>
        public override IEnumerable<IHandle> HandlesEnumerator {
            get {
                foreach (IHandle handle in base.HandlesEnumerator) {
                    yield return handle;
                }
                
                foreach(IHandle handle in m_controlButtons.ControlButtons) {
                    yield return handle;
                }
            }
        }

        private NodeControlButtons m_controlButtons;
    }
}


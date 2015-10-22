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
using MonoHotDraw.Locators;
using System.Collections.Generic;
using MonoHotDraw.Handles;

namespace TraceLab.UI.GTK
{
    public class NodeControlButtons
    {
        static NodeControlButtons() 
        {
            s_infoIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.info.png");
            s_infoOnIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.infoOn.png");
        }

        public NodeControlButtons(BasicNodeControl ownerControl, ApplicationContext applicationContext)
        {
            //first column with icons
            m_newConnectionHandle = new NewConnectionHandle (ownerControl, applicationContext, new QuickActionLocator (15, 0, QuickActionPosition.Right));
 
            // HERZUM SPRINT 5.0: TLAB-230
            // HERZUM SPRINT 5.1: TLAB-230
            if (ownerControl is ScopeNodeControl || ownerControl is CommentNodeControl)
                m_infoHandle = new PixToggleButtonHandle (ownerControl, new QuickActionLocator (15, 0.12, QuickActionPosition.Right),
                                                          s_infoIcon, s_infoOnIcon);
            else
                m_infoHandle = new PixToggleButtonHandle (ownerControl, new QuickActionLocator (15, 0.8, QuickActionPosition.Right),
                                                          s_infoIcon, s_infoOnIcon);
            // END HERZUM SPRINT 5.1: TLAB-230
            // END HERZUM SPRINT 5.0: TLAB-230


            //second column with icons
            m_removeHandle = new RemoveNodeHandle (ownerControl, new QuickActionLocator (35, 0, QuickActionPosition.Right));
        }

        // HERZUM SPRINT 5.2: TLAB-249
        public void MoveIconInfo(BasicNodeControl ownerControl,double rel)
        {
            m_infoHandle = new PixToggleButtonHandle (ownerControl, new QuickActionLocator (15, rel, QuickActionPosition.Right),
                                                  s_infoIcon, s_infoOnIcon);
        }
        // END HERZUM SPRINT 5.2: TLAB-249

        public PixToggleButtonHandle InfoButton
        {
            get { return m_infoHandle; }
        }

        /// <summary>
        /// Gets the handles enumerator of all the icons:
        /// new connection, remove, and info
        /// </summary>
        /// <value>The handles enumerator.</value>
        public IEnumerable<IHandle> ControlButtons {
            get 
            {
                yield return m_newConnectionHandle;
                yield return m_removeHandle;
                yield return m_infoHandle;
            }
        }

        private static Gdk.Pixbuf s_infoIcon;
        private static Gdk.Pixbuf s_infoOnIcon;

        private PixToggleButtonHandle m_infoHandle;
        private NewConnectionHandle m_newConnectionHandle;
        private RemoveNodeHandle m_removeHandle;
    }
}


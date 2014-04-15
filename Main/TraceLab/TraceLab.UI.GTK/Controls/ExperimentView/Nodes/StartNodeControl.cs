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
using MonoHotDraw.Locators;
using System.Collections.Generic;
using MonoHotDraw.Handles;

namespace TraceLab.UI.GTK
{
    public class StartNodeControl : BasicNodeControl
    {
        public StartNodeControl(ExperimentNode node, ApplicationContext applicationContext) : base(node, applicationContext)
        {
            PaddingLeft = 5.0;
            PaddingTop = 5.0;
            PaddingRight = 5.0;
            PaddingBottom = 5.0;
            m_newConnectionHandle = new NewConnectionHandle (this, applicationContext, new QuickActionLocator (15, 0.5, QuickActionPosition.Right));
        }

        /// <summary>
        /// Gets the handles enumerator of all the icons:
        /// new connection, remove, and info
        /// </summary>
        /// <value>The handles enumerator.</value>
        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            {
                if(IsEditable)
                {
                    yield return m_newConnectionHandle;
                }
            }
        }

        private NewConnectionHandle m_newConnectionHandle;
    }
}


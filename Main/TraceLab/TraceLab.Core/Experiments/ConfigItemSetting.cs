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
using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Represents the config setting when defining composite components, that allows determining if the config property 
    /// should be included in composite component pane info view
    /// </summary>
    public class ConfigItemSetting : ItemSetting
    {
        public ConfigItemSetting(string alias, string type, ConfigPropertyObject propertyObject)
            : base(alias, type)
        {
            PropertyObject = propertyObject;
            Alias = alias;
        }

        private string m_alias;

        /// <summary>
        /// Gets or sets the alias used to indicate what name the item is going to be visible in the view of the composite node
        /// of this experiment
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias
        {
            get { return m_propertyObject.DisplayName; }
            set
            {
                if (m_propertyObject.DisplayName != value)
                {
                    m_propertyObject.DisplayName = value;
                    NotifyPropertyChanged("Alias");
                }
            }
        }

        public override bool Include
        {
            get { return m_propertyObject.Visible; }
            set
            {
                if (m_propertyObject.Visible != value)
                {
                    m_propertyObject.Visible = value;
                    base.Include = value;
                }
            }
        }

        private ConfigPropertyObject m_propertyObject;

        /// <summary>
        /// Gets or sets the property object. The property object that describes the configuration property
        /// </summary>
        /// <value>
        /// The property object.
        /// </value>
        public ConfigPropertyObject PropertyObject
        {
            get
            {
                return m_propertyObject;
            }
            set
            {
                if (m_propertyObject != value)
                {
                    m_propertyObject = value;
                    NotifyPropertyChanged("ConfigPropertyObject");
                }
            }
        }
    }
}

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
using System.ComponentModel;
using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Represents the setting when defining composite components, that allows determining if the io item 
    /// should be included in composite component output or input
    /// </summary>
    public class ItemSetting : INotifyPropertyChanged
    {
        public ItemSetting(string itemSettingName, string type)
        {
            m_itemSettingName = itemSettingName;
            Type = type; //via property so that it also sets friendyType
            m_include = true; //default
        }

        public ItemSetting(string itemSettingName, string type, string description)
            : this(itemSettingName, type)
        {
            Description = description;
        }

        private string m_itemSettingName;

        /// <summary>
        /// Gets the name of the item setting. This is convenience name to easily show it during benchmark item matching.
        /// </summary>
        /// <value>
        /// The name of the item setting.
        /// </value>
        public string ItemSettingName
        {
            get { return m_itemSettingName; }
        }

        private bool m_include;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ItemSetting"/> is going to be included in the view of composite node
        /// of this experiment.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Include
        {
            get { return m_include; }
            set
            {
                if (m_include != value)
                {
                    m_include = value;
                    NotifyPropertyChanged("Include");
                }
            }
        }


        private string m_type;

        /// <summary>
        /// Gets or sets the type of the item
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type
        {
            get { return m_type; }
            set
            {
                if (m_type != value)
                {
                    m_type = value;
                    m_friendlyType = TraceLab.Core.Utilities.TypesHelper.GetFriendlyName(value);
                    NotifyPropertyChanged("Type");
                    NotifyPropertyChanged("FriendlyType");
                }
            }
        }

        private string m_friendlyType;
        /// <summary>
        /// Gets or sets the user friendly type name
        /// </summary>
        /// <value>
        /// The user friendly type name
        /// </value>
        public string FriendlyType
        {
            get { return m_friendlyType; }
        }

        private string m_description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return m_description; }
            set
            {
                if (m_description != value)
                {
                    m_description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private bool m_isHighlighted = false;

        public bool IsHighlighted
        {
            get { return m_isHighlighted; }
            private set
            {
                if (m_isHighlighted != value)
                {
                    m_isHighlighted = value;
                    NotifyPropertyChanged("IsHighlighted");
                }
            }
        }

        private ItemSetting m_pairSetting;

        /// <summary>
        /// Gets or sets the setting that is in pair with current setting. 
        /// It represnts matched setting of the same name from either inputs and outputs. 
        /// </summary>
        /// <value>
        /// The pair setting.
        /// </value>
        public ItemSetting PairSetting
        {
            get { return m_pairSetting; }
            set
            {
                if (m_pairSetting != value)
                {
                    m_pairSetting = value;
                    NotifyPropertyChanged("PairSetting");
                }
            }
        }

        /// <summary>
        /// List of corresponding nodes that have either input or output of the item setting name
        /// </summary>
        internal List<IConfigurableAndIOSpecifiable> CorrespondingNodes
        {
            get;
            set;
        }

        public void Highlight()
        {
            IsHighlighted = true;

            if (PairSetting != null)
            {
                PairSetting.IsHighlighted = true;
            }

            if (CorrespondingNodes != null)
            {
                foreach (IConfigurableAndIOSpecifiable metadata in CorrespondingNodes)
                {
                    metadata.IOSpec.HighlightIO(ItemSettingName);
                }
            }
        }

        public void ClearHighlight()
        {
            IsHighlighted = false;

            if (PairSetting != null)
            {
                PairSetting.IsHighlighted = false;
            }

            if (CorrespondingNodes != null)
            {
                foreach (IConfigurableAndIOSpecifiable metadata in CorrespondingNodes)
                {
                    metadata.IOSpec.ClearHightlightIO();
                }
            }
        }

        #region INotifyPropertyChanged

        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
            }
            remove
            {
                m_propertyChanged -= value;
            }
        }
        protected void NotifyPropertyChanged(string prop)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}

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

namespace TraceLab.UI.WPF.ViewModels
{
    abstract class PackageContentItem : INotifyPropertyChanged
    {
        private string m_label;
        public virtual string Label
        {
            get { return m_label; }
            protected set
            {
                if (m_label != value)
                {
                    m_label = value;
                    NotifyPropertyChanged("Label");
                }
            }
        }

        private object m_icon;
        public virtual object Icon
        {
            get { return m_icon; }
            protected set
            {
                if (m_icon != value)
                {
                    m_icon = value;
                    NotifyPropertyChanged("Icon");
                }
            }
        }

        private PackageContentItem m_parent;
        public PackageContentItem Parent
        {
            get { return m_parent; }
            protected set
            {
                if (m_parent != value)
                {
                    m_parent = value;
                    NotifyPropertyChanged("Parent");
                }
            }
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                if (m_isExpanded != value)
                {
                    m_isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        private bool m_isSelected;
        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}

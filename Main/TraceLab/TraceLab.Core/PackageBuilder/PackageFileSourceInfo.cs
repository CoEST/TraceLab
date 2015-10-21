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
using System.ComponentModel;

namespace TraceLab.Core.PackageBuilder
{
    public class PackageFileSourceInfo : INotifyPropertyChanged
    {
        public PackageFileSourceInfo(string source)
        {
            SourceFilePath = source;
        }

        bool m_hasTypes;
        public bool HasTypes
        {
            get { return m_hasTypes; }
            set
            {
                m_hasTypes = value;
                RaisePropertyChanged("HasTypes");
            }
        }

        bool m_hasComponents;
        public bool HasComponents
        {
            get { return m_hasComponents; }
            set
            {
                m_hasComponents = value;
                RaisePropertyChanged("HasComponents");
            }
        }

        bool m_isSelected;
        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                m_isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; RaisePropertyChanged("Name"); }
        }

        PackageHeirarchyItem m_parent;
        public PackageHeirarchyItem Parent
        {
            get { return m_parent; }
            set { m_parent = value; RaisePropertyChanged("Parent"); }
        }

        public string SourceFilePath
        {
            get;
            private set;
        }

        void RemoveFromParent()
        {
            if (Parent != null)
            {
                Parent.Children.Remove(this);
            }
        }

        public string GetPath()
        {
            string path = Name;
            PackageHeirarchyItem parent = Parent;
            while (parent != null)
            {
                // Don't count the root
                if (parent.Parent != null)
                {
                    path = System.IO.Path.Combine(parent.Name, path);
                }
                parent = parent.Parent;
            }

            path = System.IO.Path.Combine(".", path);

            return path;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}

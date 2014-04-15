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
using System.Xml.Serialization;
using System.ComponentModel;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Settings
{
    /// <summary>
    /// Represents a path for use in the Settings file
    /// </summary>
    [XmlType("SettingsPath")]
    public sealed class SettingsPath : INotifyPropertyChanged, IModifiable
    {
        public SettingsPath()
        {
        }

        public SettingsPath(bool isTemporary, string path)
        {
            IsTemporary = isTemporary;
            Path = path;
            ResetModifiedFlag();
        }

        bool m_isTemp;
        [XmlIgnore]
        public bool IsTemporary
        {
            get { return m_isTemp; }
            set
            {
                if (m_isTemp != value)
                {
                    m_isTemp = value;
                    NotifyPropertyChanged("IsTemporary");
                }
            }
        }

        private string m_path = string.Empty;
        [XmlText]
        public string Path
        {
            get { return m_path; }
            set
            {
                if (m_path != value)
                {
                    m_path = value ?? string.Empty;
                    NotifyPropertyChanged("Path");
                    IsModified = true;
                }
            }
        }

        public static explicit operator string(SettingsPath path)
        {
            if (path == null)
                return null;

            return path.Path;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        #region IModifiable Members

        bool m_isModified;
        [XmlIgnore]
        public bool IsModified
        {
            get { return m_isModified; }
            set
            {
                if (m_isModified != value)
                {
                    m_isModified = value;
                    NotifyPropertyChanged("IsModified");
                }
            }
        }

        public void ResetModifiedFlag()
        {
            IsModified = false;
        }

        public override bool Equals (Object other)
        {
            SettingsPath path = other as SettingsPath;

            if(path != null)
                return this.Path.Equals(path.Path);

            return false;
        }

        #endregion
    }
}

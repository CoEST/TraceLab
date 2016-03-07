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
using System.Xml.Serialization;

namespace TraceLab.Core.Settings
{
    /// <summary>
    /// Model class of the global log logLevel settings. 
    /// </summary>
    public class GlobalLogLevelSetting : INotifyPropertyChanged, IXmlSerializable
    {
        private bool m_isLocked;
        private bool m_isEnabled;
        private NLog.LogLevel m_level;

        private GlobalLogLevelSetting()
        {
        }

        public GlobalLogLevelSetting(NLog.LogLevel level)
            : this(level, false)
        {
        }

        public GlobalLogLevelSetting(NLog.LogLevel level, bool isEnabled)
            : this(level, isEnabled, false)
        {
        }

        public GlobalLogLevelSetting(NLog.LogLevel level, bool isEnabled, bool isLocked)
        {
            m_level = level;
            IsEnabled = isEnabled;
            IsLocked = isLocked;
        }

        [XmlAttribute()]
        public NLog.LogLevel Level
        {
            get { return m_level; }
            private set { m_level = value; }
        }

        [XmlAttribute]
        public bool IsEnabled
        {
            get { return m_isEnabled; }
            set
            {
                if (!IsLocked && m_isEnabled != value)
                {
                    m_isEnabled = value;
                    OnPropertyChanged("IsEnabled");

                    if (m_isEnabled == true)
                    {
                        EnableLogging();
                    }
                    else
                    {
                        DisableLogging();
                    }
                }
            }
        }

        [XmlAttribute]
        public bool IsLocked
        {
            get { return m_isLocked; }
            set
            {
                if (m_isLocked != value)
                {
                    m_isLocked = value;
                    OnPropertyChanged("IsLocked");
                }
            }

        }

        private void DisableLogging()
        {

        }

        private void EnableLogging()
        {

        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            m_level = NLog.LogLevel.FromString(reader.GetAttribute("Level"));
            IsEnabled = bool.Parse(reader.GetAttribute("IsEnabled"));
            reader.Read();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Level", Level.ToString());
            writer.WriteAttributeString("IsEnabled", IsEnabled.ToString());
        }

        #endregion
    }
}

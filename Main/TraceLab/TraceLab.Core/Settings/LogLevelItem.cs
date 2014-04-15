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

using System.ComponentModel;
using System;

namespace TraceLab.Core.Settings
{
    public class LogLevelItem : INotifyPropertyChanged
    {
        private bool m_isLocked;
        private bool m_isEnabled;
        private readonly NLog.LogLevel m_level;
        //private string m_loggerName;
        //private NLog.Config.LoggingRule m_rule = null;

        internal LogLevelItem(NLog.LogLevel level) 
            : this(level, false)
        {
        }

        internal LogLevelItem(NLog.LogLevel level, bool isEnabled) 
            : this(level, isEnabled, false)
        {
        }

        internal LogLevelItem(NLog.LogLevel level, bool isEnabled, bool isLocked)
        {
            m_level = level;
            IsEnabled = isEnabled;
            IsLocked = isLocked;
        }
        
        public NLog.LogLevel Level
        {
            get { return m_level; }
        }

        public bool IsEnabled
        {
            get { return m_isEnabled; }
            set
            {
                if (!IsLocked && m_isEnabled != value)
                {
                    m_isEnabled = value;
                    OnPropertyChanged("IsEnabled");

                    //fire event
                    if (LogLevelEnableChanged != null)
                    {
                        LogLevelEnableChanged(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when log level is enabled or disabled.
        /// </summary>
        public event System.EventHandler LogLevelEnableChanged;

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

        private void OnPropertyChanged(string property)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(property));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, e);
        }

        #endregion
    }
}

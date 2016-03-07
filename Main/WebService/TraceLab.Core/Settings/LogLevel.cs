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

namespace TraceLab.Core.Settings
{
    [Serializable]
    public sealed class LogLevel
    {
        private string m_level;

        private LogLevel()
        {
        }

        public LogLevel(NLog.LogLevel level)
        {
            m_level = level.ToString();
        }

        static public implicit operator LogLevel(NLog.LogLevel level)
        {
            LogLevel logLevel = new LogLevel(level);
            return logLevel;
        }

        static public implicit operator NLog.LogLevel(LogLevel level)
        {
            return NLog.LogLevel.FromString(level.Level);
        }

        [XmlAttribute]
        public string Level
        {
            get
            {
                return m_level;
            }
            set
            {
                m_level = value;
            }
        }

        public override bool Equals(object obj)
        {
            LogLevel level = obj as LogLevel;
            if (level != null)
            {
                if (string.Equals (level.Level, Level, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            NLog.LogLevel lev = this;
            return lev.GetHashCode();
        }
    }
}

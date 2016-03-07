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

namespace TraceLab.Core.Components
{
    [Serializable]
    public class EnumValue
    {
        private readonly int m_hashCode = (new Random()).Next(int.MinValue, int.MaxValue);

        public string Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            else if (obj is string)
            {
                return Equals((string) obj);
            }
            else if(obj is EnumValue)
            {
                return Equals((EnumValue) obj);
            }

            return false;
        }

        public bool Equals(EnumValue other)
        {
            if (other != null && Value != null)
            {
                return Value.Equals(other.Value);
            }
            else if (other != null && Value == null && other.Value == null)
            {
                return true;
            }

            return false;
        }

        public bool Equals(string obj)
        {
            return string.Equals(Value, obj);
        }

        public override int GetHashCode()
        {
            return m_hashCode;
        }
    }
}
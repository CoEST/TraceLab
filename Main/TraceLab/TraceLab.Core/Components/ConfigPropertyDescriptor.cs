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

namespace TraceLab.Core.Components
{
    [Serializable]
    public class ConfigPropertyDescriptor : PropertyDescriptor
    {
        public ConfigPropertyObject ConfigProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// base could potential hold custom attributes for the property
        /// currently passing null
        /// </summary>
        /// <param name="property"></param>
        public ConfigPropertyDescriptor(ConfigPropertyObject property, Attribute[] attrs)
            : base(property.Name, attrs)
        {
            ConfigProperty = property;
        }

        public override string DisplayName
        {
            get
            {
                return ConfigProperty.DisplayName;
            }
        }

        public override string Description
        {
            get
            {
                return ConfigProperty.Description;
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(ConfigWrapper);
            }
        }

        public override object GetValue(object component)
        {
            return ((ConfigWrapper)component).ConfigValues[ConfigProperty.Name].Value;
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                return new ConfigPropertyConverter(ConfigProperty);
            }
        }

        public override Type PropertyType
        {
            get
            {
                if (ConfigProperty.IsEnum)
                {
                    return typeof(System.Enum);
                }

                return System.Type.GetType(ConfigProperty.AssemblyQualifiedName);
            }
        }

        public override void ResetValue(object component)
        {
            if (PropertyType.Equals(typeof(string)))
            {
                ((ConfigWrapper)component).ConfigValues[ConfigProperty.Name].Value = string.Empty;
            }
            else
            {
                ((ConfigWrapper)component).ConfigValues[ConfigProperty.Name].Value = null;
            }
        }

        public override void SetValue(object component, object value)
        {
            if (!object.Equals(((ConfigWrapper)component).ConfigValues[ConfigProperty.Name].Value, value))
            {
                ((ConfigWrapper)component).ConfigValues[ConfigProperty.Name].Value = value;
                ((ConfigWrapper)component).OnPropertyChanged(ConfigProperty.Name);
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}

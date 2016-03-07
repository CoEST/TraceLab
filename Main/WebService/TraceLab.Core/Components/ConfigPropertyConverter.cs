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
using System.Globalization;

namespace TraceLab.Core.Components
{
    class ConfigPropertyConverter : TypeConverter
    {
        ConfigPropertyObject m_instance;
        public ConfigPropertyConverter(ConfigPropertyObject instance)
        {
            m_instance = instance;
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return base.IsValid(context, value);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (m_instance != null)
            {
                return m_instance.IsEnum;
            }

            return base.GetStandardValuesSupported(context);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            if (m_instance != null)
            {
                return m_instance.IsEnum;
            }

            return base.GetStandardValuesSupported(context);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (m_instance != null)
            {
                return new StandardValuesCollection(m_instance.EnumInfo.PossibleValues);
            }

            return base.GetStandardValues(context);
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (m_instance != null)
            {
                if (m_instance.IsEnum)
                {
                    var enumConverter = TypeDescriptor.GetConverter(m_instance.EnumInfo);
                    return enumConverter.CanConvertFrom(context, sourceType);
                }
                else
                {
                    Type dataType = Type.GetType(m_instance.AssemblyQualifiedName);
                    var converter = TypeDescriptor.GetConverter(dataType);
                    if (converter != null)
                    {
                        return converter.CanConvertFrom(context, sourceType);
                    }
                }
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (m_instance != null)
            {
                if (m_instance.IsEnum)
                {
                    var enumConverter = TypeDescriptor.GetConverter(m_instance.EnumInfo);
                    return enumConverter.ConvertFrom(context, culture, value);
                }
                else
                {
                    Type dataType = Type.GetType(m_instance.AssemblyQualifiedName);
                    var converter = TypeDescriptor.GetConverter(dataType);
                    if (converter != null)
                    {
                        return converter.ConvertFrom(value);
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
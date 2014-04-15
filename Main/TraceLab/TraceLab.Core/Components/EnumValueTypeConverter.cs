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
using System.Security.Permissions;

namespace TraceLab.Core.Components
{
    public class EnumValueTypeConverter : TypeConverter
    {
        public EnumValueTypeConverter()
        {
        }

        private EnumValueCollection m_instance;
        public EnumValueTypeConverter(EnumValueCollection instance)
        {
            if (instance == null)
                throw new ArgumentNullException();
            m_instance = instance;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            if (m_instance != null)
            {
                if (sourceType == typeof(EnumValue))
                    return true;
                Type sourceEnum = Type.GetType(m_instance.SourceEnum);
                if (sourceEnum != null && sourceEnum == sourceType)
                    return true;
            }

            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (m_instance == null)
            {
                throw new ArgumentException("Context's instance must be value.");
            }

            EnumValueCollection copy = new EnumValueCollection(m_instance);

            Type sourceEnum = Type.GetType(copy.SourceEnum);
            if (sourceEnum != null && sourceEnum == value.GetType())
            {
                value = value.ToString();
            }

            if (value != null && value.GetType() == typeof(string))
            {
                foreach (EnumValue possible in copy.PossibleValues)
                {
                    if (possible.Value.Equals(value as string, StringComparison.CurrentCultureIgnoreCase))
                    {
                        copy.CurrentValue = possible;
                        break;
                    }
                }
            }
            else if (value != null && value.GetType() == typeof(EnumValue))
            {
                EnumValue val = value as EnumValue;
                foreach (EnumValue possible in copy.PossibleValues)
                {
                    if (possible.Value.Equals(val.Value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        copy.CurrentValue = possible;
                        break;
                    }
                }
            }

            TypeDescriptor.AddProvider(new EnumValueDescriptionProvider(copy), copy);

            return copy;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            if (m_instance != null)
            {
                Type sourceEnum = Type.GetType(m_instance.SourceEnum);
                if (sourceEnum != null && sourceEnum == destinationType)
                    return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            EnumValueCollection instance = value as EnumValueCollection;
            if (m_instance != null)
            {
                instance = m_instance;
            }

            Type sourceEnum = Type.GetType(instance.SourceEnum);
            if (sourceEnum != null && sourceEnum == destinationType)
            {
                return Enum.Parse(destinationType, instance.CurrentValue.Value);
            }
            else if (destinationType == typeof(string))
            {
                return instance.CurrentValue;
            }

            return null;
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be called to find a common set of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>
        /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exhaustive list of possible values; false if other values are possible.
        /// </returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
        /// </returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (m_instance == null)
            {
                return new StandardValuesCollection(new string[]{});
            }
            return new StandardValuesCollection(m_instance.PossibleValues);
        }
    }
}
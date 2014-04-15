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
    public class EnumValueDescriptionProvider : TypeDescriptionProvider
    {
        private EnumValueCollection m_instance;
        private EnumValueDescriptor m_instanceDescriptor;
        public EnumValueDescriptionProvider(EnumValueCollection instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            m_instance = instance;
            m_instanceDescriptor = new EnumValueDescriptor(m_instance);
        }

        public EnumValueDescriptionProvider()
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
        /// <param name="instance">An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"/>.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"/> that can provide metadata for the type.
        /// </returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (objectType == typeof(EnumValueCollection))
            {
                if (instance != null && m_instance != instance)
                {
                    return new EnumValueDescriptor((EnumValueCollection)instance);
                }

                return m_instanceDescriptor;
            }

            return base.GetTypeDescriptor(objectType, instance);
        }
    }
}
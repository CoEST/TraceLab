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

namespace TraceLab.Core.Serialization
{
    /// <summary>
    /// A private helper class for use within the XmlSerializerFactory
    /// </summary>
    class SerializerKey : IEquatable<SerializerKey>
    {
        readonly private HashSet<Type> m_support;
        readonly private Type m_main;

        internal SerializerKey(Type main, IEnumerable<Type> support)
        {
            m_main = main;
            if(support == null)
                support = Type.EmptyTypes;
            m_support = new HashSet<Type>(support);
        }

        private HashSet<Type> Support { get { return m_support; } }
        private Type Main { get { return m_main; } }

        public override bool Equals(object obj)
        {
            return Equals(obj as SerializerKey);
        }

        public bool Equals(SerializerKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool equal = true;

            equal &= (Main == other.Main);

            if (Support.Count == other.Support.Count)
            {
                var duplicate = new HashSet<Type>(Support);
                duplicate.SymmetricExceptWith(other.Support);
                equal &= (duplicate.Count == 0);
            }
            else
            {
                equal = false;
            }

            return equal;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int code = 0;
                if(m_support != null)
                {
                    code = Support.Aggregate(code, (current, type) => current ^ type.GetHashCode()*397);
                }
                code ^= (m_main != null ? m_main.GetHashCode() : 0);
                return code;
            }
        }

        public static bool operator ==(SerializerKey left, SerializerKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SerializerKey left, SerializerKey right)
        {
            return !Equals(left, right);
        }
    }

    /// <summary>
    /// A Factory/cache for XmlSerializers
    /// 
    /// Due to the limitations of the XmlSerializer constructor, when using the extended constructor (with supporting types)
    /// a new assembly is ALWAYS generated.  If you're doing a lot of serialization, this will get expensive very quickly.
    /// 
    /// The solution that we've chosen for now is to cache the XmlSerializers based on the types that are being deserialized.
    /// </summary>
    public static class XmlSerializerFactory
    {
        private static readonly Dictionary<SerializerKey, XmlSerializer> m_serializers = new Dictionary<SerializerKey, XmlSerializer>();

        public static XmlSerializer GetSerializer(Type mainType, Type[] supportingTypes)
        {
            XmlSerializer serial = null;
            lock (m_serializers)
            {
                var key = new SerializerKey(mainType, supportingTypes);
                if (!m_serializers.TryGetValue(key, out serial))
                {
                    // Use different calls for this, because calling it with the type array will
                    // force a serialization assembly to be generated, even if one was precompiled.
                    if (supportingTypes == null || supportingTypes.Length == 0)
                    {
                        serial = new XmlSerializer(mainType);
                    }
                    else
                    {
                        serial = new XmlSerializer(mainType, supportingTypes);
                    }

                    m_serializers.Add(key, serial);
                }
            }


            return serial;
        }

        internal static void Clear()
        {
            lock (m_serializers)
            {
                m_serializers.Clear();
            }
        }
    }
}

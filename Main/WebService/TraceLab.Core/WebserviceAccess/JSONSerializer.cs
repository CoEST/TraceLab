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
using System.IO;
using System.Runtime.Serialization.Json;

namespace TraceLab.Core.WebserviceAccess
{
    /// <summary>
    /// Helper class for serializing and deserializing json to/from objects.
    /// </summary>
    public class JsonSerializer
    {
        /// <summary>
        /// Serializes the specified object to json format.
        /// </summary>
        /// <typeparam name="T">The type of the instances that are serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            return Serialize<T>(obj, null);
        }

        /// <summary>
        /// Serializes the specified object to json format.
        /// </summary>
        /// <typeparam name="T">The type of the instances that are serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="knownTypes">
        ///    An System.Collections.Generic.IEnumerable<T> of System.Type that contains
        ///    the types that may be present in the object graph.
        /// </param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, IEnumerable<Type> knownTypes)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType(), knownTypes);
            MemoryStream memorystream = new MemoryStream();
            jsonSerializer.WriteObject(memorystream, obj);
            string jsonObject = Encoding.Default.GetString(memorystream.ToArray());
            memorystream.Dispose();
            return jsonObject;
        }

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <typeparam name="T">The type of the instances that are deserialized.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns>The deserialized object</returns>
        public static T Deserialize<T>(string json)
        {
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(Messages.ResponseEmpty);
            }

            MemoryStream memorystream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            T obj = (T)serializer.ReadObject(memorystream);
            memorystream.Close();
            memorystream.Dispose();
            return obj;
        }
    }
}

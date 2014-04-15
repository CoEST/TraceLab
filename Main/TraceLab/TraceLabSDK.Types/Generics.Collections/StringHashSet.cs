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
using System.Runtime.Serialization;

namespace TraceLabSDK.Types.Generics.Collections
{
    /// <summary>
    /// Represents hash set of strings
    /// </summary>
    [Serializable]
    public class StringHashSet : HashSet<string>
    {
        private static int version = 1;

        #region Methods

        /// <summary>
        /// Initializes a new instance of the StringHashSet class that is empty
        /// </summary>
        public StringHashSet() : base() { }

        /// <summary>
        /// Initializes a new instance of the StringHashSet class
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new set.</param>
        public StringHashSet(IEnumerable<string> collection) : base(collection) { }

        /// <summary>
        /// Initializes a new instance of the StringHashSet class with serialized data.
        /// Supports serialization
        /// </summary>
        /// <param name="info">A SerializationInfo object that contains the information required to serialize the StringHashSet object.</param>
        /// <param name="context">A StreamingContext structure that contains the source and destination of the serialized stream associated with the StringHashSet object.</param>
        protected StringHashSet(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            var dataVersion = reader.ReadInt32();
            if (dataVersion != StringHashSet.version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                int dictionaryCount = reader.ReadInt32();
                for (int i = 0; i < dictionaryCount; ++i)
                {
                    string s = reader.ReadString();
                    this.Add(s);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(StringHashSet.version);

            writer.Write(this.Count);
            foreach (string s in this)
            {
                writer.Write(s);
            }
        }

        #endregion
    }
}

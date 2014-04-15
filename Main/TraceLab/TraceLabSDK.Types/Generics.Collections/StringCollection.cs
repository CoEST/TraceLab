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
using System.Xml.Serialization;

namespace TraceLabSDK.Types.Generics.Collections
{
    /// <summary>
    /// Represents the list of strings
    /// </summary>
    [Serializable]
    public class StringList : List<string>
    {
    }

    /// <summary>
    /// A dictionary of strings to doubles
    /// </summary>
    [Serializable]
    public class StringDoubleDictionary : SerializableDictionary<string, double>
    {
        private static int version = 1;

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="StringDoubleDictionary"/> class.
        /// </summary>
        public StringDoubleDictionary() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringDoubleDictionary"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public StringDoubleDictionary(Dictionary<string, double> other) : base(other) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringDoubleDictionary"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected StringDoubleDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            var dataVersion = reader.ReadInt32();
            if (dataVersion != StringDoubleDictionary.version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                int dictionaryCount = reader.ReadInt32();
                for (int i = 0; i < dictionaryCount; ++i)
                {
                    string key = reader.ReadString();
                    double value = reader.ReadDouble();
                    this.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(StringDoubleDictionary.version);

            writer.Write(this.Count);
            foreach (KeyValuePair<string, double> pair in this)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }

        #endregion
    }
}

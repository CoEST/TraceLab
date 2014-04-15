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

namespace TraceLabSDK.Types
{
    /// <summary>
    /// Represents container for the metrics. 
    /// Currently it is simply a list of metrics.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLKeyValuePairsList : List<KeyValuePair<string, double>>, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_nameOfCollection;
        /// <summary>
        /// Gets or sets the name of collection.
        /// </summary>
        /// <value>
        /// The name of collection.
        /// </value>
        public string NameOfCollection
        {
            get { return this.m_nameOfCollection; }
            set { this.m_nameOfCollection = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLKeyValuePairsList()
            : base()
        {
            this.m_nameOfCollection = String.Empty;        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TLKeyValuePairsList"/> class with the given name of the collection
        /// </summary>
        /// <param name="nameOfCollection">The name of collection.</param>
        public TLKeyValuePairsList(string nameOfCollection)
            : base()
        {
            this.m_nameOfCollection = nameOfCollection;
        }

        /// <summary>
        /// Adds the specified key with value
        /// Needed for java usability.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, double value)
        {
            Add(new KeyValuePair<string, double>(key, value));
        }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            var dataVersion = reader.ReadInt32();
            if (dataVersion != TLKeyValuePairsList.version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                this.m_nameOfCollection = reader.ReadString();

                int listCount = reader.ReadInt32();
                for (int i = 0; i < listCount; ++i)
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
            writer.Write(TLKeyValuePairsList.version);

            writer.Write(this.m_nameOfCollection);

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

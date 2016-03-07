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
using System.Collections.ObjectModel;

namespace TraceLabSDK.Types.Contests
{
    /// <summary>
    /// List of datasets
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLDatasetsList : KeyedCollection<string, TLDataset>, IRawSerializable
    {
        private static int version = 1;

        #region Methods

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        protected override string GetKeyForItem(TLDataset item)
        {
            return item.Name;
        }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            int dataversion = reader.ReadInt32();
            if (dataversion != TLDatasetsList.version)
            {
                throw new InvalidOperationException("Binary reader did not read correct data version. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                int collectionCount = reader.ReadInt32();
                for (int i = 0; i < collectionCount; i++)
                {
                    TLDataset dataset = new TLDataset();
                    dataset.ReadData(reader);
                    this.Add(dataset);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLDatasetsList.version);

            writer.Write(this.Count);
            foreach (TLDataset dataset in this)
            {
                dataset.WriteData(writer);
            }
        }

        #endregion
    }
}

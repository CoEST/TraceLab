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
using TraceLabSDK.Types.Generics.Collections;

namespace TraceLabSDK.Types
{
    /// <summary>
    /// Helper class for Java - nice name for a list of TLSingleLinks
    /// </summary>
    [Serializable]
    public class TLLinksList : List<TLSingleLink>, IRawSerializable
    {
        private static int version = 1;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TLSingleLink s in this)
            {
                sb.Append(s.ToString() + ", ");
            }
            return sb.ToString();
        }

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            int dataversion = reader.ReadInt32();
            if (dataversion != TLLinksList.version)
            {
                throw new InvalidOperationException("Binary reader did not read correct data version. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                int listCount = reader.ReadInt32();
                for (int i = 0; i < listCount; i++)
                {
                    TLSingleLink node = new TLSingleLink();
                    node.ReadData(reader);
                    this.Add(node);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLLinksList.version);

            writer.Write(this.Count);
            foreach (TLSingleLink node in this)
            {
                node.WriteData(writer);
            }
        }

        #endregion
    }
}

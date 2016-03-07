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
using System.Xml.Serialization;

namespace TraceLabSDK.Types.Contests
{
    /// <summary>
    /// The collection of Similarity Matrices keyed by their name.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLSimilarityMatricesCollection : KeyedCollection<string, TLSimilarityMatrix>, IXmlSerializable
    {
        private static int version = 1;

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="TLSimilarityMatricesCollection"/> class.
        /// </summary>
        public TLSimilarityMatricesCollection() { }

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        protected override string GetKeyForItem(TLSimilarityMatrix item)
        {
            return item.Name;
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                TLSimilarityMatrix matrix = new TLSimilarityMatrix();
                matrix.ReadXml(reader);

                Add(matrix);
                reader.Read();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (TLSimilarityMatrix matrix in Items)
            {
                writer.WriteStartElement("TLSimilarityMatrix");
                matrix.WriteXml(writer);
                writer.WriteEndElement();
            }
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
            if (dataversion != TLSimilarityMatricesCollection.version)
            {
                throw new InvalidOperationException("Binary reader did not read correct data version. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                int collectionCount = reader.ReadInt32();
                for (int i = 0; i < collectionCount; i++)
                {
                    TLSimilarityMatrix matrix = new TLSimilarityMatrix();
                    matrix.ReadData(reader);
                    this.Add(matrix);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLSimilarityMatricesCollection.version);

            writer.Write(this.Count);
            foreach (TLSimilarityMatrix matrix in this)
            {
                matrix.WriteData(writer);
            }
        }

        #endregion
    }
}

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
using System.Security.Permissions;

namespace TraceLabSDK.Types
{
    /// <summary>
    /// Represents the set of stopwords.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLStopwords : HashSet<string>, IXmlSerializable, IRawSerializable
    {
        private static int version = 1;

        /// <summary>
        /// Initializes a new instance of the TLStopwords class that is empty
        /// </summary>
        public TLStopwords() : base() { }

        #region Serialization

        /// <summary>
        /// Initializes a new instance of the TLStopwords class with serialized data.
        /// Supports serialization
        /// </summary>
        /// <param name="info">A SerializationInfo object that contains the information required to serialize the TLStopwords object.</param>
        /// <param name="context">A StreamingContext structure that contains the source and destination of the serialized stream associated with the TLStopwords object.</param>
        protected TLStopwords(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable"/> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"/> instance.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"/> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"/> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"/> instance.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="info"/> is null.
        ///   </exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable"/> interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2"/> instance is invalid.
        ///   </exception>
        public override void OnDeserialization(Object sender)
        {
            base.OnDeserialization(sender);
        }
        #endregion

        #region IXmlSerializable

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, this method should and does return null.
        /// See IXmlSerializable.GetSchema Method documentation for more details.
        /// </summary>
        /// <returns>null</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream from which the object is deserialized. </param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;

            if (wasEmpty)
                return;

            version = int.Parse(reader.GetAttribute("Version"));

            reader.Read();
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                string stopword = reader.ReadElementContentAsString("Stopword", String.Empty);
                Add(stopword);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Version", version.ToString());

            foreach (string stopword in this)
            {
                writer.WriteElementString("Stopword", stopword);
            }
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");

            var strings = this.ToArray();
            for (int i = 0; i < strings.Length; ++i)
            {
                builder.Append(strings[i]);
                if (i < strings.Length - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append("]");

            return builder.ToString();
        }

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. Implementation of IRawSerializable (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            int dataversion = reader.ReadInt32();
            if (dataversion != version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; ++i)
                {
                    string stopword = reader.ReadString();
                    Add(stopword);
                }
            }
        }

        /// <summary>
        /// Writes the data. Implementation of IRawSerializable (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(Count);
            foreach (string stopword in this)
            {
                writer.Write(stopword);
            }
        }

        #endregion
    }
}

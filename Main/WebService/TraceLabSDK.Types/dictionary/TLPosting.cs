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
using System.Collections;
using System.Xml.Serialization;

namespace TraceLabSDK.Types
{
    /// <summary>
    /// Represents the posting in the dictionary
    /// </summary>
    [Serializable]
    public class TLPosting : IXmlSerializable, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_artifactId;
        /// <summary>
        /// Gets or sets Artifact Id for the current posing, in which term has been found in
        /// </summary>
        public string ArtifactId
        {
            get
            {
                return m_artifactId;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("ArtifactId cannot be null");
                m_artifactId = value;
            }
        }

        private int m_frequency;
        /// <summary>
        /// Gets or Sets the frequency of the term in the current artifact of the posting
        /// </summary>
        public int Frequency
        {
            get
            {
                return m_frequency;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Frequency has to be greater than 0.");

                m_frequency = value;
            }
        }

        private double m_weight;
        /// <summary>
        /// Gets or sets the score weight for the term in the currentl artifact of the posting
        /// </summary>
        public double Weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }

        /// <summary>
        /// Compares two specified TLTermEntry objects and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first TLTermEntry to compare. </param>
        /// <param name="y">The second TLTermEntry to compare. </param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public int Compare(TLPosting x, TLPosting y)
        {
            return ((new CaseInsensitiveComparer()).Compare(x.ArtifactId, y.ArtifactId));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLPosting() { }

        /// <summary>
        /// Creates new posting
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <param name="frequency">frequency of the term in the given artifact</param>
        /// <param name="weight">local weight of the term in the posting</param>
        public TLPosting(string artifactId, int frequency, double weight)
        {
            if (artifactId == null || artifactId == "")
            {
                throw new ArgumentException("The artifactId cannot be empty or null");
            }
            if (frequency <= 0)
            {
                throw new ArgumentException("The frequency cannot be equal or less than 0");
            }
            ArtifactId = artifactId;
            Frequency = frequency;
            Weight = weight;
        }

        #endregion

        #region IXmlSerializable Members

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
            version = int.Parse(reader.GetAttribute("Version"));
            ArtifactId = reader.GetAttribute("ArtifactId");
            Frequency = int.Parse(reader.GetAttribute("Frequency"));
            Weight = double.Parse(reader.GetAttribute("Weight"));
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("TLPosting");
            writer.WriteAttributeString("Version", version.ToString());
            writer.WriteAttributeString("ArtifactId", ArtifactId);
            writer.WriteAttributeString("Frequency", Frequency.ToString());
            writer.WriteAttributeString("Weight", Weight.ToString());
            writer.WriteEndElement();
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
            if (dataversion != version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                m_artifactId = reader.ReadString();
                m_frequency = reader.ReadInt32();
                m_weight = reader.ReadDouble();
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(m_artifactId);
            writer.Write(m_frequency);
            writer.Write(m_weight);
        }

        #endregion
    }
}

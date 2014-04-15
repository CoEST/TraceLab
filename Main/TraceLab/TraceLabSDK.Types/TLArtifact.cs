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
using System.Text;
using System.Xml.Serialization;

namespace TraceLabSDK.Types
{
    /// <summary>
    /// TraceLab use the “artifact” as a generic way to refer to any information resource item within a software project.
    /// TLArtifact represents textual representation of any kind of information resources produced in software development process, 
    /// including but not limited to requirements, design specifications, UML diagrams and defect logs, test cases, or software code.
    /// 
    /// TLArtifact consists of two properties ID and Text:
    /// ID is a unique id of the artifact.
    /// Text is a textual representation of the software project resource - for example, the text of requirement, fragment of software code, etc.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLArtifact : IXmlSerializable, IRawSerializable
    {
        private static int version = 1;

        /// <summary>
        /// Default constructor
        /// </summary>
        internal TLArtifact() { }

        /// <summary>
        /// Constructor that takes id of the artifact and text of artifact.
        /// </summary>
        /// <param name="id">id of the artifact</param>
        /// <param name="text">text, ie. requirement text, regulatory code text, user scenario text, artifact code, design class description, etc</param>
        public TLArtifact(string id, string text)
        {
            this.Id = id;
            this.Text = text;
        }

        private string m_id;
        /// <summary>
        /// ID is a unique id of the artifact.
        /// </summary>
        public string Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private string m_text;
        /// <summary>
        /// Text is a textual representation of the software project resource - for example, the text of requirement, fragment of software code, etc.
        /// </summary>
        public string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }

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
            version = int.Parse(reader.GetAttribute("Version"));
            reader.Read();
            Id = reader.ReadElementContentAsString("Id", String.Empty);
            Text = reader.ReadElementContentAsString("Text", String.Empty);
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("TLArtifact");
            writer.WriteAttributeString("Version", version.ToString());
            writer.WriteElementString("Id", Id);
            writer.WriteElementString("Text", Text);
            writer.WriteEndElement();
        }

        #endregion

        #region Equals_HashCode

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the Object.</returns>
        public override int GetHashCode()
        {
            int idhash = Id.GetHashCode();
            int textHash = Text.GetHashCode();

            int hash = idhash ^ textHash;

            return hash;
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="obj">the other object</param>
        /// <returns>true if objects are equal</returns>
        public override bool Equals(object obj)
        {
            TLArtifact other = obj as TLArtifact;

            if (other != null)
            {
                return Id.Equals(other.Id) && Text.Equals(other.Text);
            }
            return false;
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
            if (dataVersion != version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                Id = reader.ReadString();
                Text = reader.ReadString();
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(Id);
            writer.Write(Text);
        }

        #endregion
    }
}

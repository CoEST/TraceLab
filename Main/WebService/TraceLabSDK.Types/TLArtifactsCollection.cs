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
    /// TLArtifactCollection is the collection of TLArtifacts. 
    /// It is currently implemented as Dictionary for quick lookup for artifacts.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLArtifactsCollection : Dictionary<string, TLArtifact>, IXmlSerializable, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_collectionId = String.Empty;

        /// <summary>
        /// Gets or sets the collection id.
        /// </summary>
        /// <value>
        /// The collection id.
        /// </value>
        public string CollectionId
        {
            get { return m_collectionId; }
            set { m_collectionId = value; }
        }

        private string m_collectionName = String.Empty;

        /// <summary>
        /// Gets or sets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        public string CollectionName
        {
            get { return m_collectionName; }
            set { m_collectionName = value; }
        }

        private string m_collectionVersion = String.Empty;

        /// <summary>
        /// Gets or sets the collection version.
        /// </summary>
        /// <value>
        /// The collection version.
        /// </value>
        public string CollectionVersion
        {
            get { return m_collectionVersion; }
            set { m_collectionVersion = value; }
        }

        private string m_collectionDescription = String.Empty;

        /// <summary>
        /// Gets or sets the collection description.
        /// </summary>
        /// <value>
        /// The collection description.
        /// </value>
        public string CollectionDescription
        {
            get { return m_collectionDescription; }
            set { m_collectionDescription = value; }
        }

        #endregion Members

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLArtifactsCollection() : base() { }

        /// <summary>
        /// Allows finding artifact by given artifactId. 
        /// If artifact of the given id has not been found, the method returns null. 
        /// </summary>
        /// <param name="artifactId">Artifact id</param>
        /// <returns>returns artifact of the given id, or null if artifact has not been found</returns>
        public TLArtifact FindArtifactByID(string artifactId)
        {
            TLArtifact retArtifact = null;
            TryGetValue(artifactId, out retArtifact);
            return retArtifact;
        }

        /// <summary>
        /// Adds artifact to the collection with a key of artifact id. 
        /// </summary>
        /// <param name="artifact">artifact to be added to the collection</param>
        public void Add(TLArtifact artifact)
        {
            Add(artifact.Id, artifact);
        }

        #endregion Methods

        #region Serialization

        /// <summary>
        /// Initializes a new instance of the TLArtifactsCollection class with serialized data.
        /// Supports serialization
        /// </summary>
        /// <param name="info">A SerializationInfo object that contains the information required to serialize the TLArtifactsCollection object.</param>
        /// <param name="context">A StreamingContext structure that contains the source and destination of the serialized stream associated with the TLArtifactsCollection object.</param>
        protected TLArtifactsCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");

            CollectionId = (string)info.GetValue("CollectionId", typeof(string));
            CollectionName = (string)info.GetValue("CollectionName", typeof(string));
            CollectionVersion = (string)info.GetValue("CollectionVersion", typeof(string));
            CollectionDescription = (string)info.GetValue("CollectionDescription", typeof(string));
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
            if (info == null)
                throw new System.ArgumentNullException("info");

            base.GetObjectData(info, context);

            info.AddValue("CollectionId", CollectionId);
            info.AddValue("CollectionName", CollectionName);
            info.AddValue("CollectionVersion", CollectionVersion);
            info.AddValue("CollectionDescription", CollectionDescription);
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
                TLArtifact artifact = new TLArtifact();
                artifact.ReadXml(reader);

                Add(artifact.Id, artifact);
                reader.Read();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Version", version.ToString());

            foreach (TLArtifact artifact in this.Values)
            {
                artifact.WriteXml(writer);
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
            var dataVersion = reader.ReadInt32();
            if (dataVersion != TLArtifactsCollection.version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                m_collectionId = reader.ReadString();
                m_collectionVersion = reader.ReadString();
                m_collectionName = reader.ReadString();
                m_collectionDescription = reader.ReadString();

                int artifactsCount = reader.ReadInt32();
                for (int i = 0; i < artifactsCount; ++i)
                {
                    string artifactIdKey = reader.ReadString();
                    TLArtifact artifact = new TLArtifact();
                    artifact.ReadData(reader);
                    Add(artifact);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLArtifactsCollection.version);

            writer.Write(m_collectionId);
            writer.Write(m_collectionVersion);
            writer.Write(m_collectionName);
            writer.Write(m_collectionDescription);

            //store count of artifacts in the list
            writer.Write(Count);
            foreach (KeyValuePair<string, TLArtifact> artifact in this)
            {
                string artifactIdKey = artifact.Key;
                writer.Write(artifactIdKey);
                artifact.Value.WriteData(writer);
            }
        }

        #endregion
    }
}

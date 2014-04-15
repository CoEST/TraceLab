using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TraceLabSDK;

namespace AdvSoftEng.Types
{
    [Serializable]
    [WorkspaceType]
    public class NormalizedVector : Dictionary<string, double>, IXmlSerializable
    {

        internal NormalizedVector() : base() { }

        public NormalizedVector(String name)
        {
            _id = name;
        }

        private String _id;
        public String ID
        {
            get
            {
                return _id;
            }
        }

        #region Serialization

        /// <summary>
        /// Initializes a new instance of the DocumentVectorCollection class with serialized data.
        /// Supports serialization
        /// </summary>
        /// <param name="info">A SerializationInfo object that contains the information required to serialize the TLArtifactsCollection object.</param>
        /// <param name="context">A StreamingContext structure that contains the source and destination of the serialized stream associated with the TLArtifactsCollection object.</param>
        protected NormalizedVector(SerializationInfo info, StreamingContext context)
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

        private static int version = 1;

        /// <summary>
        /// Current version
        /// </summary>
        public int Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, this method should and does return null.
        /// See IXmlSerializable.GetSchema Method documentation for more details.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;

            if (wasEmpty)
                return;

            Version = int.Parse(reader.GetAttribute("Version"));
            _id = reader.ReadElementContentAsString("ID", String.Empty);

            // read terms
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                String Key = reader.ReadElementContentAsString("Key", String.Empty);
                int Value = reader.ReadElementContentAsInt("Value", String.Empty);
                Add(Key, Value);
                reader.Read();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("NormalizedVector");
            writer.WriteAttributeString("Version", Version.ToString());
            writer.WriteElementString("ID", _id);
            // write terms
            foreach (KeyValuePair<string, double> pair in this)
            {
                writer.WriteStartElement("KeyValuePair");
                writer.WriteElementString("Key", pair.Key);
                writer.WriteElementString("Value", pair.Value.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}

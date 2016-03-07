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

namespace TraceLabSDK.Types
{
    /// <summary>
    /// Represents the dictionary of term entries. 
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLDictionaryIndex : IXmlSerializable, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private Dictionary<string, TLTermEntry> m_terms = new Dictionary<string, TLTermEntry>();
        /// <summary>
        /// Data Structure that contains the collection of Terms
        /// </summary>
        private Dictionary<string, TLTermEntry> Terms
        {
            get
            {
                return m_terms;
            }
            set
            {
                m_terms = value;
            }
        }

        /// <summary>
        /// just additional collection - needed to be able to return nice looking collection
        /// in java, rather than Terms.Values as ICollection of TermEntries - which would look ugly with generics.
        /// </summary>
        private TermEntryCollection m_termEntries = new TermEntryCollection();

        /// <summary>
        /// Returns collection of termEntries collection. 
        /// </summary>
        /// <returns>collection of termEntries collection.</returns>
        public TermEntryCollection TermEntries
        {
            get
            {
                return m_termEntries;
            }
        }

        /// <summary>
        /// Returns total number of all termEntries
        /// </summary>
        /// <returns>number of terms</returns>
        public int NumberOfTermEntries
        {
            get
            {
                return Terms.Count;
            }
        }

        private Dictionary<string, double> m_vectorLengthsOfArtifacts = new Dictionary<string, double>();
        /// <summary>
        /// Stores the vector lenght of each artifact text - it is later used for normalization purposes 
        /// The vector length is the square root of the sum of the squares of all the term weights.	
        /// </summary>
        private Dictionary<string, double> VectorLengthsOfArtifacts
        {
            get
            {
                return m_vectorLengthsOfArtifacts;
            }
            set
            {
                m_vectorLengthsOfArtifacts = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLDictionaryIndex() 
        {
        }

        /// <summary>
        /// Allows to add the new term entry to the dictionary index.
        /// </summary>
        /// <param name="term">term word</param>
        /// <param name="numberOfArtifactsContainingTerm">number of all artifacts of each text contain given term</param>
        /// <param name="totalFrequencyAcrossAllArtifacts">total frequency across all artifacts</param>
        /// <param name="weight">weight for the given term</param>
        /// <returns>just created term entry</returns>
        public TLTermEntry AddTermEntry(string term, int numberOfArtifactsContainingTerm, int totalFrequencyAcrossAllArtifacts, double weight)
        {
            // Integrity checks
            if (Terms.ContainsKey(term))
                throw new ArgumentException("The dictionary already contains that term");

            TLTermEntry termEntry = new TLTermEntry(term, numberOfArtifactsContainingTerm, totalFrequencyAcrossAllArtifacts, weight);

            Terms.Add(term, termEntry);
            m_termEntries.Add(termEntry);

            return termEntry;
        }

        /// <summary>
        /// Allows to add a new term entry to the dictionary index.
        /// </summary>
        /// <param name="termEntry">term entry to be added</param>
        private void AddTermEntry(TLTermEntry termEntry)
        {
            if (Terms.ContainsKey(termEntry.Term))
                throw new ArgumentException("The dictionary already contains that term");

            Terms.Add(termEntry.Term, termEntry);
            m_termEntries.Add(termEntry);
        }

        /// <summary>
        /// Checks if dictionary contains the term entry.
        /// </summary>
        /// <param name="term">term word</param>
        /// <returns>returns true if dictionary contains termEntry, false otherwise</returns>
        public bool ContainsTermEntry(string term)
        {
            if (term == null)
                throw new ArgumentException("The term cannot be null");

            return Terms.ContainsKey(term);
        }

        /// <summary>
        /// Finds and returns termEntry for the given term word.
        /// Throws exception if the term has not been found. 
        /// </summary>
        /// <param name="term">term word</param>
        /// <returns>the termEntry if it has been found in the dictionaryIndex, otherwise throws exception</returns>
        public TLTermEntry GetTermEntry(string term)
        {
            if (term == null)
                throw new ArgumentException("The term cannot be null");

            TLTermEntry termEntry;

            if (!Terms.TryGetValue(term, out termEntry))
            {
                throw new ArgumentException(String.Format("Terms collection does not contain specified term {0}", term));
            }
            else
            {
                return termEntry;
            }
        }

        /// <summary>
        /// Returns the weigh for the given artifact text
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <returns>weight of the text of the given artifact</returns>
        public double GetDocumentVectorWeight(string artifactId)
        {
            // Integrity checks
            if (artifactId == null || artifactId == "")
                throw new ArgumentException("The artifactId cannot be enpty or null");

            double weight;

            if (!this.VectorLengthsOfArtifacts.TryGetValue(artifactId, out weight))
            {
                throw new ArgumentException("There is no length associated with this artifact.");
            }
            else
            {
                return weight;
            }
        }

        /// <summary>
        /// Allows setting the document 
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <param name="weight">weight for the artifact</param>
        public void SetDocumentVectorWeight(string artifactId, double weight)
        {
            // Integrity checks
            if (artifactId == "")
                throw new ArgumentException("The artifactId can't be null");
            if (weight < 0)
                throw new ArgumentException("Weight cannot be negative");

            if (VectorLengthsOfArtifacts.ContainsKey(artifactId) == true)
            {
                VectorLengthsOfArtifacts[artifactId] = weight;
            }
            else
            {
                VectorLengthsOfArtifacts.Add(artifactId, weight);
            }

        }

        /// <summary>
        /// Returns true if dictionary index contains weight for the given artifact.
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <returns>true, if weight for the artifact has been found, false otherwise</returns>
        public bool ContainsArtifactVectorWeight(string artifactId)
        {
            if (artifactId == null || artifactId == "")
                throw new ArgumentException("The artifactId cannot be empty or null");

            return VectorLengthsOfArtifacts.ContainsKey(artifactId);
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
            bool wasEmpty = reader.IsEmptyElement;

            if (wasEmpty)
                return;

            version = int.Parse(reader.GetAttribute("Version"));

            reader.Read();
            reader.Read();
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                TLTermEntry termEntry = new TLTermEntry();
                termEntry.ReadXml(reader);
                AddTermEntry(termEntry);
            }

            reader.Read();
            reader.Read();
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                string artifactId = reader.GetAttribute("artifactId");
                double vectorLength = double.Parse(reader.GetAttribute("vectorLength"));
                SetDocumentVectorWeight(artifactId, vectorLength);
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

            writer.WriteStartElement("TermEntries");

            foreach (TLTermEntry termEntry in Terms.Values)
            {
                termEntry.WriteXml(writer);
            }

            writer.WriteEndElement();

            writer.WriteStartElement("VectorLengthsOfArtifacts");

            foreach (string artifactId in VectorLengthsOfArtifacts.Keys)
            {
                writer.WriteStartElement("ArtifactVectorLength");
                writer.WriteAttributeString("artifactId", artifactId);
                writer.WriteAttributeString("vectorLength", VectorLengthsOfArtifacts[artifactId].ToString());
                writer.WriteEndElement();
            }

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
                int termsCount = reader.ReadInt32();

                //create new collections
                m_terms = new Dictionary<string, TLTermEntry>();
                m_termEntries = new TermEntryCollection();

                for (int i = 0; i < termsCount; ++i)
                {
                    TLTermEntry termEntry = new TLTermEntry();
                    termEntry.ReadData(reader);
                    AddTermEntry(termEntry);
                }

                int vectorLenghtsOfArtifactsCount = reader.ReadInt32();

                m_vectorLengthsOfArtifacts = new Dictionary<string, double>();
                for (int i = 0; i < vectorLenghtsOfArtifactsCount; ++i)
                {
                    string artifactId = reader.ReadString();
                    double weight = reader.ReadDouble();
                    m_vectorLengthsOfArtifacts.Add(artifactId, weight);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(version);

            writer.Write(m_terms.Count);

            foreach (TLTermEntry termEntry in m_terms.Values)
            {
                termEntry.WriteData(writer);
            }

            writer.Write(m_vectorLengthsOfArtifacts.Count);

            foreach (KeyValuePair<string, double> pair in m_vectorLengthsOfArtifacts)
            {
                string artifactId = pair.Key;
                double weight = pair.Value;
                writer.Write(artifactId);
                writer.Write(weight);
            }
        }

        #endregion
    }
}

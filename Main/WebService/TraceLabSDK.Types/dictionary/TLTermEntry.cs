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
    /// Represents term entry. Each term entry consists of 
    /// total freqeuncy across all documents that the dictionary has been built from.
    /// total weight - depends on the dictionary builder, how the weight score has been calculated
    /// collection of postings.
    /// </summary>
    [Serializable]
    public class TLTermEntry : IXmlSerializable, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_term = string.Empty;
        /// <summary>
        /// Gets or sets the term
        /// </summary>
        public string Term
        {
            get { return m_term; }
            set { m_term = value; }
        }

        private int m_numberOfArtifactsContainingTerm;
        /// <summary>
        /// Gets or sets the number of artifacts conatining the term
        /// </summary>
        /// <exception cref="System.ArgumentException">if sets to value less than 0</exception>
        public int NumberOfArtifactsContainingTerm
        {
            get
            {
                return m_numberOfArtifactsContainingTerm;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Number of documents has to be greater than 0.");

                m_numberOfArtifactsContainingTerm = value;
            }
        }

        private int m_totalFrequencyAcrossArtifacts;
        /// <summary>
        /// Gets or sets the total frequency across all artifacts
        /// </summary>
        public int TotalFrequencyAcrossArtifacts
        {
            get
            {
                return m_totalFrequencyAcrossArtifacts;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Total frequency has to be greater than 0.");

                m_totalFrequencyAcrossArtifacts = value;
            }
        }

        private double m_weight;
        /// <summary>
        /// Gets or sets the total weight score for the term entry
        /// </summary>
        public double Weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }

        private Dictionary<string, TLPosting> m_postingsLookup = new Dictionary<string, TLPosting>();
        private Dictionary<string, TLPosting> PostingsLookup
        {
            get
            {
                return m_postingsLookup;
            }
        }

        /// <summary>
        /// just additional collection - needed to be able to return nice looking collection
        /// in java, rather than Terms.Values as ICollection of TermEntries - which would look ugly with generics.
        /// </summary>
        private PostingsCollection m_postings = new PostingsCollection();
        /// <summary>
        /// Gets Postings Collections for the current TLTermEntry
        /// </summary>
        public PostingsCollection Postings
        {
            get
            {
                return m_postings;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLTermEntry() { }

        /// <summary>
        /// Constructor to create new term entry
        /// </summary>
        /// <param name="term">term word</param>
        /// <param name="numberOfArtifactsContainingTerm">number of all artifacts of each text contain given term</param>
        /// <param name="totalFrequencyAcrossAllArtifacts">total frequency across all artifacts</param>
        /// <param name="weight">weight for the given term</param>
        /// <returns>just created term entry</returns>
        /// <exception cref="System.ArgumentException">if term is null; weight is less than zero, numberOfArtifactsContainingTerm is less than 0; totalFrequencyAcrossAllArtifacts is less than 0</exception>
        public TLTermEntry(string term, int numberOfArtifactsContainingTerm, int totalFrequencyAcrossAllArtifacts, double weight)
            : this()
        {
            if (term == null)
                throw new ArgumentException("The term can't be null");
            if (numberOfArtifactsContainingTerm <= 0)
                throw new ArgumentException("NumberOfArtifactsContainingTerm has to be greater than 0.");
            if (totalFrequencyAcrossAllArtifacts <= 0)
                throw new ArgumentException("TotalFrequencyAcrossAllArtifacts has to be greater than 0.");
            if (weight < 0)
                throw new ArgumentException("Weight can't be negative");

            Term = term;
            NumberOfArtifactsContainingTerm = numberOfArtifactsContainingTerm;
            TotalFrequencyAcrossArtifacts = totalFrequencyAcrossAllArtifacts;
            Weight = weight;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows adding new posting to the termEntry.
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <param name="frequency">frequency of the term in the given artifact</param>
        /// <param name="weight">local weight of the term in the posting</param>
        /// <returns>created posting</returns>
        /// <exception cref="System.ArgumentException">if documentId is null or empty; or if frequency is less than 0</exception>
        public TLPosting AddPosting(string artifactId, int frequency, double weight)
        {
            if (artifactId == null || artifactId == "")
                throw new ArgumentException("The documentId cannot be empty or null");
            if (frequency <= 0)
                throw new ArgumentException("Frequency has to be greater than 0.");

            TLPosting posting = new TLPosting(artifactId, frequency, weight);
            PostingsLookup.Add(artifactId, posting);
            m_postings.Add(posting);

            return posting;

        }

        /// <summary>
        /// Allows adding new posting to the termEntry.
        /// </summary>
        /// <exception cref="System.ArgumentException">if posting is null</exception>
        private void AddPosting(TLPosting posting)
        {
            if (posting == null)
                throw new ArgumentException("The posting cannot be null");

            PostingsLookup.Add(posting.ArtifactId, posting);
            m_postings.Add(posting);
        }

        /// <summary>
        /// Checks if the term entry contains given posting, ie. if the term is in the given artifact.
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <returns>true, if termEntry conatins the posting, false otherwise</returns>
        /// <exception cref="System.ArgumentException">if artifact id is null or empty</exception>
        public bool ContainsPosting(string artifactId)
        {
            if (artifactId == null || artifactId == "")
                throw new ArgumentException("The documentId cannot be empty or null");

            return PostingsLookup.ContainsKey(artifactId);
        }

        /// <summary>
        /// Gets the posting for the specified artifact id
        /// </summary>
        /// <param name="artifactId">artifact id</param>
        /// <returns>the posting for the specified artifact</returns>
        /// <exception cref="System.ArgumentException">if artifact id is null or empty</exception>
        public TLPosting GetPosting(string artifactId)
        {
            if (artifactId == null || artifactId == "")
                throw new ArgumentException("The documentId cannot be empty or null");

            TLPosting posting;

            if (!PostingsLookup.TryGetValue(artifactId, out posting))
            {
                throw new ArgumentException("Postings collection does not contain specified document");
            }
            else
            {
                return posting;
            }
        }

        /// <summary>
        /// Gets number of postings
        /// </summary>
        /// <returns>number of posings</returns>
        public int GetNumberOfPostings()
        {
            return PostingsLookup.Count;
        }

        /// <summary>
        /// Compares two specified TLTermEntry objects and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first TLTermEntry to compare. </param>
        /// <param name="y">The second TLTermEntry to compare. </param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public int Compare(TLTermEntry x, TLTermEntry y)
        {
            return ((new CaseInsensitiveComparer()).Compare(x.Term, y.Term));
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
            Term = reader.GetAttribute("Term");
            NumberOfArtifactsContainingTerm = int.Parse(reader.GetAttribute("NoOfArtsWithTerm"));
            TotalFrequencyAcrossArtifacts = int.Parse(reader.GetAttribute("TotalFrequency"));
            Weight = double.Parse(reader.GetAttribute("Weight"));
            reader.Read();
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                TLPosting posting = new TLPosting();
                posting.ReadXml(reader);
                AddPosting(posting);
            }
            //read to the next
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("TLTermEntry");
            writer.WriteAttributeString("Version", version.ToString());
            writer.WriteAttributeString("Term", Term);
            writer.WriteAttributeString("NoOfArtsWithTerm", NumberOfArtifactsContainingTerm.ToString());
            writer.WriteAttributeString("TotalFrequency", TotalFrequencyAcrossArtifacts.ToString());
            writer.WriteAttributeString("Weight", Weight.ToString());
            foreach (TLPosting posting in PostingsLookup.Values)
            {
                posting.WriteXml(writer);
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
            if (dataversion != TLTermEntry.version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                m_term = reader.ReadString();
                m_numberOfArtifactsContainingTerm = reader.ReadInt32();
                m_totalFrequencyAcrossArtifacts = reader.ReadInt32();
                m_weight = reader.ReadDouble();
                int postingsCount = reader.ReadInt32();

                //reset both collections
                m_postings = new PostingsCollection();
                m_postingsLookup = new Dictionary<string, TLPosting>();

                for (int i = 0; i < postingsCount; ++i)
                {
                    TLPosting posting = new TLPosting();
                    posting.ReadData(reader);
                    AddPosting(posting);
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLTermEntry.version);
            writer.Write(m_term);
            writer.Write(m_numberOfArtifactsContainingTerm);
            writer.Write(m_totalFrequencyAcrossArtifacts);
            writer.Write(m_weight);

            writer.Write(PostingsLookup.Count);

            foreach (TLPosting posting in m_postingsLookup.Values)
            {
                posting.WriteData(writer);
            }
        }

        #endregion
    }
}

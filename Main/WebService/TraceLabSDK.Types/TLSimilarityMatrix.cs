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
using System.Runtime.Serialization;
using TraceLabSDK;

namespace TraceLabSDK.Types
{
    /// <summary>
    /// TLMatrix represents the set of links from source artifact ids to target artifact ids. 
    /// It is implemented as dictionary for fast lookup from source ids. 
    /// 
    /// This matrix can be used either as standard similarity matrix, or an answer matrix. 
    /// 
    /// It also allows to set Threshold, that maybe used either as confidenceThreshold or similarity threshold.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLSimilarityMatrix : IXmlSerializable, IRawSerializable
    {
        #region Methods

        protected const int version = 1;

        protected string m_name = string.Empty;
        protected double m_threshold = 0.0;

        /// <summary>
        /// This does not use standard serialization - see the ISerializable implementation for details.
        /// </summary>
        //[NonSerialized]
        protected Dictionary<string, Dictionary<string, double>> m_matrix = new Dictionary<string, Dictionary<string, double>>();

        [NonSerialized]
        private Dictionary<string, StringHashSet> m_cacheOfSetsPerSourceArtifacts = new Dictionary<string, StringHashSet>();

        [NonSerialized]
        private Dictionary<string, TLLinksList> m_cacheOfLinksPerSourceArtifacts = new Dictionary<string, TLLinksList>();

        protected Dictionary<string, TLLinksList> CacheOfLinksPerSourceArtifacts
        {
            get
            {
                if (m_cacheOfLinksPerSourceArtifacts == null)
                {
                    m_cacheOfLinksPerSourceArtifacts = new Dictionary<string, TLLinksList>();
                }
                return m_cacheOfLinksPerSourceArtifacts;
            }
        }

        protected Dictionary<string, StringHashSet> CacheOfSetsPerSourceArtifacts
        {
            get
            {
                if (m_cacheOfSetsPerSourceArtifacts == null)
                {
                    m_cacheOfSetsPerSourceArtifacts = new Dictionary<string, StringHashSet>();
                }

                return m_cacheOfSetsPerSourceArtifacts;
            }
        }

        /// <summary>
        /// Threshold can be interpreted in two ways
        /// If matrix is used as answer matrix Threshold represents confidence threshold, that can be useful in case of existence of "gray".
        /// In this case anything above this threshold value is considered as relevant link.
        /// 
        /// If matrix is used as similarity matrix, anything above this threshold value is considered as retrieved link.
        /// 
        /// Default Thresold value is 0.0.
        /// </summary>
        public double Threshold
        {
            get
            {
                return m_threshold;
            }
            set
            {
                if (value != m_threshold)
                {
                    //clear temporary cache of hashsets per source artifact
                    CacheOfSetsPerSourceArtifacts.Clear();
                    CacheOfLinksPerSourceArtifacts.Clear();

                    m_threshold = value;
                }
            }
        }

        /// <summary>
        /// Returns the copy of the source artifacts ids that are included in this answer set.
        /// </summary>
        public StringHashSet SourceArtifactsIds
        {
            get
            {
                return new StringHashSet(m_matrix.Keys as IEnumerable<string>);
            }
        }

        /// <summary>
        /// Returns the copy list of all links in the answer set. 
        /// It is lazy load, the list is constructed when it is requested.
        /// </summary>
        public TLLinksList AllLinks
        {
            get
            {
                var allLinks = new TLLinksList();
                foreach (KeyValuePair<string, Dictionary<string, double>> sourceArtifact in m_matrix)
                {
                    string sourceArtifactId = sourceArtifact.Key;
                    var sourceArtifactLinks = sourceArtifact.Value;

                    foreach (KeyValuePair<string, double> targetArtifact in sourceArtifactLinks)
                    {
                        string targetArtifactId = targetArtifact.Key;
                        var score = targetArtifact.Value;

                        allLinks.Add(new TLSingleLink(sourceArtifactId, targetArtifactId, score));
                    }
                }

                return allLinks;
            }
        }

        /// <summary>
        /// Gets or sets the name. Usually it is useful to name it by same name as a dataset name, which similarity matrix corresponds to. 
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Gets total count of all links in the spare matrix. 
        /// </summary>
        public int Count
        {
            get
            {
                int totalCount = 0;
                foreach (Dictionary<string, double> links in m_matrix.Values)
                {
                    totalCount += links.Count;
                }
                return totalCount;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLSimilarityMatrix() { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public TLSimilarityMatrix(TLSimilarityMatrix inMatrix) 
        {
            m_cacheOfLinksPerSourceArtifacts = inMatrix.m_cacheOfLinksPerSourceArtifacts;
            m_cacheOfSetsPerSourceArtifacts = inMatrix.m_cacheOfSetsPerSourceArtifacts;
            m_matrix = inMatrix.m_matrix;
            m_name = inMatrix.m_name;
            m_threshold = inMatrix.m_threshold;
        }

        /// <summary>
        /// Returns true if the link from sourceArtifactId to targetArtifactId exists in the set, and its confidence score is higher than Threshold. 
        /// 
        /// There are different interpratation of this method:
        /// In case the matrix is an answer matrix, the true return value means the link from source to target artifact is relevant.
        /// In case the matrix is a similarity matrix, the true return value means the link from source to target artifact is retrieved.
        /// 
        /// Notice, that the default Threshold is set to 0.0. Any link with higher score is considered as relevant/retrieved link. 
        /// 
        /// </summary>
        /// <param name="sourceArtifactId">Id of the source artifact</param>
        /// <param name="targetArtifactId">Id of the target artifact</param>
        /// <returns>true if the link from sourceArtifactId to targetArtifact is relevant or retrieved</returns>
        public bool IsLinkAboveThreshold(string sourceArtifactId, string targetArtifactId)
        {
            bool retVal = false;
            Dictionary<string, double> links;
            if (m_matrix.TryGetValue(sourceArtifactId, out links))
            {
                double score;
                if (links.TryGetValue(targetArtifactId, out score))
                {
                    retVal = (score > Threshold);
                }

            }
            return retVal;
        }

        /// <summary>
        /// Returns score (interpreted as either confidence or similarity score) for the given source and artifact link. Returns 0.0 if the link has not been found.
        /// </summary>
        /// <param name="sourceArtifactId">Id of the source artifact</param>
        /// <param name="targetArtifactId">Id of the target artifact</param>
        /// <returns>score for given link</returns>
        public double GetScoreForLink(string sourceArtifactId, string targetArtifactId)
        {
            double retVal = 0.0;
            Dictionary<string, double> links;
            if (m_matrix.TryGetValue(sourceArtifactId, out links))
            {
                links.TryGetValue(targetArtifactId, out retVal);
            }
            return retVal;
        }

        /// <summary>
        /// Get relevant links for source artifacts with score larger than threshold. 
        /// </summary>
        /// <param name="sourceArtifactId">Id of source artifact for which the set of relevant/retrieved links is requested</param>
        /// <returns>Hashset of target artifacts ids that are retrieved or relevant to the given source artifact (depends on usage).</returns>
        public StringHashSet GetSetOfTargetArtifactIdsAboveThresholdForSourceArtifact(string sourceArtifactId)
        {
            StringHashSet linksForSourceArtifact;
            if (CacheOfSetsPerSourceArtifacts.TryGetValue(sourceArtifactId, out linksForSourceArtifact) == false)
            {
                linksForSourceArtifact = new StringHashSet();

                Dictionary<string, double> links;
                if (m_matrix.TryGetValue(sourceArtifactId, out links))
                {
                    foreach (string targetArtifactId in links.Keys)
                    {
                        if (links[targetArtifactId] > Threshold)
                        {
                            linksForSourceArtifact.Add(targetArtifactId);
                        }
                    }
                }

                CacheOfSetsPerSourceArtifacts.Add(sourceArtifactId, linksForSourceArtifact);
            }
            return linksForSourceArtifact; //return empty set
        }

        /// <summary>
        /// Get relevant links for source artifacts with score larger than threshold. 
        /// </summary>
        /// <param name="sourceArtifactId">Id of source artifact for which the set of relevant/retrieved links is requested</param>
        /// <returns>Hashset of target artifacts ids that are retrieved or relevant to the given source artifact (depends on usage).</returns>
        public TLLinksList GetLinksAboveThresholdForSourceArtifact(string sourceArtifactId)
        {
            TLLinksList linksForSourceArtifact;
            if (CacheOfLinksPerSourceArtifacts.TryGetValue(sourceArtifactId, out linksForSourceArtifact) == false)
            {
                linksForSourceArtifact = new TLLinksList();

                Dictionary<string, double> links;
                if (m_matrix.TryGetValue(sourceArtifactId, out links))
                {
                    foreach (string targetArtifactId in links.Keys)
                    {
                        if (links[targetArtifactId] > Threshold)
                        {
                            linksForSourceArtifact.Add(new TLSingleLink(sourceArtifactId, targetArtifactId, links[targetArtifactId]));
                        }
                    }
                }

                CacheOfLinksPerSourceArtifacts.Add(sourceArtifactId, linksForSourceArtifact);
            }
            return linksForSourceArtifact; //return empty set
        }

        /// <summary>
        /// Adds link from source artifact id to the target artifact. 
        /// 
        /// Notice, if the link has been already added, and score is different, it will throw error exception.
        /// If the score is the same, the algorithm will skip the duplicate link. 
        /// </summary>
        /// <param name="sourceArtifactId">Id of source artifact</param>
        /// <param name="targetArtifactId">Id of target artifact</param>
        /// <param name="score">score</param>
        public void AddLink(string sourceArtifactId, string targetArtifactId, double score)
        {
            Dictionary<string, double> links;
            if (m_matrix.TryGetValue(sourceArtifactId, out links) == false)
            {
                links = new Dictionary<string, double>();
                m_matrix.Add(sourceArtifactId, links);
            }

            if (links.ContainsKey(targetArtifactId) == false)
            {
                links.Add(targetArtifactId, score);
            }
            else
            {
                if (score != links[targetArtifactId])
                {
                    throw new ArgumentException(String.Format("Link for source artifact {0} and target artifact {1} has already been added to the spare matrix", sourceArtifactId, targetArtifactId));
                }
            }
        }

        /// <summary>
        /// Returns the count of relevant or retrieved* links for given source artifact. (*depends on usage)
        /// </summary>
        /// <param name="sourceArtifactId">Id of source artifact</param>
        /// <returns>Number of relevant or retrieved links</returns>
        public int GetCountOfLinksAboveThresholdForSourceArtifact(string sourceArtifactId)
        {
            return GetSetOfTargetArtifactIdsAboveThresholdForSourceArtifact(sourceArtifactId).Count;
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
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;

            if (wasEmpty)
                return;

            var version = int.Parse(reader.GetAttribute("Version"));
            // TODO: Check version matches the class version.
            Threshold = double.Parse(reader.GetAttribute("Threshold"));
            Name = reader.GetAttribute("Name");

            reader.Read();
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                string srcArtId = reader.GetAttribute("sourceArtifactId");
                string trgArtId = reader.GetAttribute("targetArtifactId");
                double confidenceScore = double.Parse(reader.GetAttribute("score"));

                AddLink(srcArtId, trgArtId, confidenceScore);
                reader.Read();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized. </param>
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Version", version.ToString());
            writer.WriteAttributeString("Threshold", Threshold.ToString());
            if (String.IsNullOrEmpty(Name))
            {
                writer.WriteAttributeString("Name", String.Empty); 
            } 
            else
            {
                writer.WriteAttributeString("Name", Name.ToString());
            }

            foreach (TLSingleLink link in AllLinks)
            {
                writer.WriteStartElement("Link");
                writer.WriteAttributeString("sourceArtifactId", link.SourceArtifactId);
                writer.WriteAttributeString("targetArtifactId", link.TargetArtifactId);
                writer.WriteAttributeString("score", link.Score.ToString());
                writer.WriteEndElement();
            }
        }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public virtual void ReadData(System.IO.BinaryReader reader)
        {
            var dataVersion = reader.ReadInt32();
            if (dataVersion != version)
            {
                throw new InvalidOperationException("Binary reader did not correct version data. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                m_name = reader.ReadString();
                m_threshold = reader.ReadDouble();

                Dictionary<string, Dictionary<string, double>> matrix = new Dictionary<string, Dictionary<string, double>>();

                int sourceCount = reader.ReadInt32();
                for (int i = 0; i < sourceCount; ++i)
                {
                    string sourceId = reader.ReadString();
                    int linkCount = reader.ReadInt32();

                    Dictionary<string, double> links = new Dictionary<string, double>();
                    matrix[sourceId] = links;

                    for (int linkIterator = 0; linkIterator < linkCount; ++linkIterator)
                    {
                        string targetId = reader.ReadString();
                        double score = reader.ReadDouble();

                        links[targetId] = score;
                    }
                }

                m_matrix = matrix;
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(m_name); 
            writer.Write(m_threshold);
            writer.Write(m_matrix.Count);

            foreach (KeyValuePair<string, Dictionary<string, double>> sourceArtifact in m_matrix)
            {
                string sourceId = sourceArtifact.Key;
                writer.Write(sourceId);
                writer.Write(sourceArtifact.Value.Count);

                foreach (KeyValuePair<string, double> targetArtifact in sourceArtifact.Value)
                {
                    string targetId = targetArtifact.Key;
                    double score = targetArtifact.Value;

                    writer.Write(targetId);
                    writer.Write(score);
                }
            }
        }

        #endregion
    }
}

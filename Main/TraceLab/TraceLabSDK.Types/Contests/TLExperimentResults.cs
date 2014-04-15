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
using System.Xml.XPath;

namespace TraceLabSDK.Types.Contests
{
    /// <summary>
    /// Represents the set of metric results for one particular technique and set of metrics.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public sealed class TLExperimentResults : IXmlSerializable, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_techniqueName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the technique, which results were computed by.
        /// </summary>
        /// <value>
        /// The name of the technique.
        /// </value>
        public string TechniqueName
        {
            get { return m_techniqueName; }
            set { m_techniqueName = value; }
        }

        private List<DatasetResults> m_datasetsResults;

        /// <summary>
        /// Gets the datasets results.
        /// </summary>
        /// <value>
        /// The datasets results.
        /// </value>
        public IEnumerable<DatasetResults> DatasetsResults
        {
            get { return m_datasetsResults; }
        }

        private DatasetResults m_acrossAllDatasetsResults;

        /// <summary>
        /// Gets or sets the results across all datasets. 
        /// The metric used across all dataset may differ than the metrics used 
        /// per single dataset, thus it is treated as separate property and is not included in
        /// the collection of datasetsresults. 
        /// </summary>
        /// <value>
        /// The across all datasets results.
        /// </value>
        public DatasetResults AcrossAllDatasetsResults
        {
            get { return m_acrossAllDatasetsResults; }
            set { m_acrossAllDatasetsResults = value; }
        }

        private double m_score;

        /// <summary>
        /// Gets or sets the score.
        /// The score represents position in the leader board ranking.
        /// The score can be computed by any method in the component defined by the user.
        /// It has to be independent from other results, and cannot change over time. It is a const score
        /// computed for the technique. This value is going to be uploaded to the server when publishing results
        /// and will determine the position in the ranking.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public double Score
        {
            get { return m_score; }
            set { m_score = value; }
        }

        #endregion Members

        #region Methods

        internal TLExperimentResults() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TLExperimentResults"/> class.
        /// </summary>
        /// <param name="techniqueName">Name of the technique, which results were computed by.</param>
        public TLExperimentResults(string techniqueName)
        {
            TechniqueName = techniqueName;
            m_datasetsResults = new List<DatasetResults>();
        }

        /// <summary>
        /// Adds the dataset result.
        /// </summary>
        /// <param name="results">The results.</param>
        public void AddDatasetResult(DatasetResults results)
        {
            m_datasetsResults.Add(results);
        }

        #endregion Methods

        #region IXmlSerializable Members

        private IXmlSerializable m_baseData;

        /// <summary>
        /// Gets or sets the base data from which the metric results were computed.
        /// For example, the similarity matrix with tracing results.
        /// </summary>
        /// <value>
        /// The base data.
        /// </value>
        public IXmlSerializable BaseData
        {
            get { return m_baseData; }
            set { m_baseData = value; }
        }

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
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("TLExperimentResults/TechniqueName");
            if (iter != null)
                TechniqueName = iter.Value;

            iter = nav.SelectSingleNode("TLExperimentResults/Score");
            if (iter != null)
            {
                Score = iter.ValueAsDouble;
            }

            iter = nav.SelectSingleNode("TLExperimentResults/BaseData");
            if (iter != null)
            {
                string qualifiedTypeName = iter.GetAttribute("type", String.Empty);
                Type baseDataType = Type.GetType(qualifiedTypeName);
                if (baseDataType != null)
                {
                    var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(baseDataType, null);
                    iter.MoveToChild(XPathNodeType.Element); //move to nodes value
                    BaseData = (IXmlSerializable)serializer.Deserialize(iter.ReadSubtree());
                }
            }

            var datasetsResultSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(DatasetResults), null);
            iter = nav.SelectSingleNode("TLExperimentResults/DatasetsResults");
            if (iter != null)
            {
                m_datasetsResults = new List<DatasetResults>();
                XPathNodeIterator resultsNodes = iter.Select("DatasetResults");
                while (resultsNodes.MoveNext())
                {
                    var result = (DatasetResults)datasetsResultSerializer.Deserialize(resultsNodes.Current.ReadSubtree());
                    m_datasetsResults.Add(result);
                }
            }

            iter = nav.SelectSingleNode("TLExperimentResults/AcrossAllDatasetsResults");
            if (iter != null)
            {
                iter.MoveToChild(XPathNodeType.Element); //move to nodes value
                AcrossAllDatasetsResults = (DatasetResults)datasetsResultSerializer.Deserialize(iter.ReadSubtree());
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (String.IsNullOrEmpty(TechniqueName) == false)
            {
                writer.WriteElementString("TechniqueName", TechniqueName);
            }

            writer.WriteElementString("Score", Score.ToString());

            if (BaseData != null)
            {
                writer.WriteStartElement("BaseData");
                writer.WriteAttributeString("type", BaseData.GetType().GetTraceLabQualifiedName());
                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(BaseData.GetType(), null);
                serializer.Serialize(writer, BaseData);
                writer.WriteEndElement();
            }

            var datasetsResultSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(DatasetResults), null);
            if (DatasetsResults != null)
            {
                writer.WriteStartElement("DatasetsResults");
                foreach (DatasetResults datasetResults in DatasetsResults)
                {
                    datasetsResultSerializer.Serialize(writer, datasetResults);
                }
                writer.WriteEndElement();
            }

            if (AcrossAllDatasetsResults != null)
            {
                writer.WriteStartElement("AcrossAllDatasetsResults");
                datasetsResultSerializer.Serialize(writer, AcrossAllDatasetsResults);
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
            if (dataversion != TLExperimentResults.version)
            {
                throw new InvalidOperationException("Binary reader did not read correct data version. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                this.m_techniqueName = reader.ReadString();

                int resultsCount = reader.ReadInt32();
                this.m_datasetsResults = new List<DatasetResults>(resultsCount);
                for (int i = 0; i < resultsCount; i++)
                {
                    DatasetResults result = (DatasetResults)Activator.CreateInstance(typeof(DatasetResults), true);
                    result.ReadData(reader);
                    this.m_datasetsResults.Add(result);
                }

                this.m_score = reader.ReadDouble();

                bool isMemberPresent = reader.ReadBoolean();
                if (isMemberPresent)
                {
                    DatasetResults result = (DatasetResults)Activator.CreateInstance(typeof(DatasetResults), true);
                    result.ReadData(reader);
                    this.m_acrossAllDatasetsResults = result;
                }

                this.m_baseData = null;
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLExperimentResults.version);
            writer.Write(this.m_techniqueName);

            writer.Write(this.m_datasetsResults.Count);
            foreach (DatasetResults result in this.m_datasetsResults)
            {
                result.WriteData(writer);
            }

            writer.Write(this.m_score);

            if (this.m_acrossAllDatasetsResults != null)
            {
                writer.Write(true);
                this.m_acrossAllDatasetsResults.WriteData(writer);
            }
            else
            {
                writer.Write(false);
            }
        }

        #endregion IRawSerializable Members
    }
}

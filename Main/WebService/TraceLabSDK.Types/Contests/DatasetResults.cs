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
    /// Represents the results for the dataset of the given name.
    /// It contains the list of metric results.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class DatasetResults : IXmlSerializable, IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_datasetName;
        /// <summary>
        /// Gets or sets the name of the dataset which results were computed for.
        /// </summary>
        /// <value>
        /// The name of the dataset.
        /// </value>
        public string DatasetName
        {
            get { return m_datasetName; }
            set { m_datasetName = value; }
        }

        private List<Metric> m_metrics;
        /// <summary>
        /// Gets or sets the list of metrics
        /// </summary>
        /// <value>
        /// The metrics.
        /// </value>
        public IEnumerable<Metric> Metrics
        {
            get { return m_metrics; }
        }

        #endregion Members

        #region Methods

        /// <summary>
        /// Prevents a default instance of the <see cref="DatasetResults"/> class from being created.
        /// </summary>
        private DatasetResults() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetResults"/> class.
        /// </summary>
        /// <param name="datasetName">Name of the dataset, which results were computed for.</param>
        public DatasetResults(string datasetName)
        {
            DatasetName = datasetName;
            m_metrics = new List<Metric>();
        }

        /// <summary>
        /// Adds the metric.
        /// </summary>
        /// <param name="metric">The metric.</param>
        /// <exception cref="ArgumentException">throws if provided is not LineSeries or BoxSummaryData, the only types currently supported</exception>
        public void AddMetric(Metric metric)
        {
            if (metric is LineSeries || metric is BoxSummaryData)
            {
                m_metrics.Add(metric);
            }
            else
            {
                throw new ArgumentException("The only accepted metric s_metricTypes are either LineSeries or BoxSummaryData currently. ");
            }

        }

        /// <summary>
        /// Adds a LineSeries metric.
        /// </summary>
        /// <param name="metric">The metric.</param>
        public void AddLineSeriesMetric(LineSeries metric)
        {
            m_metrics.Add(metric);
        }

        /// <summary>
        /// Adds a BoxSummary metric.
        /// </summary>
        /// <param name="metric">The metric.</param>
        public void AddBoxSummaryMetric(BoxSummaryData metric)
        {
            m_metrics.Add(metric);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            DatasetResults d = obj as DatasetResults;
            if ((System.Object)d == null)
            {
                return false;
            }

            bool equal = DatasetName == d.DatasetName && Metrics.SequenceEqual(d.Metrics);
            return equal;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return DatasetName.GetHashCode();
        }

        #endregion Methods

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
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/DatasetResults/DatasetName");
            if (iter != null)
                DatasetName = iter.Value;

            iter = nav.SelectSingleNode("/DatasetResults/Metrics");
            if (iter != null)
            {
                m_metrics = new List<Metric>();
                XPathNodeIterator metricsNodes = iter.Select("Metric");
                while (metricsNodes.MoveNext())
                {
                    string qualifiedTypeName = metricsNodes.Current.GetAttribute("type", String.Empty);

                    Type metricType = Type.GetType(TypeHelper.ConvertOldTypeName(qualifiedTypeName));

                    var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(metricType, s_metricTypes);
                    var metric = (Metric)serializer.Deserialize(metricsNodes.Current.ReadSubtree());
                    m_metrics.Add(metric);
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (String.IsNullOrEmpty(DatasetName) == false)
                writer.WriteElementString("DatasetName", DatasetName);

            if (Metrics != null)
            {
                writer.WriteStartElement("Metrics");

                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(Metric), s_metricTypes);
                foreach (Metric metric in Metrics)
                {
                    serializer.Serialize(writer, metric);
                }
                writer.WriteEndElement();
            }
        }

        private static Type[] s_metricTypes = new Type[] 
                {
                    typeof(LineSeries),
                    typeof(BoxSummaryData),
                };

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            int dataversion = reader.ReadInt32();
            if (dataversion != DatasetResults.version)
            {
                throw new InvalidOperationException("Binary reader did not read correct data version. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                this.m_datasetName = reader.ReadString();
                
                int listCount = reader.ReadInt32();
                this.m_metrics = new List<Metric>(listCount);
                for (int i = 0; i < listCount; i++)
                {
                    string type = reader.ReadString();
                    IRawSerializable metricObject = (IRawSerializable)Activator.CreateInstance(Type.GetType(type), true);
                    metricObject.ReadData(reader);

                    Metric m = metricObject as Metric;
                    if (m != null)
                    {
                        this.AddMetric(m);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(DatasetResults.version);
            writer.Write(this.m_datasetName);
            writer.Write(this.m_metrics.Count);
            foreach (Metric m in this.m_metrics)
            {
                writer.Write(m.GetType().AssemblyQualifiedName);
                m.WriteData(writer);
            }
        }

        #endregion IRawSerializable Members
    }
}

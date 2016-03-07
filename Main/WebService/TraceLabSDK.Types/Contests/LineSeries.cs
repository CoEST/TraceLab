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
using TraceLab.Core;

namespace TraceLabSDK.Types.Contests
{
    /// <summary>
    /// Represent one series of points
    /// </summary>
    [Serializable]
    [WorkspaceType]
    [XmlRoot("Metric")]
    public sealed class LineSeries : Metric, IRawSerializable
    {
        #region Members

        private List<Point> m_points;
        /// <summary>
        /// Gets the points.
        /// </summary>
        public IEnumerable<Point> Points
        {
            get { return m_points; }
        }

        #endregion Members

        #region Methods

        /// <summary>
        /// Prevents a default instance of the <see cref="LineSeries"/> class from being created.
        /// </summary>
        private LineSeries() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <param name="metricDescription">The metric description.</param>
        public LineSeries(string metricName, string metricDescription)
            : base(metricName, metricDescription)
        {
            m_points = new List<Point>();
        }

        /// <summary>
        /// Add a point.
        /// </summary>
        /// <param name="point">The point.</param>
        public void AddPoint(Point point)
        {
            m_points.Add(point);
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("Metric/MetricName");
            if (iter != null)
                MetricName = iter.Value;

            iter = nav.SelectSingleNode("Metric/Description");
            if (iter != null)
                Description = iter.Value;

            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(Point), null);
            iter = nav.SelectSingleNode("Metric/Points");
            if (iter != null)
            {
                m_points = new List<Point>();
                XPathNodeIterator pointsNodes = iter.Select("Point");
                while (pointsNodes.MoveNext())
                {
                    var p = (Point)serializer.Deserialize(pointsNodes.Current.ReadSubtree());
                    m_points.Add(p);
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());

            writer.WriteElementString("MetricName", MetricName);
            writer.WriteElementString("Description", Description);

            writer.WriteStartElement("Points");
            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(Point), null);
            foreach (Point point in Points)
            {
                serializer.Serialize(writer, point);
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            LineSeries l = obj as LineSeries;
            if ((System.Object)l == null)
            {
                return false;
            }

            bool equal = MetricName == l.MetricName && Description == l.Description && Points.SequenceEqual(l.Points);
            return equal;
        }

        #endregion Methods

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void ReadData(System.IO.BinaryReader reader)
        {
            this.MetricName = reader.ReadString();
            this.Description = reader.ReadString();

            int listCount = reader.ReadInt32();
            this.m_points = new List<Point>(listCount);

            for (int i = 0; i < listCount; i++)
            {
                Point p = new Point();
                p.ReadData(reader);
                this.m_points.Add(p);
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(this.MetricName);
            writer.Write(this.Description);
            writer.Write(this.m_points.Count);
            foreach (Point p in this.m_points)
            {
                p.WriteData(writer);
            }
        }

        #endregion IRawSerializable Members
    }
}

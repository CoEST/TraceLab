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
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Represents node data that is serialized to xml
    /// </summary>
    [Serializable]
    public sealed class SerializedVertexDataWithSize : SerializedVertexData
    {

        /// <summary>
        /// Node width
        /// </summary>
        private double m_width;
        [XmlAttribute("Width")]
        public double Width
        {
            get { return m_width; }
            set
            {
                if (m_width != value)
                {
                    m_width = ConstrainDouble(value);
                    NotifyPropertyChanged("Width");
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Node height
        /// </summary>
        private double m_height;
        [XmlAttribute("Height")]
        public double Height
        {
            get { return m_height; }
            set
            {
                if (m_height != value)
                {
                    m_height = ConstrainDouble(value);
                    NotifyPropertyChanged("Height");
                    IsModified = true;
                }
            }
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            ReadXmlVertexData(reader, (r) =>
            {
                X = double.Parse(r.GetAttribute("X"), CultureInfo.CurrentCulture);
                Y = double.Parse(r.GetAttribute("Y"), CultureInfo.CurrentCulture);
                Width = double.Parse(r.GetAttribute("Width"), CultureInfo.CurrentCulture);
                Height = double.Parse(r.GetAttribute("Height"), CultureInfo.CurrentCulture);
            });
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            WriteVertexData(writer, (w) =>
            {
                w.WriteAttributeString("X", X.ToString(CultureInfo.CurrentCulture));
                w.WriteAttributeString("Y", Y.ToString(CultureInfo.CurrentCulture));
                w.WriteAttributeString("Width", Width.ToString(CultureInfo.CurrentCulture));
                w.WriteAttributeString("Height", Height.ToString(CultureInfo.CurrentCulture));
            });
        }

        public override SerializedVertexData Clone()
        {
            SerializedVertexDataWithSize clone = new SerializedVertexDataWithSize();
            clone.X = X;
            clone.Y = Y;
            clone.Width = Width;
            clone.Height = Height;
            clone.Metadata = Metadata.Clone();

            clone.m_isInitialized = m_isInitialized;

            return clone;
        }
    }
}

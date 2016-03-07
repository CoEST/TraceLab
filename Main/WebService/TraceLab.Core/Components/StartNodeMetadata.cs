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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    [XmlRoot("Metadata")]
    [Serializable]
    public class StartNodeMetadata : Metadata
    {
        public StartNodeMetadata()
        {
            Label = "Start";
        }

        public StartNodeMetadata(string label)
        {
            Label = label;
        }

        #region Deserialization Constructor

        protected StartNodeMetadata(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
        #region IXmlSerializable

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());
            writer.WriteAttributeString("Label", Label);
        }

        public override System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        #endregion

        public override Metadata Clone()
        {
            return new StartNodeMetadata(Label);
        }

        protected override bool CalculateModification()
        {
            return base.CalculateModification();
        }

        public override void ResetModifiedFlag()
        {
            base.ResetModifiedFlag();
        }
    }
}

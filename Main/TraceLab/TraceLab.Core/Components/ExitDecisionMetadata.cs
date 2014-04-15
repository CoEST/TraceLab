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
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Runtime.Serialization;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    [XmlRoot("Metadata")]
    [Serializable]
    public class ExitDecisionMetadata : Metadata
    {
        public ExitDecisionMetadata()
        {
            WaitsForAllPredecessors = false;
            Label = "Exit";
        }
        
        public override Metadata Clone()
        {
            return new ExitDecisionMetadata();
        }

        #region IXmlSerializable

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/Metadata");
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

        #region Deserialization Constructor

        protected ExitDecisionMetadata(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        public override void ResetModifiedFlag()
        {
            base.ResetModifiedFlag();
        }

        protected override bool CalculateModification()
        {
            return base.CalculateModification();
        }
    }
}

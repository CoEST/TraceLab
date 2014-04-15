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
using System.Xml;
using System.Xml.XPath;
using TraceLab.Core.Experiments;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents scopes "nodes" - nodes that contain the subgraph with its own nodes.
    /// It may either scope for decision, or scope for loop.
    /// </summary>
    [Serializable]
    public abstract class ScopeBaseMetadata : CompositeComponentBaseMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeBaseMetadata"/> class.
        /// </summary>
        protected ScopeBaseMetadata()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeBaseMetadata"/> class.
        /// </summary>
        /// <param name="compositeComponentGraph">The composite component graph.</param>
        /// <param name="label">The label.</param>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        internal ScopeBaseMetadata(CompositeComponentEditableGraph compositeComponentGraph, string label, string experimentLocationRoot)
            : this()
        {
            m_experimentLocationRoot = experimentLocationRoot;
            m_compositeComponentGraph = compositeComponentGraph;
            Label = label;
        }

        /// <summary>
        /// Initializes the component graph.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="settings">The settings.</param>
        public override void InitializeComponentGraph(CompositeComponentNode node, Settings.Settings settings)
        {
            //scope node just assigns itself to component graph
            ComponentGraph.OwnerNode = node;
            ComponentGraph.Settings = settings;
        }

        private CompositeComponentGraph m_compositeComponentGraph;
        /// <summary>
        /// Gets the component graph. 
        /// </summary>
        public override CompositeComponentGraph ComponentGraph
        {
            get
            {
                return m_compositeComponentGraph;
            }
        }

        /// <summary>
        /// Updates from definition.
        /// </summary>
        /// <param name="library">The library.</param>
        public override void UpdateFromDefinition(IComponentsLibrary library)
        {
            //just go over all nodes in the subgraph
            foreach (ExperimentNode node in ComponentGraph.Vertices)
            {
                if (node.Data != null && node.Data.Metadata != null)
                {
                    node.Data.Metadata.UpdateFromDefinition(library);
                }
            }
        }

        #region IXmlSerializable

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            ReadXmlInternal(reader);
        }

        protected XPathNavigator ReadXmlInternal(System.Xml.XmlReader reader)
        {
            IsInitialized = false;

            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/Metadata");
            Label = iter.GetAttribute("Label", String.Empty);

            //get experiment xml -> reading this xml is done in PostProcessReadXml
            iter = nav.SelectSingleNode("/Metadata/ComponentGraph");
            m_experimentXml = iter.InnerXml;

            return nav;
        }

        private string m_experimentXml;

        /// <summary>
        /// Process excuted once xml deserialization was completed.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        public override void PostProcessReadXml(Components.IComponentsLibrary library, string experimentLocationRoot)
        {
            m_experimentLocationRoot = experimentLocationRoot;

            if (m_experimentXml != null)
            {
                using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(m_experimentXml)))
                {
                    var experiment = TraceLab.Core.Experiments.ExperimentSerializer.DeserializeExperiment(reader, library);
                    m_compositeComponentGraph = new Experiments.CompositeComponentEditableGraph(experiment);
                }
                //clear variable
                m_experimentXml = null;
            }

            IsInitialized = true;
            IsModified = false;
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());
            writer.WriteAttributeString("Label", Label);

            writer.WriteStartElement("ComponentGraph");
            TraceLab.Core.Experiments.ExperimentSerializer.SerializeExperiment(writer, ComponentGraph);
            writer.WriteEndElement();
        }

        #endregion IXmlSerializable

        /// <summary>
        /// Performs a deep copy of the data in this object to another instance of the Metadata.
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(Metadata other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            base.CopyFrom(other);

            ScopeBaseMetadata metadata = (ScopeBaseMetadata)other;
            m_experimentLocationRoot = metadata.m_experimentLocationRoot;
            m_compositeComponentGraph = (CompositeComponentEditableGraph)(metadata.m_compositeComponentGraph.Clone());
            
            HasDeserializationError = metadata.HasDeserializationError;
            if (HasDeserializationError)
            {
                DeserializationErrorMessage = metadata.DeserializationErrorMessage;
            }
            
        }
    }
}

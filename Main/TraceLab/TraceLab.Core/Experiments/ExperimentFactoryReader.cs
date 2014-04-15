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
using System.Xml.XPath;
using TraceLab.Core.Components;
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// This factory is responsible for deserialization the experiment from xml file.
    /// It consists of method needed for QuickGraph.Serialization.SerializationExtensions.DeserializeFromXml.
    /// <seealso cref="TraceLab.Core.Experiments.ExperimentSerializer"/> for usage of it.
    /// </summary>
    internal class ExperimentFactoryReader
    {
        private Dictionary<string, ExperimentNode> m_vertices = new Dictionary<string, ExperimentNode>();
        private Dictionary<string, ExperimentNodeConnection> m_edges = new Dictionary<string, ExperimentNodeConnection>();
        private readonly System.Xml.Serialization.XmlSerializer m_nodeSerializer;
        private readonly System.Xml.Serialization.XmlSerializer m_nodeSerializerWithSize;
        protected Components.IComponentsLibrary m_library;
        protected string m_experimentLocationRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentFactoryReader"/> class.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="experimentLocationRoot">The experiment location root - it can be null in case the experiment belongs to composite component.</param>
        public ExperimentFactoryReader(Components.IComponentsLibrary library, IEnumerable<IPackageReference> references, string experimentLocationRoot)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            m_library = library.GetPackageAwareLibrary(references);
            m_experimentLocationRoot = experimentLocationRoot;

            //Create our own namespaces for the output
            var ns = new System.Xml.Serialization.XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");

            m_nodeSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(SerializedVertexData), null);
            m_nodeSerializerWithSize = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(SerializedVertexDataWithSize), null);

        }

        /// <summary>
        /// stores experiment guid of just being loaded experiment.
        /// </summary>
        private string experimentGuid;

        /// <summary>
        /// Construct the experiment from given xml path navigator to root experiment graph element.
        /// It also reads in Experiment Info.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public virtual Experiment GraphFactory(XPathNavigator reader)
        {
            Experiment flow = new Experiment();

            XPathNavigator experimentInfo = reader.SelectSingleNode("ExperimentInfo");

            if (experimentInfo != null)
            {
                var experimentInfoSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(ExperimentInfo), null);
                flow.ExperimentInfo = (ExperimentInfo)experimentInfoSerializer.Deserialize(experimentInfo.ReadSubtree());
            }
            else
            {
                flow.ExperimentInfo = new ExperimentInfo();
                flow.ExperimentInfo.Author = "<Author>";
                flow.ExperimentInfo.Name = "<Experiment name>";
                flow.ExperimentInfo.Contributors = "<Contributors>";
                flow.ExperimentInfo.Description = "<Description>";

                experimentGuid = flow.ExperimentInfo.Id;
            }

            return flow;
        }

        /// <summary>
        /// Constructs experiment node from given xml xpath navigator to the node.
        /// </summary>
        /// <param name="reader">The reader with a root to the node.</param>
        /// <returns>Experiment Node</returns>
        public ExperimentNode NodeFactory(XPathNavigator reader)
        {
            string id = reader.GetAttribute("id", String.Empty);

            SerializedVertexData nodeData = null;

            XPathNavigator serializedNodeData = reader.SelectSingleNode("SerializedVertexData");
            if (serializedNodeData != null)
            {
                nodeData = (SerializedVertexData)m_nodeSerializer.Deserialize(serializedNodeData.ReadSubtree());
                nodeData.PostProcessReadXml(m_library, m_experimentLocationRoot);
            }
            else
            {
                //it may be serialized vertex data with size 
                serializedNodeData = reader.SelectSingleNode("SerializedVertexDataWithSize");
                if (serializedNodeData != null)
                {
                    nodeData = (SerializedVertexDataWithSize)m_nodeSerializerWithSize.Deserialize(serializedNodeData.ReadSubtree());
                    nodeData.PostProcessReadXml(m_library, m_experimentLocationRoot);
                }
            }

            ExperimentNode vert = null;
            if (nodeData != null)
            {
                vert = NodeGenerator(id, nodeData);
                if(nodeData.Metadata != null) 
                {
                    if (nodeData.Metadata.HasDeserializationError)
                    {
                        vert.SetError(nodeData.Metadata.DeserializationErrorMessage);
                    }
                }
                m_vertices[id] = vert;
            }

            return vert;
        }

        /// <summary>
        /// Generates the correct node based on metadata contained in serialized vertex data
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="nodeData">The node data.</param>
        /// <returns>Experiment node</returns>
        private ExperimentNode NodeGenerator(string id, SerializedVertexData nodeData)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (nodeData == null)
                throw new ArgumentNullException("nodeData");

            ExperimentNode node;

            if (nodeData.Metadata is StartNodeMetadata)
            {
                node = new ExperimentStartNode(id, nodeData);
            }
            else if (nodeData.Metadata is EndNodeMetadata)
            {
                node = new ExperimentEndNode(id, nodeData);
            }
            else if (nodeData.Metadata is DecisionMetadata)
            {
                node = new ExperimentDecisionNode(id, nodeData);
            }
            else if (nodeData.Metadata is ScopeMetadata)
            {
                node = new ScopeNode(id, (SerializedVertexDataWithSize)nodeData);
            }
            else if (nodeData.Metadata is LoopScopeMetadata)
            {
                node = new LoopScopeNode(id, (SerializedVertexDataWithSize)nodeData);
            }
            else if (nodeData.Metadata is CompositeComponentMetadata)
            {
                node = new CompositeComponentNode(id, nodeData);
            }
            else if (nodeData.Metadata is ExitDecisionMetadata)
            {
                node = new ExitDecisionNode(id, nodeData);
            }
            else
            {
                ComponentNode componentNode = new ComponentNode(id, nodeData);
                node = componentNode;
            }

            return node;
        }

        /// <summary>
        /// Constructs experiment node connection from given xml xpath navigator to the edge.
        /// </summary>
        /// <param name="reader">The reader with edge root.</param>
        /// <returns>experiment node connection</returns>
        public ExperimentNodeConnection EdgeFactory(XPathNavigator reader)
        {
            string id = reader.GetAttribute("id", String.Empty);
            string source = reader.GetAttribute("source", String.Empty);
            string target = reader.GetAttribute("target", String.Empty);
            
            //try read is fixed attribute
            string isFixedAttrib = reader.GetAttribute("isFixed", String.Empty);
            bool isFixed;
            if (!Boolean.TryParse(isFixedAttrib, out isFixed)) isFixed = false;

            //try read is visible attribute
            string isVisibleAttrib = reader.GetAttribute("isVisible", String.Empty);
            bool isVisible;
            if (!Boolean.TryParse(isVisibleAttrib, out isVisible)) isVisible = true;

            //validate
            if (m_vertices.ContainsKey(source) == false || m_vertices.ContainsKey(target) == false)
            {
                throw new TraceLab.Core.Exceptions.ExperimentLoadException("The experiment is corrupted and could not be loaded. Experiment xml contains edge that refers to non-existing nodes.");
            }
            ExperimentNode sourceVert = m_vertices[source];
            ExperimentNode targetVert = m_vertices[target];

            ExperimentNodeConnection edge = new ExperimentNodeConnection(id, sourceVert, targetVert, isFixed, isVisible);
            edge.IsModified = false;
            m_edges[id] = edge;

            //read in route points from xml
            edge.RoutePoints.ReadXml(reader.ReadSubtree());

            //perform fixes for scopes 
            ScopeNodeHelper.TryFixScopeDecisionEntryAndExitNodes(sourceVert, targetVert);
            
            return edge;
        }
    }
}

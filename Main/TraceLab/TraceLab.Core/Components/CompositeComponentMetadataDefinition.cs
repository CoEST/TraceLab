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
using System.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Collections.Generic;
using System.Security.Permissions;

namespace TraceLab.Core.Components
{
    [Serializable]
    public class CompositeComponentMetadataDefinition : MetadataDefinition, IMetadataWithIOSpecDefinition, System.Xml.Serialization.IXmlSerializable
    {
        #region Constructor

        private CompositeComponentMetadataDefinition()
        {
            IOSpecDefinition = new IOSpecDefinition();
            ConfigurationWrapperDefinition = new ConfigWrapperDefinition(false, null);
        }

        public CompositeComponentMetadataDefinition(string id)
            : base(id)
        {
            IOSpecDefinition = new IOSpecDefinition();
            ConfigurationWrapperDefinition = new ConfigWrapperDefinition(false, null);
        }


        /// <summary>
        /// Initializes a new s_instance of the <see cref="CompositeComponentMetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="componentSourceFile">The component source file.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="label">The label.</param>
        /// <param name="version">The version.</param>
        /// <param name="description">The description.</param>
        /// <param name="author">The author.</param>
        public CompositeComponentMetadataDefinition(string id,
                TraceLab.Core.Experiments.CompositeComponentGraph componentGraph,
                string componentSourceFile, string componentName, string label, string version, string description, string author, 
                ComponentTags tags, List<DocumentationLink> documentationLinks)
            : base(id, componentSourceFile, componentName, label, version, description, author, tags, documentationLinks)
        {
            ComponentGraph = componentGraph;
            IOSpecDefinition = new IOSpecDefinition();
            ConfigurationWrapperDefinition = new ConfigWrapperDefinition(false, null);
        }

        /// <summary>
        /// Represents version of xml 
        /// </summary>
        private static int xmlVersion = 2;

        public IOSpecDefinition IOSpecDefinition
        {
            get;
            set;
        }

        public ConfigWrapperDefinition ConfigurationWrapperDefinition
        {
            get;
            set;
        }

        [NonSerialized]
        public TraceLab.Core.Experiments.CompositeComponentGraph ComponentGraph;

        #endregion

        public IEnumerable<PackageSystem.PackageReference> References
        {
            get;
            private set;
        }

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

        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public void ReadXml(System.Xml.XmlReader reader)
        {
            int version = int.Parse(reader.GetAttribute("xmlVersion"));

            if (version != xmlVersion && version != 1) 
            {
                throw new InvalidOperationException("Version file not recognized");
            }
            
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = ReadComponentInfo(nav);

            if (version == xmlVersion)
            {
                ReadCurrentVersionIODefinitions(nav);
            }
            else if (version == 1)
            {
                ReadOldIODefinitions(nav);
            }

            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/ConfigDefinition");
            ConfigurationWrapperDefinition.ReadXml(iter.ReadSubtree());

            //get experiment xml -> reading this xml is done in PostProcessReadXml
            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/ComponentGraph");
            m_experimentXml = iter.InnerXml;

            //read tags
            Tags = new ComponentTags(ID);

            XPathNodeIterator tagsIterator = nav.Select("/CompositeComponentMetadataDefinition/Tags//Tag");

            while (tagsIterator.MoveNext())
            {
                string tagValue = tagsIterator.Current.Value;
                Tags.SetTag(tagValue, false);
            }
        }

        private void ReadCurrentVersionIODefinitions(XPathNavigator nav)
        {
            XPathNodeIterator inputItemsIterator = nav.Select("/CompositeComponentMetadataDefinition/Input//IOItemDefinition");

            while (inputItemsIterator.MoveNext())
            {
                IOItemDefinition item = new IOItemDefinition();
                item.ReadXml(inputItemsIterator.Current.ReadSubtree());
                IOSpecDefinition.Input.Add(item.Name, item);
            }

            XPathNodeIterator outputItemsIterator = nav.Select("/CompositeComponentMetadataDefinition/Output//IOItemDefinition");

            while (outputItemsIterator.MoveNext())
            {
                IOItemDefinition item = new IOItemDefinition();
                item.ReadXml(outputItemsIterator.Current.ReadSubtree());
                IOSpecDefinition.Output.Add(item.Name, item);
            }
        }

        [Obsolete]
        private void ReadOldIODefinitions(XPathNavigator nav)
        {
            XPathNodeIterator inputItemsIterator = nav.Select("/CompositeComponentMetadataDefinition/Input//InputItemDefinition");

            while (inputItemsIterator.MoveNext())
            {
                IOItemDefinition item = new IOItemDefinition();
                item.ReadXml(inputItemsIterator.Current.ReadSubtree());
                IOSpecDefinition.Input.Add(item.Name, item);
            }

            XPathNodeIterator outputItemsIterator = nav.Select("/CompositeComponentMetadataDefinition/Output//OutputItemDefinition");

            while (outputItemsIterator.MoveNext())
            {
                IOItemDefinition item = new IOItemDefinition();
                item.ReadXml(outputItemsIterator.Current.ReadSubtree());
                IOSpecDefinition.Output.Add(item.Name, item);
            }
        }

        private XPathNavigator ReadComponentInfo(XPathNavigator nav)
        {
            XPathNavigator iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/Info/ID");
            ID = iter.Value;

            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/Info/Version");
            Version = iter.Value;

            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/Info/Label");
            Label = iter.Value;

            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/Info/Name");
            Classname = iter.Value;

            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/Info/Author");
            Author = iter.Value;

            iter = nav.SelectSingleNode("/CompositeComponentMetadataDefinition/Info/Description");
            Description = iter.Value;

            return iter;
        }

        private string m_experimentXml;

        /// <summary>
        /// Determines the dependency.
        /// </summary>
        /// <returns>Returns list of composite components definitions that this component depends on</returns>
        public List<string> DetermineDependency()
        {
            List<string> dependencyList = new List<string>();
            if (m_experimentXml != null)
            {
                XmlReader reader = XmlReader.Create(new System.IO.StringReader(m_experimentXml));
                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();

                //search for all references to composite components
                string metadataType = typeof(CompositeComponentMetadata).ToString();
                XPathExpression expression = nav.Compile("//Metadata[starts-with(@type, '" + metadataType + "')]");
                XPathNodeIterator iter = nav.Select(expression);
                
                //if it finds any composite component that this component refers to, it has to be mark as dependency, since that composite components graph has to be loaded before
                while (iter.MoveNext())
                {
                    string dependencyComponentMetadataDefinitionID = iter.Current.GetAttribute("ComponentMetadataDefinitionID", String.Empty);
                    
                    //if it has not yet been added (there might have been two nodes referring to the same graph
                    if (dependencyList.Contains(dependencyComponentMetadataDefinitionID) == false)
                    {
                        dependencyList.Add(dependencyComponentMetadataDefinitionID);
                    }
                }
            }

            return dependencyList;
        }

        public void PostProcessReadXml(Components.ComponentsLibrary library, string dataRoot)
        {
            if (m_experimentXml != null)
            {
                using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(m_experimentXml)))
                {
                    var experiment = TraceLab.Core.Experiments.ExperimentSerializer.DeserializeExperiment(reader, library);
                    ComponentGraph = new Experiments.CompositeComponentGraph(experiment);
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("xmlVersion", xmlVersion.ToString());

            writer.WriteStartElement("Info");
            writer.WriteElementString("ID", ID);
            writer.WriteElementString("Version", Version);
            writer.WriteElementString("Label", Label);
            writer.WriteElementString("Name", Classname);
            writer.WriteElementString("Author", Author);
            writer.WriteElementString("Description", Description);
            writer.WriteEndElement();

            writer.WriteStartElement("Input");
            foreach (IOItemDefinition item in IOSpecDefinition.Input.Values)
            {
                item.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Output");
            foreach (IOItemDefinition item in IOSpecDefinition.Output.Values)
            {
                item.WriteXml(writer);
            }
            writer.WriteEndElement();

            ConfigurationWrapperDefinition.WriteXml(writer);

            writer.WriteStartElement("ComponentGraph");
            TraceLab.Core.Experiments.ExperimentSerializer.SerializeExperiment(writer, ComponentGraph);
            writer.WriteEndElement();

        }

        #endregion
    }
}

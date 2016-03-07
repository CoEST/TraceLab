using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// This factory is responsible for serialization the experiment to xml file.
    /// It consists of method needed for QuickGraph.Serialization.SerializationExtensions.SerializeToXml.
    /// <seealso cref="TraceLab.Core.Experiments.ExperimentSerializer"/> for usage of it.
    /// </summary>
    internal class ExperimentFactoryWriter
    {
        public ExperimentFactoryWriter()
        {
        }

        /// <summary>
        /// Writes all experiment graph attributes.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="flow">The flow.</param>
        public virtual void WriteGraphAttributes(XmlWriter writer, IExperiment flow)
        {
            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(path);

            PropertyDescriptorCollection propertiesToWrite = TypeDescriptor.GetProperties(flow);
            foreach (PropertyDescriptor property in propertiesToWrite)
            {
                WriteXmlAttribute(writer, property, flow);
            }

            writer.WriteStartElement("References");
            {
                if (flow.References != null)
                {
                    var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(PackageSystem.PackageReference), null);
                    foreach (IPackageReference reference in flow.References)
                    {
                        serializer.Serialize(writer, reference);
                    }
                }
            }
            writer.WriteEndElement();

            // Iterate again and write any elements that are pending.  ALL attributes must be done first, or it results in invalid XML
            foreach (PropertyDescriptor property in propertiesToWrite)
            {
                WriteXmlElement(writer, property, flow);
            }
        }

        /// <summary>
        /// Writes the node attributes.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="flow">The flow.</param>
        public void WriteNodeAttributes(XmlWriter writer, ExperimentNode flow)
        {
            WriteXmlAttributesAndElements(writer, flow);
        }

        /// <summary>
        /// Writes the edge attributes.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="flow">The flow.</param>
        public void WriteEdgeAttributes(XmlWriter writer, ExperimentNodeConnection flow)
        {
            WriteXmlAttributesAndElements(writer, flow);
        }

        #region Write Xml Attributes and Elements

        /// <summary>
        /// Helper method for writing the XML attributes and elements.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="flow">The flow.</param>
        private void WriteXmlAttributesAndElements(XmlWriter writer, object flow)
        {
            PropertyDescriptorCollection propertiesToWrite = TypeDescriptor.GetProperties(flow);
            foreach (PropertyDescriptor property in propertiesToWrite)
            {
                WriteXmlAttribute(writer, property, flow);
            }

            // Iterate again and write any elements that are pending.  ALL attributes must be done first, or it results in invalid XML
            foreach (PropertyDescriptor property in propertiesToWrite)
            {
                WriteXmlElement(writer, property, flow);
            }
        }

        /// <summary>
        /// Helper method for writing the XML attribute.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="property">The property.</param>
        /// <param name="flow">The flow.</param>
        private void WriteXmlAttribute(XmlWriter writer, PropertyDescriptor property, object flow)
        {
            IEnumerable<System.Xml.Serialization.XmlAttributeAttribute> attribs = property.Attributes.OfType<System.Xml.Serialization.XmlAttributeAttribute>();
            System.Xml.Serialization.XmlAttributeAttribute xmlAttrib = attribs.ElementAtOrDefault(0);
            if (xmlAttrib != null)
            {
                object prop = property.GetValue(flow);
                if (prop != null)
                {
                    writer.WriteStartAttribute(xmlAttrib.AttributeName);
                    writer.WriteValue(prop.ToString());
                    writer.WriteEndAttribute();
                }
            }
        }

        /// <summary>
        /// Helper method for writing the XML element.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="property">The property.</param>
        /// <param name="flow">The flow.</param>
        private void WriteXmlElement(XmlWriter writer, PropertyDescriptor property, object flow)
        {
            IEnumerable<System.Xml.Serialization.XmlElementAttribute> attribs = property.Attributes.OfType<System.Xml.Serialization.XmlElementAttribute>();
            System.Xml.Serialization.XmlElementAttribute xmlAttrib = attribs.ElementAtOrDefault(0);
            if (xmlAttrib != null)
            {
                object prop = property.GetValue(flow);
                if (prop != null)
                {
                    var serializer = Serialization.XmlSerializerFactory.GetSerializer(prop.GetType(), null);
                    serializer.Serialize(writer, prop);
                }
            }
        }

        #endregion Write Xml Attributes and Elements
    }
}

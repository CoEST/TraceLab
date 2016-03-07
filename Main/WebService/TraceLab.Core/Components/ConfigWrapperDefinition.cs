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
using System.ComponentModel;
using System.Xml.XPath;
using System.Security.Permissions;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// ConfigWrapperDefinition defines a wrapper for the configuration properties of the custom config object for the component. 
    /// It doesn't store list of PropertyInfo or PropertyDescriptor, because these classes couldn't cross the AppDomain boundaries, while scanning components
    /// for metadata. 
    /// 
    /// Notice, that ConfigWrapperDefinition defines the config, but does not store values. ConfigWrapper is the model object generated based on 
    /// ConfigWrapperDefinition, which stores actual values. 
    /// </summary>
    [Serializable]
    public class ConfigWrapperDefinition : System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// List of properties information.
        /// </summary>
        internal IDictionary<string, ConfigPropertyObject> Properties
        {
            get;
            private set;
        }

        private ConfigWrapperDefinition() { }

        internal ConfigWrapperDefinition(bool isJava, string configurationTypeFullName)
        {
            Properties = new Dictionary<string, ConfigPropertyObject>();
            IsJava = isJava;
            ConfigurationTypeFullName = configurationTypeFullName;
        }

        /// <summary>
        /// Gets the full name of the configuration type based on which the wrapper has been created.
        /// </summary>
        /// <value>
        /// The full name of the configuration type.
        /// </value>
        internal String ConfigurationTypeFullName
        {
            get;
            private set;
        }

        internal void AddProperties(System.ComponentModel.PropertyDescriptorCollection properties)
        {
            foreach (System.ComponentModel.PropertyDescriptor propertyDescriptor in properties)
            {
                object defaultValue = null;
                // If this is a string, default it to an empty string (instead of the system default of null)
                if (propertyDescriptor.PropertyType == typeof(string))
                {
                    defaultValue = string.Empty;
                }
                // Otherwise, if it's value type, default it to whatever the default of that object is.
                else if (propertyDescriptor.PropertyType.IsValueType)
                {
                    defaultValue = Activator.CreateInstance(propertyDescriptor.PropertyType);
                }

                // If the user specified a default, then use that.
                DefaultValueAttribute defaultAttribute = propertyDescriptor.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                if (defaultAttribute != null)
                {
                    defaultValue = defaultAttribute.Value;
                }

                if (defaultValue != null && defaultValue.GetType().IsEnum)
                {
                    //throw new NotSupportedException("Component configuration that contain enumerations are not supported.");
                    //string value = defaultValue.GetType().GetEnumName(defaultValue);
                    //string[] names = defaultValue.GetType().GetEnumNames();
                }

                Properties.Add(propertyDescriptor.Name, new ConfigPropertyObject(propertyDescriptor.Name, defaultValue, propertyDescriptor.PropertyType, propertyDescriptor.DisplayName, propertyDescriptor.Description));
            }
        }

        internal void AddProperty(string propertyName, string propertyTypeFullName, string assemblyQualifiedName, string displayName, string description)
        {
            Properties.Add(propertyName, new ConfigPropertyObject(propertyName, null, propertyTypeFullName, assemblyQualifiedName, false, displayName, description));
        }

        internal void AddProperty(ConfigPropertyObject propertyObject)
        {
            Properties.Add(propertyObject.Name, propertyObject);
        }

        internal void RemoveProperty(string propertyName)
        {
            if (Properties.ContainsKey(propertyName) == true)
            {
                Properties.Remove(propertyName);
            }
        }

        internal bool IsJava
        {
            get;
            set;
        }

        internal void Clear()
        {
            Properties.Clear();
        }

        #region Equals_HashCode

        public override bool Equals(object obj)
        {
            ConfigWrapperDefinition other = obj as ConfigWrapperDefinition;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= object.Equals(IsJava, other.IsJava);
            isEqual &= TraceLab.Core.Utilities.CollectionsHelper.DictionaryContentEquals<string, ConfigPropertyObject>(Properties, other.Properties);

            return isEqual;
        }

        public override int GetHashCode()
        {
            int javaHash = IsJava.GetHashCode();
            int configValuesHash = Properties.GetHashCode();

            return javaHash ^ configValuesHash;
        }

        #endregion

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
        [SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, ControlThread = true)]
        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/ConfigDefinition/IsJava");
            string isJavaString = iter.Value;
            bool tmpIsJava;
            if (isJavaString != null && bool.TryParse(isJavaString, out tmpIsJava))
            {
                IsJava = tmpIsJava;
            }

            XPathNodeIterator propertyObjectsIterator = nav.Select("/ConfigDefinition//PropertyObject");
            while (propertyObjectsIterator.MoveNext())
            {
                ConfigPropertyObject propertyObject = new ConfigPropertyObject();
                propertyObject.ReadXml(propertyObjectsIterator.Current.ReadSubtree());
                Properties.Add(propertyObject.Name, propertyObject);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartElement("ConfigDefinition");

            writer.WriteElementString("IsJava", IsJava.ToString(System.Globalization.CultureInfo.CurrentCulture));
            writer.WriteStartElement("ConfigProperties");
            foreach (ConfigPropertyObject propertyObject in Properties.Values)
            {
                propertyObject.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion
    }
}

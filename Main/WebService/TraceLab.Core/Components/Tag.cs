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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml;

namespace TraceLab.Core.Components
{
    public enum TagType
    {
        Additive,
        Subtractive
    }

    public class TagChangedEventArgs : EventArgs
    {
        public TagChangedEventArgs(string tag)
        {
            Tag = tag;
        }

        public string Tag
        {
            get;
            private set;
        }
    }

    [Serializable]
    public class TagValueCollection : KeyedCollection<string, TagValue>, IXmlSerializable
    {
        private const int Version = 1;

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        protected override string GetKeyForItem(TagValue item)
        {
            return item.Value;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (reader.LocalName == "TagValueCollection")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/TagValueCollection/@Version");
                if (iter == null)
                    throw new XmlSchemaException("TagValueCollection does not have a version element");

                long ver = iter.ValueAsLong;
                if (ver == Version)
                {
                    ReadCurrentVersion(nav);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            XPathNodeIterator nodes = nav.Select("/TagValueCollection/TagValue");
            while (nodes.MoveNext())
            {
                XPathNavigator current = nodes.Current;

                XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TagValue), null);
                Add((TagValue)serial.Deserialize(current.ReadSubtree()));
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            writer.WriteAttributeString("Version", Version.ToString());


            XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TagValue), null);
            foreach (TagValue value in this)
            {
                serial.Serialize(writer, value);
            }
        }

        #endregion
    }

    [Serializable]
    public class ComponentTagCollection : KeyedCollection<string, ComponentTags>, IXmlSerializable
    {
        private const int Version = 1;

        protected override string GetKeyForItem(ComponentTags item)
        {
            return item.ComponentDefinitionId;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (reader.LocalName == "ComponentTagCollection")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/ComponentTagCollection/@Version");
                if (iter == null)
                    throw new XmlSchemaException("ComponentTagCollection does not have a version element");

                long ver = iter.ValueAsLong;
                if (ver == Version)
                {
                    ReadCurrentVersion(nav);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            XPathNodeIterator nodes = nav.Select("/ComponentTagCollection/ComponentTags");
            while (nodes.MoveNext())
            {
                XPathNavigator current = nodes.Current;

                XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(ComponentTags), null);
                Add((ComponentTags)serial.Deserialize(current.ReadSubtree()));
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            writer.WriteAttributeString("Version", Version.ToString());


            XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(ComponentTags), null);
            foreach (ComponentTags value in this)
            {
                serial.Serialize(writer, value);
            }
        }

        #endregion
    }

    [Serializable]
    public class ComponentTags : IXmlSerializable
    {
        private const int Version = 1;
        private ComponentTags()
        {
        }

        public ComponentTags(string definitionId)
        {
            ComponentDefinitionId = definitionId;
        }

        private string m_componentDefinitionId;
        public string ComponentDefinitionId
        {
            get { return m_componentDefinitionId; }
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                m_componentDefinitionId = value.Trim(); 
            }
        }

        private TagValueCollection m_values = new TagValueCollection();
        public ReadOnlyCollection<string> Values
        {
            get
            {
                List<string> newStrings = new List<string>();
                foreach (TagValue val in m_values)
                {
                    if (val.Type == TagType.Additive)
                    {
                        newStrings.Add(val.Value);
                    }
                }
                return new ReadOnlyCollection<string>(newStrings);
            }
        }

        /// <summary>
        /// Sets the tag.
        /// </summary>
        /// <param name="tag">The tag to set.  Tags should a preceeding '+' to indicate an add, or a preceeding - to indicate a remove.</param>
        /// <param name="isUserTag">if set to <c>true</c> then this tag was set by the user.</param>
        public void SetTag(string tag, bool isUserTag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Tag must be non-null and non-whitespace.", "tag");

            TagType type;
            if (tag[0] == '-')
            {
                type = TagType.Subtractive;
                tag = tag.Substring(1);
            }
            else if (tag[0] == '+')
            {
                type = TagType.Additive;
                tag = tag.Substring(1);
            }
            else
            {
                // Assume additive, with no characters to remove.
                type = TagType.Additive;
            }

            TagValue oldValue = null;
            if (m_values.Contains(tag))
            {
                oldValue = m_values[tag];
            }
            if (oldValue == null || oldValue.Type != type)
            {
                TagValue newValue = new TagValue(tag, type, isUserTag);

                if (oldValue != null)
                {
                    m_values.Remove(tag);
                }

                // If it did NOT previously exist AND this is a Subtractive type tag, then do NOT add it.
                // This is implemented via a negation of the previous line, though that is the logic that we want.
                if (type == TagType.Additive || oldValue != null)
                {
                    m_values.Add(newValue);

                    if (type == TagType.Additive)
                    {
                        FireTagAdded(tag);
                    }
                    else if (type == TagType.Subtractive)
                    {
                        FireTagRemoved(tag);
                    }
                }
            }
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (reader.LocalName == "ComponentTags")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/ComponentTags/@Version");
                if (iter == null)
                    throw new XmlSchemaException("ComponentTags does not have a version element");

                long ver = iter.ValueAsLong;
                if (ver == Version)
                {
                    ReadCurrentVersion(nav);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                throw new XmlSchemaException();
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            if (nav == null)
                throw new ArgumentNullException("nav");

            XPathNavigator iter = nav.SelectSingleNode("/ComponentTags/ComponentDefinitionId");
            if (iter == null)
                throw new XmlSchemaException();

            ComponentDefinitionId = iter.Value;

            iter = nav.SelectSingleNode("/ComponentTags/TagValueCollection");
            if (iter == null)
                throw new XmlSchemaException();

            XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TagValueCollection), null);
            var collection = (TagValueCollection)serial.Deserialize(iter.ReadSubtree());

            // These latest items take priority over anything that existed previously by the same name.
            foreach (TagValue tag in collection)
            {
                m_values.Remove(tag.Value);
                m_values.Add(tag);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteAttributeString("Version", Version.ToString());
            writer.WriteElementString("ComponentDefinitionId", ComponentDefinitionId);

            XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TagValueCollection), null);
            TagValueCollection collection = new TagValueCollection();
            foreach (TagValue value in m_values)
            {
                //if (value.IsUserTag)
                    collection.Add(value);
            }

            serial.Serialize(writer, collection);
            collection.Clear();
        }

        #endregion

        public void ApplyOverrides(ComponentTags overrides)
        {
            if (overrides == null)
                throw new ArgumentNullException("overrides");
            if (string.Equals(overrides.ComponentDefinitionId, this.ComponentDefinitionId, StringComparison.CurrentCultureIgnoreCase) == false)
                throw new ArgumentException("Cannot provide overrides from another component.", "overrides");

            foreach(TagValue value in overrides.m_values)
            {
                SetTag(value.GetCookedValue(), value.IsUserTag);
            }
        }

        public ComponentTags GetUserTags()
        {
            var tags = new ComponentTags(m_componentDefinitionId);
            foreach (TagValue value in m_values)
            {
                if (value.IsUserTag)
                {
                    tags.m_values.Add(value);
                }
            }

            return tags;
        }

        [NonSerialized]
        private EventHandler<TagChangedEventArgs> m_TagAdded;
        public event EventHandler<TagChangedEventArgs> TagAdded
        {
            add { m_TagAdded += value; }
            remove { m_TagAdded -= value; }
        }

        [NonSerialized]
        private EventHandler<TagChangedEventArgs> m_TagRemoved;
        public event EventHandler<TagChangedEventArgs> TagRemoved
        {
            add { m_TagRemoved += value; }
            remove { m_TagRemoved -= value; }
        }

        void FireTagAdded(string tag)
        {
            if (m_TagAdded != null)
                m_TagAdded(this, new TagChangedEventArgs(tag));
        }

        void FireTagRemoved(string tag)
        {
            if (m_TagRemoved != null)
                m_TagRemoved(this, new TagChangedEventArgs(tag));
        }
    }

    [Serializable]
    public class TagValue : IXmlSerializable, INotifyPropertyChanged
    {
        private const int Version = 1;

        private TagValue()
        {
        }

        public TagValue(string value, TagType type, bool isUser)
        {
            Value = value;
            Type = type;
            IsUserTag = isUser;
        }

        private string m_value;
        public string Value
        {
            get { return m_value; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tag newValue must be non-whitespace.", "newValue");

                m_value = value;
            }
        }

        [XmlIgnore]
        public bool IsUserTag
        {
            get;
            set;
        }

        [XmlIgnore]
        public TagType Type
        {
            get;
            private set;
        }

        internal void SetCookedValue(string cookedValue)
        {
            TagType type;
            if (cookedValue[0] == '-')
            {
                type = TagType.Subtractive;
                cookedValue = cookedValue.Substring(1);
            }
            else if (cookedValue[0] == '+')
            {
                type = TagType.Additive;
                cookedValue = cookedValue.Substring(1);
            }
            else
            {
                // Assume additive, with no characters to remove.
                type = TagType.Additive;
            }

            Value = cookedValue;
            Type = type;
        }

        internal string GetCookedValue()
        {
            string cooked = null;
            if (Type == TagType.Additive)
            {
                cooked = "+" + Value;
            }
            else
            {
                cooked = "-" + Value;
            }

            return cooked;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (reader.LocalName == "TagValue")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/TagValue/@Version");
                if (iter == null)
                    throw new XmlSchemaException("TagValue does not have a version element");

                long ver = iter.ValueAsLong;
                if (ver == Version)
                {
                    ReadCurrentVersion(nav);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            XPathNavigator iter = nav.SelectSingleNode("/TagValue/@IsUserTag");
            if (iter != null)
                IsUserTag = true;

            iter = nav.SelectSingleNode("/TagValue/Value");
            if (iter == null)
                throw new XmlSchemaException("TagValue does not have a Value element");

            var cookedValue = iter.Value;
            SetCookedValue(cookedValue);
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteAttributeString("Version", Version.ToString());
            if (IsUserTag)
                writer.WriteAttributeString("IsUserTag", IsUserTag.ToString());
            writer.WriteElementString("Value",  GetCookedValue());
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

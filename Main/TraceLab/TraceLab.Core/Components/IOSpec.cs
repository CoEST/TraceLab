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
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Utilities;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// IOSpec represents data structure that contains collections of input, output.
    /// </summary>
    [Serializable]
    public class IOSpec : INotifyPropertyChanged, IXmlSerializable, IModifiable
    {
        private static readonly int Version = 2;

        #region Constructor

        /// <summary>
        /// Initializes a new s_instance of the <see cref="IOSpec"/> class.
        /// </summary>
        internal IOSpec() 
        { 
            m_input = new ObservableDictionary<string, IOItem>();
            m_output = new ObservableDictionary<string, IOItem>();
            ((INotifyCollectionChanged)m_input).CollectionChanged += new NotifyCollectionChangedEventHandler(IOSpec_CollectionChanged);
            ((INotifyCollectionChanged)m_output).CollectionChanged += new NotifyCollectionChangedEventHandler(IOSpec_CollectionChanged);
            
        }

        /// <summary>
        /// Initializes a new s_instance of the <see cref="IOSpec"/> class.
        /// </summary>
        /// <param name="iOSpec">The IO spec definition.</param>
        public IOSpec(IOSpecDefinition iOSpec) : this() {
            BuildInputDictionary(iOSpec);
            BuildOutputDictionary(iOSpec);
        }

        protected IOSpec(IOSpec other) : this()
        {
            if (other == null)
                throw new ArgumentNullException("other");

            foreach(KeyValuePair<string,IOItem> pair in other.m_input)
            {
                m_input[pair.Key] = new IOItem(pair.Value.IOItemDefinition, pair.Value.MappedTo);
            }

            foreach (KeyValuePair<string, IOItem> pair in other.m_output)
            {
                m_output[pair.Key] = new IOItem(pair.Value.IOItemDefinition, pair.Value.MappedTo);
            }
        }
                
        public IOSpec Clone()
        {
            return new IOSpec(this);
        }

        #endregion

        #region Collection and IOItem changed handlers

        /// <summary>
        /// Handles the CollectionChanged event of the IOSpec observable colection.
        /// Anytime the new item is added or removed or replaced in the collection, it attaches/detaches the event listener, that changes IsModified flag.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void IOSpec_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    foreach (KeyValuePair<string, IOItem> ioItem in e.NewItems)
                    {
                        ioItem.Value.PropertyChanged += IOItem_PropertyChanged;
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    foreach (KeyValuePair<string, IOItem> ioItem in e.OldItems)
                    {
                        ioItem.Value.PropertyChanged -= IOItem_PropertyChanged;
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:

                    foreach (KeyValuePair<string, IOItem> ioItem in e.OldItems)
                    {
                        ioItem.Value.PropertyChanged -= IOItem_PropertyChanged;
                    }

                    foreach (KeyValuePair<string, IOItem> ioItem in e.NewItems)
                    {
                        ioItem.Value.PropertyChanged += IOItem_PropertyChanged;
                    }

                    break;

            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the IOItem. If IOItem was modified it changed IsModified flag to true.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void IOItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                IsModified = true;
            }
        }

        #endregion

        #region Properties Inputs Outputs

        private ObservableDictionary<string, IOItem> m_input;
        /// <summary>
        /// Gets or sets the input dictionary of input items. The key is input item name.
        /// </summary>
        /// <value>
        /// The input dictionary.
        /// </value>
        public ObservableDictionary<string, IOItem> Input
        {
            get
            {
                return m_input;
            }
            set
            {
                if (m_input != value)
                {
                    m_input = value;
                    OnPropertyChanged("Input");
                }
            }
        }

        private ObservableDictionary<string, IOItem> m_output;
        /// <summary>
        /// Gets or sets the output dictionary of output items. The key is output item name.
        /// </summary>
        /// <value>
        /// The output dictionary.
        /// </value>
        public ObservableDictionary<string, IOItem> Output
        {
            get
            {
                return m_output;
            }
            set
            {
                if (m_output != value)
                {
                    m_output = value;
                    OnPropertyChanged("Output");
                }
            }
        }

        #endregion

        #region Is Modified

        private bool m_isModified;
        private bool m_isDeferredModify;
        public bool IsModified
        {
            get
            {
                if (m_isDeferredModify)
                {
                    m_isModified = CalculateModification();
                    m_isDeferredModify = false;
                }

                return m_isModified; 
            }
            set
            {
                if (m_isModified != value)
                {
                    m_isDeferredModify = true;
                    OnPropertyChanged("IsModified");
                }
            }
        }

        private bool CalculateModification()
        {
            bool isModified = false;
            if (!isModified)
            {
                foreach (KeyValuePair<string, IOItem> pair in m_input)
                {
                    isModified |= m_input[pair.Key].IsModified;
                    if (isModified)
                        break;
                }
            }

            if (!isModified)
            {
                foreach (KeyValuePair<string, IOItem> pair in m_output)
                {
                    isModified |= m_output[pair.Key].IsModified;
                    if (isModified)
                        break;
                }
            }
                        
            return isModified;
        }

        public void ResetModifiedFlag()
        {
            foreach (KeyValuePair<string, IOItem> pair in m_output)
            {
                pair.Value.ResetModifiedFlag();
            }
            
            foreach (KeyValuePair<string, IOItem> pair in m_input)
            {
                pair.Value.ResetModifiedFlag();
            }

            IsModified = false;
        }

        #endregion

        #region Highlighting 
                
        /// <summary>
        /// Highlights the inputs and outputs whose mapping matches the given value
        /// </summary>
        /// <param name="name">The name.</param>
        public void HighlightIO(string value)
        {
            foreach (IOItem item in Input.Values)
            {
                if (item.MappedTo.Equals(value))
                {
                    item.IsHighlighted = true;
                    IsInputHighlighted = true;
                    break;
                }
            }

            foreach (IOItem item in Output.Values)
            {
                if (item.MappedTo.Equals(value))
                {
                    item.IsHighlighted = true;
                    IsOutputHighlighted = true;
                    break;
                }
            }
        }

        public void ClearHightlightIO()
        {   
            IsInputHighlighted = false;
            IsOutputHighlighted = false;
            foreach (IOItem item in Input.Values)
            {
                item.IsHighlighted = false;
            }

            foreach (IOItem item in Output.Values)
            {
                item.IsHighlighted = false;
            }
        }

        private bool m_isInputHighlighted;

        /// <summary>
        /// Gets or sets a value indicating whether input is highlighted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if input is highlighted; otherwise, <c>false</c>.
        /// </value>
        public bool IsInputHighlighted
        {
            get { return m_isInputHighlighted; }
            private set 
            {
                if (m_isInputHighlighted != value)
                {
                    m_isInputHighlighted = value;
                    OnPropertyChanged("IsInputHighlighted");
                }
            }
        }

        private bool m_isOutputHighlighted;

        /// <summary>
        /// Gets or sets a value indicating whether output is highlighted
        /// </summary>
        /// <value>
        /// 	<c>true</c> if output is highlighted; otherwise, <c>false</c>.
        /// </value>
        public bool IsOutputHighlighted
        {
            get { return m_isOutputHighlighted; }
            private set
            {
                if (m_isOutputHighlighted != value)
                {
                    m_isOutputHighlighted = value;
                    OnPropertyChanged("IsOutputHighlighted");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries the get input mapping. The method checkes collections of inputs in search of mapping for the item.
        /// If item name has been first found in inputs collection it set mapping to 'Mapped To' of the found item.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>true if mapping has been found</returns>
        public bool TryGetInputMapping(string name, out string mapping)
        {
            bool success = false;
            mapping = String.Empty;

            IOItem inputItem;
            
            if (Input.TryGetValue(name, out inputItem))
            {
                success = true;
                mapping = inputItem.MappedTo;
            }
                
            return success;
        }

        /// <summary>
        /// Tries the get output. The method checkes collections of outputs in search of mapping for the item.
        /// If item name has been first found in outputs collection it set mapping to 'Output As' of the found item. 
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>true if mapping has been found</returns>
        public bool TryGetOutputMapping(string name, out string mapping)
        {
            bool success = false;
            mapping = String.Empty;

            IOItem outputItem;
            
            if (Output.TryGetValue(name, out outputItem))
            {
                success = true;
                mapping = outputItem.MappedTo;
            }
            
            return success;
        }

        /// <summary>
        /// Iterates through inputs, outputs, in given IOSpec and for matching items in current IOSpec updates mappings
        /// </summary>
        /// <param name="temporaryIOSpec"></param>
        public void UpdateMappingsBasedOn(IOSpec temporaryIOSpec)
        {
            foreach (IOItem tmpInputItem in temporaryIOSpec.Input.Values)
            {
                IOItem inputItem;
                if (Input.TryGetValue(tmpInputItem.IOItemDefinition.Name, out inputItem))
                {
                    //check if definitions are the same
                    if (inputItem.IOItemDefinition.Equals(tmpInputItem.IOItemDefinition))
                    {
                        inputItem.MappedTo = tmpInputItem.MappedTo;
                    }
                }
            }

            foreach (IOItem tmpOutputItem in temporaryIOSpec.Output.Values)
            {
                IOItem outputItem;
                if (Output.TryGetValue(tmpOutputItem.IOItemDefinition.Name, out outputItem))
                {
                    //check if definitions are the same
                    if (outputItem.IOItemDefinition.Equals(tmpOutputItem.IOItemDefinition))
                    {
                        outputItem.MappedTo = tmpOutputItem.MappedTo;
                    }
                }
            }
        }

        #region Private Methods - BuildDicionaries

        private void BuildInputDictionary(IOSpecDefinition iOSpec)
        {
            Input.Clear();

            foreach (IOItemDefinition itemDefinition in iOSpec.Input.Values)
            {
                var item = new IOItem(itemDefinition, itemDefinition.Name);
                Input.Add(itemDefinition.Name, item);
            }
        }

        private void BuildOutputDictionary(IOSpecDefinition iOSpec)
        {
            Output.Clear();

            foreach (IOItemDefinition itemDefinition in iOSpec.Output.Values)
            {
                var item = new IOItem(itemDefinition, itemDefinition.Name);
                Output.Add(itemDefinition.Name, item);
            }
        }

        #endregion

        #endregion

        #region Equals_HashCode

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            IOSpec other = obj as IOSpec;
            if (other == null)
                return false;

            bool isEqual = true;
            isEqual &= CollectionsHelper.DictionaryContentEquals<string, IOItem>(Output, other.Output);
            isEqual &= CollectionsHelper.DictionaryContentEquals<string, IOItem>(Input, other.Input);
            
            return isEqual;
        }

        public override int GetHashCode()
        {
            int outputHash = Output.GetHashCode();
            int inputHash = Input.GetHashCode();
            
            return outputHash ^ inputHash;
        }

        #endregion

        #region INotifyPropertyChanged

        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
            }
            remove
            {
                m_propertyChanged -= value;
            }
        }

        private void OnPropertyChanged(string property)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(property));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, e);
        }

        #endregion

        #region IXmlSerializable

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            String ver = reader.GetAttribute("Version");
            int version = 0;
            if (ver != null)
            {
                version = int.Parse(ver);
            }
            
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            if (version == Version)
            {
                ReadCurrentVersionIOSpec(nav);
            }
            else if (version == 1 || version == 0)
            {
                ReadOldIOSpec(nav);
            }
        }

        private void ReadCurrentVersionIOSpec(XPathNavigator nav)
        {
            XPathNodeIterator inputItemsIterator = nav.Select("/IOSpec/Input//IOItem");

            while (inputItemsIterator.MoveNext())
            {
                IOItem item = new IOItem();
                item.ReadXml(inputItemsIterator.Current.ReadSubtree());
                Input.Add(item.IOItemDefinition.Name, item);
            }

            XPathNodeIterator outputItemsIterator = nav.Select("/IOSpec/Output//IOItem");

            while (outputItemsIterator.MoveNext())
            {
                IOItem item = new IOItem();
                item.ReadXml(outputItemsIterator.Current.ReadSubtree());
                Output.Add(item.IOItemDefinition.Name, item);
            }
        }

        [Obsolete]
        private void ReadOldIOSpec(XPathNavigator nav)
        {
            XPathNodeIterator inputItemsIterator = nav.Select("/IOSpec/Input//InputItem");

            while (inputItemsIterator.MoveNext())
            {
                IOItem item = new IOItem();
                item.ReadXml(inputItemsIterator.Current.ReadSubtree());
                Input.Add(item.IOItemDefinition.Name, item);
            }

            XPathNodeIterator outputItemsIterator = nav.Select("/IOSpec/Output//OutputItem");

            while (outputItemsIterator.MoveNext())
            {
                IOItem item = new IOItem();
                item.ReadXml(outputItemsIterator.Current.ReadSubtree());
                Output.Add(item.IOItemDefinition.Name, item);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("IOSpec");

            writer.WriteAttributeString("Version", Version.ToString(CultureInfo.CurrentCulture));

            writer.WriteStartElement("Input");
            foreach (IOItem item in Input.Values)
            {
                item.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Output");
            foreach (IOItem item in Output.Values)
            {
                item.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion

        #region Static Helper methods

        /// <summary>
        /// Reads the IO spec.
        /// </summary>
        /// <param name="nav">A cursor in the xml data</param>
        /// <returns></returns>
        internal static IOSpec ReadIOSpec(XPathNavigator nav)
        {
            IOSpec temporaryIOSpec = new IOSpec();
            if (nav != null)
            {
                var reader = nav.ReadSubtree();
                reader.Read();
                temporaryIOSpec.ReadXml(reader);
            }
            return temporaryIOSpec;
        }

        #endregion
    }
}

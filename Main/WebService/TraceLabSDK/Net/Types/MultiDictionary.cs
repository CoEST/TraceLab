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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace TraceLabSDK
{
    /// <summary>
    /// A dictonary that implements a one-to-many lookup.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class MultiDictionary<TKey, TValue> 
        :   IDictionary<TKey, IEnumerable<TValue>>, 
            ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>, 
            IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>>, 
            IDictionary, ICollection, IEnumerable, 
            INotifyCollectionChanged, INotifyPropertyChanged, 
            IXmlSerializable, ISerializable
    {
        private Dictionary<TKey, List<TValue>> m_storage = new Dictionary<TKey, List<TValue>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public MultiDictionary()
        {
        }

        /// <summary>
        /// Adds the specified value to the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            List<TValue> values;
            if(m_storage.TryGetValue(key, out values) == false)
            {
                values = new List<TValue>();
                m_storage[key] = values;
            }

            values.Add(value);
            OnCollectionChanged(GetCollectionChangeArgs(NotifyCollectionChangedAction.Add, key, new TValue[]{value}));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
            OnPropertyChanged(new PropertyChangedEventArgs("Values"));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        /// <summary>
        /// Adds the range of values to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddRange(TKey key, IEnumerable<TValue> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            List<TValue> values;
            if(m_storage.TryGetValue(key, out values) == false)
            {
                values = new List<TValue>();
                m_storage[key] = values;
            }

            values.AddRange(value);
            OnCollectionChanged(GetCollectionChangeArgs(NotifyCollectionChangedAction.Add, key, value));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
            OnPropertyChanged(new PropertyChangedEventArgs("Values"));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Remove(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            List<TValue> values;
            if (m_storage.TryGetValue(key, out values) && values != null)
            {
                if (values.Remove(value))
                {
                    OnCollectionChanged(GetCollectionChangeArgs(NotifyCollectionChangedAction.Remove, key, new TValue[] { value }));
                    OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Values"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a set of values from a given key in the dictionary
        /// </summary>
        /// <param name="key">The key whose values you want to remove</param>
        /// <param name="value">The specific values to remove</param>
        /// <returns>Returns false if the key was not found in the dictionary, otherwise true.</returns>
        public bool Remove(TKey key, IEnumerable<TValue> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            List<TValue> values;
            if (m_storage.TryGetValue(key, out values) && values != null)
            {
                List<TValue> removed = new List<TValue>();
                foreach (TValue val in value)
                {
                    if (values.Remove(val))
                    {
                        removed.Add(val);
                    }
                }

                if (removed.Count > 0)
                {
                    OnCollectionChanged(GetCollectionChangeArgs(NotifyCollectionChangedAction.Remove, key, removed));
                    OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Values"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                }
                return true;
            }

            return false;
        }

        private static int CountEnumerable(IEnumerable val)
        {
            int i = 0;
            IEnumerator enumerator = val.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ++i;
            }
            return i;
        }

        /// <summary>
        /// Checks if a given set of values are associated with the given key
        /// </summary>
        /// <param name="key">The key to test</param>
        /// <param name="value">The values to check</param>
        /// <returns>Returns true if all the values are associate with the key</returns>
        public bool Contains(TKey key, IEnumerable<TValue> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            bool contains = false;
            List<TValue> values;
            if (m_storage.TryGetValue(key, out values) && values != null &&  CountEnumerable(value) > 0)
            {
                contains = true;
                foreach (TValue val in value)
                {
                    contains &= values.Contains(val);
                }
            }

            return contains;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///   </exception>
        public void Clear()
        {
            m_storage.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
            OnPropertyChanged(new PropertyChangedEventArgs("Values"));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///   </returns>
        public int Count
        {
            get { return m_storage.Count; }
        }

        #region IDictionary<TKey, IEnumberable<TValue>>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">
        /// An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
        ///   </exception>
        public void Add(TKey key, IEnumerable<TValue> value)
        {
            this.AddRange(key, value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
        ///   </exception>
        public bool ContainsKey(TKey key)
        {
            return m_storage.ContainsKey(key); ;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///   </returns>
        public ICollection<TKey> Keys
        {
            get { return m_storage.Keys; }
        }

        /// <summary>
        /// Removes all elements from a given key
        /// </summary>
        /// <param name="key">The key to remove from the dictionary</param>
        /// <returns>Returns true if the key was found and removed.  Returns false if the key was not found in the dictionary.</returns>
        public bool Remove(TKey key)
        {
            List<TValue> values;
            if (m_storage.TryGetValue(key, out values))
            {
                var args = GetCollectionChangeArgs(NotifyCollectionChangedAction.Remove, key, values);
                OnCollectionChanged(args);
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
                OnPropertyChanged(new PropertyChangedEventArgs("Values"));
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));

                return m_storage.Remove(key);
            }

            return false;
        }

        private NotifyCollectionChangedEventArgs GetCollectionChangeArgs(NotifyCollectionChangedAction action, TKey key, IEnumerable<TValue> values)
        {
            List<KeyValuePair<TKey, TValue>> valuePairs = new List<KeyValuePair<TKey, TValue>>();
            foreach (TValue val in values)
            {
                valuePairs.Add(new KeyValuePair<TKey, TValue>(key, val));
            }

            var args = new NotifyCollectionChangedEventArgs(action, valuePairs);
            return args;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
        ///   </exception>
        public bool TryGetValue(TKey key, out IEnumerable<TValue> value)
        {
            value = null;
            List<TValue> values;
            if (m_storage.TryGetValue(key, out values))
            {
                value = values;
            }

            return value != null;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        ///   </returns>
        public ICollection<IEnumerable<TValue>> Values
        {
            get
            {
                List<TValue>[] array = new List<TValue>[m_storage.Values.Count];
                m_storage.Values.CopyTo(array, 0);
                return array;
            }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        ///   </returns>
        ///   
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and <paramref name="key"/> is not found.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
        ///   </exception>
        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                return m_storage[key];
            }
            set
            {
                // If the incoming value is null, treat it as a removal.
                if (value == null)
                {
                    Remove(key);
                }
                else if (m_storage.ContainsKey(key))
                {
                    // If the storage already contains the key, then we need to create a list of the ones that are being removed vs
                    // the ones being added.
                    IEnumerable<TValue> oldValues = m_storage[key];
                    List<TValue> newValues = new List<TValue>(value);

                    m_storage[key] = newValues;

                    if (oldValues != null)
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValues, oldValues));
                    }
                    else
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newValues));
                    }

                    OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Values"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                }
                else
                {
                    // Otherwise, we're strictly adding it.
                    Add(key, value);
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,IEnumerable<TValue>>> Members

        void ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Add(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            this.Add(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Clear()
        {
            this.Clear();
        }

        bool ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Contains(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            return this.Contains(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.CopyTo(KeyValuePair<TKey, IEnumerable<TValue>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Count
        {
            get { return this.Count; }
        }


        bool ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Remove(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            return this.Remove(item.Key, item.Value);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,IEnumerable<TValue>>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> GetEnumerator()
        {
            return new Enumerator(m_storage.GetEnumerator());
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// An enumerator for iterating through a MultiDictionary.
        /// </summary>
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>>, IDisposable, IDictionaryEnumerator, IEnumerator
        {
            private KeyValuePair<TKey, IEnumerable<TValue>> m_current;
            private Dictionary<TKey, List<TValue>>.Enumerator m_enumerator;

            internal Enumerator(Dictionary<TKey, List<TValue>>.Enumerator enumerator)
            {
                m_current = default(KeyValuePair<TKey, IEnumerable<TValue>>);
                m_enumerator = enumerator;
            }

            // Summary:
            //     Gets the element at the current position of the enumerator.
            //
            // Returns:
            //     The element in the System.Collections.Generic.Dictionary<TKey,TValue> at
            //     the current position of the enumerator.
            KeyValuePair<TKey, IEnumerable<TValue>> IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>>.Current
            {
                get { return m_current; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                m_current = default(KeyValuePair<TKey, IEnumerable<TValue>>);
                m_enumerator = default(Dictionary<TKey, List<TValue>>.Enumerator);
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            ///   </exception>
            public bool MoveNext()
            {
                bool stillValid = m_enumerator.MoveNext();
                if (stillValid)
                {
                    KeyValuePair<TKey, List<TValue>> current = m_enumerator.Current;
                    m_current = new KeyValuePair<TKey, IEnumerable<TValue>>(current.Key, current.Value);
                }
                else
                {
                    m_current = default(KeyValuePair<TKey, IEnumerable<TValue>>);
                }

                return stillValid;
            }

            /// <summary>
            /// Gets the current.
            /// </summary>
            public KeyValuePair<TKey, IEnumerable<TValue>> Current
            {
                get { return m_current; }
            }

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            bool IEnumerator.MoveNext()
            {
                return this.MoveNext();
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator)m_enumerator).Reset();
                m_current = default(KeyValuePair<TKey, IEnumerable<TValue>>);
            }

            #endregion

            #region IDictionaryEnumerator Members

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get 
                { 
                    return ((IDictionaryEnumerator)m_enumerator).Entry;
                }
            }

            object IDictionaryEnumerator.Key
            {
                get 
                {
                    return ((IDictionaryEnumerator)m_enumerator).Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    return ((IDictionaryEnumerator)m_enumerator).Value;
                }
            }

            #endregion

            #region IEnumerator<KeyValuePair<TKey,IEnumerable<TValue>>> Members

            #endregion
        }


        #region IDictionary Members

        void IDictionary.Add(object key, object value)
        {
            Type valType = value.GetType();
            if (valType == typeof(TValue))
            {
                this.Add((TKey)key, (TValue)value);
            }
            else if (valType == typeof(IEnumerable<TValue>))
            {
                this.AddRange((TKey)key, (IEnumerable<TValue>)value);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void IDictionary.Clear()
        {
            this.Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return (IDictionaryEnumerator)this.GetEnumerator();
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return false; ; }
        }

        ICollection IDictionary.Keys
        {
            get { return (ICollection)this.Keys; }
        }

        void IDictionary.Remove(object key)
        {
            this.Remove((TKey)key);
        }

        ICollection IDictionary.Values
        {
            get { return (ICollection)this.Values; }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this[(TKey)key];
            }
            set
            {
                this[(TKey)key] = (IEnumerable<TValue>)value;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { return this.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        [NonSerialized]
        private NotifyCollectionChangedEventHandler m_collectionChanged;
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                m_collectionChanged += value;
            }
            remove
            {
                m_collectionChanged -= value;
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (m_collectionChanged != null)
                m_collectionChanged(this, e);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
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

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, e);
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TKey), null);
            var valueSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TValue), null);

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TKey), null);
            var valueSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TValue), null);

            foreach (TKey key in this.Keys)
            {
                IEnumerable<TValue> values = this[key];
                foreach (TValue value in values)
                {
                    writer.WriteStartElement("item");

                    writer.WriteStartElement("key");
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();

                    writer.WriteStartElement("value");
                    valueSerializer.Serialize(writer, value);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">
        /// The caller does not have the required permission.
        ///   </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Storage", m_storage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// 
        /// This is the serialization constructor.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected MultiDictionary(SerializationInfo info, StreamingContext context)
        {
            m_storage = (Dictionary<TKey, List<TValue>>)info.GetValue("Storage", typeof(Dictionary<TKey, List<TValue>>));
        }

        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable"/> interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2"/> instance is invalid.
        ///   </exception>
        public void OnDeserialization(Object sender)
        {
            m_storage.OnDeserialization(sender);
        }
    }
}


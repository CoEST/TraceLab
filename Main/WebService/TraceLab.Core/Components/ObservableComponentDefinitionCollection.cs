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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// An observable collection of ComponentDefinitions, indexable by definition ID
    /// </summary>
    internal sealed class ObservableComponentDefinitionCollection : KeyedCollection<string, MetadataDefinition>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableComponentDefinitionCollection"/> class.
        /// </summary>
        public ObservableComponentDefinitionCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableComponentDefinitionCollection"/> class.
        /// </summary>
        /// <param name="existingCollection">The existing collection.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if existingCollection is null.</exception>
        public ObservableComponentDefinitionCollection(IEnumerable<MetadataDefinition> existingCollection)
        {
            if (existingCollection == null)
                throw new ArgumentNullException("existingCollection");

            AddRange(existingCollection);
        }

        public void AddRange(IEnumerable<MetadataDefinition> existingCollection)
        {
            if (existingCollection == null)
                throw new ArgumentNullException("existingCollection");

            foreach (MetadataDefinition meta in existingCollection)
            {
                if (!Contains(meta.ID))
                {
                    Add(meta);
                }
            }
        }

        /// <summary>
        /// Extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="item"/> is null.</exception>
        protected override string GetKeyForItem(MetadataDefinition item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return item.ID;
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:TraceLab.Core.Components.ObservableComponentDefinitionCollection"/>.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:TraceLab.Core.Components.ObservableComponentDefinitionCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.-or-<paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="item"/> is null.</exception>
        protected override void InsertItem(int index, MetadataDefinition item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            base.InsertItem(index, item);

            RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            RaisePropertyChanged("Count");
            RaisePropertyChanged("Item[]");
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:TraceLab.Core.Components.ObservableComponentDefinitionCollection"/>.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            if (0 <= index && index < Count)
            {
                object item = this[index];
                base.RemoveItem(index);
                RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                RaisePropertyChanged("Count");
                RaisePropertyChanged("Item[]");
            }
        }

        /// <summary>
        /// Replaces the item at the specified index with the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be replaced.</param>
        /// <param name="item">The new item.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.-or-<paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="item"/> is null.</exception>
        protected override void SetItem(int index, MetadataDefinition item)
        {
            if (item == null)
                throw new ArgumentNullException("item", "Item cannot be null");
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException("index");

            object oldItem = null;
            if (0 <= index && index < Count)
            {
                oldItem = this[index];
            }

            base.SetItem(index, item);

            if (oldItem != null)
            {
                RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
            }
            else
            {
                RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                RaisePropertyChanged("Count");
            }

            RaisePropertyChanged("Item[]");
        }

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void RaiseCollectionChangedEvent(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }

    internal sealed class ReadonlyObservableComponentDefinitionCollection : ReadOnlyCollection<MetadataDefinition>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private ObservableComponentDefinitionCollection m_list;
        public ReadonlyObservableComponentDefinitionCollection(ObservableComponentDefinitionCollection list) : base(list)
        {
            m_list = list;
            list.CollectionChanged += HandleCollectionChanged;
            list.PropertyChanged += HandlePropertyChanged;
        }

        public MetadataDefinition this[string id]
        {
            get
            {
                return m_list[id];
            }
        }

        void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args);
        }

        /// <summary>
        /// Handles the collection changed by forwarding the notification as if it came from this collection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnCollectionChanged(args);
        }

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        // Summary:
        //     Raises the TraceLab.Core.ComponentsReadonlyObservableComponentDefinitionCollection.CollectionChanged
        //     event using the provided arguments.
        //
        // Parameters:
        //   args:
        //     Arguments of the event being raised.
        void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //
        // Summary:
        //     Raises the TraceLab.Core.ComponentsReadonlyObservableComponentDefinitionCollection.PropertyChanged
        //     event using the provided arguments.
        //
        // Parameters:
        //   args:
        //     Arguments of the event being raised.
        void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }

        #endregion

    }
}

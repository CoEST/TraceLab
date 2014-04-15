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
using System.ComponentModel;
using System.Collections.Specialized;

namespace TraceLab.Core.Utilities
{
    abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged
    {
        /// <summary>
        /// Removes all elements from the <see cref="T:TraceLab.Core.Utilities.ObservableKeyedCollection`2"/>.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            NotifyCollectionChanged(args);
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        /// -or-
        ///   <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        ///   </exception>
        protected override void InsertItem(int index, TItem item)
        {
            if (index < 0 || Count < index)
                throw new ArgumentOutOfRangeException("index");

            base.InsertItem(index, item);
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
            NotifyCollectionChanged(args);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/>.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        /// -or-
        ///   <paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        ///   </exception>
        protected override void RemoveItem(int index)
        {
            if(index < 0 ||Count <= index)
                throw new ArgumentOutOfRangeException("index");

            TItem item = this[index];
            base.RemoveItem(index);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
            NotifyCollectionChanged(args);
        }

        /// <summary>
        /// Replaces the item at the specified index with the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be replaced.</param>
        /// <param name="item">The new item.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        /// -or-
        ///   <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        ///   </exception>
        protected override void SetItem(int index, TItem item)
        {
            if (index < 0 || Count <= index)
                throw new ArgumentOutOfRangeException("index");

            TItem oldItem = this[index];
            base.SetItem(index, item);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index);
            NotifyCollectionChanged(args);
        }

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }

        #endregion
    }
}

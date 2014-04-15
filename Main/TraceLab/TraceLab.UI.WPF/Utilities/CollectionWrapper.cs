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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TraceLab.UI.WPF.Utilities
{
    public class CollectionWrapper<T> : System.Collections.Specialized.INotifyCollectionChanged, IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        private readonly System.Windows.Threading.Dispatcher m_dispatcher;
        private delegate void CollectionChangedDelegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);
        private readonly object m_collection;


        public CollectionWrapper(System.Windows.Threading.Dispatcher dispatcher, ReadOnlyObservableCollection<T> col)
        {
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            if (col == null)
                throw new ArgumentNullException("col");

            m_dispatcher = dispatcher;

            m_collection = col;
            // Setup our listener so we can forward notifications.
            var castCollection = col as System.Collections.Specialized.INotifyCollectionChanged;
            castCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(castCollection_CollectionChanged);
        }

        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

        #region Private handlers

        void castCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CollectionChangedDelegate changeHandler = OnCollectionChanged;
            m_dispatcher.EnsureThread(changeHandler, sender, e);
        }

        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(sender, e);
        }

        #endregion



        int IList<T>.IndexOf(T item)
        {
            return ((IList<T>)m_collection).IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            if (m_collection != null) ((IList<T>)m_collection).Insert(index, item);
        }

        void IList<T>.RemoveAt(int index)
        {
            ((IList<T>)m_collection).RemoveAt(index);
        }

        T IList<T>.this[int index]
        {
            get
            {
                return ((IList<T>)this.m_collection)[index];
            }
            set
            {
                ((IList<T>)this.m_collection)[index] = value;
            }
        }

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)this.m_collection).Add(item);
        }

        void ICollection<T>.Clear()
        {
            ((ICollection<T>)this.m_collection).Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
            return ((ICollection<T>)this.m_collection).Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)this.m_collection).CopyTo(array, arrayIndex);
        }

        int ICollection<T>.Count
        {
            get {
                return ((ICollection<T>)this.m_collection).Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get {
                return ((ICollection<T>)this.m_collection).IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            return ((ICollection<T>)this.m_collection).Remove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)this.m_collection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.m_collection).GetEnumerator();
        }

        int IList.Add(object value)
        {
            return ((IList)this.m_collection).Add(value);
        }

        void IList.Clear()
        {
            ((IList)this.m_collection).Clear();
        }

        bool IList.Contains(object value)
        {
            return ((IList)this.m_collection).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)this.m_collection).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList)this.m_collection).Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get {
                return ((IList)this.m_collection).IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get {
                return ((IList)this.m_collection).IsReadOnly;
            }
        }

        void IList.Remove(object value)
        {
            ((IList)this.m_collection).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            ((IList)this.m_collection).RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return ((IList)this.m_collection)[index];
            }
            set
            {
                ((IList)this.m_collection)[index] = value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IList)this.m_collection).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get {
                return ((IList)this.m_collection).Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get {
                return ((IList)this.m_collection).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get {
                return ((IList)this.m_collection).SyncRoot;
            }
        }
    }
}

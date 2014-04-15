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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using TraceLab.Core.Components;

namespace TraceLab.UI.WPF.ViewModels
{
    /// <summary>
    /// ComponentLibraryView Node - A base node for Nodes in the hierarchical view of the ComponentsLibrary
    /// </summary>
    abstract class CLVBaseNode : INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<CLVBaseNode> m_children = new ObservableCollection<CLVBaseNode>();
        private bool m_isExpanded;
        private bool m_isSelected;
        private ReadOnlyObservableCollection<CLVBaseNode> m_rochildren;

        #endregion Fields

        #region Constructors

        public CLVBaseNode()
        {
            m_rochildren = new ReadOnlyObservableCollection<CLVBaseNode>(m_children);
            Children = CollectionViewSource.GetDefaultView(m_rochildren);
            ((ListCollectionView)Children).CustomSort = new NodeComparer();
            Children.Refresh();
            //Children.SortDescriptions.Add(new SortDescription("Label", ListSortDirection.Ascending));
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the children of this node.
        /// </summary>
        public ReadOnlyObservableCollection<CLVBaseNode> AllChildren
        {
            get { return m_rochildren; }
        }

        public ICollectionView Children
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                if (m_isExpanded != value)
                {
                    m_isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// Gets the label of this node.
        /// </summary>
        public abstract string Label
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the child node to this instance.
        /// </summary>
        /// <param name="child">The child.</param>
        public virtual void AddChild(CLVBaseNode child)
        {
            if (child == null)
                throw new ArgumentNullException();
            if (m_children == null)
                throw new InvalidOperationException();

            if (!m_children.Contains(child))
            {
                m_children.Add(child);
            }
        }

        /// <summary>
        /// Removes the child node from this instance.
        /// </summary>
        /// <param name="child">The child.</param>
        public virtual void RemoveChild(CLVBaseNode child)
        {
            if (child == null)
                throw new ArgumentNullException();
            if (m_children == null)
                throw new InvalidOperationException();

            m_children.Remove(child);
        }

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion Methods

        private bool m_isDuplicateLabel;
        public bool IsDuplicateLabel
        {
            get { return m_isDuplicateLabel;}
            set
            {
                if (m_isDuplicateLabel != value)
                {
                    m_isDuplicateLabel = value;
                    OnPropertyChanged("IsDuplicateLabel");
                }
            }
        }
    }
}

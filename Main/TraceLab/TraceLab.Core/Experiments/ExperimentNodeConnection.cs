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

using System.Diagnostics;
using QuickGraph;
using System.ComponentModel;
using System.Xml.Serialization;
using TraceLab.Core.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// A simple identifiable edge - connection between two experiment nodes.
    /// </summary>
    [DebuggerDisplay("{Source.ID} -> {Target.ID}")]
    public class ExperimentNodeConnection : Edge<ExperimentNode>, INotifyPropertyChanged, IModifiable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentNodeConnection"/> class.
        /// </summary>
        /// <param name="id">The id of the connection.</param>
        /// <param name="source">The source experiment node.</param>
        /// <param name="target">The target experiment node.</param>
        public ExperimentNodeConnection(string id, ExperimentNode source, ExperimentNode target)
            : base(source, target)
        {
            ID = id;
            IsModified = true;
            IsFixed = false;
            IsVisible = true;
            InitRoutePoints();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentNodeConnection"/> class.
        /// </summary>
        /// <param name="id">The id of the connection.</param>
        /// <param name="source">The source experiment node.</param>
        /// <param name="target">The target experiment node.</param>
        /// <param name="isFixed">if set to <c>true</c> the connection is fixed (cannot be changed by user).</param>
        /// <param name="isVisible">if set to <c>true</c> is visible.</param>
        public ExperimentNodeConnection(string id, ExperimentNode source, ExperimentNode target, bool isFixed, bool isVisible)
            : base(source, target)
        {
            ID = id;
            IsModified = true;
            IsFixed = isFixed;
            IsVisible = isVisible;
            InitRoutePoints();
        }

        private string m_id;
        /// <summary>
        /// Gets the ID of the connection
        /// </summary>
        public string ID
        {
            get { return m_id; }
            private set
            {
                if (m_id != value)
                {
                    m_id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private bool m_isFixed;

        /// <summary>
        /// Gets or sets a value indicating whether the connection is fixed, and cannot be removed. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if the connection is fixed; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("isFixed")]
        public bool IsFixed
        {
            get { return m_isFixed; }
            set 
            {
                if (m_isFixed != value)
                {
                    m_isFixed = value;
                    NotifyPropertyChanged("IsFixed");
                }
            }
        }

        private bool m_isVisible;
        
        /// <summary>
        /// Gets or sets a value indicating whether the connection is visible or not
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the connection is visible; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("isVisible")]
        public bool IsVisible
        {
            get { return m_isVisible; }
            set
            {
                if (m_isVisible != value)
                {
                    m_isVisible = value;
                    NotifyPropertyChanged("IsVisible");
                }
            }
        }

        private RoutePointsCollection m_routePoints;
        
        /// <summary>
        /// Gets the route points.
        /// </summary>
        [XmlElement]
        public RoutePointsCollection RoutePoints
        {
            get { return m_routePoints; }
        }

        /// <summary>
        /// Inits the route points.
        /// </summary>
        private void InitRoutePoints()
        {
            m_routePoints = new RoutePointsCollection(this);
            m_routePoints.PointsChanged += OnRoutePointsChanged;
        }

        /// <summary>
        /// Called when route points changed. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnRoutePointsChanged(object sender, System.EventArgs e)
        {
            IsModified = true;
            NotifyPropertyChanged("RoutePoints");
        }

        #region IModifiable Members

        private bool m_isModified;
        /// <summary>
        /// Gets or sets a value indicating whether the connection is modified.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsModified
        {
            get { return m_isModified; }
            set
            {
                if (m_isModified != value)
                {
                    m_isModified = value;
                    NotifyPropertyChanged("IsModified");
                }
            }
        }

        /// <summary>
        /// Resets the modified flag.
        /// </summary>
        public void ResetModifiedFlag()
        {
            IsModified = false;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}

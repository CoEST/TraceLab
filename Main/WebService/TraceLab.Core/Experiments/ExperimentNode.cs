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
using System.Xml.Serialization;
using TraceLab.Core.Settings;
using System.Collections.Generic;
using TraceLab.Core.Utilities;
using System.Linq;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// A simple identifiable node.
    /// </summary>
    public abstract class ExperimentNode : INotifyPropertyChanged, IModifiable
    {
        #region Fields

        private bool m_isInfoPaneExpanded;
        private bool m_isSelected;
        private string m_id;
        private SerializedVertexData m_data;

        #endregion
        
        public ExperimentNode(string id, SerializedVertexData data)
        {
            ID = id;
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentNode"/> class.  Intended for use as a default constructor prior to copying data
        /// from another node.
        /// </summary>
        protected ExperimentNode()
        {

        }

        #region Properties

        [XmlIgnore]
        public string ID
        {
            get { return m_id; }
            // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
            // protected set
            set
            // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
            {
                if (m_id != value)
                {
                    m_id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public bool IsInfoPaneExpanded
        {
            get { return m_isInfoPaneExpanded; }
            set
            {
                if (m_isInfoPaneExpanded != value)
                {
                    m_isInfoPaneExpanded = value;
                    NotifyPropertyChanged("IsInfoPaneExpanded");
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
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        private TraceLabSDK.IProgress m_progress;
        [XmlIgnore]
        public TraceLabSDK.IProgress Progress
        {
            get { return m_progress; }
            set
            {
                if (m_progress != value)
                {
                    m_progress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        [XmlElement("Data")]
        public SerializedVertexData Data
        {
            get { return m_data; }
            set
            {
                if (m_data != value)
                {
                    if (m_data != null)
                        m_data.PropertyChanged -= m_data_PropertyChanged;
                    
                    m_data = value;

                    if (m_data != null)
                        m_data.PropertyChanged += m_data_PropertyChanged;
                    
                    NotifyPropertyChanged("Data");
                }
            }
        }

        void m_data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
 
            if (e.PropertyName == "IsModified")
            {
                NotifyPropertyChanged("IsModified");

            }
            if (e.PropertyName == "ErrorMessage")
            {
                SetError(Data.ErrorMessage);
            }

            // HERZUM SPRINT 5.3 TLAB-251
            CompositeComponentGraph subgraph = Owner as CompositeComponentGraph;
            if (subgraph != null) {
                subgraph.IsModified = true;
                if (subgraph.OwnerNode != null)
                    subgraph.OwnerNode.NotifyPropertyChanged (e.PropertyName);
            }
            // END HERZUM SPRINT 5.3 TLAB-251
        }

        public virtual bool IsModified
        {
            get 
            {
                return m_data.IsModified; 
            }
            set
            {
                if (m_data.IsModified != value)
                {
                    m_data.IsModified = value;
                    NotifyPropertyChanged("IsModified");
                }
            }
        }

        public virtual void ResetModifiedFlag()
        {
            m_data.ResetModifiedFlag();
        }

        private bool m_isExecuting/* = false*/;
        [XmlIgnore]
        public bool IsExecuting
        {
            get
            {
                return m_isExecuting;
            }
            set
            {
                if (m_isExecuting != value)
                {
                    m_isExecuting = value;
                    NotifyPropertyChanged("IsExecuting");
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the incoming edges count.
        /// It is additional convenience property which triggeres, whether All/Any button is hidden/vissible respectively depending on count of incoming edges.
        /// IncomingEdgesCount is the property that should be equal to the count of edges retrieved using Experiment.InEdges(node)
        /// (The owning experiment increments and decrements this property based on fired event of added and removed edges.
        /// </summary>
        /// <value>
        /// The incoming edges count.
        /// </value>
        [XmlIgnore]
        public int IncomingEdgesCount
        {
            get { return Owner.InEdges(this).Count(); } 
        }

        /// <summary>
        /// Fires the incoming edges count property changed notification.
        /// </summary>
        public void FireIncomingEdgesCountPropertyChanged()
        {
            NotifyPropertyChanged("IncomingEdgesCount");
        }

        private IExperiment m_owner;
        [XmlIgnore]
        public virtual IExperiment Owner
        {
            get { return m_owner; }
            set
            {
                // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
                //if (m_owner != null)
                //    throw new InvalidOperationException("ExperimentNode must have owning experiment!");
                // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
                m_owner = value;
            }
        }

        [XmlIgnore]
        public bool HasError
        {
            get
            {
                return (Error == null) ? false : true;
            }
        }

        [XmlIgnore]
        public string ErrorMessage
        {
            get
            {
                return (Error == null) ? null : Error.ErrorMessage;
            }
        }

        private ExperimentNodeError m_error = null;
        [XmlIgnore]
        public ExperimentNodeError Error
        {
            get
            {
                return m_error;
            }
            set
            {
                if (m_error != value)
                {
                    m_error = value;
                    NotifyPropertyChanged("Error");
                    NotifyPropertyChanged("ErrorMessage");
                    NotifyPropertyChanged("HasError");
                    OnErrorChanged(new ExperimentNodeErrorEventArgs(value));
                }
            }
        }

        public void SetError(string errorMessage)
        {
            if (errorMessage == null)
            {
                Error = null;
            }
            else
            {
                Error = new ExperimentNodeError(errorMessage);
            }
        }

        public void SetError(string errorMessage, ExperimentNodeErrorType errorType)
        {
            if (errorMessage == null)
            {
                Error = null;
            }
            else
            {
                Error = new ExperimentNodeError(errorMessage, errorType);
            }
        }

        public void ClearError()
        {
            Error = null;
        }

        public event EventHandler<ExperimentNodeErrorEventArgs> ErrorChanged;

        private void OnErrorChanged(ExperimentNodeErrorEventArgs args)
        {
            if (ErrorChanged != null)
            {
                ErrorChanged(this, args);
            }
        }

        #endregion 

        public override string ToString()
        {
            return ID;
        }

        #region Equals_HashCode

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            var other = obj as ExperimentNode;
            isEqual = other != null ? object.Equals(ID, other.ID) : false;

            return isEqual;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        #endregion 

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        public abstract ExperimentNode Clone();

        protected virtual void CopyFrom(ExperimentNode other)
        {
            DoCopyFrom(other);
        }

        private void DoCopyFrom(ExperimentNode other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            m_owner = null;
            ID = other.ID;
            Data = other.Data.Clone();
            IsInfoPaneExpanded = other.IsInfoPaneExpanded;
            IsExecuting = other.IsExecuting;
            IsSelected = other.IsSelected;
            if (other.HasError)
            {
                this.Error = new ExperimentNodeError(other.Error.ErrorMessage, other.Error.ErrorType);
            }
        }
    }

}

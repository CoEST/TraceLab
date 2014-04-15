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
using System.Xml.Serialization;
using QuickGraph;
using TraceLab.Core.Components;
using TraceLab.Core.Utilities;
using System.Xml;
using QuickGraph.Serialization;
using System.ComponentModel;
using TraceLab.Core.ExperimentExecution;
using TraceLabSDK;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.Experiments
{
    public class Experiment : BaseExperiment, IEditableExperiment
    {
        private bool m_isModified;

        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        public Experiment() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="filepath">The filepath.</param>
        public Experiment(string name, string filepath) : base(name, filepath)
        {
        }

        /// <summary>
        /// Clones this experiment.
        /// </summary>
        /// <returns></returns>
        public override BaseExperiment Clone()
        {
            Experiment clone = new Experiment();
            clone.CopyFrom(this);
            clone.ResetModifiedFlag();
            return clone;
        }

        #region Modification

        /// <summary>
        /// Calculates the modification.
        /// </summary>
        /// <returns></returns>
        protected override bool CalculateModification()
        {
            bool isModified = base.CalculateModification();
            isModified |= m_isModified;
            return isModified;
        }

        /// <summary>
        /// Resets the modified flag.
        /// </summary>
        public override void ResetModifiedFlag()
        {
            m_isModified = false;
            base.ResetModifiedFlag();
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ExperimentNode control.
        /// If property of change is IsModified - set experiment to be modified as well.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ExperimentNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified" || e.PropertyName == "Data")
            {
                var node = (ExperimentNode)sender;
                IsModified = true;
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ExperimentNodeConnection control.
        /// If property of change is IsModified - set experiment to be modified as well.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ExperimentNodeConnection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                IsModified = true;
            }
        }

        /// <summary>
        /// Called when vertex is added to experiment
        /// When vertex is added the experiment should be set to be modified.
        /// It also attaches listener to vertex changes. So it can also set modify flag to true when anything in the node is modified.
        /// </summary>
        /// <param name="args">The args.</param>
        protected override void OnVertexAdded(ExperimentNode args)
        {
            base.OnVertexAdded(args);
            m_isModified = true;
            IsModified = true;
            args.PropertyChanged += ExperimentNode_PropertyChanged;
        }

        /// <summary>
        /// Called when vertex is removed from experiment
        /// When vertex is removed the experiment should be set to be modified.
        /// </summary>
        /// <param name="args">The args.</param>
        protected override void OnVertexRemoved(ExperimentNode args)
        {
            base.OnVertexRemoved(args);
            m_isModified = true;
            IsModified = true;
            args.PropertyChanged -= ExperimentNode_PropertyChanged;
        }

        /// <summary>
        /// Called when edge is added to experiment.
        /// When edge is added the experiment should be set to be modified.
        /// It also attaches listener to edge changes. So it can also set modify flag when edge changes.
        /// </summary>
        /// <param name="args">The args.</param>
        protected override void OnEdgeAdded(ExperimentNodeConnection args)
        {
            base.OnEdgeAdded(args);
            m_isModified = true;
            IsModified = true;
            //listen to modification of the edge
            args.PropertyChanged += ExperimentNodeConnection_PropertyChanged;
        }

        /// <summary>
        /// Called when edge is removed from experiment.
        /// When edge is removed the experiment should be set to be modified.
        /// </summary>
        /// <param name="args">The args.</param>
        protected override void OnEdgeRemoved(ExperimentNodeConnection args)
        {
            base.OnEdgeRemoved(args);
            m_isModified = true;
            IsModified = true;
            args.PropertyChanged -= ExperimentNodeConnection_PropertyChanged;
        }

        #endregion

        private ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> m_references = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>();
        /// <summary>
        /// Gets or sets the collection of references to the packages
        /// </summary>
        /// <value>
        /// The references.
        /// </value>
        [XmlIgnore]
        public override ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> References
        {
            get { return m_references; }
            set 
            {
                if (m_references != value)
                {
                    m_references = value;
                    if (m_references == null)
                    {
                        m_references = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>();
                    }

                    NotifyPropertyChanged("References");
                }
            }
        }

        /// <summary>
        /// Adds a new component at the specified coordinates
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the component definition is null</exception>
        public ExperimentNode AddComponentFromDefinition(MetadataDefinition metadataDefinition, double positionX, double positionY)
        {
            ExperimentNode newNode = ComponentFactory.AddComponentFromDefinitionToExperiment(this, metadataDefinition, positionX, positionY);
            return newNode;
        }
    }
}

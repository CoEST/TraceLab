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
using System.Collections.Specialized;
using System.Text;
using TraceLab.Core.Components;
using TraceLab.Core.Settings;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Composite Copmonent Node represents the compound node - ie. node that contains another graph inside. 
    /// </summary>
    public class CompositeComponentNode : ExperimentNode
    {
        public CompositeComponentNode(string id, SerializedVertexData data)
            : base(id, data)
        {
            InitializeComponentGraph();
        }

        public CompositeComponentNode(string id, SerializedVertexData data, TraceLab.Core.Settings.Settings settings)
            : base(id, data)
        {
            InitializeComponentGraph(settings);
        }
        
        protected CompositeComponentNode() : base()
        {
            
        }

        public CompositeComponentBaseMetadata CompositeComponentMetadata
        {
            get
            {
                if (Data != null && Data.Metadata != null)
                {
                    return Data.Metadata as CompositeComponentBaseMetadata;
                }
                else
                {
                    return null;
                }
            }
        }

        public override ExperimentNode Clone()
        {
            var clone = new CompositeComponentNode();
            clone.CopyFrom(this);
            return clone;
        }

        protected override void CopyFrom(ExperimentNode other)
        {
            base.CopyFrom(other);
            InitializeComponentGraph();
        }

        /// <summary>
        /// Initializes the component graph with null settings.
        /// </summary>
        protected virtual void InitializeComponentGraph()
        {
            InitializeComponentGraph(null);
        }

        protected virtual void InitializeComponentGraph(TraceLab.Core.Settings.Settings settings)
        {
            if (CompositeComponentMetadata.HasDeserializationError == false)
            {
                CompositeComponentMetadata.InitializeComponentGraph(this, settings);

                //subscribe to subworkflow errors
                INotifyCollectionChanged subexperimentErrorCollection = CompositeComponentMetadata.ComponentGraph.Errors;
                subexperimentErrorCollection.CollectionChanged += SubExperimentErrorCollectionChanged;
                
            }
        }

        #region Sub Experiment Error Collection Changed

        private void SubExperimentErrorCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RefreshError();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RefreshError();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RefreshError();
                    break;
                case NotifyCollectionChangedAction.Move:
                    RefreshError();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RefreshError();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RefreshError()
        {
            if (Data != null)
            {
                CompositeComponentMetadata metadata = Data.Metadata as CompositeComponentMetadata;

                if (metadata != null && metadata.ComponentGraph != null)
                {
                    SetSubExperimentError(metadata.ComponentGraph);
                }
            }
        }

        private void SetSubExperimentError(IExperiment experiment)
        {
            if (experiment.Errors.Count > 0)
            {
                StringBuilder errorMessages = new StringBuilder();
                foreach (ExperimentNodeError error in experiment.Errors.Values)
                {
                    errorMessages.Append(error.ErrorMessage);
                }
                SetError(errorMessages.ToString());
            }
            else
            {
                ClearError();
            }
        }

        #endregion
    }
}

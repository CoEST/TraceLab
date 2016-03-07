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
using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Composite Component Editable Graph represents the subexperiment of the scope component.
    /// It can be edited, new nodes can be added, removed, modified within it. 
    /// </summary>
    public class CompositeComponentEditableGraph : CompositeComponentGraph, IEditableExperiment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentEditableGraph"/> class.
        /// </summary>
        protected CompositeComponentEditableGraph() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentEditableGraph"/> class.
        /// Constructor used to create empty new editable graph. Initially it does not have OwnerNode. 
        /// The OwnerNode is set in initialization step when metadata that contains graph is assigned to scope node.
        /// </summary>
        /// <param name="initStartEnd">if set to <c>true</c> [init start end].</param>
        public CompositeComponentEditableGraph(bool initStartEnd) : base()
        {
            if (initStartEnd)
            {
                ExperimentStartNode start = new ExperimentStartNode(Guid.NewGuid().ToString(), "Enter");
                start.Data.X = 0;
                start.Data.Y = 0;
                AddVertex(start);

                ExperimentEndNode end = new ExperimentEndNode(Guid.NewGuid().ToString(), "Exit");
                end.Data.X = 0;
                end.Data.Y = 0;
                AddVertex(end);

                ReloadStartAndEndNode();
            }

            //note, OwnerNode is set in InitializeComponentGraph of ScopeNode
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentEditableGraph"/> class.
        /// </summary>
        /// <param name="componentGraph">The component graph.</param>
        public CompositeComponentEditableGraph(BaseExperiment componentGraph)
            : base()
        {
            if (componentGraph == null)
            {
                throw new ArgumentException("Component graph cannot be null");
            }

            CopyFrom(componentGraph);
            OwnerNode = null; //does not belong to any node
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeComponentEditableGraph"/> class.
        /// </summary>
        /// <param name="compositeComponentNode">The composite component node.</param>
        /// <param name="componentGraph">The component graph.</param>
        public CompositeComponentEditableGraph(CompositeComponentNode compositeComponentNode, CompositeComponentEditableGraph componentGraph)
            : base()
        {
            if (componentGraph == null)
            {
                throw new ArgumentException("Component graph cannot be null");
            }

            CopyFrom(componentGraph);
            OwnerNode = compositeComponentNode;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override BaseExperiment Clone()
        {
            var clone = new CompositeComponentEditableGraph(this);
            clone.ResetModifiedFlag();
            return clone;
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

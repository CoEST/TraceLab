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
using System.Windows.Input;
using System.Xml.Serialization;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Commands;
using System.ComponentModel;

namespace TraceLab.UI.WPF.ViewModels
{
    public sealed class SubLevelExperimentViewModel : BaseLevelExperimentViewModel
    {
        #region Constructor

        public SubLevelExperimentViewModel(CompositeComponentGraph componentGraph, BaseLevelExperimentViewModel owner)
            : base(componentGraph, owner)
        {
        }

        /// <summary>
        /// Cache for the reference to the owner node of this graph in the top level experiment.
        /// </summary>
        private CompositeComponentNode m_topOwnerCompositeComponentNode;
        public CompositeComponentNode TopOwnerCompositeComponentNode
        {
            get
            {
                if (m_topOwnerCompositeComponentNode == null)
                {
                    //methods finds and sets top owner composite node of this graph.
                    GetTopOwnerNode();
                }

                return m_topOwnerCompositeComponentNode;
            }
        }

        /// <summary>
        /// Finds the owner node in the top level experiment, (at the top of the subgraphs hierarchy) of this sublevel component graph.
        /// Also it sets the node id full path. It contains the id of all the owners node of the subgraphs all the way to the top.
        /// </summary>
        private CompositeComponentNode GetTopOwnerNode()
        {
            //if the top owner node has not yet been discovered
            if (m_topOwnerCompositeComponentNode == null)
            {
                if (Owner == null)
                    throw new InvalidOperationException("Application is at invalid state. Sublevel experiment always need to have an owner.");
                
                // if the owner of this sublevel experiment is the top level experiment
                TopLevelExperimentViewModel topLevel = Owner as TopLevelExperimentViewModel;
                if (topLevel != null)
                {
                    // then return this component graph owner node (which is a part of the top level experiment)
                    CompositeComponentGraph componentGraph = (CompositeComponentGraph)GetExperiment();
                    m_topOwnerCompositeComponentNode = componentGraph.OwnerNode;
                }
                else
                {
                    //otherwise recursive to subgraph above this graph, until it finds top level experiment
                    SubLevelExperimentViewModel levelAbove = (SubLevelExperimentViewModel)Owner;
                    m_topOwnerCompositeComponentNode = levelAbove.GetTopOwnerNode();

                    //in addition, set this component graph node if path. 
                    CompositeComponentGraph componentGraph = (CompositeComponentGraph)GetExperiment();
                    GraphIdPath = componentGraph.OwnerNode.ID;
                }
            }

            return m_topOwnerCompositeComponentNode;
        }

        private string m_fullGraphIdPath;

        /// <summary>
        /// Gets the graph id path. It is the path constructed with all owner nodes id along the path to the node id in the top level experiment.
        /// </summary>
        public string GraphIdPath
        {
            get
            {
                if (m_fullGraphIdPath == null)
                {
                    //call the method that discovers it along with top owner node
                    GetTopOwnerNode();
                }
                return m_fullGraphIdPath;
            }
            private set
            {
                if (String.IsNullOrEmpty(m_fullGraphIdPath))
                {
                    m_fullGraphIdPath = value;
                }
                else
                {
                    //otherwise add to already existing path
                    m_fullGraphIdPath += ":" + value;
                }
            }
        }

        #endregion
    }
}

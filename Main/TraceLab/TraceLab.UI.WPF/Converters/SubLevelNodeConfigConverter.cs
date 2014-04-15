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
using System.Windows.Markup;
using System.Windows.Data;
using TraceLab.Core.Components;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Converters
{
    class SubLevelNodeConfigConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IValueConverter Members

        /// <summary>
        /// The converter method returns the ConfigWrapper object for the calling node info.
        /// If the node belongs to sub level experiment it will replace the config wrapper with references to
        /// the ConfigPropertyObjects for the given node in the CompositeComponentNode in the top experiment view.
        /// If the node is already in the top experiment view, it simply returns original config wrapper.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">type ConfigWrapper</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>
        /// ConfigWrapper for the calling node info
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ExperimentNode node = value as ExperimentNode;
            if (node == null)
                throw new InvalidOperationException("Converter cannot accept value that is not of the ExperimentNode type");

            var metadata = (IConfigurableAndIOSpecifiable)node.Data.Metadata;

            //the default return value is config wrapper itself
            ConfigWrapper defaultConfigWrapper = metadata.ConfigWrapper;

            // the config wrapper that is going to be returned
            ConfigWrapper retConfigWrapper = defaultConfigWrapper;

            CompositeComponentGraph ownerGraph = node.Owner as CompositeComponentEditableGraph;
            bool isNodeInScope = (ownerGraph != null);

            //if node is not in scope (the check is needed, as scope extends composite component, but unlike composite component, it does not need to override config values)
            if (isNodeInScope == false)
            {
                //check if node is in composite component instead
                ownerGraph = node.Owner as CompositeComponentGraph;

                //check if node is in the subgraph, and also check if 
                //ownerGraph.OwnerNode as it the top graph might be a composite component - if it is during the process of creating 
                //the composite component
                bool isNodeInSubgraph = (ownerGraph != null && ownerGraph.OwnerNode != null);
                
                if (isNodeInSubgraph)
                {
                    CompositeComponentNode topCompositeComponentNode = ownerGraph.OwnerNode;

                    string fullNodeId = String.Empty;
                    GetGraphIdPath(topCompositeComponentNode, ref fullNodeId);

                    if (String.IsNullOrEmpty(fullNodeId))
                    {
                        fullNodeId = node.ID;
                    }
                    else
                    {
                        //otherwise add to already existing path
                        fullNodeId += ":" + node.ID;
                    }

                    CompositeComponentMetadata compositeComponentMetadata = (CompositeComponentMetadata)topCompositeComponentNode.Data.Metadata;
                    ConfigWrapper topConfig = compositeComponentMetadata.ConfigWrapper;

                    //replace the config wrapper
                    retConfigWrapper = topConfig.CreateViewForId(fullNodeId, true);
                }
            }

            return retConfigWrapper;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void GetGraphIdPath(CompositeComponentNode topOwnerCompositeComponentNode, ref string fullGraphIdPath)
        {
            if (topOwnerCompositeComponentNode == null)
            {
                if (topOwnerCompositeComponentNode.Owner == null)
                throw new InvalidOperationException("Application is at invalid state. Sublevel experiment always need to have an owner.");

                var ownerGraph = topOwnerCompositeComponentNode.Owner as CompositeComponentGraph;

                //check if node is in the subgraph, and also check if 
                //ownerGraph.OwnerNode as it the top graph might be a composite component - if it is during the process of creating 
                //the composite component
                bool isNodeInSubgraph = (ownerGraph != null && ownerGraph.OwnerNode != null);

                if (isNodeInSubgraph)
                {
                    //recursive to graph above this graph, until it finds top level experiment
                    CompositeComponentNode topCompositeComponentNode = ownerGraph.OwnerNode;
                    GetGraphIdPath(topOwnerCompositeComponentNode, ref fullGraphIdPath);

                    if (String.IsNullOrEmpty(fullGraphIdPath))
                    {
                        fullGraphIdPath = ownerGraph.OwnerNode.ID;
                    }
                    else
                    {
                        //otherwise add to already existing path
                        fullGraphIdPath += ":" + ownerGraph.OwnerNode.ID;
                    }
                }

            }
        }
    }
}

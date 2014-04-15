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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;
using TraceLab.Core.Utilities;
using TraceLab.UI.WPF.Utilities;
using TraceLab.UI.WPF.ViewModels.Nodes;

namespace TraceLab.UI.WPF.Views.Nodes
{
    public class BaseComponentNodeInfoControl : UserControl
    {
        public BaseComponentNodeInfoControl() { }

        #region IOItem Mouse Handlers

        protected virtual void IOItem_MouseEnterHandler(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ListViewItem listViewItem = sender as System.Windows.Controls.ListViewItem;
            IExperiment experiment;
            if (TryFindExperiment(listViewItem, out experiment))
            {
                string mapping = GetMappingName(listViewItem);
                ExperimentHelper.HighlightIOInExperiment(experiment, mapping);
            }
        }

        protected virtual void IOItem_MouseLeaveHandler(object sender, RoutedEventArgs e)
        {
            ListViewItem listViewItem = sender as System.Windows.Controls.ListViewItem;

            IExperiment experiment;
            if (TryFindExperiment(listViewItem, out experiment))
            {
                ExperimentHelper.ClearHighlightIOInExperiment(experiment);
            }
        }

        /// <summary>
        /// Gets the name of the mapping.
        /// </summary>
        /// <param name="listViewItem">The list view item.</param>
        /// <returns></returns>
        private static string GetMappingName(ListViewItem listViewItem)
        {
            string mapping = String.Empty;
            if (listViewItem.DataContext is KeyValuePair<string, IOItem>)
            {
                var item = (KeyValuePair<string, IOItem>)listViewItem.DataContext;
                mapping = item.Value.MappedTo;
            }
            return mapping;
        }

        /// <summary>
        /// Tries the find experiment.
        /// </summary>
        /// <param name="dependencyObject">The dependency object from which the parent is going to be searched for</param>
        /// <param name="experiment">The experiment.</param>
        /// <returns></returns>
        protected static bool TryFindExperiment(DependencyObject dependencyObject, out IExperiment experiment)
        {
            bool found = false;
            experiment = null;

            if (dependencyObject != null)
            {
                var nodeInfoContainer = dependencyObject.GetParent<TraceLab.UI.WPF.Views.Nodes.NodeInfoContainer>(null);

                var componentNodeInfo = nodeInfoContainer.DataContext as ExperimentNodeInfo;
                if (componentNodeInfo != null)
                {
                    experiment = componentNodeInfo.Node.Owner;
                    found = true;
                }
            }

            return found;
        }

        #endregion

        #region Resizing

        private const double c_minResizeHeight = 76;

        /// <summary>
        /// Resizes the info pane. 
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        /// <param name="iospecExpander">The iospec expander.</param>
        /// <param name="iospecRow">The iospec row.</param>
        /// <param name="outputsListView">The outputs list view.</param>
        /// <param name="inputsListView">The inputs list view.</param>
        protected static void resizeInfoPane(System.Windows.Controls.Primitives.DragDeltaEventArgs e, 
                                             Expander iospecExpander, RowDefinition iospecRow, ListView outputsListView, ListView inputsListView)
        {
            if (iospecExpander != null && outputsListView != null && inputsListView != null)
            {
                if (iospecExpander.IsExpanded == true && outputsListView.Items.Count + inputsListView.Items.Count > 1)
                {
                    double deltaVertical = Math.Min(-e.VerticalChange, iospecRow.ActualHeight - c_minResizeHeight);
                    double newHeight;

                    //if iospecExpander has been just expanded iospecRow has height set to auto
                    if (iospecRow.Height == GridLength.Auto)
                    {
                        iospecRow.MaxHeight = iospecRow.ActualHeight; //record current actual height as max height for iospec row
                        newHeight = iospecRow.ActualHeight - deltaVertical;
                    }
                    else
                    {
                        newHeight = iospecRow.Height.Value - deltaVertical;
                    }

                    if (newHeight > iospecRow.MaxHeight)
                    {
                        newHeight = iospecRow.MaxHeight;
                    }

                    iospecRow.Height = new GridLength(newHeight);
                }
            }

            e.Handled = true;
        }

        protected static void iospecExpander_Collapsed(Thumb resizeThumb, RowDefinition iospecRow)
        {
            if (resizeThumb != null && iospecRow != null)
            {
                resizeThumb.Visibility = System.Windows.Visibility.Collapsed;

                //reset iospecHeight to Auto
                iospecRow.Height = GridLength.Auto;
            }
        }

        protected static void iospecExpander_Expanded(Thumb resizeThumb, ListView outputsListView, ListView inputsListView)
        {
            //show resize thumb if there are more than 2 io items
            if (resizeThumb != null && outputsListView != null && inputsListView != null)
            {
                if (outputsListView.Items.Count + inputsListView.Items.Count > 1)
                {
                    resizeThumb.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        #endregion

    }
}

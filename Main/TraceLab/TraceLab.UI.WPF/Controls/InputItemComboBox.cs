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

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TraceLab.Core.Components;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;
using TraceLab.UI.WPF.Views.Nodes;
using System.Windows.Controls;

namespace TraceLab.UI.WPF.Controls
{
    public class InputItemComboBox : System.Windows.Controls.ComboBox
    {
        public InputItemComboBox()
        {
            this.Loaded += new RoutedEventHandler(InputItemComboBox_Loaded);
        }

        void InputItemComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        protected override void OnDropDownOpened(System.EventArgs e)
        {
            Refresh();
            base.OnDropDownOpened(e);
        }

        protected override void OnContextMenuOpening(System.Windows.Controls.ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size baseSize = base.MeasureOverride(constraint);

            if (baseSize.Width < constraint.Width)
                baseSize.Width = constraint.Width;

            return baseSize;
        }

        void Refresh()
        {
            List<string> entries = new List<string>();

            if (DataContext is KeyValuePair<string, IOItem>)
            {
                KeyValuePair<string, IOItem> keyValuePair = (KeyValuePair<string, IOItem>)DataContext;
                IOItem currentItem = keyValuePair.Value;
            
                var infoControl = TemplatedParent.GetParent<NodeInfoContainer>(null);
                var control = infoControl != null ? infoControl.OriginElement : null;
                if (control != null)
                {
                    // We'll need to query the canvas for links
                    //NodeGraphLayout canvas = control.RootCanvas as NodeGraphLayout;
                    TraceLab.UI.WPF.Views.DockableGraph topGraph = control.GetParent<TraceLab.UI.WPF.Views.DockableGraph>(null);
                    NodeGraphLayout canvas = topGraph.graphLayout;

                    // Add the current mapping
                    entries.Add(currentItem.MappedTo);

                    // Get the current item's type so that we can limit the selection to only this type
                    string currentType = currentItem.IOItemDefinition.Type;
                    
                    var availableInputMappingsPerNode = new TraceLab.Core.Utilities.InputMappings(canvas.Graph);

                    ExperimentNode currentNode = control.Vertex as ExperimentNode;

                    //if currentNode is a ExperimentNode, and is ioValidator has incoming outputs for this node (otherwise, it has not been connected to start node)
                    if (currentNode != null && availableInputMappingsPerNode.ContainsMappingsForNode(currentNode))
                    {
                        foreach (string incomingOutput in availableInputMappingsPerNode[currentNode].Keys)
                        {
                            if (string.Equals(currentType, availableInputMappingsPerNode[currentNode][incomingOutput]))
                            {
                                entries.Add(incomingOutput);
                            }
                        }
                    
                    }

                }
            }
            this.ItemsSource = entries.Distinct();
        }
    }
}

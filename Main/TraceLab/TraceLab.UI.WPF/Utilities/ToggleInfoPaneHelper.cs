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
using System.Windows.Input;
using System.Windows;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Utilities
{
    class ToggleInfoPaneHelper
    {
        /// <summary>
        /// Toogles the info pane node.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="toggleInfoPaneForNodeCommand">The toggle info pane for node command.</param>
        internal static void Toogle(object param, ICommand toggleInfoPaneForNodeCommand)
        {
            var vertexControl = param as GraphSharp.Controls.VertexControl;
            if (vertexControl != null)
            {
                Toogle(vertexControl, toggleInfoPaneForNodeCommand);
            }
        }

        /// <summary>
        /// Toogles off/on the info pane for the given vertex
        /// </summary>
        /// <param name="vertexControl">The vertex control.</param>
        /// <param name="toggleInfoPaneForNodeCommand">The toggle info pane for node command.</param>
        private static void Toogle(GraphSharp.Controls.VertexControl vertexControl, ICommand toggleInfoPaneForNodeCommand)
        {
            if (toggleInfoPaneForNodeCommand != null)
            {
                var node = vertexControl.Vertex as ExperimentNode;
                if (node != null)
                {
                    Point suggestedLocation = new Point();
                    var param = new TraceLab.UI.WPF.Commands.ToggleNodeInfoParameters() { SuggestedLocation = suggestedLocation, Node = node };

                    if (vertexControl != null)
                    {
                        const int padding = 5;
                        suggestedLocation = new Point(vertexControl.TopLeftX + vertexControl.ActualWidth, vertexControl.TopLeftY + vertexControl.ActualHeight + padding);
                        param.SuggestedLocation = suggestedLocation;
                    }

                    toggleInfoPaneForNodeCommand.Execute(param);
                }
            }
        }
    }
}

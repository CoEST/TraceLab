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
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Converters
{
    public class GetNodeControlConverter : MarkupExtension, IMultiValueConverter
    {
        public GetNodeControlConverter() : base()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UIElement final = null;
            var obj = values[0] as UIElement;
            var node = values[1] as ExperimentNode;
            if (obj != null)
            {
                var source = obj.GetParent<TraceLab.UI.WPF.Views.GraphView>(null);

                if (source != null)
                {
                    final = FindVertexControl(node, source);
                }
                else
                {
                    var readonlyGraph = obj.GetParent<TraceLab.UI.WPF.Views.ReadonlyDockableGraph>(null);
                    if (readonlyGraph != null)
                    {
                        final = readonlyGraph.ControlForNode(node);
                    }
                }
            }

            return final;
        }

        /// <summary>
        /// Finds the vertex control (view) of the given node
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="graphView">The graph view.</param>
        /// <returns></returns>
        private GraphSharp.Controls.VertexControl FindVertexControl(ExperimentNode node, TraceLab.UI.WPF.Views.GraphView graphView)
        {
            GraphSharp.Controls.VertexControl vertexControl = null;

            //check if node belongs to scope, ie. editable composite component graph of the scopenode
            var ownerGraph = node.Owner as CompositeComponentEditableGraph;
            if (ownerGraph != null)
            {
                //double check if the OwnerNode of the graph is a ScopeNode
                var scopeNode = ownerGraph.OwnerNode as ScopeNodeBase;
                if (scopeNode != null)
                {
                    var scopeVertexControl = FindVertexControl(scopeNode, graphView);
                    var scopeGraphView = FindVisualChild<TraceLab.UI.WPF.Views.GraphView>(scopeVertexControl);
                    if (scopeGraphView != null)
                    {
                        vertexControl = scopeGraphView.ControlForNode(node);
                    }
                }
            }
            else
            {
                vertexControl = graphView.ControlForNode(node);
            }

            return vertexControl;
        }

        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }


        //void WalkDownLogicalTree(object current)
        //{
        //    System.Diagnostics.Debug.WriteLine(current);

        //    GraphSharp.Controls.VertexControl vertexControl = current as GraphSharp.Controls.VertexControl;
        //    if (vertexControl != null)
        //    {
        //        if (vertexControl.Vertex is ScopeNode)
        //        {
        //            TraceLab.UI.WPF.Views.GraphView graphView = FindVisualChild<TraceLab.UI.WPF.Views.GraphView>(vertexControl);
        //            System.Diagnostics.Debug.WriteLine(graphView);
        //        }
        //    }

        //    // The logical tree can contain any type of object, not just 
        //    // instances of DependencyObject subclasses.  LogicalTreeHelper
        //    // only works with DependencyObject subclasses, so we must be
        //    // sure that we do not pass it an object of the wrong type.
        //    DependencyObject depObj = current as DependencyObject;

        //    if (depObj != null)
        //        foreach (object logicalChild in LogicalTreeHelper.GetChildren(depObj))
        //            WalkDownLogicalTree(logicalChild);

        //}
    }
}

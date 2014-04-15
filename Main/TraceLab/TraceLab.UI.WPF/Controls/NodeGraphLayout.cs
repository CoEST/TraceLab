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

using GraphSharp.Controls;
using TraceLab.Core.Experiments;
using System.Windows;
using System.Collections.Generic;

namespace TraceLab.UI.WPF.Controls
{
    public class NodeGraphLayout : GraphLayout<ExperimentNode, ExperimentNodeConnection, BaseExperiment>
    {
        public NodeGraphLayout()
            : base()
        {
            InitVerticesPosition = false;
        }

        public static readonly DependencyProperty FillRectangleProperty = DependencyProperty.Register("FillRectangle", typeof(Rect), typeof(NodeGraphLayout));

        public Rect FillRectangle
        {
            get { return (Rect)GetValue(FillRectangleProperty); }
            private set { SetValue(FillRectangleProperty, value); }
        }

        public VertexControl GetNodeControl(ExperimentNode node)
        {
            return base.GetOrCreateVertexControl(node);
        }

        public IEnumerable<VertexControl> GetAllVertexControls()
        {
            foreach (VertexControl control in _vertexControls.Values)
            {
                yield return control;
            }
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            var desiredSize = base.MeasureOverride(constraint);

            if (Graph != null && (desiredSize.IsEmpty || (desiredSize.Width == 0 && desiredSize.Height == 0)))
            {
                double left = double.PositiveInfinity;
                double top = double.PositiveInfinity;
                double right = double.NegativeInfinity;
                double bottom = double.NegativeInfinity;
                // measure ourselves

                var enumer = LogicalChildren;
                while (enumer.MoveNext())
                {
                    VertexControl control = enumer.Current as VertexControl;
                    if (control != null)
                    {
                        left = System.Math.Min(left, control.TopLeftX);
                        top = System.Math.Min(top, control.TopLeftY);

                        right = System.Math.Max(right, control.TopLeftX + control.ActualWidth);
                        bottom = System.Math.Max(bottom, control.TopLeftY + control.ActualHeight);
                    }
                }

                if (!double.IsNaN(bottom) && !double.IsNaN(right) && !double.IsInfinity(left) && !double.IsInfinity(top))
                {
                    FillRectangle = new Rect(left, top, right - left, bottom - top);
                }
            }

            return desiredSize;
        }
    }
}

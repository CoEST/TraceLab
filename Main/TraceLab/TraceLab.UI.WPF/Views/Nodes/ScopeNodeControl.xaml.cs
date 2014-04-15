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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Adorners;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Interaction logic for ScopeNodeControl.xaml
    /// </summary>
    [TemplatePart(Name = "PART_NodeControl", Type = typeof(TraceLabSDK.IProgress))]
    public class ScopeNodeControl : Control
    {
        public ScopeNodeControl()
        {
        }

        private FrameworkElement m_nodeControl;
        internal GraphSharp.Controls.VertexControl VertexControl
        {
            get;
            private set;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Template != null)
            {
                //locate template part and gets it data context which is suppose to be a VertexControl
                m_nodeControl = (FrameworkElement)Template.FindName("PART_NodeControl", this);
                VertexControl = (GraphSharp.Controls.VertexControl)m_nodeControl.DataContext;
                ((ExperimentNode)VertexControl.Vertex).PropertyChanged += experimentNode_PropertyChanged;
            }
        }

        private void experimentNode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                ExperimentNode node = (ExperimentNode)sender;

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(m_nodeControl);

                if (node.IsSelected == true)
                {
                    var resizer = new ResizingAdorner(m_nodeControl);
                    resizer.ResizingHorizontally += MoveCenterHorizontally;
                    resizer.ResizingVertically += MoveCenterVertically;
                    adornerLayer.Add(resizer);
                }
                else
                {
                    adornerLayer.Remove(adornerLayer.GetAdorners(m_nodeControl)[0]);
                }
            }
        }

        private void MoveCenterHorizontally(object sender, DragDeltaEventArgs args)
        {
            VertexControl.CenterX += args.HorizontalChange / 2;
        }

        private void MoveCenterVertically(object sender, DragDeltaEventArgs args)
        {
            VertexControl.CenterY += args.VerticalChange / 2;
        }

        /// <summary>
        /// Gets or sets a value indicating whether control should display button that allows removing the node
        /// </summary>
        /// <value>
        /// 	<c>true</c> displays trash button; otherwise does not display it.
        /// </value>
        public bool DisplayRemoveNodeButton
        {
            get;
            set;
        }
    }
}

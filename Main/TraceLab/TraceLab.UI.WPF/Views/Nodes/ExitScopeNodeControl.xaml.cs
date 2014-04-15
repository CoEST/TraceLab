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
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Interaction logic for ExitScopeNodeControl.xaml
    /// </summary>
    public class ExitScopeNodeControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitScopeNodeControl"/> class.
        /// </summary>
        public ExitScopeNodeControl()
        {
        }

        private ExperimentEndNode m_exitScopeNode;
        private ScopeNode m_scopeNode;
        private double m_labelHeight;

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var vertexControl = (GraphSharp.Controls.VertexControl)DataContext;
            m_exitScopeNode = (ExperimentEndNode)vertexControl.Vertex;
            
            //listen to property changed of the parent scope node, and update exit node x and y, if scope size is changing. 
            ScopeNodeControl scope = this.GetParent<ScopeNodeControl>(null);
            
            //find node label border - needed border height to adjust node move inside the canvas
            Border labelBorder = (Border)scope.Template.FindName("labelBorder", scope);
            m_labelHeight = labelBorder.ActualHeight;

            m_scopeNode = (ScopeNode)scope.VertexControl.Vertex;
            
            //listen to scope node size change and exit node position changes
            m_scopeNode.DataWithSize.PropertyChanged += ScopeNodeData_PropertyChanged;
            m_scopeNode.ExitDecisionNode.Data.PropertyChanged += ExitDecisionNodeData_PropertyChanged;

            //intially move node to the border
            MoveNodeAlongScopeBorder();
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ExitDecisionNode Data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ExitDecisionNodeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
            {
                MoveNodeAlongScopeBorder();
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ScopeNode Data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ScopeNodeData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width" || e.PropertyName == "Height" || e.PropertyName == "X" || e.PropertyName == "Y")
            {
                MoveNodeAlongScopeBorder();
            }
        }

        /// <summary>
        /// Moves the node along scope border.
        /// </summary>
        private void MoveNodeAlongScopeBorder()
        {
            Point? intersection = ScopeHelper.GetIntersection(m_scopeNode, m_scopeNode.ExitDecisionNode, 15, 15, 15, 15, m_labelHeight);
            if (intersection != null)
            {
                m_exitScopeNode.Data.X = intersection.Value.X;
                m_exitScopeNode.Data.Y = intersection.Value.Y;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Any/All button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is any all button enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnyAllButtonEnabled
        {
            get;
            set;
        }
    }
}

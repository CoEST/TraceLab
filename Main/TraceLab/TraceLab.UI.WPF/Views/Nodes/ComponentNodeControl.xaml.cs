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
using System.Windows.Input;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Interaction logic for ComponentNodeControl.xaml
    /// </summary>
    [TemplatePart(Name = "PART_NodeControl", Type = typeof(TraceLabSDK.IProgress))]
    public class ComponentNodeControl : Control
    {
        private TraceLabSDK.IProgress m_progress;

        public ComponentNodeControl()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Template != null)
            {
                if (ComponentNode != null)
                {
                    //find actual component control (without adorners)
                    Control nodeControl = (Control)Template.FindName("PART_NodeControl", this);
                    AttachMouseDoubleClick(nodeControl);

                    //also get the progress
                    SetProgress(nodeControl);
                }    
            }
        }
        
        private void AttachMouseDoubleClick(Control nodeControl)
        {
            nodeControl.MouseDoubleClick += new MouseButtonEventHandler(componentControl_MouseDoubleClick);

            //find toogleInfoButton and its command, so that we can execute it on the MouseDoubleClick event
            var infoToggleButton = (ToggleButton)nodeControl.FindName("PART_ToogleInfoButton");

            m_toogleInfoCommand = infoToggleButton.Command;
        }

        private void SetProgress(Control nodeControl)
        {
            m_progress = (TraceLabSDK.IProgress)nodeControl.FindName("PART_Progress");
            ComponentNode.Progress = m_progress;
        }

        private ICommand m_toogleInfoCommand;
        private void componentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (m_toogleInfoCommand != null)
            {
                DependencyObject obj = e.OriginalSource as DependencyObject;
                var control = obj.GetParent<GraphSharp.Controls.VertexControl>(null);
                m_toogleInfoCommand.Execute(control);
            }
        }
        
        public static readonly DependencyProperty ComponentNodeProperty = DependencyProperty.Register("ComponentNode", typeof(ExperimentNode), typeof(ComponentNodeControl), new UIPropertyMetadata(OnComponentNodePropertyChanged));

        public ExperimentNode ComponentNode
        {
            get { return (ExperimentNode)GetValue(ComponentNodeProperty); }
            set { SetValue(ComponentNodeProperty, value); }
        }

        private static void OnComponentNodePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = (ComponentNodeControl)sender;

            if (args.OldValue != null)
            {
                var oldMetadata = (ExperimentNode)args.OldValue;
                oldMetadata.Progress = null;
            }

            if (control.m_progress != null && args.NewValue != null)
            {
                var newMetadata = (ExperimentNode)args.NewValue;
                newMetadata.Progress = control.m_progress;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether control should display button that allows creating link
        /// </summary>
        /// <value>
        /// 	<c>true</c> displays link creation button; otherwise does not display it.
        /// </value>
        public bool DisplayLinkCreationButton
        {
            get;
            set;
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

        /// <summary>
        /// Gets or sets a value indicating whether control should display button that allows viewing node info pane
        /// </summary>
        /// <value>
        /// 	<c>true</c> displays toogle info button; otherwise does not display it.
        /// </value>
        public bool DisplayToogleInfoButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether control should display button that allows viewing component subgraph
        /// </summary>
        /// <value>
        /// 	<c>true</c> displays magnifier button; otherwise does not display it.
        /// </value>
        public bool DisplayMagnifierButton
        {
            get;
            set;
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

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ComponentNodeControl));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}

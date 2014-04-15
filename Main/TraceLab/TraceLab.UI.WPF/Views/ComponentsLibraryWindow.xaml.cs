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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using AvalonDock;
using TraceLab.UI.WPF.Utilities;
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class ComponentsLibraryWindow : DockableContent
    {
        private Point m_dragOrigin;
        private readonly Point NullOrigin = new Point(double.NaN, double.NaN);
        private object m_draggingComponentDefinition = null;

        public ComponentsLibraryWindow()
        {
            InitializeComponent();
            m_dragOrigin = NullOrigin;
        }
        
        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_dragOrigin != NullOrigin && m_draggingComponentDefinition != null)
            {
                Point current = e.GetPosition(ComponentLibrary);
                if (Math.Abs(current.X - m_dragOrigin.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(current.Y - m_dragOrigin.Y) >= SystemParameters.MinimumVerticalDragDistance)
                {
                    IDataObject obj = new DataObject();
                    obj.SetData(System.Windows.DataFormats.GetDataFormat("ComponentDefinition").Name, m_draggingComponentDefinition);
                    DragDrop.DoDragDrop(ComponentLibrary, obj, DragDropEffects.Copy);
                    m_dragOrigin = NullOrigin;
                }
            }
        }

        private void ListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_dragOrigin = e.GetPosition(ComponentLibrary);
            var node = GetClickedNode<TraceLab.UI.WPF.ViewModels.CLVComponentNode>(e.GetPosition(ComponentLibrary));
            if (node != null)
            {
                m_draggingComponentDefinition = node.Component;
            }
        }

        private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_dragOrigin = NullOrigin;
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            var expression = FilterBox.GetBindingExpression(TraceLab.UI.WPF.Controls.SearchTextBox.TextProperty);
            //expression.UpdateSource();
        }

        private T GetClickedNode<T>(Point position) where T : TraceLab.UI.WPF.ViewModels.CLVBaseNode
        {
            T node = null;

            DependencyObject clickedVisualElement = ComponentLibrary.InputHitTest(position) as DependencyObject;
            TreeViewItem itemContainer = null;
            if (clickedVisualElement != null)
            {
                itemContainer = clickedVisualElement.GetParent<TreeViewItem>(this);
            }

            if (clickedVisualElement != null && itemContainer != null)
            {
                var parent = itemContainer.GetParent<TreeViewItem>(this);
                var generator = ComponentLibrary.ItemContainerGenerator;
                if (parent != null)
                {
                    generator = parent.ItemContainerGenerator;
                }

                var clickedItem = generator.ItemFromContainer(itemContainer);
                node = clickedItem as T;
            }

            return node;
        }

        private void OpenImportManager_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TraceLab.UI.WPF.ViewModels.ComponentsLibraryViewModelWrapper;
            if (viewModel != null)
            {
                OpenPkgReferenceManagerWindow(viewModel);
            }
        }

        private void PART_RemoveImportButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TraceLab.UI.WPF.ViewModels.ComponentsLibraryViewModelWrapper;
            if (viewModel != null)
            {
                Button removeButton = (Button)sender;
                CLVReferenceNode reference = (CLVReferenceNode)removeButton.DataContext;

                viewModel.Experiment.References.Remove(reference.Reference);
            }
        }
        
        /// <summary>
        /// Disabled Dock as Tabbed Document in the context menu of the window
        /// </summary>
        protected override bool CanExecuteCommand(ICommand command)
        {
            if (command == DockableContentCommands.ShowAsDocument)
            {
                if (DockableStyle == DockableStyle.DockableToBorders)
                {
                    return false;
                }
                if (State == DockableContentState.Document)
                {
                    return false;
                }
            } 
            
            return base.CanExecuteCommand(command);
        }

        /// <summary>
        /// Opens the Package Reference Manager Window.
        /// </summary>
        /// <param name="ComponentsLibraryVMWrapper">The Components Library ViewModel Wrapper.</param>
        private void OpenPkgReferenceManagerWindow(ComponentsLibraryViewModelWrapper ComponentsLibraryVMWrapper)
        {
            var model = new PackageManagerViewModel(ComponentsLibraryVMWrapper.Experiment);

            ExperimentImportManagerView view = new ExperimentImportManagerView();
            view.DataContext = model;

            Window importManager = new Window();
            importManager.Title = "Package Reference Manager: " + ComponentsLibraryVMWrapper.Experiment.ExperimentInfo.Name;
            importManager.Icon = new BitmapImage(new Uri("pack://application:,,,/TraceLab.UI.WPF;component/Resources/Icon_PkgReference16.png"));
            importManager.Content = view;
            importManager.Owner = this.GetParent<Window>(null);
            importManager.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            importManager.SizeToContent = SizeToContent.WidthAndHeight;
            importManager.ResizeMode = ResizeMode.NoResize;

            importManager.ShowDialog();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}

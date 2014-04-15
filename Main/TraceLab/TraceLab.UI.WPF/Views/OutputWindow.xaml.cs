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

using AvalonDock;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : DockableContent
    {
        public OutputWindow()
        {
            InitializeComponent();
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

        private void ToolBar_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ViewExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TraceLab.Core.ViewModels.ComponentLogInfo logInfo = button.DataContext as TraceLab.Core.ViewModels.ComponentLogInfo;
            if (logInfo != null)
            {
                ComponentExceptionDisplay display = new ComponentExceptionDisplay();
                display.ShowInTaskbar = false;
                display.ShowActivated = true;
                display.Owner = this.GetParentManager(null).GetParent<MainWindowBase>(null);
                display.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                
                //create view model
                var exceptionViewModel = new TraceLab.UI.WPF.ViewModels.ComponentExceptionDisplayViewModel(logInfo);

                display.DataContext = exceptionViewModel;
                
                display.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
                display.Title = "Component exception details";
                
                display.ShowDialog();
            }
        }
    }
}

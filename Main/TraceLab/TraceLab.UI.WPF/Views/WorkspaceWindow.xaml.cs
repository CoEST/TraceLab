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
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvalonDock;
using TraceLab.Core.Workspaces;
using TraceLab.UI.WPF.ViewModels;
using TraceLabSDK;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class WorkspaceWindow : DockableContent
    {
        public WorkspaceWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(WorkspaceWindow_Loaded);
        }

        void WorkspaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayErrors();
        }

        private void tracesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if(grid != null)
            {
                var unit = grid.SelectedItem as WpfWorkspaceUnitWrapper;
                if(unit != null && unit.Type != null && unit.Data != null)
                {
                    //also first try to display it with wpf method, and eventually fallback to windows form display
                    var displaysFuncs = new WorkspaceViewerLoader.DisplayEditor[]
                    {
                        DisplayWPFWindow,
                        WorkspaceViewerLoader.DisplayWindowsFormEditor
                    };

                    string error = String.Empty;

                    //try find first wpf specific gui, and then default gui
                    if (!WorkspaceViewerLoader.LoadViewer(unit.Data, unit.FriendlyUnitName, WorkspaceUIAssemblyExtensions.Extensions, displaysFuncs, out error))
                    {
                        NLog.LogManager.GetCurrentClassLogger().Warn(error);
                        MessageBox.Show(error);
                    }   
                }
            }
        }

        /// <summary>
        /// Displays the WPF window.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <returns>if window has been displayed, otherwise false</returns>
        private bool DisplayWPFWindow(IWorkspaceUnitEditor editor, String windowTitle) 
        {
            FrameworkElement frameworkElement = editor as FrameworkElement;
            if (frameworkElement != null)
            {
                Window wrapperWindow = new Window
                {
                    Content = editor,
                    Owner = Application.Current.MainWindow,
                    Title = windowTitle
                };

                wrapperWindow.ShowInTaskbar = false;
                wrapperWindow.ShowActivated = true;
                wrapperWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                wrapperWindow.ShowDialog();

                return true;
            }

            return false;
        }
        
        private void DisplayErrors()
        {
            var appModel = DataContext as TraceLab.UI.WPF.ViewModels.ApplicationViewModelWrapper;
            var viewModel = appModel.WorkspaceViewModel;
            if (viewModel != null && viewModel.TypeDefinitionErrors != null && viewModel.TypeDefinitionErrors.FirstOrDefault() != null)
            {
                var errors = new List<string>(viewModel.TypeDefinitionErrors);
                viewModel.ClearLoadErrors();
                Window errorWindow = new Window();
                var errorControl = new ComponentLibraryErrorDisplay();
                errorWindow.Content = errorControl;
                errorControl.HeaderText = "Errors encountered while loading component definitions: ";
                errorControl.Errors = errors;
                errorWindow.Height = 500;
                errorWindow.Width = 800;
                errorWindow.Owner = this.GetParent<MainWindow>(null);
                errorWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                errorWindow.Title = "Definition Load Errors";
                errorWindow.ShowDialog();
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
    }
}

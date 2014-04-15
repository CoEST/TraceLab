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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TraceLab.Core.Components;
using TraceLab.UI.WPF.ViewModels;
using TraceLabSDK.Component.Config;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for FolderPickerDialog.xaml
    /// </summary>
    public partial class FolderPickerDialog : Window
    {
        public FolderPickerDialog()
        {
            InitializeComponent();
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(DirectoryPath), typeof(FolderPickerDialog), new PropertyMetadata(PathChanged));
        public DirectoryPath Path
        {
            get { return (DirectoryPath)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        private static void PathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FolderPickerDialog dialog = (FolderPickerDialog)sender;
            if (args.NewValue != null)
            {
                MainWindow owner = dialog.Owner as MainWindow;
                TraceLab.Core.Components.TraceLabFilePath filePath = args.NewValue as TraceLab.Core.Components.TraceLabFilePath;
            }
        }

        #region TreeView Functions

        /// <summary>
        /// Handles the visibility change of the Tree View.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void TreeView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PackageFileChooser.IsVisible)
            {
                SelectButton.IsEnabled = false;
                var expression = PackageFileChooser.GetBindingExpression(TreeView.ItemsSourceProperty);
                expression.UpdateTarget();
            }
            else
            {
                SelectButton.IsEnabled = Directory.Exists(PathTextBox.Text);
            }
        }

        private static bool SelectTarget(PackageContentItem model, string target)
        {
            bool found = false;
            PackageFileContentItem file = model as PackageFileContentItem;
            PackageFolderContentItem folder = model as PackageFolderContentItem;
            if (file != null && file.ID == target)
            {
                file.IsSelected = true;
                found = true;
            }
            else if (folder != null)
            {
                foreach (PackageContentItem child in folder.Contents)
                {
                    if (SelectTarget(child, target))
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// If the chosen item is a Package or Folder then the Select Button is enabled, and the correct
        /// path is set.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedPropertyChangedEventArgs&lt;System.Object&gt;"/> instance containing the event data.</param>
        private void PkgFolderChooser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            bool enableButton = false;
            PackageFolderContentItem folder = PackageFileChooser.SelectedItem as PackageFolderContentItem;
            if (folder != null && !(folder is PackageViewModel))
            {
                Stack<string> subDirs = new Stack<string>();
                subDirs.Push(folder.Label);

                PackageContentItem parent = folder.Parent;
                while (parent.Parent != null)
                {
                    subDirs.Push(parent.Label);
                    parent = parent.Parent;
                }

                PackageViewModel package = parent as PackageViewModel;
                if (package != null)
                {
                    string subDirectory = "";
                    while (subDirs.Count > 0)
                    {
                        subDirectory = System.IO.Path.Combine(subDirectory, subDirs.Pop());
                    }

                    if (Directory.Exists(System.IO.Path.Combine(package.Package.Location, subDirectory)))
                    {
                        TraceLabSDK.PackageSystem.IPackageReference reference = new TraceLab.Core.PackageSystem.PackageReference(package.Package);
                        TraceLabDirectoryPath path = new TraceLabDirectoryPath();
                        path.Init(reference, subDirectory);
                        Path = path;
                        enableButton = true;
                    }
                }
            }
            else if (folder != null && folder is PackageViewModel)
            {
                PackageViewModel package = folder as PackageViewModel;
                if (Directory.Exists(package.Package.Location))
                {
                    TraceLabSDK.PackageSystem.IPackageReference reference = new TraceLab.Core.PackageSystem.PackageReference(package.Package);
                    TraceLabDirectoryPath path = new TraceLabDirectoryPath();
                    path.Init(reference, ".");
                    Path = path;
                    enableButton = true;
                }
            }
            SelectButton.IsEnabled = enableButton;
        }

        #endregion

        #region Expanding & Collapsing
        // Functions for controlling the expanding/collapsing actions for the Expanders

        private void DiskExpander_Expanded(object sender, RoutedEventArgs e)
        {
            PackageExpander.IsExpanded = false;
            SelectButton.IsEnabled = Directory.Exists(PathTextBox.Text);
        }

        private void DiskExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            PackageExpander.IsExpanded = true;
        }

        private void PackageExpander_Expanded(object sender, RoutedEventArgs e)
        {
            DiskExpander.IsExpanded = false;
        }

        private void PackageExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            DiskExpander.IsExpanded = true;
        }

        private void FolderPickerLoaded(object sender, RoutedEventArgs e)
        {
            DiskExpander.IsExpanded = true;
            PackageExpander.IsExpanded = false;
        }

        #endregion

        #region GUI Events

        /// <summary>
        /// Opens a folder browser dialog when the button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            folderDialog.ShowNewFolderButton = true;

            bool? success = folderDialog.ShowDialog();
            if (success == true && Directory.Exists(folderDialog.SelectedPath))
            {
                try
                {
                    DirectoryPath path = new DirectoryPath();
                    path.Init(folderDialog.SelectedPath, Path.DataRoot);
                    Path = path;
                }
                catch (System.ArgumentException ex)
                {
                    NLog.LogManager.GetCurrentClassLogger().ErrorException("Failed to set reference to a file", ex);
                    MessageBox.Show(ex.Message, "Failed to set reference to a file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        /// <summary>
        /// Closes the FolderPickerDialog with a success
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SelectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Checks if path entered in TextBox exists, if so then it enables the SelectButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectButton.IsEnabled = Directory.Exists(PathTextBox.Text);
        }

        #endregion
    }
}

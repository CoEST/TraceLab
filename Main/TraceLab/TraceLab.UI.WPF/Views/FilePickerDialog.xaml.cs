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
using System.Windows.Data;
using TraceLabSDK.Component.Config;
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for FilePickerDialog.xaml
    /// </summary>
    public partial class FilePickerDialog : Window
    {
        public FilePickerDialog()
        {
            InitializeComponent();
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(FilePath), typeof(FilePickerDialog), new PropertyMetadata(PathChanged));
        public FilePath Path
        {
            get { return (FilePath)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        private static void PathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FilePickerDialog dialog = (FilePickerDialog)sender;
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
                SelectButton.IsEnabled = File.Exists(PathTextBox.Text);
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
        /// If the chosen item is a file then the Select Button is enabled, and the correct path is set.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedPropertyChangedEventArgs&lt;System.Object&gt;"/> instance containing the event data.</param>
        private void PkgFileChooser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var file = PackageFileChooser.SelectedItem as PackageFileContentItem;
            if (file != null)
            {
                SelectButton.IsEnabled = true;
                PackageViewModel package = null;
                // Get the Package parent
                PackageContentItem parent = file.Parent;
                package = parent as PackageViewModel;
                while (package == null && parent != null)
                {
                    parent = parent.Parent;
                    package = parent as PackageViewModel;
                }

                if (package != null)
                {
                    TraceLabSDK.PackageSystem.IPackageReference reference = new TraceLab.Core.PackageSystem.PackageReference(package.Package);

                    var path = new TraceLab.Core.Components.TraceLabFilePath();
                    path.Init(reference, file.ID);
                    Path = path;
                }
            }
            else
            {
                SelectButton.IsEnabled = false;
            }
        }

        #endregion

        #region Expanding & Collapsing
        // Functions for controlling the expanding/collapsing actions for the Expanders

        private void DiskExpander_Expanded(object sender, RoutedEventArgs e)
        {
            PackageExpander.IsExpanded = false;
            SelectButton.IsEnabled = File.Exists(PathTextBox.Text);
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

        private void FilePickerLoaded(object sender, RoutedEventArgs e)
        {
            DiskExpander.IsExpanded = true;
            PackageExpander.IsExpanded = false;
        }

        #endregion

        #region GUI Events

        /// <summary>
        /// Opens a file browser dialog when the button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFileDialog fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            
            //don't check if file exists, as dialog might be used for specifying the output filepath
            fileDialog.CheckFileExists = false;
            fileDialog.Title = "Select File";

            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                try
                {
                    FilePath path = new FilePath();
                    path.Init(fileDialog.FileName, Path.DataRoot);
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
        /// Closes the FilePice
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void SelectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Checks if path entered in TextBox is absolute. 
        /// File doesn't have to exist as user might want to specify an output path.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool enabled = false;
            try
            {
                enabled = System.IO.Path.IsPathRooted(PathTextBox.Text);
            } 
            catch(ArgumentException)
            {
                //throws if path contains one or more of the invalid characters defined in GetInvalidPathChars.
            }

            SelectButton.IsEnabled = enabled;
        }

        #endregion
    }

    public class ReferenceResolver : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var packageReferences = (IEnumerable<TraceLabSDK.PackageSystem.IPackageReference>)value;
            var packages = new List<TraceLab.UI.WPF.ViewModels.PackageViewModel>();
            if (packageReferences != null)
            {
                foreach (TraceLabSDK.PackageSystem.IPackageReference reference in packageReferences)
                {
                    var pack = TraceLab.Core.PackageSystem.PackageManager.Instance.GetPackage(reference);
                    if (pack != null)
                    {
                        packages.Add(new TraceLab.UI.WPF.ViewModels.PackageViewModel(pack));
                    }
                }
            }

            return packages;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

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
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;
using TraceLab.UI.WPF.Controls;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.Core.PackageSystem;
using TraceLab.Core.PackageBuilder;

namespace TraceLab.UI.WPF.Views.PackageBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PackageBuilderMainWindow : Window
    {
        /// <summary>
        /// Initializes a new empty instance of the Package Builder.
        /// </summary>
        public PackageBuilderMainWindow()
        {
            InitializeComponent();
            var info = new PackageBuilderViewModel();
            DataContext = info;
        }

        /// <summary>
        /// Initializes a new instance of the Package Builder with context set to the given experiment, 
        /// types and components.
        /// </summary>
        /// <param name="pInfo">Experiment info.</param>
        /// <param name="pTypes">Collection of type assembly files.</param>
        /// <param name="pComponents">Collection of component assembly files.</param>
        public PackageBuilderMainWindow(Experiment originalExperiment, Dictionary<string, Type> supportedTypes)
        {
            InitializeComponent();

            var info = new PackageBuilderViewModel(originalExperiment, supportedTypes);
            
            DataContext = info;
        }

        private void TreeListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                TreeListView view = (TreeListView)sender;

                //data context of the view is multibinding to two objects - the first one is the packageBuilder, 
                //the second one is that currentState (so that when it changes the view gets notification)
                List<object> values = view.DataContext as List<object>;
                if (values != null)
                {
                    PackageBuilderViewModel viewModel = (PackageBuilderViewModel)values[0];
                    PackageSourceInfo source = viewModel.PackageSourceInfo;
                    PackageFileSourceInfo info = view.SelectedItem as PackageFileSourceInfo;
                    if (info != source.Root && info != null)
                    {
                        info.Parent.Children.Remove(info);
                        view.Focus();
                    }
                }
            }
        }

        #region Drag & Drop

        static T GetItemAtLocation<T>(TreeView treeView, Point location)
        {
            T foundItem = default(T);
            HitTestResult hitTestResults = VisualTreeHelper.HitTest(treeView, location);

            if (hitTestResults.VisualHit is FrameworkElement)
            {
                object dataObject = (hitTestResults.VisualHit as FrameworkElement).DataContext;

                if (dataObject is T)
                {
                    foundItem = (T)dataObject;
                }
            }

            return foundItem;
        }

        private void TreeView_DragOver(object sender, DragEventArgs e)
        {
        }

        private void TreeViewItem_DragOver(object sender, DragEventArgs e)
        {
            DependencyObject realSource = e.Source as DependencyObject;
            var item = realSource.GetParent<TreeListViewItem>(null);
            if (item != null)
            {
                var file = item.DataContext as PackageFileSourceInfo;
                file.IsSelected = true;
            }
            else
            {
                var tree = realSource.GetParent<TreeListView>(null);
                if (tree != null)
                {
                    var source = item.DataContext as PackageSourceInfo;
                    source.Root.IsSelected = true;
                }
            }

            e.Handled = true;
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            var viewModel = (PackageBuilderViewModel)DataContext;
            var pkgInfo = viewModel.PackageSourceInfo;

            var target = (FrameworkElement)e.Source;
            PackageHeirarchyItem parent = target.DataContext as PackageHeirarchyItem;
            PackageFileSourceInfo file = target.DataContext as PackageFileSourceInfo;
            if (parent == null && file != null)
            {
                parent = file.Parent;
            }
            if (parent == null)
            {
                parent = pkgInfo.Root;
            }

            if (e.Data is System.Windows.DataObject && ((System.Windows.DataObject)e.Data).ContainsFileDropList())
            {
                foreach (string filePath in ((System.Windows.DataObject)e.Data).GetFileDropList())
                {
                    viewModel.Add(parent, filePath);
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// Hides the overlay displayed on top of the TreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TreeView_MouseLeftButtonClick(object sender, MouseButtonEventArgs e)
        {
            var treeViewBorder = sender as System.Windows.Controls.Border;
            if (treeViewBorder != null)
            {
                treeViewBorder.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        #endregion

        #region Build Package

        /// <summary>
        /// Opens a file dialog for selecting the package file destination.
        /// </summary>
        /// <param name="pFileName">Name of the package file.</param>
        /// <returns>Package file path.</returns>
        private string GetFilePath(string pFileName)
        {
            Ookii.Dialogs.Wpf.VistaSaveFileDialog fileDialog = new Ookii.Dialogs.Wpf.VistaSaveFileDialog();
            fileDialog.CheckFileExists = false;
            fileDialog.Title = "Save Package As";
            fileDialog.Filter = "TraceLab Package Files|*.tpkg";
            fileDialog.FileName = pFileName;
            fileDialog.DefaultExt = "tpkg";
            fileDialog.AddExtension = true;
            
            bool? success = fileDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                return fileDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Handles the Click event of the Build Package Button
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void BuildPkgButton_Click(object sender, RoutedEventArgs e)
        {
            MainBorder.Visibility = System.Windows.Visibility.Visible;

            var viewModel = (PackageBuilderViewModel)DataContext;
            PackageSourceInfo info = viewModel.PackageSourceInfo;
            bool noError = true;
            TraceLab.Core.PackageSystem.Package pkg = null;

            string pkgFilePath = GetFilePath(info.Name);
            if (pkgFilePath != null)
            {
                string pkgFileName = System.IO.Path.GetFileNameWithoutExtension(pkgFilePath);
                string pkgTempDirectory = pkgFilePath + "~temp";

                try
                {
                    System.IO.Directory.CreateDirectory(pkgTempDirectory);

                    try
                    {
                        pkg = new TraceLab.Core.PackageSystem.Package(info.Name, pkgTempDirectory, false);
                    }
                    catch (TraceLab.Core.PackageSystem.PackageAlreadyExistsException)
                    {
                        MessageBox.Show("Package already exists in: " + pkgTempDirectory,
                            "Package Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        noError = false;
                    }
                    catch (TraceLab.Core.PackageSystem.PackageException ex)
                    {
                        MessageBox.Show("Error creating package: " + ex.Message,
                            "Package Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        noError = false;
                    }

                    if (pkg != null && noError)
                    {
                        foreach (PackageFileSourceInfo item in info.Files)
                        {
                            try
                            {
                                noError = noError && PackageCreator.AddItemToPackage(pkg, item, viewModel.IsExperimentPackage);
                            }
                            catch (TraceLab.Core.Exceptions.PackageCreationFailureException ex)
                            {
                                MessageBox.Show("Error creating package: " + ex.Message,
                                    "Package Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                noError = false;
                            }
                        }

                        pkg.SaveManifest();

                        using (System.IO.FileStream stream = new System.IO.FileStream(pkgFilePath, System.IO.FileMode.Create))
                        {
                            pkg.Pack(stream);
                        }
                    }
                }
                catch (System.IO.IOException error)
                {
                    MessageBox.Show("Unable to create package. Error: " + error.Message,
                        "Package Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    noError = false;
                }
                catch (System.UnauthorizedAccessException error)
                {
                    MessageBox.Show("Unable to create package - Unauthorized access: " + error.Message,
                        "Package Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    noError = false;
                }

                try
                {
                    if (System.IO.Directory.Exists(pkgTempDirectory))
                    {
                        System.IO.Directory.Delete(pkgTempDirectory, true);
                    }
                }
                catch (System.IO.IOException error)
                {
                    MessageBox.Show("Unable to cleanup after package creation. Error: " + error.Message,
                        "After Package Cleanup Failure", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (System.UnauthorizedAccessException error)
                {
                    MessageBox.Show("Unable to cleanup after package creation. Unauthorized access: " + error.Message,
                        "After Package Cleanup Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            if (noError && pkg != null)
            {
                MessageBox.Show("Package \"" + info.Name + "\" was built successfully.", "Package Created",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            MainBorder.Visibility = System.Windows.Visibility.Hidden;
        }

        #endregion

        private void GenerateExperimentPackageButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (PackageBuilderViewModel)DataContext;
            viewModel.GeneratePackageContent(); //it sets the root package source info
            viewModel.CurrentState = PackageBuilderWizardPage.FileViewer;
        }
    }

    internal static class WpfHelpers
    {
        public static T GetParent<T>(this DependencyObject obj, DependencyObject limit) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            var targetType = typeof(T);
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null && !targetType.IsAssignableFrom(parent.GetType()))
            {
                if (parent == limit)
                {
                    parent = null;
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }

        public static T GetParent<T>(this DependencyObject obj, DependencyObject limit, int ancestorLevel) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            int level = 0;
            while (parent != null)
            {
                if (parent == limit)
                {
                    parent = null;
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);

                if (parent.GetType() == typeof(T))
                {
                    level++;
                }

                if (level == ancestorLevel)
                {
                    break; //parent found
                }
            }

            return parent as T;
        }
    }
}

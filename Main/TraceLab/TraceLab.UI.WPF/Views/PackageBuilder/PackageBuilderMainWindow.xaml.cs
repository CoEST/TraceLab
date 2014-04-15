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
using TraceLab.UI.WPF.Views.PackageBuilder.PackageSource;
using TraceLab.Core.PackageSystem;

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
            this.isExperimentPackage = true;

            var info = new PackageBuilderViewModel(originalExperiment, supportedTypes);
            DataContext = info;
        }

        #region Experiment Package

        /// <summary>
        /// Especifies if the package is a self-contained experiment package
        /// </summary>
        private bool isExperimentPackage = false;

        /// <summary>
        /// Sets the contents of the package from the given experiment, types and components.
        /// </summary>
        /// <param name="pInfo">Experiment info.</param>
        /// <param name="pTypes">Collection of type assembly files.</param>
        /// <param name="pComponents">Collection of component assembly files.</param>
        /// <returns></returns>
        private PackageSourceInfo SetPackageContent(ExperimentPackagingResults pResults)
        {
            var info = new PackageSourceInfo();

            // Adding components
            if (pResults.ComponentAssemblies.Count > 0)
            {
                PackageHeirarchyItem componentsFolder = CreateFolder(info.Root, "Components");
                componentsFolder.HasComponents = true;
                foreach (string component in pResults.ComponentAssemblies)
                {
                    AddFile(componentsFolder, component);
                }
            }

            // Adding experiment
            PackageHeirarchyItem experimentFolder = CreateFolder(info.Root, "Experiment");
            AddFile(experimentFolder, pResults.Experiment.ExperimentInfo.FilePath);
            info.Name = pResults.Experiment.ExperimentInfo.Name + " Package";

            // Adding refered types into subfolder of Experiment
            if (pResults.Files.Count > 0)
            {
                foreach (PackageFileInfo file in pResults.Files)
                {
                    PackageHeirarchyItem lastFolder = CreateRelativeFolders(experimentFolder, file);
                    AddFile(lastFolder, file.AbsoluteLocation);
                }
            }

            //Adding refered directories into subfolder of Experiment
            if (pResults.Directories.Count > 0)
            {
                foreach (PackageFileInfo dir in pResults.Directories)
                {
                    PackageHeirarchyItem lastFolder = CreateRelativeFolders(experimentFolder, dir);
                    AddFolder(lastFolder, dir.AbsoluteLocation);
                }
            }

            // Adding types
            if (pResults.TypeAssemblies.Count > 0)
            {
                PackageHeirarchyItem typesFolder = CreateFolder(info.Root, "Types");
                typesFolder.HasTypes = true;
                foreach (string type in pResults.TypeAssemblies)
                {
                    AddFile(typesFolder, type);
                }
            }

            return info;
        }

        private PackageHeirarchyItem CreateRelativeFolders(PackageHeirarchyItem experimentFolder, PackageFileInfo file)
        {
            PackageHeirarchyItem lastFolder = experimentFolder;
            foreach (string folder in file.FoldersPath)
            {
                PackageHeirarchyItem folderInfo;
                if (ContainsFolder(lastFolder, folder, out folderInfo))
                {
                    lastFolder = folderInfo;
                }
                else
                {
                    lastFolder = CreateFolder(lastFolder, folder);
                }
            }
            return lastFolder;
        }

        #endregion

        #region Tree Manipulation

        /// <summary>
        /// Creates an empty folder inside the PackageHeirarchyItem provided.
        /// </summary>
        /// <param name="pParent">Parent node of the new folder.</param>
        /// <param name="pFolderName">Name of the folder.</param>
        /// <returns></returns>
        private PackageHeirarchyItem CreateFolder(PackageHeirarchyItem pParent, string pFolderName)
        {
            PackageHeirarchyItem newFolder = new PackageHeirarchyItem("");
            newFolder.Name = pFolderName;
            newFolder.Parent = pParent;
            pParent.Children.Add(newFolder);

            return newFolder;
        }

        private void Add(PackageHeirarchyItem parent, string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                AddFile(parent, filePath);
            }
            else
            {
                AddFolder(parent, filePath);
            }
        }

        private void AddFile(PackageHeirarchyItem parent, string filePath)
        {
            var name = System.IO.Path.GetFileName(filePath);
            if (parent != null && !Contains(parent, name))
            {
                var newItem = new PackageFileSourceInfo(filePath);
                newItem.Name = name;
                newItem.Parent = parent;

                parent.Children.Add(newItem);
            }
        }

        private void AddFolder(PackageHeirarchyItem parent, string filePath)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(filePath);
            if (parent != null && !Contains(parent, dir.Name))
            {
                var newItem = new PackageHeirarchyItem(filePath);
                newItem.Name = dir.Name;
                newItem.Parent = parent;

                parent.Children.Add(newItem);

                foreach (System.IO.FileInfo file in dir.EnumerateFiles())
                {
                    AddFile(newItem, file.FullName);
                }

                foreach (System.IO.DirectoryInfo subDir in dir.EnumerateDirectories())
                {
                    AddFolder(newItem, subDir.FullName);
                }
            }
        }

        static bool Contains(PackageHeirarchyItem info, string name)
        {
            bool contains = false;
            foreach (PackageFileSourceInfo item in info.Children)
            {
                if (string.Equals(name, item.Name))
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        static bool ContainsFolder(PackageHeirarchyItem info, string name, out PackageHeirarchyItem resultFolder)
        {
            resultFolder = null;
            bool contains = false;
            foreach (PackageFileSourceInfo item in info.Children)
            {
                if (string.Equals(name, item.Name))
                {
                    resultFolder = item as PackageHeirarchyItem;
                    //if resultFolder now is different than null it means there is such folder in hierarchy,
                    //otherwise there is file of same name - so return false
                    if (resultFolder != null)
                    {
                        contains = true;
                    }
                    break;
                }
            }
            return contains;
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

        #endregion

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
                    Add(parent, filePath);
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
                            noError = noError && AddItemToPackage(pkg, item);
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

        /// <summary>
        /// Adds item and its content (file or folder) to package.
        /// </summary>
        /// <param name="pPkg">Package being created.</param>
        /// <param name="item">Item to be added.</param>
        /// <returns>True is there was no error during the operation, false otherwise.</returns>
        private bool AddItemToPackage(TraceLab.Core.PackageSystem.Package pkg, PackageFileSourceInfo item)
        {
            bool noError = true;
            string targetPath = System.IO.Path.Combine(pkg.Location, item.GetPath());
 
            PackageHeirarchyItem dir = item as PackageHeirarchyItem;
            if (dir != null)
            {
                if (item.Parent != null)
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                foreach (PackageFileSourceInfo child in dir.Children)
                {
                    noError = noError && AddItemToPackage(pkg, child);
                }

                if (dir.HasComponents)
                {
                    pkg.SetDirectoryHasComponents(dir.GetPath(), true);
                }
                if (dir.HasTypes)
                {
                    pkg.SetDirectoryHasTypes(dir.GetPath(), true);
                }
            }
            else
            {
                System.IO.File.Copy(item.SourceFilePath, targetPath);
                //Add reference to this created package to all experiments and composite components
                if (this.isExperimentPackage && targetPath.EndsWith(".teml") || targetPath.EndsWith(".tcml"))
                {   
                    noError = noError && AddPkgRefToExperiment(pkg, targetPath);
                }
                System.IO.File.SetAttributes(targetPath, System.IO.File.GetAttributes(targetPath) & ~System.IO.FileAttributes.ReadOnly);
                pkg.AddFile(targetPath);
            }

            return noError;
        }

        /// <summary>
        /// Modifies experiment to add reference to new package.
        /// </summary>
        /// <param name="pPkg">The package being created.</param>
        /// <param name="pExperimentFile">The experiment file.</param>
        /// <returns>True is there was no error during the operation, false otherwise.</returns>
        private bool AddPkgRefToExperiment(TraceLab.Core.PackageSystem.Package pPkg, string pExperimentFile)
        {
            bool noError = true;
            if (System.IO.File.Exists(pExperimentFile))
            {
                try
                {
                    XmlDocument xmlExperiment = new XmlDocument();
                    xmlExperiment.Load(pExperimentFile);

                    XmlNode nodeReferences = xmlExperiment.SelectSingleNode("//References");

                    XmlElement newPkgReference = xmlExperiment.CreateElement("PackageReference");
                    newPkgReference.SetAttribute("ID", pPkg.ID);
                    newPkgReference.SetAttribute("Name", pPkg.Name);
                    nodeReferences.AppendChild(newPkgReference);

                    xmlExperiment.Save(pExperimentFile);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to modify experiment - reference to new package could not be added.",
                        "Package Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    noError = false;
                }
            }
            else
            {
                noError = false;
            }
            return noError;
        }

        #endregion

        private void GenerateExperimentPackageButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (PackageBuilderViewModel)DataContext;
            var ePkgResults = viewModel.Pack();
            PackageSourceInfo pkgInfo = SetPackageContent(ePkgResults);
            viewModel.PackageSourceInfo = pkgInfo;
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

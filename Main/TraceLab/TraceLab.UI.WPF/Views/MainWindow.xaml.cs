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
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.UI.WPF.Views.PackageBuilder;
using TraceLab.Core.ViewModels;
using TraceLab.UI.WPF.Controls;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MainWindowBase
    {
        private bool m_isLoaded;

        // Override metadata so that we get property changed notifications.
        static MainWindow()
        {
            PropertyMetadata data = DataContextProperty.GetMetadata(typeof(MainWindow));
            DataContextProperty.OverrideMetadata(typeof(MainWindow), 
                new FrameworkPropertyMetadata(  data.DefaultValue, FrameworkPropertyMetadataOptions.Inherits, OnDataContextPropertyChanged));
        }

        private static void OnDataContextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MainWindow window = sender as MainWindow;

            ApplicationViewModelWrapper oldWrapper = args.OldValue as ApplicationViewModelWrapper;
            ApplicationViewModelWrapper newWrapper = args.NewValue as ApplicationViewModelWrapper;

            // Detach the old
            if (oldWrapper != null)
            {
                if (oldWrapper.ExperimentViewModel != null)
                {
                    oldWrapper.ExperimentViewModel.ExperimentCompleted -= window.vm_ExperimentCompleted;
                }
                oldWrapper.UnhandledException -= window.newWrapper_UnhandledException;
            }

            // Attach the new
            if (newWrapper != null)
            {
                if (newWrapper.ExperimentViewModel != null)
                {
                    newWrapper.ExperimentViewModel.ExperimentCompleted += window.vm_ExperimentCompleted;
                }
                newWrapper.UnhandledException += window.newWrapper_UnhandledException;
            }
        }

        void newWrapper_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Action show = () =>
            {
                Exception exception = (Exception)e.ExceptionObject;
                //exception = exception.GetBaseException();

                ExceptionDisplay display = new ExceptionDisplay();
                display.ShowInTaskbar = false;
                display.ShowActivated = true;
                display.Owner = this;
                display.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                display.DataContext = exception;
                display.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
                display.Title = "An unhandled exception occurred";

                display.ShowDialog();
            };

            Dispatcher.Invoke(show);
        }

        internal MainWindow(ApplicationViewModelWrapper context)
        {
            InitializeComponent();
            DataContext = context;

            this.Closing += MainWindow_Closing;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            DockManager.Loaded += new RoutedEventHandler(DockManager_Loaded);
            DockManager.DeserializationCallback = DockManager_DeserializationHelper;
        }

        void DockManager_DeserializationHelper(object sender, AvalonDock.DeserializationCallbackEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Name);
        }

        void DockManager_Loaded(object sender, RoutedEventArgs e)
        {
            //Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(LoadLayout));
        }

        void LoadLayout()
        {
            if (m_isLoaded == false)
            {
                try
                {
                    ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;

                    string layoutPath = TraceLab.Core.Settings.Settings.LayoutPath;
                    if (string.IsNullOrEmpty(layoutPath) == false && System.IO.File.Exists(layoutPath))
                    {
                        DockManager.RestoreLayout(layoutPath);
                    }

                    m_isLoaded = true;
                }
                catch (Exception ex)
                {
                    NLog.LogManager.GetCurrentClassLogger().ErrorException("Layout failed to be restored from previously saved layout.xml (in %APPDATA%/TraceLab). File was corrupted. Application restored default layout.", ex);
                }
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            this.Left = appvm.SettingsViewModel.MainWindowRect.Left;
            this.Top = appvm.SettingsViewModel.MainWindowRect.Top;
            this.Width = appvm.SettingsViewModel.MainWindowRect.Width;
            this.Height = appvm.SettingsViewModel.MainWindowRect.Height;

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(LoadLayout));
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;

            ExperimentViewModel experimentVM = null;
            if (appvm.ExperimentDocumentWrapper != null)
            {
                experimentVM = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;
                if (experimentVM != null && experimentVM.IsModified)
                {
                    var result = MessageBox.Show(TraceLab.Core.Messages.ClosingUnsavedDocumentWarning,
                                    "Modified Experiment Not Saved", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            appvm.SettingsViewModel.MainWindowRect.Height = Height;
            appvm.SettingsViewModel.MainWindowRect.Width = Width;
            appvm.SettingsViewModel.MainWindowRect.Left = Left;
            appvm.SettingsViewModel.MainWindowRect.Top = Top;

            appvm.SaveSettings();
            appvm.SaveUserTags();

            DockManager.SaveLayout(TraceLab.Core.Settings.Settings.LayoutPath);

            if (experimentVM != null && String.IsNullOrEmpty(experimentVM.TopLevel.ExperimentInfo.FilePath) == true)
            {
                //experiment was never saved, so delete its units from the workspace
                appvm.WorkspaceViewModel.DeleteExperimentUnits();
            }
        }

        void vm_ExperimentCompleted(object sender, ExperimentEventArgs e)
        {
            Action method = () =>
            {
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Tick += (o, s) =>
                {
                    e.Dispatcher.Dispose();
                };

                timer.Start();
            };

            this.Dispatcher.BeginInvoke(method, null);
        }

        #region Open Component Graph Command

        private void ExecuteOpenComponentGraphCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (OpenComponentGraphCommand != null)
            {
                OpenComponentGraphCommand.Execute(e.Parameter);
            }
        }

        private void CanExecuteOpenComponentGraphCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (OpenComponentGraphCommand != null)
            {
                e.CanExecute = OpenComponentGraphCommand.CanExecute(e.Parameter);
            }
        }

        #endregion

        #region Execute Toggle Node Info

        private void ExecuteToggleNodeInfo(object sender, ExecutedRoutedEventArgs e)
        {
            ToggleInfoPaneForNodeCommand.Execute(e.Parameter);
        }

        #endregion

        #region Show DefineCompositeComponentWizardDialog

        private void CanShowDefineCompositeComponentWizard(object sender, CanExecuteRoutedEventArgs e)
        {
            bool canDo = false;
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            if (appvm != null && appvm.ExperimentDocumentWrapper != null && appvm.ExperimentDocumentWrapper.Count() > 0)
            {
                var experiment = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;
                if (experiment != null)
                {
                    bool isAnyNodeSelected = false;
                    var enumerator = experiment.TopLevel.Vertices.GetEnumerator();
                    while (isAnyNodeSelected == false && enumerator.MoveNext())
                    {
                        var node = enumerator.Current;
                        if (node is ExperimentStartNode == false && node is ExperimentEndNode == false)
                        {
                            isAnyNodeSelected = node.IsSelected;
                        }
                    }
                    canDo = isAnyNodeSelected;
                }
            }

            e.CanExecute = canDo;
        }

        private void ShowDefineCompositeComponentWizard(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            var experiment = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;

            if (experiment != null)
            {
                //create dialog with data contex of Define Benchmark
                var wizard = new DefineCompositeComponentWizard(this);

                Action<TraceLab.Core.Components.CompositeComponentMetadataDefinition> howToAddToComponentLibrary =
                    (TraceLab.Core.Components.CompositeComponentMetadataDefinition metadataDefinition) =>
                    {
                        appvm.ComponentLibraryViewModel.AddReplaceCompositeComponentMetadataDefinition(metadataDefinition);
                    };

                //create view model
                var setup = new DefiningCompositeComponentSetupViewModel(experiment, appvm.SettingsViewModel.ComponentPaths, howToAddToComponentLibrary);

                wizard.DataContext = setup;

                wizard.Owner = this;
                wizard.Show();
            }
        }

        #endregion

        #region Toolbar Buttons Functions

        /// <summary>
        /// Handles the Click event of the toolbar drowndown menu button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void TBButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                b.ContextMenu.PlacementTarget = (UIElement)b;
                b.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the ContextMenuOpening event of the toolbar drowndown menu button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.ContextMenuEventArgs"/> instance containing the event data.</param>
        private void TBButton_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the MenuItems inside the Help drowndown menu.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void HelpMenuItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                WebsiteLink link = (WebsiteLink)item.DataContext;
                Process.Start(new ProcessStartInfo(link.LinkURL));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Click event for opening the Package Builder.
        /// </summary>
        /// <param name="sender">The source of the event (Toolbar Button).</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OpenPkgBuilder_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                PackageBuilder.PackageBuilderMainWindow pkgBuilderWindow = new PackageBuilder.PackageBuilderMainWindow();
                pkgBuilderWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                pkgBuilderWindow.Show();
            }
        }

        #endregion

        /// <summary>
        /// Opens the About TraceLab Dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void AboutTraceLabMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AboutTraceLabContent info = new AboutTraceLabContent(TraceLab.Core.ViewModels.ApplicationViewModel.Version,
                                                                TraceLab.Core.ViewModels.ApplicationViewModel.RegisteredUser);
            AboutTraceLabDialog dialog = new AboutTraceLabDialog(this);
            dialog.DataContext = info;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();
        }

        private void CaptureScreenShot(object sender, ExecutedRoutedEventArgs e)
        {
            //Window is always visual so no need to check it
            //System.Windows.Media.Visual visual = this as System.Windows.Media.Visual;

            ScreenshotDialog dialog = new ScreenshotDialog();
            dialog.DataContext = this;
            dialog.ShowDialog();
            e.Handled = true;
        }

        private void CanCaptureScreenShot(object sender, CanExecuteRoutedEventArgs e)
        {
            //window is always visual so no need to check it
            //e.CanExecute = (this as System.Windows.Media.Visual) != null;
            e.CanExecute = true;
            e.Handled = true;
        }
    }

    public class MainWindowBase : Window
    {
        public MainWindowBase()
        {
            m_showBenchmarkWizardDialogCommand = new TraceLab.UI.WPF.Commands.DelegateCommand(ShowBenchmarkWizardDialog, CanShowBenchmarkWizard);
            m_showDefineBenchmarkDialogCommand = new TraceLab.UI.WPF.Commands.DelegateCommand(ShowDefineBenchmarkDialog, CanShowDefineBenchmarkDialog);
        }

        #region Dependency properties

        public static readonly DependencyProperty ToggleInfoPaneForNodeProperty = DependencyProperty.Register("ToggleInfoPaneForNodeCommand", typeof(ICommand), typeof(MainWindowBase));
        public static readonly DependencyProperty IsExperimentRunningProperty = DependencyProperty.Register("IsExperimentRunning", typeof(Boolean), typeof(MainWindowBase));
        public static readonly DependencyProperty OpenComponentGraphCommandProperty = DependencyProperty.Register("OpenComponentGraphCommand", typeof(ICommand), typeof(MainWindowBase));
        public static readonly DependencyProperty ProgressBarProperty = DependencyProperty.Register("ProgressBar", typeof(TraceLabSDK.IProgress), typeof(MainWindowBase));

        public TraceLabSDK.IProgress ProgressBar
        {
            get { return (TraceLabSDK.IProgress)GetValue(ProgressBarProperty); }
            set { SetValue(ProgressBarProperty, value); }
        }
        
        public ICommand ToggleInfoPaneForNodeCommand
        {
            get { return (ICommand)GetValue(ToggleInfoPaneForNodeProperty); }
            set { SetValue(ToggleInfoPaneForNodeProperty, value); }
        }

        public Boolean IsExperimentRunning
        {
            get { return (Boolean)GetValue(IsExperimentRunningProperty); }
            set { SetValue(IsExperimentRunningProperty, value); }
        }

        public ICommand OpenComponentGraphCommand
        {
            get { return (ICommand)GetValue(OpenComponentGraphCommandProperty); }
            set { SetValue(OpenComponentGraphCommandProperty, value); }
        }

        #endregion

        #region ShowBenchmarkWizardDialog

        private ICommand m_showBenchmarkWizardDialogCommand;
        public ICommand ShowBenchmarkWizardDialogCommand
        {
            get
            {
                return m_showBenchmarkWizardDialogCommand;
            }
        }

        private bool CanShowBenchmarkWizard(object param)
        {
            bool canDo = false;
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            if (appvm != null && appvm.ExperimentDocumentWrapper != null && appvm.ExperimentDocumentWrapper.Count() > 0)
            {
                var experimentToBeBenchmarked = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;
                canDo = experimentToBeBenchmarked != null;
            }

            return canDo;
        }

        private void ShowBenchmarkWizardDialog(object param)
        {
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            var experimentToBeBenchmarked = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;

            if (experimentToBeBenchmarked != null)
            {
                //start wizard - load benchmarks from benchmarks directory, initiates wizard state
                appvm.BenchmarkWizardViewModel.StartWizard((Experiment)experimentToBeBenchmarked.TopLevel);

                //create wizard with data contex of benchmark wizard view model
                BenchmarkWizardDialog box = new BenchmarkWizardDialog(this);
                box.DataContext = appvm.BenchmarkWizardViewModel;
                box.ShowDialog();
            }
        }

        #endregion

        #region ShowDefineBenchmarkWindow

        private ICommand m_showDefineBenchmarkDialogCommand;
        public ICommand ShowDefineBenchmarkDialogCommand
        {
            get
            {
                return m_showDefineBenchmarkDialogCommand;
            }
        }

        private bool CanShowDefineBenchmarkDialog(object param)
        {
            bool canDo = false;
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            if (appvm != null && appvm.ExperimentDocumentWrapper != null && appvm.ExperimentDocumentWrapper.Count() > 0)
            {
                var experiment = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;
                canDo = experiment != null;
            }

            return canDo;
        }

        private void ShowDefineBenchmarkDialog(object param)
        {
            ApplicationViewModelWrapper appvm = (ApplicationViewModelWrapper)DataContext;
            var experiment = appvm.ExperimentDocumentWrapper[0] as ExperimentViewModel;

            if (experiment != null)
            {
                var loggerNameRoot = new TraceLab.Core.Components.LoggerNameRoot(experiment.TopLevel.ExperimentInfo.Id);
                //validate experiment first
                bool isValid = TraceLab.Core.ExperimentExecution.ExperimentValidator.ValidateExperiment((IExperiment)experiment.TopLevel, 
                                                                            appvm.WorkspaceViewModel.WorkspaceTypeDirectories, loggerNameRoot);

                if (isValid == false)
                {
                    //show error dialog
                    string caption = TraceLab.Core.Messages.ExperimentNotValid;
                    string errorText = TraceLab.Core.Messages.InvalidExperimentErrorMessage;
                    MessageBox.Show(errorText, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    //create dialog with data contex of Define Benchmark
                    DefineBenchmarkDialog box = new DefineBenchmarkDialog();

                    //until multiple benchmark directories are not implemented
                    List<string> benchmarkDirectories = new List<string>();
                    benchmarkDirectories.Add(appvm.BenchmarkWizardViewModel.BenchmarksDirectory);

                    //create view model
                    DefiningBenchmarkViewModel definingBenchmarkViewModel = new DefiningBenchmarkViewModel((Experiment)experiment.TopLevel, 
                                                                                benchmarkDirectories,
                                                                                (TraceLab.Core.Components.ComponentsLibrary)appvm.ComponentLibraryViewModel,
                                                                                (TraceLab.Core.Workspaces.Workspace)appvm.WorkspaceViewModel,
                                                                                appvm.WorkspaceViewModel.WorkspaceTypeDirectories,
                                                                                appvm.SettingsViewModel.WebserviceAddress);

                    box.DataContext = definingBenchmarkViewModel;
                    box.Owner = this;
                    box.ShowDialog();
                }
            }
        }

        #endregion
    }
}

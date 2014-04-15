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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;
using TraceLab.Core.PackageSystem;
using TraceLab.UI.WPF.Commands;
using TraceLab.UI.WPF.Views;
using TraceLab.UI.WPF.Views.PackageBuilder;
using TraceLabSDK;
using System.IO;

namespace TraceLab.UI.WPF.ViewModels
{
    sealed class ExperimentViewModel : INotifyPropertyChanged
    {
        TopLevelExperimentViewModel m_topLevel;
        BaseLevelExperimentViewModel m_currentView;
        Dictionary<string, SubLevelExperimentViewModel> m_subLevels = new Dictionary<string, SubLevelExperimentViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentViewModel"/> class.
        /// </summary>
        /// <param name="experiment">The experiment that this ViewModel represents.  This is intentionally NOT the interface</param>
        public ExperimentViewModel(BaseExperiment experiment)
        {
            if (experiment == null)
                throw new ArgumentNullException("experiment");
        
            //determine if experiment is editable
            IEditableExperiment editableExperiment = experiment as IEditableExperiment;
            if (editableExperiment != null)
            {
                m_topLevel = new TopLevelEditableExperimentViewModel(editableExperiment);
            }
            else
            {
                m_topLevel = new TopLevelExperimentViewModel(experiment);
            }

            m_currentView = m_topLevel;
            
            OpenComponentGraph = new DelegateCommand(OpenComponentGraphFunc, CanOpenComponentGraph);
            RunExperiment = new DelegateCommand(RunExperimentFunc, CanRunExperimentFunc);
            StopExperiment = new DelegateCommand(StopExperimentFunc, CanStopExperimentFunc);
            AboutExperimentCommand = new DelegateCommand(DoAboutExperiment);
            PackExperiment = new DelegateCommand(PackExperimentFunc);

            //top level does not have parent id
            AddSubLevels(String.Empty, m_topLevel);

            experiment.NodeAdded += OnNodeAdded;
            experiment.NodeRemoved += OnNodeRemoved;

            experiment.NodeFinished += OnNodeFinished;
            experiment.NodeHasError += OnNodeHasError;

            experiment.ExperimentStarted += OnExperimentStarted;
            experiment.ExperimentCompleted += OnExperimentCompleted;
        }
        
        #region Events and handlers

        public event EventHandler<ExperimentEventArgs> ExperimentStarted;
        public event EventHandler<ExperimentEventArgs> ExperimentCompleted;
        
        void OnExperimentStarted(object sender, ExperimentEventArgs e)
        {
            if (ExperimentStarted != null)
                ExperimentStarted(this, e);
        }

        void OnExperimentCompleted(object sender, ExperimentEventArgs e)
        {
            if (ExperimentCompleted != null)
                ExperimentCompleted(this, e);
        }
                
        void OnNodeFinished(object sender, ExperimentNodeEventArgs e)
        {
        }

        void OnNodeHasError(object sender, ExperimentNodeEventArgs e)
        {
        }

        void OnNodeRemoved(ExperimentNode vertex)
        {
            var node = vertex as CompositeComponentNode;
            if (node != null)
            {
                if (m_subLevels.ContainsKey(node.ID))
                {
                    RemoveSubLevels(node.CompositeComponentMetadata.ComponentGraph);
                    m_subLevels.Remove(node.ID);
                }
            }
        }
        
        /// <summary>
        /// Called when node added to the experiment, or editable composite graph of a scope
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        void OnNodeAdded(ExperimentNode vertex)
        {
            var node = vertex as CompositeComponentNode;
            if (node != null)
            {
                string subLevelId = node.CompositeComponentMetadata.ComponentGraph.GraphIdPath;
                BaseLevelExperimentViewModel parentLevelViewModel;
                
                //component might be added to the editable scope composite graph. 
                if (node.Owner is CompositeComponentEditableGraph)
                {
                    //thus find parent view
                    string parentLevelId = subLevelId.Remove(subLevelId.LastIndexOf(":"));
                    parentLevelViewModel = FindSubLevel(parentLevelId);
                }
                else
                {
                    //otherwise use top level view
                    parentLevelViewModel = m_topLevel;
                }

                //create sub level view model for the given graph with given sub level id
                CreateSubLevelViewModel(node.CompositeComponentMetadata.ComponentGraph, subLevelId, parentLevelViewModel);
            }
        }

        /// <summary>
        /// Creates sub level view for the given composite component graph.
        /// SubLevelViewModel represents the view for the breadcrumbs.
        /// 
        /// Method also collects the sub level into dictionary of sublevel id to its corresponding view model,
        /// so that when user opens subgraph it can locate coressponging view model.
        /// </summary>
        /// <param name="componentGraph">The component graph.</param>
        /// <param name="subLevelId">The sub level id.</param>
        /// <param name="parentLevelViewModel">The parent level view model - it may either top level experiment view model, or another sublevel view model.</param>
        private void CreateSubLevelViewModel(CompositeComponentGraph componentGraph, string subLevelId, BaseLevelExperimentViewModel parentLevelViewModel)
        {
            //if it is null the loading failed... and node should be marked with error already
            if (componentGraph != null)
            {
                var subLevel = new SubLevelExperimentViewModel(componentGraph, parentLevelViewModel);
                m_subLevels.Add(subLevelId, subLevel);
                AddSubLevels(subLevelId, subLevel);

                //if the graph is editable graph listen to added node event
                if (componentGraph is CompositeComponentEditableGraph)
                {
                    componentGraph.NodeAdded += OnNodeAdded;
                }
            }
        }


        #endregion

        private void RemoveSubLevels(IExperiment experiment)
        {
            if (experiment == null)
                throw new ArgumentNullException("experiment");

            foreach (ExperimentNode node in experiment.Vertices)
            {
                CompositeComponentNode compositeNode = node as CompositeComponentNode;
                if (compositeNode != null && m_subLevels.ContainsKey(compositeNode.ID))
                {
                    CompositeComponentMetadata data = compositeNode.Data.Metadata as CompositeComponentMetadata;
                    var subLevel = m_subLevels[compositeNode.ID];

                    if (subLevel != null)
                    {
                        RemoveSubLevels(subLevel);
                    }

                    m_subLevels.Remove(compositeNode.ID);
                }
            }
        }

        private void AddSubLevels(string parentNodeId, BaseLevelExperimentViewModel experiment)
        {
            if (experiment == null)
                throw new ArgumentNullException("experiment");

            foreach (ExperimentNode node in experiment.Vertices)
            {
                CompositeComponentNode compositeNode = node as CompositeComponentNode;
                if (compositeNode != null)
                {
                    CompositeComponentBaseMetadata data = compositeNode.Data.Metadata as CompositeComponentBaseMetadata;
                    
                    string subLevelId;
                    if (String.IsNullOrEmpty(parentNodeId) == false)
                    {
                        subLevelId = parentNodeId + ":" + compositeNode.ID;
                    }
                    else
                    {
                        subLevelId = compositeNode.ID;
                    }

                    CreateSubLevelViewModel(data.ComponentGraph, subLevelId, experiment);
                }
            }
        }

        public BaseLevelExperimentViewModel CurrentView
        {
            get 
            {
                return m_currentView;
            }
            set
            {
                if (m_currentView != value)
                {
                    m_currentView = value;
                    NotifyPropertyChanged("CurrentView");
                }
            }
        }
         
        public IExperiment TopLevel
        {
            get
            {
                return m_topLevel.GetExperiment();
            }
        }

        public bool IsModified
        {
            get
            {
                return m_topLevel.IsModified;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        internal BaseLevelExperimentViewModel FindSubLevel(IExperiment experiment)
        {
            string id = string.Empty;

            return FindSubLevel(id);
        }

        internal BaseLevelExperimentViewModel FindSubLevel(string id)
        {
            SubLevelExperimentViewModel vm = null;
            m_subLevels.TryGetValue(id, out vm);
            return vm;
        }
            
        #region Open Component Graph

        private ICommand m_openComponentGraph;
        public ICommand OpenComponentGraph
        {
            get
            {
                return m_openComponentGraph;
            }
            private set
            {
                m_openComponentGraph = value;
            }
        }

        public void OpenComponentGraphFunc(object param)
        {
            CompositeComponentGraph componentGraph = param as CompositeComponentGraph;
            SubLevelExperimentViewModel componentGraphVM = param as SubLevelExperimentViewModel;
            CompositeComponentNode node = param as CompositeComponentNode;
            TopLevelExperimentViewModel topLevel = param as TopLevelExperimentViewModel;
            if (node != null)
            {
                componentGraph = node.CompositeComponentMetadata.ComponentGraph;
            }
            else if (componentGraphVM != null)
            {
                componentGraph = (CompositeComponentGraph)componentGraphVM.GetExperiment();
            }

            BaseLevelExperimentViewModel view = null;
            if (componentGraph != null)
            {
                view = FindSubLevel(componentGraph.GraphIdPath);
            }
            else if (topLevel != null)
            {
                view = topLevel;
            }

            CurrentView = view;
        }

        private bool CanOpenComponentGraph(object param)
        {
            bool canOpen = true;

            CompositeComponentNode node = param as CompositeComponentNode;
            if (node != null)
            {
                //in case component graph has not been loaded because of error, then don't allow to open sub level component graph
                if (node.CompositeComponentMetadata.ComponentGraph == null)
                {
                    canOpen = false;
                }
            }

            return canOpen;
        }

        #endregion
        
        #region RunExperiment

        public ICommand RunExperiment
        {
            get;
            private set;
        }

        private bool CanRunExperimentFunc(object param)
        {
            bool canRun = false;

            var args = param as List<object>;
            if (args != null && args.Count == 3)
            {
                //validate args
                var progress = args[0] as IProgress;
                var workspace = args[1] as WorkspaceViewModelWrapper;
                var componentsLibrary = args[2] as ComponentsLibraryViewModelWrapper;

                if (progress != null && workspace != null && componentsLibrary != null)
                {
                    canRun = TopLevel.IsExperimentRunning == false && TopLevel.Vertices.Count() >= 2;
                }
            }
            return canRun;
        }

        private void RunExperimentFunc(object param)
        {
            var args = param as List<object>;
            if (args != null && args.Count == 3)
            {
                //validate args
                var progress = args[0] as IProgress;
                var workspaceVM = args[1] as WorkspaceViewModelWrapper;
                var componentsLibrary = args[2] as ComponentsLibraryViewModelWrapper;

                if (progress != null && workspaceVM != null && componentsLibrary != null)
                {
                    var workspace = (TraceLab.Core.Workspaces.Workspace)workspaceVM;

                    //var typeDirectories = new HashSet<string>();
                    //var packageManager = TraceLab.Core.PackageSystem.PackageManager.Instance;
                    //foreach (TraceLabSDK.PackageSystem.IPackageReference packageReference in TopLevel.References)
                    //{
                    //    var package = packageManager.GetPackage(packageReference);
                    //    foreach (string typesLocation in packageManager.GetDependantTypeLocations(package))
                    //    {
                    //        typeDirectories.Add(typesLocation);
                    //    }
                    //}

                    //workspace.TypeDirectories.AddRange(typeDirectories);
                    try
                    {
                        TopLevel.RunExperiment(progress, workspace, (ComponentsLibrary)componentsLibrary);
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        progress.SetError(true);
                        progress.CurrentStatus = "Experiment failed to run";
                        NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Experiment failed to run. Some component dependencies could not be resolved. Please re-scan the components library to determine which components are failing.\n{0}", ex.Message));
                    }
                }
            }
        }

        #endregion

        #region StopExperiment

        public ICommand StopExperiment
        {
            get;
            private set;
        }

        private bool CanStopExperimentFunc(object param)
        {
            return (TopLevel.IsExperimentRunning == true);
        }

        private void StopExperimentFunc(object param)
        {
            TopLevel.StopRunningExperiment();
        }

        #endregion

        #region About Experiment Command

        /// <summary>
        /// Gets the about experiment command.
        /// </summary>
        public ICommand AboutExperimentCommand
        {
            get;
            private set;
        }

        private void DoAboutExperiment(object param)
        {
            Window parentWindow = param as Window;
            AboutExperimentDialog box = new AboutExperimentDialog(parentWindow);
            ExperimentInfo info = new ExperimentInfo(this.TopLevel.ExperimentInfo);
            box.DataContext = info;

            bool? result = box.ShowDialog();
            if (result == true)
            {
                this.TopLevel.ExperimentInfo.Name = info.Name;
                this.TopLevel.ExperimentInfo.Author = info.Author;
                this.TopLevel.ExperimentInfo.Contributors = info.Contributors;
                this.TopLevel.ExperimentInfo.Description = info.Description;
                this.TopLevel.IsModified = this.TopLevel.ExperimentInfo.IsModified;
            }
        }

        #endregion

        #region Pack Experiment

        /// <summary>
        /// Gets the pack experiment command.
        /// </summary>
        public ICommand PackExperiment
        {
            get;
            private set;
        }

        /// <summary>
        /// Extracts all assemblies containing the components and types used in the current experiment and passes
        /// them to the Package Builder.
        /// </summary>
        /// <param name="param">The Application View Model Wrapper.</param>
        private void PackExperimentFunc(object param)
        {
            bool isExperimentSaved = !this.CurrentView.IsModified;
            if (isExperimentSaved == false)
            {
                MessageBoxResult result = MessageBox.Show("To create a package, the experiment needs to be saved first.\nWould you like to proceed?", "Save modified experiment", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        isExperimentSaved = ExperimentManager.Save(this.CurrentView.GetExperiment(), this.CurrentView.ExperimentInfo.FilePath);
                    }
                    catch (Exception e)
                    {
                        isExperimentSaved = false;
                        MessageBox.Show("Package building process interrupted. The following error ocurred:\n" + e.Message, "Error while saving experiment", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            if (isExperimentSaved && param is ApplicationViewModelWrapper)
            {
                ApplicationViewModelWrapper AppVMWrapper = (ApplicationViewModelWrapper)param;

                Experiment originalExperiment = (Experiment)this.TopLevel;
                
                TraceLab.UI.WPF.Views.PackageBuilder.PackageBuilderMainWindow pkgBuilderWindow =
                    new TraceLab.UI.WPF.Views.PackageBuilder.PackageBuilderMainWindow(originalExperiment, AppVMWrapper.WorkspaceViewModel.SupportedTypes);
                pkgBuilderWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                pkgBuilderWindow.ShowDialog();
            }
        }

        #endregion
    }
}

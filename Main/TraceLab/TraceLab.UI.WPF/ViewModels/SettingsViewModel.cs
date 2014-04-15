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
using System.Text;
using TraceLab.Core.Settings;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TraceLab.UI.WPF.Commands;
using System.ComponentModel;

namespace TraceLab.UI.WPF.ViewModels
{
    internal class SettingsViewModel : INotifyPropertyChanged
    {
        ComponentsLibraryViewModelWrapper m_componentsLibrary;

        public SettingsViewModel(Settings settings, ComponentsLibraryViewModelWrapper componentLibrary)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (componentLibrary == null)
                throw new ArgumentNullException("componentLibrary");

            m_componentsLibrary = componentLibrary;

            m_settings = settings;
            m_settings.PropertyChanged += new PropertyChangedEventHandler(m_settings_PropertyChanged);
            m_settings.TypePaths.CollectionChanged += TypePaths_CollectionChanged;
            m_settings.ComponentPaths.CollectionChanged += ComponentPaths_CollectionChanged;
            m_settings.PackagePaths.CollectionChanged += PackagePaths_CollectionChanged;

            BuildPathViewModels(m_settings.ComponentPaths, m_componentPaths, SettingsPathType.Components);
            BuildPathViewModels(m_settings.TypePaths, m_typePaths, SettingsPathType.Type);
            BuildPathViewModels(m_settings.PackagePaths, m_packagePaths, SettingsPathType.Package);

            SetGlobalLogLevelSettingCommand = new DelegateCommand(SetGlobalLogLevelSettingFunc, CanSetGlobalLogLevelSettingFunc);
        }

        private void BuildPathViewModels(ObservableCollection<SettingsPath> source, ObservableCollection<SettingsPathViewModel> destination, SettingsPathType type)
        {
            destination.Clear();
            for (int i = 0; i < source.Count; ++i)
            {
                var vm = new SettingsPathViewModel(source[i], type);
                vm.IsAdded = true;
                destination.Add(vm);
            }
        }

        void Collection_Changed(ObservableCollection<SettingsPath> sender, ObservableCollection<SettingsPathViewModel> destination, SettingsPathType type, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        var vm = new SettingsPathViewModel(e.NewItems[0] as SettingsPath, type);
                        vm.IsAdded = true;
                        destination.Add(vm);
                    };
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    {
                        destination.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        destination.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    {
                        var vm = new SettingsPathViewModel(e.NewItems[0] as SettingsPath, type);
                        vm.IsAdded = true;
                        destination[e.OldStartingIndex] = vm;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    {
                        BuildPathViewModels(sender, destination, type);
                    }
                    break;
                default:
                    break;
            }
        }

        void ComponentPaths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ComponentPathsModified = true;
            ObservableCollection<SettingsPath> castSender = (ObservableCollection<SettingsPath>)sender;
            Collection_Changed(castSender, ComponentPaths, SettingsPathType.Components, e);
        }

        void TypePaths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TypePathsModified = true;
            ObservableCollection<SettingsPath> castSender = (ObservableCollection<SettingsPath>)sender;
            Collection_Changed(castSender, TypePaths, SettingsPathType.Type, e);
        }

        void PackagePaths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PackagePathsModified = true;
            ObservableCollection<SettingsPath> castSender = (ObservableCollection<SettingsPath>)sender;
            Collection_Changed(castSender, PackagePaths, SettingsPathType.Package, e);
        }

        void m_settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private Settings m_settings;

        public IEnumerable<GlobalLogLevelSetting> GlobalLogLevelsSettings
        {
            get
            {
                return m_settings.ExperimentSettings != null ? m_settings.ExperimentSettings.GlobalLogLevelsSettings : null;
            }
        }

        #region Set Global Log Level Command

        public ICommand SetGlobalLogLevelSettingCommand
        {
            get;
            private set;
        }

        private void SetGlobalLogLevelSettingFunc(object param)
        {
            GlobalLogLevelSetting logLevelSetting = param as GlobalLogLevelSetting;
            if (logLevelSetting != null)
            {
                logLevelSetting.IsEnabled = !logLevelSetting.IsEnabled;
                m_settings.ExperimentSettings.SetGlobalLogLevelSetting(logLevelSetting);
            }
        }

        private bool CanSetGlobalLogLevelSettingFunc(object param)
        {
            bool canSet = false;

            GlobalLogLevelSetting logLevelSetting = param as GlobalLogLevelSetting;
            if (logLevelSetting != null)
            {
                canSet = (logLevelSetting.IsLocked) ? false : true;
            }
            
            return canSet;
        }

        private void SetGlobalLogLevelSetting(object sender, ExecutedRoutedEventArgs e)
        {
            if (SetGlobalLogLevelSettingCommand != null)
            {
                SetGlobalLogLevelSettingCommand.Execute(e.Parameter);
            }
        }

        private void CanSetGlobalLogLevelSetting(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SetGlobalLogLevelSettingCommand != null)
            {
                e.CanExecute = SetGlobalLogLevelSettingCommand.CanExecute(e.Parameter);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        public MainWindowData MainWindowRect
        {
            get
            {
                return m_settings.MainWindowRect;
            }
        }

        public string DefaultExperiment
        {
            get { return m_settings.DefaultExperiment; }
            set
            {
                m_settings.DefaultExperiment = value;
            }
        }

        public string WebserviceAddress
        {
            get { return m_settings.WebserviceAddress; }
            set
            {
                m_settings.WebserviceAddress = value;
            }
        }


        public string DefaultExperimentsDirectory
        {
            get { return m_settings.DefaultExperimentsDirectory; }
            set
            {
                m_settings.DefaultExperimentsDirectory = value;
            }
        }

        private ObservableCollection<SettingsPathViewModel> m_packagePaths = new ObservableCollection<SettingsPathViewModel>();
        public ObservableCollection<SettingsPathViewModel> PackagePaths
        {
            get { return m_packagePaths; }
        }

        private ObservableCollection<SettingsPathViewModel> m_componentPaths = new ObservableCollection<SettingsPathViewModel>();
        public ObservableCollection<SettingsPathViewModel> ComponentPaths
        {
            get { return m_componentPaths; }
        }

        private ObservableCollection<SettingsPathViewModel> m_typePaths = new ObservableCollection<SettingsPathViewModel>();
        public ObservableCollection<SettingsPathViewModel> TypePaths
        {
            get { return m_typePaths; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        internal SettingsPathViewModel BeginAddPath(SettingsPathType type)
        {
            var vm = new SettingsPathViewModel(new SettingsPath(), type);
            vm.IsAdded = false;
            vm.IsEditing = true;
            if (type == SettingsPathType.Components)
                ComponentPaths.Add(vm);
            else
                TypePaths.Add(vm);

            return vm;
        }

        internal void FinalizeAddPath(SettingsPathViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.Path))
            {
                if (vm.PathType == SettingsPathType.Components)
                {
                    ComponentPaths.Remove(vm);
                    m_settings.ComponentPaths.Add(new SettingsPath(false, vm.Path));
                }
                else if (vm.PathType == SettingsPathType.Type)
                {
                    TypePaths.Remove(vm);
                    m_settings.TypePaths.Add(new SettingsPath(false, vm.Path));
                }
                else if (vm.PathType == SettingsPathType.Package)
                {
                    PackagePaths.Remove(vm);
                    m_settings.PackagePaths.Add(new SettingsPath(false, vm.Path));
                }
            }
        }

        internal void MoveUp(SettingsPathViewModel pathVM)
        {
            ObservableCollection<SettingsPath> paths = GetPaths(pathVM);

            var index = paths.IndexOf(pathVM.SettingsPath);
            paths.Move(index, index - 1);
        }

        internal void MoveDown(SettingsPathViewModel pathVM)
        {
            ObservableCollection<SettingsPath> paths = GetPaths(pathVM);
            var index = paths.IndexOf(pathVM.SettingsPath);
            paths.Move(index, index + 1);
        }


        internal void Delete(SettingsPathViewModel pathVM)
        {
            ObservableCollection<SettingsPath> paths = GetPaths(pathVM);

            paths.Remove(pathVM.SettingsPath);
        }

        private ObservableCollection<SettingsPath> GetPaths(SettingsPathViewModel pathVM)
        {
            ObservableCollection<SettingsPath> paths = null;
            if (pathVM.PathType == SettingsPathType.Components)
            {
                paths = m_settings.ComponentPaths;
            }
            else if (pathVM.PathType == SettingsPathType.Type)
            {
                paths = m_settings.TypePaths;
            }
            else if (pathVM.PathType == SettingsPathType.Package)
            {
                paths = m_settings.PackagePaths;
            }
            else
            {
                throw new InvalidOperationException("Unknown enum value");
            }
            return paths;
        }

        internal void ApplyChanges()
        {
            bool isModified = false;
            isModified |= HasModifiedPath(m_settings.TypePaths);

            if (isModified || TypePathsModified)
            {
                // Warn the user that type changes require a restart of TraceLab.
                System.Windows.MessageBox.Show("Changes to types require a restart of TraceLab.", "TraceLab restart required", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                isModified |= HasModifiedPath(m_settings.ComponentPaths);
                if (isModified || ComponentPathsModified)
                {
                    m_componentsLibrary.Rescan.Execute(null);
                    ComponentPathsModified = false;

                    foreach (SettingsPath path in m_settings.ComponentPaths)
                    {
                        path.ResetModifiedFlag();
                    }
                }

                isModified |= HasModifiedPath(m_settings.PackagePaths);
            }

            Settings.SaveSettings(m_settings);
        }

        private bool HasModifiedPath(ObservableCollection<SettingsPath> paths)
        {
            bool isModified = false;
            foreach (SettingsPath path in paths)
            {
                if (path.IsModified)
                {
                    isModified = true;
                    break;
                }
            }

            return isModified;
        }

        public bool TypePathsModified { get; set; }
        public bool ComponentPathsModified { get; set; }
        public bool PackagePathsModified { get; set; }
    }

    public enum SettingsPathType
    {
        Components,
        Type,
        Package
    }

    public class SettingsPathViewModel : INotifyPropertyChanged
    {
        private SettingsPath m_path;
        public SettingsPathViewModel(SettingsPath path, SettingsPathType type)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            m_path = path;
            m_path.PropertyChanged += new PropertyChangedEventHandler(m_path_PropertyChanged);

            m_type = type;
        }

        void m_path_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        public bool IsTemporary
        {
            get { return m_path.IsTemporary; }
        }

        public SettingsPath SettingsPath
        {
            get { return m_path; }
        }

        public string Path
        {
            get { return m_path.Path; }
            set
            {
                if (m_path.Path != value)
                {
                    m_path.Path = value;
                }
            }
        }

        private bool m_isEditing;
        public bool IsEditing
        {
            get { return m_isEditing; }
            set
            {
                if (m_isEditing != value)
                {
                    m_isEditing = value;
                    NotifyPropertyChanged("IsEditing");
                }
            }
        }

        private SettingsPathType m_type;
        public SettingsPathType PathType
        {
            get { return m_type; }
        }

        private bool m_isAdded;
        public bool IsAdded
        {
            get { return m_isAdded; }
            set
            {
                if (m_isAdded != value)
                {
                    m_isAdded = value;
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        public static implicit operator string(SettingsPathViewModel path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return path.Path;
        }
    }
}

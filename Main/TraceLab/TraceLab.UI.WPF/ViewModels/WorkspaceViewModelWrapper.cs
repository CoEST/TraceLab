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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using TraceLab.Core.Workspaces;
using System.Collections.Generic;

namespace TraceLab.UI.WPF.ViewModels
{
    public class WorkspaceViewModelWrapper : IDisposable
    {
        private readonly ReadOnlyObservableCollection<WpfWorkspaceUnitWrapper> m_readOnlyUnits;

        private readonly ObservableCollection<WpfWorkspaceUnitWrapper> m_units =
            new ObservableCollection<WpfWorkspaceUnitWrapper>();

        /// <summary>
        /// Wrapped workspace view model
        /// </summary>
        private readonly TraceLab.Core.ViewModels.WorkspaceViewModel m_workspaceViewModel;

        /// <summary>
        /// A WPF-aware viewmodel to wrap the main system WorkspaceViewModel.  
        /// 
        /// Note: This must be constructed on the main UI thread.
        /// </summary>
        /// <param name="workspaceViewModel">The WorkspaceViewModel to wrap.</param>
        public WorkspaceViewModelWrapper(TraceLab.Core.ViewModels.WorkspaceViewModel workspaceViewModel)
        {
            if (workspaceViewModel == null)
                throw new ArgumentNullException("workspaceViewModel");

            m_workspaceViewModel = workspaceViewModel;
            m_workspaceViewModel.PropertyChanged += new PropertyChangedEventHandler(m_workspaceViewModel_PropertyChanged);

            Dispatch = Dispatcher.CurrentDispatcher;

            // Pre-fill our units with what's in the workspace view model prior to exposing it
            // this is a mild performance tweak.
            foreach (WorkspaceUnit unit in m_workspaceViewModel.WorkspaceUnitCollection)
            {
                AddUnit(unit);
            }

            m_readOnlyUnits = new ReadOnlyObservableCollection<WpfWorkspaceUnitWrapper>(m_units);

            INotifyCollectionChanged workspaceCollection = m_workspaceViewModel.WorkspaceUnitCollection;
            workspaceCollection.CollectionChanged += WorkspaceCollectionCollectionChanged;
        }

        void m_workspaceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public ReadOnlyObservableCollection<WpfWorkspaceUnitWrapper> WorkspaceUnitCollection
        {
            get { return m_readOnlyUnits; }
        }

        #region Units Collection Changed Handling

        private Dispatcher Dispatch { get; set; }

        private void WorkspaceCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddUnit((WorkspaceUnit) e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveUnit((WorkspaceUnit) e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveUnit((WorkspaceUnit) e.OldItems[0]);
                    AddUnit((WorkspaceUnit) e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Move:
                    RemoveUnit((WorkspaceUnit) e.OldItems[0]);
                    InsertUnit((WorkspaceUnit) e.NewItems[0], e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ClearUnits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddUnit(WorkspaceUnit unit)
        {
            Dispatch.Invoke(new Action<WorkspaceUnit>(DoAddUnit), DispatcherPriority.Send, unit);
        }

        void DoAddUnit(object param)
        {
            var unit = (WorkspaceUnit)param;
            m_units.Add(new WpfWorkspaceUnitWrapper(Dispatch, unit));
        }

        private void InsertUnit(WorkspaceUnit unit, int index)
        {
            Dispatch.Invoke(new Action(() => m_units.Insert(index, new WpfWorkspaceUnitWrapper(Dispatch, unit))),
                            DispatcherPriority.Send);
        }

        private void RemoveUnit(WorkspaceUnit unit)
        {
            Dispatch.Invoke(new Action(() => m_units.Remove(new WpfWorkspaceUnitWrapper(Dispatch, unit))),
                            DispatcherPriority.Send);
        }

        private void ClearUnits()
        {
            Dispatch.Invoke(new Action(() => m_units.Clear()),
                            DispatcherPriority.Send);
        }

        #endregion

        /// <summary>
        /// Deletes the experiment units from the workspace.
        /// </summary>
        public void DeleteExperimentUnits()
        {
            m_workspaceViewModel.DeleteExperimentUnits();
        }

        public List<string> WorkspaceTypeDirectories
        {
            get
            {
                return m_workspaceViewModel.WorkspaceTypeDirectories;
            }
        }

        public IEnumerable<string> TypeDefinitionErrors
        {
            get { return m_workspaceViewModel.TypeDefinitionErrors; }
        }

        public static explicit operator Workspace(WorkspaceViewModelWrapper wrapper)
        {
            if (wrapper == null)
                throw new ArgumentNullException("wrapper");

            return (Workspace)wrapper.m_workspaceViewModel;
        }

        internal void ClearLoadErrors()
        {
            m_workspaceViewModel.ClearLoadErrors();
        }

        public Dictionary<string, Type> SupportedTypes
        {
            get { return m_workspaceViewModel.SupportedTypes; }
        }


        #region IDisposable Pattern

        ~WorkspaceViewModelWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // detach event handlers
                m_workspaceViewModel.PropertyChanged -= new PropertyChangedEventHandler(m_workspaceViewModel_PropertyChanged);
                INotifyCollectionChanged workspaceCollection = m_workspaceViewModel.WorkspaceUnitCollection;
                workspaceCollection.CollectionChanged -= WorkspaceCollectionCollectionChanged;
            }
        }

        #endregion
    }
}
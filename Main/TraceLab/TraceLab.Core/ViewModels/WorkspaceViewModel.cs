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

using System.Collections.ObjectModel;
using TraceLab.Core.Workspaces;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TraceLab.Core.ViewModels
{
    public class WorkspaceViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Workspace m_workspace;

        public WorkspaceViewModel(TraceLab.Core.Workspaces.Workspace workspace, string experimentId)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace");
            if (String.IsNullOrEmpty(experimentId))
                throw new ArgumentException("Experiment id cannot be null or empty", "experimentId");

            m_workspace = workspace;
            m_workspace.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(workspace_PropertyChanged);

            m_experimentId = experimentId;

            //init observable collections
            m_experimentWorkspaceUnits = new ObservableCollection<WorkspaceUnit>();
            m_readonlyExperimentWorkspaceUnits = new ReadOnlyObservableCollection<WorkspaceUnit>(m_experimentWorkspaceUnits);

            //prefill local units with units of this experiment
            foreach (WorkspaceUnit unit in m_workspace.Units)
            {
                AddUnit(unit);
            }

            INotifyCollectionChanged workspaceUnitCollection = m_workspace.Units;
            workspaceUnitCollection.CollectionChanged += WorkspaceCollectionCollectionChanged;

            (new TraceLab.Core.Components.LibraryHelper(m_workspace.TypeDirectories)).PreloadWorkspaceTypes(System.AppDomain.CurrentDomain);
        }

        public static explicit operator Workspace(WorkspaceViewModel workspaceViewModel)
        {
            if (workspaceViewModel == null)
                throw new ArgumentNullException("workspaceViewModel");

            return workspaceViewModel.m_workspace;
        }

        void workspace_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TypeDefinitionErrors")
            {
                NotifyPropertyChanged("TypeDefinitionErrors");
            }
        }

        private readonly string m_experimentId;

        /// <summary>
        /// Deletes the experiment units from the workspace.
        /// </summary>
        public void DeleteExperimentUnits()
        {
            m_workspace.DeleteUnits(m_experimentId);
        }

        private readonly ObservableCollection<WorkspaceUnit> m_experimentWorkspaceUnits;
        private readonly ReadOnlyObservableCollection<WorkspaceUnit> m_readonlyExperimentWorkspaceUnits;
        public ReadOnlyObservableCollection<WorkspaceUnit> WorkspaceUnitCollection
        {
            get
            {
                return m_readonlyExperimentWorkspaceUnits;
            }
        }

        #region Units Collection Changed Handling

        private void WorkspaceCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddUnit((WorkspaceUnit)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveUnit((WorkspaceUnit)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveUnit((WorkspaceUnit)e.OldItems[0]);
                    AddUnit((WorkspaceUnit)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Move:
                    RemoveUnit((WorkspaceUnit)e.OldItems[0]);
                    InsertUnit((WorkspaceUnit)e.NewItems[0], e.NewStartingIndex);
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
            if (unit.RealUnitName.StartsWith(m_experimentId))
            {
                m_experimentWorkspaceUnits.Add(unit);
            }
        }

        private void InsertUnit(WorkspaceUnit unit, int index)
        {
            if (unit.RealUnitName.StartsWith(m_experimentId))
            {
                m_experimentWorkspaceUnits.Insert(index, unit);
            }
        }

        private void RemoveUnit(WorkspaceUnit unit)
        {
            if (unit.RealUnitName.StartsWith(m_experimentId))
            {
                m_experimentWorkspaceUnits.Remove(unit);
            }
        }

        private void ClearUnits()
        {
            m_experimentWorkspaceUnits.Clear();
        }

        #endregion

        public List<string> WorkspaceTypeDirectories
        {
            get
            {
                return m_workspace.TypeDirectories;
            }
        }

        public IEnumerable<string> TypeDefinitionErrors
        {
            get { return m_workspace.TypeDefinitionErrors; }
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }

        #endregion

        public void ClearLoadErrors()
        {
            m_workspace.ClearLoadErrors();
        }

        public Dictionary<string, Type> SupportedTypes
        {
            get { return m_workspace.SupportedTypes; }
        }

        #region IDisposable Pattern
        
        ~WorkspaceViewModel()
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
                // dispose managed resources here
                m_workspace.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(workspace_PropertyChanged);

                //clear this workspaceVM units               
                INotifyCollectionChanged workspaceUnitCollection = m_workspace.Units;
                workspaceUnitCollection.CollectionChanged -= WorkspaceCollectionCollectionChanged;
            }

            // dispose any unmanaged resources here
        }

        #endregion
    }
}

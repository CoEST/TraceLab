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
using System.Windows.Input;
using System.Windows.Threading;
using TraceLab.Core.ViewModels;
using TraceLab.UI.WPF.Commands;

namespace TraceLab.UI.WPF.ViewModels
{
    class LogViewModelWrapper : INotifyPropertyChanged
    {
        private readonly ReadOnlyObservableCollection<LogInfo> m_readOnlyUnits;
        private readonly ObservableCollection<LogInfo> m_units = new ObservableCollection<LogInfo>();

        LogViewModel m_model;
        private Dispatcher Dispatch 
        { 
            get; 
            set; 
        }

        public ICommand ClearCommand
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollection<LogInfo> Events
        {
            get { return m_readOnlyUnits; }
        }

        public LogViewModelWrapper(LogViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            Dispatch = Dispatcher.CurrentDispatcher;
            ClearCommand = new DelegateCommand(ClearFunc, CanClearFunc);
            m_readOnlyUnits = new ReadOnlyObservableCollection<LogInfo>(m_units);
            m_model = model;

            // Pre-fill our units with what's in the workspace view model prior to exposing it
            // this is a mild performance tweak.
            foreach (LogInfo unit in model.Events)
            {
                AddUnit(unit);
            }

            INotifyCollectionChanged logEventsCollection = m_model.Events;
            logEventsCollection.CollectionChanged += EventsCollectionChanged;
        }


        private void EventsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddUnit((LogInfo)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveUnit((LogInfo)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveUnit((LogInfo)e.OldItems[0]);
                    AddUnit((LogInfo)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Move:
                    RemoveUnit((LogInfo)e.OldItems[0]);
                    InsertUnit((LogInfo)e.NewItems[0], e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ClearUnits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddUnit(LogInfo unit)
        {
            Dispatch.Invoke(new Action(() => m_units.Add(unit)),
                            DispatcherPriority.Send);
        }

        private void InsertUnit(LogInfo unit, int index)
        {
            Dispatch.Invoke(new Action(() => m_units.Insert(index, unit)),
                            DispatcherPriority.Send);
        }

        private void RemoveUnit(LogInfo unit)
        {
            Dispatch.Invoke(new Action(() => m_units.Remove(unit)),
                            DispatcherPriority.Send);
        }

        private void ClearUnits()
        {
            Dispatch.Invoke(new Action(() => m_units.Clear()),
                            DispatcherPriority.Send);
        }

        private void ClearFunc(object param)
        {
            if (m_model != null)
            {
                m_model.Clear();
            }
        }

        private bool CanClearFunc(object param)
        {
            if (m_model != null)
            {
                return m_model.Events.Count > 0;
            }

            return false;
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}

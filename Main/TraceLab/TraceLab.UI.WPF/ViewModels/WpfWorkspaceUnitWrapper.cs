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
using System.ComponentModel;
using TraceLab.Core.Workspaces;
using System.Windows.Threading;

namespace TraceLab.UI.WPF.ViewModels
{

    public class WpfWorkspaceUnitWrapper : INotifyPropertyChanged, IDisposable
    {
        private object m_data;
        private WorkspaceUnit m_unit;

        public WpfWorkspaceUnitWrapper(Dispatcher dispatch, WorkspaceUnit unit)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");
            if (dispatch == null)
                throw new ArgumentNullException("dispatch");

            m_unit = unit;
            m_unit.PropertyChanged += UnitPropertyChanged;
            Dispatch = dispatch;
        }

        private Dispatcher Dispatch { get; set; }

        public string FriendlyUnitName
        {
            get { return m_unit.FriendlyUnitName; }
        }

        public object Data
        {
            get
            {
                if (m_data == null)
                {
                    Dispatch.Invoke(new Action(() => { m_data = m_unit.Data; }), DispatcherPriority.Send);
                    // Cache updated, make sure that we're displaying the right type.
                    OnPropertyChanged("Type");
                }

                return m_data;
            }
            set { m_unit.Data = value; }
        }

        public Type Type
        {
            get { return m_unit.Type; }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region IDisposable Pattern

        ~WpfWorkspaceUnitWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_unit = null;
                m_data = null;
            }
        }

        #endregion

        private void UnitPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                m_data = null;
            }
            OnPropertyChanged(e.PropertyName);
        }

        public override bool Equals(object obj)
        {
            WorkspaceUnit otherUnit = null;

            if (obj is WpfWorkspaceUnitWrapper)
            {
                var otherWrapper = obj as WpfWorkspaceUnitWrapper;
                otherUnit = otherWrapper.m_unit;
            }
            if (obj is WorkspaceUnit)
            {
                otherUnit = obj as WorkspaceUnit;
            }

            return m_unit.Equals(otherUnit);
        }

        public override int GetHashCode()
        {
            return m_unit.GetHashCode();
        }
    }
}

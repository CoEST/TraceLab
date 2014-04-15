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
using TraceLab.Core.ViewModels;
using MonoDevelop.Components.Docking;
using Mono.Unix;
using TraceLab.Core.Workspaces;
using System.Collections.Specialized;
using Gtk;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    public class WorkspaceWindowPad : IDockPad
    {
        private bool m_initialized = false;
        private DockFrame m_dockFrame;
        private string m_experimentId; //stores the id to the experiment which current workspace window is associated with
        private WorkspaceViewModel m_workspaceViewModel;
        private Gtk.ListStore m_workspaceStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.WorkspaceWindowPad"/> class.
        /// </summary>
        public WorkspaceWindowPad()
        {
        }

        /// <summary>
        /// Initialize the window pad in the given dock frame.
        /// </summary>
        /// <param name='dockFrame'>
        /// Dock frame.
        /// </param>
        public void Initialize(DockFrame dockFrame)
        {
            m_dockFrame = dockFrame;
            DockItem workspaceDockingWindow = m_dockFrame.AddItem("Workspace");
            workspaceDockingWindow.Label = Catalog.GetString("Workspace");
            workspaceDockingWindow.DefaultLocation = "ComponentsLibrary/Center";
            workspaceDockingWindow.Behavior |= DockItemBehavior.CantClose;

            workspaceDockingWindow.Content = CreateWorkspaceView();

            m_initialized = true;
        }

        /// <summary>
        /// Sets the application model on the given pad.
        /// Pad refreshes its information according to the given application model.
        /// </summary>
        /// <param name='applicationViewModel'>
        /// Application model.
        /// </param>
        public void SetApplicationModel(ApplicationViewModel applicationViewModel) 
        {
            if(m_initialized == false || m_dockFrame.GdkWindow == null) 
            {
                //GdkWindow is for each dock frame is assigned when windowShell calls ShowAll(). See DockContainer.OnRealize method
                throw new InvalidOperationException("WorkspaceWindowPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            if(applicationViewModel.WorkspaceViewModel != null) 
            {
                applicationViewModel.PropertyChanged += HandlePropertyChanged;

                SetWorkspaceViewModel(applicationViewModel);
            }
        }

        private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplicationViewModel applicationViewModel = sender as ApplicationViewModel;
            if(applicationViewModel != null && e.PropertyName == "WorkspaceViewModel") 
            {
                SetWorkspaceViewModel(applicationViewModel);
            }
        }

        private void SetWorkspaceViewModel(ApplicationViewModel applicationViewModel) 
        {
            if(m_workspaceViewModel != null) 
            {
                //detach handlers
                ((INotifyCollectionChanged)m_workspaceViewModel.WorkspaceUnitCollection).CollectionChanged -= WorkspaceCollectionChanged;
                m_workspaceStore.Clear();
            }
            
            m_workspaceViewModel = applicationViewModel.WorkspaceViewModel;
            m_experimentId = applicationViewModel.Experiment.ExperimentInfo.Id;
            
            // Load existing tasks
            foreach(WorkspaceUnit unit in m_workspaceViewModel.WorkspaceUnitCollection) 
            {
                AddUnit(unit);
            }
            
            //attach listener to workspace collection
            ((INotifyCollectionChanged)m_workspaceViewModel.WorkspaceUnitCollection).CollectionChanged += WorkspaceCollectionChanged;
        }

        private Gtk.Widget CreateWorkspaceView() 
        {
            ScrolledWindow sw = new ScrolledWindow();
            Gtk.TreeView treeView = new Gtk.TreeView();
            
            m_workspaceStore = new Gtk.ListStore(typeof(string));
            treeView.Model = m_workspaceStore;
            
            CellRendererText textRenderer = new CellRendererText();

            //create columns with associated cell renderings
            treeView.AppendColumn("Name", textRenderer, "text", 0);
                        
            sw.Add(treeView);
            sw.ShowAll();
            return sw;
        }

        private void WorkspaceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddUnit((WorkspaceUnit)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //RemoveUnit((WorkspaceUnit)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    //RemoveUnit((WorkspaceUnit)e.OldItems[0]);
                    //AddUnit((WorkspaceUnit)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Move:
                    //RemoveUnit((WorkspaceUnit)e.OldItems[0]);
                    //AddUnit((WorkspaceUnit)e.NewItems[0]);
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
                m_workspaceStore.AppendValues(unit.FriendlyUnitName);
            }
        }

//        Dictionary<string, TreeIter> m_experimentWorkspaceUnits = new Dictionary<string, TreeIter>();       
//        private void RemoveUnit(WorkspaceUnit unit)
//        {
//            if (unit.RealUnitName.StartsWith(m_experimentId))
//            {
//                TreeIter item;
//                if(m_experimentWorkspaceUnits.TryGetValue(unit.RealUnitName, out item)) 
//                {
//                    m_workspaceStore.Remove(ref item);
//                }
//            }
//        }

        internal void ClearUnits()
        {
            m_workspaceStore.Clear();
        }
    }
}


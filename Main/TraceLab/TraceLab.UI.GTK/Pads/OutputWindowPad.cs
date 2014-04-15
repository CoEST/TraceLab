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
using Gtk;
using MonoDevelop.Components.Docking;
using Mono.Unix;
using TraceLab.Core.ViewModels;
using NLog;
using System.Collections.Specialized;

namespace TraceLab.UI.GTK
{
    public class OutputWindowPad : IDockPad
    {
        private bool m_initialized = false;
        private DockFrame m_dockFrame;
        private LogViewModel m_logViewModel;
        private Gtk.ListStore m_logStore;
        private Gdk.Pixbuf m_iconInfo;
        private Gdk.Pixbuf m_iconTrace;
        private Gdk.Pixbuf m_iconDebug;
        private Gdk.Pixbuf m_iconWarning;
        private Gdk.Pixbuf m_iconError;
        private Button m_buttonClear;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.OutputWindowPad"/> class.
        /// </summary>
        public OutputWindowPad() 
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
            DockItem outputDockingWindow = m_dockFrame.AddItem("Output");
            outputDockingWindow.Label = Catalog.GetString("Output");
            outputDockingWindow.DefaultHeight = 100;
            outputDockingWindow.DefaultLocation = "ExperimentPad/Bottom"; //or experiment 
            outputDockingWindow.Behavior |= DockItemBehavior.CantClose;

            DockItemToolbar toolbar = outputDockingWindow.GetToolbar (PositionType.Top);
                        
            m_buttonClear = new Button (new Gtk.Image ("gtk-clear", IconSize.Menu));
            m_buttonClear.Clicked += new EventHandler (OnButtonClearClick);
            toolbar.Add(m_buttonClear);

            toolbar.ShowAll();

            outputDockingWindow.Content = CreateOutputView();

            LoadIcons();

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
                throw new InvalidOperationException("OutputWindowPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            applicationViewModel.PropertyChanged += HandlePropertyChanged;

            SetLogViewModel(applicationViewModel.LogViewModel);
        }

        private void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplicationViewModel applicationViewModel = sender as ApplicationViewModel;
            if(applicationViewModel != null && e.PropertyName == "LogViewModel") 
            {
                SetLogViewModel(applicationViewModel.LogViewModel);
            }
        }

        private void SetLogViewModel(LogViewModel logViewModel) 
        {
            if(m_logViewModel != null) 
            {
                //detach handlers
                ((INotifyCollectionChanged)m_logViewModel.Events).CollectionChanged -= EventsCollectionChanged;
                m_logStore.Clear();
            }
            
            m_logViewModel = logViewModel;
            
            // Load existing tasks
            foreach(LogInfo logInfo in m_logViewModel.Events) 
            {
                AddEventUnit(logInfo);
            }
            
            //attach listener to Events collection, so that view is updated anytime there is new log message in the collection
            ((INotifyCollectionChanged)m_logViewModel.Events).CollectionChanged += EventsCollectionChanged;
        }

        /// <summary>
        /// Creates the output view.
        /// </summary>
        /// <returns>
        /// The output view.
        /// </returns>
        private Gtk.Widget CreateOutputView() 
        {
            ScrolledWindow sw = new ScrolledWindow();
            Gtk.TreeView treeView = new Gtk.TreeView();

            //init log view model with four columns (icon, severity, source, and message)
            m_logStore = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(string), typeof(string), typeof(string));
            treeView.Model = m_logStore;

            CellRendererText textRenderer = new CellRendererText();
            CellRendererPixbuf iconRenderer = new CellRendererPixbuf();
            CellRendererText wrappingTextRenderer = new CellRendererText();
            wrappingTextRenderer.WrapMode = Pango.WrapMode.Word;

            //create columns with associated cell renderings
            treeView.AppendColumn("!", iconRenderer, "pixbuf", 0);
            treeView.AppendColumn("Severity", textRenderer, "text", 1);
            treeView.AppendColumn("Source", textRenderer, "text", 2);
            treeView.AppendColumn("Message", wrappingTextRenderer, "text", 3);

            foreach(TreeViewColumn col in treeView.Columns)
                col.Alignment = 0.5f;

            sw.Add(treeView);
            sw.ShowAll();
            return sw;
        }

        /// <summary>
        /// Loads the icons from resources
        /// </summary>
        private void LoadIcons() 
        {
            m_iconInfo    = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_info_12x12.png");
            m_iconTrace   = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_trace_12x12.png");
            m_iconDebug   = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_debug_12x12.png");
            m_iconWarning = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_warning_12x12.png");
            m_iconError   = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_error_12x12.png");
        }

        /// <summary>
        /// Handles the Events collection changed event.
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// E.
        /// </param>
        private void EventsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddEventUnit((LogInfo)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // not needed (currently there is no functionality for removing single item within the Events collection)
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // not needed (currently there is no functionality for replacing items within the Events collection)
                    break;
                case NotifyCollectionChangedAction.Move:
                    // not needed (currently there is no functionality for moving items within the Events collection)
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ClearEventUnits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Adds the event unit to the log store
        /// </summary>
        /// <param name='logInfo'>
        /// Log info.
        /// </param>
        private void AddEventUnit(LogInfo logInfo) 
        {
            Gdk.Pixbuf icon;
            
            if(logInfo.Level == NLog.LogLevel.Info)
                icon = m_iconInfo;
            else if(logInfo.Level == NLog.LogLevel.Trace) 
                icon = m_iconTrace;
            else if(logInfo.Level == NLog.LogLevel.Debug)
                icon = m_iconDebug;
            else if(logInfo.Level == NLog.LogLevel.Warn)
                icon = m_iconWarning;
            else if(logInfo.Level == NLog.LogLevel.Error) 
                icon = m_iconError;
            else 
                icon = m_iconError; //in case it is LogLevel.Fatal

            m_logStore.AppendValues(icon, logInfo.Level.ToString(), logInfo.SourceName, logInfo.Message);
        }

        /// <summary>
        /// Handles the Reset event of the log events collection change.
        /// Clears the event units. 
        /// </summary>
        private void ClearEventUnits()
        {
            //clear log store view
            m_logStore.Clear();
        }

        /// <summary>
        /// Raises the button clear click event.
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// E.
        /// </param>
        private void OnButtonClearClick (object sender, EventArgs e)
        {
            //clear view model
            m_logViewModel.Clear(); 

            //in effect it will raise Reset event of collection change and clear log store
        }
    }
}




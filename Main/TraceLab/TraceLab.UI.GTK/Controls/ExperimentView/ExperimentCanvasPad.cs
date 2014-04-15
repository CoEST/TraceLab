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
//
using System;
using MonoDevelop.Components.Docking;
using Mono.Unix;
using Gtk;
using TraceLab.Core.ViewModels;
using TraceLab.Core.Experiments;
using MonoHotDraw.Figures;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TraceLab.Core.Components;

namespace TraceLab.UI.GTK
{
    public class ExperimentCanvasPad : IDockPad
    {
        private bool m_initialized = false;
        private DockFrame m_dockFrame;
        private DockItem m_experimentPad;
        private ApplicationViewModel m_applicationViewModel;
        private ExperimentCanvasWidget m_experimentCanvasWidget;
        private ApplicationContext m_applicationContext;
        private ExperimentDrawer m_experimentDrawer;

        public ExperimentCanvasPad(ApplicationContext application)
        {
            m_applicationContext = application;
        }

        public void Initialize(DockFrame dockFrame)
        {
            m_dockFrame = dockFrame;
            m_experimentPad = m_dockFrame.AddItem("ExperimentPad");
            m_experimentPad.Label = Catalog.GetString("ExperimentPad");
            m_experimentPad.Behavior = DockItemBehavior.Locked;
            m_experimentPad.Expand = true;
            m_experimentPad.DrawFrame = false;

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
                throw new InvalidOperationException("ExperimentCanvasPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            m_applicationViewModel = applicationViewModel;

            if(m_applicationViewModel.Experiment == null) 
            {
                m_experimentPad.Content = new WelcomePageWidget(m_applicationContext);
            } 
            else 
            {
                CreateExperimentControlToolbar();

                m_experimentCanvasWidget = new ExperimentCanvasWidget();
                m_experimentPad.Content = m_experimentCanvasWidget;

                bool isExperimentEditable = m_applicationViewModel.Experiment is IEditableExperiment;

                m_experimentDrawer = new ExperimentDrawer(m_experimentCanvasWidget, m_applicationContext.NodeControlFactory,
                                                          m_applicationContext.NodeConnectionControlFactory);

                m_experimentDrawer.DrawExperiment(m_applicationViewModel.Experiment, isExperimentEditable);
                m_applicationViewModel.Experiment.NodeRemoved += OnNodeRemoved;
                m_applicationViewModel.Experiment.EdgeRemoved += OnEdgeRemoved;

                if(isExperimentEditable) 
                {
                    //enable drop of components to canvas
                    EnableDrop();
                }
            }
        }

        /// <summary>
        /// When node is removed in model experiment,
        /// the node control must be also removed from the view to keep view and model synced.
        /// </summary>
        /// <param name='vertex'>
        /// Vertex.
        /// </param>
        private void OnNodeRemoved(ExperimentNode vertex)
        {
            BasicNodeControl componentControl;
            if(m_applicationContext.NodeControlFactory.TryGetNodeControl(vertex, out componentControl)) 
            {
                m_experimentCanvasWidget.ExperimentCanvas.View.RemoveFromSelection(componentControl);
                m_experimentCanvasWidget.ExperimentCanvas.View.Remove(componentControl);
                m_applicationContext.NodeControlFactory.RemoveFromLookup(vertex);
            }
        }

        /// <summary>
        /// When edge is removed in model experiment,
        /// the edge control must be also removed from the view to keep view and model synced.
        /// </summary>
        /// <param name='edge'>
        /// Edge.
        /// </param>
        private void OnEdgeRemoved(ExperimentNodeConnection nodeConnection) 
        {
            NodeConnectionControl connectionControl;
            if(m_applicationContext.NodeConnectionControlFactory.TryGetConnectionControl(nodeConnection, out connectionControl)) 
            {
                m_experimentCanvasWidget.ExperimentCanvas.View.RemoveFromSelection(connectionControl);
                m_experimentCanvasWidget.ExperimentCanvas.View.Remove(connectionControl);
                m_applicationContext.NodeConnectionControlFactory.RemoveFromLookup(nodeConnection);
            }
        }

        #region Drop

        /// <summary>
        /// Enables the drop of components to experiment canvas
        /// </summary>
        /// <param name='canvas'>
        /// Canvas.
        /// </param>
        private void EnableDrop() 
        {
            m_experimentCanvasWidget.DragDataReceived += HandleDragDataReceived;
            Gtk.Drag.DestSet(m_experimentCanvasWidget, DestDefaults.All, ComponentsLibraryWidget.Targets, Gdk.DragAction.Copy);
        }

        private void DisableDrop() 
        {
            m_experimentCanvasWidget.DragDataReceived -= HandleDragDataReceived;
            Gtk.Drag.DestUnset(m_experimentCanvasWidget);
        }

        /// <summary>
        /// Handles the drag data received.
        /// See the EnableDrag and HandleDragDataGet in the ComponentLibraryPad where the drag
        /// has started and drag source is set
        /// </summary>
        /// <param name='o'>
        /// O.
        /// </param>
        /// <param name='args'>
        /// Arguments.
        /// </param>
        private void HandleDragDataReceived (object o, DragDataReceivedArgs args)
        {
            Widget source = Drag.GetSourceWidget(args.Context);
            TreeView treeView = source as TreeView;
            TreeIter selectedItem;

            if(treeView != null && treeView.Selection.GetSelected(out selectedItem)) 
            {
                ComponentsLibraryNode selectedNode = treeView.Model.GetValue(selectedItem, 0) as ComponentsLibraryNode;

                Cairo.PointD translation = m_experimentCanvasWidget.ExperimentCanvas.View.ViewToDrawing(args.X, args.Y);
                ExperimentNode node = m_applicationViewModel.Experiment.AddComponentFromDefinition(selectedNode.Data, 
                                                                                                       translation.X, translation.Y);
                m_experimentDrawer.DrawComponent(node, true);
            }

            Drag.Finish(args.Context, true, false, args.Time);
        }

        #endregion

        private void CreateExperimentControlToolbar() 
        {
            DockItemToolbar toolbar = m_experimentPad.GetToolbar(PositionType.Top);
            Button runExperimentButton = new Button(new Gtk.Image(Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Play16.png")));
            runExperimentButton.Clicked += new EventHandler(OnRunExperimentButtonClick);
            runExperimentButton.TooltipText = "Run Experiment";
            toolbar.Add(runExperimentButton);

            Button stopExperimentButton = new Button(new Gtk.Image(Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Stop.png")));
            stopExperimentButton.Clicked += new EventHandler(OnStopExperimentButtonClick);
            stopExperimentButton.TooltipText = "Stop experiment after all currently running components have finished processing";
            toolbar.Add(stopExperimentButton);

            Button defineCompositeComponentButton = new Button(new Gtk.Image(Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.DefineCompositeComponent.png")));
            defineCompositeComponentButton.Clicked += new EventHandler(OnDefineCompositeComponentButtonClick);
            defineCompositeComponentButton.TooltipText = "Define composite component from selected nodes";
            toolbar.Add(defineCompositeComponentButton);

            Button aboutExperimentButton = new Button(new Gtk.Image(Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Info16.png")));
            aboutExperimentButton.Clicked += new EventHandler(OnAboutExperimentButtonClick);
            aboutExperimentButton.TooltipText = "About Experiment";
            toolbar.Add(aboutExperimentButton);

            toolbar.ShowAll();
        }

        private void OnRunExperimentButtonClick(object sender, EventArgs e)
        {
            TraceLabSDK.IProgress progress = m_applicationContext.MainWindow.WindowShell.StatusBar;
            m_applicationContext.MainWindow.WorkspaceWindowPad.ClearUnits();
            m_applicationViewModel.Experiment.RunExperiment(progress, 
                                                            m_applicationViewModel.Workspace, 
                                                            TraceLab.Core.Components.ComponentsLibrary.Instance);

        }

        private void OnStopExperimentButtonClick(object sender, EventArgs e)
        {
            m_applicationViewModel.Experiment.StopRunningExperiment();
        }

        private void OnDefineCompositeComponentButtonClick(object sender, EventArgs e)
        {
            var wizard = new DefineCompositeComponentWizard(m_applicationContext);
            wizard.Modal = true;
            wizard.Show();
        }

        private void OnAboutExperimentButtonClick(object sender, EventArgs e)
        {
            AboutExperimentDialog aboutExpDialog = new AboutExperimentDialog(m_applicationViewModel);           
            aboutExpDialog.Run();
        }

        #region Experiment crumbs

        internal void DisplaySubgraph(CompositeComponentMetadata metadata) 
        {
            //always clear crumbs
            m_experimentCanvasWidget.ExperimentCrumbs.RemoveAll();

            List<Crumb> crumbs = ExperimentCrumbGatherer.GatherCrumbs(metadata);

            m_experimentCanvasWidget.ExperimentCrumbs.ShowAll();

            foreach(Crumb crumb in crumbs) 
            {
                m_experimentCanvasWidget.ExperimentCrumbs.Append(crumb);
                crumb.Clicked += Redraw;
            }

            //set last one active (triggers clicked event)
            m_experimentCanvasWidget.ExperimentCrumbs.Active = crumbs[crumbs.Count - 1];

            //attach handler to the first element so that it hides and removes all crumbs
            crumbs[0].Clicked += (object sender, EventArgs e) => 
            {
                m_experimentCanvasWidget.ExperimentCrumbs.HideAll();
            };
        }

        private void Redraw(object sender, EventArgs e)
        {
            ExperimentCrumb crumb = (ExperimentCrumb)sender;
            m_experimentCanvasWidget.ExperimentCanvas.View.ClearAll();
            m_applicationContext.NodeControlFactory.ClearLookup();
            m_applicationContext.NodeConnectionControlFactory.ClearLookup();

            bool isExperimentEditable = crumb.Graph is IEditableExperiment;
            m_experimentDrawer.DrawExperiment(crumb.Graph, isExperimentEditable);

            if(isExperimentEditable == true) 
            {
                EnableDrop();
            } 
            else 
            {
                DisableDrop();
            }
        }

        #endregion
    }
}


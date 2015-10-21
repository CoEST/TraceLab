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

// HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
using MonoHotDraw;
using TraceLab.Core.Experiments;
// END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

// HERZUM SPRINT 3.1 TLAB-82
using TraceLabSDK;
// END HERZUM SPRINT 3.1 TLAB-82

namespace TraceLab.UI.GTK
{
    public class ExperimentCanvasPad : IDockPad
    {
        //STATIC MEMBERS
        private static string PUBLISH_RESULTS_TOOLTIP = "Publish Results";
        private static string PUBLISH_CHALLENGE_TOOLTIP = "Publish Challenge";
        private static string TPKG_EXT = ".tpkg";
       
        private bool m_initialized = false;
        private DockFrame m_dockFrame;
        private DockItem m_experimentPad;
        private ApplicationViewModel m_applicationViewModel;
        private ExperimentCanvasWidget m_experimentCanvasWidget;
        private ApplicationContext m_applicationContext;
        private ExperimentDrawer m_experimentDrawer;
        // HERZUM SPRINT 1.0
        private CompositeComponentEditableGraph m_subExperiment = null;
        // END HERZUM SPRINT 1.0
        public ExperimentCanvasPad (ApplicationContext application)
        {
            m_applicationContext = application;
        }

        public void Initialize(DockFrame dockframe){
            m_dockFrame = dockframe;
            m_experimentPad = m_dockFrame.AddItem ("ExperimentPad");
            m_experimentPad.Label = Catalog.GetString ("ExperimentPad");
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
        public void SetApplicationModel (ApplicationViewModel applicationViewModel)
        {

            if (m_initialized == false || m_dockFrame.GdkWindow == null) {
                //GdkWindow is for each dock frame is assigned when windowShell calls ShowAll(). See DockContainer.OnRealize method
                throw new InvalidOperationException ("ExperimentCanvasPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            m_applicationViewModel = applicationViewModel;        

            if(m_applicationViewModel.Experiment == null) 
            {
                m_experimentPad.Content = new WelcomePageWidget(m_applicationContext);
            } 
            else 
            {
                
                bool isExperimentEditable = m_applicationViewModel.Experiment is IEditableExperiment;

                //we check whether the challenge is password protected and which password has been used by the user > if challenge password than make the exp not editable
                string isChallengeString = m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge;
                if (!string.IsNullOrEmpty (isChallengeString) && isChallengeString.Equals ("True")) {
                    isExperimentEditable = !hasChallengePasswordBeenUsed();
                } 

                CreateExperimentControlToolbar();

                m_experimentCanvasWidget = new ExperimentCanvasWidget();
                m_experimentPad.Content = m_experimentCanvasWidget;

    
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

                // TLAB-184
                (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).PanTool += PanToolHandler;
                (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).PanToolButtonReleased += PanToolButtonReleasedHandler;
                /// TLAB-184
            }
        }

        private bool hasChallengePasswordBeenUsed(){
            if (!string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword) &&
                m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword.Equals(m_applicationViewModel.Experiment.unlockingPassword)){
                   return true;
            }
            return false;        
        }

        private bool hasAnyPasswordBeenUsed(){
            if (!string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword) ||
                  !string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.ExperimentPassword)){
                return true;
            }
            return false;        
        }

        /// <summary>
        /// When node is removed in model experiment,
        /// the node control must be also removed from the view to keep view and model synced.
        /// </summary>
        /// <param name='vertex'>
        /// Vertex.
        /// </param>
        private void OnNodeRemoved (ExperimentNode vertex)
        {
            BasicNodeControl componentControl;
            if (m_applicationContext.NodeControlFactory.TryGetNodeControl (vertex, out componentControl)) {
                m_experimentCanvasWidget.ExperimentCanvas.View.RemoveFromSelection (componentControl);
                m_experimentCanvasWidget.ExperimentCanvas.View.Remove (componentControl);
                m_applicationContext.NodeControlFactory.RemoveFromLookup (vertex);

                // HERZUM SPRINT 2.0: TLAB-65
                if (componentControl.GetType ().Name == "ChallengeNodeControl") {
                    ScopeNodeControl challengeNodeControl = componentControl as ChallengeNodeControl;
                    m_experimentCanvasWidget.ExperimentCanvas.View.RemoveWidget (challengeNodeControl.ScopeCanvasWidget);
                    // HERZUM SPRINT 2.5: TLAB-173
                    ExperimentCanvasPadFactory.RemoveSubExperimentCanvasPad (m_applicationContext, challengeNodeControl);
                    // END HERZUM SPRINT 2.5: TLAB-173
                }
                // END HERZUM 2.0: TLAB-65

                // HERZUM SPRINT 1.0
                if (componentControl.GetType().Name=="ScopeNodeControl")
                {
                    ScopeNodeControl scopeNodeControl = componentControl as ScopeNodeControl;
                    m_experimentCanvasWidget.ExperimentCanvas.View.RemoveWidget (scopeNodeControl.ScopeCanvasWidget);
                    // HERZUM SPRINT 2.5: TLAB-173
                    ExperimentCanvasPadFactory.RemoveSubExperimentCanvasPad (m_applicationContext, scopeNodeControl);
                    // END HERZUM SPRINT 2.5: TLAB-173
                }
                // END HERZUM 1.0  

                // HERZUM SPRINT 1.0
                if (componentControl.GetType().Name=="CommentNodeControl")
                {
                    CommentNodeControl commentNodeControl = componentControl as CommentNodeControl;
                    m_experimentCanvasWidget.ExperimentCanvas.View.RemoveWidget (commentNodeControl.CommentCanvasWidget);
                    // HERZUM SPRINT 2.5: TLAB-173
                    ExperimentCanvasPadFactory.RemoveSubExperimentCanvasPad (m_applicationContext, commentNodeControl);
                    // END HERZUM SPRINT 2.5: TLAB-173
                }
                // END HERZUM SPRINT 1.0

                // // HERZUM SPRINT 1.1 LOOP
                if (componentControl.GetType().Name=="LoopNodeControl")
                {
                    LoopNodeControl loopNodeControl = componentControl as LoopNodeControl;
                    m_experimentCanvasWidget.ExperimentCanvas.View.RemoveWidget (loopNodeControl.ScopeCanvasWidget);
                    // HERZUM SPRINT 2.5: TLAB-173
                    ExperimentCanvasPadFactory.RemoveSubExperimentCanvasPad (m_applicationContext, loopNodeControl);
                    // END HERZUM SPRINT 2.5: TLAB-173
                }
                // END HERZUM SPRINT 1.1 LOOP

                // HERZUM SPRINT 4 TLAB-210
                if (componentControl is ScopeNodeControl || componentControl is LoopNodeControl) {
                    ScopeBaseMetadata scopeMetadata = componentControl.ExperimentNode.Data.Metadata as ScopeBaseMetadata;
                    if (scopeMetadata.ComponentGraph.IncludeChallenge()) {
                        m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge = "False";
                        m_applicationContext.MainWindow.ExperimentCanvasPad.UpdateExperimentToolbarForExperiment ();
                    }
                }
                // END HERZUM SPRINT 4 TLAB-210

                //TLAb-66
                if (componentControl is ChallengeNodeControl) {
                    m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge = "False";
                    m_applicationContext.MainWindow.ExperimentCanvasPad.UpdateExperimentToolbarForExperiment ();
                }

            }

            // HERZUM SPRINT 2.2 TLAB-142
            m_applicationContext.MainWindow.HideComponentInfoPad (componentControl);
            // HERZUM SPRINT 2.2 TLAB-142
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

        // TLAB-184
        private void PanToolHandler (object sender, EventArgs e)
        {
            this.ExperimentCanvasWidget.PanTool ();
        }

        private void PanToolButtonReleasedHandler (object sender, EventArgs e)
        {
            this.ExperimentCanvasWidget.PanToolButtonReleased ();
        }
        ///  TLAB-184
         
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>

        // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
        private void CopyHandler (object sender, EventArgs e)
        {
            if (m_subExperiment != null)
                TraceLab.Core.Experiments.Clipboard.Copy (m_subExperiment);
            else
                TraceLab.Core.Experiments.Clipboard.Copy (m_applicationViewModel.Experiment);
        }

        private void CutHandler (object sender, EventArgs e)
        {
            if (m_subExperiment != null)
                TraceLab.Core.Experiments.Clipboard.Cut (m_subExperiment);
            else
                TraceLab.Core.Experiments.Clipboard.Cut (m_applicationViewModel.Experiment);
        }

        private void PasteHandler (object sender, MouseEventArgs e)
        {
            // HERZUM SPRINT 4: TLAB-204
            if (m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge != null  && m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge.Equals ("True") && TraceLab.Core.Experiments.Clipboard.IncludeChallenge ()){
                ShowMessageDialog("Cannot paste another Challenge Scope into a Challenge",
                                  "Paste", Gtk.ButtonsType.Ok, Gtk.MessageType.Warning);
                return;
            }
            // END HERZUM SPRINT 4: TLAB-204

            if (m_subExperiment != null) {
                TraceLab.Core.Experiments.Clipboard.Paste (m_subExperiment, e.MouseEvent.X, e.MouseEvent.Y);
                DisplayAddedSubgraph(m_subExperiment);
            }
            else {
                TraceLab.Core.Experiments.Clipboard.Paste (m_applicationViewModel.Experiment, e.MouseEvent.X, e.MouseEvent.Y);
                DisplayAddedSubgraph (m_applicationViewModel.Experiment);
            }

            // HERZUM SPRINT 4: TLAB-204
            if (TraceLab.Core.Experiments.Clipboard.IncludeChallenge())
            {
                m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge = "True";    
                UpdateExperimentToolbarForChallenge ();
            }
            // END HERZUM SPRINT 4: TLAB-204
        }
        // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

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

            // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
            (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).Copy += CopyHandler;
            (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).Cut += CutHandler;
            (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).Paste += PasteHandler;
            // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

        }

        private void DisableDrop() 
        {
            // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
            (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).Copy -= CopyHandler;
            (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).Cut -= CutHandler;
            (m_experimentCanvasWidget.ExperimentCanvas.View as StandardDrawingView).Paste -= PasteHandler;
            // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

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


                if (selectedNode.Data.ID.Equals (ChallengeMetadataDefinition.ChallengeScopeGuid) && isChallengeExp()){
                    Drag.Finish (args.Context, true, false, args.Time);
                    return;
                } else if(selectedNode.Data.ID.Equals (ChallengeMetadataDefinition.ChallengeScopeGuid)){
                    m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge = "True";
                    m_applicationContext.MainWindow.ExperimentCanvasPad.UpdateExperimentToolbarForChallenge ();
                }

                ExperimentNode node = null;

                if (m_subExperiment != null)
                    node = m_subExperiment.AddComponentFromDefinition (selectedNode.Data, translation.X, translation.Y);
                else
                    node = m_applicationViewModel.Experiment.AddComponentFromDefinition (selectedNode.Data, translation.X, translation.Y);

                m_experimentDrawer.DrawComponent (node, true);

                // HERZUM SPRINT 1.1 IF
                if (node is ExperimentDecisionNode) {

                    BasicNodeControl decisionControl;
                    double x = 0;
                    if (m_applicationContext.NodeControlFactory.TryGetNodeControl (node, out decisionControl)) {
                        x = decisionControl.DisplayBox.X + decisionControl.DisplayBox.Width / 2 - 10;
                    }

                    ExperimentNode exitDecisionNode = null;
                    // HERZUM SPRINT 2.0: TLAB-148
                    IEnumerable<ExperimentNodeConnection> edges = decisionControl.ExperimentNode.Owner.Edges;
                    // END HERZUM SPRINT 2.0: TLAB-148

                    // HERZUM SPRINT 2.0: TLAB-148
                    // foreach (ExperimentNodeConnection edge in m_applicationViewModel.Experiment.Edges){
                    foreach (ExperimentNodeConnection edge in edges) {
                        // END HERZUM SPRINT 2.0: TLAB-148
                        if (edge.Source.Equals (decisionControl.ExperimentNode)) { 
                            if (edge.Target is ExitDecisionNode) {
                                edge.Target.Data.X = x - 10;
                                exitDecisionNode = edge.Target;
                            } else if (edge.Target is ScopeNode) {
                                if (edge.Target.Data.Metadata.Label.Equals ("Scope 1"))
                                    edge.Target.Data.X = x - 50 - 190;
                                else 
                                    edge.Target.Data.X = x + 50;
                            }
                            m_experimentDrawer.DrawComponent (edge.Target, true);
                            m_experimentDrawer.DrawEdge (edge, false);
                        }
                    }

                    // HERZUM SPRINT 2.0: TLAB-148
                    // foreach (ExperimentNodeConnection edge in m_applicationViewModel.Experiment.Edges){
                    foreach (ExperimentNodeConnection edge in edges) {
                        // END HERZUM SPRINT 2.0: TLAB-148
                        if (edge.Target.Equals (exitDecisionNode) && !edge.Source.Equals (node))
                            m_experimentDrawer.DrawEdge (edge, false);
                    }
                }
                // END HERZUM SPRINT 1.1 IF
            }

            Drag.Finish (args.Context, true, false, args.Time);
        }

        #endregion

        private void CreateExperimentControlToolbar ()
        {
            DockItemToolbar toolbar = m_experimentPad.GetToolbar(PositionType.Top);

            Button runExperimentButton = new Button(new Gtk.Image(Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Play16.png")));
            runExperimentButton.Clicked += new EventHandler(OnRunExperimentButtonClick);
            runExperimentButton.TooltipText = "Run Experiment";
            toolbar.Add (runExperimentButton);

            Button stopExperimentButton = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.Stop.png")));
            stopExperimentButton.Clicked += new EventHandler (OnStopExperimentButtonClick);
            stopExperimentButton.TooltipText = "Stop experiment after all currently running components have finished processing";
            toolbar.Add (stopExperimentButton);

            Button defineCompositeComponentButton = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.DefineCompositeComponent.png")));
            defineCompositeComponentButton.Clicked += new EventHandler (OnDefineCompositeComponentButtonClick);
            defineCompositeComponentButton.TooltipText = "Define composite component from selected nodes";
            toolbar.Add (defineCompositeComponentButton);

            Button packageExperimentButton = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.Icon_PkgCreate16.png")));
            packageExperimentButton.Clicked += OnPackageExperimentButtonClicked;
            packageExperimentButton.TooltipText = "Build package from experiment";
            toolbar.Add (packageExperimentButton);

            Button aboutExperimentButton = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.Icon_Info16.png")));
            aboutExperimentButton.Clicked += new EventHandler (OnAboutExperimentButtonClick);

            if(isChallengeExp()) 
                aboutExperimentButton.TooltipText = "About Challenge";
            else
                aboutExperimentButton.TooltipText = "About Experiment";
            
            toolbar.Add (aboutExperimentButton);
        
             if (isChallengeExp ()) {
                if (!string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitvalue)) {
                    Button publishResults = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.publish3_icon.gif")));
                    publishResults.TooltipText = PUBLISH_RESULTS_TOOLTIP;
                    toolbar.Add (publishResults);
                }

                Button publishChallenge = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.publish4_icon.png")));
                publishChallenge.TooltipText = PUBLISH_CHALLENGE_TOOLTIP;
                publishChallenge.Clicked += new EventHandler(OnPublishChallengeButtonClicked); 
                toolbar.Add (publishChallenge);
            }

            toolbar.ShowAll ();
        }
        
        private void UpdateExperimentToolbarForChallenge(){
       //     DockItemToolbar toolbar = m_experimentPad.GetToolbar(PositionType.Top);
            
            DockItemToolbar toolbar =   m_applicationContext.MainWindow.ExperimentCanvasPad.m_experimentPad.GetToolbar (PositionType.Top);

                        if (!string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitvalue)) {
                Button publishResults = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.publish3_icon.gif")));
                publishResults.TooltipText = PUBLISH_RESULTS_TOOLTIP;
                toolbar.Add (publishResults);
            }

            Button publishChallenge = new Button (new Gtk.Image (Gdk.Pixbuf.LoadFromResource ("TraceLab.UI.GTK.Resources.publish4_icon.png")));
            publishChallenge.TooltipText = PUBLISH_CHALLENGE_TOOLTIP;
            publishChallenge.Clicked += new EventHandler(OnPublishChallengeButtonClicked); 
            toolbar.Add (publishChallenge);

          //  Widget[] w  = m_experimentPad.GetToolbar(PositionType.Top).Children;
            Widget[] w  =  m_applicationContext.MainWindow.ExperimentCanvasPad.m_experimentPad.GetToolbar(PositionType.Top).Children;

            foreach (object widget in w) {
                Button b = widget as Button;
                if (b.TooltipText.Equals ("About Experiment"))
                    b.TooltipText = "About Challenge";
            }
            
            toolbar.ShowAll ();

        }

        private void UpdateExperimentToolbarForExperiment(){
          //  DockItemToolbar toolbar = m_experimentPad.GetToolbar(PositionType.Top);
            DockItemToolbar toolbar =   m_applicationContext.MainWindow.ExperimentCanvasPad.m_experimentPad.GetToolbar (PositionType.Top);

            //Widget[] w  = m_experimentPad.GetToolbar(PositionType.Top).Children;
            Widget[] w  = m_applicationContext.MainWindow.ExperimentCanvasPad.m_experimentPad.GetToolbar(PositionType.Top).Children;

            foreach (object widget in w) {
                Button b = widget as Button;
                if (b.TooltipText.Equals (PUBLISH_RESULTS_TOOLTIP) || b.TooltipText.Equals (PUBLISH_CHALLENGE_TOOLTIP))
                    toolbar.Remove (b);

                if (b.TooltipText.Equals ("About Challenge"))
                    b.TooltipText = "About Experiment";
            }
            
            toolbar.ShowAll ();
   
        }

        private void OnRunExperimentButtonClick (object sender, EventArgs e)
        {
            // HERZUM SPRINT 4: TLAB-215
            /*
            string message = "";
            if (m_applicationViewModel.Experiment != null && m_applicationViewModel.Experiment.ThereAreNodeErrors (out message)){

                ShowMessageDialog(message,
                                  "Run Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;
            }
            */
            // END HERZUM SPRINT 4: TLAB-215

            // HERZUM SPRINT 4: TLAB-215
            try {
                // END HERZUM SPRINT 4.1: TLAB-215
                TraceLabSDK.IProgress progress = m_applicationContext.MainWindow.WindowShell.StatusBar;
                m_applicationContext.MainWindow.WorkspaceWindowPad.ClearUnits ();
                m_applicationViewModel.Experiment.RunExperiment (progress, 
                                                             m_applicationViewModel.Workspace, 
                                                             TraceLab.Core.Components.ComponentsLibrary.Instance);
            }
            // HERZUM SPRINT 4.1: TLAB-215
            catch (Exception ex)
            {
                TraceLabSDK.IProgress progress = m_applicationContext.MainWindow.WindowShell.StatusBar;
                progress.CurrentStatus = "Operation failed: " + ex.Message;
                return;
            }
            // END HERZUM SPRINT 4.1: TLAB-215

        }

        private void OnStopExperimentButtonClick (object sender, EventArgs e)
        {
            m_applicationViewModel.Experiment.StopRunningExperiment ();
        }

        private void OnDefineCompositeComponentButtonClick (object sender, EventArgs e)
        {
            var wizard = new DefineCompositeComponentWizard (m_applicationContext);
            wizard.Modal = true;
            wizard.Show ();
        }

        private void OnPackageExperimentButtonClicked (object sender, EventArgs e)
        {
            // HERZUM SPRINT 4: TLAB-215

            string message = "";
            if (m_applicationViewModel.Experiment != null && m_applicationViewModel.Experiment.ThereAreNodeErrors (out message)){
                
                ShowMessageDialog(message,
                                  "Packaging Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;
            }

            // END HERZUM SPRINT 4.1: TLAB-215
            try {
                // END HERZUM SPRINT 4.1: TLAB-215
                var pckBuilderViewModel = new TraceLab.Core.PackageBuilder.PackageBuilderViewModel (
                    m_applicationViewModel.Experiment, 
                    m_applicationViewModel.WorkspaceViewModel.SupportedTypes);

                var packageBuilder = new PackageBuilderWindow (pckBuilderViewModel);
                packageBuilder.Show ();
                // HERZUM SPRINT 4.1: TLAB-215
            }
            catch (Exception ex)
            {
                    TraceLabSDK.IProgress progress = m_applicationContext.MainWindow.WindowShell.StatusBar;
                    progress.CurrentStatus = "Operation failed: " + ex.Message;
                    return;
            }
            // END HERZUM SPRINT 4.1: TLAB-215
        }

        // HERZUM SPRINT 3.1 TLAB-82
        private void PublishExperiment (String filePathPackage)
        {
            // HERZUM SPRINT 4: TLAB-215

            string message = "";
            if (m_applicationViewModel.Experiment != null && m_applicationViewModel.Experiment.ThereAreNodeErrors (out message)){

                ShowMessageDialog(message,
                                  "Packaging Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;

            }

            // END HERZUM SPRINT 4: TLAB-215

            // END HERZUM SPRINT 4: TLAB-215
            try {
            // END HERZUM SPRINT 4: TLAB-215
                var pckBuilderViewModel = new TraceLab.Core.PackageBuilder.PackageBuilderViewModel (
                    m_applicationViewModel.Experiment, 
                    m_applicationViewModel.WorkspaceViewModel.SupportedTypes);

                pckBuilderViewModel.ExperimentPackageConfig.IncludeIndependentFilesDirs = true;
                pckBuilderViewModel.ExperimentPackageConfig.IncludeOtherPackagesAssemblies = true;
                pckBuilderViewModel.ExperimentPackageConfig.IncludeOtherPackagesFilesDirs = true;

                if (pckBuilderViewModel.PackageSourceInfo == null)
                    pckBuilderViewModel.PackageSourceInfo = new TraceLab.Core.PackageBuilder.PackageSourceInfo ();
                pckBuilderViewModel.PackageSourceInfo.Name = m_applicationViewModel.Experiment.ExperimentInfo.Name;

                var challengePackageBuilder = new ChallengePackageBuilder (pckBuilderViewModel);

                challengePackageBuilder.Build (filePathPackage);
            // HERZUM SPRINT 4.1: TLAB-215
            }
            catch (Exception ex)
            {
                TraceLabSDK.IProgress progress = m_applicationContext.MainWindow.WindowShell.StatusBar;
                progress.CurrentStatus = "Operation failed: " + ex.Message;
                return;
            }
            // END HERZUM SPRINT 4.1: TLAB-215

        }
        // END HERZUM SPRINT 3.1 TLAB-82

        private void OnAboutExperimentButtonClick (object sender, EventArgs e)
        {
            AboutExperimentDialog aboutExpDialog = new AboutExperimentDialog (m_applicationViewModel);           
            aboutExpDialog.Run ();
        }


        // HERZUM SPRINT 3: TLAB-86
        internal void ShowMessageDialog(string message, string title, Gtk.ButtonsType buttonsType, Gtk.MessageType messageType)
        {
            Gtk.MessageDialog dlg = new Gtk.MessageDialog(new Gtk.Window("Message"), 
                                                          Gtk.DialogFlags.Modal, 
                                                          messageType, 
                                                          buttonsType, message);
            dlg.Title = title;
            dlg.Run();
            dlg.Destroy();
        }

        private void OnPublishChallengeButtonClicked (object sender, EventArgs e)
        {
            bool valueMetricVariableInTeml = false;
            string valueMetricVariable;
            string typeMetricVariable;
            string filePathPackage;

            if (!chooseTpkgPath (out filePathPackage))
            {
                return;
            }
                       
            if (filePathPackage==null)
            {
                return;
            }

            if (m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitname == null)
            {
                ShowMessageDialog("Set the variable metric.",
                                  "Publish Challenge Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;
            }

            if (!SearchExperimentResultsUnitvalue(m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitname, out typeMetricVariable, out valueMetricVariable))
            {
                ShowMessageDialog("Run the experiment.",
                                  "Publish Challenge Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;
            }

            if (m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitvalue == null)
            {
                m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnittype = typeMetricVariable;
                m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitvalue= valueMetricVariable;
            }
            else
                valueMetricVariableInTeml = true;

            // HERZUM SPRINT 3.1: TLAB-86
            // string filePath = filePathPackage.Remove(filePathPackage.LastIndexOf("\\")) + "\\" + m_applicationViewModel.Experiment.ExperimentInfo.Name +".teml";
            string filePath = System.IO.Path.GetDirectoryName(filePathPackage);
            if (RuntimeInfo.OperatingSystem == RuntimeInfo.OS.Windows)
                filePath = filePath + "\\" + m_applicationViewModel.Experiment.ExperimentInfo.Name +".teml";
            else
                filePath = filePath + "/" + m_applicationViewModel.Experiment.ExperimentInfo.Name +".teml";
            // END HERZUM SPRINT 3.1: TLAB-86

            if (!ExperimentManager.PublishTeml (m_applicationViewModel.Experiment, filePath))
            {
                ShowMessageDialog("Publication failed.",
                                  "Publish Challenge Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;
            }

            PublishExperiment (filePathPackage);

            File.Delete (filePath);

            if (!valueMetricVariableInTeml)
            {
                m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnittype = null;
                m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitvalue= null;
            }
        }

        private bool SearchExperimentResultsUnitvalue(string nameMetricVariable, out string typeMetricVariable, out string valueMetricVariable)
        {        
            return m_applicationContext.MainWindow.WorkspaceWindowPad.SearchValueMetricVariable (nameMetricVariable, out typeMetricVariable, out valueMetricVariable);
        }


        private void DefiningBenchmarkTest()
        {            
            // TraceLab.Core.Benchmarks.DefiningBenchmark benchmarkDefiner = new TraceLab.Core.Benchmarks.DefiningBenchmark(m_applicationViewModel.Experiment, ComponentsLibrary.Instance, m_applicationViewModel.Workspace,  TraceLab.Core.PackageSystem.PackageManager.Instance, m_applicationViewModel.Workspace.TypeDirectories, null);
            // benchmarkDefiner.DefineBenchmarkApplication (m_applicationViewModel);
        }
        // END HERZUM SPRINT 3: TLAB-86

        private bool chooseTpkgPath(out string path){
            bool result = false;
            var fileChooserDialog = new FileChooserDialog (Mono.Unix.Catalog.GetString ("Choose package name"),
                                                           null,
                                                           FileChooserAction.Save,
                                                           Gtk.Stock.Cancel,
                                                           Gtk.ResponseType.Cancel,
                                                           Gtk.Stock.Save, Gtk.ResponseType.Ok);

            fileChooserDialog.DoOverwriteConfirmation = true;
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            AddFilters(fileChooserDialog);
            String currentPath = changePathToTpkg(m_applicationViewModel.Experiment.ExperimentInfo.FilePath);
            fileChooserDialog.SetFilename(currentPath);
            fileChooserDialog.SetCurrentFolder (currentPath);

            int response = fileChooserDialog.Run();

            string filename = null;

            if(response == (int)Gtk.ResponseType.Ok) 
            {
                filename = fileChooserDialog.Filename;
            }

            fileChooserDialog.Destroy();
            result = true;
            path = addTpkgExtIfNeeded(filename);

            return result;
        }

        private string addTpkgExtIfNeeded(string path){
            string ext = System.IO.Path.GetExtension (path);
            string filename = path;
            if(!string.IsNullOrEmpty(path) && !ext.Equals(TPKG_EXT)){
                filename = filename + TPKG_EXT;
            } 

            return filename;
        }

        private string changePathToTpkg(string originalPath){
            string path = System.IO.Path.GetDirectoryName (originalPath);
            string fileNoExt = System.IO.Path.GetFileNameWithoutExtension (originalPath);
            string newExt = ".tpkg";

            return System.IO.Path.Combine(path, fileNoExt+newExt);
        }

        private static string AddFilters(FileChooserDialog dialog) 
        {
            string defaultExtension = "tpkg";
            // Add experiment files filter
            FileFilter fileFilter = new FileFilter();
            fileFilter.AddPattern(string.Format("*.{0}", defaultExtension));
            fileFilter.Name = Mono.Unix.Catalog.GetString(string.Format("Experiment files (.{0})", defaultExtension));
            dialog.AddFilter(fileFilter);

            //add another option of All files
            FileFilter allFilesFilter = new FileFilter();
            allFilesFilter.Name = Mono.Unix.Catalog.GetString("All files");
            allFilesFilter.AddPattern("*.*");
            dialog.AddFilter(allFilesFilter);

            return defaultExtension;
        }

        #region Experiment crumbs

        // HERZUM SPRINT 1.0

        // HERZUM SPRINT 3.0: TLAB-172
        private bool isInCompositeCrumb(){
            return m_applicationContext.MainWindow.ExperimentCanvasPad.ExperimentCanvasWidget.ExperimentCrumbs.CrumbList.Count > 0;
        }
        // END HERZUM SPRINT 3.0: TLAB-172

        public ExperimentCanvasWidget ExperimentCanvasWidget
        {
            get { return m_experimentCanvasWidget; }
        }

        // HERZUM SPRINT 2.5 TLAB-157
        /*
        private bool IsInEditableExperiment(CompositeComponentGraph experiment){
            if (experiment == null || experiment.OwnerNode == null)
                return true;

            CompositeComponentGraph fatherExperiment = experiment.OwnerNode.Owner as CompositeComponentGraph ;
            if (fatherExperiment != null && fatherExperiment.OwnerNode!= null){
                CompositeComponentBaseMetadata meta = fatherExperiment.OwnerNode.Data.Metadata as CompositeComponentBaseMetadata;
                if (meta != null && meta.ComponentGraph is CompositeComponentEditableGraph)
                    return IsInEditableExperiment (meta.ComponentGraph);
                else
                    return false;
            } else
                return true;
        }
        */
        // END SPRINT 2.5 TLAB-157

        public void SetScopeApplicationModel(ScopeNodeControl scopeNodeControl, ApplicationViewModel applicationViewModel, CompositeComponentGraph subExperiment) 
        {
            // HERZUM SPRINT 1.0
            // scopeNodeControlCurrent = scopeNodeControl;
            // END HERZUM SPRINT 1.0

                if(m_initialized == false || m_dockFrame.GdkWindow == null) 
            {
                //GdkWindow is for each dock frame is assigned when windowShell calls ShowAll(). See DockContainer.OnRealize method
                throw new InvalidOperationException ("ExperimentCanvasPad must be first initialized and dockFrame must have assigned GdkWindow before setting application model.");
            }

            m_applicationViewModel = applicationViewModel;

            // HERZUM SPRINT 1.0
            // Experiment e = new Experiment ();
            // m_applicationViewModel = ApplicationViewModel.CreateNewApplicationViewModel (applicationViewModel, e);
            // m_applicationContext = new ApplicationContext (m_applicationViewModel);
            // END HERZUM SPRINT 1.0

            if(m_applicationViewModel.Experiment == null) 
            {
                m_experimentPad.Content = new WelcomePageWidget(m_applicationContext);
            } 
            else 
            {
                // CreateExperimentControlToolbar();

                // HERZUM SPRINT 1.0
                m_subExperiment = new CompositeComponentEditableGraph (subExperiment);
                // END HERZUM SPRINT 1.0

                // HERZUM SPRINT 1.0 PROGRESS
                // HERZUM SPRINT 2.4 TLAB-157
                if (subExperiment.OwnerNode != null)
                    // END HERZUM SPRINT 2.4 TLAB-157
                    m_subExperiment.OwnerNode = subExperiment.OwnerNode;
                // HERZUM SPRINT 2.4 TLAB-157
                if (subExperiment.GraphIdPath != null)
                    // END HERZUM SPRINT 2.4 TLAB-157
                    m_subExperiment.GraphIdPath = subExperiment.GraphIdPath;
                // END HERZUM SPRINT 1.0

                // HERZUM SPRINT 4.3: TLAB-238 TLAB-243
                if (m_subExperiment.ExperimentInfo != null)
                    m_subExperiment.ExperimentInfo.FilePath = m_applicationContext.Application.Experiment.ExperimentInfo.FilePath;
                // END HERZUM SPRINT 4.3: TLAB-238 TLAB-243

                // HERZUM SPRINT 1.1 LOOP
                // ScopeMetadata scopeMetadata = scopeNodeControlCurrent.ExperimentNode.Data.Metadata as ScopeMetadata;

                // HERZUM SPRINT 2.1
                // ScopeBaseMetadata scopeMetadata = scopeNodeControlCurrent.ExperimentNode.Data.Metadata as ScopeBaseMetadata;
                ScopeBaseMetadata scopeMetadata = scopeNodeControl.ExperimentNode.Data.Metadata as ScopeBaseMetadata;
                // END HERZUM SPRINT 2.1

                // END HERZUM SPRINT 1.1 LOOP

                scopeMetadata.SetSubExperiment (m_subExperiment);
                m_experimentCanvasWidget = new ExperimentCanvasWidget ();
                m_experimentPad.Content = m_experimentCanvasWidget;

                bool isExperimentEditable = m_applicationViewModel.Experiment is IEditableExperiment;
             //   isExperimentEditable = isChallengePasswordBeenUsed ();

                m_experimentDrawer = new ExperimentDrawer (m_experimentCanvasWidget, m_applicationContext.NodeControlFactory,
                                                           m_applicationContext.NodeConnectionControlFactory);

                //m_experimentDrawer.DrawExperiment(m_applicationViewModel.Experiment, isExperimentEditable);

                // HERZUM SPRINT 2.5 TLAB-157
                // isExperimentEditable = isExperimentEditable && IsInEditableExperiment (m_subExperiment);
                isExperimentEditable = checkEditable (m_subExperiment);
                // END HERZUM SPRINT 2.5 TLAB-157

                if (scopeNodeControl is ChallengeNodeControl)
                    isExperimentEditable = true;

                // HERZUM SPRINT 3.0: TLAB-172
                isExperimentEditable = isExperimentEditable && !isInCompositeCrumb();
                // END HERZUM SPRINT 3.0: TLAB-172

                m_experimentDrawer.DrawExperiment(m_subExperiment, isExperimentEditable);               

                // HERZUM SPRINT 1.0
                m_subExperiment.NodeRemoved += OnNodeRemoved;
                m_subExperiment.EdgeRemoved += OnEdgeRemoved;
                //
                //m_applicationViewModel.Experiment.NodeRemoved += OnNodeRemoved;
                //m_applicationViewModel.Experiment.EdgeRemoved += OnEdgeRemoved;
                // END HERZUM SPRINT 1.0

                if(isExperimentEditable) 
                {
                    //enable drop of components to canvas
                    EnableDrop ();
                }
            }
        }

        //TRUE if it is editable
        private bool checkEditable(CompositeComponentGraph experiment){
            if (experiment == null || experiment.OwnerNode == null)
                return true;

            if(isChallengeExp()){
                if (!hasAnyPasswordBeenUsed () || !hasChallengePasswordBeenUsed()) {
                    return true;
                } else {
                    CompositeComponentGraph fatherExperiment = experiment.OwnerNode.Owner as CompositeComponentGraph;
                    if (fatherExperiment != null && fatherExperiment.OwnerNode != null) {
                        if ((fatherExperiment.OwnerNode.Data.Metadata as ChallengeMetadata) != null) {
                            return true;
                        } else {
                            return checkEditable (fatherExperiment);
                        }

                    } else {
                        return false;
                    }
                }
            } else {
                return true;
            }
                 
        }

        // ***************************************************
        // HERZUM SPRINT 2.4 TLAB-157
        public void SetApplicationModel(ApplicationViewModel applicationViewModel,  ExperimentCanvasWidget experimentCanvasWidget, CompositeComponentGraph subExperiment) 
        {
            // HERZUM SPRINT 1.0
            // scopeNodeControlCurrent = scopeNodeControl;
            // END HERZUM SPRINT 1.0

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
                // CreateExperimentControlToolbar();


                m_subExperiment = new CompositeComponentEditableGraph(subExperiment);
                if (subExperiment.OwnerNode != null)
                    m_subExperiment.OwnerNode = subExperiment.OwnerNode;
                if (subExperiment.GraphIdPath != null)
                    m_subExperiment.GraphIdPath = subExperiment.GraphIdPath;

                m_experimentCanvasWidget = experimentCanvasWidget;


                m_experimentPad.Content = m_experimentCanvasWidget;

                // bool isExperimentEditable = m_applicationViewModel.Experiment is IEditableExperiment;


                m_experimentDrawer = new ExperimentDrawer(m_experimentCanvasWidget, 
                                                          new NodeControlFactory(m_applicationContext),
                                                          new NodeConnectionControlFactory(m_applicationContext));


                m_experimentDrawer.DrawExperiment(m_subExperiment, false);

            }
        }
        // END HERZUM SPRINT 2.4 TLAB-157
        // ***************************************************

        /*
        // HERZUM SPRINT 1.1 CANVAS
        internal void PaintScope (CompositeComponentGraph componentGraph) 
        {
            this.m_experimentDrawer.DrawExperiment (componentGraph, true);
        }
        // END HERZUM SPRINT 1.1 CANVAS
        */

        internal void DisplayScope (ExperimentCanvasWidget scope_experimentCanvasWidget, double x, double y) 
        {   
            // HERZUM SPRINT 1.2 TLAB-133
            // scope_experimentCanvasWidget.HeightRequest = (int)height;
            // scope_experimentCanvasWidget.WidthRequest = (int)width;
            // END HERZUM SPRINT 1.2 TLAB-133

            m_experimentCanvasWidget.ExperimentCanvas.View.AddWidget (scope_experimentCanvasWidget, x, y);


            scope_experimentCanvasWidget.ExperimentCanvas.View.ClearSelection ();

            // HERZUM SPRINT 1.0
            /*
            if (scopeNodeControlCurrent!=null){
                scopeNodeControlCurrent.maxExperimentButton.Clicked += new EventHandler(OnMaxSCopeNodeButtonClick);
                scopeNodeControlCurrent.maxExperimentButton.TooltipText = "Massimizzi";
                scopeNodeControlCurrent.maxExperimentButton.Show ();
            }
            */

            // m_experimentCanvasWidget.ExperimentCanvas.View.AddWidget (scopeNodeControlCurrent.maxExperimentButton, x + 300, y-40);
            // END HERZUM SPRINT 1.0

            m_experimentPad.Widget.ShowAll ();
        }


        internal void MoveScope(ExperimentCanvasWidget scope_experimentCanvasWidget, double x, double y) 
        {   
            m_experimentCanvasWidget.ExperimentCanvas.View.MoveWidget (scope_experimentCanvasWidget, x, y);

            scope_experimentCanvasWidget.ExperimentCanvas.View.ClearSelection ();

            // HERZUM SPRINT 2.1
            /*
            // HERZUM SPRINT 1.0
            if (scopeNodeControlCurrent!=null){
                m_experimentCanvasWidget.Show();
            }
            // END HERZUM SPRINT 1.0
            */
            m_experimentCanvasWidget.Show ();
            // END HERZUM SPRINT 2.1
        }


        // HERZUM SPRINT 1.1 FOCUS
        /*
        internal void RedrawScope(ExperimentCanvasWidget scopeCanvasWidget, double x, double y, double width, double height)
        {   
            m_experimentCanvasWidget.ExperimentCanvas.View.RemoveWidget (scopeCanvasWidget);
            m_experimentCanvasWidget.ExperimentCanvas.View.AddWidget (scopeCanvasWidget, x, y);
            scopeCanvasWidget.ExperimentCanvas.View.ClearSelection();
        }
        */
        // HERZUM SPRINT 5.3 TLAB-185
        // internal void RedrawScope (Widget widget, double x, double y, double width, double height)
        internal void RedrawScope (Widget widget, double x, double y)
        // END HERZUM SPRINT 5.3 TLAB-185
        {   

            m_experimentCanvasWidget.ExperimentCanvas.View.RemoveWidget (widget);
            m_experimentCanvasWidget.ExperimentCanvas.View.AddWidget (widget, x, y);

        }
        // END HERZUM SPRINT 1.1 FOCUS
        // END HERZUM 1.0  

        // HERZUM SPRINT 1.1 IF
        // DUPLICATE
        // No visible from ScopeNode.cs
        private void TryFixScopeDecisionEntryAndExitNodes (ExperimentNode sourceVert, ExperimentNode targetVert)
        {
            //case 1: sourceVert is ScopeNode, and target is ExitDecisionNode
            ScopeNode scopeNode = sourceVert as ScopeNode;
            if (scopeNode != null) {
                ExitDecisionNode exitDecisionNode = targetVert as ExitDecisionNode;
                if (exitDecisionNode != null) {
                    scopeNode.ExitDecisionNode = exitDecisionNode;
                }
            } else {
                //case 2: targetNode is ScopeNode, and source is DecisionNode
                scopeNode = targetVert as ScopeNode;
                if (scopeNode != null) {
                    ExperimentDecisionNode decisionNode = sourceVert as ExperimentDecisionNode;
                    if (decisionNode != null) {
                        scopeNode.DecisionNode = decisionNode;
                    }
                }
            }
        }

        // HERZUM SPRINT 2.0: TLAB-148
        internal void AddScopeInDecisionComponent(DecisionNodeControl decisionNodeControl)
        {   
            double x = 0;
            IEnumerable<ExperimentNodeConnection> edges = null;
            IEnumerable<ExperimentNode> vertices = null;

            if (m_subExperiment != null){
                edges = m_subExperiment.Edges;
                vertices = m_subExperiment.Vertices;
            }
            else {
                edges = m_applicationViewModel.Experiment.Edges;
                vertices = m_applicationViewModel.Experiment.Vertices;
            }

            foreach (ExperimentNodeConnection edge in edges){
                if (edge.Source.Equals(decisionNodeControl.ExperimentNode))
                    if (edge.Target is ScopeNode)
                        if (edge.Target.Data.X > x)
                            x = edge.Target.Data.X;
            }

            String label = "Scope";

            for (int n=1;; n++) {
                bool found = false;
                foreach (ExperimentNodeConnection edge in edges)
                    if (edge.Source.Equals(decisionNodeControl.ExperimentNode))
                        if (edge.Target is ScopeNode)
                            if (edge.Target.Data.Metadata.Label.Equals("Scope " + n)){
                            found = true;
                            break;
                        }
                if (!found){
                    label = "Scope " + n;
                    break;
                }
            }

            Cairo.PointD translation = m_experimentCanvasWidget.ExperimentCanvas.View.ViewToDrawing(decisionNodeControl.DisplayBox.X, decisionNodeControl.DisplayBox.Y);
            ScopeMetadataDefinition meta_def = new ScopeMetadataDefinition(ScopeMetadataDefinition.ScopeGuid);

            meta_def.Label = label;

            ExperimentNode scopeNode = null;
            if (m_subExperiment != null)
                scopeNode = m_subExperiment.AddComponentFromDefinition (meta_def, 
                                                                        x + 230, translation.Y + 120);
            else
                scopeNode = m_applicationViewModel.Experiment.AddComponentFromDefinition (meta_def, x + 230, translation.Y + 120);

            TryFixScopeDecisionEntryAndExitNodes (decisionNodeControl.ExperimentNode, scopeNode);

            m_experimentDrawer.DrawComponent (scopeNode, true);

            ExperimentNodeConnection new_edge = null;
            if (m_subExperiment != null)
                new_edge = m_subExperiment.AddConnection (decisionNodeControl.ExperimentNode, scopeNode);
            else
                new_edge = m_applicationViewModel.Experiment.AddConnection (decisionNodeControl.ExperimentNode, scopeNode);

            m_experimentDrawer.DrawEdge (new_edge, false);

            ExperimentNode exitNode = null;
            bool response = false;
            foreach (ExperimentNode n in vertices)
                if (n is ExitDecisionNode) {
                if (m_subExperiment != null)
                    response = m_subExperiment.TryGetEdge (decisionNodeControl.ExperimentNode, n, out new_edge);
                else
                    response = m_applicationViewModel.Experiment.TryGetEdge (decisionNodeControl.ExperimentNode, n, out new_edge);
                if (response) {
                    exitNode = n;
                    break;
                }
            }

            if (exitNode != null) {
                TryFixScopeDecisionEntryAndExitNodes (scopeNode, exitNode);
                if (m_subExperiment != null)
                    new_edge = m_subExperiment.AddConnection (scopeNode, exitNode);
                else
                    new_edge = m_applicationViewModel.Experiment.AddConnection (scopeNode, exitNode);
                m_experimentDrawer.DrawEdge (new_edge, false);
            }

        }
        // END HERZUM SPRINT 2.0: TLAB-148




        internal void DisplaySubgraph(CompositeComponentMetadata metadata) 
        {
            //always clear crumbs
            m_experimentCanvasWidget.ExperimentCrumbs.RemoveAll ();

            List<Crumb> crumbs = ExperimentCrumbGatherer.GatherCrumbs (metadata);

            m_experimentCanvasWidget.ExperimentCrumbs.ShowAll ();

            foreach(Crumb crumb in crumbs) 
            {
                m_experimentCanvasWidget.ExperimentCrumbs.Append(crumb);
                crumb.Clicked += Redraw;
            }

            //set last one active (triggers clicked event)
            m_experimentCanvasWidget.ExperimentCrumbs.Active = crumbs [crumbs.Count - 1];

            //attach handler to the first element so that it hides and removes all crumbs
            crumbs[0].Clicked += (object sender, EventArgs e) => 
            {
                m_experimentCanvasWidget.ExperimentCrumbs.HideAll();

                // HERZUM SPRINT 2.5 TLAB-157
                //always clear crumbs
                m_experimentCanvasWidget.ExperimentCrumbs.RemoveAll();
                // END HERZUM SPRINT 2.5 TLAB-157
            };
        }

        // HERZUM SPRINT 2.5 TLAB-157
        private void ClearWidgets(BaseExperiment experiment)
        {
            if  (experiment == null) return;
            foreach(ExperimentNode node in experiment.Vertices) {
                if (node is ScopeNodeBase)
                {
                    BasicNodeControl componentControl = null;
                    if (m_applicationContext.NodeControlFactory.TryGetNodeControl (node, out componentControl)){
                        ScopeNodeControl scopeNodeControl = componentControl as ScopeNodeControl;
                        if (scopeNodeControl != null){
                            ClearWidgets((node.Data.Metadata as ScopeBaseMetadata).ComponentGraph);
                            OnNodeRemoved (node);
                        }
                    }
                } 
                if (node is CompositeComponentNode)
                    ClearWidgets((node.Data.Metadata as CompositeComponentBaseMetadata).ComponentGraph);
            }
        }
        // END HERZUM SPRINT 2.5 TLAB-157

        private void Redraw(object sender, EventArgs e)
        {
            ExperimentCrumb crumb = (ExperimentCrumb)sender;
            m_experimentCanvasWidget.ExperimentCanvas.View.ClearAll();

            // HERZUM SPRINT 2.5 TLAB-157
            if (m_subExperiment == null)
                ClearWidgets(m_applicationContext.Application.Experiment);
            // END HERZUM SPRINT 2.5 TLAB-157

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


        // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
        internal void DrawSubExperiment(CompositeComponentGraph sub_experiment)
        {   
            if (m_subExperiment != null)
                m_experimentDrawer.DrawExperiment (sub_experiment, true);              
        }

        internal void DisplayAddedSubgraph(BaseExperiment experiment) 
        {

            foreach(ExperimentNode node in experiment.Vertices) 
            {
                BasicNodeControl componentControl = null;
                if (!m_applicationContext.NodeControlFactory.TryGetNodeControl (node, out componentControl))
                    m_experimentDrawer.DrawComponent(node, true);
            }

            foreach(ExperimentNodeConnection edge in experiment.Edges) 
            {
                NodeConnectionControl componentControl = null;
                if (!m_applicationContext.NodeConnectionControlFactory.TryGetConnectionControl(edge, out componentControl))
                    m_experimentDrawer.DrawEdge(edge, true);
            }
        }
        // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

        #endregion

        private bool isChallengeExp(){
            if (!string.IsNullOrEmpty(m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge) && m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge.Equals ("True")) {
                return true;
            } else {
                return false;
            }

        }

    }
}


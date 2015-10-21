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
using TraceLab.Core.Components;
using Gtk;
using TraceLab.Core.Utilities;
using TraceLab.Core.Experiments;
using MonoDevelop.Components.PropertyGrid;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ComponentInfoPanel : Gtk.EventBox
    {
        private PropertyGrid propertyGrid;

        // HERZUM SPRINT 2.4: TLAB-162
        private ApplicationContext m_applicationContext;
        public ComponentInfoPanel (ApplicationContext applicationContext) : this()
        {
            m_applicationContext = applicationContext;

            // HERZUM SPRINT 5.0: TLAB-238 TLAB-243
            propertyGrid.DataRoot = System.IO.Path.GetDirectoryName(m_applicationContext.Application.Experiment.ExperimentInfo.FilePath);
            // END // HERZUM SPRINT 5.0: TLAB-238 TLAB-243
        }
        // END HERZUM SPRINT 2.4: TLAB-162

        public ComponentInfoPanel()
        { 
            this.Build();
            //set EventBox VisibleWindow to invisible so it is only used to trap events
            this.VisibleWindow = false;
            this.AboveChild = false;
            //this.Events = Gdk.EventMask.AllEventsMask;

            propertyGrid = new PropertyGrid();
            this.configurationExpander.Add(propertyGrid);
            this.configurationExpander.ShowAll();
            this.errorTextView.InitColorTags();
            // HERZUM SPRINT 4.2: TLAB-225
            this.IOExpander.Expanded = false;
            this.configurationExpander.Expanded = false;
            this.componentInfoExpander.Expanded = false;
            propertyGrid.ShowToolbar = false;
            propertyGrid.PropertySort=PropertySort.Alphabetical;
            this.ResizeMode = ResizeMode.Immediate;
            // END HERZUM SPRINT 4.2: TLAB-225

        }

        private BasicNodeControl m_component;
        public BasicNodeControl Component 
        {
            get { return m_component; }
            set 
            {
                if(m_component != value) 
                {
                    m_component = value;
                    m_metadata = m_component.ExperimentNode.Data.Metadata as IConfigurableAndIOSpecifiable;

                    if(m_metadata == null) 
                    {
                        throw new InvalidOperationException("Component info panel can be displayed only for IConfigurableAndIOSpecifiable components");
                    }

                    bool isEditable = m_component.IsEditable;
                    
                    SetComponentInfo(isEditable);
                    SetConfigurationInfo(isEditable);
                    SetInputInfo(isEditable);
                    SetOutputInfo(isEditable);
                    // HERZUM BUG FIX: alignment input-output TLAB-255
                    ResizeInputOutputInfo ();
                    // END HERZUM BUG FIX: alignment input-output TLAB-255
                    RefreshError();

                    m_component.ExperimentNode.ErrorChanged += HandleErrorChanged;
                    m_experimentOwner = m_component.ExperimentNode.Owner;
                }
            }
        }

        private IExperiment m_experimentOwner;

        /// <summary>
        /// Displays error
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void HandleErrorChanged (object sender, ExperimentNodeErrorEventArgs e)
        {
            RefreshError();
        }

        private void RefreshError()
        {
            if (m_component.ExperimentNode.HasError) 
            {
                this.errorTextView.ShowError(m_component.ExperimentNode.ErrorMessage);
            }
            else 
            {
                this.errorTextView.Visible = false;
            }
        }

        private IConfigurableAndIOSpecifiable m_metadata;

        private void SetComponentInfo(bool isEditable)
        {
            this.componentLabelValue.Text = m_metadata.Label;
            this.componentLabelValue.IsEditable = isEditable;
            this.componentLabelValue.Sensitive = isEditable;

            // corressponding definition might be null, if component is not in the library
            if(m_metadata.MetadataDefinition != null)
            {
                this.componentNameValue.Text = m_metadata.MetadataDefinition.Label;
                this.versionValue.Text = m_metadata.MetadataDefinition.Version;
                this.authorValue.Text = m_metadata.MetadataDefinition.Author;
                this.descriptionValue.Text = m_metadata.MetadataDefinition.Description;
            }
        }

        private void SetConfigurationInfo(bool isEditable) 
        {
            if(m_metadata.ConfigWrapper.ConfigValues.Count > 0)
            {
                propertyGrid.CurrentObject = m_metadata.ConfigWrapper;
                propertyGrid.Sensitive = isEditable;
            }
            else
            {
                this.configurationExpander.Visible = false;
            }
        }

        // HERZUM BUG FIX: alignment input-output TLAB-255
        private int maxLengthInput=0;
        private int maxLengthInputMapped=0;
        TreeViewColumn t_Input, t_InputMapped,  t_InputType;
        // END HERZUM BUG FIX: alignment input-output TLAB-255
        private void SetInputInfo(bool isEditable) 
        {
            Gdk.Color colorBack;
            if(m_metadata.IOSpec.Input.Count > 0)
            {
                //set columns in input and output views
                Gtk.NodeStore inputStore = new Gtk.NodeStore(typeof(IOItemNode));

                Gtk.CellRendererText textRenderer = new Gtk.CellRendererText();

                // HERZUM SPRINT 4.2: TLAB-226
                // HERZUM SPRINT 5: TLAB-242
                colorBack = new  Gdk.Color (245, 245, 245);
                textRenderer.CellBackgroundGdk = colorBack;
                // END HERZUM SPRINT 5: TLAB-242
                //textRenderer.CellBackground = "grey";
                // END HERZUM SPRINT 4.2: TLAB-226

                //inputs
                // HERZUM BUG FIX: alignment input-output TLAB-255
                //HERZUM SPRINT 5.4 TLAB-241;
                t_Input = this.inputView.AppendColumn( Convert.ToChar (187) + " Input", textRenderer, "text", 0);
                t_Input.Resizable = true;
                //END HERZUM SPRINT 5.4 TLAB-241
                t_InputMapped=CreateInputMappingColumnWithComboBox (inputStore, "Mapped to");
                t_InputMapped.Resizable = true;
                inputView.AppendColumn(t_InputMapped);
                t_InputType=this.inputView.AppendColumn("Type", textRenderer, "text", 2);
                t_InputType.Resizable = true;
                //END HERZUM BUG FIX TLAB-255

                //prepare input and output store
                // HERZUM BUG FIX: alignment input-output TLAB-255
                int currentLengthInput=0;
                int currentLengthInputMapped=0;
                foreach(IOItem item in m_metadata.IOSpec.Input.Values) 
                {
                    inputStore.AddNode(new IOItemNode(item));
                    currentLengthInput = item.IOItemDefinition.Name.Length;
                    currentLengthInputMapped = item.MappedTo.Length;
                    if (currentLengthInput>maxLengthInput)
                        maxLengthInput = currentLengthInput;
                    if (currentLengthInputMapped > maxLengthInputMapped)
                        maxLengthInputMapped = currentLengthInputMapped;
                }
                // END HERZUM BUG FIX: alignment input-output TLAB-255

                inputView.NodeStore = inputStore;

                //disables/enables the controls
                inputView.Sensitive = isEditable;

                inputView.HoverSelection = true;
                inputView.NodeSelection.Changed += SelectionHandleChanged;
            }
            else
            {
                inputView.Visible = false;
            }
        }

        private void SelectionHandleChanged (object sender, EventArgs e)
        {
            Gtk.NodeSelection selection = (Gtk.NodeSelection) sender;

            ExperimentHelper.ClearHighlightIOInExperiment(m_experimentOwner);

            //selected node maybe null if the moust moved out from the tree view, and selection change to nothing
            if (selection.SelectedNode != null) 
            {
                IOItemNode item = (IOItemNode)selection.SelectedNode;
                ExperimentHelper.HighlightIOInExperiment(m_experimentOwner, item.MappedTo);

                // HERZUM BUG FIX TLAB-254.3  
                propertyGrid.UnSelect ();
                // END HERZUM BUG FIX TLAB-254.3
            }
        }

        private void RefreshIOHighlightInExperiment(string mapping)
        {
            ExperimentHelper.ClearHighlightIOInExperiment(m_experimentOwner);
            ExperimentHelper.HighlightIOInExperiment(m_experimentOwner, mapping);
        }


        // HERZUM BUG FIX: alignment input-output TLAB-255
        private int maxLengthOutput=0;
        private int maxLengthOutputAs=0;
        TreeViewColumn t_Output, t_OutputAs,  t_OutputType;
        // END HERZUM BUG FIX: alignment input-output TLAB-255
        private void SetOutputInfo(bool isEditable) 
        {
            if(m_metadata.IOSpec.Output.Count > 0)
            {
                //set columns in input and output views
                Gtk.NodeStore outputStore = new Gtk.NodeStore(typeof(IOItemNode));
                Gtk.CellRendererText textRenderer = new Gtk.CellRendererText();

                Gtk.CellRendererText editableTextRenderer = new CellRendererText();
                editableTextRenderer.Editable = isEditable;
                editableTextRenderer.Edited += delegate(object o, EditedArgs args) {
                    IOItemNode n = (IOItemNode)outputStore.GetNode (new TreePath (args.Path));
                    n.MappedTo = args.NewText;
                    RefreshIOHighlightInExperiment(n.MappedTo);
                };

                // HERZUM SPRINT 4.2: TLAB-226
                //textRenderer.CellBackground = "grey";
                // HERZUM SPRINT 5: TLAB-242
                Gdk.Color colorBack = new  Gdk.Color (245, 245, 245);
                textRenderer.CellBackgroundGdk = colorBack;
                // END HERZUM SPRINT 5: TLAB-242
                // END HERZUM SPRINT 4.2: TLAB-226 

                // HERZUM BUG FIX: alignment input-output TLAB-255
                //HERZUM SPRINT 5.4 TLAB-241
                t_Output = this.outputView.AppendColumn(Convert.ToChar(171) + " Output", textRenderer, "text", 0);
                t_Output.Resizable = true;
                //END HERZUM SPRINT 5.4 TLAB-241
                t_OutputAs=this.outputView.AppendColumn("Output as", editableTextRenderer, "text", 1);
                t_OutputAs.Resizable = true;
                t_OutputType=this.outputView.AppendColumn("Type", textRenderer, "text", 2);
                t_OutputType.Resizable = true;
                // END HERZUM BUG FIX: alignment input-output TLAB-255

                int currentLengthOutput;
                int currentLengthOutputAs;
                foreach(IOItem item in m_metadata.IOSpec.Output.Values) 
                {
                    outputStore.AddNode(new IOItemNode(item));
                    // HERZUM BUG FIX: alignment input-output TLAB-255
                    currentLengthOutput = item.IOItemDefinition.Name.Length;
                    currentLengthOutputAs = item.MappedTo.Length;
                    if (currentLengthOutput>maxLengthOutput)
                        maxLengthOutput = currentLengthOutput;
                    if (currentLengthOutputAs>maxLengthOutputAs)
                        maxLengthOutputAs = currentLengthOutputAs;
                    // END HERZUM BUG FIX: alignment input-output TLAB-255
                }

                outputView.NodeStore = outputStore;
                
                //disables/enables the controls
                outputView.Sensitive = isEditable;

                outputView.HoverSelection = true;
                outputView.NodeSelection.Changed += SelectionHandleChanged;
            }
            else
            {
                outputView.Visible = false;
            }
        }

        // HERZUM BUG FIX: alignment input-output TLAB-255
        private void ResizeInputOutputInfo() 
        {
            if (maxLengthOutput < maxLengthInput)
            {
                if (outputView.Columns.Length > 0)
                    outputView.Columns [0].MinWidth = maxLengthInput * 7 + 15;
                if (inputView.Columns.Length > 0)
                    inputView.Columns [0].MinWidth = maxLengthInput * 7 + 15;
            }
            else
                {
                if (inputView.Columns.Length>0)
                    inputView.Columns [0].MinWidth = maxLengthOutput * 7 + 15;
                if (outputView.Columns.Length>0)
                    outputView.Columns [0].MinWidth = maxLengthOutput * 7 + 15;
                }


            if (maxLengthOutputAs < maxLengthInputMapped)
            {
                if (outputView.Columns.Length > 0)
                    outputView.Columns [1].MinWidth = maxLengthInputMapped * 7 + 15;
                if (inputView.Columns.Length > 0)
                    inputView.Columns [1].MinWidth = maxLengthInputMapped * 7 + 15;
            }
            else
            {
                if (inputView.Columns.Length>0)
                    inputView.Columns [1].MinWidth = maxLengthOutputAs * 7 + 15;
                if (outputView.Columns.Length>0)
                    outputView.Columns [1].MinWidth = maxLengthOutputAs * 7 + 15;
            }
        }
        // END HERZUM BUG FIX: alignment input-output TLAB-255


        /// <summary>
        /// Creates the input mapping column with combo box.
        /// TODO improve the input mapping combo box:
        /// 1. it doesn't set values until combo box loses focus within table view confirm change - Edited event is raised only then
        /// 2. there is no indication that the field can be modified - render combo box always, OR show some icon that it can be modified
        /// </summary>
        /// <returns>
        /// The input mapping column with combo box.
        /// </returns>
        /// <param name='inputStore'>
        /// Input store.
        /// </param>
        private TreeViewColumn CreateInputMappingColumnWithComboBox(NodeStore inputStore, string columntTitle)
        {
            Gtk.CellRendererCombo comboRenderer = new Gtk.CellRendererCombo();
            
            comboRenderer.HasEntry = false;
            comboRenderer.Mode = CellRendererMode.Editable;
            comboRenderer.TextColumn = 0;
            comboRenderer.Editable = true;

            ListStore comboBoxStore = new ListStore (typeof(string));
            comboRenderer.Model = comboBoxStore;

            //when user activates combo box, refresh combobox store with available input mapping per node
            comboRenderer.EditingStarted += delegate (object o, EditingStartedArgs args) 
            {
                comboBoxStore.Clear ();
                IOItemNode currentItem = (IOItemNode)inputStore.GetNode (new TreePath (args.Path));
                ExperimentNode currentNode = m_component.ExperimentNode;
                string currentType = currentItem.Type;
                // HERZUM SPRINT 2.4: TLAB-162
                //InputMappings availableInputMappingsPerNode = new InputMappings (currentNode.Owner);
                InputMappings availableInputMappingsPerNode = new InputMappings (m_applicationContext.Application.Experiment);
                // END HERZUM SPRINT 2.4: TLAB-162
                if (currentNode != null && availableInputMappingsPerNode.ContainsMappingsForNode (currentNode)) {
                    foreach (string incomingOutput in availableInputMappingsPerNode [currentNode].Keys) {
                        if (string.Equals (currentType, availableInputMappingsPerNode [currentNode] [incomingOutput])) {
                            comboBoxStore.AppendValues (Mono.Unix.Catalog.GetString (incomingOutput));
                        }
                    }
                }
            };

            //when edition has been completed set current item node with proper mapping
            comboRenderer.Edited += delegate (object o, EditedArgs args) {
                IOItemNode n = (IOItemNode)inputStore.GetNode (new TreePath (args.Path));
                n.MappedTo = args.NewText;
                RefreshIOHighlightInExperiment(n.MappedTo);
            };

            //finally create the column with above combo renderer
            var mappedToColumn = new TreeViewColumn ();
            mappedToColumn.Title = columntTitle;
            mappedToColumn.PackStart (comboRenderer, true);

            //this method sets the text view to current mapping, when combo box is not active
            mappedToColumn.SetCellDataFunc (comboRenderer, delegate (TreeViewColumn tree_column, CellRenderer cell, ITreeNode node) {
                IOItemNode currentItem = (IOItemNode)node;
                comboRenderer.Text = currentItem.MappedTo;
            });

            return mappedToColumn;
        }

        /// <summary>
        /// Handles the changed of the component label.
        /// It updates label in the model metadata
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// E.
        /// </param>
        protected void HandleComponentLabelChanged(object sender, EventArgs e)
        {
            m_metadata.Label = this.componentLabelValue.Text;
            //update text in the control
            this.Component.Text = m_metadata.Label;
        }


        // HERZUM SPRINT 4.2: TLAB-225
        // HERZUM SPRINT 5.1: TLAB-225
        int wPanel, hPanel, hIO=0;
        protected void OnIOExpanderActivated (object sender, EventArgs e)
        {
            System.Collections.IEnumerator enumerator;
            if (hIO==0)
            {
                if (inputView.NodeStore!=null)
                {
                    // HERZUM BUG FIX TLAB-254
                    hIO += 30;
                    // END HERZUM BUG FIX TLAB-254
                    enumerator =inputView.NodeStore.GetEnumerator();
                    while (enumerator.MoveNext())
                        hIO += 25;
                }
                if (outputView.NodeStore!=null)
                {
                    // HERZUM BUG FIX TLAB-254
                    hIO += 30;
                    // END HERZUM BUG FIX TLAB-254
                    enumerator =outputView.NodeStore.GetEnumerator();
                    while (enumerator.MoveNext())
                        hIO += 25;
                }
            }
            GdkWindow.GetSize(out wPanel, out hPanel);
            if (IOExpander.Expanded)
            {
                GdkWindow.Resize(wPanel,hPanel+hIO);
            }
            else
            {
                if (configurationExpander.Visible && !configurationExpander.Expanded && componentInfoExpander.Visible && !componentInfoExpander.Expanded)
                    GdkWindow.Resize (600, 100);
                else
                    GdkWindow.Resize (wPanel, hPanel - hIO);
            }
            //throw new NotImplementedException ();
        }


        int hConf=0;
        protected void OnConfigurationExpanderActivated (object sender, EventArgs e)
        {
            if (hConf==0)
            {
                // HERZUM BUG FIX TLAB-254
                //hConf = 10;
                // END HERZUM BUG FIX TLAB-254
                foreach (Gtk.Object obj in configurationExpander.Children)
                    if (obj is PropertyGrid)
                        //hConf += ((PropertyGrid)obj).CountPropertyGrid ()*27;  
                        hConf += ((PropertyGrid)obj).CountPropertyGrid ()*25;                
            }

            GdkWindow.GetSize (out wPanel, out hPanel);
            if (configurationExpander.Expanded) {
                GdkWindow.Resize (wPanel, hPanel + hConf);
            } else {
                if (componentInfoExpander.Visible && !componentInfoExpander.Expanded && IOExpander.Visible && !IOExpander.Expanded)
                    GdkWindow.Resize (600, 100);
                else
                    GdkWindow.Resize (wPanel, hPanel - hConf);
            }
            //throw new NotImplementedException ();
        }


        int wInfo=0, hInfo=0, hDesc=0; 
        protected void OnComponentInfoExpanderActivated (object sender, EventArgs e)
        {
            if (wInfo==0 || hInfo==0)
            {
                componentInfoExpander.GdkWindow.GetSize (out wInfo, out hInfo);
                hInfo = hInfo - hConf - hIO;
                if (descriptionValue.Visible) {
                    // HERZUM BUG FIX TLAB-254
                    //hDesc = (int)(((descriptionValue.Text.Length / 50)) + 1 * 20) + 30;   
                    hDesc = (int)(((descriptionValue.Text.Length / 50) + 1) * 10+5);     
                    // END HERZUM BUG FIX TLAB-254
                }
            }

            GdkWindow.GetSize (out wPanel, out hPanel);
            if (componentInfoExpander.Expanded) {
                GdkWindow.Resize (wPanel, hPanel + hInfo + hDesc);
               
            } else {
                if (configurationExpander.Visible && !configurationExpander.Expanded && IOExpander.Visible && !IOExpander.Expanded)
                    GdkWindow.Resize (600, 100);
                else
                    GdkWindow.Resize(wPanel,hPanel - hInfo -  hDesc);
            }
            //throw new NotImplementedException ();
        }
        // END HERZUM SPRINT 5.1: TLAB-225
        // END HERZUM SPRINT 4.2: TLAB-225

    }
}


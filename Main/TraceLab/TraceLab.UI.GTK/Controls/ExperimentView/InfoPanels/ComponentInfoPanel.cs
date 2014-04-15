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

        private void SetInputInfo(bool isEditable) 
        {
            if(m_metadata.IOSpec.Input.Count > 0)
            {
                //set columns in input and output views
                Gtk.NodeStore inputStore = new Gtk.NodeStore(typeof(IOItemNode));

                Gtk.CellRendererText textRenderer = new Gtk.CellRendererText();

                //inputs
                this.inputView.AppendColumn("Input", textRenderer, "text", 0);
                this.inputView.AppendColumn(CreateInputMappingColumnWithComboBox(inputStore, "Mapped to"));
                this.inputView.AppendColumn("Type", textRenderer, "text", 2);

                //prepare input and output store
                foreach(IOItem item in m_metadata.IOSpec.Input.Values) 
                {
                    inputStore.AddNode(new IOItemNode(item));
                }

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
            }
        }

        private void RefreshIOHighlightInExperiment(string mapping)
        {
            ExperimentHelper.ClearHighlightIOInExperiment(m_experimentOwner);
            ExperimentHelper.HighlightIOInExperiment(m_experimentOwner, mapping);
        }

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
                
                this.outputView.AppendColumn("Output", textRenderer, "text", 0);
                this.outputView.AppendColumn("Output as", editableTextRenderer, "text", 1);
                this.outputView.AppendColumn("Type", textRenderer, "text", 2);

                foreach(IOItem item in m_metadata.IOSpec.Output.Values) 
                {
                    outputStore.AddNode(new IOItemNode(item));
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
                InputMappings availableInputMappingsPerNode = new InputMappings (currentNode.Owner);
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
    }
}


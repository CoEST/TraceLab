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
using System.Collections.Generic;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DecisionInfoPanel : Gtk.EventBox
    {
        public DecisionInfoPanel()
        {
            this.Build();
            //set EventBox VisibleWindow to invisible so it is only used to trap events
            this.VisibleWindow = false;
            this.AboveChild = false;
            this.errorTextView.InitColorTags();
        }

        public DecisionInfoPanel(ApplicationContext applicationContext) : this()
        {
            m_applicationContext = applicationContext;
        }

        private ApplicationContext m_applicationContext;
        private DecisionMetadata m_metadata;
        private DecisionNodeControl m_decisionControl;

        public DecisionNodeControl DecisionControl 
        {
            get { return m_decisionControl; }
            set 
            {
                if(m_decisionControl != value) 
                {
                    m_decisionControl = value;
                    m_metadata = m_decisionControl.ExperimentNode.Data.Metadata as DecisionMetadata;

                    bool isEditable = m_decisionControl.IsEditable;

                    SetDecisionData(isEditable);

                    RefreshError();

                    m_decisionControl.ExperimentNode.ErrorChanged += HandleErrorChanged;
                }
            }
        }

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
            if (m_decisionControl.ExperimentNode.HasError) {
                this.errorTextView.ShowError(m_decisionControl.ExperimentNode.ErrorMessage);
            }
            else {
                this.errorTextView.Visible = false;
            }
        }

        private void SetDecisionData(bool isEditable)
        {
            this.componentLabelValue.Text = m_metadata.Label;
            this.componentLabelValue.IsEditable = isEditable;
            this.componentLabelValue.Sensitive = isEditable;

            this.decisionCodeEntryBox.Buffer.Text = m_metadata.DecisionCode;
            this.decisionCodeEntryBox.Editable = isEditable;
            this.decisionCodeEntryBox.Sensitive = isEditable;

            this.decisionCodeEntryBox.Buffer.Changed += (object sender, EventArgs e) => 
            {
                m_metadata.DecisionCode = this.decisionCodeEntryBox.Buffer.Text;
            };
        }

        /// <summary>
        /// Handles the event of Check Code Button Click.
        /// It compiles decision code.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void CheckCode (object sender, EventArgs e)
        {
            List<string> workspaceTypeDirectories = m_applicationContext.Application.WorkspaceViewModel.WorkspaceTypeDirectories;
            Experiment experiment = m_applicationContext.Application.Experiment;
            LoggerNameRoot loggerNameRoot = new LoggerNameRoot(experiment.ExperimentInfo.Id);

            //compile decision - the result is set in metadata Compilation Status, and eventual message in Error
            TraceLab.Core.Decisions.DecisionCompilationRunner.CompileDecision(m_decisionControl.ExperimentNode, 
                                                                              experiment, 
                                                                              workspaceTypeDirectories, 
                                                                              loggerNameRoot);

            //retrieve compilation status from metadata
            if(m_metadata.CompilationStatus == CompilationStatus.Successful) 
            {
                this.errorTextView.ShowInfoMessage("Compilation successful!");
            } 
            //otherwise error displays on ErrorChanged event - see HandleErrorChanged method
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
        protected void labelChanged (object sender, EventArgs e)
        {
            m_metadata.Label = this.componentLabelValue.Text;
        }
    }
}


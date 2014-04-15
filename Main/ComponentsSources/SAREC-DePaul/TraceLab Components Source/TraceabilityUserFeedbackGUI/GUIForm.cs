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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TraceLabSDK.Types;
using System.Threading.Tasks;

namespace TraceabilityUserFeedbackGUI
{
    public partial class GUIForm : Form
    {
        private TLArtifactsCollection originalSourceArtifacts;
        private TLArtifactsCollection originalTargetAfacts;
        private SimilarityMatrixUserFeedback extendedSimilarityMatrix;
        private Config config;

        public SimilarityMatrixUserFeedback ExtendedSimilarityMatrix
        {
            get{ return extendedSimilarityMatrix;}
        }

        public GUIForm(TLArtifactsCollection orgSourceArt, TLArtifactsCollection orgTargetArt, SimilarityMatrixUserFeedback in_extendedSimilarityMatrix, Config config_in)
        {
            InitializeComponent();

            originalSourceArtifacts = orgSourceArt;
            originalTargetAfacts = orgTargetArt;
            extendedSimilarityMatrix = in_extendedSimilarityMatrix;
            config = config_in;

            if (config.OutSavePath != null) // do we have the path set
            {
                cbx_saveWork.Checked = true;
                cbx_saveWork.Enabled = true;
                cbx_saveWork.Text = "Save my work to file (" + config.OutSavePath + ")";
            }
            else
            {
                cbx_saveWork.Checked = false;
                cbx_saveWork.Enabled = false;
                cbx_saveWork.Text = "Save my work to file (Incorrect configuration property)";
            }

            // load the source artifacts to gui:
            lsv_originalSourceArtifacts.BeginUpdate();

            ListViewItem[] linksList = new ListViewItem[originalSourceArtifacts.Count];

            int i = 0;
            foreach (KeyValuePair<string, TLArtifact> kvp in originalSourceArtifacts)
            {
                linksList[i++] = new ListViewItem(kvp.Key); 
            }

            lsv_originalSourceArtifacts.Items.AddRange(linksList);
            lsv_originalSourceArtifacts.EndUpdate();
            

            if (lsv_originalSourceArtifacts.Items.Count > 0)
                lsv_originalSourceArtifacts.Items[0].Selected = true;

            //set the sorter for the target artifacts:
            lsv_originalTargetArtifacts.ListViewItemSorter = new WeightsSorter();
        
        }

        private void lsv_originalSourceArtifacts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
            if (lsv_originalSourceArtifacts.SelectedItems.Count > 0)
            {
                string selectedSourceId = e.Item.Text;

                  rtb_sourceArtifactsDescrpition.Text = originalSourceArtifacts[selectedSourceId].Text;

             //load the target artifacts to gui:
                
                lsv_originalTargetArtifacts.Items.Clear();

                    TLLinksList linksList =  extendedSimilarityMatrix.GetLinksAboveThresholdForSourceArtifact(selectedSourceId);
                
                    lsv_originalTargetArtifacts.BeginUpdate();
                    
                    ListViewItem[] items = new ListViewItem[linksList.Count];
                    for (int i = 0; i < linksList.Count; i++)
                    {
                        items[i] = new ListViewItem(linksList[i].TargetArtifactId);
                        items[i].SubItems.Add(linksList[i].Score.ToString("F5"));
                    
                    }
                    lsv_originalTargetArtifacts.Items.AddRange(items);

                lsv_originalTargetArtifacts.EndUpdate();

                if (lsv_originalTargetArtifacts.Items.Count > 0)
                    lsv_originalTargetArtifacts.Items[0].Selected = true;



                // get made decision on satisfaction state for the sourceArtifact and display it on the radios:
                SimilarityMatrixUserFeedback.sourceSatisfactionState setAnswer = extendedSimilarityMatrix.getSourceSatisfactionDecision(selectedSourceId);

                switch (setAnswer)
                {
                    case SimilarityMatrixUserFeedback.sourceSatisfactionState.notSatisfied:
                        rdb_satisfactionUnsatisfied.Checked = true;
                        break;
                    case SimilarityMatrixUserFeedback.sourceSatisfactionState.satisfied:
                        rdb_satisfactionSatisfied.Checked = true;
                        break;
                    case SimilarityMatrixUserFeedback.sourceSatisfactionState.undecided:
                        rdb_satisfactionUndecided.Checked = true;
                        break;
                    case SimilarityMatrixUserFeedback.sourceSatisfactionState.notSet:
                        rdb_satisfactionUnsatisfied.Checked = rdb_satisfactionSatisfied.Checked = rdb_satisfactionUndecided.Checked = false;
                        break;
                }
            }
        }

        private void lsv_originalTargetArtifacts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lsv_originalTargetArtifacts.SelectedItems.Count > 0)
            {
                string selectedSourceId = lsv_originalSourceArtifacts.SelectedItems[0].Text;
                string selectedTargetId = lsv_originalTargetArtifacts.SelectedItems[0].Text;

                rtb_targetArtifactsDescrpition.Text = originalTargetAfacts[selectedTargetId].Text;


                 //get the decision previously made
                SimilarityMatrixUserFeedback.linkStates setAnser = extendedSimilarityMatrix.getTargetLinkDecision(selectedSourceId, selectedTargetId);
                switch (setAnser)
                {
                    case SimilarityMatrixUserFeedback.linkStates.link:
                        rdb_link.Checked = true;
                        break;

                    case SimilarityMatrixUserFeedback.linkStates.notLink:
                        rdb_notALink.Checked = true;
                        break;

                    case SimilarityMatrixUserFeedback.linkStates.undecided:
                        rdb_undecided.Checked =true;
                        break;
                        
                    case SimilarityMatrixUserFeedback.linkStates.notSet:
                        rdb_link.Checked = rdb_notALink.Checked = rdb_undecided.Checked = false;
                        break;
                }
            }
            else
            {
                rtb_targetArtifactsDescrpition.Text = "";
            }

           
        }

        private void rdb_satisfactionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (lsv_originalSourceArtifacts.SelectedItems.Count <= 0)
                return;

            //ignor the sender and check wchich radio is currently selected:
            string selectedSourceId = lsv_originalSourceArtifacts.SelectedItems[0].Text;

            if (rdb_satisfactionSatisfied.Checked)
            {
                extendedSimilarityMatrix.setSourceSatisfactionDecision(selectedSourceId, SimilarityMatrixUserFeedback.sourceSatisfactionState.satisfied);
            }
            else if (rdb_satisfactionUndecided.Checked)
            {
                extendedSimilarityMatrix.setSourceSatisfactionDecision(selectedSourceId, SimilarityMatrixUserFeedback.sourceSatisfactionState.undecided);
            }
            else if (rdb_satisfactionUnsatisfied.Checked)
            {
                extendedSimilarityMatrix.setSourceSatisfactionDecision(selectedSourceId, SimilarityMatrixUserFeedback.sourceSatisfactionState.notSatisfied);
            }
            else
            {
                extendedSimilarityMatrix.setSourceSatisfactionDecision(selectedSourceId, SimilarityMatrixUserFeedback.sourceSatisfactionState.notSet);
            }
        }

        private void rdb_targetLinkCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (lsv_originalSourceArtifacts.SelectedItems.Count <= 0 || lsv_originalTargetArtifacts.SelectedItems.Count <=0)
                return;

            string selectedSourceId = lsv_originalSourceArtifacts.SelectedItems[0].Text;
            string selectedTargetId = lsv_originalTargetArtifacts.SelectedItems[0].Text;

            if (rdb_undecided.Checked)
            {
                extendedSimilarityMatrix.setTargetLinkDecision(selectedSourceId, selectedTargetId, SimilarityMatrixUserFeedback.linkStates.undecided);
            }
            else if (rdb_link.Checked)
            {
                extendedSimilarityMatrix.setTargetLinkDecision(selectedSourceId, selectedTargetId, SimilarityMatrixUserFeedback.linkStates.link);
            }
            else if (rdb_notALink.Checked)
            {
                extendedSimilarityMatrix.setTargetLinkDecision(selectedSourceId, selectedTargetId, SimilarityMatrixUserFeedback.linkStates.notLink);
            }
            else
            {
                extendedSimilarityMatrix.setTargetLinkDecision(selectedSourceId, selectedTargetId, SimilarityMatrixUserFeedback.linkStates.notSet);
            }
        }

        private void selectNextSource_handler(object sender, EventArgs e)
        {
            if (lsv_originalSourceArtifacts.SelectedItems.Count <= 0)
            {
                lsv_originalSourceArtifacts.Items[0].Selected = true;
                return;
            }

            int currentSelecetedIndex = lsv_originalSourceArtifacts.SelectedItems[0].Index;

            if (lsv_originalSourceArtifacts.Items.Count - 1 > currentSelecetedIndex)
            {
                lsv_originalSourceArtifacts.Items[currentSelecetedIndex + 1].Selected = true;
            }
        }    

        private void selectNextTarget_handler(object sender, EventArgs e)
        {
            if (lsv_originalTargetArtifacts.SelectedItems.Count <= 0)
            {
                lsv_originalTargetArtifacts.Items[0].Selected = true;
            }

            int currentSelectedIndex = lsv_originalTargetArtifacts.SelectedItems[0].Index;

            if (lsv_originalTargetArtifacts.Items.Count - 1 > currentSelectedIndex)
            {
                lsv_originalTargetArtifacts.Items[currentSelectedIndex + 1].Selected = true;
            }
        }

        private void GUIForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) 
            {
                case Keys.T:
                    selectNextTarget_handler(sender, null);
                    break;
                case Keys.S:
                    selectNextSource_handler(sender, null);
                    break;
            }
        }

        // disable the keyboard for 
        private void lsv_artifacts_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void btn_finish_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

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

namespace TraceabilityUserFeedbackGUI
{
    partial class GUIForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lsv_originalSourceArtifacts = new System.Windows.Forms.ListView();
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtb_sourceArtifactsDescrpition = new System.Windows.Forms.RichTextBox();
            this.rtb_targetArtifactsDescrpition = new System.Windows.Forms.RichTextBox();
            this.lsv_originalTargetArtifacts = new System.Windows.Forms.ListView();
            this.targets_Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.targets_weight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.rdb_undecided = new System.Windows.Forms.RadioButton();
            this.rdb_link = new System.Windows.Forms.RadioButton();
            this.rdb_notALink = new System.Windows.Forms.RadioButton();
            this.btn_nextTarget = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdb_satisfactionUndecided = new System.Windows.Forms.RadioButton();
            this.rdb_satisfactionSatisfied = new System.Windows.Forms.RadioButton();
            this.btn_nextSource = new System.Windows.Forms.Button();
            this.rdb_satisfactionUnsatisfied = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbx_saveWork = new System.Windows.Forms.CheckBox();
            this.btn_finish = new System.Windows.Forms.Button();
            this.btn_reports = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Artifacts:";
            // 
            // lsv_originalSourceArtifacts
            // 
            this.lsv_originalSourceArtifacts.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.lsv_originalSourceArtifacts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lsv_originalSourceArtifacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id});
            this.lsv_originalSourceArtifacts.FullRowSelect = true;
            this.lsv_originalSourceArtifacts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsv_originalSourceArtifacts.HideSelection = false;
            this.lsv_originalSourceArtifacts.Location = new System.Drawing.Point(20, 35);
            this.lsv_originalSourceArtifacts.MultiSelect = false;
            this.lsv_originalSourceArtifacts.Name = "lsv_originalSourceArtifacts";
            this.lsv_originalSourceArtifacts.Size = new System.Drawing.Size(189, 170);
            this.lsv_originalSourceArtifacts.TabIndex = 4;
            this.lsv_originalSourceArtifacts.UseCompatibleStateImageBehavior = false;
            this.lsv_originalSourceArtifacts.View = System.Windows.Forms.View.Details;
            this.lsv_originalSourceArtifacts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsv_originalSourceArtifacts_ItemSelectionChanged);
            this.lsv_originalSourceArtifacts.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.lsv_artifacts_PreviewKeyDown);
            // 
            // Id
            // 
            this.Id.Text = "Id";
            this.Id.Width = 152;
            // 
            // rtb_sourceArtifactsDescrpition
            // 
            this.rtb_sourceArtifactsDescrpition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_sourceArtifactsDescrpition.Location = new System.Drawing.Point(243, 35);
            this.rtb_sourceArtifactsDescrpition.Name = "rtb_sourceArtifactsDescrpition";
            this.rtb_sourceArtifactsDescrpition.Size = new System.Drawing.Size(636, 170);
            this.rtb_sourceArtifactsDescrpition.TabIndex = 5;
            this.rtb_sourceArtifactsDescrpition.Text = "";
            // 
            // rtb_targetArtifactsDescrpition
            // 
            this.rtb_targetArtifactsDescrpition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_targetArtifactsDescrpition.Location = new System.Drawing.Point(242, 35);
            this.rtb_targetArtifactsDescrpition.Name = "rtb_targetArtifactsDescrpition";
            this.rtb_targetArtifactsDescrpition.Size = new System.Drawing.Size(633, 152);
            this.rtb_targetArtifactsDescrpition.TabIndex = 8;
            this.rtb_targetArtifactsDescrpition.Text = "";
            // 
            // lsv_originalTargetArtifacts
            // 
            this.lsv_originalTargetArtifacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.targets_Id,
            this.targets_weight});
            this.lsv_originalTargetArtifacts.FullRowSelect = true;
            this.lsv_originalTargetArtifacts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsv_originalTargetArtifacts.HideSelection = false;
            this.lsv_originalTargetArtifacts.Location = new System.Drawing.Point(20, 35);
            this.lsv_originalTargetArtifacts.MultiSelect = false;
            this.lsv_originalTargetArtifacts.Name = "lsv_originalTargetArtifacts";
            this.lsv_originalTargetArtifacts.Size = new System.Drawing.Size(189, 152);
            this.lsv_originalTargetArtifacts.TabIndex = 7;
            this.lsv_originalTargetArtifacts.UseCompatibleStateImageBehavior = false;
            this.lsv_originalTargetArtifacts.View = System.Windows.Forms.View.Details;
            this.lsv_originalTargetArtifacts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsv_originalTargetArtifacts_ItemSelectionChanged);
            // 
            // targets_Id
            // 
            this.targets_Id.Text = "Id";
            this.targets_Id.Width = 95;
            // 
            // targets_weight
            // 
            this.targets_weight.Tag = "numeric";
            this.targets_weight.Text = "Weight";
            this.targets_weight.Width = 70;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Target Artifacts:";
            // 
            // rdb_undecided
            // 
            this.rdb_undecided.AutoSize = true;
            this.rdb_undecided.Location = new System.Drawing.Point(24, 23);
            this.rdb_undecided.Name = "rdb_undecided";
            this.rdb_undecided.Size = new System.Drawing.Size(85, 20);
            this.rdb_undecided.TabIndex = 9;
            this.rdb_undecided.TabStop = true;
            this.rdb_undecided.Text = "Undecided";
            this.rdb_undecided.UseVisualStyleBackColor = true;
            this.rdb_undecided.CheckedChanged += new System.EventHandler(this.rdb_targetLinkCheckbox_CheckedChanged);
            // 
            // rdb_link
            // 
            this.rdb_link.AutoSize = true;
            this.rdb_link.Location = new System.Drawing.Point(128, 23);
            this.rdb_link.Name = "rdb_link";
            this.rdb_link.Size = new System.Drawing.Size(48, 20);
            this.rdb_link.TabIndex = 10;
            this.rdb_link.TabStop = true;
            this.rdb_link.Text = "Link";
            this.rdb_link.UseVisualStyleBackColor = true;
            this.rdb_link.CheckedChanged += new System.EventHandler(this.rdb_targetLinkCheckbox_CheckedChanged);
            // 
            // rdb_notALink
            // 
            this.rdb_notALink.AutoSize = true;
            this.rdb_notALink.Location = new System.Drawing.Point(207, 23);
            this.rdb_notALink.Name = "rdb_notALink";
            this.rdb_notALink.Size = new System.Drawing.Size(79, 20);
            this.rdb_notALink.TabIndex = 11;
            this.rdb_notALink.TabStop = true;
            this.rdb_notALink.Text = "Not a link";
            this.rdb_notALink.UseVisualStyleBackColor = true;
            this.rdb_notALink.CheckedChanged += new System.EventHandler(this.rdb_targetLinkCheckbox_CheckedChanged);
            // 
            // btn_nextTarget
            // 
            this.btn_nextTarget.Location = new System.Drawing.Point(385, 18);
            this.btn_nextTarget.Name = "btn_nextTarget";
            this.btn_nextTarget.Size = new System.Drawing.Size(112, 25);
            this.btn_nextTarget.TabIndex = 12;
            this.btn_nextTarget.Text = "Next Target (t)";
            this.btn_nextTarget.UseVisualStyleBackColor = true;
            this.btn_nextTarget.Click += new System.EventHandler(this.selectNextTarget_handler);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.rtb_sourceArtifactsDescrpition);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lsv_originalSourceArtifacts);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 268);
            this.panel1.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdb_satisfactionUndecided);
            this.groupBox2.Controls.Add(this.rdb_satisfactionSatisfied);
            this.groupBox2.Controls.Add(this.btn_nextSource);
            this.groupBox2.Controls.Add(this.rdb_satisfactionUnsatisfied);
            this.groupBox2.Location = new System.Drawing.Point(376, 207);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 53);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Source Satisfaction";
            // 
            // rdb_satisfactionUndecided
            // 
            this.rdb_satisfactionUndecided.AutoSize = true;
            this.rdb_satisfactionUndecided.Location = new System.Drawing.Point(24, 23);
            this.rdb_satisfactionUndecided.Name = "rdb_satisfactionUndecided";
            this.rdb_satisfactionUndecided.Size = new System.Drawing.Size(85, 20);
            this.rdb_satisfactionUndecided.TabIndex = 9;
            this.rdb_satisfactionUndecided.TabStop = true;
            this.rdb_satisfactionUndecided.Text = "Undecided";
            this.rdb_satisfactionUndecided.UseVisualStyleBackColor = true;
            this.rdb_satisfactionUndecided.CheckedChanged += new System.EventHandler(this.rdb_satisfactionCheckbox_CheckedChanged);
            // 
            // rdb_satisfactionSatisfied
            // 
            this.rdb_satisfactionSatisfied.AutoSize = true;
            this.rdb_satisfactionSatisfied.Location = new System.Drawing.Point(128, 23);
            this.rdb_satisfactionSatisfied.Name = "rdb_satisfactionSatisfied";
            this.rdb_satisfactionSatisfied.Size = new System.Drawing.Size(75, 20);
            this.rdb_satisfactionSatisfied.TabIndex = 10;
            this.rdb_satisfactionSatisfied.TabStop = true;
            this.rdb_satisfactionSatisfied.Text = "Satisfied";
            this.rdb_satisfactionSatisfied.UseVisualStyleBackColor = true;
            this.rdb_satisfactionSatisfied.CheckedChanged += new System.EventHandler(this.rdb_satisfactionCheckbox_CheckedChanged);
            // 
            // btn_nextSource
            // 
            this.btn_nextSource.Location = new System.Drawing.Point(385, 18);
            this.btn_nextSource.Name = "btn_nextSource";
            this.btn_nextSource.Size = new System.Drawing.Size(112, 25);
            this.btn_nextSource.TabIndex = 12;
            this.btn_nextSource.Text = "Next Source (s)";
            this.btn_nextSource.UseVisualStyleBackColor = true;
            this.btn_nextSource.Click += new System.EventHandler(this.selectNextSource_handler);
            // 
            // rdb_satisfactionUnsatisfied
            // 
            this.rdb_satisfactionUnsatisfied.AutoSize = true;
            this.rdb_satisfactionUnsatisfied.Location = new System.Drawing.Point(207, 23);
            this.rdb_satisfactionUnsatisfied.Name = "rdb_satisfactionUnsatisfied";
            this.rdb_satisfactionUnsatisfied.Size = new System.Drawing.Size(98, 20);
            this.rdb_satisfactionUnsatisfied.TabIndex = 11;
            this.rdb_satisfactionUnsatisfied.TabStop = true;
            this.rdb_satisfactionUnsatisfied.Text = "Not Satisfied";
            this.rdb_satisfactionUnsatisfied.UseVisualStyleBackColor = true;
            this.rdb_satisfactionUnsatisfied.CheckedChanged += new System.EventHandler(this.rdb_satisfactionCheckbox_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.rtb_targetArtifactsDescrpition);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lsv_originalTargetArtifacts);
            this.panel2.Location = new System.Drawing.Point(12, 280);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(894, 249);
            this.panel2.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rdb_undecided);
            this.groupBox1.Controls.Add(this.rdb_link);
            this.groupBox1.Controls.Add(this.btn_nextTarget);
            this.groupBox1.Controls.Add(this.rdb_notALink);
            this.groupBox1.Location = new System.Drawing.Point(372, 193);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 53);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target link";
            // 
            // cbx_saveWork
            // 
            this.cbx_saveWork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbx_saveWork.AutoSize = true;
            this.cbx_saveWork.Location = new System.Drawing.Point(32, 548);
            this.cbx_saveWork.Name = "cbx_saveWork";
            this.cbx_saveWork.Size = new System.Drawing.Size(144, 20);
            this.cbx_saveWork.TabIndex = 15;
            this.cbx_saveWork.Text = "Save my work to file";
            this.cbx_saveWork.UseVisualStyleBackColor = true;
            // 
            // btn_finish
            // 
            this.btn_finish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_finish.Location = new System.Drawing.Point(803, 547);
            this.btn_finish.Name = "btn_finish";
            this.btn_finish.Size = new System.Drawing.Size(75, 23);
            this.btn_finish.TabIndex = 16;
            this.btn_finish.Text = "Finished";
            this.btn_finish.UseVisualStyleBackColor = true;
            this.btn_finish.Click += new System.EventHandler(this.btn_finish_Click);
            // 
            // btn_reports
            // 
            this.btn_reports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_reports.Location = new System.Drawing.Point(721, 547);
            this.btn_reports.Name = "btn_reports";
            this.btn_reports.Size = new System.Drawing.Size(75, 23);
            this.btn_reports.TabIndex = 17;
            this.btn_reports.Text = "Reports";
            this.btn_reports.UseVisualStyleBackColor = true;
            // 
            // GUIForm
            // 
            this.ClientSize = new System.Drawing.Size(925, 584);
            this.Controls.Add(this.btn_reports);
            this.Controls.Add(this.btn_finish);
            this.Controls.Add(this.cbx_saveWork);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "GUIForm";
            this.Text = "User Feedback";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GUIForm_KeyUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lsv_originalSourceArtifacts;
        private System.Windows.Forms.ColumnHeader Id;
        private System.Windows.Forms.RichTextBox rtb_sourceArtifactsDescrpition;
        private System.Windows.Forms.RichTextBox rtb_targetArtifactsDescrpition;
        private System.Windows.Forms.ListView lsv_originalTargetArtifacts;
        private System.Windows.Forms.ColumnHeader targets_Id;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader targets_weight;
        private System.Windows.Forms.RadioButton rdb_undecided;
        private System.Windows.Forms.RadioButton rdb_link;
        private System.Windows.Forms.RadioButton rdb_notALink;
        private System.Windows.Forms.Button btn_nextTarget;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdb_satisfactionUndecided;
        private System.Windows.Forms.RadioButton rdb_satisfactionSatisfied;
        private System.Windows.Forms.Button btn_nextSource;
        private System.Windows.Forms.RadioButton rdb_satisfactionUnsatisfied;
        private System.Windows.Forms.CheckBox cbx_saveWork;
        private System.Windows.Forms.Button btn_finish;
        private System.Windows.Forms.Button btn_reports;
    }
}
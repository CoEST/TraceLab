namespace TraceLabSDK.Types
{
    partial class TLSimilarityMatrixEditor
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
            this.similaritiesGrid = new System.Windows.Forms.DataGridView();
            this.sourceArtifactIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetArtifactIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scoreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.similaritiesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // similaritiesGrid
            // 
            this.similaritiesGrid.AllowUserToAddRows = false;
            this.similaritiesGrid.AllowUserToDeleteRows = false;
            this.similaritiesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.similaritiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.similaritiesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sourceArtifactIdColumn,
            this.targetArtifactIdColumn,
            this.scoreColumn});
            this.similaritiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.similaritiesGrid.Location = new System.Drawing.Point(0, 0);
            this.similaritiesGrid.Name = "similaritiesGrid";
            this.similaritiesGrid.ReadOnly = true;
            this.similaritiesGrid.Size = new System.Drawing.Size(422, 254);
            this.similaritiesGrid.TabIndex = 0;
            // 
            // sourceArtifactIdColumn
            // 
            this.sourceArtifactIdColumn.HeaderText = "Source Artifact Id";
            this.sourceArtifactIdColumn.Name = "sourceArtifactIdColumn";
            this.sourceArtifactIdColumn.ReadOnly = true;
            // 
            // targetArtifactIdColumn
            // 
            this.targetArtifactIdColumn.HeaderText = "Target Artifact Id";
            this.targetArtifactIdColumn.Name = "targetArtifactIdColumn";
            this.targetArtifactIdColumn.ReadOnly = true;
            // 
            // scoreColumn
            // 
            this.scoreColumn.HeaderText = "Score";
            this.scoreColumn.Name = "scoreColumn";
            this.scoreColumn.ReadOnly = true;
            // 
            // TLSimilarityMatrixEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 254);
            this.Controls.Add(this.similaritiesGrid);
            this.Name = "TLSimilarityMatrixEditor";
            this.Text = "TLSimilariyMatrixEditor";
            ((System.ComponentModel.ISupportInitialize)(this.similaritiesGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView similaritiesGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceArtifactIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetArtifactIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scoreColumn;
    }
}
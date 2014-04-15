namespace SEMERU.UI.BoxPlotGUI.BoxPlotUserControl
{
    partial class SummaryDataMetricBoxPlot
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.pvalueTextBox = new System.Windows.Forms.TextBox();
            this.pvaluelabel = new System.Windows.Forms.Label();
            this.testLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(309, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.Size = new System.Drawing.Size(226, 198);
            this.dataGridView1.TabIndex = 5;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.Location = new System.Drawing.Point(3, 12);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(300, 300);
            this.zedGraphControl1.TabIndex = 4;
            // 
            // pvalueTextBox
            // 
            this.pvalueTextBox.Location = new System.Drawing.Point(374, 275);
            this.pvalueTextBox.Name = "pvalueTextBox";
            this.pvalueTextBox.ReadOnly = true;
            this.pvalueTextBox.Size = new System.Drawing.Size(100, 20);
            this.pvalueTextBox.TabIndex = 1;
            // 
            // pvaluelabel
            // 
            this.pvaluelabel.AutoSize = true;
            this.pvaluelabel.Location = new System.Drawing.Point(323, 278);
            this.pvaluelabel.Name = "pvaluelabel";
            this.pvaluelabel.Size = new System.Drawing.Size(45, 13);
            this.pvaluelabel.TabIndex = 0;
            this.pvaluelabel.Text = "p-value:";
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testLabel.Location = new System.Drawing.Point(323, 229);
            this.testLabel.MaximumSize = new System.Drawing.Size(200, 0);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(66, 15);
            this.testLabel.TabIndex = 6;
            this.testLabel.Text = "testLabel";
            this.testLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SummaryDataMetricBoxPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.testLabel);
            this.Controls.Add(this.pvaluelabel);
            this.Controls.Add(this.pvalueTextBox);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "SummaryDataMetricBoxPlot";
            this.Size = new System.Drawing.Size(549, 333);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Label pvaluelabel;
        private System.Windows.Forms.TextBox pvalueTextBox;
        private System.Windows.Forms.Label testLabel;
    }
}

// // TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// // Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
// //
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// //
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see<http://www.gnu.org/licenses/>.
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TraceLabSDK.Types
{
    public class TLArtifactsCollectionEditor : Form, TraceLabSDK.IWorkspaceUnitEditor
    {
        public TLArtifactsCollectionEditor() 
        {
            InitializeComponent();
        }

        private DataGridView artifactsGrid;
        private DataGridViewTextBoxColumn idColumn;
        private DataGridViewTextBoxColumn textColumn;

        #region IWorkspaceUnitEditor implementation

        private object data;
        public object Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                DisplayData();
            }
        }

        #endregion

        private void DisplayData()
        {
            var artifacts = (TLArtifactsCollection)Data;
            List<DataGridViewRow> rows = new List<DataGridViewRow>(artifacts.Count);
            foreach (TLArtifact art in artifacts.Values)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(artifactsGrid, art.Id, art.Text);
                rows.Add(row);
            }

            artifactsGrid.Rows.AddRange(rows.ToArray());
        }

        private void InitializeComponent()
        {
            this.artifactsGrid = new System.Windows.Forms.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.artifactsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // artifactsGrid
            // 
            this.artifactsGrid.AllowUserToAddRows = false;
            this.artifactsGrid.AllowUserToDeleteRows = false;
            this.artifactsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.artifactsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.artifactsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.textColumn});
            this.artifactsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.artifactsGrid.Location = new System.Drawing.Point(0, 0);
            this.artifactsGrid.Name = "artifactsGrid";
            this.artifactsGrid.ReadOnly = true;
            this.artifactsGrid.Size = new System.Drawing.Size(284, 262);
            this.artifactsGrid.TabIndex = 0;
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "Id";
            this.idColumn.HeaderText = "Id";
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            this.idColumn.Width = 41;
            // 
            // textColumn
            // 
            this.textColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.textColumn.DataPropertyName = "Text";
            this.textColumn.HeaderText = "Text";
            this.textColumn.Name = "textColumn";
            this.textColumn.ReadOnly = true;
            // 
            // TLArtifactsCollectionEditor
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.artifactsGrid);
            this.Name = "TLArtifactsCollectionEditor";
            ((System.ComponentModel.ISupportInitialize)(this.artifactsGrid)).EndInit();
            this.ResumeLayout(false);

        }
    }
}


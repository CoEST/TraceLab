using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TraceLabSDK.Types
{
    public partial class TLSimilarityMatrixEditor : Form, TraceLabSDK.IWorkspaceUnitEditor
    {
        public TLSimilarityMatrixEditor()
        {
            InitializeComponent();
        }

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
            var similarities = (TLSimilarityMatrix)Data;
            List<DataGridViewRow> rows = new List<DataGridViewRow>(similarities.Count);
            foreach (TLSingleLink link in similarities.AllLinks)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(similaritiesGrid, link.SourceArtifactId, link.TargetArtifactId, link.Score);
                rows.Add(row);
            }

            similaritiesGrid.Rows.AddRange(rows.ToArray());
        }
    }
}

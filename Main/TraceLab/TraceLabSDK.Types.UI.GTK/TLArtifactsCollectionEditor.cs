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
using Gtk;

namespace TraceLabSDK.Types
{
    public partial class TLArtifactsCollectionEditor : Gtk.Bin, TraceLabSDK.IWorkspaceUnitEditor
    {
        public TLArtifactsCollectionEditor ()
        {
            this.Build ();

            m_store = new Gtk.ListStore(typeof(string), typeof(string));
            treeView.Model = m_store;

            //create columns with associated cell renderings
            CellRendererText idCellRenderer = new CellRendererText();
            TreeViewColumn idColumn = treeView.AppendColumn("ID", idCellRenderer);
            idColumn.AddAttribute (idCellRenderer, "text", 0);


            CellRendererText contentCellRenderer = new CellRendererText();
            TreeViewColumn contentColumn = treeView.AppendColumn("Text", contentCellRenderer);
            contentColumn.AddAttribute (contentCellRenderer, "text", 1);
        }

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

        private void DisplayData()
        {
            var artifacts = (TLArtifactsCollection)Data;
            foreach(TLArtifact artifact in artifacts.Values)
            {
                m_store.AppendValues(artifact.Id, artifact.Text);
            }
        }

        private Gtk.ListStore m_store;
    }
}


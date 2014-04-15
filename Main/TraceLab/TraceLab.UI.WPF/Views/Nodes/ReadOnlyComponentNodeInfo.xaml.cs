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

using System.ComponentModel;
using System.Windows;

namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Interaction logic for ReadOnlyComponentNodeInfo.xaml
    /// </summary>
    public partial class ReadOnlyComponentNodeInfo : BaseComponentNodeInfoControl, INotifyPropertyChanged
    {
        public ReadOnlyComponentNodeInfo()
        {
            InitializeComponent();
        }

        private bool m_showFilterCheckboxes;
        public bool ShowFilterCheckboxes
        {
            get { return m_showFilterCheckboxes; }
            set
            {
                if (value != m_showFilterCheckboxes)
                {
                    m_showFilterCheckboxes = value;
                    NotifyPropertyChanged("ShowFilterCheckboxes");
                }
            }
        }

        #region Resizing

        private void resizeThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //delegate resizing to parents class (less duplicate code iwht ComponentNodeInfo and ReadOnlyComponentNodeInfo)
            resizeInfoPane(e, iospecExpander, iospecRow, outputsLV, inputsLV);    
        }

        private void iospecExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            iospecExpander_Collapsed(resizeThumb, iospecRow);
        }

        private void iospecExpander_Expanded(object sender, RoutedEventArgs e)
        {
            iospecExpander_Expanded(resizeThumb, outputsLV, inputsLV);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}

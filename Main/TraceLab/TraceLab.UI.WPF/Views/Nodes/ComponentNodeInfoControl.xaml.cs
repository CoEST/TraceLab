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

using System.Windows;
using System.Windows.Controls;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Interaction logic for ComponentNodeInfoControl.xaml
    /// </summary>
    public partial class ComponentNodeInfoControl : BaseComponentNodeInfoControl
    {
        public ComponentNodeInfoControl()
        {
            InitializeComponent();
        }
        
        #region Input ComboBoxItem Mouse handlers

        /// <summary>
        /// Keeps reference to last combo box that mouse was over. Used just to not do overprocessing on mouse move.
        /// </summary>
        private ComboBoxItem m_lastComboBoxItemCached;

        /// <summary>
        /// Handles the PreviewMouseMoveHandler event of the InputComboBoxIOItem control.
        /// MouseEnter could not have been used, as it seems to be handled by ComboBoxItem, and unfortunatelly this handler would never
        /// been invoked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void InputComboBoxIOItem_PreviewMouseMoveHandler(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBoxItem comboBoxItem = sender as System.Windows.Controls.ComboBoxItem;
            if (comboBoxItem != m_lastComboBoxItemCached)
            {
                m_lastComboBoxItemCached = comboBoxItem;
                IExperiment experiment;
                if (TryFindExperiment(comboBoxItem, out experiment))
                {
                    string mapping = (string)comboBoxItem.DataContext;
                    TraceLab.Core.Utilities.ExperimentHelper.HighlightIOInExperiment(experiment, mapping);
                }
            }
        }

        /// <summary>
        /// Handles the MouseLeaveHandler event of the InputComboBoxIOItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void InputComboBoxIOItem_MouseLeaveHandler(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBoxItem comboBoxItem = sender as System.Windows.Controls.ComboBoxItem;

            IExperiment experiment;
            if (TryFindExperiment(comboBoxItem, out experiment))
            {
                TraceLab.Core.Utilities.ExperimentHelper.ClearHighlightIOInExperiment(experiment);
            }
        }


        /// <summary>
        /// Tries the find experiment from the given combobox item.
        /// </summary>
        /// <param name="comboBoxItem">The combo box item.</param>
        /// <param name="experiment">The experiment.</param>
        /// <returns>true if experiment has been found</returns>
        private static bool TryFindExperiment(ComboBoxItem comboBoxItem, out IExperiment experiment)
        {
            bool found = false;
            experiment = null;

            if (comboBoxItem != null)
            {
                //find combobox
                var combobox = comboBoxItem.GetParent<ItemsPresenter>(null);
                if (combobox != null)
                {
                    if (TryFindExperiment(combobox.TemplatedParent, out experiment))
                    {
                        found = true;
                    }
                }
            }

            return found;
        }

        #endregion

        #region OutputAs Text Changed handler

        private void OutputAsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            IExperiment experiment;
            if (TryFindExperiment(textBox, out experiment))
            {
                TraceLab.Core.Utilities.ExperimentHelper.ClearHighlightIOInExperiment(experiment);
                TraceLab.Core.Utilities.ExperimentHelper.HighlightIOInExperiment(experiment, textBox.Text);
            }
        }

        #endregion

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
    }
}

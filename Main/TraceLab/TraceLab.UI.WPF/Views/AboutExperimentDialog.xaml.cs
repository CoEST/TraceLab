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
using System.Windows;
using System.Windows.Controls;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for AboutExperimentDialog
    /// </summary>
    public partial class AboutExperimentDialog : Window
    {
        /// <summary>
        /// Default constructor is protected so callers must use one with a parent.
        /// </summary>
        protected AboutExperimentDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor that takes a parent for this AboutExperimentDialog dialog.
        /// </summary>
        /// <param name="parent">Parent window for this dialog.</param>
        public AboutExperimentDialog(Window parent)
            : this()
        {
            this.Owner = parent;
        }

        /// <summary>
        /// Handles the Click event of the Update Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

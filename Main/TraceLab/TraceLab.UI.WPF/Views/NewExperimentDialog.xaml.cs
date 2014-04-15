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
using System.Windows.Input;
using System.IO;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for NewExperimentDialog.xaml
    /// </summary>
    public partial class NewExperimentDialog : Window
    {
        private static readonly string s_fileExtension = ".teml";
        private bool m_filenameBoxModified;

        protected NewExperimentDialog()
        {
            InitializeComponent();
        }

        public NewExperimentDialog(Window parent, string experimentDirectory)
            : this()
        {
            this.Owner = parent;
            DirectoryBox.Text = experimentDirectory;
            this.m_filenameBoxModified = false;
        }

        /// <summary>
        /// Validates the data entered in the TextBoxes. If all data is valid, then the CreateButton is enabled.
        /// </summary>
        private void ValidateTextBoxesData()
        {
            bool isDataValid = true;
            isDataValid = isDataValid && (String.IsNullOrWhiteSpace(ExperimentNameBox.Text) == false);
            isDataValid = isDataValid && Directory.Exists(DirectoryBox.Text);
            isDataValid = isDataValid && (String.IsNullOrWhiteSpace(FileNameBox.Text) == false);
            isDataValid = isDataValid && (FileNameBox.Text.Trim() != s_fileExtension);
            CreateButton.IsEnabled = isDataValid;
        }

        #region Event handlers

        /// <summary>
        /// Validates data entered in TextBoxes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.m_filenameBoxModified == false)
            {
                FileNameBox.Text = ExperimentNameBox.Text;
            }
            ValidateTextBoxesData();
        }

        /// <summary>
        /// Called when [browse click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnBrowseClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                var button = (Button)sender;
                ExperimentInfo info = button.DataContext as ExperimentInfo;
                DirectoryBox.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the FileNameBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void FileNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            this.m_filenameBoxModified = true;
        }

        /// <summary>
        /// Handles the Click event of the CreateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string filename = FileNameBox.Text.Trim();
            if (filename.EndsWith(s_fileExtension) == false)
            {
                filename += s_fileExtension;
            }

            ExperimentInfo info = CreateButton.DataContext as ExperimentInfo;
            info.FilePath = System.IO.Path.Combine(DirectoryBox.Text.Trim(), filename);

            if (File.Exists(info.FilePath))
            {
                string message = filename + " already exists.\nDo you want to replace the existing file?";
                if (MessageBox.Show(this, message, "File Replace Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)
                    == MessageBoxResult.Yes)
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                this.DialogResult = true;
            }
        }

        #endregion
    }
}

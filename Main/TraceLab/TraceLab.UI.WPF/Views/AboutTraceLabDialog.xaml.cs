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
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Content holder for the About TraceLab Dialog.
    /// </summary>
    class AboutTraceLabContent
    {
        public string VersionMSG { get; private set; }
        public string RegistrationMSG { get; private set; }

        public AboutTraceLabContent(string pVersion, string pRegisteredUser)
        {
            if (string.IsNullOrEmpty(pVersion) == false)
            {
                this.VersionMSG = "Version " + pVersion;
            }
            if (string.IsNullOrEmpty(pRegisteredUser) == false)
            {
                this.RegistrationMSG = "Registered to " + pRegisteredUser;
            }
        }
    }

    /// <summary>
    /// Interaction logic for AboutTraceLabDialog
    /// </summary>
    public partial class AboutTraceLabDialog : Window
    {
        /// <summary>
        /// Default constructor is protected so callers must use one with a parent.
        /// </summary>
        protected AboutTraceLabDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor that takes a parent for this AboutTraceLabDialog dialog.
        /// </summary>
        /// <param name="parent">Parent window for this dialog.</param>
        public AboutTraceLabDialog(Window parent)
            : this()
        {
            this.Owner = parent;
        }

        /// <summary>
        /// Handles the Click event of the Update Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void OpenHyperlinkRequest(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

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
using System.Windows.Controls;
using System.Diagnostics;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.Core.ViewModels;

namespace TraceLab.UI.WPF.Views
{
	/// <summary>
	/// Interaction logic for StartPage.xaml
	/// </summary>
	public partial class StartPage : UserControl
	{
		public StartPage()
		{ 
			this.InitializeComponent();
		}

        #region Event Handlers

        /// <summary>
        /// Opens default web browser with provided link.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ListBox_MouseLeftButtonDown_OpenLinkURL(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            if (item != null)
            {
                WebsiteLink link = (WebsiteLink)item.DataContext;
                Process.Start(new ProcessStartInfo(link.LinkURL));
                e.Handled = true;
            }
        }

        #endregion
    }
}

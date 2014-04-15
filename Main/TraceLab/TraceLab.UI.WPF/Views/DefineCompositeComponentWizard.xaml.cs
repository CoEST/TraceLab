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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for DefineCompositeComponentWizard.xaml
    /// </summary>
    public partial class DefineCompositeComponentWizard : Window
    {
        public DefineCompositeComponentWizard()
        {
            InitializeComponent();
        }

        public DefineCompositeComponentWizard(Window parent)
            : this()
        {
            Owner = parent;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.Focus();
        }

        #region Open Component Graph Command

        public static readonly DependencyProperty OpenComponentGraphCommandProperty = DependencyProperty.Register("OpenComponentGraphCommand", typeof(ICommand), typeof(DefineCompositeComponentWizard));

        public ICommand OpenComponentGraphCommand
        {
            get { return (ICommand)GetValue(OpenComponentGraphCommandProperty); }
            set { SetValue(OpenComponentGraphCommandProperty, value); }
        }

        private void ExecuteOpenComponentGraphCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (OpenComponentGraphCommand != null)
            {
                OpenComponentGraphCommand.Execute(e.Parameter);
            }
        }

        private void CanExecuteOpenComponentGraphCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (OpenComponentGraphCommand != null)
            {
                e.CanExecute = OpenComponentGraphCommand.CanExecute(e.Parameter);
            }
        }

        #endregion

        #region Execute Toggle Node Info

        public static readonly DependencyProperty ToggleInfoPaneForNodeProperty = DependencyProperty.Register("ToggleInfoPaneForNodeCommand", typeof(ICommand), typeof(DefineCompositeComponentWizard));

        public ICommand ToggleInfoPaneForNodeCommand
        {
            get { return (ICommand)GetValue(ToggleInfoPaneForNodeProperty); }
            set { SetValue(ToggleInfoPaneForNodeProperty, value); }
        }

        private void ExecuteToggleNodeInfo(object sender, ExecutedRoutedEventArgs e)
        {
            ToggleInfoPaneForNodeCommand.Execute(e.Parameter);
        }

        #endregion

        #region Mouse handlers of the IOItem of the list view control.

        /// <summary>
        /// Handles the MouseEnterHandler event of the IOItem of the list view control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IOItem_MouseEnterHandler(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ListViewItem item = sender as System.Windows.Controls.ListViewItem;
            if (item != null)
            {
                KeyValuePair<string, ItemSetting> itemSetting = (KeyValuePair<string, ItemSetting>)item.DataContext;
                itemSetting.Value.Highlight();
            }
        }

        /// <summary>
        /// Handles the MouseLeaveHandler event of the IOItem of the list view control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IOItem_MouseLeaveHandler(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ListViewItem item = sender as System.Windows.Controls.ListViewItem;
            if (item != null)
            {
                KeyValuePair<string, ItemSetting> itemSetting = (KeyValuePair<string, ItemSetting>)item.DataContext;
                itemSetting.Value.ClearHighlight();
            }
        }

        #endregion
    }
}

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
using System.Windows.Controls.Primitives;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.UI.WPF.EventArgs;

namespace TraceLab.UI.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AssistListPopup.xaml
    /// </summary>
    public partial class AssistListPopup : Popup
    {
        public AssistListPopup()
        {
            InitializeComponent();

            AssistListBox.MouseDoubleClick += new MouseButtonEventHandler(AssistListBox_MouseDoubleClick);
            //AssistListBox.PreviewKeyDown += new KeyEventHandler(AssistListBox_PreviewKeyDown);
        }

        protected override void OnOpened(System.EventArgs e)
        {
            AssistListBox.SelectedIndex = 0;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Focusing on internal list");
            AssistListBox.Focus();
        }

        public new event EventHandler Closed;

        /// <summary>
        /// Invokes the Closed event; called whenever popup is closed (loses focus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(System.EventArgs e)
        {
            if (Closed != null)
                Closed(this, e);
        }

        public event EventHandler<SelectedStatementEventArgs> SelectedStatement;

        /// <summary>
        /// Invokes the SelectedItem event; called whenever item in the listBox is selected
        /// </summary>
        /// <param name="e"></param>
        private void OnSelectedStatement(SelectedStatementEventArgs e)
        {
            if (SelectedStatement != null)
                SelectedStatement(this, e);
        }

        //private void AssistListBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Space)
        //    {
        //        // Hide the Popup
        //        IsOpen = false;

        //        //mark routed event as handled
        //        e.Handled = true;

        //        if (AssistListBox.SelectedItem != null)
        //        {
        //            Statement statement = (Statement)Enum.Parse(typeof(Statement), AssistListBox.SelectedItem as string);
        //            OnSelectedStatement(new SelectedStatementEventArgs(statement));
        //        }
        //    }
        //    else if (e.Key == Key.Escape)
        //    {
        //        //mark routed event as handled
        //        e.Handled = true;

        //        // Hide the Popup
        //        IsOpen = false;
        //    }
        //}

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsKeyboardFocusWithin == false)
            {
                IsOpen = false;
                OnClosed(System.EventArgs.Empty);
            }
        }
        
        private void AssistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Statement statement = (Statement)Enum.Parse(typeof(Statement), AssistListBox.SelectedItem as string);
            OnSelectedStatement(new SelectedStatementEventArgs(statement));
        }

        internal void MoveToNext()
        {
            IEnumerable<string> items = AssistListBox.ItemsSource as IEnumerable<string>;
            if (items != null)
            {
                int num = items.Count();
                if (AssistListBox.SelectedIndex < num - 1)
                {
                    AssistListBox.SelectedIndex += 1;
                }
                else
                {
                    AssistListBox.SelectedIndex = 0;
                }
            }
        }

        internal void MoveToPrevious()
        {
            IEnumerable<string> items = AssistListBox.ItemsSource as IEnumerable<string>;
            if (items != null)
            {
                int num = items.Count();
                if (0 < AssistListBox.SelectedIndex)
                {
                    AssistListBox.SelectedIndex -= 1;
                }
                else
                {
                    AssistListBox.SelectedIndex = num - 1;
                }
            }
        }

        internal void SelectCurrent()
        {
            if (AssistListBox.SelectedItem != null)
            {
                Statement statement = (Statement)Enum.Parse(typeof(Statement), AssistListBox.SelectedItem as string);
                OnSelectedStatement(new SelectedStatementEventArgs(statement));
            }
        }
    }
}

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
using TraceLab.UI.WPF.EventArgs;
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Controls
{
    /// <summary>
    /// ComboBoxStatement represents the Select and Load statements in the decision code.
    /// The items of the combobox are requested to be filled by firing the event 
    /// ComboBoxDropDownOpened.
    /// </summary>
    public class ComboBoxStatement : StackPanel
    {
        private TextBlock m_statementWithOpeningParenthesis = new TextBlock();
        private ComboBox m_comboBox;
        private TextBlock m_closingParenthesis = new TextBlock();

        public ComboBoxStatement(Statement statement)
        {
            System.Diagnostics.Debug.WriteLine("Constructing statement");
            Statement = statement;

            InitStackPanelStyling();
            InitComboBox();

            m_statementWithOpeningParenthesis.Text = statement.ToString() + "(";
            m_closingParenthesis.Text = ")";

            Children.Add(m_statementWithOpeningParenthesis);
            Children.Add(m_comboBox);
            Children.Add(m_closingParenthesis);
        }

        public ComboBoxStatement(Statement statement, string selectedItem)
            : this(statement)
        {
            if (m_comboBox.Items.MoveCurrentTo(selectedItem))
            {
                m_comboBox.SelectedItem = selectedItem;
            }
            else
            {
                m_comboBox.Items.Add(selectedItem);
                m_comboBox.SelectedItem = selectedItem;
            }
        }

        private void InitComboBox()
        {
            m_comboBox = new ComboBox();
            m_comboBox.FontFamily = new System.Windows.Media.FontFamily("Verdana");
            m_comboBox.FontSize = 10.0;
            m_comboBox.Height = 14.0;
            m_comboBox.Padding = new Thickness(1.0, 1.0, 1.0, 1.0);
            m_comboBox.Margin = new Thickness(0.0, 1.0, 0.0, 0.0);

            m_comboBox.DropDownOpened += OnComboBoxDropDownOpened;
            m_comboBox.DropDownClosed += OnComboBoxDropDownClosed;

        }

        public event EventHandler<FillItemsNeededEventArgs> ComboBoxDropDownOpened;
        public event EventHandler<FillItemsNeededEventArgs> ComboBoxDropDownClosed;

        private void OnComboBoxDropDownOpened(object sender, System.EventArgs args)
        {
            //fire wrapper event
            if (ComboBoxDropDownOpened != null)
                ComboBoxDropDownOpened(sender, new FillItemsNeededEventArgs(Statement, m_comboBox.SelectedItem));
        }

        private void OnComboBoxDropDownClosed(object sender, System.EventArgs args)
        {
            //fire wrapper event
            if (ComboBoxDropDownClosed != null)
                ComboBoxDropDownClosed(sender, new FillItemsNeededEventArgs(Statement, m_comboBox.SelectedItem));
        }

        private void InitStackPanelStyling()
        {
            //set orientation to vertical
            this.Orientation = Orientation.Horizontal;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            this.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);

            //style textblocks
            m_statementWithOpeningParenthesis.Height = 14.0;
            m_statementWithOpeningParenthesis.Padding = new Thickness(0.0, 2.0, 0.0, 0.0);
            m_statementWithOpeningParenthesis.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            m_closingParenthesis.Height = 14.0;
            m_closingParenthesis.Padding = new Thickness(0.0, 2.0, 0.0, 0.0);
            m_closingParenthesis.Margin = new Thickness(0.0);
            m_closingParenthesis.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        }

        public string FormattedVisualStatement
        {
            get
            {
                return m_statementWithOpeningParenthesis.Text + "\"" + m_comboBox.SelectedItem + "\"" + m_closingParenthesis.Text;
            }
        }

        public Statement Statement
        {
            get;
            set;
        }
    }
}

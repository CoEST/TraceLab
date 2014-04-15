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
using System.Windows;
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for ExperimentImportManagerView.xaml
    /// </summary>
    public partial class ExperimentImportManagerView : UserControl
    {
        public ExperimentImportManagerView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window parent = this.Parent as Window;
            if (parent != null)
            {
                parent.DialogResult = true;
            }
        }
    }

    public class IsPackageIncludedSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate found;
            FrameworkElement element = container as FrameworkElement;
            if (element != null)
            {
                if (item != null && item is PackageViewModel)
                    found = element.FindResource("PackageIncludeColumn") as DataTemplate;
                else
                    found = element.FindResource("EmptyTemplate") as DataTemplate;
            }
            else
                found = base.SelectTemplate(item, container);

            return found;
        }
    }
}

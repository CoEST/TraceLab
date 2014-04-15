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

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for ComponentLibraryErrorDisplay.xaml
    /// </summary>
    public partial class ComponentLibraryErrorDisplay : UserControl
    {
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(ComponentLibraryErrorDisplay));
        public static readonly DependencyProperty ErrorsProperty = DependencyProperty.Register("Errors", typeof(IEnumerable<string>), typeof(ComponentLibraryErrorDisplay));

        public ComponentLibraryErrorDisplay()
        {
            InitializeComponent();
        }

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public IEnumerable<string> Errors
        {
            get { return (IEnumerable<string>)GetValue(ErrorsProperty); }
            set { SetValue(ErrorsProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = this.Parent as Window;
            if (window == null)
                throw new InvalidOperationException("Unknown heirarchy");

            window.Close();
        }
    }
}

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
using System.Windows.Markup;
using System.Windows.Data;

namespace TraceLab.UI.WPF.Converters
{
    /// <summary>
    /// If string is null or empty the converterter returns Collapsed visibility, otherwise Visible
    /// </summary>
    class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public BoolToVisibilityConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //validate target type
            if (targetType != typeof(System.Windows.Visibility)) throw new InvalidOperationException();

            System.Windows.Visibility visibility = System.Windows.Visibility.Collapsed; //default
            if (parameter != null)
            {
                //get default visibility hidden or collapsed visibility
                visibility = (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), (string)parameter);
            }

            if ((bool)value == true)
            {
                visibility = System.Windows.Visibility.Visible;
            }
            
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //it is one way visibility
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

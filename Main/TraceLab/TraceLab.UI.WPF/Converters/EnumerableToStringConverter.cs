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
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections;

namespace TraceLab.UI.WPF.Converters
{
    class EnumerableOfStringsToStringConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string target = string.Empty;
            IEnumerable obj = value as IEnumerable;
            foreach (object valueObject in obj)
            {
                string valueStr = valueObject.ToString().Trim(); 
                if(!string.IsNullOrWhiteSpace(valueStr))
                {
                    if (target.Length != 0)
                        target += ", ";
                    target += valueStr;
                }
            }

            return target;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string valueStr = (string)value;
            var values = valueStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> target = new List<string>();

            foreach(string str in values)
            {
                string trimmed = str.Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    target.Add(trimmed);
                }
            }

            return target;
        }

        #endregion
    }
}

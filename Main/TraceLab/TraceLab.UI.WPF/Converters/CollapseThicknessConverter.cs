﻿// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
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
using System.Windows.Data;
using System.Windows.Markup;

namespace TraceLab.UI.WPF.Converters
{
    /// <summary>
    /// Collapses a Thickness into a single double
    /// </summary>
    public class CollapseThicknessConverter : MarkupExtension, IValueConverter
    {
        public CollapseThicknessConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(!(value is Thickness))
                throw new NotSupportedException("CollapseThicknessConverter: Can only convert from Thickness");
            if(targetType != typeof(double))
                throw new NotSupportedException("CollapseThicknessConverter: Can only convert to doubles");

            var thick = (Thickness)value;
            var first = Math.Max(thick.Left, thick.Right);
            var second = Math.Max(thick.Top, thick.Bottom);
            return Math.Max(first, second);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

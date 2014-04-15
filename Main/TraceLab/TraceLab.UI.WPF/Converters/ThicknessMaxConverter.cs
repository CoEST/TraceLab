using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Markup;

namespace TraceLab.UI.WPF.Converters
{
    public class ThicknessMaxConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            double horizontalMax = Math.Max(thickness.Left, thickness.Right);
            double verticalMax = Math.Max(thickness.Top, thickness.Bottom);
            return Math.Max(horizontalMax, verticalMax);
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

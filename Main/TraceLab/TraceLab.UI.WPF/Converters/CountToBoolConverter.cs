using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace TraceLab.UI.WPF.Converters
{
    public class CountToBoolConverter : MarkupExtension, IValueConverter
    {
        public CountToBoolConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(int) || targetType != typeof(bool))
            {
                throw new InvalidOperationException();
            }

            int val = (int)value;
            return val > 0;
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

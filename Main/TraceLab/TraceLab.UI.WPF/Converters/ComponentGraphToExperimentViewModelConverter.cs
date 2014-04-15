using System;
using System.Windows.Data;
using System.Windows.Markup;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Converters
{
    public class ComponentGraphToExperimentViewModelConverter : MarkupExtension, IValueConverter
    {
        public ComponentGraphToExperimentViewModelConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEditableExperiment graph = value as IEditableExperiment;
            if (graph == null)
            {
                return null;
            }

            return new TopLevelEditableExperimentViewModel(graph);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Windows.DependencyProperty.UnsetValue;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

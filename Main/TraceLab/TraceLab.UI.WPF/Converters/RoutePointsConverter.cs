using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;
using TraceLab.Core.Experiments;
using System.Collections.ObjectModel;

namespace TraceLab.UI.WPF.Converters
{
    /// <summary>
    /// Route points converter converts to RoutePointsCollection into array of Points (Point[])
    /// required by GraphSharp. 
    /// See EdgeStyle.xaml, and GraphSharp.Converters:EdgeRouteToPathDataConverter
    /// </summary>
    class RoutePointsConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a RoutePointsCollection into array of Points (Point[]).
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RoutePointsCollection sourcePoints = value as RoutePointsCollection;
            Point[] targetPoints;
            if (sourcePoints == null)
            {
                targetPoints = null;
            }
            else
            {
                targetPoints = new Point[sourcePoints.Count];
                int i = 0;
                foreach (RoutePoint p in sourcePoints)
                {
                    targetPoints[i++] = new Point(p.X, p.Y);
                }
            }

            return targetPoints;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //not needed
            throw new NotImplementedException();
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

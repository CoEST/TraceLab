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
using System.Windows.Data;
using System.Windows.Markup;
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Converters
{
    public class ExperimentWrapperToExperimentConverter : MarkupExtension, IValueConverter
    {
        public ExperimentWrapperToExperimentConverter() { }

        /// <summary>
        /// Converts a IGraphViewModel to its referenced wrapped Experiment.
        /// Note:
        /// in case of SubLevelExperimentViewModel it returns SubLevelExperimentViewModel -> CompositeComponentGraph -> Experiment
        /// in case of TopLevelExperimentViewModel it returns TopLevelExperimentViewModel -> Experiment
        /// 
        /// If parameter is set to True
        /// in case of SubLevelExperimentViewModel it returns SubLevelExperimentViewModel -> CompositeComponentGraph
        /// </summary>
        /// <param name="value">The IGraphViewModel produced by the binding source.</param>
        /// <param name="targetType">The type IExperiment of the binding target property.</param>
        /// <param name="parameter">If converter parameter is set to True, in case of SubLevelExperimentViewModel it will return its direct wrapped composite component graph. Otherwise as default the actual wrapped referred Experiment is going to be return</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IGraphViewModel wrapper = value as IGraphViewModel;
            if (wrapper == null)
            {
                return null;
            }

            object exp = wrapper.GetExperiment();
            // as default return actual Experiment 
            // in case of SubLevelExperimentViewModel -> CompositeComponentGraph -> Experiment
            // in case of TopLevelExperimentViewModel -> Experiment
            return exp;
        }

        /// <summary>
        /// Converts a value. It returns back the value. Needed for ActiveDocument on the DockingManager binding.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
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

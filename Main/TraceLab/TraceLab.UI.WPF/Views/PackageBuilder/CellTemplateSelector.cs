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
using System.Windows.Controls;
using System.Windows;
using TraceLab.Core.PackageBuilder;

namespace TraceLab.UI.WPF.Views.PackageBuilder
{
    class HasComponentsTemplateSelector : DataTemplateSelector 
    {
        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                var folder = item as PackageHeirarchyItem;

                if (folder != null)
                    return element.FindResource("HasComponentsCellTemplate") as DataTemplate;
                else
                    return element.FindResource("EmptyCellTemplate") as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }

    class HasTypesTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                var folder = item as PackageHeirarchyItem;

                if (folder != null)
                    return element.FindResource("HasTypesCellTemplate") as DataTemplate;
                else
                    return element.FindResource("EmptyCellTemplate") as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}

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
using System.Windows;
using TraceLab.UI.WPF.Resources;
using TraceLab.UI.WPF.Utilities;
using WPG.Data;

namespace TraceLab.UI.WPF.Selectors
{
    public class ConfigPropertyTemplateSelector : WPG.PropertyTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Property property = item as Property;
            if (property == null)
            {
                throw new ArgumentException("item must be of type Property");
            }

            DataTemplate template = null;

            //search for template in ConfigPropertyGridDataTemplates, specifically datatemplates for FilePath and DirectoryPath

            //if parent window is MainWindow just use ConfigPropertyGridDataTemplates
            var window = container.GetParent<TraceLab.UI.WPF.Views.MainWindow>(null);
            if (window != null)
            {
                template = FindDataTemplate(property, new ConfigPathsDataTemplates());
            }
            else
            {
                //if parent window is a benchmark window use ConfigPropertyGridDataTemplatesBenchmark
                //the only difference are bindings to the experiment path 
                var benchmarkWizardWindow = container.GetParent<TraceLab.UI.WPF.Views.BenchmarkWizardDialog>(null);
                if (benchmarkWizardWindow != null)
                {
                    template = FindDataTemplate(property, new ConfigPathsDataTemplatesBenchmark());
                }
            }

            //otherwise use base template selector
            if (template == null)
            {
                template = base.SelectTemplate(item, container);
            }

            return template;
        }

        private DataTemplate FindDataTemplate(Property property, ResourceDictionary templates)
        {
            DataTemplate template = null;

            string propType = property.PropertyType.FullName;

            if (propType.Equals(typeof(TraceLabSDK.Component.Config.FilePath).FullName) == true ||
                propType.Equals(typeof(TraceLabSDK.Component.Config.DirectoryPath).FullName) == true)
            {
                  object dataTempalteKey = new DataTemplateKey(property.PropertyType);
                  template = templates[dataTempalteKey] as DataTemplate;
            }
            return template;
        }
    }
}

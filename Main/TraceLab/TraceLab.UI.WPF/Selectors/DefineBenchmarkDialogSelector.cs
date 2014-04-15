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
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Selectors
{
    public class DefineBenchmarkDialogSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            List<object> values = item as List<object>;
            if (values != null)
            {
                DefiningBenchmarkViewModel bvm = (DefiningBenchmarkViewModel)values[0];
                DefiningBenchmarkDialogState currentState = (DefiningBenchmarkDialogState)values[1];

                //find the proper dialog window
                Window win = Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.GetType().ToString().Equals("TraceLab.UI.WPF.Views.DefineBenchmarkDialog"));

                // Select one of the DataTemplate objects, based on the current state
                switch (currentState)
                {
                    case DefiningBenchmarkDialogState.DefineBenchmark:
                        return win.FindResource("defineBenchmark") as DataTemplate;
                    case DefiningBenchmarkDialogState.Authentication:
                        return win.FindResource("contestPublishing") as DataTemplate; //use the same template as UploadingContest
                    case DefiningBenchmarkDialogState.UploadingContest:
                        return win.FindResource("contestPublishing") as DataTemplate; //use the same template as Authenticate
                }
            }

            return null;
        }
    }
}

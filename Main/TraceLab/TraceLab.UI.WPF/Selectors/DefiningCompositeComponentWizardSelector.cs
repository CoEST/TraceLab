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
    class DefiningCompositeComponentWizardSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            List<object> values = item as List<object>;
            if (values != null)
            {
                DefiningCompositeComponentWizardState currentState = (DefiningCompositeComponentWizardState)values[1];

                //find benchmark dialog window
                Window win = Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.GetType().ToString().Equals("TraceLab.UI.WPF.Views.DefineCompositeComponentWizard"));

                // Select one of the DataTemplate objects, based on the current state
                switch (currentState)
                {
                    case DefiningCompositeComponentWizardState.IOSpec:
                        return win.FindResource("ioSpecPanel") as DataTemplate;
                    case DefiningCompositeComponentWizardState.Configuration:
                        return win.FindResource("configurationPanel") as DataTemplate;
                    case DefiningCompositeComponentWizardState.Info:
                        return win.FindResource("componentInfoPanel") as DataTemplate;
                    case DefiningCompositeComponentWizardState.Confirmation:
                        return win.FindResource("confirmationPanel") as DataTemplate;
                }
            }
            
            return null;
        }
        
    }
}

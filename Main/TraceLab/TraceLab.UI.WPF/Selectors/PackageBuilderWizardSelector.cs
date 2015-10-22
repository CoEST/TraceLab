using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using TraceLab.Core.PackageBuilder;

namespace TraceLab.UI.WPF.Selectors
{
    class PackageBuilderWizardSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            List<object> values = item as List<object>;
            if (values != null)
            {
                PackageBuilderWizardPage currentState = (PackageBuilderWizardPage)values[1];

                //find benchmark dialog window
                Window win = Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.GetType().ToString().Equals("TraceLab.UI.WPF.Views.PackageBuilder.PackageBuilderMainWindow"));

                // Select one of the DataTemplate objects, based on the current state
                switch (currentState)
                {
                    case PackageBuilderWizardPage.Config:
                        return win.FindResource("configurationPage") as DataTemplate;
                    case PackageBuilderWizardPage.FileViewer:
                        return win.FindResource("packageFileTreeViewer") as DataTemplate;
                }
            }

            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using TraceLab.UI.WPF.ViewModels;
using System.Linq;
using TraceLab.Core.Workspaces;

namespace TraceLab.UI.WPF.Selectors
{
    class WorkspaceUnitValueDisplaySelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            WpfWorkspaceUnitWrapper unitWrapper = item as WpfWorkspaceUnitWrapper;
            DataTemplate template = null;
            if (unitWrapper != null)
            {
                FrameworkElement control = container as FrameworkElement;
                
                //if primitive value the display value (deserilizes Data from from Workspace)
                if (unitWrapper.Type.IsPrimitive || unitWrapper.Type == String.Empty.GetType())
                {
                    template = (DataTemplate)control.FindResource("displayValue");
                }
                else
                {
                    var assemblyExtensions = WorkspaceUIAssemblyExtensions.Extensions;
                    if (TraceLab.Core.Workspaces.WorkspaceViewerLoader.CheckIfEditorExists(unitWrapper.Type, assemblyExtensions))
                    {
                        template = (DataTemplate)control.FindResource("displayEditorIcon");
                    }
                    else
                    {
                        template = (DataTemplate)control.FindResource("displayViewerMissing");
                    }
                }
            }
            
            return template;
        }


    }
}

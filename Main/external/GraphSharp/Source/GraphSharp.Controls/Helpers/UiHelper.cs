using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GraphSharp.Helpers
{
    internal static class UiHelper
    {
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return default(T);
            }
            T local = default(T);
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; (i < childrenCount) && (local == null); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                T local2 = child as T;
                if (local2 != null)
                {
                    if (!string.IsNullOrEmpty(childName))
                    {
                        FrameworkElement element = child as FrameworkElement;
                        if ((element != null) && (element.Name == childName))
                        {
                            local = (T)child;
                        }
                    }
                    else
                    {
                        local = (T)child;
                    }
                }
                if (local == null)
                {
                    local = FindChild<T>(child, childName);
                }
            }
            return local;
        }
    }
}

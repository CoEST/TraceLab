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

using System.Windows;
using System.Windows.Media;

namespace TraceLab.UI.WPF.Utilities
{
    internal static class WpfHelpers
    {
        public static T GetParent<T>(this DependencyObject obj, DependencyObject limit) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            var targetType = typeof(T);
            DependencyObject parent = GetVisualParent(obj);
            while (parent != null && !targetType.IsAssignableFrom(parent.GetType()))
            {
                if (parent == limit)
                {
                    parent = null;
                    break;
                }

                parent = GetVisualParent(parent);
            }

            return parent as T;
        }

        public static T GetParent<T>(this DependencyObject obj, DependencyObject limit, int ancestorLevel) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject parent = GetVisualParent(obj);
            
            int level = 0;
            while (parent != null)
            {
                if (parent == limit)
                {
                    parent = null;
                    break;
                }

                parent = GetVisualParent(parent);
                
                if (parent.GetType() == typeof(T))
                {
                    level++;
                }

                if (level == ancestorLevel)
                {
                    break; //parent found
                }
            }

            return parent as T;
        }

        /// <summary>
        /// Gets the visual parent of the given dependency object.
        /// 
        /// The visual tree represents all of the elements in your UI which render to an output device (typically, the screen).        
        /// However, there is one wrinkle which makes this a little more complicated. 
        /// Anything which descends from ContentElement can appear in a user interface, but is not actually in the visual tree. 
        /// WPF will "pretend" that those elements are in the visual tree, to facilitate consistent event routing, but it's just an illusion. 
        /// VisualTreeHelper does not work with ContentElement objects because ContentElement does not derive from Visual or Visual3D. 
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private static DependencyObject GetVisualParent(DependencyObject obj)
        {
            DependencyObject parent;
            if (obj is Visual || obj is System.Windows.Media.Media3D.Visual3D)
            {
                parent = VisualTreeHelper.GetParent(obj);
            }
            else
            {
                // If we're in Logical Land then we must walk 
                // up the logical tree until we find a 
                // Visual/Visual3D to get us back to Visual Land.
                parent = LogicalTreeHelper.GetParent(obj);
            }
            return parent;
        }
    }
}

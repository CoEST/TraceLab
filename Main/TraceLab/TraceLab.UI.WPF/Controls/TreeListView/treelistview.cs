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
using System.Windows.Controls;
using System.Windows;

namespace TraceLab.UI.WPF.Controls
{
    public class TreeListViewExpander : Expander
    {
    }

    public class TreeListView : TreeView
    {
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(TreeListView));
        public static DependencyProperty HeaderVisibleProperty = DependencyProperty.Register("HeaderVisible", typeof(bool), typeof(TreeListView), new PropertyMetadata(true));

        public GridViewColumnCollection Columns
        {
            get
            {
                return (GridViewColumnCollection)GetValue(ColumnsProperty);
            }
            set
            {
                SetValue(ColumnsProperty, value);
            }
        }

        public bool HeaderVisible
        {
            get { return (bool)GetValue(HeaderVisibleProperty); }
            set { SetValue(HeaderVisibleProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            TreeListViewItem treeItem = element as TreeListViewItem;
            if (treeItem != null)
            {
                treeItem.SetColumns(null);
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            TreeListViewItem treeItem = element as TreeListViewItem;
            if (treeItem != null)
            {
                treeItem.SetColumns(Columns);
            }
        }
    }

    public class TreeListViewItem : TreeViewItem
    {
        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(TreeListViewItem));

        public GridViewColumnCollection Columns
        {
            get
            {
                return (GridViewColumnCollection)GetValue(ColumnsProperty);
            }
            private set
            {
                SetValue(ColumnsProperty, value);
            }
        }

        public void SetColumns(GridViewColumnCollection newColumns)
        {
            Columns = newColumns;
        }


        /// <summary>
        /// Item's hierarchy in the tree
        /// </summary>
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    TreeListViewItem parent =
                        ItemsControl.ItemsControlFromItemContainer(this)
                            as TreeListViewItem;
                    _level = (parent != null) ? parent.Level + 1 : 0;
                }
                return _level;
            }
        }


        protected override DependencyObject
                           GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            TreeListViewItem treeItem = element as TreeListViewItem;
            if (treeItem != null)
            {
                treeItem.SetColumns(Columns);
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            TreeListViewItem treeItem = element as TreeListViewItem;
            if (treeItem != null)
            {
                treeItem.SetColumns(null);
            }
        }

        protected override void OnExpanded(RoutedEventArgs e)
        {
            base.OnExpanded(e);
        }

        private int _level = -1;
    }

}

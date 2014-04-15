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
using System.Windows.Input;

namespace TraceLab.UI.WPF.Controls
{
    public abstract class PathTypeEditor<T> : Control where T : TraceLabSDK.Component.Config.BasePath
    {
        static PathTypeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathTypeEditor<T>), new FrameworkPropertyMetadata(typeof(PathTypeEditor<T>)));
            PathProperty = DependencyProperty.Register("Path", typeof(T), typeof(PathTypeEditor<T>));
            DataRootProperty = DependencyProperty.Register("DataRoot", typeof(string), typeof(PathTypeEditor<T>));

            FoundFileCommand = new RoutedCommand("FoundFileCommand", typeof(PathTypeEditor<T>));
            CommandManager.RegisterClassCommandBinding(typeof(PathTypeEditor<T>), new CommandBinding(FoundFileCommand, OnFoundFileCommand));
        }

        public PathTypeEditor()
        {
        }

        public static readonly DependencyProperty PathProperty;
        public T Path
        {
            get { return (T)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty DataRootProperty;
        public string DataRoot
        {
            get { return (string)GetValue(DataRootProperty); }
            set { SetValue(DataRootProperty, value); }
        }

        #region Commands

        public readonly static RoutedCommand FoundFileCommand;

        private static void OnFoundFileCommand(object sender, ExecutedRoutedEventArgs e)
        {
            PathTypeEditor<T> _FilePathTypeEditor = (PathTypeEditor<T>)sender;
            _FilePathTypeEditor.UpdateFilePath();
        }

        protected abstract void UpdateFilePath();

        #endregion
    }
}

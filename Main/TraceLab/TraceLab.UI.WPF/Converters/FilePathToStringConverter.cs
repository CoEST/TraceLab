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
using System.Windows.Data;
using System.Windows.Markup;
using TraceLabSDK.Component.Config;

namespace TraceLab.UI.WPF.Converters
{
    public class FilePathConverter : MarkupExtension, IMultiValueConverter
    {
        public FilePathConverter()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 2)
                throw new ArgumentException("Invalid number of arguments");
            if (targetType != typeof(FilePath) && targetType != typeof(DirectoryPath))
                throw new NotSupportedException("This converter only converts to filepaths");
            if (values[1] == System.Windows.DependencyProperty.UnsetValue)
                throw new ArgumentException("The experiment path has to be set. If filepath is null, the new FilePath object is initialized based on experiment path.");

            BasePath path = null;

            //if experiment path is set extract the experiment root, that is the data root for
            //FilePaths and DirectoryPaths
            if (targetType == typeof(FilePath))
            {
                path = (FilePath)values[0];
                //if experiment path has been set 
                if (path == null)
                {
                    string experimentPath = (string)values[1];
                    string dataRoot = System.IO.Path.GetDirectoryName(experimentPath);
                    path = new FilePath();
                    path.Init("", dataRoot);
                }
            }
            else if (targetType == typeof(DirectoryPath))
            {
                path = (DirectoryPath)values[0];
                if (path == null)
                {
                    string experimentPath = (string)values[1];
                    string dataRoot = System.IO.Path.GetDirectoryName(experimentPath);
                    path = new DirectoryPath();
                    path.Init("", dataRoot);
                }
            }
            
            return path;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] { value, null };
        }

        #endregion
    }

    //public class FilePathToStringConverter : MarkupExtension, IValueConverter
    //{
    //    public string DataRoot { get; set; }

    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        FilePath filePath = value as FilePath;
    //        string stringPath;
    //        if (filePath == null)
    //        {
    //            stringPath = String.Empty;
    //        }
    //        else
    //        {
    //            stringPath = filePath.Absolute;
    //        }
                        
    //        return stringPath;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //        //string stringPath = (string)value;
    //        //var path = new FilePath(stringPath);
    //        //return path;
    //    }

    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //}
}

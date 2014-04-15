//Copyright (c) 2007-2010, Adolfo Marinucci
//All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, 
//are permitted provided that the following conditions are met:
//
//* Redistributions of source code must retain the above copyright notice, 
//  this list of conditions and the following disclaimer.
//* Redistributions in binary form must reproduce the above copyright notice, 
//  this list of conditions and the following disclaimer in the documentation 
//  and/or other materials provided with the distribution.
//* Neither the name of Adolfo Marinucci nor the names of its contributors may 
//  be used to endorse or promote products derived from this software without 
//  specific prior written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
//AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
//PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
//HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
//OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
//EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Markup;

namespace AvalonDock
{
    //public class FindResourcePathConverter : IValueConverter
    //{
    //    #region IValueConverter Members

    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (value == null)
    //        {
    //            return null;
    //            //return new Uri(@"DocumentHS.png", UriKind.Relative);
    //        }

    //        return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}

    /// <summary>
    /// Converter from boolean values to visibility (inverse mode)
    /// </summary>
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? Visibility.Visible :
            (parameter != null && ((string)parameter) == "Hidden" ? Visibility.Hidden : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(object), typeof(Image))]
    public class ObjectToImageConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double width = 16.0;
            if (parameter != null &&
                parameter is double)
                width = (double)parameter;

            if (value is string)
            {
                Uri iconUri;
                // try to resolve given value as an absolute URI
                if (Uri.TryCreate(value as String, UriKind.RelativeOrAbsolute, out iconUri))
                {
                    var img = new BitmapImage(iconUri);
                    if (img != null)
                    {
                        return new Image()
                        {
#if NET4
                            UseLayoutRounding = true,
#endif
                            Width = width,
                            Source = img
                        };
                    }
                }
            }
            else if (value is BitmapImage)
            {
                var img = value as BitmapImage;
                return new Image()
                {
#if NET4
                    UseLayoutRounding = true,
#endif
                    Width = width,
                    Source = new BitmapImage(img.UriSource)
                };
            }


            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class Converters
    {
        static BoolToVisibilityConverter _BoolToVisibilityConverter = null;

        public static BoolToVisibilityConverter BoolToVisibilityConverter
        {
            get
            {
                if (_BoolToVisibilityConverter == null)
                    _BoolToVisibilityConverter = new BoolToVisibilityConverter();


                return _BoolToVisibilityConverter;
            }
        }

        static ObjectToImageConverter _ObjectToImageConverter = null;

        public static ObjectToImageConverter ObjectToImageConverter
        {
            get
            {
                if (_ObjectToImageConverter == null)
                    _ObjectToImageConverter = new ObjectToImageConverter();


                return _ObjectToImageConverter;
            }
        }

    }

    public class DocumentContentConverter : MarkupExtension, IMultiValueConverter
    {
        public DocumentContentConverter()
        {
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DocumentPane pane = value[0] as DocumentPane;
            
            if (pane != null)
            {
                return pane.GetContent(value[1]);
            }

            return null;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal static class PaneHelper
    {
        public static object GetContent(this Pane pane, object obj)
        {
            ManagedContent content = (ManagedContent)pane.ItemContainerGenerator.ContainerFromItem(pane.SelectedItem);
            if (content == null && obj is ManagedContent)
            {
                content = obj as ManagedContent;
            }

            if (content != null)
            {
                return content.Content;
            }
            
            return null;
        }
    }
}

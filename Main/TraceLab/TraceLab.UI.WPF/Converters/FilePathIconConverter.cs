using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using TraceLab.UI.WPF.Views.PackageBuilder;
using System.Windows.Media.Imaging;

namespace TraceLab.UI.WPF.Converters
{
    /// <summary>
    /// Converts filepath to the corresponding icon in the system
    /// </summary>
    class FilePathIconConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string sourceFilePath = value as string;

            var source = IconExtractor.GetIcon(sourceFilePath, true).ToBitmap();

            BitmapSource bitSrc = null;

            using (var hBitmap = new SafeHBitmapHandle(source.GetHbitmap(), true))
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap.DangerousGetHandle(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            return bitSrc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //not needed
            throw new NotImplementedException();
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

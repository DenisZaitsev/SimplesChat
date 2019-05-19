using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ChatClient
{


    /// <summary>
    /// Small converter from bitmap to bitmap source (i'm not sure why wpf image needs this, but okay)
    /// </summary>
    public class BitmapToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object source, Type targetType,object parameter, CultureInfo culture)
        {
            BitmapSource bitSrc = null;
            Bitmap bmp = source as Bitmap;


            var hBitmap = bmp.GetHbitmap();

            bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bitSrc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

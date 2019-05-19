using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.IO;
using System.Windows;
using System.ComponentModel;

namespace TCPTalker
{
    [Serializable]
    public class ImageMessage : Message
    {
        
        public Bitmap Image { get; set; }

        public ImageMessage(DateTime dateTime, string sender, Bitmap bitmap) : base(Header.ImageMessage, dateTime, sender)
        {
            this.Image = bitmap;
        }

        private static BitmapSource ToBitmapSource(Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            return bitSrc;
        }
    }
}

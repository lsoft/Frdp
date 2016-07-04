using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Frdp.Server.Bitmap
{
    public static class BitmapConversion
    {
        public static BitmapSourceWrapper BitmapToBitmapSource(
            this System.Drawing.Bitmap source
            )
        {
            var hbitmap = source.GetHbitmap();

            var bsource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hbitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                    );

            var result = new BitmapSourceWrapper(
                bsource,
                hbitmap
                );

            return
                result;
        }
    }
}
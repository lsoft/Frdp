using System;
using System.Windows.Media.Imaging;

namespace Frdp.Server.Bitmap
{
    public class BitmapSourceWrapper : IDisposable
    {
        private volatile bool _disposed = false;

        public BitmapSource Source
        {
            get;
            private set;
        }

        public IntPtr HBitmap
        {
            get;
            private set;
        }

        public BitmapSourceWrapper(
            BitmapSource source,
            IntPtr hBitmap
            )
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            Source = source;
            HBitmap = hBitmap;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                DeleteObject(HBitmap);

                _disposed = true;
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
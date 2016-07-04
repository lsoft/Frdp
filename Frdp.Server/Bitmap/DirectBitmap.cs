using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Frdp.Server.Bitmap
{
    public class DirectBitmap : IDisposable
    {
        public System.Drawing.Bitmap Bitmap
        {
            get;
            private set;
        }

        public byte[] Bits
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        private GCHandle _bitsHandle;

        private volatile bool _disposed;

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;

            Bits = new byte[width * height * 4];
            _bitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new System.Drawing.Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, _bitsHandle.AddrOfPinnedObject());
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                try
                {
                    Bitmap.Dispose();
                }
                catch
                {
                    //nothing to do
                }

                try
                {
                    _bitsHandle.Free();
                }
                catch
                {
                    //nothing to do
                }
            }
        }
    }
}
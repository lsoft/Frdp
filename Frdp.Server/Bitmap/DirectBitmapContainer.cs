using System;
using System.Runtime.InteropServices;
using System.Threading;
using Frdp.Common.Block.Diff;

namespace Frdp.Server.Bitmap
{
    public class DirectBitmapContainer
    {
        private readonly Frdp.Common.ILogger _logger;

        private volatile DirectBitmap _bitmap = null;

        public DirectBitmap DirectBitmap
        {
            get
            {
                return
                    _bitmap;
            }
        }

        public delegate void BitmapChangedDelegate(DirectBitmapContainer container);

        public event BitmapChangedDelegate BitmapChangedEvent;

        public DirectBitmapContainer(
            Frdp.Common.ILogger logger
            )
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;

            _bitmap = new DirectBitmap(1, 1);
        }

        public void SetSize(
            int width,
            int height
            )
        {
            if (_bitmap == null || _bitmap.Width != width || _bitmap.Height != height)
            {
                var newBitmap = new DirectBitmap(
                    width,
                    height
                    );

                var oldBitmap = Interlocked.Exchange(ref _bitmap, newBitmap);

                if (oldBitmap != null)
                {
                    oldBitmap.Dispose();
                }
            }
        }

        public void TakeBlock(
            BlockDiff diff,
            uint blockWidth,
            uint blockHeight
            )
        {
            if (diff == null)
            {
                throw new ArgumentNullException("diff");
            }

            byte[] data;
            uint startIndex;
            uint length;
            diff.OpenData(
                out data,
                out startIndex,
                out length
                );

            var imageWidth = _bitmap.Width;
            var imageHeight = _bitmap.Height;

            unsafe
            {
                fixed (byte* p = _bitmap.Bits)
                {
                    uint x1 = (uint)diff.X*blockWidth;
                    uint y1 = (uint)diff.Y*blockHeight;

                    fixed (byte* pfrom = data)
                    {
                        DoCopy(
                            blockWidth
                            , blockHeight
                            , x1
                            , y1
                            , imageWidth
                            , imageHeight
                            , new IntPtr(p)
                            , new IntPtr(pfrom)
                            );
                    }

                }
            }
        }

        /*
        private void DoCopy(
            ulong blockWidth
            ,ulong blockHeight
            ,ulong x1
            ,ulong y1
            ,long imageWidth
            ,long imageHeight
            ,byte* p
            ,byte* data
            )
        {
            var cc = 0;
            for (ulong y = 0; y < blockHeight; y++)
            {
                var x2 = x1;
                var y2 = y1 + y;

                if (y2 < (ulong)imageHeight)
                {
                    var p3 = p + (y2 * (ulong)imageWidth + x2) * 4;

                    for (ulong x = 0; x < blockWidth; x++)
                    {
                        if (x1 + x < (ulong)imageWidth)
                        {
                            p3[0] = data[(int)x + cc];
                            p3[1] = data[(int)x + cc];
                            p3[2] = data[(int)x + cc];
                            p3[3] = 255;

                            p3 += 4;
                        }
                    }

                    cc += (int)blockWidth;
                }
            }
        }
        //*/

        [DllImport("PerformanceLibrary.dll")]
        public static extern void DoCopy(
            uint blockWidth
            , uint blockHeight
            , uint x1
            , uint y1
            , int imageWidth
            , int imageHeight
            , IntPtr p
            , IntPtr data
            );

        public void MarkImageChanged()
        {
            OnBitmapChanged();
        }

        protected virtual void OnBitmapChanged()
        {
            try
            {
                BitmapChangedDelegate handler = BitmapChangedEvent;
                if (handler != null)
                {
                    handler(this);
                }
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }
        }

    }
}
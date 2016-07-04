using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Util;

namespace Frdp.Client.Helpers
{
    public static class OpenCVHelper
    {
        public static Mat ImprovedToMat(Bitmap src)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }


            int w = src.Width;
            int h = src.Height;
            MatType matType;
            switch (src.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                    matType = MatType.CV_8UC(3);
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    matType = MatType.CV_8UC(4);
                    break;
                case PixelFormat.Format16bppRgb565:
                    matType = MatType.CV_8UC(2);
                    break;
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Format1bppIndexed:
                    matType = MatType.CV_8UC(1);
                    break;
                default:
                    throw new NotImplementedException();
            }


            var dst = new Mat(h, w, matType);

            ImprovedToMat(src, dst);

            return
                dst;
        }


        public static unsafe void ImprovedToMat(Bitmap src, Mat dst)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }
            if (dst == null)
            {
                throw new ArgumentNullException("dst");
            }
            if (dst.IsDisposed)
            {
                throw new ArgumentException("The specified dst is disposed.", "dst");
            }
            if (dst.Depth() != MatType.CV_8U)
            {
                throw new NotSupportedException("Mat depth != CV_8U");
            }
            if (dst.Dims() != 2)
            {
                throw new NotSupportedException("Mat dims != 2");
            }
            if (src.Width != dst.Width || src.Height != dst.Height)
            {
                throw new ArgumentException("src.Size != dst.Size");
            }

            int w = src.Width;
            int h = src.Height;
            var rect = new Rectangle(0, 0, w, h);
            BitmapData bd = null;
            try
            {
                bd = src.LockBits(rect, ImageLockMode.ReadOnly, src.PixelFormat);

                var p = (byte*)bd.Scan0.ToPointer();
                int sstep = bd.Stride;
                int offset = sstep - (w / 8);
                var dstep = (uint)dst.Step();
                IntPtr dstData = dst.Data;
                var dstPtr = (byte*)dstData.ToPointer();

                bool submat = dst.IsSubmatrix();
                bool continuous = dst.IsContinuous();

                switch (src.PixelFormat)
                {
                    case PixelFormat.Format1bppIndexed:
                    {
                        if (dst.Channels() != 1)
                        {
                            throw new ArgumentException("Invalid nChannels");
                        }
                        if (submat)
                        {
                            throw new NotImplementedException("submatrix not supported");
                        }

                        int x = 0;
                        int y;
                        int bytePos;
                        byte b;
                        int i;
                        for (y = 0; y < h; y++)
                        {
                            // 横は必ず4byte幅に切り上げられる。
                            // この行の各バイトを調べていく
                            for (bytePos = 0; bytePos < sstep; bytePos++)
                            {
                                if (x < w)
                                {
                                    // 現在の位置のバイトからそれぞれのビット8つを取り出す
                                    b = p[bytePos];
                                    for (i = 0; i < 8; i++)
                                    {
                                        if (x >= w)
                                        {
                                            break;
                                        }
                                        // IplImageは8bit/pixel
                                        dstPtr[dstep * y + x] = ((b & 0x80) == 0x80) ? (byte)255 : (byte)0;
                                        b <<= 1;
                                        x++;
                                    }
                                }
                            }
                            // 次の行へ
                            x = 0;
                            p += sstep;
                        }
                    }
                        break;

                    case PixelFormat.Format16bppRgb565:
                        if (dst.Channels() != 2)
                        {
                            throw new ArgumentException("Invalid nChannels");
                        }

                        // ステップが同じで連続なら、一気にコピー
                        if (dstep == sstep && !submat && continuous)
                        {
                            var length = (uint)(dst.DataEnd.ToInt64() - dstData.ToInt64());
                            Utility.CopyMemory(dstData, bd.Scan0, length);
                        }
                        else
                        {
                            // 各行ごとにdstの行バイト幅コピー
                            var sp = (byte*)bd.Scan0;
                            var dp = (byte*)dst.Data;
                            for (int y = 0; y < h; y++)
                            {
                                Utility.CopyMemory(dp, sp, dstep);
                                sp += sstep;
                                dp += dstep;
                            }
                        }

                        break;

                    case PixelFormat.Format8bppIndexed:
                    case PixelFormat.Format24bppRgb:
                    {
                        if (src.PixelFormat == PixelFormat.Format8bppIndexed)
                        {
                            if (dst.Channels() != 1)
                            {
                                throw new ArgumentException("Invalid nChannels");
                            }
                        }
                        if (src.PixelFormat == PixelFormat.Format24bppRgb)
                        {
                            if (dst.Channels() != 3)
                            {
                                throw new ArgumentException("Invalid nChannels");
                            }
                        }

                        // ステップが同じで連続なら、一気にコピー
                        if (dstep == sstep && !submat && continuous)
                        {
                            var length = (uint)(dst.DataEnd.ToInt64() - dstData.ToInt64());
                            Utility.CopyMemory(dstData, bd.Scan0, length);
                        }
                        else
                        {
                            // 各行ごとにdstの行バイト幅コピー
                            var sp = (byte*)bd.Scan0;
                            var dp = (byte*)dst.Data;
                            for (int y = 0; y < h; y++)
                            {
                                Utility.CopyMemory(dp, sp, dstep);
                                sp += sstep;
                                dp += dstep;
                            }
                        }
                    }
                        break;

                    case PixelFormat.Format32bppRgb:
                    case PixelFormat.Format32bppArgb:
                    case PixelFormat.Format32bppPArgb:
                    {
                        switch (dst.Channels())
                        {
                            case 4:
                                if (!submat && continuous)
                                {
                                    var length = (uint)(dst.DataEnd.ToInt64() - dstData.ToInt64());
                                    Utility.CopyMemory(dstData, bd.Scan0, length);
                                }
                                else
                                {
                                    var sp = (byte*)bd.Scan0;
                                    var dp = (byte*)dst.Data;
                                    for (int y = 0; y < h; y++)
                                    {
                                        Utility.CopyMemory(dp, sp, dstep);
                                        sp += sstep;
                                        dp += dstep;
                                    }
                                }
                                break;
                            case 3:
                                for (int y = 0; y < h; y++)
                                {
                                    for (int x = 0; x < w; x++)
                                    {
                                        dstPtr[y * dstep + x * 3 + 0] = p[y * sstep + x * 4 + 0];
                                        dstPtr[y * dstep + x * 3 + 1] = p[y * sstep + x * 4 + 1];
                                        dstPtr[y * dstep + x * 3 + 2] = p[y * sstep + x * 4 + 2];
                                    }
                                }
                                break;
                            default:
                                throw new ArgumentException("Invalid nChannels");
                        }
                    }
                        break;
                }
            }
            finally
            {
                if (bd != null)
                {
                    src.UnlockBits(bd);
                }
            }
        }
    }
}
using System;
using OpenCvSharp.Gpu;

namespace Frdp.Common
{
    public static class StreamHelper
    {
        public static ulong ReadULong(
            this System.IO.Stream s
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            var b = new byte[sizeof(ulong)];

            s.Read(b, 0, b.Length);

            return
                BitConverter.ToUInt64(b, 0);
        }

        public static long ReadLong(
            this System.IO.Stream s
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            var b = new byte[sizeof(long)];

            s.Read(b, 0, b.Length);

            return
                BitConverter.ToInt64(b, 0);
        }

        public static uint ReadUInt(
            this System.IO.Stream s
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            var b = new byte[sizeof(uint)];

            s.Read(b, 0, b.Length);

            return
                BitConverter.ToUInt32(b, 0);
        }

        public static int ReadInt(
            this System.IO.Stream s
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            var b = new byte[sizeof (int)];

            s.Read(b, 0, b.Length);

            return
                BitConverter.ToInt32(b, 0);
        }

        public static void Write(
            this System.IO.Stream s,
            ulong data
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            s.Write(BitConverter.GetBytes(data));
        }

        public static void Write(
            this System.IO.Stream s,
            long data
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            s.Write(BitConverter.GetBytes(data));
        }

        public static void Write(
            this System.IO.Stream s,
            uint data
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            s.Write(BitConverter.GetBytes(data));
        }

        public static void Write(
            this System.IO.Stream s,
            int data
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            s.Write(BitConverter.GetBytes(data));
        }

        public static void Write(
            this System.IO.Stream s,
            byte[] data
            )
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            s.Write(data, 0, data.Length);
        }
    }
}
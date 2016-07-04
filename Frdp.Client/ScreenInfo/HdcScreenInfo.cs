using System;
using Frdp.Client.Helpers;
using Frdp.Common;

namespace Frdp.Client.ScreenInfo
{
    public class HdcScreenInfo : IScreenInfo
    {
        private readonly IntPtr _hDesk;
        private readonly IntPtr _hSrce;

        public IntPtr WindowDC
        {
            get
            {
                return
                    _hSrce;
            }
        }

        public int Left
        {
            get;
            private set;
        }

        public int Top
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public HdcScreenInfo(
            IntPtr hDesk,
            IntPtr hSrce,
            int left,
            int top,
            int width,
            int height
            )
        {
            _hDesk = hDesk;
            _hSrce = hSrce;

            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public void Dispose()
        {
            InvokeHelper.ReleaseDC(_hDesk, _hSrce);
        }


    }
}
using System;

namespace Frdp.Client.ScreenInfo
{
    public interface IScreenInfo : IDisposable
    {
        IntPtr WindowDC
        {
            get;
        }

        int Left
        {
            get;
        }

        int Top
        {
            get;
        }

        int Width
        {
            get;
        }

        int Height
        {
            get;
        }
    }
}
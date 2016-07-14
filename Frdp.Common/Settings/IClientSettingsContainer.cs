using System;

namespace Frdp.Common.Settings
{
    public interface IClientSettingsContainer
    {
        uint BlockWidth
        {
            get;
        }

        uint BlockHeight
        {
            get;
        }

        int ScaleFactorX
        {
            get;
        }

        int ScaleFactorY
        {
            get;
        }

        TimeSpan TimeoutBetweenFrames
        {
            get;
        }

        byte Mask
        {
            get;
        }
    }
}
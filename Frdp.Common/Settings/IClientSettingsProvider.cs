using System;

namespace Frdp.Common.Settings
{
    public interface IClientSettingsProvider : IClientSettingsContainer
    {
        void SetBlockSize(
            uint blockWidth,
            uint blockHeight
            );

        void SetScaleFactor(
            int scaleFactorX,
            int scaleFactorY
            );

        void SetTimeoutBetweenFrames(
            TimeSpan timeoutBetweenFrames
            );
    }
}
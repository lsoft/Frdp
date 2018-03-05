using System;

namespace Frdp.Common.Settings
{
    public interface IClientSettingsProvider : IClientSettingsContainer
    {
        void SetBlockWidth(
            uint blockWidth
            );


        void SetBlockHeight(
            uint blockHeight
            );


        //void SetBlockSize(
        //    uint blockWidth,
        //    uint blockHeight
        //    );

        void SetScaleFactorX(
            int scaleFactorX
            );

        void SetScaleFactorY(
            int scaleFactorY
            );

        //void SetScaleFactor(
        //    int scaleFactorX,
        //    int scaleFactorY
        //    );

        void SetTimeoutBetweenFrames(
            TimeSpan timeoutBetweenFrames
            );

        void SetMask(
            byte mask
            );
    }
}
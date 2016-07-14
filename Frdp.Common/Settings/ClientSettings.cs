using System;

namespace Frdp.Common.Settings
{
    public class ClientSettings : IClientSettingsProvider
    {
        public uint BlockWidth
        {
            get;
            private set;
        }

        public uint BlockHeight
        {
            get;
            private set;
        }

        public int ScaleFactorX
        {
            get;
            private set;
        }

        public int ScaleFactorY
        {
            get;
            private set;
        }

        public TimeSpan TimeoutBetweenFrames
        {
            get;
            private set;
        }

        public byte Mask
        {
            get;
            set;
        }

        public ClientSettings(
            uint blockWidth, 
            uint blockHeight, 
            int scaleFactorX, 
            int scaleFactorY, 
            TimeSpan timeoutBetweenFrames,
            byte mask
            )
        {
            BlockWidth = blockWidth;
            BlockHeight = blockHeight;
            ScaleFactorX = scaleFactorX;
            ScaleFactorY = scaleFactorY;
            TimeoutBetweenFrames = timeoutBetweenFrames;
            Mask = mask;
        }

        public ClientSettings()
        {
            BlockWidth = 64;
            BlockHeight = 64;
            ScaleFactorX = 2;
            ScaleFactorY = 2;
            TimeoutBetweenFrames = TimeSpan.Zero;
            Mask = Convert.ToByte("11110000", 2);
        }

        public void SetBlockSize(
            uint blockWidth,
            uint blockHeight
            )
        {
            if (blockWidth == 0)
            {
                throw new ArgumentException("blockWidth == 0");
            }
            if (blockHeight == 0)
            {
                throw new ArgumentException("blockHeight == 0");
            }
            if (Math.Abs(Math.Log(blockWidth, 2.0) % 1) > double.Epsilon)
            {
                throw new ArgumentException("BlockWidth must be power of two!");
            }
            if (Math.Abs(Math.Log(blockHeight, 2.0) % 1) > double.Epsilon)
            {
                throw new ArgumentException("BlockHeight must be power of two!");
            }

            BlockWidth = blockWidth;
            BlockHeight = blockHeight;
        }

        public void SetScaleFactor(
            int scaleFactorX,
            int scaleFactorY
            )
        {
            ScaleFactorX = scaleFactorX;
            ScaleFactorY = scaleFactorY;
        }

        public void SetTimeoutBetweenFrames(
            TimeSpan timeoutBetweenFrames
            )
        {
            TimeoutBetweenFrames = timeoutBetweenFrames;
        }

        public void SetMask(byte mask)
        {
            this.Mask = mask;
        }
    }
}

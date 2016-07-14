using System;
using Frdp.Common.Settings;

namespace Frdp.Client.Block.Cutter.Settings
{
    public class BlockSettings : IBlockSettings
    {
        public byte Mask
        {
            get;
            private set;
        }

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

        public int BlockHorizontalShift
        {
            get;
            private set;
        }

        public int BlockVerticalShift
        {
            get;
            private set;
        }

        public uint BlockHorizontalRightMask
        {
            get;
            private set;
        }

        public uint BlockVerticalRightMask
        {
            get;
            private set;
        }

        public uint SingleBlockSize
        {
            get;
            private set;
        }

        public BlockSettings(
            IClientSettingsContainer clientSettings
            )
        {
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }

            Mask = clientSettings.Mask;

            BlockWidth = clientSettings.BlockWidth;
            BlockHeight = clientSettings.BlockHeight;

            BlockHorizontalRightMask = (0xFFFFFFFF % clientSettings.BlockWidth);
            BlockHorizontalShift = (int)Math.Log(clientSettings.BlockWidth, 2);

            BlockVerticalRightMask = (0xFFFFFFFF % clientSettings.BlockHeight);
            BlockVerticalShift = (int)Math.Log(clientSettings.BlockHeight, 2);

            SingleBlockSize = BlockWidth * BlockHeight;
        }

        public void CalculateBlockCount(
            int imageWidth,
            int imageHeight,
            out int blockCountHorizontal,
            out int blockCountVertical,
            out int totalBlockCount
            )
        {
            blockCountHorizontal =
                (int)(imageWidth / BlockWidth)
                + (((imageWidth % BlockWidth) > 0) ? 1 : 0)
                ;

            blockCountVertical =
                (int)(imageHeight / BlockHeight)
                + (((imageHeight % BlockHeight) > 0) ? 1 : 0)
                ;

            totalBlockCount = blockCountHorizontal * blockCountVertical;
        }

        public bool IsSizeEquals(
            IBlockSettings right
            )
        {
            if (right == null)
            {
                return
                    false;
            }

            return
                this.BlockHeight == right.BlockHeight
                && this.BlockWidth == right.BlockWidth
                ;
        }
    }
}

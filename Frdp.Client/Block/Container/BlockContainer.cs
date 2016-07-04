using System;
using Frdp.Client.Block.Cutter.Settings;
using Frdp.Client.Crc;

namespace Frdp.Client.Block.Container
{
    public class BlockContainer : IBlockContainer
    {
        public IBlockSettings BlockSettings
        {
            get;
            private set;
        }

        public int ImageWidth
        {
            get;
            private set;
        }

        public int ImageHeight
        {
            get;
            private set;
        }

        public int BlockCountHorizontal
        {
            get;
            private set;
        }

        public int BlockCountVertical
        {
            get;
            private set;
        }

        public int TotalBlockCount
        {
            get;
            private set;
        }

        public byte[] BlockData
        {
            get;
            private set;
        }

        public ushort[] BlockCrc
        {
            get;
            private set;
        }

        public BlockContainer(
            IBlockSettings blockSettings,
            int imageWidth,
            int imageHeight,
            int blockCountHorizontal,
            int blockCountVertical
            )
        {
            if (blockSettings == null)
            {
                throw new ArgumentNullException("blockSettings");
            }

            BlockSettings = blockSettings;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            BlockCountHorizontal = blockCountHorizontal;
            BlockCountVertical = blockCountVertical;

            TotalBlockCount = blockCountHorizontal * blockCountVertical;

            BlockData = new byte[TotalBlockCount * blockSettings.SingleBlockSize];
            BlockCrc = new ushort[TotalBlockCount];
        }

        public uint GetIndexForBlock(
            uint blockIndex
            )
        {
            return
                blockIndex * BlockSettings.SingleBlockSize;
        }

        public bool IsImageSizeEquals(
            IBlockContainer right
            )
        {
            if (right == null)
            {
                return
                    false;
            }

            return
                this.ImageHeight == right.ImageHeight
                && this.ImageWidth == right.ImageWidth;
        }

        public void CalculateCrc(
            ICrcCalculator crcCalculator
            )
        {
            if (crcCalculator == null)
            {
                throw new ArgumentNullException("crcCalculator");
            }

            for (uint cc = 0; cc < TotalBlockCount; cc++)
            {
                var startIndex = this.GetIndexForBlock(cc);

                BlockCrc[cc] = crcCalculator.Calculate(
                    this.BlockData,
                    startIndex,
                    this.BlockSettings.SingleBlockSize
                    );
            }
        }
    }
}

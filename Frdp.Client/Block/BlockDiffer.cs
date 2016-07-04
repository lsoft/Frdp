using System;
using Frdp.Client.Block.Container;
using Frdp.Common.Block.Diff;

namespace Frdp.Client.Block
{
    public class BlockDiffer : IBlockDiffer
    {
        public BlockDiffer()
        {
        }

        public DiffContainer CalculateDiff(
            IBlockContainer left,
            IBlockContainer right
            )
        {
            //left allowed to be null
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var result = new DiffContainer(
                right.BlockSettings.BlockWidth,
                right.BlockSettings.BlockHeight,
                right.ImageWidth,
                right.ImageHeight,
                right.BlockCountHorizontal,
                right.BlockCountVertical,
                right.TotalBlockCount
                );

            //нужно ли делать полную копию
            var completeCopy = false;

            //если это первый кадр
            if (left == null)
            {
                completeCopy = true;
            }
            else
            {
                //если изменился размер блока
                if (!left.BlockSettings.IsSizeEquals(right.BlockSettings))
                {
                    completeCopy = true;
                }
                else
                {
                    //если изменилось разрешение экрана
                    if(!left.IsImageSizeEquals(right))
                    {
                        completeCopy = true;
                    }
                }
            }

            var cc = 0u;
            for (var y = 0; y < right.BlockCountVertical; y++)
            {
                for (var x = 0; x < right.BlockCountHorizontal; x++)
                {
                    if (completeCopy || (left.BlockCrc[cc] != right.BlockCrc[cc]))
                    {
                        var diff = new BlockDiff(
                            x,
                            y,
                            right.BlockData,
                            right.GetIndexForBlock(cc),
                            right.BlockSettings.SingleBlockSize
                            );

                        result.AddDiff(diff);
                    }
                    cc++;
                }
            }

            return result;
        }
    }
}

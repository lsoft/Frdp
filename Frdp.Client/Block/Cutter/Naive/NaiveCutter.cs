using System;
using Frdp.Client.Block.Container;
using OpenCvSharp;

namespace Frdp.Client.Block.Cutter.Naive
{
    /// <summary>
    /// Разделитель на блоки с простейшим алгоритмом.
    /// В 2 раза медленнее, чем самая быстрая реализация на C# (на 500 вызовов испольуется 7.7 секунд на моем ноутбуке)
    /// </summary>
    public class NaiveCutter : ICutter
    {
        private readonly IBlockContainer _blockContainer;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="blockContainer">Сами блоки</param>
        public NaiveCutter(
            IBlockContainer blockContainer
            )
        {
            if (blockContainer == null)
            {
                throw new ArgumentNullException("blockContainer");
            }

            _blockContainer = blockContainer;
        }

        public void DoCut(
            Mat m1
            )
        {
            if (m1 == null)
            {
                throw new ArgumentNullException("m1");
            }

            unsafe
            {
                fixed(byte* blockData = _blockContainer.BlockData)
                {
                    var singleBlockSize = _blockContainer.BlockSettings.SingleBlockSize;

                    var blockWidth = _blockContainer.BlockSettings.BlockWidth;
                    var blockHorizontalShift = _blockContainer.BlockSettings.BlockHorizontalShift;
                    var blockVerticalShift = _blockContainer.BlockSettings.BlockVerticalShift;
                    var blockHorizontalRightMask = _blockContainer.BlockSettings.BlockHorizontalRightMask;
                    var blockVerticalRightMask = _blockContainer.BlockSettings.BlockVerticalRightMask;

                    var blockCountHorizontal = _blockContainer.BlockCountHorizontal;

                    var mask = _blockContainer.BlockSettings.Mask;

                    uint cc = 0;
                    for (uint h = 0; h < m1.Height; h++)
                    {
                        //var vbi = (h & BlockVerticalLeftMask) >> BlockVerticalShift;
                        var vbi = h >> blockVerticalShift;
                        var bi = vbi*blockCountHorizontal + 0;

                        var inbiindex = (h & blockVerticalRightMask)*blockWidth;

                        for (uint w = 0; w < m1.Width; w++)
                        {
                            var a = m1.DataPointer[cc] & mask;

                            //var hbi = (w & BlockHorizontalLeftMask) >> BlockHorizontalShift;
                            var hbi = w >> blockHorizontalShift;
                            var concretebi = bi + hbi;

                            var concreteinbiindex = inbiindex + (w & blockHorizontalRightMask);

                            blockData[concretebi*singleBlockSize + concreteinbiindex] = (byte) a;

                            cc++;
                        }
                    }
                }
            }
        }

    }
}

using System;
using Frdp.Client.Block.Container;
using OpenCvSharp;

namespace Frdp.Client.Block.Cutter.Efficient
{
    /// <summary>
    /// Разделитель на блоки с эффективным алгоритмом
    /// (на 500 вызовов испольуется 4.0 секунд на моем ноутбуке)
    /// </summary>
    public class EfficientCutter : ICutter
    {
        private readonly IBlockContainer _blockContainer;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="blockContainer">Сами блоки</param>
        public EfficientCutter(
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
                    var blockVerticalShift = _blockContainer.BlockSettings.BlockVerticalShift;
                    var blockVerticalRightMask = _blockContainer.BlockSettings.BlockVerticalRightMask;

                    var blockCountHorizontal = _blockContainer.BlockCountHorizontal;

                    var mask = _blockContainer.BlockSettings.Mask;

                    uint cc = 0;
                    for (uint h = 0; h < m1.Height; h++)
                    {
                        var vbi = h >> blockVerticalShift;
                        var bi = vbi*blockCountHorizontal + 0;

                        var preleft = (h & blockVerticalRightMask)*blockWidth;

                        for (uint hbi = 0; hbi < blockCountHorizontal; hbi++)
                        {
                            var w = hbi*blockWidth;

                            var wright = Math.Min(
                                (long) ((hbi + 1)*blockWidth),
                                (long) m1.Width
                                );

                            var concretebi = bi + hbi;

                            var left = preleft + concretebi*singleBlockSize;

                            var right = left + (wright - w);

                            for (var concreteinbiindex = left; concreteinbiindex < right; concreteinbiindex++)
                            {
                                var a = m1.DataPointer[cc] & mask;

                                blockData[concreteinbiindex] = (byte) a;

                                cc++;
                            }
                        }
                    }
                }
            }
        }

    }
}

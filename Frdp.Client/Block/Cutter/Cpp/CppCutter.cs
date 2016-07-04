using System;
using System.Runtime.InteropServices;
using Frdp.Client.Block.Container;
using OpenCvSharp;

namespace Frdp.Client.Block.Cutter.Cpp
{
    /// <summary>
    /// Разделитель на блоки с алгоритмом, написанном на C++
    /// (на 500 вызовов испольуется 0.21 секунд на моем ноутбуке)
    /// </summary>
    public class CppCutter : ICutter
    {
        private readonly IBlockContainer _blockContainer;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="blockContainer">Сами блоки</param>
        public CppCutter(
            IBlockContainer blockContainer
            )
        {
            if (blockContainer == null)
            {
                throw new ArgumentNullException("blockContainer");
            }

            _blockContainer = blockContainer;
        }

        public void DoCut(Mat m1)
        {
            unsafe
            {
                fixed (byte* blockData = _blockContainer.BlockData)
                {
                    DoCut(
                        _blockContainer.BlockSettings.Mask,

                        _blockContainer.BlockSettings.BlockWidth,
                        _blockContainer.BlockSettings.BlockHeight,

                        _blockContainer.BlockCountHorizontal,
                        _blockContainer.BlockCountVertical,

                        _blockContainer.BlockSettings.BlockHorizontalShift,
                        _blockContainer.BlockSettings.BlockVerticalShift,

                        _blockContainer.BlockSettings.BlockHorizontalRightMask,
                        _blockContainer.BlockSettings.BlockVerticalRightMask,

                        new IntPtr(blockData),
                        new IntPtr(m1.DataPointer), 

                        m1.Height,
                        m1.Width
                        );
                }
            }
        }

        [DllImport("PerformanceLibrary.dll")]
        public static extern int DoCut(
            byte mask,

            uint blockWidth,
            uint blockHeight,

            int blockCountHorizontal,
            int blockCountVertical,

            int blockHorizontalShift,
            int blockVerticalShift,

            uint blockHorizontalRightMask,
            uint blockVerticalRightMask,

            IntPtr blockData,

            IntPtr mat,

            int m1_Height,
            int m1_Width
            );

    }
}

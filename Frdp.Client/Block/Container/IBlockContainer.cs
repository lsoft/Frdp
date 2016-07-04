using Frdp.Client.Block.Cutter.Settings;

namespace Frdp.Client.Block.Container
{
    public interface IBlockContainer
    {
        IBlockSettings BlockSettings
        {
            get;
        }

        /// <summary>
        /// »сходный размер захватываемого изображени€ по горизонтали
        /// (может быть меньше, чем BlockCountHorizontal * BlockWidth)
        /// </summary>
        int ImageWidth
        {
            get;
        }

        /// <summary>
        /// »сходный размер захватываемого изображени€ по вертикали
        /// (может быть меньше, чем BlockCountVertical * BlockHeight)
        /// </summary>
        int ImageHeight
        {
            get;
        }

        /// <summary>
        ///  оличество блоков по горизонтали
        /// </summary>
        int BlockCountHorizontal
        {
            get;
        }

        /// <summary>
        ///  оличество блоков по вертикали
        /// </summary>
        int BlockCountVertical
        {
            get;
        }

        /// <summary>
        /// ќбщее количество блоков
        /// </summary>
        int TotalBlockCount
        {
            get;
        }

        /// <summary>
        /// —одержимое всех блоков
        /// </summary>
        byte[] BlockData
        {
            get;
        }

        /// <summary>
        /// ’ранилище CRC сумм блоков
        /// </summary>
        ushort[] BlockCrc
        {
            get;
        }

        uint GetIndexForBlock(
            uint blockIndex
            );

        bool IsImageSizeEquals(
            IBlockContainer right
            );
    }
}
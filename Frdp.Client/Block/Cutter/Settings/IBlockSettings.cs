namespace Frdp.Client.Block.Cutter.Settings
{
    public interface IBlockSettings
    {
        /// <summary>
        /// Маска, накладываемая на картинку перед сохранением в блок
        /// </summary>
        byte Mask
        {
            get;
        }

        /// <summary>
        /// Длина блока в пикселях
        /// </summary>
        uint BlockWidth
        {
            get;
        }

        /// <summary>
        /// Высота блока в пикселях
        /// </summary>
        uint BlockHeight
        {
            get;
        }

        /// <summary>
        /// На сколько битов сдвигать X координату пикселя картинки, чтобы получить X координату самого блока
        /// </summary>
        int BlockHorizontalShift
        {
            get;
        }

        /// <summary>
        /// На сколько битов сдвигать Y координату пикселя картинки, чтобы получить Y координату самого блока
        /// </summary>
        int BlockVerticalShift
        {
            get;
        }

        /// <summary>
        /// Маска для X координаты пикселя картинки, чтобы получить X координату пикселя внутри блока
        /// </summary>
        uint BlockHorizontalRightMask
        {
            get;
        }

        /// <summary>
        /// Маска для Y координаты пикселя картинки, чтобы получить Y координату пикселя внутри блока
        /// </summary>
        uint BlockVerticalRightMask
        {
            get;
        }

        /// <summary>
        /// Размер блока в байтах
        /// </summary>
        uint SingleBlockSize
        {
            get;
        }

        void CalculateBlockCount(
            int imageWidth,
            int imageHeight,
            out int blockCountHorizontal,
            out int blockCountVertical,
            out int totalBlockCount
            );

        bool IsSizeEquals(
            IBlockSettings blockSettings
            );
    }
}
namespace Frdp.Client.Block.Cutter.Settings
{
    public interface IBlockSettings
    {
        /// <summary>
        /// �����, ������������� �� �������� ����� ����������� � ����
        /// </summary>
        byte Mask
        {
            get;
        }

        /// <summary>
        /// ����� ����� � ��������
        /// </summary>
        uint BlockWidth
        {
            get;
        }

        /// <summary>
        /// ������ ����� � ��������
        /// </summary>
        uint BlockHeight
        {
            get;
        }

        /// <summary>
        /// �� ������� ����� �������� X ���������� ������� ��������, ����� �������� X ���������� ������ �����
        /// </summary>
        int BlockHorizontalShift
        {
            get;
        }

        /// <summary>
        /// �� ������� ����� �������� Y ���������� ������� ��������, ����� �������� Y ���������� ������ �����
        /// </summary>
        int BlockVerticalShift
        {
            get;
        }

        /// <summary>
        /// ����� ��� X ���������� ������� ��������, ����� �������� X ���������� ������� ������ �����
        /// </summary>
        uint BlockHorizontalRightMask
        {
            get;
        }

        /// <summary>
        /// ����� ��� Y ���������� ������� ��������, ����� �������� Y ���������� ������� ������ �����
        /// </summary>
        uint BlockVerticalRightMask
        {
            get;
        }

        /// <summary>
        /// ������ ����� � ������
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
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
        /// �������� ������ �������������� ����������� �� �����������
        /// (����� ���� ������, ��� BlockCountHorizontal * BlockWidth)
        /// </summary>
        int ImageWidth
        {
            get;
        }

        /// <summary>
        /// �������� ������ �������������� ����������� �� ���������
        /// (����� ���� ������, ��� BlockCountVertical * BlockHeight)
        /// </summary>
        int ImageHeight
        {
            get;
        }

        /// <summary>
        /// ���������� ������ �� �����������
        /// </summary>
        int BlockCountHorizontal
        {
            get;
        }

        /// <summary>
        /// ���������� ������ �� ���������
        /// </summary>
        int BlockCountVertical
        {
            get;
        }

        /// <summary>
        /// ����� ���������� ������
        /// </summary>
        int TotalBlockCount
        {
            get;
        }

        /// <summary>
        /// ���������� ���� ������
        /// </summary>
        byte[] BlockData
        {
            get;
        }

        /// <summary>
        /// ��������� CRC ���� ������
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
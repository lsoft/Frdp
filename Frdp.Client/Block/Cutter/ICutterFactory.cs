using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter
{
    public interface ICutterFactory
    {
        /// <summary>
        /// ������� ����������� �� �����
        /// </summary>
        /// <param name="blockContainer">���� �����</param>
        ICutter CreateCutter(
            IBlockContainer blockContainer
            );

    }
}
using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter.Efficient
{
    public class EfficientCutterFactory : ICutterFactory
    {
        /// <summary>
        /// ������� ����������� �� �����
        /// </summary>
        /// <param name="blockContainer">���� �����</param>
        public ICutter CreateCutter(IBlockContainer blockContainer
            )
        {
            return
                new EfficientCutter(
                    blockContainer
                    );
        }
    }
}
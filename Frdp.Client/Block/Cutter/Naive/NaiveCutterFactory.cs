using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter.Naive
{
    public class NaiveCutterFactory : ICutterFactory
    {
        /// <summary>
        /// ������� ����������� �� �����
        /// </summary>
        /// <param name="blockContainer">���� �����</param>
        public ICutter CreateCutter(IBlockContainer blockContainer
            )
        {
            return 
                new NaiveCutter(
                    blockContainer
                    );
        }
    }
}
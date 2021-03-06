using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter.Cpp
{
    public class CppCutterFactory : ICutterFactory
    {
        /// <summary>
        /// ������� ����������� �� �����
        /// </summary>
        /// <param name="blockContainer">���� �����</param>
        public ICutter CreateCutter(IBlockContainer blockContainer
            )
        {
            return
                new CppCutter(
                    blockContainer
                    );
        }
    }
}
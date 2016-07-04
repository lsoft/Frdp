using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter.Cpp
{
    public class CppCutterFactory : ICutterFactory
    {
        /// <summary>
        /// Создать разделитель на блоки
        /// </summary>
        /// <param name="blockContainer">Сами блоки</param>
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
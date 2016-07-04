using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter.Efficient
{
    public class EfficientCutterFactory : ICutterFactory
    {
        /// <summary>
        /// Создать разделитель на блоки
        /// </summary>
        /// <param name="blockContainer">Сами блоки</param>
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
using Frdp.Client.Block.Container;

namespace Frdp.Client.Block.Cutter
{
    public interface ICutterFactory
    {
        /// <summary>
        /// Создать разделитель на блоки
        /// </summary>
        /// <param name="blockContainer">Сами блоки</param>
        ICutter CreateCutter(
            IBlockContainer blockContainer
            );

    }
}
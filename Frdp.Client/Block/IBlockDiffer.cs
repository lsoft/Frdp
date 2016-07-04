using Frdp.Client.Block.Container;
using Frdp.Common.Block.Diff;

namespace Frdp.Client.Block
{
    public interface IBlockDiffer
    {
        DiffContainer CalculateDiff(
            IBlockContainer left,
            IBlockContainer right
            );
    }
}
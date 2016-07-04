using Frdp.Common.Block.Diff;

namespace Frdp.Server.Applier
{
    public interface IDiffApplier
    {
        void Apply(
            DiffContainer diffContainer
            );
    }
}
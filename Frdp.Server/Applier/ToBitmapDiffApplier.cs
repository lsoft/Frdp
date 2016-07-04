using System;
using Frdp.Common.Block.Diff;
using Frdp.Server.Bitmap;

namespace Frdp.Server.Applier
{
    public class ToBitmapDiffApplier : IDiffApplier
    {
        private readonly DirectBitmapContainer _container;

        public ToBitmapDiffApplier(
            DirectBitmapContainer container
            )
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        public void Apply(
            DiffContainer diffContainer
            )
        {
            if (diffContainer == null)
            {
                throw new ArgumentNullException("diffContainer");
            }

            _container.SetSize(
                diffContainer.ImageWidth,
                diffContainer.ImageHeight
                );

            foreach (var diff in diffContainer.Diffs)
            {
                _container.TakeBlock(
                    diff,
                    diffContainer.BlockWidth,
                    diffContainer.BlockHeight
                    );
            }

            _container.MarkImageChanged();
        }
    }
}

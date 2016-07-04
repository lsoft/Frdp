using System;
using Frdp.Common.Block.Diff;
using Frdp.Common.Command;

namespace Frdp.Client.Channel
{
    public interface IChannel : IDisposable
    {
        ICommandContainer SendFrame(
            DiffContainer diffContainer
            );
    }
}

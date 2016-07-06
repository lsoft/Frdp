using System;
using Frdp.Common.Block.Diff;
using Frdp.Common.Command;

namespace Frdp.Client.Channel.MainChannel
{
    public interface IMainChannel : IDisposable
    {
        ICommandContainer SendFrame(
            DiffContainer diffContainer
            );
    }
}

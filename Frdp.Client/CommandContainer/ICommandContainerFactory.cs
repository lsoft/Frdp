using Frdp.Common.Command;
using Frdp.Wcf;
using Frdp.Wcf.Contract;
using Frdp.Wcf.Contract.MainChannel;

namespace Frdp.Client.CommandContainer
{
    public interface ICommandContainerFactory
    {
        ICommandContainer CreateCommandContainer(
            ServerCommands serverCommands
            );
    }
}
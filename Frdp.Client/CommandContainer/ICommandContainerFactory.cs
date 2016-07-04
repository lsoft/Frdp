using Frdp.Common.Command;
using Frdp.Wcf;
using Frdp.Wcf.Contract;

namespace Frdp.Client.CommandContainer
{
    public interface ICommandContainerFactory
    {
        ICommandContainer CreateCommandContainer(
            ServerCommands serverCommands
            );
    }
}
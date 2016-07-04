using Frdp.Common.Command;

namespace Frdp.Client.CommandExecutor
{
    internal interface ICommandExecutor
    {
        void Execute(BaseCommand command);
    }

    internal interface ICommandExecutor<T>
        where T : BaseCommand
    {
        void Execute(
            T command
            );
    }
}
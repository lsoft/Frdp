using Frdp.Common.Command;

namespace Frdp.Client.CommandContainer
{
    public interface ICommandFactory
    {
        BaseCommand CreateCommand(
            string type,
            object argument
            );
    }
}
using Frdp.Wcf.Contract.MainChannel;

namespace Frdp.Server.Wcf.MainChannel.Result
{
    public interface IWcfResultFactory
    {
        ServerCommands CreateServerCommands(
            );
    }
}
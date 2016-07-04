using Frdp.Wcf;
using Frdp.Wcf.Contract;

namespace Frdp.Server.Wcf.Result
{
    public interface IWcfResultFactory
    {
        ServerCommands CreateServerCommands(
            );
    }
}
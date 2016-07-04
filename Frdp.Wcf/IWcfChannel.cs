using System.ServiceModel;
using Frdp.Wcf.Contract;

namespace Frdp.Wcf
{
    [ServiceContract]
    public interface IWcfChannel
    {
        [OperationContract]
        ServerCommands ExecuteRequest(Packet request);
    }
}
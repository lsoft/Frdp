using System.ServiceModel;

namespace Frdp.Wcf.Contract.MainChannel
{
    [ServiceContract]
    public interface IWcfMainChannel
    {
        [OperationContract]
        ServerCommands ExecuteRequest(Packet request);
    }
}
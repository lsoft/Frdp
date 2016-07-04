using System.ServiceModel;

namespace Frdp.Server.Wcf
{
    public interface IWCFListener
    {
        void StartListen(
            EndpointAddress endpointAddress
            );

        void StopListen();
    }
}
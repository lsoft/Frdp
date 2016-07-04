using System.ServiceModel;

namespace Frdp.Wcf.Endpoint
{
    public interface IEndpointProvider
    {
        EndpointAddress GetEndpointAddress();
    }
}
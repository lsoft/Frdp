using System.ServiceModel;

namespace Frdp.Wcf.Endpoint
{
    public class EndpointContainer : IEndpointContainer, IEndpointProvider
    {
        public string EndpointAddress
        {
            get;
            set;
        }

        public EndpointContainer(
            )
        {
        }

        public EndpointContainer(
            string endpointAddress
            )
        {
            EndpointAddress = endpointAddress;
        }

        public EndpointAddress GetEndpointAddress()
        {
            return
                new EndpointAddress(EndpointAddress);
        }
    }
}

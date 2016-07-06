using System;
using System.ServiceModel.Channels;
using Frdp.Client.Channel.FileChannel;
using Frdp.Wcf.Endpoint;

namespace Frdp.Client.Wcf.FileChannel
{
    public class WcfFileChannelFactory : IFileChannelFactory
    {
        private readonly IBindingProvider _bindingProvider;
        private readonly IEndpointProvider _endpointProvider;

        public WcfFileChannelFactory(
            IBindingProvider bindingProvider,
            IEndpointProvider endpointProvider
            )
        {
            if (bindingProvider == null)
            {
                throw new ArgumentNullException("bindingProvider");
            }
            if (endpointProvider == null)
            {
                throw new ArgumentNullException("endpointProvider");
            }

            _bindingProvider = bindingProvider;
            _endpointProvider = endpointProvider;
        }

        public IFileChannel OpenChannel()
        {
            var result = new WcfFileChannel(
                _bindingProvider,
                _endpointProvider
                );

            return
                result;
        }
    }
}
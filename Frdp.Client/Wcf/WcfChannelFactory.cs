using System;
using System.ServiceModel.Channels;
using Frdp.Client.CommandContainer;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;
using IChannel = Frdp.Client.Channel.IChannel;
using IChannelFactory = Frdp.Client.Channel.IChannelFactory;

namespace Frdp.Client.Wcf
{
    public class WcfChannelFactory : Channel.IChannelFactory
    {
        private readonly CustomBinding _binding;
        private readonly IEndpointProvider _endpointProvider;
        private readonly ICommandContainerFactory _commandContainerFactory;

        public WcfChannelFactory(
            CustomBinding binding,
            IEndpointProvider endpointProvider,
            ICommandContainerFactory commandContainerFactory
            )
        {
            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }
            if (endpointProvider == null)
            {
                throw new ArgumentNullException("endpointProvider");
            }
            if (commandContainerFactory == null)
            {
                throw new ArgumentNullException("commandContainerFactory");
            }

            _binding = binding;
            _endpointProvider = endpointProvider;
            _commandContainerFactory = commandContainerFactory;
        }

        public IChannel OpenChannel()
        {
            var result = new WcfChannel(
                _binding,
                _endpointProvider,
                _commandContainerFactory
                );

            return
                result;
        }
    }
}
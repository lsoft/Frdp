using System;
using System.ServiceModel.Channels;
using Frdp.Client.Channel;
using Frdp.Client.Channel.MainChannel;
using Frdp.Client.CommandContainer;
using Frdp.Wcf.Endpoint;

namespace Frdp.Client.Wcf.MainChannel
{
    public class WcfMainChannelFactory : IMainChannelFactory
    {
        private readonly IBindingProvider _bindingProvider;
        private readonly IEndpointProvider _endpointProvider;
        private readonly ICommandContainerFactory _commandContainerFactory;

        public WcfMainChannelFactory(
            IBindingProvider bindingProvider,
            IEndpointProvider endpointProvider,
            ICommandContainerFactory commandContainerFactory
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
            if (commandContainerFactory == null)
            {
                throw new ArgumentNullException("commandContainerFactory");
            }

            _bindingProvider = bindingProvider;
            _endpointProvider = endpointProvider;
            _commandContainerFactory = commandContainerFactory;
        }

        public IMainChannel OpenChannel()
        {
            var result = new WcfMainChannel(
                _bindingProvider,
                _endpointProvider,
                _commandContainerFactory
                );

            return
                result;
        }
    }
}
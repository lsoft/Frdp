using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Frdp.Client.Channel.MainChannel;
using Frdp.Client.CommandContainer;
using Frdp.Common.Block.Diff;
using Frdp.Common.Command;
using Frdp.Wcf;
using Frdp.Wcf.Contract;
using Frdp.Wcf.Contract.MainChannel;
using Frdp.Wcf.Endpoint;

namespace Frdp.Client.Wcf.MainChannel
{
    public class WcfMainChannel : IMainChannel
    {
        private readonly ICommandContainerFactory _commandContainerFactory;
        private volatile bool _disposed = false;
        
        private readonly ChannelFactory<IWcfMainChannel> _factory;
        private readonly IWcfMainChannel _channel;

        public WcfMainChannel(
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

            _commandContainerFactory = commandContainerFactory;

            var endpoint = endpointProvider.GetEndpointAddress();

            //создаем соединение
            var binding = bindingProvider.CreateBinding();
            _factory = new ChannelFactory<IWcfMainChannel>(binding);
            _channel = _factory.CreateChannel(endpoint);

            if ((IContextChannel)_channel != null)
            {
                ((IContextChannel)_channel).OperationTimeout = new TimeSpan(0, 10, 0);

            }

            //открываем соединение
            try
            {
                ((System.ServiceModel.IClientChannel)_channel).Open();
            }
            catch (Exception excp)
            {
                throw;
            }
        }

        public ICommandContainer SendFrame(
            DiffContainer diffContainer
            )
        {
            var settings = new WcfClientSettings(
                diffContainer.BlockWidth,
                diffContainer.BlockHeight,
                diffContainer.ImageWidth,
                diffContainer.ImageHeight,
                diffContainer.BlockCountHorizontal,
                diffContainer.BlockCountVertical,
                diffContainer.TotalBlockCount
                );

            var diffs = diffContainer.Diffs.ConvertAll(
                j => new Diff(j.X, j.Y, j.ExtractData())
                );

            var packet = new Packet(
                settings,
                diffs
                );

            var serverCommands = _channel.ExecuteRequest(packet);

            var result = _commandContainerFactory.CreateCommandContainer(
                serverCommands
                );

            return
                result;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                var c = ((System.ServiceModel.IClientChannel)_channel);

                try
                {
                    if (c.State != CommunicationState.Faulted && c.State != CommunicationState.Closed && c.State != CommunicationState.Closing)
                    {
                        c.Close();
                    }
                }
                catch (Exception excp)
                {
                    c.Abort();
                }

                try
                {
                    if (_factory.State != CommunicationState.Faulted && _factory.State != CommunicationState.Closed && _factory.State != CommunicationState.Closing)
                    {
                        _factory.Close();
                    }
                }
                catch (Exception excp)
                {
                    _factory.Abort();
                }

                _disposed = true;
            }
        }
    }
}

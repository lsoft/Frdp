using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Frdp.Client.Channel.FileChannel;
using Frdp.Wcf;
using Frdp.Wcf.Contract.FileChannel;
using Frdp.Wcf.Endpoint;

namespace Frdp.Client.Wcf.FileChannel
{
    public class WcfFileChannel : IFileChannel
    {
        private volatile bool _disposed = false;

        private readonly ChannelFactory<IWcfFileChannel> _factory;
        private readonly IWcfFileChannel _channel;

        public WcfFileChannel(
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

            var endpointAddress = endpointProvider.GetEndpointAddress();

            //создаем соединение
            var binding = bindingProvider.CreateBinding();
            _factory = new ChannelFactory<IWcfFileChannel>(binding);

            var ub = new UriBuilder(endpointAddress.Uri);
            ub.Port += 100;
            var correctedEndpoint = new EndpointAddress(ub.Uri);

            _channel = _factory.CreateChannel(correctedEndpoint);

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

        public byte[] GetData(string filepath, long offset, int length)
        {
            return
                _channel.GetData(
                    filepath,
                    offset,
                    length
                    );
        }

        public void AppendData(
            string filepath,
            bool forceToCreateFolder,
            byte[] data
            )
        {
            _channel.AppendData(
                filepath,
                forceToCreateFolder,
                data
                );
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

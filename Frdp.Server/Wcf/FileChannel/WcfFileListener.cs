using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Extensions.Compression;
using System.Xml;
using Frdp.Common;
using Frdp.Wcf.Contract.FileChannel;

namespace Frdp.Server.Wcf.FileChannel
{
    public class WcfFileListener : IDisposable, IWCFListener
    {
        private readonly IServiceBehavior _behaviour;
        private readonly ILogger _logger;
        
        private ServiceHost _serviceHost;

        private volatile bool _started = false;

        public WcfFileListener(
            IServiceBehavior behaviour,
            ILogger logger
            )
        {
            if (behaviour == null)
            {
                throw new ArgumentNullException("behaviour");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _behaviour = behaviour;
            _logger = logger;
        }

        public void StartListen(
            EndpointAddress endpointAddress
            )
        {
            if (endpointAddress == null)
            {
                throw new ArgumentNullException("endpointAddress");
            }

            if (_started)
            {
                return;
            }

            //создаем службу

            var tcpTransport = new TcpTransportBindingElement();
            tcpTransport.MaxReceivedMessageSize = 1024 * 1024 * 128;
            tcpTransport.MaxBufferPoolSize = 1024 * 1024 * 10;
            //tcpTransport.TransferMode = TransferMode.Buffered;

            var messageEncoder = new BinaryMessageEncodingBindingElement();
            messageEncoder.ReaderQuotas = XmlDictionaryReaderQuotas.Max;

            var compressionElement = new CompressionMessageEncodingBindingElement(
                messageEncoder,
                CompressionAlgorithm.GZip
                );

            var binding = new CustomBinding(
                compressionElement,
                tcpTransport
                );

            //binding.OpenTimeout = new TimeSpan(0, 30, 0);
            //binding.SendTimeout = new TimeSpan(0, 30, 0);
            //binding.CloseTimeout = new TimeSpan(0, 30, 0);
            //binding.ReceiveTimeout = new TimeSpan(0, 30, 0);

            var ub = new UriBuilder(endpointAddress.Uri);
            ub.Port += 100;

            _serviceHost = new ServiceHost(typeof(WcfFileService), new Uri[] { });
            _serviceHost.AddServiceEndpoint(typeof(IWcfFileChannel), binding, ub.Uri);
            _serviceHost.Description.Behaviors.Add(_behaviour);

            _serviceHost.Open();

            _started = true;
        }

        public void StopListen()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            try
            {
                if (_serviceHost != null)
                {
                    _serviceHost.Close();
                }
            }
            catch (Exception excp)
            {
                if (_serviceHost != null)
                {
                    _serviceHost.Abort();
                }

                _logger.LogException(excp);
            }
        }

    }
}
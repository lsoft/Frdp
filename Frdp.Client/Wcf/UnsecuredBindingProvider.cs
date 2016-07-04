using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Extensions.Compression;
using System.Xml;

namespace Frdp.Client.Wcf
{
    public class UnsecuredBindingProvider : IBindingProvider
    {
        public CustomBinding CreateBinding()
        {
            var tcpTransport = new TcpTransportBindingElement();
            tcpTransport.MaxReceivedMessageSize = 1024 * 1024 * 10;
            tcpTransport.MaxBufferSize = 1024 * 1024 * 10;
            tcpTransport.MaxBufferPoolSize = 1024 * 1024 * 10;
            tcpTransport.TransferMode = TransferMode.Streamed;

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

            //binding.SendTimeout = new TimeSpan(0, 5, 0);
            //binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            //binding.OpenTimeout = new TimeSpan(0, 5, 0);
            //binding.CloseTimeout = new TimeSpan(0, 5, 0);

            return binding;
        }
    }
}
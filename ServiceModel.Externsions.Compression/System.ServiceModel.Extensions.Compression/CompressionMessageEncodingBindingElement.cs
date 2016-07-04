using System.ServiceModel.Channels;
using System.Xml;

#if !WindowsCE
using System.ServiceModel.Description;
#endif


namespace System.ServiceModel.Extensions.Compression
{
    public sealed class CompressionMessageEncodingBindingElement : MessageEncodingBindingElement
#if !WindowsCE
                                                                   , IPolicyExportExtension
#endif
    {
        private MessageEncodingBindingElement _innerBindingElement;

        private CompressionAlgorithm _compressionAlgorithm;

        private long _maxReceivedMessageSize;


        public CompressionMessageEncodingBindingElement()
            : this(new TextMessageEncodingBindingElement(), CompressionAlgorithm.GZip)
        {
        }


        public CompressionMessageEncodingBindingElement(
            MessageEncodingBindingElement messageEncoderBindingElement,
            CompressionAlgorithm compressionAlgorithm)
        {
            _innerBindingElement = messageEncoderBindingElement;
            _compressionAlgorithm = compressionAlgorithm;
        }


        public MessageEncodingBindingElement InnerMessageEncodingBindingElement
        {
            get { return _innerBindingElement; }
            set { _innerBindingElement = value; }
        }


        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new CompressionMessageEncoderFactory(
                _innerBindingElement.CreateMessageEncoderFactory(), _compressionAlgorithm, _maxReceivedMessageSize);
        }


        public override MessageVersion MessageVersion
        {
            get { return _innerBindingElement.MessageVersion; }
            set { _innerBindingElement.MessageVersion = value; }
        }


        public CompressionAlgorithm CompressionAlgorithm
        {
            get { return _compressionAlgorithm; }
            set { _compressionAlgorithm = value; }
        }


        public override BindingElement Clone()
        {
            return new CompressionMessageEncodingBindingElement(_innerBindingElement, _compressionAlgorithm);
        }


        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof (T) == typeof (XmlDictionaryReaderQuotas))
            {
                return _innerBindingElement.GetProperty<T>(context);
            }
            return base.GetProperty<T>(context);
        }


        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            SetMaxReceivedMessageSizeFromTransport(context);
            return context.BuildInnerChannelFactory<TChannel>();
        }


        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            SetMaxReceivedMessageSizeFromTransport(context);
            return context.BuildInnerChannelListener<TChannel>();
        }

        private void SetMaxReceivedMessageSizeFromTransport(BindingContext context)
        {
            var transport = context.Binding.Elements.Find<TransportBindingElement>();
            if (transport != null)
            {
                _maxReceivedMessageSize = transport.MaxReceivedMessageSize;
            }
        }


        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }


#if !WindowsCE
        void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext policyContext)
        {
            if (policyContext == null)
            {
                throw new ArgumentNullException("policyContext");
            }
            var document = new XmlDocument();
            policyContext.GetBindingAssertions()
                .Add(
                    document.CreateElement(
                        CompressionMessageEncodingPolicyConstants.CompressionEncodingPrefix,
                        CompressionMessageEncodingPolicyConstants.CompressionEncodingName,
                        CompressionMessageEncodingPolicyConstants.CompressionEncodingNamespace));
        }
#endif
    }
}
#if !WindowsCE
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;


namespace System.ServiceModel.Extensions.Compression
{
    public class CompressionMessageEncodingElement : BindingElementExtensionElement
    {
        public override Type BindingElementType
        {
            get { return typeof (CompressionMessageEncodingBindingElement); }
        }


        [ConfigurationProperty("compressionAlgorithm", DefaultValue = CompressionAlgorithm.GZip)]
        public CompressionAlgorithm CompressionAlgorithm
        {
            get { return (CompressionAlgorithm) base["compressionAlgorithm"]; }
            set { base["compressionAlgorithm"] = value; }
        }

        [ConfigurationProperty("innerMessageEncoding", DefaultValue = "textMessageEncoding")]
        public string InnerMessageEncoding
        {
            get { return (string) base["innerMessageEncoding"]; }
            set { base["innerMessageEncoding"] = value; }
        }


        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            var binding = (CompressionMessageEncodingBindingElement) bindingElement;
            PropertyInformationCollection propertyInfo = ElementInformation.Properties;
            
            if (propertyInfo["innerMessageEncoding"].ValueOrigin != PropertyValueOrigin.Default)
            {
                switch (InnerMessageEncoding)
                {
                    case "textMessageEncoding":
                        binding.InnerMessageEncodingBindingElement = new TextMessageEncodingBindingElement();
                        break;
                    case "binaryMessageEncoding":
                        binding.InnerMessageEncodingBindingElement = new BinaryMessageEncodingBindingElement();
                        break;
                }
            }

            if (propertyInfo["compressionAlgorithm"].ValueOrigin != PropertyValueOrigin.Default)
            {
                binding.CompressionAlgorithm = (CompressionAlgorithm) propertyInfo["compressionAlgorithm"].Value;
            }
        }


        protected override BindingElement CreateBindingElement()
        {
            var bindingElement = new CompressionMessageEncodingBindingElement();
            ApplyConfiguration(bindingElement);
            return bindingElement;
        }
    }
}


#endif
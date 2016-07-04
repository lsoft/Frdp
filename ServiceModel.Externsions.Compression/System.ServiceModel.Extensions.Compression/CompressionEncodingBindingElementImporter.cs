#if !WindowsCE
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Xml;


namespace System.ServiceModel.Extensions.Compression
{
    public class CompressionEncodingBindingElementImporter : IPolicyImportExtension
    {
        void IPolicyImportExtension.ImportPolicy(MetadataImporter importer, PolicyConversionContext context)
        {
            if (importer == null)
            {
                throw new ArgumentNullException("importer");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ICollection<XmlElement> assertions = context.GetBindingAssertions();
            foreach (XmlElement assertion in assertions)
            {
                if ((assertion.NamespaceURI == CompressionMessageEncodingPolicyConstants.CompressionEncodingNamespace)
                    && (assertion.LocalName == CompressionMessageEncodingPolicyConstants.CompressionEncodingName))
                {
                    assertions.Remove(assertion);
                    context.BindingElements.Add(new CompressionMessageEncodingBindingElement());
                    break;
                }
            }
        }
    }
}


#endif
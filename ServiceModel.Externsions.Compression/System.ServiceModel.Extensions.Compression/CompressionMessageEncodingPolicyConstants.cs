#if !WindowsCE
namespace System.ServiceModel.Extensions.Compression
{
    static class CompressionMessageEncodingPolicyConstants
    {
        public const string CompressionEncodingName = "CompressionEncoding";
        public const string CompressionEncodingNamespace = "http://schemas.ppk.ru/02/2015/policy/wcfcompress";
        public const string CompressionEncodingPrefix = "compress";
    }
}
#endif
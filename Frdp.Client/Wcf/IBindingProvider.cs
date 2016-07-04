using System.ServiceModel.Channels;

namespace Frdp.Client.Wcf
{
    public interface IBindingProvider
    {
        CustomBinding CreateBinding();
    }
}
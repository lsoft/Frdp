namespace Frdp.Client.Channel
{
    public interface IChannelFactory
    {
        IChannel OpenChannel();
    }
}
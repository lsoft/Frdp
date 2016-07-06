namespace Frdp.Client.NetworkWorker.MainChannel
{
    public interface IMainChannelWorker
    {
        void AsyncStart(
            );

        void SyncStop(
            );
    }
}
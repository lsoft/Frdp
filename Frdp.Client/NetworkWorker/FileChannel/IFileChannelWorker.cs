namespace Frdp.Client.NetworkWorker.FileChannel
{
    public interface IFileChannelWorker
    {
        void AsyncStart(
            );

        void SyncStop(
            );
    }
}
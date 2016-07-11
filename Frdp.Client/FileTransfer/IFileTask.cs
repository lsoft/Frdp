using Frdp.Client.Channel.FileChannel;

namespace Frdp.Client.FileTransfer
{
    public interface IFileTask : IFileTaskIdentifier
    {
        event TaskChangedDelegate TaskChangeEvent;

        bool IsDownloadTask
        {
            get;
        }

        void SafelyDelete(
            );

        void ForceToClose(
            );

        void ProcessOneIteration(
            IFileChannel channel
            );
    }
}
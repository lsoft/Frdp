namespace Frdp.Client.FileTransfer
{
    public interface IFileTask : IFileTaskIdentifier
    {
        event FileTask.TaskChangedDelegate TaskChangeEvent;

        void SaveReceivedPart(
            byte[] data
            );

        int GetPartLength(
            int defaultLength
            );

        void SafelyDelete(
            );

        void ForceToClose(
            );
    }
}
namespace Frdp.Client.FileTransfer
{
    public interface IFileTaskIdentifier
    {
        string RemoteFilePathInvariant
        {
            get;
        }

        string RemoteFilePath
        {
            get;
        }

        string LocalFilePathInvariant
        {
            get;
        }

        string LocalFilePath
        {
            get;
        }

        long TotalFileSize
        {
            get;
        }

        long CurrentFileSize
        {
            get;
        }

        bool IsAlive
        {
            get;
        }
    }
}
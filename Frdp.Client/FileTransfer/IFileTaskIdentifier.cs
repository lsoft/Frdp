namespace Frdp.Client.FileTransfer
{
    public interface IFileTaskIdentifier
    {
        string ServerFilePathInvariant
        {
            get;
        }

        string ServerFilePath
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
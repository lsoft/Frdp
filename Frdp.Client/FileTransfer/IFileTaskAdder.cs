namespace Frdp.Client.FileTransfer
{
    public interface IFileTaskAdder
    {
        void AddTask(
            IFileTask task
            );
    }
}
using System.Collections.Generic;

namespace Frdp.Client.FileTransfer.Container
{
    public interface IFileTaskContainer
    {
        bool AddTask(
            IFileTask task
            );

        void RemoveTask(
            IFileTask task
            );

        List<IFileTask> GetAliveTasks(
            );
    }
}
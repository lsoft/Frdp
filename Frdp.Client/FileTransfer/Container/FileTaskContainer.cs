using System;
using System.Collections.Generic;

namespace Frdp.Client.FileTransfer.Container
{
    public class FileTaskContainer : IFileTaskContainer, IDisposable
    {
        private readonly List<IFileTask> _tasks = new List<IFileTask>();

        private readonly object _locker = new object();

        private volatile bool _disposed = false;

        public FileTaskContainer(
            )
        {
        }

        public bool AddTask(
            IFileTask task
            )
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            lock (_locker)
            {
                if (_tasks.Find(j =>
                    j.IsAlive
                    && j.ServerFilePathInvariant == task.ServerFilePathInvariant
                    && j.LocalFilePathInvariant == task.LocalFilePathInvariant
                    ) != null)
                {
                    //таск по доставке этого файла уже есть

                    return
                        false;
                }

                _tasks.Add(task);
            }

            return
                true;
        }

        public void RemoveTask(
            IFileTask task
            )
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            lock (_locker)
            {
                _tasks.Remove(task);
            }
        }

        public List<IFileTask> GetAliveTasks(
            )
        {
            lock (_locker)
            {
                _tasks.RemoveAll(j => !j.IsAlive);

                return 
                    new List<IFileTask>(_tasks);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                //удаляем недоставленное
                DeleteUncompletedFiles();
            }
        }

        private void DeleteUncompletedFiles()
        {
            foreach (var task in _tasks)
            {
                if (task.IsAlive)
                {
                    task.SafelyDelete();
                }
            }
        }
    }
}
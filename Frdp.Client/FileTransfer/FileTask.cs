using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using Frdp.Common;

namespace Frdp.Client.FileTransfer
{
    public class FileTask : IFileTask
    {
        private readonly ILogger _logger;

        public delegate void TaskChangedDelegate();

        private bool _forceToClose = false;

        public string RemoteFilePathInvariant
        {
            get
            {
                return
                    RemoteFilePath.ToLowerInvariant();
            }
        }

        public string RemoteFilePath
        {
            get;
            private set;
        }

        public string LocalFilePathInvariant
        {
            get
            {
                return
                    LocalFilePath.ToLowerInvariant();
            }
        }

        public string LocalFilePath
        {
            get;
            private set;
        }

        public long TotalFileSize
        {
            get;
            private set;
        }

        public long CurrentFileSize
        {
            get;
            private set;
        }

        public bool IsAlive
        {
            get
            {
                var isDied = _forceToClose || CurrentFileSize >= TotalFileSize;

                return
                    !isDied;
            }
        }

        public event TaskChangedDelegate TaskChangeEvent;

        public FileTask(
            string remoteFilePath, 
            string localFilePath, 
            long totalFileSize,
            bool forceToCreateFolder,
            bool forceToDeleteFile,
            ILogger logger
            )
        {
            if (remoteFilePath == null)
            {
                throw new ArgumentNullException("remoteFilePath");
            }
            if (localFilePath == null)
            {
                throw new ArgumentNullException("localFilePath");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            RemoteFilePath = remoteFilePath;
            LocalFilePath = localFilePath;
            TotalFileSize = totalFileSize;
            CurrentFileSize = 0L;

            _logger = logger;

            if (forceToCreateFolder)
            {
                var fi = new FileInfo(LocalFilePath);
                var d = fi.Directory.FullName;

                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
            }
            if (forceToDeleteFile)
            {
                SafelyDelete();
            }
        }

        public void SaveReceivedPart(
            byte[] data
            )
        {
            using (var fs = new FileStream(LocalFilePath, FileMode.Append, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);

                fs.Flush();
            }

            CurrentFileSize += data.Length;

            OnTaskChange();
        }

        public int GetPartLength(
            int defaultLength
            )
        {
            var diff = (int)(TotalFileSize - CurrentFileSize);

            return
                Math.Min(diff, defaultLength);
        }

        public void SafelyDelete(
            )
        {
            try
            {
                if (File.Exists(LocalFilePath))
                {
                    File.Delete(LocalFilePath);
                }
            }
            catch (Exception excp)
            {
                _logger.LogException(
                    excp,
                    "Ошибка удаления файла задачи"
                    );
            }
        }

        public void ForceToClose()
        {
            this._forceToClose = true;
        }

        protected virtual void OnTaskChange()
        {
            TaskChangedDelegate handler = TaskChangeEvent;
            if (handler != null)
            {
                handler();
            }
        }

    }
}

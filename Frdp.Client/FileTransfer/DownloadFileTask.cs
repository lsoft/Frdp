using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using Frdp.Client.Channel.FileChannel;
using Frdp.Common;

namespace Frdp.Client.FileTransfer
{
    public class DownloadFileTask : IFileTask
    {
        private const int DefaultPartLength = 65536;

        private readonly ILogger _logger;

        private bool _forceToClose = false;

        public bool IsDownloadTask
        {
            get
            {
                return
                    true;
            }
        }

        public string ServerFilePathInvariant
        {
            get
            {
                return
                    ServerFilePath.ToLowerInvariant();
            }
        }

        public string ServerFilePath
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

        public DownloadFileTask(
            string serverFilePath, 
            string localFilePath, 
            long totalFileSize,
            bool forceToCreateFolder,
            bool forceToDeleteFile,
            ILogger logger
            )
        {
            if (serverFilePath == null)
            {
                throw new ArgumentNullException("serverFilePath");
            }
            if (localFilePath == null)
            {
                throw new ArgumentNullException("localFilePath");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            ServerFilePath = serverFilePath;
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

        public void ProcessOneIteration(IFileChannel channel)
        {
            //определяем размер блока, который мы потребуем и смещение от начала
            var offset = this.CurrentFileSize;
            var size = this.GetPartLength(DefaultPartLength);

            //доставляем данные
            var data = channel.GetData(
                this.ServerFilePath,
                offset,
                size
                );

            //сохраняем полученные данные
            this.SaveReceivedPart(data);
        }

        protected virtual void OnTaskChange()
        {
            TaskChangedDelegate handler = TaskChangeEvent;
            if (handler != null)
            {
                handler();
            }
        }

        private int GetPartLength(
            int defaultLength
            )
        {
            var diff = (int)(TotalFileSize - CurrentFileSize);

            return
                Math.Min(diff, defaultLength);
        }

        private void SaveReceivedPart(
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

    }
}

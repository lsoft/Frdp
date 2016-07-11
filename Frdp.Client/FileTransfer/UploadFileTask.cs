using System;
using System.IO;
using Frdp.Client.Channel.FileChannel;
using Frdp.Common;

namespace Frdp.Client.FileTransfer
{
    public class UploadFileTask : IFileTask
    {
        private const int DefaultPartLength = 65536;

        private readonly bool _forceToCreateFolder;
        private readonly ILogger _logger;

        private bool _forceToClose = false;

        public bool IsDownloadTask
        {
            get
            {
                return
                    false;
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

        public UploadFileTask(
            string serverFilePath,
            string localFilePath,
            bool forceToCreateFolder,
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
            CurrentFileSize = 0L;

            var fi = new FileInfo(localFilePath);

            TotalFileSize = fi.Length;

            _forceToCreateFolder = forceToCreateFolder;
            _logger = logger;
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


            //сохраняем полученные данные
            var data = this.GetFileData(
                offset,
                size
                );

            //отправляем данные
            channel.AppendData(
                this.ServerFilePath,
                _forceToCreateFolder,
                data
                );

            CurrentFileSize += size;

            OnTaskChange();
        }

        private byte[] GetFileData(
            long offset,
            int size
            )
        {
            byte[] buffer = null;

            if (File.Exists(LocalFilePath))
            {
                using (var fs = new FileStream(LocalFilePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Position = offset;

                    buffer = new byte[size];
                    fs.Read(buffer, 0, size);
                }
            }

            return
                buffer;
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

    }
}
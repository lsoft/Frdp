using System;
using Frdp.Client.FileTransfer;
using Frdp.Common;
using Frdp.Common.Command;

namespace Frdp.Client.CommandExecutor
{
    internal class StartFileTaskCommandExecutor : ICommandExecutor<StartFileTaskCommand>
    {
        private readonly IFileTaskAdder _fileTaskAdder;
        private readonly ILogger _logger;

        public StartFileTaskCommandExecutor(
            IFileTaskAdder fileTaskAdder,
            ILogger logger
            )
        {
            if (fileTaskAdder == null)
            {
                throw new ArgumentNullException("fileTaskAdder");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _fileTaskAdder = fileTaskAdder;
            _logger = logger;
        }

        public void Execute(StartFileTaskCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            IFileTask task;
            if (command.DownloadFile)
            {
                task = new DownloadFileTask(
                    command.ServerFilePath,
                    command.ClientFilePath,
                    command.TotalFileSize,
                    command.ForceToCreateFolder,
                    command.ForceToDeleteFile,
                    _logger
                    );
            }
            else
            {
                task = new UploadFileTask(
                    command.ServerFilePath,
                    command.ClientFilePath,
                    command.ForceToCreateFolder,
                    _logger
                    );
            }

            _fileTaskAdder.AddTask(task);
        }
    }
}
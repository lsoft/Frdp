using System;
using System.Windows.Input;
using System.Windows.Threading;
using Frdp.Client.FileTransfer;
using Frdp.Client.FileTransfer.Container;
using Frdp.Wpf;

namespace Frdp.Client.ViewModel
{
    public class FileViewModel : BaseViewModel
    {
        private readonly IFileTask _fileTask;

        public string Header
        {
            get
            {
                return
                    _fileTask.IsDownloadTask ? "Получение файла " : "Отправка файла";
            }
        }

        public double Progress
        {
            get
            {
                return
                    100.0*(_fileTask.CurrentFileSize/(double) _fileTask.TotalFileSize);
            }
        }

        public string RemoteFilePath
        {
            get
            {
                return
                    _fileTask.ServerFilePath;
            }
        }

        public string LocalFilePath
        {
            get
            {
                return
                    _fileTask.LocalFilePath;
            }
        }

        public bool TaskCancelled
        {
            get;
            private set;
        }
        
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                        j =>
                        {
                            CancelTask();

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
                        },
                        j => _fileTask.IsAlive
                        );
                }

                return
                    _cancelCommand;
            }
        }

        public FileViewModel(
            Dispatcher dispatcher,
            IFileTask fileTask
            ) : base(dispatcher)
        {
            if (fileTask == null)
            {
                throw new ArgumentNullException("fileTask");
            }

            _fileTask = fileTask;

            _fileTask.TaskChangeEvent += FileTaskOnTaskChange;
        }

        public void Close()
        {
            _fileTask.TaskChangeEvent -= FileTaskOnTaskChange;

            CancelTask();
        }

        private void FileTaskOnTaskChange()
        {
            this._dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        OnPropertyChanged(string.Empty);
                        OnCommandInvalidate();
                    })
                );
        }

        private void CancelTask()
        {
            _fileTask.ForceToClose();

            TaskCancelled = true;
        }
    }
}
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Frdp.Common.Command;
using Frdp.Server.Wcf;
using Frdp.Wpf;
using Microsoft.Win32;

namespace Frdp.Server.ViewModel
{
    public class FileViewModel : BaseViewModel
    {
        private readonly IListener _listener;
        private readonly ICommandContainer _commandContainer;

        private bool _downloadFile = true;

        public string LocalFilePath
        {
            get;
            set;
        }

        public string RemoteFilePath
        {
            get;
            set;
        }

        public bool ForceToCreateFolder
        {
            get;
            set;
        }

        public bool ForceToDeleteFile
        {
            get;
            set;
        }

        public bool DownloadFile
        {
            get
            {
                return
                    _downloadFile;
            }

            set
            {
                _downloadFile = value;

                OnPropertyChanged(string.Empty);
            }
        }

        public bool UploadFile
        {
            get
            {
                return
                    !_downloadFile;
            }

            set
            {
                _downloadFile = !value;
                OnPropertyChanged(string.Empty);

            }
        }

        private ICommand _chooseDownloadFileCommand;
        public ICommand ChooseDownloadFileCommand
        {
            get
            {
                if (_chooseDownloadFileCommand == null)
                {
                    _chooseDownloadFileCommand = new RelayCommand(
                        j =>
                        {
                            var ofd = new OpenFileDialog();
                            ofd.InitialDirectory = Directory.GetCurrentDirectory();

                            var result = ofd.ShowDialog();
                            if (result.HasValue && result.Value)
                            {
                                var filePath = ofd.FileName;

                                LocalFilePath = filePath;

                                if (string.IsNullOrEmpty(RemoteFilePath))
                                {
                                    //дл€ удобства заполним папку назначени€, вдруг совпадают
                                    var fi = new FileInfo(filePath);

                                    var rfi = Path.Combine(
                                        fi.Directory.FullName,
                                        fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + " {received}" + fi.Extension
                                        );

                                    RemoteFilePath = rfi;
                                }

                                _downloadFile = true;

                                OnPropertyChanged(string.Empty);
                                OnCommandInvalidate();
                            }
                        },
                        j => true
                        );
                }

                return
                    _chooseDownloadFileCommand;
            }
        }


        private ICommand _startFileTaskCommand;
        public ICommand StartFileTaskCommand
        {
            get
            {
                if (_startFileTaskCommand == null)
                {
                    _startFileTaskCommand = new RelayCommand(
                        j =>
                        {
                            BaseCommand cmd;
                            if (_downloadFile)
                            {
                                var fi = new FileInfo(LocalFilePath);

                                cmd = new StartFileTaskCommand(
                                    true,
                                    LocalFilePath,
                                    RemoteFilePath,
                                    fi.Length,
                                    ForceToCreateFolder,
                                    ForceToDeleteFile
                                    );
                            }
                            else
                            {
                                cmd = new StartFileTaskCommand(
                                    false,
                                    LocalFilePath,
                                    RemoteFilePath,
                                    0L, //unused for uploading
                                    ForceToCreateFolder,
                                    false //unused for uploading
                                    );
                            }

                            _commandContainer.AddCommand(
                                cmd
                                );

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();

                            MessageBox.Show(
                                " оманда поставлена в очередь на отправку и будет доставлена на клиента при первой возможности.",
                                " оманда создана",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information
                                );
                        },
                        j => _listener.IsListening
                        );
                }

                return
                    _startFileTaskCommand;
            }
        }

        public FileViewModel(
            Dispatcher dispatcher,
            IListener listener,
            ICommandContainer commandContainer
            ) : base(dispatcher)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("listener");
            }
            if (commandContainer == null)
            {
                throw new ArgumentNullException("commandContainer");
            }

            _listener = listener;
            _commandContainer = commandContainer;
        }
    }
}
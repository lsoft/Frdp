using System;
using System.IO;
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
        
        private ICommand _openFileDialogCommand;
        public ICommand OpenFileDialogCommand
        {
            get
            {
                if (_openFileDialogCommand == null)
                {
                    _openFileDialogCommand = new RelayCommand(
                        j =>
                        {
                            var ofd = new OpenFileDialog();
                            ofd.InitialDirectory = Directory.GetCurrentDirectory();

                            var result = ofd.ShowDialog();
                            if (result.HasValue && result.Value)
                            {
                                var filePath = ofd.FileName;

                                LocalFilePath = filePath;

                                //для удобства заполним папку назначения, вдруг совпадают
                                var fi = new FileInfo(filePath);

                                var rfi = Path.Combine(
                                    fi.Directory.FullName,
                                    fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + " {received}" + fi.Extension
                                    );

                                if (string.IsNullOrEmpty(RemoteFilePath))
                                {
                                    RemoteFilePath = rfi;
                                }

                                OnPropertyChanged(string.Empty);
                                OnCommandInvalidate();
                            }
                        },
                        j => true
                        );
                }

                return
                    _openFileDialogCommand;
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
                            var fi = new FileInfo(LocalFilePath);

                            var cmd = new StartFileTaskCommand(
                                LocalFilePath,
                                RemoteFilePath,
                                fi.Length,
                                ForceToCreateFolder,
                                ForceToDeleteFile
                                );

                            _commandContainer.AddCommand(
                                cmd
                                );

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
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
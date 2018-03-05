using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Frdp.Common;
using Frdp.Common.Command;
using Frdp.Common.Settings;
using Frdp.Server.Bitmap;
using Frdp.Server.Wcf;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;
using Frdp.Wpf;

namespace Frdp.Server.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IEndpointContainer _endpointContainer;
        private readonly IListener _listener;
        private readonly ICommandContainer  _commandContainer;
        private readonly IClientSettingsProvider _clientSettings;

        //private uint _blockWidth = 64;
        //private uint _blockHeight = 64;
        //private int _scaleFactorX = 2;
        //private int _scaleFactorY = 2;
        //private TimeSpan _timeoutBetweenFrames = TimeSpan.Zero;

        public string Endpoint
        {
            get
            {
                return
                    _endpointContainer.EndpointAddress;
            }

            set
            {
                _endpointContainer.EndpointAddress = value;
            }
        }

        public bool IsListening
        {
            get
            {
                return
                    _listener.IsListening;
            }
        }

        public string TimeoutBetweenFramesMsec
        {
            get
            {
                return
                    _clientSettings.TimeoutBetweenFrames.TotalMilliseconds.ToString();
            }

            set
            {
                var isOk = false;

                var v = 0u;
                
                if (uint.TryParse(value, out v))
                {
                    isOk = true;
                }

                if (isOk)
                {
                    _clientSettings.SetTimeoutBetweenFrames(
                        TimeSpan.FromMilliseconds(v)
                        );

                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Некорректный таймаут между кадрами! Изменение отменено.";
                }

                OnCommandInvalidate();
                OnPropertyChanged(string.Empty);
            }
        }

        public string ScaleFactorX
        {
            get
            {
                return
                    _clientSettings.ScaleFactorX.ToString();
            }

            set
            {
                bool isOk = false;

                var v = 0;
                
                if (int.TryParse(value, out v))
                {
                    if (v.In(1, 2, 3, 4))
                    {
                        isOk = true;
                    }
                }

                if (isOk)
                {
                    _clientSettings.SetScaleFactorX(v); //_scaleFactorX = v;

                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Некорректный фактор масштабирования! Изменение отменено.";
                }

                OnCommandInvalidate();
                OnPropertyChanged(string.Empty);
            }
        }

        public string ScaleFactorY
        {
            get
            {
                return
                    _clientSettings.ScaleFactorY.ToString();
            }

            set
            {
                bool isOk = false;

                var v = 0;
                
                if (int.TryParse(value, out v))
                {
                    if (v.In(1, 2, 3, 4))
                    {
                        isOk = true;
                    }
                }

                if (isOk)
                {
                    _clientSettings.SetScaleFactorY(v); //_scaleFactorY = v;

                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Некорректный фактор масштабирования! Изменение отменено.";
                }

                OnCommandInvalidate();
                OnPropertyChanged(string.Empty);
            }
        }

        public string BlockWidth
        {
            get
            {
                return
                    _clientSettings.BlockWidth.ToString();
            }

            set
            {
                bool isOk = false;

                uint v = 0u;
                
                if (uint.TryParse(value, out v))
                {
                    if (v.In(16u, 32u, 64u, 128u))
                    {
                        isOk = true;
                    }
                }

                if (isOk)
                {
                    _clientSettings.SetBlockWidth(v); //_blockWidth = v;

                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Некорректный размер блока! Изменение отменено.";
                }

                OnCommandInvalidate();
                OnPropertyChanged(string.Empty);
            }
        }

        public string BlockHeight
        {
            get
            {
                return
                    _clientSettings.BlockHeight.ToString();
            }

            set
            {
                bool isOk = false;

                uint v = 0u;
                
                if (uint.TryParse(value, out v))
                {
                    if (v.In(16u, 32u, 64u, 128u))
                    {
                        isOk = true;
                    }
                }

                if (isOk)
                {
                    _clientSettings.SetBlockHeight(v); //_blockHeight = v;

                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Некорректный размер блока! Изменение отменено.";
                }

                OnCommandInvalidate();
                OnPropertyChanged(string.Empty);
            }
        }

        public List<ColorQualityClass> ColorQuality
        {
            get;
            private set;
        }

        public ColorQualityClass SelectedColorQuality
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        private ICommand _startListeningCommand;
        public ICommand StartListeningCommand
        {
            get
            {
                if (_startListeningCommand == null)
                {
                    _startListeningCommand = new RelayCommand(
                        j =>
                        {
                            _listener.StartListen();

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
                        },
                        j => true
                        );
                }

                return
                    _startListeningCommand;
            }
        }

        private ICommand _stopListeningCommand;
        public ICommand StopListeningCommand
        {
            get
            {
                if (_stopListeningCommand == null)
                {
                    _stopListeningCommand = new RelayCommand(
                        j =>
                        {
                            _listener.StopListen();

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
                        },
                        j => true
                        );
                }

                return
                    _stopListeningCommand;
            }
        }

        private ICommand _pushCommand;
        public ICommand PushCommand
        {
            get
            {
                if (_pushCommand == null)
                {
                    _pushCommand = new RelayCommand(
                        j =>
                        {
                            DoPushCommand();

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
                        },
                        j => true
                        );
                }

                return
                    _pushCommand;
            }
        }


        public MainViewModel(
            Dispatcher dispatcher,
            DirectBitmapContainer container,
            IEndpointContainer endpointContainer,
            IListener listener,
            ICommandContainer commandContainer,
            IClientSettingsProvider clientSettings
            )
            : base(dispatcher)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (endpointContainer == null)
            {
                throw new ArgumentNullException("endpointContainer");
            }
            if (listener == null)
            {
                throw new ArgumentNullException("listener");
            }
            if (commandContainer == null)
            {
                throw new ArgumentNullException("commandContainer");
            }
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }

            _endpointContainer = endpointContainer;
            _listener = listener;
            _commandContainer = commandContainer;
            _clientSettings = clientSettings;

            _endpointContainer.EndpointAddress = "net.tcp://localhost:3310/Frdp";

            this.ColorQuality = new List<ColorQualityClass>
            {
                new ColorQualityClass(Convert.ToByte("11110000", 2), "16 градаций серого"),
                new ColorQualityClass(Convert.ToByte("11111000", 2), "32 градаций серого"),
                new ColorQualityClass(Convert.ToByte("11111100", 2), "64 градаций серого"),
                new ColorQualityClass(Convert.ToByte("11111110", 2), "128 градаций серого"),
                new ColorQualityClass(Convert.ToByte("11111111", 2), "256 градаций серого (полная палитра)"),
            };
            this.SelectedColorQuality = this.ColorQuality[0];
        }

        private void DoPushCommand()
        {
            //var cs = new ClientSettings(
            //    _blockWidth,
            //    _blockHeight,
            //    _scaleFactorX,
            //    _scaleFactorY,
            //    _timeoutBetweenFrames,
            //    SelectedColorQuality.Mask
            //    );

            var cmd = new ChangeClientSettingsCommand(
                _clientSettings
                );

            _commandContainer.AddCommand(
                cmd
                );
        }
    }
}

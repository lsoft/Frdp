using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Frdp.Common;
using Frdp.Common.Command;
using Frdp.Common.Settings;
using Frdp.Server.Bitmap;
using Frdp.Server.Wcf;
using Frdp.Wpf;

namespace Frdp.Server.ViewModel
{
    public class RdpViewModel : BaseViewModel
    {
        private readonly ICommandContainer _commandContainer;
        private readonly IClientSettingsContainer _clientSettings;
        private volatile BitmapSourceWrapper _desktop = null;

        private int _realWidth;
        private int _realHeight;

        private bool _resizeableModeChecked;
        private bool _fixBitmapChecked;
        private bool _scaledFixBitmapChecked;
        
        public bool IsConnectionDoesNotOccurs
        {
            get
            {
                return
                    !IsConnectionOccurs;
            }
        }

        public bool IsConnectionOccurs
        {
            get
            {
                return
                    _desktop != null;
            }
        }

        public ImageSource Desktop
        {
            get
            {
                return
                    _desktop != null ? _desktop.Source : null;
            }
        }

        public int MinWidth
        {
            get
            {
                return
                    0;
            }
        }

        public int MaxWidth
        {
            get;
            private set;
        }

        public int RealWidth
        {
            get
            {
                return
                    _realWidth;
            }
            set
            {
                _realWidth = value;
            }
        }

        public int MinHeight
        {
            get
            {
                return
                    0;
            }
        }

        public int MaxHeight
        {
            get;
            private set;
        }

        public int RealHeight
        {
            get
            {
                return
                    _realHeight;
            }
            set
            {
                _realHeight = value;
            }
        }

        public System.Windows.ResizeMode CurrentResizeMode
        {
            get;
            private set;
        }

        public bool ResizeableModeChecked
        {
            get
            {
                return
                    _resizeableModeChecked;
            }
            set
            {
                if (value)
                {
                    _resizeableModeChecked = true;
                    _fixBitmapChecked = false;
                    _scaledFixBitmapChecked = false;

                    CurrentResizeMode = System.Windows.ResizeMode.CanResize;
                }

                OnPropertyChanged(string.Empty);
            }
        }

        public bool FixBitmapChecked
        {
            get
            {
                return
                    _fixBitmapChecked;
            }
            set
            {
                if (value)
                {
                    _fixBitmapChecked = true;
                    _resizeableModeChecked = false;
                    _scaledFixBitmapChecked = false;

                    CurrentResizeMode = System.Windows.ResizeMode.CanMinimize;
                }

                OnPropertyChanged(string.Empty);
            }
        }

        public bool ScaledFixBitmapChecked
        {
            get
            {
                return
                    _scaledFixBitmapChecked;
            }
            set
            {
                if (value)
                {
                    _scaledFixBitmapChecked = true;
                    _resizeableModeChecked = false;
                    _fixBitmapChecked = false;

                    CurrentResizeMode = System.Windows.ResizeMode.CanMinimize;
                }

                OnPropertyChanged(string.Empty);
            }
        }

        public RdpViewModel(
            Dispatcher dispatcher,
            DirectBitmapContainer container,
            ICommandContainer commandContainer,
            IClientSettingsContainer clientSettings
            ) : base(dispatcher)
        {
            if (commandContainer == null)
            {
                throw new ArgumentNullException("commandContainer");
            }
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }

            _commandContainer = commandContainer;
            _clientSettings = clientSettings;

            container.BitmapChangedEvent += ContainerOnBitmapChanged;

            RealWidth = 800;
            MaxWidth = 800;

            RealHeight = 600;
            MaxHeight = 600;

            CurrentResizeMode = System.Windows.ResizeMode.CanResize;
            
            _resizeableModeChecked = true;
            _fixBitmapChecked = false;
            _scaledFixBitmapChecked = false;
        }

        public void SetRelativeCursorPosition(double x, double y)
        {
            var cmd = new MousePositionCommand(
                x,
                y
                );

            _commandContainer.AddCommand(cmd);
        }

        public void MarkLeftButtonDown()
        {
            var cmd = new MouseButtonCommand(
                MouseButtonCommand.MouseButtonCommandEnum.LeftButtonDown
                );

            _commandContainer.AddCommand(cmd);
        }

        public void MarkLeftButtonUp()
        {
            var cmd = new MouseButtonCommand(
                MouseButtonCommand.MouseButtonCommandEnum.LeftButtonUp
                );

            _commandContainer.AddCommand(cmd);
        }

        public void MarkRightButtonDown()
        {
            var cmd = new MouseButtonCommand(
                MouseButtonCommand.MouseButtonCommandEnum.RightButtonDown
                );

            _commandContainer.AddCommand(cmd);
        }

        public void MarkRightButtonUp()
        {
            var cmd = new MouseButtonCommand(
                MouseButtonCommand.MouseButtonCommandEnum.RightButtonUp
                );

            _commandContainer.AddCommand(cmd);
        }

        public void MarkKeyboardEvent(
            bool isKeyDown,
            Key key
            )
        {
            var cmd = new KeyboardButtonCommand(
                isKeyDown,
                key
                );

            _commandContainer.AddCommand(cmd);
        }

        private void ContainerOnBitmapChanged(
            DirectBitmapContainer container
            )
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        var localContainer = container;

                        var newDesktop = localContainer.DirectBitmap.Bitmap.BitmapToBitmapSource();

                        ProcessDesktopFrame(newDesktop);

                        ProcessWindowSize(newDesktop);
                    })
                );
        }

        private void ProcessWindowSize(BitmapSourceWrapper newDesktop)
        {
            if (_resizeableModeChecked)
            {
                return;
            }

            var hscale = 1;
            var vscale = 1;

            if (_scaledFixBitmapChecked)
            {
                hscale = _clientSettings.ScaleFactorX;
                vscale = _clientSettings.ScaleFactorY;
            }

            var newRealWidth = newDesktop.Source.PixelWidth * hscale;
            if (RealWidth != newRealWidth)
            {
                RealWidth = newRealWidth;
                OnPropertyChanged("RealWidth");
            }

            var newRealHeight = newDesktop.Source.PixelHeight * vscale;
            if (RealHeight != newRealHeight)
            {
                RealHeight = newRealHeight;
                OnPropertyChanged("RealHeight");
            }
        }

        private void ProcessDesktopFrame(BitmapSourceWrapper newDesktop)
        {
            var oldDesktop = Interlocked.Exchange(ref _desktop, newDesktop);

            if (oldDesktop != null)
            {
                oldDesktop.Dispose();
            }

            OnPropertyChanged("Desktop");
        }

        public void CloseSession()
        {
            var cmd = new CloseSessionCommand(
                );

            _commandContainer.AddCommand(cmd);
        }

        public void RecalculateMaxSizes(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            var hmonitor = InvokeHelper.MonitorFromWindow(hWnd, InvokeHelper.MONITOR_DEFAULTTONEAREST);
            
            if (hmonitor == IntPtr.Zero)
            {
                return;
            }

            var minfo = InvokeHelper.GetMonitorInfoEx();
            var r1 = InvokeHelper.GetMonitorInfo(hmonitor, ref minfo);

            if (!r1)
            {
                return;
            }

            MaxWidth = minfo.WorkArea.Right - minfo.WorkArea.Left;
            MaxHeight = minfo.WorkArea.Bottom - minfo.WorkArea.Top;

            this._dispatcher.BeginInvoke(
                new Action(
                    () =>
                        {
                            OnPropertyChanged("MaxWidth");
                            OnPropertyChanged("MaxHeight");
                        }
                    )
                );
        }
    }
}
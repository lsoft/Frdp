using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Frdp.Common.Command;
using Frdp.Server.Bitmap;
using Frdp.Server.Wcf;
using Frdp.Wpf;

namespace Frdp.Server.ViewModel
{
    public class RdpViewModel : BaseViewModel
    {
        private readonly ICommandContainer _commandContainer;
        private volatile BitmapSourceWrapper _desktop = null;

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

        public int RealWidth
        {
            get;
            private set;
        }

        public int RealHeight
        {
            get;
            private set;
        }

        public RdpViewModel(
            Dispatcher dispatcher,
            DirectBitmapContainer container,
            ICommandContainer commandContainer
            ) : base(dispatcher)
        {
            if (commandContainer == null)
            {
                throw new ArgumentNullException("commandContainer");
            }

            _commandContainer = commandContainer;

            container.BitmapChangedEvent += ContainerOnBitmapChanged;

            RealWidth = 800;
            RealHeight = 600;
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

                        var oldDesktop = Interlocked.Exchange(ref _desktop, newDesktop);

                        if (oldDesktop != null)
                        {
                            oldDesktop.Dispose();
                        }

                        OnPropertyChanged("Desktop");

                        if (RealWidth != newDesktop.Source.PixelWidth)
                        {
                            RealWidth = newDesktop.Source.PixelWidth;
                            OnPropertyChanged("RealWidth");
                        }
                        if (RealHeight != newDesktop.Source.PixelHeight)
                        {
                            RealHeight = newDesktop.Source.PixelHeight;
                            OnPropertyChanged("RealHeight");
                        }
                    })
                );
        }

        public void CloseSession()
        {
            var cmd = new CloseSessionCommand(
                );

            _commandContainer.AddCommand(cmd);
        }
    }
}
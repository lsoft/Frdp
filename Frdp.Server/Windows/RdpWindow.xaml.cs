using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Frdp.Server.AppController;
using Frdp.Server.Keyboard;
using Frdp.Server.ViewModel;

namespace Frdp.Server.Windows
{
    /// <summary>
    /// Interaction logic for RdpWindow.xaml
    /// </summary>
    public partial class RdpWindow : Window
    {
        private readonly ISpecialButtonInterceptorFactory _interceptorFactory;
        private readonly IApplicationController _applicationController;

        private ISpecialButtonInterceptor _interceptor;

        public RdpWindow(
            RdpViewModel viewModel,
            ISpecialButtonInterceptorFactory interceptorFactory,
            IApplicationController applicationController
            )
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            if (interceptorFactory == null)
            {
                throw new ArgumentNullException("interceptorFactory");
            }
            if (applicationController == null)
            {
                throw new ArgumentNullException("applicationController");
            }

            _interceptorFactory = interceptorFactory;
            _applicationController = applicationController;

            this.DataContext = viewModel;

            InitializeComponent();

        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            var x = 0.0;
            var y = 0.0;

            var control = sender as FrameworkElement;
            if (control != null)
            {
                var aw = control.ActualWidth;
                var ah = control.ActualHeight;

                if (aw > 0 && ah > 0)
                {
                    var p = e.GetPosition(control);

                    x = p.X / aw;
                    y = p.Y / ah;

                    //Debug.WriteLine(
                    //    "{0}   {1}",
                    //    x,
                    //    y
                    //    );

                    var vm = this.DataContext as RdpViewModel;
                    if (vm != null)
                    {
                        vm.SetRelativeCursorPosition(x, y);
                    }
                }
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                vm.MarkLeftButtonDown();
            }
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                vm.MarkLeftButtonUp();
            }
        }

        private void UIElement_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                vm.MarkRightButtonDown();
            }
        }

        private void UIElement_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                vm.MarkRightButtonUp();
            }
        }

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                var key = e.Key == Key.System
                    ? e.SystemKey
                    : e.Key;

                //Debug.WriteLine("Taken down " + key);

                vm.MarkKeyboardEvent(
                    true,
                    key
                    );

                e.Handled = true;
            }
        }

        private void Window_OnKeyUp(object sender, KeyEventArgs e)
        {
            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                var key = e.Key == Key.System
                    ? e.SystemKey
                    : e.Key;

                //Debug.WriteLine("Taken up " + key);

                vm.MarkKeyboardEvent(
                    false,
                    key
                    );

                e.Handled = true;
            }
        }

        private void RdpWindow_OnClosing(object sender, CancelEventArgs e)
        {
            CloseInterceptor();

            var vm = this.DataContext as RdpViewModel;
            if (vm != null)
            {
                vm.CloseSession();

                //e.Cancel = true;
            }
        }

        private void RdpWindow_OnActivated(object sender, EventArgs e)
        {
            CreateInterceptor();
        }

        private void RdpWindow_OnClosed(object sender, EventArgs e)
        {
            CloseInterceptor();

            _applicationController.InitiateShutdown();
        }

        private void RdpWindow_OnDeactivated(object sender, EventArgs e)
        {
            CloseInterceptor();
        }

        private void CloseInterceptor()
        {
            var interceptor = Interlocked.Exchange(ref _interceptor, null);

            if (interceptor != null)
            {
                interceptor.Dispose();
            }
        }


        private void CreateInterceptor()
        {
            var newInterceptor = _interceptorFactory.CreateInterceptor();

            var oldInterceptor = Interlocked.Exchange(ref _interceptor, newInterceptor);

            if (oldInterceptor != null)
            {
                oldInterceptor.Dispose();
            }
        }

    }

}

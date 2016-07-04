using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Frdp.Server.AppController;
using Frdp.Server.Keyboard;
using Frdp.Server.ViewModel;
using Frdp.Wpf;

namespace Frdp.Server.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private readonly IApplicationController _applicationController;

        public MainWindow(
            MainViewModel viewModel,
            IApplicationController applicationController
            )
        {
            _applicationController = applicationController;
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            if (applicationController == null)
            {
                throw new ArgumentNullException("applicationController");
            }

            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _applicationController.InitiateShutdown();
        }
    }
}

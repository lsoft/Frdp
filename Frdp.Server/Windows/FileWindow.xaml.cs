using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Frdp.Server.AppController;
using Frdp.Server.ViewModel;

namespace Frdp.Server.Windows
{
    /// <summary>
    /// Interaction logic for FileWindow.xaml
    /// </summary>
    public partial class FileWindow : Window
    {
        private readonly IApplicationController _applicationController;

        public FileWindow(
            IApplicationController applicationController,
            FileViewModel viewModel
            )
        {
            if (applicationController == null)
            {
                throw new ArgumentNullException("applicationController");
            }
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            _applicationController = applicationController;

            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void FileWindow_OnClosed(object sender, EventArgs e)
        {
            _applicationController.InitiateShutdown();
        }
    }
}

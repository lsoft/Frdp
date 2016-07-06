using System;
using Frdp.Client.ViewModel;

namespace Frdp.Client.Windows
{
    /// <summary>
    /// Interaction logic for FileWindow.xaml
    /// </summary>
    public partial class FileWindow : System.Windows.Window
    {
        public FileWindow(
            FileViewModel viewModel
            )
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void FileWindow_OnClosed(object sender, EventArgs e)
        {
            var vm = (FileViewModel) this.DataContext;
            vm.Close();
        }
    }
}

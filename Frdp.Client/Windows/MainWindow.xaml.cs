using System.Windows;
using Frdp.Client.ViewModel;

namespace Frdp.Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow(
            MainViewModel viewModel
            )
        {
            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            EndpointTextBox.Focus();
            EndpointTextBox.SelectAll();
        }
    }
}

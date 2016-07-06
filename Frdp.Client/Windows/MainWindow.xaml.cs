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
    }
}

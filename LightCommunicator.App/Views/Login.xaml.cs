using System.Windows;
using System.Windows.Controls;
using LightCommunicator.App.ViewModel;

namespace LightCommunicator.App.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();

            Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LoginViewModel();
        }
    }
}

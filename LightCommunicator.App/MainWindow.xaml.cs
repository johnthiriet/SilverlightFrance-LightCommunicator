using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace LightCommunicator.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();

            Loaded += Shell_Loaded;
        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationContainer.Content = new Views.Login();
            Messenger.Default.Register<string>(this, m =>
            {
                if (m == "Connected")
                    NavigationContainer.Content = new Views.Chat();
            });
        }
    }
}

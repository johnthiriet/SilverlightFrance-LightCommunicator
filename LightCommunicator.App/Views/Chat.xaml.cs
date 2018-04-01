using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LightCommunicator.App.ViewModel;

namespace LightCommunicator.App.Views
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        public Chat()
        {
            InitializeComponent();

            Loaded += Chat_Loaded;
        }

        private void Chat_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ChatViewModel();
            message.KeyDown += message_KeyDown;
        }

        void message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;

            var vm = DataContext as ChatViewModel;

            if (vm.Send.CanExecute(null))
                vm.Send.Execute(null);
        }
    }
}

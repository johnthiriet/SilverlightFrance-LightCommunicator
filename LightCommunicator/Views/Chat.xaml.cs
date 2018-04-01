using System.Windows.Controls;
using System.Windows.Navigation;
using LightCommunicator.Silverlight.ViewModel;

namespace LightCommunicator.Silverlight.Views
{
    public partial class Chat : Page
    {
        private ChatViewModel _vm;

        public Chat()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = _vm = new ChatViewModel();
        }

        private void message_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.Message = ((TextBox)sender).Text;
            _vm.Send.RaiseCanExecuteChanged();
            send.IsEnabled = !string.IsNullOrWhiteSpace(_vm.Message);
        }

    }
}

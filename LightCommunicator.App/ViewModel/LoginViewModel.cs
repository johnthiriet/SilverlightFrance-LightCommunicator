using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace LightCommunicator.App.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _canConnect;
        private string _login;
        private ICommand _connectCommand;

        public ICommand Connect
        {
            get
            {
                return _connectCommand;
            }
        }

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                if (_login != value)
                {
                    _login = value;
                    RaisePropertyChanged("Login");
                    _canConnect = !string.IsNullOrWhiteSpace(_login);
                }
            }
        }

        public LoginViewModel()
        {
            _connectCommand = new RelayCommand(ConnectAction, CanConnect);
        }

        private void ConnectAction()
        {
            ApplicationContext.Instance.Login = Login;
            Messenger.Default.Send("Connected");
            ChatService.Instance.Connect(ApplicationContext.Instance.Login);
        }

        private bool CanConnect()
        {
            return _canConnect;
        }
    }
}

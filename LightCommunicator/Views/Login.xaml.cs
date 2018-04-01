using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using LightCommunicator.Silverlight.ViewModel;

namespace LightCommunicator.Silverlight.Views
{
    public partial class Login : Page
    {
        private LoginViewModel _vm;

        public Login()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = _vm = new LoginViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.Login = ((TextBox) sender).Text;
            _vm.Connect.RaiseCanExecuteChanged();
            btnConnect.IsEnabled = _vm.Connect.CanExecute(null);
        }
    }
}

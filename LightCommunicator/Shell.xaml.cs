using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using LightCommunicator.Silverlight.Views;

namespace LightCommunicator.Silverlight
{
    public partial class Shell : UserControl
    {
        public Shell()
        {
            InitializeComponent();

            Loaded += Shell_Loaded;
        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("Login", UriKind.Relative));

            Messenger.Default.Register<string>(this, m =>
            {
                if (m == "Connected")
                    ContentFrame.Navigate(new Uri("Chat", UriKind.Relative));
            });
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }

    }
}
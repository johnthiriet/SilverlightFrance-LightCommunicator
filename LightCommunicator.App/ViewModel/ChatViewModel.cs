using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LightCommunicator.App.ChatServer;

namespace LightCommunicator.App.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<ChatMessage> _messages;
        private string _message;

        private readonly ChatService _chatService;

        public IEnumerable<ChatMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged("Message");
                }
            }
        }

        public ICommand Send
        {
            get
            {
                return new RelayCommand(
                    () =>
                    {
                        _chatService.SendMessage(Message);
                        Message = string.Empty;
                    },
                    () =>
                    {
                        return !string.IsNullOrWhiteSpace(Message);
                    });
            }
        }

        public ChatViewModel()
        {
            _chatService = ChatService.Instance;
            _messages = new ObservableCollection<ChatMessage>();

            _chatService.Messages
                .ObserveOn(new DispatcherSynchronizationContext())
                .SubscribeOn(Scheduler.ThreadPool)
                .Subscribe(OnMessagePushed);
        }

        private void OnMessagePushed(ChatMessage message)
        {
            _messages.Add(message);
        }
    }
}

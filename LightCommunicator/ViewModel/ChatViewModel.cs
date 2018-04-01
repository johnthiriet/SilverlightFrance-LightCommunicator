using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LightCommunicator.Silverlight.ChatServer;

namespace LightCommunicator.Silverlight.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ChatMessage> _messages;
        private readonly ChatService _chatService;
        private string _message;

        public IEnumerable<ChatMessage> Messages
        {
            get { return _messages; }
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

        public RelayCommand Send
        {
            get
            {
                return new RelayCommand(
                    () =>
                    {
                        _chatService.SendMessage(Message);
                        Message = string.Empty;
                    },
                    () => true);
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
